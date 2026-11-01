; SmartCalculator v3.0.0 — Inno Setup Installer Script
; Build with: ISCC.exe SmartCalculator.iss

#define MyAppName "SmartCalculator"
#define MyAppVersion "3.0.0"
#define MyAppPublisher "Ahmad Ibrahim"
#define MyAppURL "https://github.com/ahmadibrahim4geo/SmartCalculator"
#define MyAppExeName "SmartCalculator.exe"

[Setup]
AppId={{B8F5C3A1-2D4E-4F6A-8B0C-1D2E3F4A5B6C}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
UninstallDisplayIcon={app}\{#MyAppExeName}
UninstallDisplayName={#MyAppName} {#MyAppVersion}
OutputDir=..\release\v3.0.0
OutputBaseFilename=SmartCalculator_Setup_v3.0.0
SetupIconFile=..\Assets\AppIcon.ico
Compression=lzma2
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=admin
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
ChangesAssociations=no
LicenseFile=..\LICENSE
VersionInfoVersion=3.0.0.0
VersionInfoProductVersion=3.0.0
CloseApplications=yes
RestartApplications=no

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "Create a &desktop shortcut"; GroupDescription: "Additional icons:"; Flags: checkedonce

[Files]
Source: "..\bin\Release\net8.0-windows\win-x64\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "..\Help.html"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\HELP.md"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Help.chm"; DestDir: "{app}"; Flags: ignoreversion skipifsourcedoesntexist
Source: "..\README.md"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\LICENSE"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; WorkingDir: "{app}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; WorkingDir: "{app}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#MyAppName}}"; Flags: nowait postinstall skipifsilent
