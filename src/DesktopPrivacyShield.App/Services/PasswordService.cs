using DesktopPrivacyShield.App.Utils;

namespace DesktopPrivacyShield.App.Services;

public sealed class PasswordService : IPasswordService
{
    private readonly IConfigService _configService;
    public int FailedAttempts { get; private set; }

    public PasswordService(IConfigService configService)
    {
        _configService = configService;
    }

    public bool HasPassword() => !string.IsNullOrWhiteSpace(_configService.Current.Password.Hash);

    public async Task SetPasswordAsync(string password)
    {
        var config = _configService.Current;
        var salt = CryptoHelper.CreateSalt();
        config.Password.Salt = salt;
        config.Password.Hash = CryptoHelper.HashPassword(password, salt, config.Password.Iterations);
        await _configService.SaveAsync(config);
        ResetFailedAttempts();
    }

    public async Task<bool> VerifyPasswordAsync(string password)
    {
        var config = _configService.Current;
        if (!HasPassword())
        {
            return false;
        }

        var isValid = CryptoHelper.VerifyPassword(
            password,
            config.Password.Salt,
            config.Password.Hash,
            config.Password.Iterations);

        if (isValid)
        {
            ResetFailedAttempts();
            return true;
        }

        FailedAttempts++;
        var delay = GetCurrentDelay();
        if (delay > TimeSpan.Zero)
        {
            await Task.Delay(delay);
        }

        return false;
    }

    public async Task<bool> ChangePasswordAsync(string currentPassword, string newPassword)
    {
        if (!await VerifyPasswordAsync(currentPassword))
        {
            return false;
        }

        await SetPasswordAsync(newPassword);
        return true;
    }

    public TimeSpan GetCurrentDelay()
    {
        return FailedAttempts switch
        {
            <= 3 => TimeSpan.Zero,
            <= 5 => TimeSpan.FromSeconds(5),
            <= 10 => TimeSpan.FromSeconds(30),
            _ => TimeSpan.FromSeconds(60)
        };
    }

    public void ResetFailedAttempts() => FailedAttempts = 0;
}
