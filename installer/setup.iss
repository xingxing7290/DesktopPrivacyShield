; DesktopPrivacyShield installer stub
; Fill OutputDir/AppVersion/AppPublisher before packaging.

[Setup]
AppName=DesktopPrivacyShield
AppVersion=0.1.0
DefaultDirName={autopf}\DesktopPrivacyShield
DefaultGroupName=DesktopPrivacyShield
OutputBaseFilename=DesktopPrivacyShield-Setup

[Files]
Source: "..\src\DesktopPrivacyShield.App\bin\Release\net8.0-windows\publish\*"; DestDir: "{app}"; Flags: recursesubdirs ignoreversion

[Icons]
Name: "{group}\DesktopPrivacyShield"; Filename: "{app}\DesktopPrivacyShield.App.exe"
Name: "{commondesktop}\DesktopPrivacyShield"; Filename: "{app}\DesktopPrivacyShield.App.exe"

[Run]
Filename: "{app}\DesktopPrivacyShield.App.exe"; Description: "Launch DesktopPrivacyShield"; Flags: nowait postinstall skipifsilent
