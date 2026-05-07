# DesktopPrivacyShield v0.2.0

发布日期：2026-05-07

## 发布摘要

`0.2.0` 是首个视觉升级版本。在 `0.1.0` 的桌面遮罩基础上，这个版本补充了新的品牌 logo、整体 UI 现代化改造，以及“进入遮罩后自动调用 Windows 系统锁屏”选项。

## 本次更新

- 新增“进入遮罩后自动调用 Windows 系统锁屏”设置项
- 遮罩显示后可自动调用 `LockWorkStation()`
- 新增品牌 logo，并接入应用图标、窗口图标和托盘图标
- 重做主界面、首次设置界面、设置界面和遮罩界面
- 统一深色主题、卡片、按钮、输入框和视觉样式
- 修正多处界面文案与说明

## 交付产物

- 安装包：`release/DesktopPrivacyShield-Setup-0.2.0.exe`
- SHA256：`release/SHA256SUMS.txt`
- 便携发布目录：`publish/win-x64/`

## 验证记录

- `dotnet build DesktopPrivacyShield.sln`
- `dotnet publish src/DesktopPrivacyShield.App/DesktopPrivacyShield.App.csproj -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true`
- `ISCC installer/setup.iss`

## 说明

“自动系统锁屏”不是在应用窗口上叠加一个系统页面。实际行为是：

1. 先显示本软件遮罩
2. 再调用 Windows 原生锁屏
3. 用户解锁 Windows 后，仍会回到本软件遮罩界面

这是符合 Windows 安全桌面边界的实现方式。
