Public Class RemotingExperiments


	'Note from 31052023: The final implementation of DS-Remoting (as found in RC4) differs from this mock-up, most notably in that DSCLI.exe and DSCore.dll are used instead of DSCLI.ILM.exe



	REM ╔═════════════════════════════════════════════╗
	REM ║             DocScript Remoting              ║
	REM ╚═════════════════════════════════════════════╝

	REM Headless Mode - Creates an unseen DSCLI.exe process, which can't Output, Input, or Play Audio
	'	Needs:
	'		- DSCLI.ILM.exe on the remote computer
	'		- (E.g.) DSRemotingSession_03042023_135503_ScriptAK47.DS on the remote computer
	'
	'	Process:
	'		- E.g. CopyFiles_ToRemoteComputer("\\192.168.1.85\C$\Windows\DSRemoting\", "User", "Pass", {"DSCLI.ILM.exe", "...ScriptAK47.DS"})
	'		- E.g. CreateRemoteProcess("192.168.1.85", "User", "Pass", "C:\Windows\DSRemoting\DSCLI.ILM.exe /Run /SourceFile:""C:\Windows\DSRemoting\...ScriptAK47.DS""")


	REM Shell Mode - Starts a DSCLI.exe process, and handles command-line interaction to and from it
	'	Needs:
	'		- DSCLI.ILM.exe on the remote computer
	'		- (E.g.) DSRemotingSession_03042023_135503_ScriptAK47.DS on the remote computer
	'		- PsExec.exe on the local computer
	'
	'	Process:
	'		- E.g. CopyFiles_ToRemoteComputer("\\192.168.1.85\C$\Windows\DSRemoting\", "User", "Pass", {"DSCLI.ILM.exe", "...ScriptAK47.DS"})
	'		- E.g. PsExec.exe \\192.168.1.85 -u Administrator -p PASS123 C:\Windows\DSRemoting\DSCLI.ILM.exe /Run /SourceFile:""C:\Windows\DSRemoting\...ScriptBC09.DS"" /DocScriptCLAs:"..."


	REM Interactive Mode - Starts a DSCLI.exe process on a specified Session of the remote computer; Supports GUI execution
	'	Needs:
	'		- DSCLI.ILM.exe on the remote computer
	'		- (E.g.) DSRemotingSession_03042023_135503_ScriptAK47.DS on the remote computer
	'		- PsExec.exe on the local computer
	'
	'	Features:
	'		- Can choose the session on the target computer
	'		- Can choose to execute in /GUI mode
	'
	'	Process:
	'		- E.g. CopyFiles_ToRemoteComputer("\\192.168.1.85\C$\Windows\DSRemoting\", "User", "Pass", {"DSCLI.ILM.exe", "...ScriptAK47.DS"})
	'		- E.g. PsExec.exe \\192.168.1.85 -u Administrator -p PASS123 -i -d C:\Windows\DSRemoting\DSCLI.ILM.exe /Run /GUI /SourceFile:""C:\Windows\DSRemoting\...ScriptBC09.DS"" /DocScriptCLAs:"..."


	REM Resources:
	'	Copy file over network, with crednetials in .NET	https://stackoverflow.com/questions/17786037/copy-files-with-authentication-in-c-sharp


	Private Sub WMI() Handles Button1.Click
		Try

			Dim _TargetComputer$ = Me.TargetPCTextBox.Text

			Dim _ConnectionOptions As New System.Management.ConnectionOptions() With {
			 .Username = Me.UsernameTextBox.Text,
			 .Password = Me.PasswordTextBox.Text
			}

			Dim _ManagmentScope As New System.Management.ManagementScope(path:=String.Format("\\{0}\root\cimv2", _TargetComputer), options:=_ConnectionOptions)
			_ManagmentScope.Connect()

			Dim _ManagmentClass As New System.Management.ManagementClass(_ManagmentScope, New System.Management.ManagementPath("Win32_Process"), New System.Management.ObjectGetOptions())
			_ManagmentClass.InvokeMethod("Create", {Me.CommandTextBox.Text})

			MsgBox("Created Process on " & _TargetComputer)

		Catch ex As Exception
			MsgBox(ex.Message, MsgBoxStyle.Critical, "That went down well, didn't it...")
		End Try
	End Sub

	Public Sub New()

		' This call is required by the designer.
		InitializeComponent()

		' Add any initialization after the InitializeComponent() call.
		Application.EnableVisualStyles()

	End Sub

	Private Sub AuthAndCopyFile_ToRemoteHost() Handles AuthAndCopyButton.Click
		Try

			'Disconnect in case we are currently connected with our credentials
			NetworkShare.DisconnectFromShare((New IO.FileInfo(Me.RemoteFileTextBox.Text)).DirectoryName.Output(Sub(__$) Me.Text = __), force:=True)

			'Connect \w new credentials
			NetworkShare.ConnectToShare(
			 uri:=(New IO.FileInfo(Me.RemoteFileTextBox.Text)).DirectoryName,
			 username:=Me.UsernameTextBox.Text,
			 password:=Me.PasswordTextBox.Text
			)

			If Not IO.Directory.Exists((New IO.FileInfo(Me.RemoteFileTextBox.Text)).DirectoryName) Then IO.Directory.CreateDirectory((New IO.FileInfo(Me.RemoteFileTextBox.Text)).DirectoryName)
			IO.File.Copy(Me.LocalFileTextBox.Text, Me.RemoteFileTextBox.Text, overwrite:=True)

			'Final Disconnection
			NetworkShare.DisconnectFromShare((New IO.FileInfo(Me.RemoteFileTextBox.Text)).DirectoryName.Output(Sub(__$) Me.Text = __), force:=False)

			MsgBox("Copied file to " & Me.RemoteFileTextBox.Text)

		Catch ex As Exception
			MsgBox(ex.Message, MsgBoxStyle.Critical, "That went down well, didn't it...")
		End Try
	End Sub

	''' <summary>
	''' Uses WMI to run the specified command line on the remote Windows(TM) computer.
	''' </summary>
	''' <param name="_TargetPC_HostnameOrIP">E.g. "192.168.1.85"</param>
	''' <param name="_Username_ForTargetPC">E.g. ".\Administrator"</param>
	''' <param name="_Password_ForTargetPC">E.g. "Pass!123"</param>
	''' <param name="_CommandLineToRun_OnRemotePC">E.g. "C:\Res\DSCLI.exe C:\Res\Script.DS"</param>
	''' <remarks></remarks>
	Public Shared Sub WMI_CreateRemoteProcess(
	  ByVal _TargetPC_HostnameOrIP$,
	  ByVal _Username_ForTargetPC$,
	  ByVal _Password_ForTargetPC$,
	  ByVal _CommandLineToRun_OnRemotePC$
	)
		Try

			Dim _ConnectionOptions As New System.Management.ConnectionOptions() With {.Username = _Username_ForTargetPC, .Password = _Password_ForTargetPC}
			Dim _ManagmentScope As New System.Management.ManagementScope(path:=String.Format("\\{0}\root\cimv2", _TargetPC_HostnameOrIP), options:=_ConnectionOptions)
			_ManagmentScope.Connect()

			Dim _ManagmentClass As New System.Management.ManagementClass(_ManagmentScope, New System.Management.ManagementPath("Win32_Process"), New System.Management.ObjectGetOptions())
			_ManagmentClass.InvokeMethod("Create", {_CommandLineToRun_OnRemotePC})

		Catch _Ex As Exception : Throw New DSException("@CreateRemoteProcess: " & _Ex.Message, _Ex) : End Try
	End Sub

	''' <summary>
	''' Copies each of the _FilesToCopyThither into the _UNCDirectoryOnRemotePC_IntoWhichFilesWillBeCopied, using the specified credentials.
	''' </summary>
	''' <param name="_RemotePC_Username">E.g. ".\Administrator"</param>
	''' <param name="_RemotePC_Password">E.g. "Pass!123"</param>
	''' <param name="_UNCDirectoryOnRemotePC_IntoWhichFilesWillBeCopied">E.g. "\\192.168.1.85\C$\Windows\DSRemoting\"</param>
	''' <param name="_FilesToCopyThither">E.g. {"C:\File1.txt", "F:\Folder\File2.PNG"}</param>
	''' <remarks></remarks>
	Public Shared Sub CopyFiles_ToRemoteComputer(
	  ByVal _UNCDirectoryOnRemotePC_IntoWhichFilesWillBeCopied As IO.DirectoryInfo,
	  ByVal _RemotePC_Username$,
	  ByVal _RemotePC_Password$,
	  ByVal _FilesToCopyThither As IO.FileInfo()
	)
		Try

			'Disconnect in case we are currently connected with our credentials
			NetworkShare.DisconnectFromShare(_UNCDirectoryOnRemotePC_IntoWhichFilesWillBeCopied.FullName, force:=True)

			'Connect \w new credentials
			NetworkShare.ConnectToShare(
			 uri:=_UNCDirectoryOnRemotePC_IntoWhichFilesWillBeCopied.FullName,
			 username:=_RemotePC_Username,
			 password:=_RemotePC_Password
			)

			'Use the open authenticated UNC Connection
			If Not IO.Directory.Exists(_UNCDirectoryOnRemotePC_IntoWhichFilesWillBeCopied.FullName) Then IO.Directory.CreateDirectory(_UNCDirectoryOnRemotePC_IntoWhichFilesWillBeCopied.FullName)
			For Each _SourceFile As IO.FileInfo In _FilesToCopyThither
				[_SourceFile].CopyTo(destFileName:=IO.Path.Combine(_UNCDirectoryOnRemotePC_IntoWhichFilesWillBeCopied.FullName, _SourceFile.Name), overwrite:=True)
			Next

			'Final Disconnection
			NetworkShare.DisconnectFromShare(_UNCDirectoryOnRemotePC_IntoWhichFilesWillBeCopied.FullName, force:=False)

		Catch _Ex As Exception : Throw New DSException("@CopyFiles_ToRemoteComputer: " & _Ex.Message, _Ex) : End Try
	End Sub

End Class








Module NetworkShare

	Function ConnectToShare(ByVal uri As String, ByVal username As String, ByVal password As String) As String

		Dim nr As New NETRESOURCE() With {
			.dwType = RESOURCETYPE_DISK,
			.lpRemoteName = uri
		}

		Dim ret As Integer = WNetUseConnection(IntPtr.Zero, nr, password, username, 0, Nothing, Nothing, Nothing)

		If ret = NO_ERROR Then
			Return Nothing
		Else
			Return GetError(ret)
		End If

	End Function

	Function DisconnectFromShare(ByVal uri As String, ByVal force As Boolean) As String
		Dim ret As Integer = WNetCancelConnection(uri, force)

		If ret = NO_ERROR Then
			Return Nothing
		Else
			Return GetError(ret)
		End If
	End Function

	<System.Runtime.InteropServices.DllImport("Mpr.dll")>
	Private Function WNetUseConnection(ByVal hwndOwner As IntPtr, ByVal lpNetResource As NETRESOURCE, ByVal lpPassword As String, ByVal lpUserID As String, ByVal dwFlags As Integer, ByVal lpAccessName As String, ByVal lpBufferSize As String, ByVal lpResult As String) As Integer
	End Function

	<System.Runtime.InteropServices.DllImport("Mpr.dll")>
	Private Function WNetCancelConnection(ByVal lpName As String, ByVal fForce As Boolean) As Integer
	End Function

	<System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)>
	Private Class NETRESOURCE
		Public dwScope As Integer = 0
		Public dwType As Integer = 0
		Public dwDisplayType As Integer = 0
		Public dwUsage As Integer = 0
		Public lpLocalName As String = ""
		Public lpRemoteName As String = ""
		Public lpComment As String = ""
		Public lpProvider As String = ""
	End Class

	Const RESOURCETYPE_DISK As Integer = &H1
	Const CONNECT_UPDATE_PROFILE As Integer = &H1
	Const NO_ERROR As Integer = 0
	Const ERROR_ACCESS_DENIED As Integer = 5
	Const ERROR_ALREADY_ASSIGNED As Integer = 85
	Const ERROR_BAD_DEVICE As Integer = 1200
	Const ERROR_BAD_NET_NAME As Integer = 67
	Const ERROR_BAD_PROVIDER As Integer = 1204
	Const ERROR_CANCELLED As Integer = 1223
	Const ERROR_EXTENDED_ERROR As Integer = 1208
	Const ERROR_INVALID_ADDRESS As Integer = 487
	Const ERROR_INVALID_PARAMETER As Integer = 87
	Const ERROR_INVALID_PASSWORD As Integer = 1216
	Const ERROR_MORE_DATA As Integer = 234
	Const ERROR_NO_MORE_ITEMS As Integer = 259
	Const ERROR_NO_NET_OR_BAD_PATH As Integer = 1203
	Const ERROR_NO_NETWORK As Integer = 1222
	Const ERROR_SESSION_CREDENTIAL_CONFLICT As Integer = 1219
	Const ERROR_BAD_PROFILE As Integer = 1206
	Const ERROR_CANNOT_OPEN_PROFILE As Integer = 1205
	Const ERROR_DEVICE_IN_USE As Integer = 2404
	Const ERROR_NOT_CONNECTED As Integer = 2250
	Const ERROR_OPEN_FILES As Integer = 2401

	Private Structure ErrorClass

		Public num As Integer
		Public message As String

		Public Sub New(ByVal num As Integer, ByVal message As String)
			Me.num = num
			Me.message = message
		End Sub

	End Structure

	Private ERROR_LIST As ErrorClass() = New ErrorClass() {New ErrorClass(ERROR_ACCESS_DENIED, "Error: Access Denied"), New ErrorClass(ERROR_ALREADY_ASSIGNED, "Error: Already Assigned"), New ErrorClass(ERROR_BAD_DEVICE, "Error: Bad Device"), New ErrorClass(ERROR_BAD_NET_NAME, "Error: Bad Net Name"), New ErrorClass(ERROR_BAD_PROVIDER, "Error: Bad Provider"), New ErrorClass(ERROR_CANCELLED, "Error: Cancelled"), New ErrorClass(ERROR_EXTENDED_ERROR, "Error: Extended Error"), New ErrorClass(ERROR_INVALID_ADDRESS, "Error: Invalid Address"), New ErrorClass(ERROR_INVALID_PARAMETER, "Error: Invalid Parameter"), New ErrorClass(ERROR_INVALID_PASSWORD, "Error: Invalid Password"), New ErrorClass(ERROR_MORE_DATA, "Error: More Data"), New ErrorClass(ERROR_NO_MORE_ITEMS, "Error: No More Items"), New ErrorClass(ERROR_NO_NET_OR_BAD_PATH, "Error: No Net Or Bad Path"), New ErrorClass(ERROR_NO_NETWORK, "Error: No Network"), New ErrorClass(ERROR_BAD_PROFILE, "Error: Bad Profile"), New ErrorClass(ERROR_CANNOT_OPEN_PROFILE, "Error: Cannot Open Profile"), New ErrorClass(ERROR_DEVICE_IN_USE, "Error: Device In Use"), New ErrorClass(ERROR_EXTENDED_ERROR, "Error: Extended Error"), New ErrorClass(ERROR_NOT_CONNECTED, "Error: Not Connected"), New ErrorClass(ERROR_OPEN_FILES, "Error: Open Files"), New ErrorClass(ERROR_SESSION_CREDENTIAL_CONFLICT, "Error: Credential Conflict")}

	Private Function GetError(ByVal errNum As Integer) As String

		For Each er As ErrorClass In ERROR_LIST
			If er.num = errNum Then Return er.message
		Next

		Return "Error: Unknown, " & errNum

	End Function

End Module