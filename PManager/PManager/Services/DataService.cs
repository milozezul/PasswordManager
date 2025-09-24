using Microsoft.EntityFrameworkCore;
using PManager.Data;
using PManager.Interfaces.Services;
using PManager.Models.Database;
using SharedModels.Database;
using SharedModels.DataService;
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

        public async Task<List<Record>> GetRecords(string category)
        {
            //lowercase compare
            //clean string
            int userId = GetUserId();

            return await _context.UserRecords
                .Where(u => u.UserId == userId)
                .Select(u => u.Record)
                .Where(r => r.Category.Name == category)
                .ToListAsync();                     
        }

        public async Task<Record?> GetRecordById(int id)
        {
            int userId = GetUserId();

            return await _context.UserRecords
                .Where(u => u.UserId == userId)
                .Select(u => u.Record)
                .SingleOrDefaultAsync(r => r.Id == id);            
        }
        
        public async Task<RecordPasswordsModel?> GetPasswordsByRecordId(int recordId, string password)
        {
            var record = await GetRecordById(recordId);

            if (record == null) return null;

            var passwords = await _context.RecordPasswords
                .Where(r => r.RecordId == record.Id)
                .Select(p => p.Password)
                .ToListAsync();
            var decryptedPasswords = passwords
                .Select(c => new DecryptedPassword()
                {
                    Id = c.Id,
                    Value = c.IsActive == true ? Encoding.UTF8.GetString(_encryptService.DecryptWithPassword(c.Value, password)) : "DIACTIVATED",
                    IsActive = c.IsActive
                }).ToList();

            var result = new RecordPasswordsModel()
            {
                Record = record,
                Passwords = decryptedPasswords
            };
            return result;
        }
        
        async Task<RecordPasswordsModel> GetEncryptedPasswordsByRecordId(int recordId)
        {
            var record = await GetRecordById(recordId);

            if (record == null) return null;

            var passwords = await _context.RecordPasswords
                .Where(r => r.RecordId == record.Id)
                .Select(p => p.Password)
                .ToListAsync();
            var decryptedPasswords = passwords
                .Select(c => new DecryptedPassword()
                {
                    Id = c.Id,
                    Value = c.IsActive == true ? Encoding.UTF8.GetString(c.Value) : "DIACTIVATED",
                    IsActive = c.IsActive
                }).ToList();

            var result = new RecordPasswordsModel()
            {
                Record = record,
                Passwords = decryptedPasswords
            };
            return result;
        }
        
        public async Task<List<Category>> GetCategories()
        {
            int userId = GetUserId();

            return await _context.UserCategories
                .Where(u => u.UserId == userId)
                .Select(u => u.Category)
                .ToListAsync();
        }
        
        public async Task<Category?> CreateCategory(string name)
        {
            //lowercase
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
                        Name = name
                    }
                });
            await _context.SaveChangesAsync();
            return category.Entity.Category;
        }
        
        async Task<Category?> GetCategoryByName(string name)
        {
            //lowercase
            //clean string
            int userId = GetUserId();

            return  await _context.UserCategories
                .Where(u => u.UserId == userId)
                .Select(u => u.Category)
                .SingleOrDefaultAsync(c => c.Name == name);
        }
        
        public async Task<Record?> CreateRecord(string category, string name, string url)
        {
            //lowercase compare
            //clean strings
            //validate strings
            var categoryInput = await GetCategoryByName(category);

            if (categoryInput == null) return null;
            
            int userId = GetUserId();

            var userrecord = await _context.UserRecords
                .AddAsync(new UserRecord()
                {
                    UserId = userId,
                    Record = new Record()
                    {
                        Category = categoryInput,
                        Name = name,
                        Url = url
                    }
                });
            await _context.SaveChangesAsync();
            return userrecord.Entity.Record;
        }
        
        public async Task<Password?> AddPassword(int recordId, string newPassword, string password)
        {
            //clean password
            //remove roundtrip
            var record = await GetRecordById(recordId);

            if (record == null) return null;

            var createdPassword = await _context.Passwords
                .AddAsync(new Password()
                {
                    Value = _encryptService.EncryptWithPassword(Encoding.UTF8.GetBytes(newPassword), password),
                    IsActive = true
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
        
        public async Task<RecordPasswordsModel?> CreateRecordWithPassword(string category, string name, string url, string newPassword, string password)
        {
            //clean strings
            //lowercase strings
            //reduce number of save changes per query 2 operations
            var record = await CreateRecord(category, name, url);

            if (record == null) return null;

            var createdPassword = await AddPassword(record.Id, newPassword, password);

            var passwords = await GetEncryptedPasswordsByRecordId(record.Id);

            await _context.SaveChangesAsync();
            return passwords;
        }       
        
        public async Task DeactivatePassword(int recordId, int passwordId, string password)
        {
            var record = await GetRecordById(recordId);

            if (record == null) return;

            var foundPassword = await _context.RecordPasswords
                .Where(r => r.RecordId == record.Id && r.PasswordId == passwordId)
                .Select(p => p.Password)
                .SingleOrDefaultAsync();

            if (foundPassword != null)
            {
                var passwordValue = Encoding.UTF8.GetString(_encryptService.DecryptWithPassword(foundPassword.Value, password));
                if (!string.IsNullOrEmpty(passwordValue))
                {
                    foundPassword.IsActive = false;

                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task ActivatePassword(int recordId, int passwordId, string password)
        {
            var record = await GetRecordById(recordId);

            if (record == null) return;

            var foundPassword = await _context.RecordPasswords
                .Where(r => r.RecordId == record.Id && r.PasswordId == passwordId)
                .Select(p => p.Password)
                .SingleOrDefaultAsync();

            if (foundPassword != null)
            {
                var passwordValue = Encoding.UTF8.GetString(_encryptService.DecryptWithPassword(foundPassword.Value, password));
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

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
