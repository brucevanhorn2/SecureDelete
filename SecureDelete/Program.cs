namespace SecureDelete;

using System;
using System.IO;
using System.Security.Cryptography;

internal static class SecureDelete
{
    private static void Main(string[] args)
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
        using var aes = Aes.Create();
        aes.KeySize = 256;
        aes.GenerateKey();
        aes.GenerateIV();

        using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Write, FileShare.None);
        using var cs = new CryptoStream(fs, aes.CreateEncryptor(), CryptoStreamMode.Write);
        var buffer = new byte[4096];
        RandomNumberGenerator.Fill(buffer); 

        long written = 0;
        while (written < fileSize)
        {
            var toWrite = (int)Math.Min(buffer.Length, fileSize - written);
            cs.Write(buffer, 0, toWrite);
            written += toWrite;
        }
    }
}
