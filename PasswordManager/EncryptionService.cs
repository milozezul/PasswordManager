using System.Buffers.Binary;
using System.Security.Cryptography;

namespace PasswordManager
{
    public static class EncryptionService
    {
        const int NONCE_SIZE = 12;
        const int TAG_SIZE = 16;

        static byte[] RandomBytes(int len)
        {
            var b = new byte[len];
            RandomNumberGenerator.Fill(b);
            return b;
        }
        static byte[] DeriveKeyFromPassword(string password, byte[] salt, int iterations)
        {
            using var kdf = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
            return kdf.GetBytes(32);
        }

        static byte[] EncryptInternal(byte[] plainText, byte[] key, byte[]? aad)
        {
            var nonce = RandomBytes(NONCE_SIZE);
            var tag = new byte[TAG_SIZE];
            var cipherText = new byte[plainText.Length];

            using var aes = new AesGcm(key, TAG_SIZE);
            aes.Encrypt(nonce, plainText, cipherText, tag, aad);

            var result = new byte[nonce.Length + cipherText.Length + tag.Length];

            Buffer.BlockCopy(nonce, 0, result, 0, nonce.Length);
            Buffer.BlockCopy(cipherText, 0, result, nonce.Length, cipherText.Length);
            Buffer.BlockCopy(tag, 0, result, nonce.Length + cipherText.Length, tag.Length);

            return result;
        }

        static byte[] DecryptInternal(byte[] blob, byte[] key, byte[]? aad)
        {
            if (blob.Length < NONCE_SIZE + TAG_SIZE)
            {
                throw new CryptographicException("Blob too short");
            }

            var nonce = new byte[NONCE_SIZE];
            Buffer.BlockCopy(blob, 0, nonce, 0, NONCE_SIZE);

            var tag = new byte[TAG_SIZE];
            Buffer.BlockCopy(blob, blob.Length - TAG_SIZE, tag, 0, TAG_SIZE);

            var ciphertextLen = blob.Length - NONCE_SIZE - TAG_SIZE;
            var ciphertext = new byte[ciphertextLen];
            Buffer.BlockCopy(blob, NONCE_SIZE, ciphertext, 0, ciphertextLen);

            var plaintext = new byte[ciphertextLen];

            using var aes = new AesGcm(key, TAG_SIZE);
            aes.Decrypt(nonce, ciphertext, tag, plaintext, aad);

            return plaintext;
        }
        static byte[] AttachHeaders(byte[] payload, byte[] salt, byte[] key, int iterations, int version)
        {
            List<IHeaderType> headerTypes = new List<IHeaderType>
            {
                new HeaderType<byte>() { Value = (byte)version, Length = 1 },
                new HeaderType<int>() { Value = iterations, Length = 4 },
                new HeaderType<byte>() { Value = (byte)salt.Length, Length = 1 },
                new HeaderType<byte[]> { Value = salt, Length = salt.Length },
                new HeaderType<byte[]> { Value = payload, Length = payload.Length }
            };

            int resultSize = headerTypes.Aggregate(0, (acc, item) => acc + item.Length);
            byte[] result = new byte[resultSize];

            int index = 0;

            foreach (var h in headerTypes)
            {
                var byteheader = h as HeaderType<byte>;
                if (byteheader != null)
                {
                    result[index] = byteheader.Value;
                }

                var intheader = h as HeaderType<int>;
                if (intheader != null)
                {
                    BinaryPrimitives.WriteInt32BigEndian(result.AsSpan(index, intheader.Length), intheader.Value);
                }

                var bytearrheader = h as HeaderType<byte[]>;
                if (bytearrheader != null)
                {
                    Buffer.BlockCopy(bytearrheader.Value, 0, result, index, bytearrheader.Length);
                }

                index += h.Length;
            }

            CryptographicOperations.ZeroMemory(key);

            return result;
        }

        public static byte[] EncryptWithPassword(byte[] plaintext, string password, byte[]? aad = null, int iterations = 200_000)
        {
            var salt = RandomBytes(TAG_SIZE);
            var key = DeriveKeyFromPassword(password, salt, iterations);
            var payload = EncryptInternal(plaintext, key, aad);
            var result = AttachHeaders(payload, salt, key, iterations, 1);
            return result;
        }

        public static byte[] DecryptWithPassword(byte[] blob, string password, byte[]? aad = null)
        {
            int p = 0;
            byte version = blob[p++];

            if (version != 1)
            {
                throw new CryptographicException("Unsupported blob version");
            }

            int iterations = BinaryPrimitives.ReadInt32BigEndian(blob.AsSpan(p, 4));
            p += 4;

            int saltLen = blob[p++];

            if (saltLen <= 0 || saltLen > 64)
            {
                throw new CryptographicException("Bad salt length");
            }

            var salt = new byte[saltLen];
            Buffer.BlockCopy(blob, p, salt, 0, saltLen);
            p += saltLen;

            var payload = new byte[blob.Length - p];
            Buffer.BlockCopy(blob, p, payload, 0, payload.Length);

            var key = DeriveKeyFromPassword(password, salt, iterations);
            try
            {
                return DecryptInternal(payload, key, aad);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not decrypt");
                return null;
            }
            finally
            {
                CryptographicOperations.ZeroMemory(key);
            }
        }
    }

    interface IHeaderType
    {
        int Length { get; }
    }
    class HeaderType<T>: IHeaderType
    {
        public T Value { get; set; }
        public int Length { get; set; }
    }
}
