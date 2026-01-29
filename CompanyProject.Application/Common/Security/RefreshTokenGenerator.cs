using System.Security.Cryptography;

namespace CompanyProject.Application.Common.Security;

public static class RefreshTokenGenerator
{
    // generates a random secure string
    public static string Generate()
    {
        var bytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);

        return Convert.ToBase64String(bytes);
    }
}
