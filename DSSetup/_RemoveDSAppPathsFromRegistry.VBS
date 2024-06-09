REM Ben Mullan 2023
REM This script REMOVES the AppPaths from the registry for the DS Binaries,
REM so that they can't be run from e.g. the Run dialog.
REM This script must be run, from the same directory as that of the DocScript Binaries.

Const DSIDE_RegKeyPath = "SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\DSIDE.exe"
Const DSCLI_RegKeyPath = "SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\DSCLI.exe"
Const DSExpr_RegKeyPath = "SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\DSExpr.exe"

Const HKEY_LOCAL_MACHINE = &H80000002
Set Registry = GetObject("winmgmts:\\.\root\default:StdRegProv")

'DSIDE
Registry.DeleteKey HKEY_LOCAL_MACHINE, DSIDE_RegKeyPath

'DSCLI
Registry.DeleteKey HKEY_LOCAL_MACHINE, DSCLI_RegKeyPath

'DSExpr
Registry.DeleteKey HKEY_LOCAL_MACHINE, DSExpr_RegKeyPath

MsgBox """App Paths"" have successfully been removed from the registry.", vbInformation + vbOKOnly, "DS Uninstallation"