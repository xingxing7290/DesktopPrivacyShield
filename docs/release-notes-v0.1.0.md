# DesktopPrivacyShield v0.1.0

发布日期：2026-05-07

## 发布摘要

这是首个可用版本，提供 Windows 桌面隐私遮罩的完整 MVP 交付，包括遮罩、解锁、托盘、热键、空闲检测、设置、日志和正式安装脚本。

## 包含内容

- 全屏遮罩与多显示器覆盖
- 本地密码设置与 PBKDF2 哈希存储
- 密码错误重试延迟
- 系统托盘控制
- 全局快捷键 `Ctrl + Alt + L`
- 空闲自动遮罩
- 开机自启开关
- Windows 原生锁屏入口
- 本地配置文件与日志
- 基础设置界面
- Windows 安装包

## 交付产物

- 安装包：`release/DesktopPrivacyShield-Setup-0.1.0.exe`
- SHA256：`release/SHA256SUMS.txt`
- 便携发布目录：`publish/win-x64/`

## 校验值

```text
9368572A68D835FFE0183C367156EBDEC9A863CEA48866619032F2444A9885E4  DesktopPrivacyShield-Setup-0.1.0.exe
```

## 验证记录

- `dotnet build DesktopPrivacyShield.sln`
- `dotnet test src/DesktopPrivacyShield.Tests/DesktopPrivacyShield.Tests.csproj`
- `dotnet publish src/DesktopPrivacyShield.App/DesktopPrivacyShield.App.csproj -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true`
- `ISCC installer/setup.iss`

## 已知限制

- 不是系统级安全锁屏
- 无法覆盖 Windows 安全桌面
- 程序被强制结束后遮罩也会退出
