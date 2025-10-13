using Microsoft.EntityFrameworkCore;
using PManager.Data;
using PManager.Interfaces.Services;
using PManager.Models.Database;
using SharedModels.Database;
using SharedModels.DataService;
using SharedModels.InputModels;
using System.Text;

namespace PManager
{
    public class DataService : IDisposable, IDataService
    {
        PManagerDbContext _context;
        IEncryptionService _encryptService;
        IHttpContextAccessor _httpContext;
        public DataService(PManagerDbContext context, IEncryptionService encryptService, IHttpContextAccessor httpContext)
        {
            _context = context;
            _encryptService = encryptService;
            _httpContext = httpContext;
        }

        public int GetUserId()
        {
            var claim = _httpContext.HttpContext.User.Claims.SingleOrDefault(u => u.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid");
            if (int.TryParse(claim.Value, out int res))
            {
                return res;
            }
            else
            {
                return -1;
            }
        }

        public int GetRecordId()
        {
            var claim = _httpContext.HttpContext.User.Claims.SingleOrDefault(u => u.Type == "rid");
            if (int.TryParse(claim.Value, out int res))
            {
                return res;
            }
            else
            {
                return -1;
            }
        }

        public int GetPasswordId()
        {
            var claim = _httpContext.HttpContext.User.Claims.SingleOrDefault(u => u.Type == "pid");
            if (int.TryParse(claim.Value, out int res))
            {
                return res;
            }
            else
            {
                return -1;
            }
        }

        public string GetFallback()
        {
            var claim = _httpContext.HttpContext.User.Claims.SingleOrDefault(u => u.Type == "fallback");
            return claim.Value;
        }

        public async Task<Record?> GetRecordById(int id)
        {
            int userId = GetUserId();

            return await _context.UserRecords
                .Where(u => u.UserId == userId)
                .Select(u => u.Record)
                .SingleOrDefaultAsync(r => r.Id == id);
        }

        public async Task<DecryptedPassword?> GetPasswordByPasswordId(PasswordLocationInput model)
        {
            var record = await GetRecordById(model.RecordId);

            if (record == null) return null;

            var result = await _context.RecordPasswords
                .Where(r => r.RecordId == record.Id && r.PasswordId == model.PasswordId)
                .Include(r => r.Password)
                .Include(p => p.Password.Notes)
                .Select(p => p.Password)
                .SingleOrDefaultAsync();

            var passwordValue = Encoding.UTF8.GetString(_encryptService.DecryptWithPassword(result.Value, model.Password));

            string passwordValueStatus = result.IsActive ? passwordValue : (!string.IsNullOrEmpty(passwordValue) ? "Inactive" : "");

            return new DecryptedPassword()
            {
                Id = result.Id,
                Value = passwordValueStatus,
                IsActive = result.IsActive,
                DateCreated = result.DateCreated,
                ExpirationDate = result.ExpirationDate,
                Notes = result.Notes.Select(n => new NoteData()
                {
                    DateCreated = n.DateCreated,
                    Id = n.Id,
                    Text = n.Text
                }).ToList()
            };
        }

        public async Task<bool> ReencryptPassword(PasswordReencryptInput model)
        {
            var record = await GetRecordById(model.RecordId);

            if (record == null) return false;

            var result = await _context.RecordPasswords
                .Where(r => r.RecordId == record.Id && r.PasswordId == model.PasswordId)
                .Include(r => r.Password)
                .Include(p => p.Password.Notes)
                .Select(p => p.Password)
                .SingleOrDefaultAsync();

            var passwordValue = Encoding.UTF8.GetString(_encryptService.DecryptWithPassword(result.Value, model.OldKey));

            if (string.IsNullOrEmpty(passwordValue)) return false;

            var passwordBytes = _encryptService.EncryptWithPassword(Encoding.UTF8.GetBytes(passwordValue), model.NewKey);

            if (passwordBytes == null || passwordBytes.Length == 0) return false;

            result.Value = passwordBytes;

            await _context.SaveChangesAsync();

            return true;
        }
        
        public async Task<RecordPasswordsModel?> GetPasswordsByRecordId(RecordPasswordsInput model)
        {
            var record = await GetRecordById(model.RecordId);

            if (record == null) return null;

            var passwords = await _context.RecordPasswords
                .Where(r => r.RecordId == record.Id)
                .Include(r => r.Password)
                .Include(p => p.Password.Notes)
                .Select(p => p.Password)
                .ToListAsync();
            var decryptedPasswords = passwords
                .Select(c => {
                    var passwordValue = Encoding.UTF8.GetString(_encryptService.DecryptWithPassword(c.Value, model.Password));
                    string passwordValueStatus = c.IsActive ? passwordValue : (!string.IsNullOrEmpty(passwordValue) ? "Inactive" : "");
                    return new DecryptedPassword()
                    {
                        Id = c.Id,
                        Value = passwordValueStatus,
                        IsActive = c.IsActive,
                        DateCreated = c.DateCreated,
                        ExpirationDate = c.ExpirationDate,
                        Notes = c.Notes.Select(n => new NoteData()
                        {
                            DateCreated = n.DateCreated,
                            Id = n.Id,
                            Text = n.Text
                        }).ToList()
                    };
                }).ToList();

            var result = new RecordPasswordsModel()
            {
                Record = record,
                Passwords = decryptedPasswords
            };
            return result;
        }
        
        public async Task<Category?> CreateCategory(string name)
        {
            //clean string
            var existedCategory = await GetCategoryByName(name);

            if (existedCategory != null) return null;

            int userId = GetUserId();

            var category = await _context.UserCategories
                .AddAsync(new UserCategory()
                {
                    UserId = userId,
                    Category = new Category()
                    {
                        Name = name,
                        Description = string.Empty
                    }
                });
            await _context.SaveChangesAsync();
            return category.Entity.Category;
        }

        public async Task<bool> EditRecordName(EditInput model)
        {
            int userId = GetUserId();

            var record = await _context.UserRecords
                .Include(u => u.Record)
                .SingleOrDefaultAsync(u => u.UserId == userId && u.Record.Id == model.Id);

            if (record == null)
            {
                return false;
            }

            record.Record.Name = model.Value;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditCategoryName(EditInput model)
        {
            int userId = GetUserId();

            var existedCategory = await GetCategoryByName(model.Value);

            if (existedCategory != null) return false;

            var category = await _context.UserCategories
                .Include(u => u.Category)
                .SingleOrDefaultAsync(u => u.UserId == userId && u.Category.Id == model.Id);

            if (category == null)
            {
                return false;
            }

            category.Category.Name = model.Value;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditCategoryDescription(EditInput model)
        {
            int userId = GetUserId();

            var category = await _context.UserCategories
                .Include(u => u.Category)
                .SingleOrDefaultAsync(u => u.UserId == userId && u.Category.Id == model.Id);

            if (category == null)
            {
                return false;
            }

            category.Category.Description = model.Value;

            await _context.SaveChangesAsync();

            return true;
        }
        
        async Task<Category?> GetCategoryByName(string name)
        {
            //clean string
            int userId = GetUserId();

            return  await _context.UserCategories
                .Where(u => u.UserId == userId)
                .Select(u => u.Category)
                .SingleOrDefaultAsync(c => c.Name == name);
        }

        async Task<Category?> GetCategoryById(int id)
        {
            int userId = GetUserId();

            return await _context.UserCategories
                .Where(u => u.UserId == userId)
                .Select(u => u.Category)
                .SingleOrDefaultAsync(c => c.Id == id);
        }
        
        public async Task<Record?> CreateRecord(CreateRecordInput model)
        {
            //lowercase compare
            //clean strings
            //validate strings
            var categoryInput = await GetCategoryById(model.CategoryId);

            if (categoryInput == null) return null;
            
            int userId = GetUserId();

            var userrecord = await _context.UserRecords
                .AddAsync(new UserRecord()
                {
                    UserId = userId,
                    Record = new Record()
                    {
                        Category = categoryInput,
                        Name = model.Name,
                        Url = model.Url,
                        Username = model.Username
                    }
                });
            await _context.SaveChangesAsync();
            return userrecord.Entity.Record;
        }
        
        public async Task<Password?> AddPassword(PasswordAddInput model)
        {
            //clean password
            //remove roundtrip
            var record = await GetRecordById(model.RecordId);

            if (record == null) return null;

            var createdPassword = await _context.Passwords
                .AddAsync(new Password()
                {
                    Value = _encryptService.EncryptWithPassword(Encoding.UTF8.GetBytes(model.NewPassword), model.Password),
                    IsActive = true,
                    DateCreated = DateTime.Now,
                    Notes = new List<Note>(),
                    ExpirationDate = model.ExpirationDate
                });

            var recordPassword = await _context.RecordPasswords
                .AddAsync(new RecordPasswords()
                {
                    Password = createdPassword.Entity,
                    Record = record
                });
            await _context.SaveChangesAsync();
            return createdPassword.Entity;
        }
        
        public async Task<bool> AddNoteToPassword(NoteInputModel model)
        {
            var record = await GetRecordById(model.RecordId);

            if (record == null) return false;

            var recordPassword = await _context.RecordPasswords
                .Where(rp => rp.RecordId == model.RecordId && rp.PasswordId == model.PasswordId)
                .Include(rp => rp.Password)
                .SingleOrDefaultAsync();

            if (recordPassword == null) return false;

            recordPassword.Password.Notes.Add(new Note()
            {
                DateCreated = DateTime.Now,
                Text = model.Text
            });

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task DeactivatePassword(PasswordStatusInput model)
        {
            var record = await GetRecordById(model.RecordId);

            if (record == null) return;

            var foundPassword = await _context.RecordPasswords
                .Where(r => r.RecordId == record.Id && r.PasswordId == model.PasswordId)
                .Select(p => p.Password)
                .SingleOrDefaultAsync();

            if (foundPassword != null)
            {
                var passwordValue = Encoding.UTF8.GetString(_encryptService.DecryptWithPassword(foundPassword.Value, model.Password));
                if (!string.IsNullOrEmpty(passwordValue))
                {
                    foundPassword.IsActive = false;

                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task ActivatePassword(PasswordStatusInput model)
        {
            var record = await GetRecordById(model.RecordId);

            if (record == null) return;

            var foundPassword = await _context.RecordPasswords
                .Where(r => r.RecordId == record.Id && r.PasswordId == model.PasswordId)
                .Select(p => p.Password)
                .SingleOrDefaultAsync();

            if (foundPassword != null)
            {
                var passwordValue = Encoding.UTF8.GetString(_encryptService.DecryptWithPassword(foundPassword.Value, model.Password));
                if (!string.IsNullOrEmpty(passwordValue))
                {
                    foundPassword.IsActive = true;

                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task<List<CategoryRecords>> GetAllRecords()
        {
            int userId = GetUserId();

            var categories = await _context.UserCategories
                .Where(c => c.UserId == userId)
                .Select(c => new CategoryRecords()
                {
                    Category = c.Category,
                    Records = new List<Record>()
                })
                .ToListAsync();

            var records = await _context.UserRecords
                .Where(r => r.UserId == userId)
                .Select(r => r.Record)
                .ToListAsync();

            categories.ForEach((c) =>
            {
                records.ForEach((r) => {
                    if (r.Category.Id == c.Category.Id)
                    {
                        c.Records.Add(r);
                    }
                });
            });

            return categories;
        }

        public async Task<bool> DeletePasswordNote(NoteDeleteInput model)
        {
            var record = await GetRecordById(model.RecordId);

            if (record == null) return false;

            var recordPassword = await _context.RecordPasswords
                .Where(rp => rp.RecordId == model.RecordId && rp.PasswordId == model.PasswordId)
                .Include(rp => rp.Password)
                .Include(rp => rp.Password.Notes)
                .SingleOrDefaultAsync();

            if (recordPassword == null) return false;

            var note = recordPassword.Password.Notes.SingleOrDefault(n => n.Id == model.NoteId);

            if (note == null) return false;

            recordPassword.Password.Notes.Remove(note);

            await _context.SaveChangesAsync();

            return true;
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
