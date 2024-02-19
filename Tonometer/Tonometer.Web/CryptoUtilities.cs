#region

using System.Security.Cryptography;
using System.Text;

#endregion

namespace Tonometer.Web;

public static class CryptoUtilities
{
    public static string HashPassword(string password)
    {
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = SHA256.HashData(bytes);
        return Convert.ToBase64String(hash);
    }
}
