using System.Windows;
using DesktopPrivacyShield.App.ViewModels;

namespace DesktopPrivacyShield.App.Views;

public partial class SettingsWindow : Window
{
    private readonly SettingsViewModel _viewModel;

    public SettingsWindow(SettingsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;
    }

    private void OnSaveClicked(object sender, RoutedEventArgs e)
    {
        _viewModel.CurrentPassword = CurrentPasswordInput.Password;
        _viewModel.NewPassword = NewPasswordInput.Password;
        _viewModel.ConfirmNewPassword = ConfirmPasswordInput.Password;
        _viewModel.SaveCommand.Execute(null);
    }
}
