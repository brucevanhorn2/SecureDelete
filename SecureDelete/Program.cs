namespace SecureDelete;

using System;
using System.IO;
using System.Security.Cryptography;

class SecureDelete
{
    static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Usage: SecureDelete <file-path>");
            return;
        }

        var filePath = args[0];

        if (!File.Exists(filePath))
        {
            Console.WriteLine("File not found: " + filePath);
            return;
        }

        try
        {
            EncryptFileWithRandomKey(filePath);
            File.Delete(filePath);
            Console.WriteLine("File securely encrypted and deleted: " + filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    static void EncryptFileWithRandomKey(string filePath)
    {
        var fileInfo = new FileInfo(filePath);
        var fileSize = fileInfo.Length;

        // Generate a random AES key (not stored, so recovery is impossible)
        using (Aes aes = Aes.Create())
        {
            aes.KeySize = 256;
            aes.GenerateKey();
            aes.GenerateIV();

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Write, FileShare.None))
            using (CryptoStream cs = new CryptoStream(fs, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                var buffer = new byte[4096]; // 4KB buffer
                RandomNumberGenerator.Fill(buffer); // Fill with random bytes

                long written = 0;
                while (written < fileSize)
                {
                    var toWrite = (int)Math.Min(buffer.Length, fileSize - written);
                    cs.Write(buffer, 0, toWrite);
                    written += toWrite;
                }
            }
        }
    }
}
