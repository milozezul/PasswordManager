namespace PManager.Interfaces.Services
{
    public interface IEncryptionService
    {
        byte[] DecryptWithPassword(byte[] blob, string password, byte[]? aad = null);
        byte[] EncryptWithPassword(byte[] plaintext, string password, byte[]? aad = null, int iterations = 200000);
    }
}