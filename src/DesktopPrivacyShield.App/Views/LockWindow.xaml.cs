using System.Windows;
using System.Windows.Threading;
using DesktopPrivacyShield.App.Models;
using DesktopPrivacyShield.App.Services;
using DesktopPrivacyShield.App.ViewModels;

namespace DesktopPrivacyShield.App.Views;

public partial class LockWindow : Window
{
    private readonly bool _isPrimary;
    private readonly ILockService _lockService;
    private readonly LockViewModel _viewModel;
    private readonly DispatcherTimer _clockTimer;

    public LockWindow(MonitorInfo monitor, bool isPrimary, ILockService lockService, LockViewModel viewModel)
    {
        InitializeComponent();
        _isPrimary = isPrimary;
        _lockService = lockService;
        _viewModel = viewModel;
        _viewModel.LoadSettings();
        DataContext = _viewModel;

        Left = monitor.Left;
        Top = monitor.Top;
        Width = monitor.Width;
        Height = monitor.Height;

        if (!_isPrimary)
        {
            ContentPanel.Visibility = Visibility.Collapsed;
            SecondaryMonitorPanel.Visibility = Visibility.Visible;
        }

        _clockTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _clockTimer.Tick += (_, _) => UpdateClock();
        Loaded += OnLoaded;
        Closed += (_, _) => _clockTimer.Stop();
    }

    public bool IsPrimaryWindow => _isPrimary;

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Normal;
        Activate();
        Focus();
        UpdateClock();
        _clockTimer.Start();

        if (_isPrimary)
        {
            PasswordInput.Focus();
        }
    }

    private void UpdateClock()
    {
        ClockText.Visibility = _viewModel.ShowClock ? Visibility.Visible : Visibility.Collapsed;
        ClockText.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    private async void OnUnlockClicked(object sender, RoutedEventArgs e)
    {
        if (!_isPrimary)
        {
            return;
        }

        _viewModel.Password = PasswordInput.Password;
        await _lockService.UnlockAsync(_viewModel.Password);
        PasswordInput.Clear();
    }

    public void SetStatusMessage(TimeSpan delay)
    {
        _viewModel.StatusMessage = delay > TimeSpan.Zero
            ? $"密码错误，请稍后重试。当前延迟 {delay.TotalSeconds:0} 秒。"
            : "密码错误。";
    }
}
