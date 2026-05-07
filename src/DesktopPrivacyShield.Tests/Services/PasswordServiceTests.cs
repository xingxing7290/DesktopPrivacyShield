using DesktopPrivacyShield.App.Models;
using DesktopPrivacyShield.App.Services;

namespace DesktopPrivacyShield.Tests.Services;

public sealed class PasswordServiceTests
{
    [Fact]
    public async Task SetPasswordAsync_ShouldPersistHashWithoutPlaintext()
    {
        var configService = new InMemoryConfigService();
        var service = new PasswordService(configService);

        await service.SetPasswordAsync("secret-123");

        Assert.NotEmpty(configService.Current.Password.Hash);
        Assert.NotEmpty(configService.Current.Password.Salt);
        Assert.DoesNotContain("secret-123", configService.Current.Password.Hash, StringComparison.Ordinal);
    }

    [Fact]
    public async Task VerifyPasswordAsync_ShouldResetFailedAttemptsOnSuccess()
    {
        var configService = new InMemoryConfigService();
        var service = new PasswordService(configService);
        await service.SetPasswordAsync("secret-123");

        var failed = await service.VerifyPasswordAsync("bad-pass");
        var success = await service.VerifyPasswordAsync("secret-123");

        Assert.False(failed);
        Assert.True(success);
        Assert.Equal(0, service.FailedAttempts);
    }

    private sealed class InMemoryConfigService : IConfigService
    {
        public AppConfig Current { get; private set; } = new();

        public bool ConfigExists() => true;

        public AppConfig Load() => Current;

        public Task SaveAsync(AppConfig config)
        {
            Current = config;
            return Task.CompletedTask;
        }

        public string GetConfigPath() => "memory";
    }
}
