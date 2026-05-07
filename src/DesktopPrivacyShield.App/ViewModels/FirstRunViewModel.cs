using CommunityToolkit.Mvvm.ComponentModel;
using DesktopPrivacyShield.App.Infrastructure;
using DesktopPrivacyShield.App.Services;

namespace DesktopPrivacyShield.App.ViewModels;

public sealed partial class FirstRunViewModel : ObservableObject
{
    private readonly IPasswordService _passwordService;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _confirmPassword = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public RelayAsyncCommand SaveCommand { get; }

    public event EventHandler? PasswordConfigured;

    public FirstRunViewModel(IPasswordService passwordService)
    {
        _passwordService = passwordService;
        SaveCommand = new RelayAsyncCommand(SaveAsync);
    }

    private async Task SaveAsync()
    {
        ErrorMessage = string.Empty;
        if (string.IsNullOrWhiteSpace(Password) || Password.Length < 4)
        {
            ErrorMessage = "密码至少需要 4 位。";
            return;
        }

        if (Password != ConfirmPassword)
        {
            ErrorMessage = "两次输入的密码不一致。";
            return;
        }

        await _passwordService.SetPasswordAsync(Password);
        PasswordConfigured?.Invoke(this, EventArgs.Empty);
    }
}
