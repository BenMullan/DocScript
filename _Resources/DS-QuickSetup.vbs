REM This script will download the latest versions of DSIDE, DSCLI, and DSExpr, into .\DocScript\
REM It will then open the folder containing the binaries, and launch DSIDE
REM Alternatively, you can download the Setup MSI from https://github.com/BenMullan/DocScript/releases/

Main()

Sub Main()
	
	WScript.Echo vbNewLine & vbNewLine & "Checking for .NET Framework (v4.0)..."
	If Not (GetObject("winmgmts:!root/default:StdRegProv").EnumKey(&H80000002, "SOFTWARE\Microsoft\.NETFramework\Policy\v4.0\", "") = 0) Then
		If MsgBox( _
			"The .NET Framework (v4.0) doesn't seem to be installed, but is required for DocScript." & vbNewLine & vbNewLine & _
			"Would you like to download it form Microsoft now?", _
			vbYesNo + vbExclamation, ".NET 4 Required" _
		) = vbYes Then CreateObject("WScript.Shell").Run "https://www.microsoft.com/en-US/Download/confirmation.aspx?id=17718"
		Exit Sub
	End If
	
	WScript.Echo vbNewLine & vbNewLine & "Testing TLS 1.2 Connection to DocScript Source Control..."
	If Not TLS12IsWorking()	Then
		If MsgBox( _
			"This computer could not connect to the DocScript Source Control Server, most probably because TLS 1.2 is not present or is disabled." & vbNewLine & vbNewLine & _
			"Would you like to enable TLS 1.2 now?", _
			vbYesNo + vbExclamation, "TLS 1.2 Required" _
		) = vbYes Then CreateObject("WScript.Shell").Run "https://support.microsoft.com/en-us/topic/update-to-enable-tls-1-1-and-tls-1-2-as-default-secure-protocols-in-winhttp-in-windows-c4bd73d2-31d7-761e-0178-11268bb10392"
		Exit Sub
	End If
	
	WScript.Echo vbNewLine & vbNewLine & "Downloading DocScript Binaries..."
	If Not CreateObject("Scripting.FileSystemObject").FolderExists(GetCurrentDirectory() & "DocScript\") Then _
		CreateObject("Scripting.FileSystemObject").CreateFolder(GetCurrentDirectory() & "DocScript\")
	
	DSFiles = Array( _
		"DSCore.dll", "DSCLI.exe", "DSIDE.exe", "DSExpr.exe", "ICSharpCode.AvalonEdit.dll", _
		"DocScriptFile.ICO", "PsExec.exe", "ILMerge.exe", "System.Compiler.dll" _
	)
	
	For Each DSFile In DSFiles
		DownloadFile _
			"https://github.com/BenMullan/DocScript/raw/master/DSWindowsIDE/bin/Release/" & DSFile, _
			GetCurrentDirectory() & "DocScript\" & DSFile
	Next
	
	Set GHLink = WScript.CreateObject("WScript.Shell").CreateShortcut(GetCurrentDirectory() & "DocScript\_DocScript_HomePage.LNK")
	GHLink.TargetPath = "https://github.com/BenMullan/DocScript/#docscript" : GHLink.Save()
	
	WScript.Echo vbNewLine & vbNewLine & "Launching DSIDE..."
	
	CreateObject("WScript.Shell").Run "Explorer """ & IIf( InStr(UCase(GetCurrentDirectory()), "SYSTEM32"), Replace(UCase(GetCurrentDirectory()), "SYSTEM32", "SYSWOW64"), GetCurrentDirectory() ) & "DocScript\""", 1
	CreateObject("WScript.Shell").Run GetCurrentDirectory() & "DocScript\DSIDE.exe /OpenSourceString:""#Thank you for Downloading!;#See the SAMPLE PROGRAMS at:;#https://github.com/BenMullan/DocScript/tree/master/_Resources/SamplePrograms/;;Function <Void> Main ();\\\\Output(`Press F5 to run me...`);EndFunction""", 1
	
	WScript.Echo "...Finished"
	CreateObject("Scripting.FileSystemObject").DeleteFile WScript.ScriptFullName, True 'Force Deletion
	
End Sub

REM ↓ Utility Functions ↓

Sub DownloadFile(URL, SaveAsPath)
	
	WScript.Echo "Saving " & SaveAsPath
	Dim HttpRequest	: Set HttpRequest = CreateObject("MSXML2.ServerXMLHTTP")
	Dim ByteStream	: Set ByteStream = CreateObject("Adodb.Stream")

	HttpRequest.Open "GET", URL, False 'Not Async
	HttpRequest.Send()

	With ByteStream
		.Type = 1 'Binary
		.Open()
		.Write HttpRequest.ResponseBody
		.SaveToFile SaveAsPath, 2 'Overwrite
	End With
	
End Sub

Function GetCurrentDirectory()
	CurrentDir = CreateObject("Scripting.FileSystemObject").GetParentFolderName(WScript.ScriptFullName)
	If Right(CurrentDir, 1) = "\" Then GetCurrentDirectory = CurrentDir Else GetCurrentDirectory = CurrentDir & "\"
End Function

Function IIf(Expr, TruePart, FalsePart)
    If CBool(Expr) Then
		IIf = TruePart
    Else
		IIf = FalsePart
    End If
End Function

Function TLS12IsWorking()
	
	'Clear any previous errors
	On Error Resume Next
	Err.Clear
	
	'Perform the potentially-error-generating operation
	Dim HttpRequest	: Set HttpRequest = CreateObject("MSXML2.ServerXMLHTTP")
	HttpRequest.Open "GET", "https://github.com/BenMullan/DocScript/", False 'Not Async
	HttpRequest.Send()
	
	'Determine if an Error occured
	If Not (Err.Number = 0) Then
		On Error Goto 0
		TLS12IsWorking = False : Exit Function
	End If
	
	TLS12IsWorking = True : Exit Function
	
End Function