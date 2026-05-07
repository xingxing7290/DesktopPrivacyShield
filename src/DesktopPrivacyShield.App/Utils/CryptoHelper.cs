using System.Security.Cryptography;

namespace DesktopPrivacyShield.App.Utils;

public static class CryptoHelper
{
    public static string CreateSalt(int size = 16)
    {
        var bytes = RandomNumberGenerator.GetBytes(size);
        return Convert.ToBase64String(bytes);
    }

    public static string HashPassword(string password, string saltBase64, int iterations, int size = 32)
    {
        var salt = Convert.FromBase64String(saltBase64);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            iterations,
            HashAlgorithmName.SHA256,
            size);
        return Convert.ToBase64String(hash);
    }

    public static bool VerifyPassword(string password, string saltBase64, string expectedHashBase64, int iterations)
    {
        var actual = HashPassword(password, saltBase64, iterations);
        return CryptographicOperations.FixedTimeEquals(
            Convert.FromBase64String(actual),
            Convert.FromBase64String(expectedHashBase64));
    }
}
