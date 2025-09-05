using PManager.Data;

namespace PManager
{
    public class DataService: IDisposable
    {
        PManagerDbContext _context;
        public DataService(PManagerDbContext context)
        {
            _context = context;
        }
        public async Task<List<Categories>> GetListOfCategories()
        {
            return _context.Categories.ToList();
        }

        public async Task GetListOfPasswords(string category)
        {
            /*
            var encryptedData = EncryptionService.EncryptWithPassword(Encoding.UTF8.GetBytes("Hello!"), "Forever13");
            var decryptedData = EncryptionService.DecryptWithPassword(encryptedData, "Forever1");
            if (decryptedData != null)
                Console.WriteLine(Encoding.UTF8.GetString(decryptedData));
            */
        }

        public async Task GetPasswordById(int id, int? subid)
        {

        }

        public async Task SavePassword(int? id)
        {

        }

        public async Task DiactivatePassword(int id, int subid)
        {

        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
