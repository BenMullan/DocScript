REM Ben Mullan 2023
REM This script ADDS the AppPaths to the registry for the DS Binaries,
REM so that they can be run from e.g. the Run dialog.
REM This script must be run, from the same directory as that of the DocScript Binaries.

Dim DSInstallationDir : DSInstallationDir = CreateObject("Scripting.FileSystemObject").GetParentFolderName(WScript.ScriptFullName) & "\"
Const DSIDE_RegKeyPath = "SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\DSIDE.exe"
Const DSCLI_RegKeyPath = "SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\DSCLI.exe"
Const DSExpr_RegKeyPath = "SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\DSExpr.exe"

Const HKEY_LOCAL_MACHINE = &H80000002
Set Registry = GetObject("winmgmts:\\.\root\default:StdRegProv")

'DSIDE
Registry.CreateKey HKEY_LOCAL_MACHINE, DSIDE_RegKeyPath
Registry.SetStringValue HKEY_LOCAL_MACHINE, DSIDE_RegKeyPath, "", DSInstallationDir & "DSIDE.exe"

'DSCLI
Registry.CreateKey HKEY_LOCAL_MACHINE, DSCLI_RegKeyPath
Registry.SetStringValue HKEY_LOCAL_MACHINE, DSCLI_RegKeyPath, "", DSInstallationDir & "DSCLI.exe"

'DSExpr
Registry.CreateKey HKEY_LOCAL_MACHINE, DSExpr_RegKeyPath
Registry.SetStringValue HKEY_LOCAL_MACHINE, DSExpr_RegKeyPath, "", DSInstallationDir & "DSExpr.exe"

MsgBox """App Paths"" have successfully been added to the registry, so that the DocScript binaries (DSIDE.exe, DSCLI.exe, and DSExpr.exe) can be called from the Run dialog.", vbInformation + vbOKOnly, "DS Installation"