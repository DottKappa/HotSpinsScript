using System;
using System.Text;

public static class Encryptor
{
    private static readonly string key = "hot spins";

    public static string Encrypt(string data)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(data);
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] result = new byte[bytes.Length];

        for (int i = 0; i < bytes.Length; i++)
        {
            result[i] = (byte)(bytes[i] ^ keyBytes[i % keyBytes.Length]);
        }

        return Convert.ToBase64String(result);
    }

    public static string Decrypt(string encryptedData)
    {
        byte[] data = Convert.FromBase64String(encryptedData);
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] result = new byte[data.Length];

        for (int i = 0; i < data.Length; i++)
        {
            result[i] = (byte)(data[i] ^ keyBytes[i % keyBytes.Length]);
        }

        return Encoding.UTF8.GetString(result);
    }
}
