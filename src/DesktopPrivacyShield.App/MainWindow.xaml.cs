using System.Windows;
using DesktopPrivacyShield.App.ViewModels;

namespace DesktopPrivacyShield.App;

public partial class MainWindow : Window
{
    private readonly MainWindowViewModel _viewModel;

    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;
    }

    private void OnOpenSettingsClicked(object sender, RoutedEventArgs e)
    {
        _viewModel.OpenSettings();
    }
}
