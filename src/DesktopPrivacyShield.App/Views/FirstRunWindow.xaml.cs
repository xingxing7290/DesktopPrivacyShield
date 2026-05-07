using System.Windows;
using DesktopPrivacyShield.App.ViewModels;

namespace DesktopPrivacyShield.App.Views;

public partial class FirstRunWindow : Window
{
    private readonly FirstRunViewModel _viewModel;

    public FirstRunWindow(FirstRunViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _viewModel.PasswordConfigured += (_, _) =>
        {
            DialogResult = true;
            Close();
        };
        DataContext = _viewModel;
    }

    private void OnSaveClicked(object sender, RoutedEventArgs e)
    {
        _viewModel.Password = PasswordInput.Password;
        _viewModel.ConfirmPassword = ConfirmInput.Password;
        _viewModel.SaveCommand.Execute(null);
    }
}
