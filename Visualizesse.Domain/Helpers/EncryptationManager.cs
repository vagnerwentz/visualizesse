using System.Text;
using System.Security.Cryptography;

namespace Visualizesse.Domain.Helpers;

public static class EncryptationManager
{
    private readonly static string key = "78bef509ee9e7c3fa0d519870f25f9cf7783e9b6";
    public static string Encrypt(string text) 
    {
        var hash = SHA1.Create();
        var encoding = new ASCIIEncoding();
        var bytes = encoding.GetBytes(text);

        bytes = hash.ComputeHash(bytes);

        var hexadecimalString = new StringBuilder();

        foreach (var item in bytes) 
        {
            hexadecimalString.Append(item.ToString("x2"));
        }

        return hexadecimalString.ToString();
    }
}