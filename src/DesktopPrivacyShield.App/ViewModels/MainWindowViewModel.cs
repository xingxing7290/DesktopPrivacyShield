using CommunityToolkit.Mvvm.ComponentModel;
using DesktopPrivacyShield.App.Infrastructure;
using DesktopPrivacyShield.App.Services;

namespace DesktopPrivacyShield.App.ViewModels;

public sealed partial class MainWindowViewModel : ObservableObject
{
    private readonly ILockService _lockService;
    private readonly IApplicationCoordinator _coordinator;

    [ObservableProperty]
    private bool _isLocked;

    public RelayAsyncCommand LockCommand { get; }

    public MainWindowViewModel(ILockService lockService, IApplicationCoordinator coordinator)
    {
        _lockService = lockService;
        _coordinator = coordinator;
        LockCommand = new RelayAsyncCommand(() => _lockService.LockAsync(), () => !IsLocked);
        IsLocked = _lockService.IsLocked;
        _lockService.LockStateChanged += (_, locked) =>
        {
            IsLocked = locked;
            LockCommand.RaiseCanExecuteChanged();
        };
    }

    public void OpenSettings() => _coordinator.ShowSettings();
}
