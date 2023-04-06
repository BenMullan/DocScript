Namespace Runtime

	''' <summary>Contains resources for executing DocScript programs on remote computers</summary>
	Public Module Remoting

		Public Const DSCLI_ILM_EXE_FileName$ = "DSCLI.ILM.exe"

		''' <summary>
		''' Uses WMI to run the specified command line on the remote Windows(TM) computer.
		''' </summary>
		''' <param name="_TargetPC_HostnameOrIP">E.g. "192.168.1.85"</param>
		''' <param name="_Username_ForTargetPC">E.g. ".\Administrator"</param>
		''' <param name="_Password_ForTargetPC">E.g. "Pass!123"</param>
		''' <param name="_CommandLineToRun_OnRemotePC">E.g. "C:\Res\DSCLI.exe C:\Res\Script.DS"</param>
		''' <remarks></remarks>
		Public Sub WMI_CreateRemoteProcess(
		  ByVal _TargetPC_HostnameOrIP$,
		  ByVal _Username_ForTargetPC$,
		  ByVal _Password_ForTargetPC$,
		  ByVal _CommandLineToRun_OnRemotePC$
		)
			Try : LogSystemMessage("Creating Remote Process on " & _TargetPC_HostnameOrIP.InSquares(), LogEvent.DSEventSeverity.Infomation)

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
		''' <param name="_UNCDirectoryOnRemotePC_IntoWhichFilesWillBeCopied">E.g. "\\192.168.1.85\C$\Windows\DSRemoting\". The presence/absence of a trailing Backslash [\] is accounted-for.</param>
		''' <param name="_FilesToCopyThither">E.g. {"C:\File1.txt", "F:\Folder\File2.PNG"}</param>
		''' <remarks></remarks>
		Public Sub CopyFiles_ToRemoteComputer(
		 ByVal _UNCDirectoryOnRemotePC_IntoWhichFilesWillBeCopied As IO.DirectoryInfo,
		 ByVal _RemotePC_Username$,
		 ByVal _RemotePC_Password$,
		 ByVal _FilesToCopyThither As IO.FileInfo()
		)
			Try : LogSystemMessage("Copying Files to remote UNC Directory " & _UNCDirectoryOnRemotePC_IntoWhichFilesWillBeCopied.FullName.WithBackslashOnEnd().InSquares(), LogEvent.DSEventSeverity.Infomation)

				'Disconnect in case we are currently connected with our credentials
				Remoting.DisconnectFromShare(_UNCDirectoryOnRemotePC_IntoWhichFilesWillBeCopied.FullName.WithoutBackslashOnEnd(), force:=True)

				'Connect \w new credentials
				Remoting.ConnectToShare(
				 uri:=_UNCDirectoryOnRemotePC_IntoWhichFilesWillBeCopied.FullName.WithoutBackslashOnEnd(),
				 username:=_RemotePC_Username,
				 password:=_RemotePC_Password
				)

				'Use the open authenticated UNC Connection
				If Not _UNCDirectoryOnRemotePC_IntoWhichFilesWillBeCopied.Exists Then IO.Directory.CreateDirectory(_UNCDirectoryOnRemotePC_IntoWhichFilesWillBeCopied.FullName.WithoutBackslashOnEnd())
				For Each _SourceFile As IO.FileInfo In _FilesToCopyThither
					'[_SourceFile].CopyTo(destFileName:=IO.Path.Combine(_UNCDirectoryOnRemotePC_IntoWhichFilesWillBeCopied.FullName, _SourceFile.Name), overwrite:=True)
					IO.File.Copy(
					 sourceFileName:=[_SourceFile].FullName,
					 destFileName:=IO.Path.Combine(_UNCDirectoryOnRemotePC_IntoWhichFilesWillBeCopied.FullName.WithBackslashOnEnd(), _SourceFile.Name),
					 overwrite:=True
					)
				Next

				'Final Disconnection
				Remoting.DisconnectFromShare(_UNCDirectoryOnRemotePC_IntoWhichFilesWillBeCopied.FullName.WithoutBackslashOnEnd(), force:=False)

			Catch _Ex As Exception : Throw New DSException("@CopyFiles_ToRemoteComputer: " & _Ex.Message, _Ex) : End Try
		End Sub

		''' <summary>
		''' Deletes each _NamesOfFilesWithinThatUNCDirectory_ToBeDeleted File, from the _UNCDirectoryOnRemotePC_IntoWhichFilesWillBeCopied, using the specified credentials.
		''' </summary>
		''' <param name="_RemotePC_Username">E.g. ".\Administrator"</param>
		''' <param name="_RemotePC_Password">E.g. "Pass!123"</param>
		''' <param name="_UNCDirectoryOnRemotePC_FromWhichFilesWillBeDeleted">E.g. "\\192.168.1.85\C$\Windows\DSRemoting\". The presence/absence of a trailing Backslash [\] is accounted-for.</param>
		''' <param name="_NamesOfFilesWithinThatUNCDirectory_ToBeDeleted">E.g. {"File1.txt", "Folder\File2.PNG"}</param>
		''' <remarks></remarks>
		Public Sub DeleteFiles_FromRemoteComputer(
		 ByVal _UNCDirectoryOnRemotePC_FromWhichFilesWillBeDeleted As IO.DirectoryInfo,
		 ByVal _RemotePC_Username$,
		 ByVal _RemotePC_Password$,
		 ByVal _NamesOfFilesWithinThatUNCDirectory_ToBeDeleted As [String]()
		)
			Try : LogSystemMessage("Deleting Files from remote UNC Directory " & _UNCDirectoryOnRemotePC_FromWhichFilesWillBeDeleted.FullName.WithBackslashOnEnd().InSquares(), LogEvent.DSEventSeverity.Infomation)

				'Disconnect in case we are currently connected with our credentials
				Remoting.DisconnectFromShare(_UNCDirectoryOnRemotePC_FromWhichFilesWillBeDeleted.FullName.WithoutBackslashOnEnd(), force:=True)

				'Connect \w new credentials
				Remoting.ConnectToShare(
				 uri:=_UNCDirectoryOnRemotePC_FromWhichFilesWillBeDeleted.FullName.WithoutBackslashOnEnd(),
				 username:=_RemotePC_Username,
				 password:=_RemotePC_Password
				)

				'Use the open authenticated UNC Connection
				For Each _NameOfFileToDelete$ In _NamesOfFilesWithinThatUNCDirectory_ToBeDeleted
					IO.File.Delete(IO.Path.Combine(_UNCDirectoryOnRemotePC_FromWhichFilesWillBeDeleted.FullName.WithBackslashOnEnd(), _NameOfFileToDelete))
				Next

				'Final Disconnection
				Remoting.DisconnectFromShare(_UNCDirectoryOnRemotePC_FromWhichFilesWillBeDeleted.FullName.WithoutBackslashOnEnd(), force:=False)

			Catch _Ex As Exception : Throw New DSException("@DeleteFiles_FromRemoteComputer: " & _Ex.Message, _Ex) : End Try
		End Sub

#Region "(Private) Authenticated Network Share Access Logic (Does not follow Private_ nomenclature)"

		Private Function ConnectToShare(ByVal uri As String, ByVal username As String, ByVal password As String) As String

			Dim nr As NETRESOURCE = New NETRESOURCE()
			nr.dwType = RESOURCETYPE_DISK
			nr.lpRemoteName = uri
			Dim ret As Integer = WNetUseConnection(IntPtr.Zero, nr, password, username, 0, Nothing, Nothing, Nothing)

			If ret = NO_ERROR Then
				Return Nothing
			Else
				Return GetError(ret)
			End If

		End Function

		Private Function DisconnectFromShare(ByVal uri As String, ByVal force As Boolean) As String
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

		Private Const RESOURCETYPE_DISK As Integer = &H1
		Private Const CONNECT_UPDATE_PROFILE As Integer = &H1
		Private Const NO_ERROR As Integer = 0
		Private Const ERROR_ACCESS_DENIED As Integer = 5
		Private Const ERROR_ALREADY_ASSIGNED As Integer = 85
		Private Const ERROR_BAD_DEVICE As Integer = 1200
		Private Const ERROR_BAD_NET_NAME As Integer = 67
		Private Const ERROR_BAD_PROVIDER As Integer = 1204
		Private Const ERROR_CANCELLED As Integer = 1223
		Private Const ERROR_EXTENDED_ERROR As Integer = 1208
		Private Const ERROR_INVALID_ADDRESS As Integer = 487
		Private Const ERROR_INVALID_PARAMETER As Integer = 87
		Private Const ERROR_INVALID_PASSWORD As Integer = 1216
		Private Const ERROR_MORE_DATA As Integer = 234
		Private Const ERROR_NO_MORE_ITEMS As Integer = 259
		Private Const ERROR_NO_NET_OR_BAD_PATH As Integer = 1203
		Private Const ERROR_NO_NETWORK As Integer = 1222
		Private Const ERROR_SESSION_CREDENTIAL_CONFLICT As Integer = 1219
		Private Const ERROR_BAD_PROFILE As Integer = 1206
		Private Const ERROR_CANNOT_OPEN_PROFILE As Integer = 1205
		Private Const ERROR_DEVICE_IN_USE As Integer = 2404
		Private Const ERROR_NOT_CONNECTED As Integer = 2250
		Private Const ERROR_OPEN_FILES As Integer = 2401

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

#End Region

	End Module

End Namespace