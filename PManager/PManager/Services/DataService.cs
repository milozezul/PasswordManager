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

        private int GetUserId()
        {
            var claim = _httpContext.HttpContext.User.Claims.FirstOrDefault(u => u.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid");
            if (int.TryParse(claim.Value, out int res))
            {
                return res;
            }
            else
            {
                return -1;
            }
        }
        //implemented
        public async Task<List<Record>> GetRecords(string category)
        {
            //lowercase compare
            //clean string
            int userId = GetUserId();
            var userrecords = await _context.UserRecords
                .Where(u => u.UserId == userId)
                .Select(u => u.Record)
                .Where(r => r.Category.Name == category)
                .ToListAsync();
            return userrecords;
        }
        //private
        async Task<Record> GetRecordById(int id)
        {
            int userId = GetUserId();
            var userrecords = await _context.UserRecords
                .Where(u => u.UserId == userId)
                .Select(u => u.Record)
                .SingleOrDefaultAsync(r => r.Id == id);
            return userrecords;
        }
        //implemented
        public async Task<RecordPasswordsModel> GetPasswordsByRecordId(int recordId, string password)
        {
            var record = await GetRecordById(recordId);

            if (record == null) return null;

            var passwords = await _context.RecordPasswords.Where(r => r.RecordId == record.Id).Select(p => p.Password).ToListAsync();
            var decryptedPasswords = passwords.Select(c => new DecryptedPassword()
            {
                Id = c.Id,
                Value = Encoding.UTF8.GetString(_encryptService.DecryptWithPassword(c.Value, password))
            }).ToList();

            var result = new RecordPasswordsModel()
            {
                Record = record,
                Passwords = decryptedPasswords
            };
            return result;
        }
        //private
        async Task<RecordPasswordsModel> GetEncryptedPasswordsByRecordId(int recordId)
        {
            var record = await GetRecordById(recordId);

            if (record == null) return null;

            var passwords = await _context.RecordPasswords.Where(r => r.RecordId == record.Id).Select(p => p.Password).ToListAsync();
            var decryptedPasswords = passwords.Select(c => new DecryptedPassword()
            {
                Id = c.Id,
                Value = Encoding.UTF8.GetString(c.Value)
            }).ToList();

            var result = new RecordPasswordsModel()
            {
                Record = record,
                Passwords = decryptedPasswords
            };
            return result;
        }
        //implemented
        public async Task<List<Category>> GetCategories()
        {
            int userId = GetUserId();
            return await _context.UserCategories.Where(u => u.UserId == userId).Select(u => u.Category).ToListAsync();            
        }
        //implemented
        public async Task<Category> CreateCategory(string name)
        {
            //lowercase
            //clean string
            var existedCategory = await GetCategoryByName(name);
            if (existedCategory != null) return null;//put some message in service respone model

            int userId = GetUserId();
            var category = await _context.UserCategories.AddAsync(new UserCategory()
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
        //private
        async Task<Category> GetCategoryByName(string name)
        {
            //lowercase
            //clean string
            try
            {
                int userId = GetUserId();
                var category = await _context.UserCategories.Where(u => u.UserId == userId).Select(u => u.Category).SingleOrDefaultAsync(c => c.Name == name);
                return category;
            }
            catch (ArgumentNullException ex)
            {
                return null;
            }
        }
        //implemented
        public async Task<Record> CreateRecord(string category, string name, string url)
        {
            //lowercase compare
            //clean strings
            //validate strings
            var categoryInput = await GetCategoryByName(category);
            if (categoryInput != null)
            {
                /*var record = await _context.Records.AddAsync(new Record()
                {
                    Category = categoryInput,
                    Name = name,
                    Url = url
                });*/
                int userId = GetUserId();
                var userrecord = await _context.UserRecords.AddAsync(new UserRecord()
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
            else
            {
                throw new Exception("Category doesn't exist")
                {
                    Source = "Task<Record> CreateRecord(string category, string name, string url)"
                };
            }
        }
        //implemented
        public async Task<Password> AddPassword(int recordId, string newPassword, string password)
        {
            //clean password
            var record = await GetRecordById(recordId);

            if (record == null) return null;

            var createdPassword = await _context.Passwords.AddAsync(new Password()
            {
                Value = _encryptService.EncryptWithPassword(Encoding.UTF8.GetBytes(newPassword), password)
            });

            var recordPassword = await _context.RecordPasswords.AddAsync(new RecordPasswords()
            {
                Password = createdPassword.Entity,
                Record = record
            });
            await _context.SaveChangesAsync();
            return createdPassword.Entity;
        }
        //implemented
        public async Task<RecordPasswordsModel> CreateRecordWithPassword(string category, string name, string url, string newPassword, string password)
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
        //implemented
        public async Task<DecryptedPassword> GetSpecificPassword(int recordId, int passwordId, string password)
        {
            var record = await GetRecordById(recordId);

            if (record == null) return null;

            var foundPassword = await _context.RecordPasswords.Where(r => r.RecordId == record.Id && r.PasswordId == passwordId).Select(p => p.Password).FirstAsync();
            var decryptedPassword = new DecryptedPassword()
            {
                Id = foundPassword.Id,
                Value = Encoding.UTF8.GetString(_encryptService.DecryptWithPassword(foundPassword.Value, password))
            };
            
            return decryptedPassword;
        }
        //on hold
        public async Task DeactivateRecord(int recordId, string password)
        {

        }
        //on hold
        public async Task DeactivatePassword(int recordId, int passwordId, string password)
        {

        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
