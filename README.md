# DesktopPrivacyShield

DesktopPrivacyShield 是一个基于 `.NET 8 + WPF` 的 Windows 桌面隐私遮罩工具。

它的目标不是替代 Windows 系统级锁屏，而是在当前用户会话需要继续保持活动时，用全屏置顶遮罩覆盖真实桌面内容，避免旁人直接看到屏幕上的任务状态。适合 AI 自动化、桌面脚本、渲染任务、长时间运行程序等场景。

## 功能概览

- 全屏遮罩
- 多显示器覆盖
- 主显示器密码解锁
- PBKDF2 本地密码哈希
- 密码错误次数限制与延迟
- 系统托盘菜单
- 全局快捷键 `Ctrl + Alt + L`
- 空闲自动遮罩
- 开机自启配置
- Windows 原生锁屏入口
- JSON 本地配置文件
- Serilog 本地日志
- 首次运行密码设置
- 基础设置窗口

## 适用场景

- 电脑正在运行 AI 程序、自动化脚本或桌面任务
- 用户需要短暂离开工位
- 不希望旁人看到当前桌面内容
- 又不希望立即切换到 Windows 系统锁屏，影响当前会话中的自动化流程

## 安全边界

本软件是“桌面隐私遮罩工具”，不是系统级安全锁屏。

它不会做下面这些事：

- 不拦截 `Ctrl + Alt + Del`
- 不禁用任务管理器
- 不隐藏自身进程
- 不修改系统安全策略
- 不保存明文密码
- 不记录用户输入的密码
- 不上传密码、日志或桌面内容

这意味着它主要防止的是“旁人直接看到屏幕内容”，而不是防止具备管理员权限的用户强制关闭程序。

如果你需要真正的系统安全保护，优先使用 Windows 自带锁屏：

```text
Win + L
```

## 技术栈

- C#
- .NET 8
- WPF
- MVVM
- Microsoft.Extensions.Hosting / Dependency Injection
- Serilog

## 项目结构

```text
DesktopPrivacyShield/
├─ src/
│  ├─ DesktopPrivacyShield.App/
│  └─ DesktopPrivacyShield.Tests/
├─ docs/
├─ installer/
├─ publish/
├─ DesktopPrivacyShield.sln
└─ README.md
```

## 主要目录说明

- `src/DesktopPrivacyShield.App/`
  WPF 主程序代码，包括视图、视图模型、服务、配置模型和 Win32 封装。

- `src/DesktopPrivacyShield.Tests/`
  单元测试项目，目前覆盖密码服务的基础行为。

- `docs/`
  项目文档，包括开发日志、安全边界说明和用户手册。

- `installer/`
  Inno Setup 安装脚本。

- `publish/`
  本地发布输出目录。

## 运行环境

- Windows 10 / Windows 11
- .NET 8 SDK（开发构建需要）

发布后的自包含版本不依赖本机预装 .NET Runtime。

## 配置与日志

配置文件默认位置：

```text
%AppData%\DesktopPrivacyShield\config.json
```

日志目录：

```text
%AppData%\DesktopPrivacyShield\logs\
```

## 使用方式

### 首次运行

1. 启动程序
2. 设置本地解锁密码
3. 保存后进入主界面，并驻留系统托盘

### 进入遮罩模式

可通过以下任一方式触发：

- 主界面点击“立即遮罩”
- 托盘菜单点击“立即遮罩”
- 按下全局快捷键 `Ctrl + Alt + L`
- 在启用了空闲自动遮罩后，等待达到设定空闲时间

### 解锁

1. 在主显示器输入密码
2. 点击“解锁”
3. 密码正确后关闭遮罩窗口，恢复桌面显示

### 修改设置

当前设置界面支持：

- 开机自启
- 启动后立即遮罩
- 空闲自动遮罩
- 快捷键启用
- 多显示器覆盖
- 是否显示时钟
- 自定义遮罩文案
- 修改密码

## 本地构建

```powershell
dotnet build DesktopPrivacyShield.sln
```

## 运行开发版本

```powershell
dotnet run --project .\src\DesktopPrivacyShield.App\DesktopPrivacyShield.App.csproj
```

## 运行测试

```powershell
dotnet test .\src\DesktopPrivacyShield.Tests\DesktopPrivacyShield.Tests.csproj
```

## 发布可执行文件

生成 Windows x64 单文件自包含版本：

```powershell
dotnet publish .\src\DesktopPrivacyShield.App\DesktopPrivacyShield.App.csproj `
  -c Release `
  -r win-x64 `
  --self-contained true `
  /p:PublishSingleFile=true `
  /p:IncludeNativeLibrariesForSelfExtract=true `
  -o .\publish\win-x64
```

当前仓库已经生成的可执行文件路径：

```text
publish\win-x64\DesktopPrivacyShield.App.exe
```

## 安装包

仓库内提供了 Inno Setup 脚本：

```text
installer\setup.iss
```

打包安装程序前，先执行 `dotnet publish` 生成发布文件。

## 文档

- [开发日志](docs/dev-log.md)
- [安全边界说明](docs/security-boundary.md)
- [用户手册](docs/user-manual.md)
- [变更日志](CHANGELOG.md)
- [v0.2.0 发布说明](docs/release-notes-v0.2.0.md)

## 当前限制

- 遮罩窗口不能覆盖 Windows 安全桌面
- 无法拦截系统级组合键
- 程序被强制结束后，遮罩也会消失
- 当前界面以 MVP 为主，样式与交互仍有优化空间

## 后续可扩展方向

- 模糊背景模式
- 黑屏模式 / 极简主题切换
- 更完善的审计日志
- 更完整的设置项校验
- 正式安装包与卸载流程完善
- 自动更新能力
