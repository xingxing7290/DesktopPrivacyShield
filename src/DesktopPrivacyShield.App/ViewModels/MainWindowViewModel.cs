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

    public string StatusTitle => IsLocked ? "遮罩已启用" : "桌面可见";

    public string StatusDescription => IsLocked
        ? "当前桌面内容已被遮罩覆盖，后台任务保持运行。"
        : "当前没有启用隐私遮罩，桌面内容处于正常显示状态。";

    public string StatusAccent => IsLocked ? "运行中" : "待命中";

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
            OnPropertyChanged(nameof(StatusTitle));
            OnPropertyChanged(nameof(StatusDescription));
            OnPropertyChanged(nameof(StatusAccent));
        };
    }

    public void OpenSettings() => _coordinator.ShowSettings();
}
