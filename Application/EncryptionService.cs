using System.Security.Cryptography;
using System.Text;

namespace MyDMS.Application
{
    public static class EncryptionService
    {
        
        private static readonly byte[] Key = DMSConfig.EncryptionKey;
        private static readonly byte[] IV = DMSConfig.EncryptionIV;

        public static string Encrypt(string input)
        {
            using var aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using var ms = new MemoryStream();
            
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(input);
                    sw.Flush();
                }
            }

            return Convert.ToBase64String(ms.ToArray());
        }

        public static string Decrypt(string input)
        {
            var dInput = Convert.FromBase64String(input);

            using var aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using var ms = new MemoryStream(dInput);
            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            {
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        public static byte[] EncryptFile(IFormFile file)
        {
            using var ms = new MemoryStream();
            file.CopyToAsync(ms);
            var fileBytes = ms.ToArray();

            using var aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;

            var enc = aes.CreateEncryptor(aes.Key, aes.IV);

            return enc.TransformFinalBlock(fileBytes, 0, fileBytes.Length);
        }

        public static byte[] DecryptFile(byte[] fileBytes)
        {
            using var aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;

            var dec = aes.CreateDecryptor(aes.Key, aes.IV);

            return dec.TransformFinalBlock(fileBytes, 0, fileBytes.Length);
        }

        public static void EncryptFile(string inputPath, string outputPath)
        {
            using (var aes = Aes.Create())
            using (var encryptor = aes.CreateEncryptor(Key, IV))
            using (var inputStream = new FileStream(inputPath, FileMode.Open))
            using (var outputStream = new FileStream(outputPath, FileMode.Create))
            using (var cryptoStream = new CryptoStream(outputStream, encryptor, CryptoStreamMode.Write))
            {
                inputStream.CopyTo(cryptoStream); // this is an encrypted file (outputPath = encrypted file path)
            }
        }

        public static void DecryptFile(string inputPath, string outputPath)
        {
            using (var aes = Aes.Create())
            using (var decryptor = aes.CreateDecryptor(Key, IV))
            using (var inputStream = new FileStream(inputPath, FileMode.Open))
            using (var outputStream = new FileStream(outputPath, FileMode.Create))
            using (var cryptoStream = new CryptoStream(inputStream, decryptor, CryptoStreamMode.Read))
            {
                cryptoStream.CopyTo(outputStream); // decrypted file.......EVIDENT 
            }
        }

    }
}
