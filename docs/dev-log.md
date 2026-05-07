# 开发日志

## 2026-05-07

- 初始化 `DesktopPrivacyShield.sln`、WPF 应用和测试项目。
- 按 README 设计实现配置模型、PBKDF2 密码服务、托盘、全局热键、空闲检测、开机自启、Windows 原生锁屏服务。
- 实现首次运行密码设置窗口、主界面、设置窗口、全屏遮罩窗口和多显示器遮罩逻辑。
- 接入 `Microsoft.Extensions.Hosting`、依赖注入和 Serilog 文件日志。
- 完成基础单元测试，并验证应用工程可编译。
- 补充 `0.1.0` 版本元数据、`CHANGELOG.md`、GitHub Release 说明和正式 Inno Setup 安装脚本。
- 补充 `0.2.0` 功能版本：新增自动调用 Windows 系统锁屏选项，重做应用品牌 logo 与主要窗口视觉样式。
