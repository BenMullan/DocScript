Public Class DSRemotingWindow

	Public Const PlaceholderFor_DSIDE_Dir$ = "$(DSIDE_Dir)"

	Public Const Default_PsExecPath$ = "$(DSIDE_Dir)\PsExec.exe"
	Public Const Default_RemoteUNCPath$ = "C$\Windows\DSRemoting\"

	Public Const PermissableCharsFor_ComputerNameOrIPAddr$ = "0123456789ABCDEFGHIJKLNMOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz.-"
	Public Const PermissableCharsFor_Username$ = "0123456789ABCDEFGHIJKLNMOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz.-_@({})\ "

	Public ReadOnly ProgramForRemoteExecution As DocScript.Runtime.Program
	Public CurrentRemotingSession_ScriptFileName$ = Nothing

	Protected ReadOnly SnapshotOf_TabPage_Shell_, SnapshotOf_TabPage_Interactive_ As Forms.TabPage
	Public TheActionInProgressDialog As ActionInProgressDialog = Nothing

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
			.SetToolTip(Me.TargetUsername_TextBox, "Use the form Domain\Username, .\Username, Username@Domain, or just Username")
			.SetToolTip(Me.DocScriptCLAs_TextBox, "The Command-line Arguments received by the DocScript Program in the _CLAs parameter to the Main() Function")
			.SetToolTip(Me.TargetUNCDirectory_TextBox, "The Path on the Remote Computer, in which to store the DocScript Runtime files. E.g. ""C$\DSR\"".")
			.SetToolTip(Me.TargetUNCDirectory_TextBox, "Indicates that the copy of DSCLI.exe, DSCore.dll, and {SessionName}.DS used by DS-Remoting, are to be deleted shortly after the remote script's execution has begun.")

			.SetToolTip(Me.InteractiveMode_UseGUIExecution_CheckBox, "Indicates that the /GUI flag will be used for the remote DSCLI interpreter process")
			.SetToolTip(Me.InteractiveMode_HideConsoleBeforeExecution_CheckBox, "Indicates that the /HideConsoleBeforeExecution flag will be used for the remote DSCLI interpreter process." & vbCrLf & vbCrLf & "WARNING: Runtime-errors in the remote DocScript program will not be visible, when this checkbox is ticked.")
			.SetToolTip(Me.InteractiveMode_PsExecPath_TextBox, "PsExec is a utility used to execute the script remotely.")
			.SetToolTip(Me.ShellMode_PsExecPath_TextBox, "PsExec is a utility used to execute the script remotely.")

			REM At this point, it was discovered that the Designer could be used to set ToolTips on Controls.
			REM The remaining ToolTips can therefore be found on individual controls.

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

		REM These are set now, so that it can be stored in one constant, once
		Me.InteractiveMode_PsExecPath_TextBox.Text = DSRemotingWindow.Default_PsExecPath
		Me.ShellMode_PsExecPath_TextBox.Text = DSRemotingWindow.Default_PsExecPath
		Me.TargetUNCDirectory_TextBox.Text = DSRemotingWindow.Default_RemoteUNCPath

		REM Only permit valid input to the Textboxes
		AddHandler Me.SimpleMode_ComputerTextBox.TextChanged, Sub() Call {(Sub() Me.SimpleMode_ComputerTextBox.Text = Me.SimpleMode_ComputerTextBox.Text.OnlyAllowTheseChars(DSRemotingWindow.PermissableCharsFor_ComputerNameOrIPAddr.ToCharArray())), (Sub() Me.SimpleMode_ComputerTextBox.SelectionStart = Me.SimpleMode_ComputerTextBox.Text.Length), (Sub() Me.SimpleMode_ComputerTextBox.SelectionLength = 0)}.ToList().ForEach(Sub(_Sub As [Action]) _Sub.Invoke())
		AddHandler Me.TargetComputer_TextBox.TextChanged, Sub() Call {(Sub() Me.TargetComputer_TextBox.Text = Me.TargetComputer_TextBox.Text.OnlyAllowTheseChars(DSRemotingWindow.PermissableCharsFor_ComputerNameOrIPAddr.ToCharArray())), (Sub() Me.TargetComputer_TextBox.SelectionStart = Me.TargetComputer_TextBox.Text.Length), (Sub() Me.TargetComputer_TextBox.SelectionLength = 0)}.ToList().ForEach(Sub(_Sub As [Action]) _Sub.Invoke())
		AddHandler Me.TargetUsername_TextBox.TextChanged, Sub() Call {(Sub() Me.TargetUsername_TextBox.Text = Me.TargetUsername_TextBox.Text.OnlyAllowTheseChars(DSRemotingWindow.PermissableCharsFor_Username.ToCharArray())), (Sub() Me.TargetUsername_TextBox.SelectionStart = Me.TargetUsername_TextBox.Text.Length), (Sub() Me.TargetUsername_TextBox.SelectionLength = 0)}.ToList().ForEach(Sub(_Sub As [Action]) _Sub.Invoke())

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

	REM ShortcutKeys
	Public Sub HandleShortcutKey(ByVal _Sender As [Object], ByVal _KeyEventArgs As Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		If (_KeyEventArgs.Modifiers = Forms.Keys.Control) Then

			REM Ctrl + {Something}

			Select Case _KeyEventArgs.KeyCode
				Case Forms.Keys.S : If Me.IsCurrentlyInAdvancedMode Then Me.SaveCurrentSessionSettingsToFile()
				Case Forms.Keys.O : Me.ChangeLayoutTo_AdvancedMode() : Me.OpenSessionSettingsFromFile()
			End Select

		End If
	End Sub

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

	'Handler herefor applied in Constructor, because otherwise this would be run during UI initialisation, before the SnapshotOf_* objects had been initialised
	Public Sub ShowAndHideCorrectTabPages_DependingOnSelectedMode(ByVal __Unused__Sender As [Object], ByVal __Unused__EventArgs As EventArgs)
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

	Private Sub ShellMode_BrowseForPsExec_() Handles ShellMode_BrowseForPsExecButton.Click
		Me.Cursor = Forms.Cursors.WaitCursor : Me.Enabled = False
		Dim _SelectedPsExecFile As IO.FileInfo = UsefulMethods.RequestFilePath_WithOpenFileDialog("Browse for PsExec.exe", {New Tuple(Of String, String)("PE Binary Executables", "EXE")}, UsefulMethods.GetExecutingAssemblyDirectory.FullName.WithoutBackslashOnEnd())
		If _SelectedPsExecFile IsNot Nothing Then Me.ShellMode_PsExecPath_TextBox.Text = _SelectedPsExecFile.FullName
		Me.Enabled = True : Me.Cursor = Forms.Cursors.Default
	End Sub

	Private Sub InteractiveMode_BrowseForPsExec_() Handles InteractiveMode_BrowseForPsExecButton.Click
		Me.Cursor = Forms.Cursors.WaitCursor : Me.Enabled = False
		Dim _SelectedPsExecFile As IO.FileInfo = UsefulMethods.RequestFilePath_WithOpenFileDialog("Browse for PsExec.exe", {New Tuple(Of String, String)("PE Binary Executables", "EXE")}, UsefulMethods.GetExecutingAssemblyDirectory.FullName.WithoutBackslashOnEnd())
		If _SelectedPsExecFile IsNot Nothing Then Me.InteractiveMode_PsExecPath_TextBox.Text = _SelectedPsExecFile.FullName
		Me.Enabled = True : Me.Cursor = Forms.Cursors.Default
	End Sub

#Region "Ad-hoc RFE and PsExec Methods"

	REM Remote-File-Explorer
	Public Sub LaunchNewRFE_ToSpecifiedComputer() Handles Utility_LaunchNewRFE_LinkLabel.LinkClicked
		Try

			REM Ensure the required data have been specified
			If {Me.TargetComputer_TextBox.Text, Me.TargetUsername_TextBox.Text, Me.TargetPassword_TextBox.Text}.Any(AddressOf [String].IsNullOrEmpty) _
			 Then Throw New Exception("The Computer, Username, and Password data must all be specified, under [Target].")

			REM Show the RFE
			Dim _NewRFEWindowThread As New System.Threading.Thread(Sub() Call (New RemoteFileExplorerWindow("\\" & Me.TargetComputer_TextBox.Text & "\C$\", Me.TargetUsername_TextBox.Text, Me.TargetPassword_TextBox.Text)).ShowDialog())
			_NewRFEWindowThread.SetApartmentState(System.Threading.ApartmentState.STA)
			_NewRFEWindowThread.Start()

		Catch _Ex As Exception : MsgBox(_Ex.Message, MsgBoxStyle.Critical, "Couldn't open Remote-File-Explorer") : End Try
	End Sub

	Public Sub LaunchNewPsExecCMDSession_ToSpecifiedComputer() Handles Utility_StartRemoteShellSession_LinkLabel.LinkClicked
		Try

			REM Ensure the required data have been specified
			If {Me.TargetComputer_TextBox.Text, Me.TargetUsername_TextBox.Text, Me.TargetPassword_TextBox.Text}.Any(AddressOf [String].IsNullOrEmpty) _
			 Then Throw New Exception("The Computer, Username, and Password data must all be specified, under [Target].")

			REM Resolve a PsExec.exe path
			Dim _PsExec_ExePath$ = Me.ShellMode_PsExecPath_TextBox.Text.Replace(DSRemotingWindow.PlaceholderFor_DSIDE_Dir, UsefulMethods.GetExecutingAssemblyDirectory.FullName.WithoutBackslashOnEnd())
			If Not IO.File.Exists(_PsExec_ExePath$) Then _PsExec_ExePath = Me.InteractiveMode_PsExecPath_TextBox.Text.Replace(DSRemotingWindow.PlaceholderFor_DSIDE_Dir, UsefulMethods.GetExecutingAssemblyDirectory.FullName.WithoutBackslashOnEnd())
			If Not IO.File.Exists(_PsExec_ExePath$) Then _PsExec_ExePath = InputBox(_PsExec_ExePath & " does not exist." & vbCrLf & vbCrLf & "Enter a path to ""PsExec.exe""...")
			If Not IO.File.Exists(_PsExec_ExePath$) Then Throw New IO.FileNotFoundException("The PsExec.exe process path could not be resolved.", _PsExec_ExePath)

			REM Prepare PsExec CLAs
			Dim _PsExec_CLAs$ = [String].Format("""\\{0}"" -nobanner -accepteula -u ""{1}"" -p ""{2}"" CMD.EXE", Me.TargetComputer_TextBox.Text, Me.TargetUsername_TextBox.Text, Me.TargetPassword_TextBox.Text)

			REM Identify a Command Processor to use (e.g. cmd.exe or command.com)
			Dim _CommandProcessorExecutable_Path$ = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Windows), "System32", "cmd.exe")
			If Not IO.File.Exists(_CommandProcessorExecutable_Path$) Then _CommandProcessorExecutable_Path = InputBox(_CommandProcessorExecutable_Path & " does not exist." & vbCrLf & vbCrLf & "Enter a path to ""cmd.exe"" or another command processor executable supporting the same command-line arguments...")
			If Not IO.File.Exists(_CommandProcessorExecutable_Path$) Then Throw New IO.FileNotFoundException("The command processor executable path could not be resolved.", _CommandProcessorExecutable_Path)

			REM Launch the new Process
			Dim _PsExec_ViaCmdExe_Process As New System.Diagnostics.Process() With {
			.StartInfo = (New ProcessStartInfo() With {
			 .CreateNoWindow = False,
			 .WindowStyle = ProcessWindowStyle.Normal,
			 .FileName = _CommandProcessorExecutable_Path,
			 .Arguments = String.Format("/K Title DSR Ad-hoc Shell Session && Prompt The PsExec session has finished.$_Type ""exit"" and press [Enter] to close...$_DSRemoting$G && ""{0}"" {1}", _PsExec_ExePath, _PsExec_CLAs)
			})
			  }
			_PsExec_ViaCmdExe_Process.Start()

		Catch _Ex As Exception : MsgBox(_Ex.Message, MsgBoxStyle.Critical, "Couldn't launch Shell-Session") : End Try
	End Sub

#End Region

#End Region

#Region "File Save and Open Implementation"

	Public Const DSRemotingSession_FileExtention$ = "DSRemotingSession"

	Public Sub SaveCurrentSessionSettingsToFile() Handles SaveSessionButton.Click
		Try

			Me.Cursor = Forms.Cursors.WaitCursor : Me.Enabled = False
			Dim _TargetFile As IO.FileInfo = UsefulMethods.RequestFilePath_WithSaveFileDialog("Save Connection Settings...", {New Tuple(Of [String], [String])("DSRemoting Connection Settings", DSRemotingSession_FileExtention)})
			If _TargetFile IsNot Nothing Then My.Computer.FileSystem.WriteAllText(_TargetFile.FullName, Me.SerialisationForCurrentConnectionSettings.ToString(SaveOptions.None), [append]:=False)

		Catch _Ex As Exception
			MsgBox("The Connection Settings could not be saved because " & _Ex.Message, MsgBoxStyle.Critical Or MsgBoxStyle.ApplicationModal)
		Finally
			Me.Enabled = True : Me.Cursor = Forms.Cursors.Default : Me.Activate()
		End Try
	End Sub

	Public ReadOnly Property SerialisationForCurrentConnectionSettings As XElement
		Get

			Return _
			 <DSRemotingSession>

				 <!-- Automatically generated by DSRemoting -->

				 <ConnectionSettings
					 DSRMode=<%= Me.CurrentlySelectedDSRMode.ToString("G") %>
					 Computer=<%= Me.TargetComputer_TextBox.Text %>
					 Username=<%= Me.TargetUsername_TextBox.Text %>
					 Password=<%= Me.TargetPassword_TextBox.Text %>
					 DocScriptCLAs=<%= Me.DocScriptCLAs_TextBox.Text %>
					 UNCDirectory=<%= Me.TargetUNCDirectory_TextBox.Text %>
					 DeleteDSRemotingFilesAtEnd=<%= Me.DeleteDSFilesFromRemoteAtEnd_CheckBox.Checked.ToString() %>
					 ShellMode_PsExecPath=<%= Me.ShellMode_PsExecPath_TextBox.Text %>
					 InteractiveMode_PsExecPath=<%= Me.InteractiveMode_PsExecPath_TextBox.Text %>
					 InteractiveMode_RunInGUIMode=<%= Me.InteractiveMode_UseGUIExecution_CheckBox.Checked.ToString() %>
					 InteractiveMode_HideInterpreterConsole=<%= Me.InteractiveMode_HideConsoleBeforeExecution_CheckBox.Checked.ToString() %>
					 InteractiveMode_Session=<%= If(Me.InteractiveMode_UseConsoleSession_RadioButton.Checked, "Console", Me.InteractiveMode_CustomSessionNumber_NumericUpDown.Value.ToString()) %>
				 />

			 </DSRemotingSession>

		End Get
	End Property

	Public Sub OpenSessionSettingsFromFile() Handles OpenSessionButton.Click
		Try

			Me.Cursor = Forms.Cursors.WaitCursor : Me.Enabled = False
			Dim _TargetFile As IO.FileInfo = UsefulMethods.RequestFilePath_WithOpenFileDialog("Open Connection Settings...", {New Tuple(Of [String], [String])("DSRemoting Connection Settings", DSRemotingSession_FileExtention)})
			If _TargetFile IsNot Nothing Then Me.LoadConnectionSettingsFromSerialisationThereof(XElement.Parse(_TargetFile.FullName.ReadTextFromThisFile()))
			Me.Text = _TargetFile.Name

		Catch _Ex As Exception
			MsgBox("The Connection Settings could not be opened because " & _Ex.Message, MsgBoxStyle.Critical Or MsgBoxStyle.ApplicationModal)
		Finally
			Me.Enabled = True : Me.Cursor = Forms.Cursors.Default : Me.Activate()
		End Try
	End Sub

#Region "Drag-and-Drop Handling"

	REM Also note that Me.AllowDrop = True

	Protected Sub HandleFileDragEnter_(ByVal _Sender As [Object], ByVal _DragEventArgs As Windows.Forms.DragEventArgs) Handles Me.DragEnter
		_DragEventArgs.Effect = Forms.DragDropEffects.All
	End Sub

	Protected Sub HandleFileDragDrop_(ByVal _Sender As [Object], ByVal _DragEventArgs As Windows.Forms.DragEventArgs) Handles Me.DragDrop
		Try : Me.Cursor = Forms.Cursors.WaitCursor : Me.Enabled = False

			'Get all Files Dropped. TryCast() returns Nothing if the conversion fails.
			Dim _Files$() = TryCast(_DragEventArgs.Data.GetData(DataFormats.FileDrop), [String]())
			If (_Files IsNot Nothing) AndAlso (_Files.Length >= 1) Then
				Me.ChangeLayoutTo_AdvancedMode()
				Me.LoadConnectionSettingsFromSerialisationThereof(XElement.Parse(_Files.First().ReadTextFromThisFile()))
				Me.Text = (New IO.FileInfo(_Files.First())).Name
			End If

		Catch _Ex As Exception : MsgBox("The Connection Settings could not be opened via Drag-and-Drop because " & _Ex.Message, MsgBoxStyle.Critical Or MsgBoxStyle.ApplicationModal)
		Finally : Me.Enabled = True : Me.Cursor = Forms.Cursors.Default : Me.Activate()
		End Try
	End Sub

#End Region

	Public Sub LoadConnectionSettingsFromSerialisationThereof(ByVal _DSRemotingSession As XElement)
		Try

			REM We're expecting an XElement like the one in [SerialisationForCurrentConnectionSettings]
			REM Outer Tag: <DSRemotingSession>

			With _DSRemotingSession.<ConnectionSettings>.First()

				Select Case .@DSRMode.ToUpper()
					Case DSRemotingMode.Headless.ToString("G").ToUpper() : Me.HeadlessMode_RadioButton.Checked = True
					Case DSRemotingMode.Shell.ToString("G").ToUpper() : Me.ShellMode_RadioButton.Checked = True
					Case DSRemotingMode.Interactive.ToString("G").ToUpper() : Me.InteractiveMode_RadioButton.Checked = True
					Case Else : Throw New DSException("The DSRemoting Mode was unrecognised: " & .@DSRMode)
				End Select

				Me.TargetComputer_TextBox.Text = .@Computer
				Me.TargetUsername_TextBox.Text = .@Username
				Me.TargetPassword_TextBox.Text = .@Password
				Me.DocScriptCLAs_TextBox.Text = .@DocScriptCLAs
				Me.TargetUNCDirectory_TextBox.Text = .@UNCDirectory
				Me.DeleteDSFilesFromRemoteAtEnd_CheckBox.Checked = (.@DeleteDSRemotingFilesAtEnd.ToUpper() = Boolean.TrueString.ToUpper())
				Me.ShellMode_PsExecPath_TextBox.Text = .@ShellMode_PsExecPath
				Me.InteractiveMode_PsExecPath_TextBox.Text = .@InteractiveMode_PsExecPath
				Me.InteractiveMode_UseGUIExecution_CheckBox.Checked = (.@InteractiveMode_RunInGUIMode.ToUpper() = Boolean.TrueString.ToUpper())
				Me.InteractiveMode_HideConsoleBeforeExecution_CheckBox.Checked = (.@InteractiveMode_HideInterpreterConsole.ToUpper() = Boolean.TrueString.ToUpper())

				If .@InteractiveMode_Session.ToUpper() = "CONSOLE" Then
					Me.InteractiveMode_UseConsoleSession_RadioButton.Checked = True
				Else
					Me.InteractiveMode_CustomSessionNumber_NumericUpDown.Value = Convert.ToDecimal(.@InteractiveMode_Session)
					Me.InteractiveMode_UseCustomSession_RadioButton.Checked = True
				End If

			End With

		Catch _Ex As Exception : Throw New DSException("The Connection Settings XML File could not be loaded because " & _Ex.Message) : End Try
	End Sub

#End Region

#Region "Actual DS-Remoting Logic"

	Public Enum DSRemotingMode As [Byte]
		Headless = 1
		Shell = 2
		Interactive = 3
	End Enum

	Public ReadOnly Property CurrentlySelectedDSRMode As DSRemotingMode
		Get
			If Me.HeadlessMode_RadioButton.Checked Then : Return DSRemotingMode.Headless
			ElseIf Me.ShellMode_RadioButton.Checked Then : Return DSRemotingMode.Shell
			ElseIf Me.InteractiveMode_RadioButton.Checked Then : Return DSRemotingMode.Interactive
			Else : Throw New Exception("The selected DSRemoting Mode was unrecognised")
			End If
		End Get
	End Property

	''' <summary>The main DS-Remoting [junction-box]-type Method.</summary>
	Public Sub ExecuteDSRemotingOperation() Handles ConnectButton.Click
		Try

			Me.TheActionInProgressDialog = New ActionInProgressDialog(_ActionDescription:="Initialising...")
			Me.TheActionInProgressDialog.ShowInNewThread()
			Me.Enabled = False

			REM For all DSR-Modes, the script must tempoarerily be saved to disk, so that it can be copied to the remote host.
			REM It will be deleted in the [Finally] Statement later.
			Me.TheActionInProgressDialog.UpdateText("Reconstructing Program Source...")
			Me.CurrentRemotingSession_ScriptFileName$ = [String].Format("DSRemotingSession_{2}_{0}_{1}.DS", DateTime.Now.ToString("ddMMyyyy"), DateTime.Now.ToString("HHmmss"), UsefulMethods.GetRandomString(4))
			My.Computer.FileSystem.WriteAllText(
			 file:=IO.Path.Combine(UsefulMethods.GetExecutingAssemblyDirectory().FullName.WithBackslashOnEnd(), CurrentRemotingSession_ScriptFileName),
			 text:=Me.ProgramForRemoteExecution.ReconstructedSource,
			 append:=False
			)

			If Me.IsCurrentlyInAdvancedMode Then

				REM Ensure the required data have been specified
				If {Me.TargetComputer_TextBox, Me.TargetUsername_TextBox, Me.TargetPassword_TextBox}.Any(Function(_TextBox As Forms.TextBox) String.IsNullOrEmpty(_TextBox.Text)) _
				   Then Throw New Exception("The Computer, Username, and Password data must all be specified")

				REM For all DSR-Modes, a copy of DSCLI.exe, DSCore.dll, and the session-specific DS Script, must be copied to the remote host
				Me.TheActionInProgressDialog.UpdateText("Authenticating and copying Files...")
				DocScript.Runtime.Remoting.CopyFiles_ToRemoteComputer(
				 _UNCDirectoryOnRemotePC_IntoWhichFilesWillBeCopied:=(New IO.DirectoryInfo("\\" & Me.TargetComputer_TextBox.Text & "\"c & Me.TargetUNCDirectory_TextBox.Text)),
				 _RemotePC_Username:=Me.TargetUsername_TextBox.Text,
				 _RemotePC_Password:=Me.TargetPassword_TextBox.Text,
				 [_FilesToCopyThither]:={
				  New IO.FileInfo(IO.Path.Combine(UsefulMethods.GetExecutingAssemblyDirectory().FullName.WithBackslashOnEnd(), Runtime.Remoting.DSCLI_EXE_FileName)),
				  New IO.FileInfo(IO.Path.Combine(UsefulMethods.GetExecutingAssemblyDirectory().FullName.WithBackslashOnEnd(), Runtime.Remoting.DSCore_DLL_FileName)),
				  New IO.FileInfo(IO.Path.Combine(UsefulMethods.GetExecutingAssemblyDirectory().FullName.WithBackslashOnEnd(), Me.CurrentRemotingSession_ScriptFileName))
				 }
				)

				REM Perform the correct Advanced-Mode Operation
				Select Case Me.CurrentlySelectedDSRMode
					Case DSRemotingMode.Shell : Me.ShellMode_StartPsExecSession()
					Case DSRemotingMode.Interactive : Me.InteractiveMode_LaunchRemoteProcess_AdvancedMode()
					Case DSRemotingMode.Headless : Me.HeadlessMode_CreateRemoteProcess()
				End Select

				REM If instructed to, delete the DocScript Interpreter and Remote-copy-of-session-script from the target computer.
				If Me.DeleteDSFilesFromRemoteAtEnd_CheckBox.Checked Then

					REM Wait a second before doing this, because otherwise, we might delete the script before the remote DSCLI.exe has read it.
					System.Threading.Thread.Sleep(millisecondsTimeout:=1500)

					REM Delete {DSCLI.exe, DSCore.dll, and *Session*.DS}
					Me.TheActionInProgressDialog.UpdateText("Deleting Files from remote computer...")
					DocScript.Runtime.Remoting.DeleteFiles_FromRemoteComputer(
					 _UNCDirectoryOnRemotePC_FromWhichFilesWillBeDeleted:=(New IO.DirectoryInfo("\\" & Me.TargetComputer_TextBox.Text & "\"c & Me.TargetUNCDirectory_TextBox.Text)),
					 _RemotePC_Username:=Me.TargetUsername_TextBox.Text,
					 _RemotePC_Password:=Me.TargetPassword_TextBox.Text,
					 _NamesOfFilesWithinThatUNCDirectory_ToBeDeleted:={Runtime.Remoting.DSCLI_EXE_FileName, Runtime.Remoting.DSCore_DLL_FileName, Me.CurrentRemotingSession_ScriptFileName}
					)

					'REM Now attempt to delete the DSRemoting\ directory

					'Dim _DSRemotingDirectory_ContentsNames$() = DocScript.Runtime.Remoting.GetContents_OfRemoteDirectory(
					'   _UNCDirectoryOnRemotePC:=New IO.DirectoryInfo("\\" & Me.TargetComputer_TextBox.Text & "\"c & Me.TargetUNCDirectory_TextBox.Text),
					'   _RemotePC_Username:=Me.TargetUsername_TextBox.Text,
					'   _RemotePC_Password:=Me.TargetPassword_TextBox.Text
					').Select(Of [String])(Function(_FSI As IO.FileSystemInfo) _FSI.Name.ToUpper()).ToArray()

					'Select Case True

					'	Case (_DSRemotingDirectory_ContentsNames.Length = 2) AndAlso _DSRemotingDirectory_ContentsNames.All(AddressOf {Runtime.Remoting.DSCLI_EXE_FileName.ToUpper(), Runtime.Remoting.DSCore_DLL_FileName.ToUpper()}.Contains)
					'		MsgBox("The remote DSRemoting Directory " & ("\\" & Me.TargetComputer_TextBox.Text & "\"c & Me.TargetUNCDirectory_TextBox.Text).InBrackets() & " could not be deleted, most likely because the remote DSCLI.exe process is still running." & vbCrLf & vbCrLf & "The DSIDE Remote-File-Explorer (Ctrl + Shift + F) could be used to manually delete it.", MsgBoxStyle.Exclamation Or MsgBoxStyle.MsgBoxSetForeground Or MsgBoxStyle.OkOnly)

					'		REM ↑ Scenario 1:
					'		REM		- Desc.:	The remote DSCLI.exe process is still running, so when we deleted the {DSCLI.exe, DSCore.dll, and *Session*.DS} files,
					'		REM					the *Session*.DS file was deleted, but {DSCLI.exe and DSCore.dll} will only have been MARKED for deletion
					'		REM					by Windows, and will only ACTUALLY BE DELETED after the DSCLI.exe process ends. Additionally, there were no other
					'		REM					files in DSRemoting\ beforehand. In this instance, we should delete the DSRemoting\ directory, but can't because the binaries
					'		REM					therein are still locked-by-handle. Therefore, tell the user this.
					'		REM		- Check:	The content of DSRemoting\ is only {DSCLI.exe and DSCore.dll}.
					'		REM		- Action:	MsgBox() that the DSRemoting\ directory could not be deleted probably owing to DSCLI still running on the remote computer.

					'	Case _DSRemotingDirectory_ContentsNames.Length = 0
					'		Me.TheActionInProgressDialog.UpdateText("Deleting DSRemoting directory...")
					'		DocScript.Runtime.Remoting.DeleteDirectory_FromRemoteComputer(
					'		 _UNCDirectoryOnRemotePC_ToDelete:=New IO.DirectoryInfo("\\" & Me.TargetComputer_TextBox.Text & "\"c & Me.TargetUNCDirectory_TextBox.Text),
					'		 _RemotePC_Username:=Me.TargetUsername_TextBox.Text,
					'		 _RemotePC_Password:=Me.TargetPassword_TextBox.Text
					'		)

					'		REM ↑ Scenario 2:
					'		REM		- Desc.:	The remote DSCLI.exe process has already finished (e.g. because the program was really short, like deleting a single file),
					'		REM					so the above DeleteFiles_FromRemoteComputer() did actually delete all of {DSCLI.exe, DSCore.dll, and *Session*.DS},
					'		REM					as none of these files were locked-by-handle. Additionally, there were no other files in DSRemoting\ beforehand. In this
					'		REM					instance, we should be able to delete DSRemoting\ now.
					'		REM		- Check:	DSRemoting\ is empty.
					'		REM		-Action:	Delete DSRemoting\.

					'	Case Else

					'		REM ↑ Scenario 3:
					'		REM		- Desc.:	The DSRemoting\ directory contains other files besides the ones used for this session, so shouldn't be deleted.
					'		REM		- Check:	(Neither of the other two cases occured).
					'		REM		- Action:	Do nothing; we have already deleted the files that were specific to this session.

					'End Select

				End If

			Else 'Simple-Mode; request Credentials first

				REM Ensure the required data have been specified
				If [String].IsNullOrEmpty(Me.SimpleMode_ComputerTextBox.Text) Then Throw New Exception("A Computer name of IP addr. must be specified")

				REM Prompt for Credentials first. Copy these into the main Username and Password Textboxes
				Me.TheActionInProgressDialog.Dismiss() 'Hide this, so that it dosen't block the EnterCredentialsDialog
				Dim _SpecifiedCredentials As Tuple(Of [String], [String]) = DocScript.EnterCredentialsDialog.Show_AndGetCredentials()
				Me.TargetUsername_TextBox.Text = _SpecifiedCredentials.[Item1] : Me.TargetPassword_TextBox.Text = _SpecifiedCredentials.[Item2]

				REM Use Interactive Mode, with the Console Session. DSR Files will not be deleted at the end.
				Me.TheActionInProgressDialog.UpdateText("Authenticating and copying Files...")
				DocScript.Runtime.Remoting.CopyFiles_ToRemoteComputer(
				 _UNCDirectoryOnRemotePC_IntoWhichFilesWillBeCopied:=(New IO.DirectoryInfo("\\" & Me.SimpleMode_ComputerTextBox.Text & "\"c & DSRemotingWindow.Default_RemoteUNCPath)),
				 _RemotePC_Username:=[_SpecifiedCredentials].Item1,
				 _RemotePC_Password:=[_SpecifiedCredentials].Item2,
				 [_FilesToCopyThither]:={
				  New IO.FileInfo(IO.Path.Combine(UsefulMethods.GetExecutingAssemblyDirectory().FullName.WithBackslashOnEnd(), Runtime.Remoting.DSCLI_EXE_FileName)),
				  New IO.FileInfo(IO.Path.Combine(UsefulMethods.GetExecutingAssemblyDirectory().FullName.WithBackslashOnEnd(), Runtime.Remoting.DSCore_DLL_FileName)),
				  New IO.FileInfo(IO.Path.Combine(UsefulMethods.GetExecutingAssemblyDirectory().FullName.WithBackslashOnEnd(), Me.CurrentRemotingSession_ScriptFileName))
				 }
				)

				Me.InteractiveMode_LaunchRemoteProcess_SimpleMode(_SpecifiedCredentials)

			End If

		Catch _DSR_Exception As Exception When True

			Try : Me.TheActionInProgressDialog.Dismiss() : Catch : End Try

			Global.Microsoft.VisualBasic.Interaction.MsgBox(
			 "The DS-Remoting operation failed. Reason: " _
			 & vbCrLf & vbCrLf & _DSR_Exception.Message _
			 & If(_DSR_Exception.Message.Contains("The network path was not found"), vbCrLf & vbCrLf & "Ensure that TCP/445 and UDP/137 are open on the target (wf.msc). Another way to open these is to enable [Network Discovery] and [File and Printer Sharing] via ""Manage Advanced Sharing Settings""." & vbCrLf & "Check that you can PING and PSEXEC into the target manually." & vbCrLf & vbCrLf & "If currently using the HostName, try using the IP-Address instead.", "") _
			 & If(_DSR_Exception.Message.Contains("The process cannot access the file"), vbCrLf & vbCrLf & "Check that another DS-Remoting session is not already in-progress on the target. You could use the [Start PsExec cmd session...] link-button to tasklist and taskkill the currently-active remote DSCLI.exe process.", "") _
			 & If(_DSR_Exception.Message.Contains("Logon failure"), vbCrLf & vbCrLf & "Ensure that the specified user account has the required permissions, and that the time is set correctly on the remote computer." & vbCrLf & "Perhaps attempt a different username syntax e.g. HOSTNAME\Username or .\Username or Username or Username@Domain." & vbCrLf & vbCrLf & "If currently using the HostName, try using the IP-Address instead.", "") _
			 & If(_DSR_Exception.Message.Contains("Could not find file"), vbCrLf & vbCrLf & "Ensure that there are copies of DSCLI.exe and DSCore.dll in the same directory as DSIDE.exe. This feature is designed to work when DocScript is installed via the MSI.", ""),
			 MsgBoxStyle.MsgBoxSetForeground Or MsgBoxStyle.Critical,
			 "DS-Remoting"
			)

		Finally

			REM Attempt to delete the tempoary local session-specific Script file
			Try : IO.File.Delete(IO.Path.Combine(UsefulMethods.GetExecutingAssemblyDirectory().FullName.WithBackslashOnEnd(), CurrentRemotingSession_ScriptFileName))
			Catch _Ex As Exception : MsgBox("The tempoary local session-specific Script file (.\" & CurrentRemotingSession_ScriptFileName & ") could not be deleted because " & _Ex.Message, MsgBoxStyle.Exclamation) : End Try

			REM Dismiss the ActionInProgressDialog
			Try : Me.TheActionInProgressDialog.Dismiss() : Catch : End Try
			Me.Enabled = True

		End Try
	End Sub

	Public Sub HeadlessMode_CreateRemoteProcess()

		'For this mode, the Advanced UI Mode must have been used.

		REM Headless Mode - Creates an unseen DSCLI.exe process, which can't Output, Input, or Play Audio
		'	Needs:
		'		- DSCLI.exe & DSCore.dll on the remote computer
		'		- (E.g.) DSRemotingSessionAK47_03042023_135503.DS on the remote computer
		'
		'	Process:
		'		- E.g. CopyFiles_ToRemoteComputer("\\192.168.1.85\C$\Windows\DSRemoting\", "User", "Pass", {"DSCLI.exe", "DSCore.dll", "...ScriptAK47.DS"})
		'		- E.g. CreateRemoteProcess("192.168.1.85", "User", "Pass", "C:\Windows\DSRemoting\DSCLI.exe /Run /SourceFile:""C:\Windows\DSRemoting\...ScriptAK47.DS""")

		'The command-to-run looks like e.g.:
		'\\192.168.1.85\C$\Windows\DSRemoting\DSCLI.exe /Run /SourceFile:"\\192.168.1.85\C$\Windows\DSRemoting\DSRemotingSessionAK47_03042023_135503.DS" /DocScriptCLAs:"Hello World 2 1"

		Me.TheActionInProgressDialog.UpdateText("Creating Remote Process...")
		DocScript.Runtime.Remoting.WMI_CreateRemoteProcess(
		 _TargetPC_HostnameOrIP:=Me.TargetComputer_TextBox.Text,
		 _Username_ForTargetPC:=Me.TargetUsername_TextBox.Text,
		 _Password_ForTargetPC:=Me.TargetPassword_TextBox.Text,
		 _CommandLineToRun_OnRemotePC:=String.Format(
		  "{0}\{1} /Run /SourceFile:""{0}\{2}""{3}",
		  (New IO.DirectoryInfo("\\" & Me.TargetComputer_TextBox.Text & "\"c & Me.TargetUNCDirectory_TextBox.Text)).FullName.WithoutBackslashOnEnd(),
		  DocScript.Runtime.Remoting.DSCLI_EXE_FileName,
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
		'		- DSCLI.exe and DSCore.dll on the remote computer
		'		- (E.g.) DSRemotingSession_03042023_135503_ScriptAK47.DS on the remote computer
		'		- PsExec.exe on the local computer
		'
		'	Process:
		'		- E.g. CopyFiles_ToRemoteComputer("\\192.168.1.85\C$\Windows\DSRemoting\", "User", "Pass", {"DSCLI.exe", "DSCore.dll", "...ScriptAK47.DS"})
		'		- E.g. "PsExec.exe" "\\192.168.1.85" -nobanner -accepteula -u Administrator -p PASS123 C:\Windows\DSRemoting\DSCLI.exe /Run /SourceFile:""C:\Windows\DSRemoting\...ScriptBC09.DS"" /DocScriptCLAs:"..."

		Dim _PsExec_CLAs$ = [String].Format("""\\{0}"" -nobanner -accepteula -u ""{1}"" -p ""{2}"" ""{3}\{4}"" /Run /SourceFile:""{3}\{5}""{6}",
		 Me.TargetComputer_TextBox.Text,
		 Me.TargetUsername_TextBox.Text,
		 Me.TargetPassword_TextBox.Text,
		 (New IO.DirectoryInfo("\\" & Me.TargetComputer_TextBox.Text & "\"c & Me.TargetUNCDirectory_TextBox.Text)).FullName.WithoutBackslashOnEnd(),
		 DocScript.Runtime.Remoting.DSCLI_EXE_FileName,
		 Me.CurrentRemotingSession_ScriptFileName,
		 If(String.IsNullOrEmpty(Me.DocScriptCLAs_TextBox.Text), "", " /DocScriptCLAs:""" & Me.DocScriptCLAs_TextBox.Text & """"c)
		)

		REM If the specified PsExec exe isn't found, then ask for one.
		Dim _PsExec_ExePath$ = Me.ShellMode_PsExecPath_TextBox.Text.Replace(DSRemotingWindow.PlaceholderFor_DSIDE_Dir, UsefulMethods.GetExecutingAssemblyDirectory.FullName.WithoutBackslashOnEnd())
		If Not IO.File.Exists(_PsExec_ExePath$) Then _PsExec_ExePath = InputBox(_PsExec_ExePath & " does not exist." & vbCrLf & vbCrLf & "Enter a path to ""PsExec.exe""...")
		If Not IO.File.Exists(_PsExec_ExePath$) Then Throw New IO.FileNotFoundException("The PsExec.exe process path could not be resolved.", _PsExec_ExePath)

		REM Identify a Command Processor to use (e.g. cmd.exe or command.com)
		Dim _CommandProcessorExecutable_Path$ = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Windows), "System32", "cmd.exe")
		If Not IO.File.Exists(_CommandProcessorExecutable_Path$) Then _CommandProcessorExecutable_Path = InputBox(_CommandProcessorExecutable_Path & " does not exist." & vbCrLf & vbCrLf & "Enter a path to ""cmd.exe"" or another command processor executable supporting the same command-line arguments...")
		If Not IO.File.Exists(_CommandProcessorExecutable_Path$) Then Throw New IO.FileNotFoundException("The command processor executable path could not be resolved.", _CommandProcessorExecutable_Path)

		Me.TheActionInProgressDialog.UpdateText("Hosting DS-Remoting Shell Session...")
		Dim _PsExec_ViaCmdExe_Process As New System.Diagnostics.Process() With {
		 .StartInfo = (New ProcessStartInfo() With {
		  .CreateNoWindow = False,
		  .WindowStyle = ProcessWindowStyle.Normal,
		  .FileName = _CommandProcessorExecutable_Path,
		  .Arguments = String.Format("/K Title DS-Remoting Shell Session && Prompt The DS-Remoting session has finished.$_Type ""exit"" and press [Enter] to close...$_DSRemoting$G && ""{0}"" {1}", _PsExec_ExePath, _PsExec_CLAs)
		 })
		}
		_PsExec_ViaCmdExe_Process.Start() : _PsExec_ViaCmdExe_Process.WaitForExit()

		'Use a new thread for the MsgBox, so that the remaining DS-Remoting operations continue (e.g. deleting the files from the remote host)
		Dim _MsgBoxThread As New System.Threading.Thread(Sub() MsgBox("The Shell Session to " & Me.TargetComputer_TextBox.Text & " ended with exit-code " & _PsExec_ViaCmdExe_Process.ExitCode.ToString(), MsgBoxStyle.Information, "DS-Remoting"))
		_MsgBoxThread.Start()

	End Sub

	Public Sub InteractiveMode_LaunchRemoteProcess_AdvancedMode()

		'For this mode, EITHER the Simple, or Advanced UI Mode could have been used.
		'This Sub handles Advanced mode, using data from the form, and the below Sub handles Simple mode, using Consts

		REM Interactive Mode - Starts a DSCLI.exe process on a specified Session of the remote computer; Supports GUI execution
		'	Needs:
		'		- DSCLI.exe and DSCore.dll on the remote computer
		'		- (E.g.) DSRemotingSession_03042023_135503_ScriptAK47.DS on the remote computer
		'		- PsExec.exe on the local computer
		'
		'	Features:
		'		- Can choose the session on the target computer
		'		- Can choose to execute in /GUI mode
		'
		'	Process:
		'		- E.g. CopyFiles_ToRemoteComputer("\\192.168.1.85\C$\Windows\DSRemoting\", "User", "Pass", {"DSCLI.exe", "...ScriptAK47.DS"})
		'		- E.g. PsExec.exe \\192.168.1.85 -nobanner -accepteula -i -d -u Administrator -p PASS123 C:\Windows\DSRemoting\DSCLI.exe /Run /GUI /HideConsoleBeforeExecution /SourceFile:""C:\Windows\DSRemoting\...ScriptBC09.DS"" /DocScriptCLAs:"..."

		Dim _PsExec_CLAs$ = [String].Format("""\\{0}"" -nobanner -accepteula -i{3} -d -u ""{1}"" -p ""{2}"" ""{4}\{5}"" /Run{6}{8} /SourceFile:""{4}\{7}""{9}",
		 Me.TargetComputer_TextBox.Text,
		 Me.TargetUsername_TextBox.Text,
		 Me.TargetPassword_TextBox.Text,
		 If(Me.InteractiveMode_UseCustomSession_RadioButton.Checked, " "c & Me.InteractiveMode_CustomSessionNumber_NumericUpDown.Value.ToString().ExtractUInt32().ToString(), ""),
		 (New IO.DirectoryInfo("\\" & Me.TargetComputer_TextBox.Text & "\"c & Me.TargetUNCDirectory_TextBox.Text)).FullName.WithoutBackslashOnEnd(),
		 DocScript.Runtime.Remoting.DSCLI_EXE_FileName,
		 If(Me.InteractiveMode_UseGUIExecution_CheckBox.Checked, " /GUI", ""),
		 Me.CurrentRemotingSession_ScriptFileName,
		 If(Me.InteractiveMode_HideConsoleBeforeExecution_CheckBox.Checked, " /HideConsoleBeforeExecution", ""),
		 If(String.IsNullOrEmpty(Me.DocScriptCLAs_TextBox.Text), "", " /DocScriptCLAs:""" & Me.DocScriptCLAs_TextBox.Text & """"c)
		)

		REM If the specified PsExec exe isn't found, then ask for one.
		Dim _PsExec_ExePath$ = Me.InteractiveMode_PsExecPath_TextBox.Text.Replace(DSRemotingWindow.PlaceholderFor_DSIDE_Dir, UsefulMethods.GetExecutingAssemblyDirectory.FullName.WithoutBackslashOnEnd())
		If Not IO.File.Exists(_PsExec_ExePath$) Then _PsExec_ExePath = InputBox(_PsExec_ExePath & " does not exist." & vbCrLf & vbCrLf & "Enter a path to ""PsExec.exe""...")
		If Not IO.File.Exists(_PsExec_ExePath$) Then Throw New IO.FileNotFoundException("The PsExec.exe process path could not be resolved.", _PsExec_ExePath)

		Me.TheActionInProgressDialog.UpdateText("Launching Interactive Remote Process...")
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

	Public Sub InteractiveMode_LaunchRemoteProcess_SimpleMode(ByVal _SpecifiedCredentials As Tuple(Of [String], [String]))

		Dim _PsExec_CLAs$ = [String].Format("""\\{0}"" -nobanner -accepteula -i -d -u ""{1}"" -p ""{2}"" ""{3}\{4}"" /Run /GUI /HideConsoleBeforeExecution /SourceFile:""{3}\{5}""",
		  Me.SimpleMode_ComputerTextBox.Text,
		  _SpecifiedCredentials.Item1,
		  _SpecifiedCredentials.Item2,
		  (New IO.DirectoryInfo("\\" & Me.SimpleMode_ComputerTextBox.Text & "\"c & DSRemotingWindow.Default_RemoteUNCPath)).FullName.WithoutBackslashOnEnd(),
		  DocScript.Runtime.Remoting.DSCLI_EXE_FileName,
		  Me.CurrentRemotingSession_ScriptFileName
		)

		REM If the Default PsExec exe isn't found, then ask for one.
		Dim _PsExec_ExePath$ = DSRemotingWindow.Default_PsExecPath.Replace(DSRemotingWindow.PlaceholderFor_DSIDE_Dir, UsefulMethods.GetExecutingAssemblyDirectory.FullName.WithoutBackslashOnEnd())
		If Not IO.File.Exists(_PsExec_ExePath$) Then _PsExec_ExePath = InputBox(_PsExec_ExePath & " does not exist." & vbCrLf & vbCrLf & "Enter a path to ""PsExec.exe""...")
		If Not IO.File.Exists(_PsExec_ExePath$) Then Throw New IO.FileNotFoundException("The PsExec.exe process path could not be resolved.", _PsExec_ExePath)

		Me.TheActionInProgressDialog.UpdateText("Launching Interactive Remote Process...")
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

#End Region

End Class