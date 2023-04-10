Public Class DSRemotingWindow

	Public Const Placeholder_DSIDE_Dir$ = "$(DSIDE_Dir)"
	Public Const Default_PsExecPath$ = "$(DSIDE_Dir)\PsExec.exe"
	Public Const Default_RemoteUNCPath$ = "C$\Windows\DSRemoting\"

	Public ReadOnly ProgramForRemoteExecution As DocScript.Runtime.Program
	Protected ReadOnly SnapshotOf_TabPage_Shell_, SnapshotOf_TabPage_Interactive_ As Forms.TabPage

	Public CurrentRemotingSession_ScriptFileName$ = Nothing
	Public TheActionInProgressWindow As ActionInProgressDialog = Nothing

	Public Sub New(ByRef _Program As DocScript.Runtime.Program)

		REM Initialise
		Me.InitializeComponent()
		Me.ProgramForRemoteExecution = _Program

		REM Set ToolTips (Why, WinForms; Why‽)
		With Me.TheToolTipProvider

			.SetToolTip(Me.DSRemotingBanner_PictureBox, "DS Remoting enables the execution of DocScript programs on remote computers, in one of three modes." & vbCrLf & "The program for remote execution, is whatever text was in the SourceEditor when this dialog was opened.")

			.SetToolTip(Me.HeadlessMode_RadioButton, "Creates an unseen DSCLI.exe process, which can't Output, Input, or Play Audio")
			.SetToolTip(Me.ShellMode_RadioButton, "Starts a DSCLI.exe process, and handles command-line interaction to and from it")
			.SetToolTip(Me.InteractiveMode_RadioButton, "Starts a DSCLI.exe process on a specified Session of the remote computer; Supports GUI execution")

			.SetToolTip(Me.SimpleMode_ComputerTextBox, "The Hostname or IP Addr. of the target Windows™ computer")

			.SetToolTip(Me.TargetComputer_TextBox, "The Hostname or IP Addr. of the target Windows™ computer")
			.SetToolTip(Me.TargetUsername_TextBox, "Use the form Domain\Username, or just Username")

		End With

		REM Account for the positioning of the SimpleMode_* Controls, which are positioned downwards for Design-time convienience only
		Const _PixelShift_Upwards% = 320
		Me.SimpleMode_InfoLabel.Location = New System.Drawing.Point(x:=Me.SimpleMode_InfoLabel.Location.X, y:=Me.SimpleMode_InfoLabel.Location.Y - _PixelShift_Upwards)
		Me.SimpleMode_ComputerTextBox.Location = New System.Drawing.Point(x:=Me.SimpleMode_ComputerTextBox.Location.X, y:=Me.SimpleMode_ComputerTextBox.Location.Y - _PixelShift_Upwards)
		Me.SimpleMode_ComputerLabel.Location = New System.Drawing.Point(x:=Me.SimpleMode_ComputerLabel.Location.X, y:=Me.SimpleMode_ComputerLabel.Location.Y - _PixelShift_Upwards)

		REM Hide the Shell Options TabPage
		Me.SnapshotOf_TabPage_Shell_ = Me.Shell_TabPage
		Me.SnapshotOf_TabPage_Interactive_ = Me.Interactive_TabPage
		Me.TheTabControl.TabPages.Remove(Me.SnapshotOf_TabPage_Shell_)

		REM These is set now, so that it can be stored in one constant, once
		Me.InteractiveMode_CustomPSEXECPATH_TextBox.Text = DSRemotingWindow.Default_PsExecPath
		Me.ShellMode_CustomPSEXECPATH_TextBox.Text = DSRemotingWindow.Default_PsExecPath
		Me.TargetUNCDirectory_TextBox.Text = DSRemotingWindow.Default_RemoteUNCPath

		REM Handler applied in Constructor, because otherwise this would be run during UI initialisation, before the SnapshotOf_* objects had been initialised
		AddHandler Me.ShellMode_RadioButton.CheckedChanged, AddressOf Me.ShowAndHideCorrectTabPages_DependingOnSelectedMode
		AddHandler Me.HeadlessMode_RadioButton.CheckedChanged, AddressOf Me.ShowAndHideCorrectTabPages_DependingOnSelectedMode
		AddHandler Me.InteractiveMode_RadioButton.CheckedChanged, AddressOf Me.ShowAndHideCorrectTabPages_DependingOnSelectedMode

		REM Make the Icons change the DSR-Mode
		AddHandler Me.HeadlessIcon_PictureBox.Click, Sub() Me.HeadlessMode_RadioButton.Checked = True
		AddHandler Me.ShellIcon_PictureBox.Click, Sub() Me.ShellMode_RadioButton.Checked = True
		AddHandler Me.InteractiveIcon_PictureBox.Click, Sub() Me.InteractiveMode_RadioButton.Checked = True

		REM Start in Simple Mode
		Me.ChangeLayoutTo_SimpleMode()

	End Sub

#Region "UI Utilities"

#Region "Close the form on pressing [Esc]"

	Protected Sub Register_CloseOnEscKey_Handler() Handles Me.Load
		Me.KeyPreview = True
		AddHandler Me.KeyDown, Sub(_Sender As Object, _KeyEventArgs As Forms.KeyEventArgs) If _KeyEventArgs.KeyCode = Forms.Keys.Escape Then Me.Close()
	End Sub

	Protected Overrides Function ProcessDialogKey(ByVal keyData As System.Windows.Forms.Keys) As Boolean
		If (Windows.Forms.Form.ModifierKeys = Forms.Keys.None) AndAlso (keyData = Forms.Keys.Escape) Then
			Me.Close() : Return True
		End If
		Return MyBase.ProcessDialogKey(keyData)
	End Function

#End Region

	REM Simple Mode
	Public Sub ChangeLayoutTo_SimpleMode()

		Me.TheTabControl.Hide()

		Me.ShowHideOptionsButton.Text = "Show &Options"
		Me.MaximumSize = New System.Drawing.Size(420, 260) : Me.MinimumSize = New System.Drawing.Size(420, 260) : Me.Size = New System.Drawing.Size(420, 260)
		Me.ConnectButton.Location = New System.Drawing.Point(x:=Me.ConnectButton.Location.X, y:=188)
		Me.ShowHideOptionsButton.Location = New System.Drawing.Point(x:=Me.ShowHideOptionsButton.Location.X, y:=188)

		Me.SimpleMode_ComputerLabel.Show() : Me.SimpleMode_ComputerTextBox.Show() : Me.SimpleMode_InfoLabel.Show()
		Me.ActiveControl = Me.SimpleMode_ComputerTextBox
		Me.SimpleMode_ComputerTextBox.Text = Me.TargetComputer_TextBox.Text

	End Sub

	REM Advanced Mode
	Public Sub ChangeLayoutTo_AdvancedMode()

		Me.TheTabControl.Show()

		Me.ShowHideOptionsButton.Text = "Hide &Options"
		Me.MaximumSize = New System.Drawing.Size(420, 470) : Me.MinimumSize = New System.Drawing.Size(420, 470) : Me.Size = New System.Drawing.Size(420, 470)
		Me.ConnectButton.Location = New System.Drawing.Point(x:=Me.ConnectButton.Location.X, y:=397)
		Me.ShowHideOptionsButton.Location = New System.Drawing.Point(x:=Me.ShowHideOptionsButton.Location.X, y:=397)

		Me.SimpleMode_ComputerLabel.Hide() : Me.SimpleMode_ComputerTextBox.Hide() : Me.SimpleMode_InfoLabel.Hide()
		Me.ActiveControl = Me.TheTabControl
		Me.TargetComputer_TextBox.Text = Me.SimpleMode_ComputerTextBox.Text

		'Start in the "Headless" Mode
		Call (Sub() Me.HeadlessMode_RadioButton.Checked = True).Invoke()

	End Sub

	Public ReadOnly Property IsCurrentlyInAdvancedMode As Boolean
		Get
			Return (Me.ShowHideOptionsButton.Text = "Hide &Options")
		End Get
	End Property

	Public Sub ToggleBetweenSimpleAndAdvancedUIModes() Handles ShowHideOptionsButton.Click
		If Me.IsCurrentlyInAdvancedMode Then Me.ChangeLayoutTo_SimpleMode() Else Me.ChangeLayoutTo_AdvancedMode()
	End Sub

	Public Sub ShowAndHideCorrectTabPages_DependingOnSelectedMode(ByVal __Unused_Sender As [Object], ByVal __Unused_EventArgs As EventArgs)	'Handler applied in Constructor, because otherwise this would be run during UI initialisation, before the SnapshotOf_* objects had been initialised
		If Me.ShellMode_RadioButton.Checked Then

			REM Show the Shell Page
			If Me.TheTabControl.TabPages.Contains(Me.SnapshotOf_TabPage_Interactive_) Then Me.TheTabControl.TabPages.Remove(Me.SnapshotOf_TabPage_Interactive_)
			If Not Me.TheTabControl.TabPages.Contains(Me.SnapshotOf_TabPage_Shell_) Then Me.TheTabControl.TabPages.Add(Me.SnapshotOf_TabPage_Shell_)

		ElseIf Me.InteractiveMode_RadioButton.Checked Then

			REM Show the Interactive Page
			If Me.TheTabControl.TabPages.Contains(Me.SnapshotOf_TabPage_Shell_) Then Me.TheTabControl.TabPages.Remove(Me.SnapshotOf_TabPage_Shell_)
			If Not Me.TheTabControl.TabPages.Contains(Me.SnapshotOf_TabPage_Interactive_) Then Me.TheTabControl.TabPages.Add(Me.SnapshotOf_TabPage_Interactive_)

		ElseIf Me.HeadlessMode_RadioButton.Checked Then

			REM Hide both the Shell and Interactive Pages
			If Me.TheTabControl.TabPages.Contains(Me.SnapshotOf_TabPage_Shell_) Then Me.TheTabControl.TabPages.Remove(Me.SnapshotOf_TabPage_Shell_)
			If Me.TheTabControl.TabPages.Contains(Me.SnapshotOf_TabPage_Interactive_) Then Me.TheTabControl.TabPages.Remove(Me.SnapshotOf_TabPage_Interactive_)

		Else : Throw New Exception("The selected mode was unrecognised")
		End If
	End Sub

#End Region

	Public Sub ExecuteDSRemotingOperation() Handles ConnectButton.Click
		Try

			Me.TheActionInProgressWindow = New ActionInProgressDialog("Initialising...")
			Me.TheActionInProgressWindow.ShowInNewThread()
			Me.Enabled = False

			REM For all DSR-Modes, the script must tempoarerily be saved to disk, so that it can be copied to the remote host
			Me.TheActionInProgressWindow.UpdateText("Reconstructing Program Source...")
			Me.CurrentRemotingSession_ScriptFileName$ = String.Format("DSRemotingSession_{2}_{0}_{1}.DS", DateTime.Now.ToString("ddMMyyyy"), DateTime.Now.ToString("HHmmss"), UsefulMethods.GetRandomString(4))
			My.Computer.FileSystem.WriteAllText(
			 file:=IO.Path.Combine(UsefulMethods.GetExecutingAssemblyDirectory().FullName.WithBackslashOnEnd(), CurrentRemotingSession_ScriptFileName),
			 text:=Me.ProgramForRemoteExecution.ReconstructedSource,
			 append:=False
			)

			If Me.IsCurrentlyInAdvancedMode Then

				REM Ensure the required data have been specified
				If {Me.TargetComputer_TextBox, Me.TargetUsername_TextBox, Me.TargetPassword_TextBox}.Any(Function(_TextBox As Forms.TextBox) String.IsNullOrEmpty(_TextBox.Text)) _
				   Then Throw New Exception("The Computer, Username, and Password data must all be specified")

				REM For all DSR-Modes, a copy of DSCLI.ILM.exe, and the session-specific DS Script, must be copied to the remote host
				Me.TheActionInProgressWindow.UpdateText("Copying Files to remote computer...")
				DocScript.Runtime.Remoting.CopyFiles_ToRemoteComputer(
				_UNCDirectoryOnRemotePC_IntoWhichFilesWillBeCopied:=(New IO.DirectoryInfo("\\" & Me.TargetComputer_TextBox.Text & "\"c & Me.TargetUNCDirectory_TextBox.Text)),
				_RemotePC_Username:=Me.TargetUsername_TextBox.Text,
				_RemotePC_Password:=Me.TargetPassword_TextBox.Text,
				_FilesToCopyThither:={
				  New IO.FileInfo(IO.Path.Combine(UsefulMethods.GetExecutingAssemblyDirectory().FullName.WithBackslashOnEnd(), Runtime.Remoting.DSCLI_ILM_EXE_FileName)),
				  New IO.FileInfo(IO.Path.Combine(UsefulMethods.GetExecutingAssemblyDirectory().FullName.WithBackslashOnEnd(), Me.CurrentRemotingSession_ScriptFileName))
				 }
				)

				REM Perform the correct Advanced-Mode Operation
				If Me.ShellMode_RadioButton.Checked Then
				ElseIf Me.InteractiveMode_RadioButton.Checked Then : Me.InteractiveMode_LaunchRemoteProcess_AdvancedMode()
				ElseIf Me.HeadlessMode_RadioButton.Checked Then : Me.HeadlessMode_CreateRemoteProcess()
				Else : Throw New Exception("The selected DSRemoting Mode was unrecognised")
				End If

				REM If instructed to, delete the DocScript Interpreter and Remote-copy-of-session-script from the target computer
				REM Wait a second before doing this, because otherwise, we might delete the script before DSCLI.exe has read it
				System.Threading.Thread.Sleep(millisecondsTimeout:=1500)
				If Me.DeleteDSFilesFromRemoteAtEnd_CheckBox.Checked Then
					Me.TheActionInProgressWindow.UpdateText("Deleting Files from remote computer...")
					DocScript.Runtime.Remoting.DeleteFiles_FromRemoteComputer(
					 _UNCDirectoryOnRemotePC_FromWhichFilesWillBeDeleted:=(New IO.DirectoryInfo("\\" & Me.TargetComputer_TextBox.Text & "\"c & Me.TargetUNCDirectory_TextBox.Text)),
					 _RemotePC_Username:=Me.TargetUsername_TextBox.Text,
					 _RemotePC_Password:=Me.TargetPassword_TextBox.Text,
					 _NamesOfFilesWithinThatUNCDirectory_ToBeDeleted:={Runtime.Remoting.DSCLI_ILM_EXE_FileName, Me.CurrentRemotingSession_ScriptFileName}
					)
				End If

			Else

				REM Ensure the required data have been specified
				If String.IsNullOrEmpty(Me.SimpleMode_ComputerTextBox.Text) Then Throw New Exception("The Computer datum must all be specified")

				REM Prompt for Credentials first
				Me.TheActionInProgressWindow.UpdateText("Resolving Credentials...") : Me.TheActionInProgressWindow.SurpressUntilNextUpdate()
				Dim _SpecifiedCredentials As Tuple(Of String, String) = EnterCredentialsDialog.Show_AndGetCredentials()

				REM Use Interactive Mode, with the Console Session. DSR Files will not be deleted at the end.
				Me.TheActionInProgressWindow.UpdateText("Copying Files to remote computer...")
				DocScript.Runtime.Remoting.CopyFiles_ToRemoteComputer(
				_UNCDirectoryOnRemotePC_IntoWhichFilesWillBeCopied:=(New IO.DirectoryInfo("\\" & Me.SimpleMode_ComputerTextBox.Text & "\"c & DSRemotingWindow.Default_RemoteUNCPath)),
				_RemotePC_Username:=_SpecifiedCredentials.Item1,
				_RemotePC_Password:=_SpecifiedCredentials.Item2,
				_FilesToCopyThither:={
				  New IO.FileInfo(IO.Path.Combine(UsefulMethods.GetExecutingAssemblyDirectory().FullName.WithBackslashOnEnd(), Runtime.Remoting.DSCLI_ILM_EXE_FileName)),
				  New IO.FileInfo(IO.Path.Combine(UsefulMethods.GetExecutingAssemblyDirectory().FullName.WithBackslashOnEnd(), Me.CurrentRemotingSession_ScriptFileName))
				 }
				)

				Me.InteractiveMode_LaunchRemoteProcess_SimpleMode(_SpecifiedCredentials)

			End If

		Catch _DSR_Exception As Exception When True

			Me.TheActionInProgressWindow.Dismiss()
			MsgBox("The DS-Remoting operation failed. Reason: " & vbCrLf & vbCrLf & _DSR_Exception.Message & If(_DSR_Exception.Message.Contains("Logon failure"), vbCrLf & "Ensure that the specified user account has the required permissions, and that the time is set correctly on the remote computer. Perhaps attempt a different username syntax e.g. HOSTNAME\Username or .\Username or Username or Username@Domain.", If(_DSR_Exception.Message.Contains("Could not find file"), vbCrLf & "Ensure that there is a copy of DSCLI.ILM.exe in the same directory as DSIDE.exe. This feature is designed to work when DocScript is installed via the MSI.", "")), MsgBoxStyle.Critical, "DS-Remoting")

		Finally

			REM Attempt to delete the tempoary local session-specific Script file
			Try : IO.File.Delete(IO.Path.Combine(UsefulMethods.GetExecutingAssemblyDirectory().FullName.WithBackslashOnEnd(), CurrentRemotingSession_ScriptFileName)) : Catch : End Try

			REM Dismiss the ActionInProgressDialog
			Me.TheActionInProgressWindow.Dismiss()
			Me.Enabled = True

		End Try
	End Sub

	Public Sub HeadlessMode_CreateRemoteProcess()

		'For this mode, the Advanced UI Mode must have been used.

		REM Headless Mode - Creates an unseen DSCLI.exe process, which can't Output, Input, or Play Audio
		'	Needs:
		'		- DSCLI.ILM.exe on the remote computer
		'		- (E.g.) DSRemotingSessionAK47_03042023_135503.DS on the remote computer
		'
		'	Process:
		'		- E.g. CopyFiles_ToRemoteComputer("\\192.168.1.85\C$\Windows\DSRemoting\", "User", "Pass", {"DSCLI.ILM.exe", "...ScriptAK47.DS"})
		'		- E.g. CreateRemoteProcess("192.168.1.85", "User", "Pass", "C:\Windows\DSRemoting\DSCLI.ILM.exe /Run /SourceFile:""C:\Windows\DSRemoting\...ScriptAK47.DS""")

		'The command-to-run looks like e.g.:
		'\\192.168.1.85\C$\Windows\DSRemoting\DSCLI.ILM.exe /Run /SourceFile:"\\192.168.1.85\C$\Windows\DSRemoting\DSRemotingSessionAK47_03042023_135503.DS" /DocScriptCLAs:"Hello World 2 1"

		Me.TheActionInProgressWindow.UpdateText("Creating Remote Process...")
		DocScript.Runtime.Remoting.WMI_CreateRemoteProcess(
		 _TargetPC_HostnameOrIP:=Me.TargetComputer_TextBox.Text,
		 _Username_ForTargetPC:=Me.TargetUsername_TextBox.Text,
		 _Password_ForTargetPC:=Me.TargetPassword_TextBox.Text,
		 _CommandLineToRun_OnRemotePC:=String.Format(
		 "{0}\{1} /Run /SourceFile:""{0}\{2}""{3}",
		 (New IO.DirectoryInfo("\\" & Me.TargetComputer_TextBox.Text & "\"c & Me.TargetUNCDirectory_TextBox.Text)).FullName.WithoutBackslashOnEnd(),
		 DocScript.Runtime.Remoting.DSCLI_ILM_EXE_FileName,
		 Me.CurrentRemotingSession_ScriptFileName,
		 If(String.IsNullOrEmpty(Me.DocScriptCLAs_TextBox.Text), "", " /DocScriptCLAs:""" & Me.DocScriptCLAs_TextBox.Text & """"c)
		 )
		)

		'Use a new thread for the MsgBox, so that the remaining DS-Remoting operations continue (e.g. deleting the files from the remote host)
		Dim _MsgBoxThread As New System.Threading.Thread(Sub() MsgBox("The Interpreter Process was successfully started on " & Me.TargetComputer_TextBox.Text, MsgBoxStyle.Information, "DS-Remoting"))
		_MsgBoxThread.Start()

	End Sub

	Public Sub ShellMode_StartPsExecSession()

		'For this mode, the Advanced UI Mode must have been used.

		REM Shell Mode - Starts a DSCLI.exe process, and handles command-line interaction to and from it
		'	Needs:
		'		- DSCLI.ILM.exe on the remote computer
		'		- (E.g.) DSRemotingSession_03042023_135503_ScriptAK47.DS on the remote computer
		'		- PsExec.exe on the local computer
		'
		'	Process:
		'		- E.g. CopyFiles_ToRemoteComputer("\\192.168.1.85\C$\Windows\DSRemoting\", "User", "Pass", {"DSCLI.ILM.exe", "...ScriptAK47.DS"})
		'		- E.g. PsExec.exe \\192.168.1.85 -u Administrator -p Logon#123 C:\Windows\DSRemoting\DSCLI.ILM.exe /Run /SourceFile:""C:\Windows\DSRemoting\...ScriptBC09.DS"" /DocScriptCLAs:"..."

		Throw New NotImplementedException("Shell Mode has not been implemented yet")

	End Sub

	Public Sub InteractiveMode_LaunchRemoteProcess_AdvancedMode()

		'For this mode, EITHER the Simple, or Advanced UI Mode could have been used.
		'This Sub handles Advanced mode, using data from the form, and the below Sub handles Simple mode, using Consts

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
		'		- E.g. PsExec.exe \\192.168.1.85 -nobanner -accepteula -i -d -u Administrator -p Logon#123 C:\Windows\DSRemoting\DSCLI.ILM.exe /Run /GUI /SourceFile:""C:\Windows\DSRemoting\...ScriptBC09.DS"" /DocScriptCLAs:"..."

		Dim _PsExec_CLAs$ = [String].Format("""\\{0}"" -nobanner -accepteula -i{3} -d -u ""{1}"" -p ""{2}"" ""{4}\{5}"" /Run{6} /SourceFile:""{4}\{7}""{8}",
			Me.TargetComputer_TextBox.Text,
			Me.TargetUsername_TextBox.Text,
			Me.TargetPassword_TextBox.Text,
			If(Me.InteractiveMode_UseCustomSession_RadioButton.Checked, " "c & Me.InteractiveMode_CustomSessionNumber_NumericUpDown.Value.ToString().Extract_UInt32().ToString(), ""),
			(New IO.DirectoryInfo("\\" & Me.TargetComputer_TextBox.Text & "\"c & Me.TargetUNCDirectory_TextBox.Text)).FullName.WithoutBackslashOnEnd(),
			DocScript.Runtime.Remoting.DSCLI_ILM_EXE_FileName,
			If(Me.InteractiveMode_UseGUIExecution_CheckBox.Checked, " /GUI", ""),
			Me.CurrentRemotingSession_ScriptFileName,
			If(String.IsNullOrEmpty(Me.DocScriptCLAs_TextBox.Text), "", " /DocScriptCLAs:""" & Me.DocScriptCLAs_TextBox.Text & """"c)
		)

		REM If the Default PsExec exe isn't found, then ask for one.
		Dim _PsExec_ExePath$ = DSRemotingWindow.Default_PsExecPath.Replace(DSRemotingWindow.Placeholder_DSIDE_Dir, UsefulMethods.GetExecutingAssemblyDirectory.FullName.WithoutBackslashOnEnd())
		If Not IO.File.Exists(_PsExec_ExePath$) Then _PsExec_ExePath = InputBox(_PsExec_ExePath & " does not exist." & vbCrLf & vbCrLf & "Enter a path to ""PsExec.exe""...")
		If Not IO.File.Exists(_PsExec_ExePath$) Then Throw New IO.FileNotFoundException("The PsExec.exe process path could not be resolved.", _PsExec_ExePath)

		Me.TheActionInProgressWindow.UpdateText("Launching Interactive Remote Process...")
		Dim _PsExec_Process As New System.Diagnostics.Process() With {
		 .StartInfo = (New ProcessStartInfo() With {
		  .CreateNoWindow = True,
		  .WindowStyle = ProcessWindowStyle.Hidden,
		  .FileName = _PsExec_ExePath,
		  .Arguments = _PsExec_CLAs
		 })
		}
		Dim _PsExec_Output$ = UsefulMethods.RunProcess_AndReturnCommandLineOutput(_PsExec_Process)

		If (_PsExec_Process.ExitCode < 10) Then
			My.Computer.Clipboard.SetText(_PsExec_ExePath & " "c & _PsExec_CLAs)
			Throw New DSException("The PsExec Process exited with an unexpected exit code of " & _PsExec_Process.ExitCode.ToString() & ". Output: " & vbCrLf & vbCrLf & _PsExec_Output.Replace(vbCrLf & vbCrLf, vbCrLf) & vbCrLf & vbCrLf & "The Command-line used to invoke PsExec has been copied to the clipboard.")
		Else

			'Use a new thread for the MsgBox, so that the remaining DS-Remoting operations continue (e.g. deleting the files from the remote host)
			Dim _MsgBoxThread As New System.Threading.Thread(Sub() MsgBox("The Interactive Process was successfully started on " & Me.TargetComputer_TextBox.Text, MsgBoxStyle.Information, "DS-Remoting"))
			_MsgBoxThread.Start()

		End If

	End Sub

	Public Sub InteractiveMode_LaunchRemoteProcess_SimpleMode(ByVal _SpecifiedCredentials As Tuple(Of String, String))

		Dim _PsExec_CLAs$ = [String].Format("""\\{0}"" -nobanner -accepteula -i{3} -d -u ""{1}"" -p ""{2}"" ""{4}\{5}"" /Run{6} /SourceFile:""{4}\{7}""{8}",
			 Me.SimpleMode_ComputerTextBox.Text,
			 _SpecifiedCredentials.Item1,
			 _SpecifiedCredentials.Item2,
			 "",
			 (New IO.DirectoryInfo("\\" & Me.SimpleMode_ComputerTextBox.Text & "\"c & DSRemotingWindow.Default_RemoteUNCPath)).FullName.WithoutBackslashOnEnd(),
			 DocScript.Runtime.Remoting.DSCLI_ILM_EXE_FileName,
			 " /GUI",
			 Me.CurrentRemotingSession_ScriptFileName,
			 ""
		)

		REM If the Default PsExec exe isn't found, then ask for one.
		Dim _PsExec_ExePath$ = DSRemotingWindow.Default_PsExecPath.Replace(DSRemotingWindow.Placeholder_DSIDE_Dir, UsefulMethods.GetExecutingAssemblyDirectory.FullName.WithoutBackslashOnEnd())
		If Not IO.File.Exists(_PsExec_ExePath$) Then _PsExec_ExePath = InputBox(_PsExec_ExePath & " does not exist." & vbCrLf & vbCrLf & "Enter a path to ""PsExec.exe""...")
		If Not IO.File.Exists(_PsExec_ExePath$) Then Throw New IO.FileNotFoundException("The PsExec.exe process path could not be resolved.", _PsExec_ExePath)

		Me.TheActionInProgressWindow.UpdateText("Launching Interactive Remote Process...")
		Dim _PsExec_Process As New System.Diagnostics.Process() With {
		 .StartInfo = (New ProcessStartInfo() With {
		  .CreateNoWindow = True,
		  .WindowStyle = ProcessWindowStyle.Hidden,
		  .FileName = _PsExec_ExePath,
		  .Arguments = _PsExec_CLAs
		 })
		}
		Dim _PsExec_Output$ = UsefulMethods.RunProcess_AndReturnCommandLineOutput(_PsExec_Process)

		If (_PsExec_Process.ExitCode < 10) Then
			My.Computer.Clipboard.SetText(_PsExec_ExePath & " "c & _PsExec_CLAs)
			Throw New DSException("The PsExec Process exited with an unexpected exit code of " & _PsExec_Process.ExitCode.ToString() & ". Output: " & vbCrLf & vbCrLf & _PsExec_Output.Replace(vbCrLf & vbCrLf & vbCrLf, vbCrLf) & vbCrLf & vbCrLf & "The Command-line used to invoke PsExec has been copied to the clipboard.")
		Else

			'Use a new thread for the MsgBox, so that the remaining DS-Remoting operations continue (e.g. deleting the files from the remote host)
			Dim _MsgBoxThread As New System.Threading.Thread(Sub() MsgBox("The Interactive Process was successfully started on " & Me.SimpleMode_ComputerTextBox.Text, MsgBoxStyle.Information, "DS-Remoting"))
			_MsgBoxThread.Start()

		End If

	End Sub

End Class




REM ADD TEXTBOX CHAR VALIDATION!!!!!!!!!
REM Save the last Server in the .Config File!!