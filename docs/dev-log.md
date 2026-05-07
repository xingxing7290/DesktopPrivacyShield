# 开发日志

## 2026-05-07

- 初始化 `DesktopPrivacyShield.sln`、WPF 应用和测试项目。
- 按 README 设计实现配置模型、PBKDF2 密码服务、托盘、全局热键、空闲检测、开机自启、Windows 原生锁屏服务。
- 实现首次运行密码设置窗口、主界面、设置窗口、全屏遮罩窗口和多显示器遮罩逻辑。
- 接入 `Microsoft.Extensions.Hosting`、依赖注入和 Serilog 文件日志。
- 完成基础单元测试，并验证应用工程可编译。
