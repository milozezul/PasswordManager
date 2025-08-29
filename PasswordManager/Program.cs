using System.Text;

namespace PasswordManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var encryptedData = EncryptionService.EncryptWithPassword(Encoding.UTF8.GetBytes("Hello!"), "Forever13");
            var decryptedData = EncryptionService.DecryptWithPassword(encryptedData, "Forever1");
            if (decryptedData != null)
            Console.WriteLine(Encoding.UTF8.GetString(decryptedData));
        }
    }
}
