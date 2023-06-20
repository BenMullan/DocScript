<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DSRemotingWindow : Inherits System.Windows.Forms.Form

	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> _
	Protected Overrides Sub Dispose(ByVal disposing As Boolean)
		Try
			If disposing AndAlso components IsNot Nothing Then
				components.Dispose()
			End If
		Finally
			MyBase.Dispose(disposing)
		End Try
	End Sub

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer

	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.  
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DSRemotingWindow))
		Me.TheTabControl = New System.Windows.Forms.TabControl()
		Me.Mode_TabPage = New System.Windows.Forms.TabPage()
		Me.Utility_LaunchNewRFE_LinkLabel = New System.Windows.Forms.LinkLabel()
		Me.InteractiveIcon_PictureBox = New System.Windows.Forms.PictureBox()
		Me.Utility_StartRemoteShellSession_LinkLabel = New System.Windows.Forms.LinkLabel()
		Me.ShellIcon_PictureBox = New System.Windows.Forms.PictureBox()
		Me.HeadlessIcon_PictureBox = New System.Windows.Forms.PictureBox()
		Me.InteractiveMode_RadioButton = New System.Windows.Forms.RadioButton()
		Me.ShellMode_RadioButton = New System.Windows.Forms.RadioButton()
		Me.HeadlessMode_RadioButton = New System.Windows.Forms.RadioButton()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.HeadlessDescriptionLabel = New System.Windows.Forms.Label()
		Me.SelectModeLabel = New System.Windows.Forms.Label()
		Me.Target_TabPage = New System.Windows.Forms.TabPage()
		Me.GroupBox1 = New System.Windows.Forms.GroupBox()
		Me.DeleteDSFilesFromRemoteAtEnd_CheckBox = New System.Windows.Forms.CheckBox()
		Me.PictureBox5 = New System.Windows.Forms.PictureBox()
		Me.DocScriptCLAs_TextBox = New System.Windows.Forms.TextBox()
		Me.Label8 = New System.Windows.Forms.Label()
		Me.Label7 = New System.Windows.Forms.Label()
		Me.TargetUNCDirectory_TextBox = New System.Windows.Forms.TextBox()
		Me.HostGroupBox = New System.Windows.Forms.GroupBox()
		Me.PictureBox4 = New System.Windows.Forms.PictureBox()
		Me.TargetPassword_TextBox = New System.Windows.Forms.TextBox()
		Me.Label5 = New System.Windows.Forms.Label()
		Me.TargetUsername_TextBox = New System.Windows.Forms.TextBox()
		Me.Label4 = New System.Windows.Forms.Label()
		Me.TargetComputer_TextBox = New System.Windows.Forms.TextBox()
		Me.Label3 = New System.Windows.Forms.Label()
		Me.Shell_TabPage = New System.Windows.Forms.TabPage()
		Me.GroupBox3 = New System.Windows.Forms.GroupBox()
		Me.Label9 = New System.Windows.Forms.Label()
		Me.ShellMode_PsExecPath_TextBox = New System.Windows.Forms.TextBox()
		Me.ShellMode_BrowseForPsExecButton = New System.Windows.Forms.Button()
		Me.PictureBox8 = New System.Windows.Forms.PictureBox()
		Me.Interactive_TabPage = New System.Windows.Forms.TabPage()
		Me.GroupBox4 = New System.Windows.Forms.GroupBox()
		Me.InteractiveMode_CustomSessionNumber_NumericUpDown = New System.Windows.Forms.NumericUpDown()
		Me.InteractiveMode_UseCustomSession_RadioButton = New System.Windows.Forms.RadioButton()
		Me.InteractiveMode_UseConsoleSession_RadioButton = New System.Windows.Forms.RadioButton()
		Me.Label6 = New System.Windows.Forms.Label()
		Me.PictureBox6 = New System.Windows.Forms.PictureBox()
		Me.InteractiveMode_HideConsoleBeforeExecution_CheckBox = New System.Windows.Forms.CheckBox()
		Me.InteractiveMode_UseGUIExecution_CheckBox = New System.Windows.Forms.CheckBox()
		Me.GroupBox2 = New System.Windows.Forms.GroupBox()
		Me.Label10 = New System.Windows.Forms.Label()
		Me.InteractiveMode_PsExecPath_TextBox = New System.Windows.Forms.TextBox()
		Me.InteractiveMode_BrowseForPsExecButton = New System.Windows.Forms.Button()
		Me.PictureBox7 = New System.Windows.Forms.PictureBox()
		Me.ConnectButton = New System.Windows.Forms.Button()
		Me.ShowHideOptionsButton = New System.Windows.Forms.Button()
		Me.DSRemotingBanner_PictureBox = New System.Windows.Forms.PictureBox()
		Me.TheToolTipProvider = New System.Windows.Forms.ToolTip(Me.components)
		Me.SimpleMode_ComputerTextBox = New System.Windows.Forms.TextBox()
		Me.SimpleMode_ComputerLabel = New System.Windows.Forms.Label()
		Me.SimpleMode_InfoLabel = New System.Windows.Forms.Label()
		Me.TheTabControl.SuspendLayout()
		Me.Mode_TabPage.SuspendLayout()
		CType(Me.InteractiveIcon_PictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.ShellIcon_PictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.HeadlessIcon_PictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.Target_TabPage.SuspendLayout()
		Me.GroupBox1.SuspendLayout()
		CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.HostGroupBox.SuspendLayout()
		CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.Shell_TabPage.SuspendLayout()
		Me.GroupBox3.SuspendLayout()
		CType(Me.PictureBox8, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.Interactive_TabPage.SuspendLayout()
		Me.GroupBox4.SuspendLayout()
		CType(Me.InteractiveMode_CustomSessionNumber_NumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupBox2.SuspendLayout()
		CType(Me.PictureBox7, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.DSRemotingBanner_PictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'TheTabControl
		'
		Me.TheTabControl.Controls.Add(Me.Mode_TabPage)
		Me.TheTabControl.Controls.Add(Me.Target_TabPage)
		Me.TheTabControl.Controls.Add(Me.Shell_TabPage)
		Me.TheTabControl.Controls.Add(Me.Interactive_TabPage)
		Me.TheTabControl.Location = New System.Drawing.Point(12, 90)
		Me.TheTabControl.Name = "TheTabControl"
		Me.TheTabControl.SelectedIndex = 0
		Me.TheTabControl.Size = New System.Drawing.Size(380, 291)
		Me.TheTabControl.TabIndex = 0
		'
		'Mode_TabPage
		'
		Me.Mode_TabPage.Controls.Add(Me.Utility_LaunchNewRFE_LinkLabel)
		Me.Mode_TabPage.Controls.Add(Me.InteractiveIcon_PictureBox)
		Me.Mode_TabPage.Controls.Add(Me.Utility_StartRemoteShellSession_LinkLabel)
		Me.Mode_TabPage.Controls.Add(Me.ShellIcon_PictureBox)
		Me.Mode_TabPage.Controls.Add(Me.HeadlessIcon_PictureBox)
		Me.Mode_TabPage.Controls.Add(Me.InteractiveMode_RadioButton)
		Me.Mode_TabPage.Controls.Add(Me.ShellMode_RadioButton)
		Me.Mode_TabPage.Controls.Add(Me.HeadlessMode_RadioButton)
		Me.Mode_TabPage.Controls.Add(Me.Label2)
		Me.Mode_TabPage.Controls.Add(Me.Label1)
		Me.Mode_TabPage.Controls.Add(Me.HeadlessDescriptionLabel)
		Me.Mode_TabPage.Controls.Add(Me.SelectModeLabel)
		Me.Mode_TabPage.Location = New System.Drawing.Point(4, 22)
		Me.Mode_TabPage.Name = "Mode_TabPage"
		Me.Mode_TabPage.Padding = New System.Windows.Forms.Padding(3)
		Me.Mode_TabPage.Size = New System.Drawing.Size(372, 265)
		Me.Mode_TabPage.TabIndex = 0
		Me.Mode_TabPage.Text = "Mode"
		Me.Mode_TabPage.UseVisualStyleBackColor = True
		'
		'Utility_LaunchNewRFE_LinkLabel
		'
		Me.Utility_LaunchNewRFE_LinkLabel.AutoSize = True
		Me.Utility_LaunchNewRFE_LinkLabel.BackColor = System.Drawing.SystemColors.ControlLightLight
		Me.Utility_LaunchNewRFE_LinkLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Utility_LaunchNewRFE_LinkLabel.Location = New System.Drawing.Point(226, 247)
		Me.Utility_LaunchNewRFE_LinkLabel.Name = "Utility_LaunchNewRFE_LinkLabel"
		Me.Utility_LaunchNewRFE_LinkLabel.Size = New System.Drawing.Size(143, 13)
		Me.Utility_LaunchNewRFE_LinkLabel.TabIndex = 7
		Me.Utility_LaunchNewRFE_LinkLabel.TabStop = True
		Me.Utility_LaunchNewRFE_LinkLabel.Text = "Launch remote file explorer..."
		Me.TheToolTipProvider.SetToolTip(Me.Utility_LaunchNewRFE_LinkLabel, "Launches an new DocScript Remote File Explorer, targeting the specified" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Computer" & _
			  "'s C$\ share, using the specified Username and Password." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "This is done on a ne" & _
			  "w Thread.")
		'
		'InteractiveIcon_PictureBox
		'
		Me.InteractiveIcon_PictureBox.Image = Global.DocScript.My.Resources.Resources.DSR_InteractiveMode
		Me.InteractiveIcon_PictureBox.Location = New System.Drawing.Point(52, 189)
		Me.InteractiveIcon_PictureBox.Name = "InteractiveIcon_PictureBox"
		Me.InteractiveIcon_PictureBox.Size = New System.Drawing.Size(32, 31)
		Me.InteractiveIcon_PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
		Me.InteractiveIcon_PictureBox.TabIndex = 2
		Me.InteractiveIcon_PictureBox.TabStop = False
		'
		'Utility_StartRemoteShellSession_LinkLabel
		'
		Me.Utility_StartRemoteShellSession_LinkLabel.AutoSize = True
		Me.Utility_StartRemoteShellSession_LinkLabel.BackColor = System.Drawing.SystemColors.ControlLightLight
		Me.Utility_StartRemoteShellSession_LinkLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Utility_StartRemoteShellSession_LinkLabel.Location = New System.Drawing.Point(4, 247)
		Me.Utility_StartRemoteShellSession_LinkLabel.Name = "Utility_StartRemoteShellSession_LinkLabel"
		Me.Utility_StartRemoteShellSession_LinkLabel.Size = New System.Drawing.Size(138, 13)
		Me.Utility_StartRemoteShellSession_LinkLabel.TabIndex = 6
		Me.Utility_StartRemoteShellSession_LinkLabel.TabStop = True
		Me.Utility_StartRemoteShellSession_LinkLabel.Text = "Start PsExec cmd session..."
		Me.TheToolTipProvider.SetToolTip(Me.Utility_StartRemoteShellSession_LinkLabel, "Launches a PsExec cmd.exe Session, using the currently-entered Computer, Username" & _
			  ", Password, and PsExec path.")
		'
		'ShellIcon_PictureBox
		'
		Me.ShellIcon_PictureBox.Image = Global.DocScript.My.Resources.Resources.DSR_ShellMode
		Me.ShellIcon_PictureBox.Location = New System.Drawing.Point(52, 120)
		Me.ShellIcon_PictureBox.Name = "ShellIcon_PictureBox"
		Me.ShellIcon_PictureBox.Size = New System.Drawing.Size(32, 31)
		Me.ShellIcon_PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
		Me.ShellIcon_PictureBox.TabIndex = 2
		Me.ShellIcon_PictureBox.TabStop = False
		'
		'HeadlessIcon_PictureBox
		'
		Me.HeadlessIcon_PictureBox.Image = Global.DocScript.My.Resources.Resources.DSR_HeadlessMode
		Me.HeadlessIcon_PictureBox.Location = New System.Drawing.Point(52, 48)
		Me.HeadlessIcon_PictureBox.Name = "HeadlessIcon_PictureBox"
		Me.HeadlessIcon_PictureBox.Size = New System.Drawing.Size(32, 31)
		Me.HeadlessIcon_PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
		Me.HeadlessIcon_PictureBox.TabIndex = 2
		Me.HeadlessIcon_PictureBox.TabStop = False
		'
		'InteractiveMode_RadioButton
		'
		Me.InteractiveMode_RadioButton.AutoSize = True
		Me.InteractiveMode_RadioButton.Checked = True
		Me.InteractiveMode_RadioButton.Location = New System.Drawing.Point(26, 172)
		Me.InteractiveMode_RadioButton.Name = "InteractiveMode_RadioButton"
		Me.InteractiveMode_RadioButton.Size = New System.Drawing.Size(75, 17)
		Me.InteractiveMode_RadioButton.TabIndex = 5
		Me.InteractiveMode_RadioButton.TabStop = True
		Me.InteractiveMode_RadioButton.Text = "&Interactive"
		Me.InteractiveMode_RadioButton.UseVisualStyleBackColor = True
		'
		'ShellMode_RadioButton
		'
		Me.ShellMode_RadioButton.AutoSize = True
		Me.ShellMode_RadioButton.Location = New System.Drawing.Point(26, 103)
		Me.ShellMode_RadioButton.Name = "ShellMode_RadioButton"
		Me.ShellMode_RadioButton.Size = New System.Drawing.Size(48, 17)
		Me.ShellMode_RadioButton.TabIndex = 3
		Me.ShellMode_RadioButton.Text = "&Shell"
		Me.ShellMode_RadioButton.UseVisualStyleBackColor = True
		'
		'HeadlessMode_RadioButton
		'
		Me.HeadlessMode_RadioButton.AutoSize = True
		Me.HeadlessMode_RadioButton.Location = New System.Drawing.Point(26, 31)
		Me.HeadlessMode_RadioButton.Name = "HeadlessMode_RadioButton"
		Me.HeadlessMode_RadioButton.Size = New System.Drawing.Size(69, 17)
		Me.HeadlessMode_RadioButton.TabIndex = 1
		Me.HeadlessMode_RadioButton.Text = "&Headless"
		Me.HeadlessMode_RadioButton.UseVisualStyleBackColor = True
		'
		'Label2
		'
		Me.Label2.AutoSize = True
		Me.Label2.ForeColor = System.Drawing.SystemColors.GrayText
		Me.Label2.Location = New System.Drawing.Point(92, 174)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(253, 39)
		Me.Label2.TabIndex = 6
		Me.Label2.Text = "  Starts an Interpreter process on a specified session" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "of the remote computer. S" & _
		  "upports GUI execution." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Uses PsExec."
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.ForeColor = System.Drawing.SystemColors.GrayText
		Me.Label1.Location = New System.Drawing.Point(92, 105)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(239, 39)
		Me.Label1.TabIndex = 4
		Me.Label1.Text = "Starts a remote DSCLI.exe process, and handles" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "command-line interaction to it, f" & _
		  "rom this computer." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Uses PsExec."
		'
		'HeadlessDescriptionLabel
		'
		Me.HeadlessDescriptionLabel.AutoSize = True
		Me.HeadlessDescriptionLabel.ForeColor = System.Drawing.SystemColors.GrayText
		Me.HeadlessDescriptionLabel.Location = New System.Drawing.Point(92, 33)
		Me.HeadlessDescriptionLabel.Name = "HeadlessDescriptionLabel"
		Me.HeadlessDescriptionLabel.Size = New System.Drawing.Size(266, 52)
		Me.HeadlessDescriptionLabel.TabIndex = 2
		Me.HeadlessDescriptionLabel.Text = resources.GetString("HeadlessDescriptionLabel.Text")
		'
		'SelectModeLabel
		'
		Me.SelectModeLabel.AutoSize = True
		Me.SelectModeLabel.Location = New System.Drawing.Point(4, 6)
		Me.SelectModeLabel.Name = "SelectModeLabel"
		Me.SelectModeLabel.Size = New System.Drawing.Size(151, 13)
		Me.SelectModeLabel.TabIndex = 0
		Me.SelectModeLabel.Text = "Select a DS-Remoting Mode..."
		'
		'Target_TabPage
		'
		Me.Target_TabPage.Controls.Add(Me.GroupBox1)
		Me.Target_TabPage.Controls.Add(Me.HostGroupBox)
		Me.Target_TabPage.Location = New System.Drawing.Point(4, 22)
		Me.Target_TabPage.Name = "Target_TabPage"
		Me.Target_TabPage.Padding = New System.Windows.Forms.Padding(3)
		Me.Target_TabPage.Size = New System.Drawing.Size(372, 265)
		Me.Target_TabPage.TabIndex = 1
		Me.Target_TabPage.Text = "Target"
		Me.Target_TabPage.UseVisualStyleBackColor = True
		'
		'GroupBox1
		'
		Me.GroupBox1.Controls.Add(Me.DeleteDSFilesFromRemoteAtEnd_CheckBox)
		Me.GroupBox1.Controls.Add(Me.PictureBox5)
		Me.GroupBox1.Controls.Add(Me.DocScriptCLAs_TextBox)
		Me.GroupBox1.Controls.Add(Me.Label8)
		Me.GroupBox1.Controls.Add(Me.Label7)
		Me.GroupBox1.Controls.Add(Me.TargetUNCDirectory_TextBox)
		Me.GroupBox1.Location = New System.Drawing.Point(6, 144)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(360, 115)
		Me.GroupBox1.TabIndex = 1
		Me.GroupBox1.TabStop = False
		Me.GroupBox1.Text = "Options"
		'
		'DeleteDSFilesFromRemoteAtEnd_CheckBox
		'
		Me.DeleteDSFilesFromRemoteAtEnd_CheckBox.AutoSize = True
		Me.DeleteDSFilesFromRemoteAtEnd_CheckBox.Location = New System.Drawing.Point(60, 81)
		Me.DeleteDSFilesFromRemoteAtEnd_CheckBox.Name = "DeleteDSFilesFromRemoteAtEnd_CheckBox"
		Me.DeleteDSFilesFromRemoteAtEnd_CheckBox.Size = New System.Drawing.Size(266, 17)
		Me.DeleteDSFilesFromRemoteAtEnd_CheckBox.TabIndex = 4
		Me.DeleteDSFilesFromRemoteAtEnd_CheckBox.Text = "Delete DocScript files from remote computer at end"
		Me.DeleteDSFilesFromRemoteAtEnd_CheckBox.UseVisualStyleBackColor = True
		'
		'PictureBox5
		'
		Me.PictureBox5.Image = Global.DocScript.My.Resources.Resources.TargetOptions
		Me.PictureBox5.Location = New System.Drawing.Point(8, 13)
		Me.PictureBox5.Name = "PictureBox5"
		Me.PictureBox5.Size = New System.Drawing.Size(40, 35)
		Me.PictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
		Me.PictureBox5.TabIndex = 2
		Me.PictureBox5.TabStop = False
		'
		'DocScriptCLAs_TextBox
		'
		Me.DocScriptCLAs_TextBox.Location = New System.Drawing.Point(145, 22)
		Me.DocScriptCLAs_TextBox.Name = "DocScriptCLAs_TextBox"
		Me.DocScriptCLAs_TextBox.Size = New System.Drawing.Size(195, 20)
		Me.DocScriptCLAs_TextBox.TabIndex = 1
		'
		'Label8
		'
		Me.Label8.AutoSize = True
		Me.Label8.Location = New System.Drawing.Point(54, 56)
		Me.Label8.Name = "Label8"
		Me.Label8.Size = New System.Drawing.Size(78, 13)
		Me.Label8.TabIndex = 2
		Me.Label8.Text = "UNC Directory:"
		'
		'Label7
		'
		Me.Label7.AutoSize = True
		Me.Label7.Location = New System.Drawing.Point(54, 25)
		Me.Label7.Name = "Label7"
		Me.Label7.Size = New System.Drawing.Size(85, 13)
		Me.Label7.TabIndex = 0
		Me.Label7.Text = "DocScript CLAs:"
		'
		'TargetUNCDirectory_TextBox
		'
		Me.TargetUNCDirectory_TextBox.Location = New System.Drawing.Point(145, 53)
		Me.TargetUNCDirectory_TextBox.Name = "TargetUNCDirectory_TextBox"
		Me.TargetUNCDirectory_TextBox.Size = New System.Drawing.Size(195, 20)
		Me.TargetUNCDirectory_TextBox.TabIndex = 3
		Me.TargetUNCDirectory_TextBox.Text = "{Set at Runtime}"
		'
		'HostGroupBox
		'
		Me.HostGroupBox.Controls.Add(Me.PictureBox4)
		Me.HostGroupBox.Controls.Add(Me.TargetPassword_TextBox)
		Me.HostGroupBox.Controls.Add(Me.Label5)
		Me.HostGroupBox.Controls.Add(Me.TargetUsername_TextBox)
		Me.HostGroupBox.Controls.Add(Me.Label4)
		Me.HostGroupBox.Controls.Add(Me.TargetComputer_TextBox)
		Me.HostGroupBox.Controls.Add(Me.Label3)
		Me.HostGroupBox.Location = New System.Drawing.Point(6, 6)
		Me.HostGroupBox.Name = "HostGroupBox"
		Me.HostGroupBox.Size = New System.Drawing.Size(360, 132)
		Me.HostGroupBox.TabIndex = 0
		Me.HostGroupBox.TabStop = False
		Me.HostGroupBox.Text = "Host"
		'
		'PictureBox4
		'
		Me.PictureBox4.Image = Global.DocScript.My.Resources.Resources.Computer
		Me.PictureBox4.Location = New System.Drawing.Point(8, 13)
		Me.PictureBox4.Name = "PictureBox4"
		Me.PictureBox4.Size = New System.Drawing.Size(40, 35)
		Me.PictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
		Me.PictureBox4.TabIndex = 2
		Me.PictureBox4.TabStop = False
		'
		'TargetPassword_TextBox
		'
		Me.TargetPassword_TextBox.Location = New System.Drawing.Point(127, 94)
		Me.TargetPassword_TextBox.Name = "TargetPassword_TextBox"
		Me.TargetPassword_TextBox.Size = New System.Drawing.Size(213, 20)
		Me.TargetPassword_TextBox.TabIndex = 5
		Me.TargetPassword_TextBox.UseSystemPasswordChar = True
		'
		'Label5
		'
		Me.Label5.AutoSize = True
		Me.Label5.Location = New System.Drawing.Point(54, 97)
		Me.Label5.Name = "Label5"
		Me.Label5.Size = New System.Drawing.Size(56, 13)
		Me.Label5.TabIndex = 4
		Me.Label5.Text = "Password:"
		'
		'TargetUsername_TextBox
		'
		Me.TargetUsername_TextBox.Location = New System.Drawing.Point(127, 59)
		Me.TargetUsername_TextBox.Name = "TargetUsername_TextBox"
		Me.TargetUsername_TextBox.Size = New System.Drawing.Size(213, 20)
		Me.TargetUsername_TextBox.TabIndex = 3
		'
		'Label4
		'
		Me.Label4.AutoSize = True
		Me.Label4.Location = New System.Drawing.Point(54, 62)
		Me.Label4.Name = "Label4"
		Me.Label4.Size = New System.Drawing.Size(58, 13)
		Me.Label4.TabIndex = 2
		Me.Label4.Text = "Username:"
		'
		'TargetComputer_TextBox
		'
		Me.TargetComputer_TextBox.Location = New System.Drawing.Point(127, 24)
		Me.TargetComputer_TextBox.Name = "TargetComputer_TextBox"
		Me.TargetComputer_TextBox.Size = New System.Drawing.Size(213, 20)
		Me.TargetComputer_TextBox.TabIndex = 1
		'
		'Label3
		'
		Me.Label3.AutoSize = True
		Me.Label3.Location = New System.Drawing.Point(54, 27)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(55, 13)
		Me.Label3.TabIndex = 0
		Me.Label3.Text = "Computer:"
		'
		'Shell_TabPage
		'
		Me.Shell_TabPage.Controls.Add(Me.GroupBox3)
		Me.Shell_TabPage.Location = New System.Drawing.Point(4, 22)
		Me.Shell_TabPage.Name = "Shell_TabPage"
		Me.Shell_TabPage.Padding = New System.Windows.Forms.Padding(3)
		Me.Shell_TabPage.Size = New System.Drawing.Size(372, 265)
		Me.Shell_TabPage.TabIndex = 2
		Me.Shell_TabPage.Text = "Shell Mode Options"
		Me.Shell_TabPage.UseVisualStyleBackColor = True
		'
		'GroupBox3
		'
		Me.GroupBox3.Controls.Add(Me.Label9)
		Me.GroupBox3.Controls.Add(Me.ShellMode_PsExecPath_TextBox)
		Me.GroupBox3.Controls.Add(Me.ShellMode_BrowseForPsExecButton)
		Me.GroupBox3.Controls.Add(Me.PictureBox8)
		Me.GroupBox3.Location = New System.Drawing.Point(6, 6)
		Me.GroupBox3.Name = "GroupBox3"
		Me.GroupBox3.Size = New System.Drawing.Size(360, 78)
		Me.GroupBox3.TabIndex = 0
		Me.GroupBox3.TabStop = False
		Me.GroupBox3.Text = "PsExec"
		'
		'Label9
		'
		Me.Label9.AutoSize = True
		Me.Label9.Location = New System.Drawing.Point(57, 23)
		Me.Label9.Name = "Label9"
		Me.Label9.Size = New System.Drawing.Size(70, 13)
		Me.Label9.TabIndex = 4
		Me.Label9.Text = "PsExec path:"
		'
		'ShellMode_PsExecPath_TextBox
		'
		Me.ShellMode_PsExecPath_TextBox.Location = New System.Drawing.Point(57, 43)
		Me.ShellMode_PsExecPath_TextBox.Name = "ShellMode_PsExecPath_TextBox"
		Me.ShellMode_PsExecPath_TextBox.Size = New System.Drawing.Size(266, 20)
		Me.ShellMode_PsExecPath_TextBox.TabIndex = 1
		Me.ShellMode_PsExecPath_TextBox.Text = "{Set at Runtime}"
		'
		'ShellMode_BrowseForPsExecButton
		'
		Me.ShellMode_BrowseForPsExecButton.Location = New System.Drawing.Point(329, 41)
		Me.ShellMode_BrowseForPsExecButton.Name = "ShellMode_BrowseForPsExecButton"
		Me.ShellMode_BrowseForPsExecButton.Size = New System.Drawing.Size(25, 23)
		Me.ShellMode_BrowseForPsExecButton.TabIndex = 2
		Me.ShellMode_BrowseForPsExecButton.Text = "..."
		Me.ShellMode_BrowseForPsExecButton.UseVisualStyleBackColor = True
		'
		'PictureBox8
		'
		Me.PictureBox8.Image = Global.DocScript.My.Resources.Resources.PsExec_NotActuallyTheLogoForItButStill
		Me.PictureBox8.Location = New System.Drawing.Point(10, 19)
		Me.PictureBox8.Name = "PictureBox8"
		Me.PictureBox8.Size = New System.Drawing.Size(40, 35)
		Me.PictureBox8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
		Me.PictureBox8.TabIndex = 3
		Me.PictureBox8.TabStop = False
		'
		'Interactive_TabPage
		'
		Me.Interactive_TabPage.Controls.Add(Me.GroupBox4)
		Me.Interactive_TabPage.Controls.Add(Me.GroupBox2)
		Me.Interactive_TabPage.Location = New System.Drawing.Point(4, 22)
		Me.Interactive_TabPage.Name = "Interactive_TabPage"
		Me.Interactive_TabPage.Padding = New System.Windows.Forms.Padding(3)
		Me.Interactive_TabPage.Size = New System.Drawing.Size(372, 265)
		Me.Interactive_TabPage.TabIndex = 3
		Me.Interactive_TabPage.Text = "Interactive Mode Options"
		Me.Interactive_TabPage.UseVisualStyleBackColor = True
		'
		'GroupBox4
		'
		Me.GroupBox4.Controls.Add(Me.InteractiveMode_CustomSessionNumber_NumericUpDown)
		Me.GroupBox4.Controls.Add(Me.InteractiveMode_UseCustomSession_RadioButton)
		Me.GroupBox4.Controls.Add(Me.InteractiveMode_UseConsoleSession_RadioButton)
		Me.GroupBox4.Controls.Add(Me.Label6)
		Me.GroupBox4.Controls.Add(Me.PictureBox6)
		Me.GroupBox4.Controls.Add(Me.InteractiveMode_HideConsoleBeforeExecution_CheckBox)
		Me.GroupBox4.Controls.Add(Me.InteractiveMode_UseGUIExecution_CheckBox)
		Me.GroupBox4.Location = New System.Drawing.Point(6, 90)
		Me.GroupBox4.Name = "GroupBox4"
		Me.GroupBox4.Size = New System.Drawing.Size(360, 169)
		Me.GroupBox4.TabIndex = 1
		Me.GroupBox4.TabStop = False
		Me.GroupBox4.Text = "Options"
		'
		'InteractiveMode_CustomSessionNumber_NumericUpDown
		'
		Me.InteractiveMode_CustomSessionNumber_NumericUpDown.Location = New System.Drawing.Point(164, 118)
		Me.InteractiveMode_CustomSessionNumber_NumericUpDown.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
		Me.InteractiveMode_CustomSessionNumber_NumericUpDown.Name = "InteractiveMode_CustomSessionNumber_NumericUpDown"
		Me.InteractiveMode_CustomSessionNumber_NumericUpDown.Size = New System.Drawing.Size(70, 20)
		Me.InteractiveMode_CustomSessionNumber_NumericUpDown.TabIndex = 3
		'
		'InteractiveMode_UseCustomSession_RadioButton
		'
		Me.InteractiveMode_UseCustomSession_RadioButton.AutoSize = True
		Me.InteractiveMode_UseCustomSession_RadioButton.Location = New System.Drawing.Point(72, 118)
		Me.InteractiveMode_UseCustomSession_RadioButton.Name = "InteractiveMode_UseCustomSession_RadioButton"
		Me.InteractiveMode_UseCustomSession_RadioButton.Size = New System.Drawing.Size(86, 17)
		Me.InteractiveMode_UseCustomSession_RadioButton.TabIndex = 2
		Me.InteractiveMode_UseCustomSession_RadioButton.Text = "This session:"
		Me.InteractiveMode_UseCustomSession_RadioButton.UseVisualStyleBackColor = True
		'
		'InteractiveMode_UseConsoleSession_RadioButton
		'
		Me.InteractiveMode_UseConsoleSession_RadioButton.AutoSize = True
		Me.InteractiveMode_UseConsoleSession_RadioButton.Checked = True
		Me.InteractiveMode_UseConsoleSession_RadioButton.Location = New System.Drawing.Point(72, 95)
		Me.InteractiveMode_UseConsoleSession_RadioButton.Name = "InteractiveMode_UseConsoleSession_RadioButton"
		Me.InteractiveMode_UseConsoleSession_RadioButton.Size = New System.Drawing.Size(123, 17)
		Me.InteractiveMode_UseConsoleSession_RadioButton.TabIndex = 1
		Me.InteractiveMode_UseConsoleSession_RadioButton.TabStop = True
		Me.InteractiveMode_UseConsoleSession_RadioButton.Text = "The Console session"
		Me.InteractiveMode_UseConsoleSession_RadioButton.UseVisualStyleBackColor = True
		'
		'Label6
		'
		Me.Label6.AutoSize = True
		Me.Label6.Location = New System.Drawing.Point(57, 79)
		Me.Label6.Name = "Label6"
		Me.Label6.Size = New System.Drawing.Size(47, 13)
		Me.Label6.TabIndex = 4
		Me.Label6.Text = "Session:"
		'
		'PictureBox6
		'
		Me.PictureBox6.Image = Global.DocScript.My.Resources.Resources.Desktop
		Me.PictureBox6.Location = New System.Drawing.Point(10, 19)
		Me.PictureBox6.Name = "PictureBox6"
		Me.PictureBox6.Size = New System.Drawing.Size(40, 35)
		Me.PictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
		Me.PictureBox6.TabIndex = 3
		Me.PictureBox6.TabStop = False
		'
		'InteractiveMode_HideConsoleBeforeExecution_CheckBox
		'
		Me.InteractiveMode_HideConsoleBeforeExecution_CheckBox.AutoSize = True
		Me.InteractiveMode_HideConsoleBeforeExecution_CheckBox.Checked = True
		Me.InteractiveMode_HideConsoleBeforeExecution_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked
		Me.InteractiveMode_HideConsoleBeforeExecution_CheckBox.Location = New System.Drawing.Point(57, 45)
		Me.InteractiveMode_HideConsoleBeforeExecution_CheckBox.Name = "InteractiveMode_HideConsoleBeforeExecution_CheckBox"
		Me.InteractiveMode_HideConsoleBeforeExecution_CheckBox.Size = New System.Drawing.Size(181, 17)
		Me.InteractiveMode_HideConsoleBeforeExecution_CheckBox.TabIndex = 0
		Me.InteractiveMode_HideConsoleBeforeExecution_CheckBox.Text = "Hide Interpreter's Win32 Console"
		Me.InteractiveMode_HideConsoleBeforeExecution_CheckBox.UseVisualStyleBackColor = True
		'
		'InteractiveMode_UseGUIExecution_CheckBox
		'
		Me.InteractiveMode_UseGUIExecution_CheckBox.AutoSize = True
		Me.InteractiveMode_UseGUIExecution_CheckBox.Checked = True
		Me.InteractiveMode_UseGUIExecution_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked
		Me.InteractiveMode_UseGUIExecution_CheckBox.Location = New System.Drawing.Point(57, 22)
		Me.InteractiveMode_UseGUIExecution_CheckBox.Name = "InteractiveMode_UseGUIExecution_CheckBox"
		Me.InteractiveMode_UseGUIExecution_CheckBox.Size = New System.Drawing.Size(168, 17)
		Me.InteractiveMode_UseGUIExecution_CheckBox.TabIndex = 0
		Me.InteractiveMode_UseGUIExecution_CheckBox.Text = "Run DS-Program in GUI mode"
		Me.InteractiveMode_UseGUIExecution_CheckBox.UseVisualStyleBackColor = True
		'
		'GroupBox2
		'
		Me.GroupBox2.Controls.Add(Me.Label10)
		Me.GroupBox2.Controls.Add(Me.InteractiveMode_PsExecPath_TextBox)
		Me.GroupBox2.Controls.Add(Me.InteractiveMode_BrowseForPsExecButton)
		Me.GroupBox2.Controls.Add(Me.PictureBox7)
		Me.GroupBox2.Location = New System.Drawing.Point(6, 6)
		Me.GroupBox2.Name = "GroupBox2"
		Me.GroupBox2.Size = New System.Drawing.Size(360, 78)
		Me.GroupBox2.TabIndex = 0
		Me.GroupBox2.TabStop = False
		Me.GroupBox2.Text = "PsExec"
		'
		'Label10
		'
		Me.Label10.AutoSize = True
		Me.Label10.Location = New System.Drawing.Point(57, 23)
		Me.Label10.Name = "Label10"
		Me.Label10.Size = New System.Drawing.Size(70, 13)
		Me.Label10.TabIndex = 2
		Me.Label10.Text = "PsExec path:"
		'
		'InteractiveMode_PsExecPath_TextBox
		'
		Me.InteractiveMode_PsExecPath_TextBox.Location = New System.Drawing.Point(57, 43)
		Me.InteractiveMode_PsExecPath_TextBox.Name = "InteractiveMode_PsExecPath_TextBox"
		Me.InteractiveMode_PsExecPath_TextBox.Size = New System.Drawing.Size(266, 20)
		Me.InteractiveMode_PsExecPath_TextBox.TabIndex = 0
		Me.InteractiveMode_PsExecPath_TextBox.Text = "{Set at Runtime}"
		'
		'InteractiveMode_BrowseForPsExecButton
		'
		Me.InteractiveMode_BrowseForPsExecButton.Location = New System.Drawing.Point(329, 41)
		Me.InteractiveMode_BrowseForPsExecButton.Name = "InteractiveMode_BrowseForPsExecButton"
		Me.InteractiveMode_BrowseForPsExecButton.Size = New System.Drawing.Size(25, 23)
		Me.InteractiveMode_BrowseForPsExecButton.TabIndex = 1
		Me.InteractiveMode_BrowseForPsExecButton.Text = "..."
		Me.InteractiveMode_BrowseForPsExecButton.UseVisualStyleBackColor = True
		'
		'PictureBox7
		'
		Me.PictureBox7.Image = Global.DocScript.My.Resources.Resources.PsExec_NotActuallyTheLogoForItButStill
		Me.PictureBox7.Location = New System.Drawing.Point(10, 19)
		Me.PictureBox7.Name = "PictureBox7"
		Me.PictureBox7.Size = New System.Drawing.Size(40, 35)
		Me.PictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
		Me.PictureBox7.TabIndex = 3
		Me.PictureBox7.TabStop = False
		'
		'ConnectButton
		'
		Me.ConnectButton.Location = New System.Drawing.Point(273, 525)
		Me.ConnectButton.Name = "ConnectButton"
		Me.ConnectButton.Size = New System.Drawing.Size(119, 23)
		Me.ConnectButton.TabIndex = 4
		Me.ConnectButton.Text = "&Connect && Execute"
		Me.TheToolTipProvider.SetToolTip(Me.ConnectButton, "Copies the DocScript Runtime files to the target," & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "and starts the DSCLI.exe inter" & _
			  "preter process.")
		Me.ConnectButton.UseVisualStyleBackColor = True
		'
		'ShowHideOptionsButton
		'
		Me.ShowHideOptionsButton.Location = New System.Drawing.Point(12, 525)
		Me.ShowHideOptionsButton.Name = "ShowHideOptionsButton"
		Me.ShowHideOptionsButton.Size = New System.Drawing.Size(90, 23)
		Me.ShowHideOptionsButton.TabIndex = 5
		Me.ShowHideOptionsButton.Text = "Hide &Options"
		Me.TheToolTipProvider.SetToolTip(Me.ShowHideOptionsButton, "By default, the Interactive mode will be used with GUI Execution," & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "and you will b" & _
			  "e prompted for credentials." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "To access the other DSR modes, click this button." & _
			  "")
		Me.ShowHideOptionsButton.UseVisualStyleBackColor = True
		'
		'DSRemotingBanner_PictureBox
		'
		Me.DSRemotingBanner_PictureBox.Image = Global.DocScript.My.Resources.Resources.DSRemotingBanner
		Me.DSRemotingBanner_PictureBox.Location = New System.Drawing.Point(-2, -2)
		Me.DSRemotingBanner_PictureBox.Name = "DSRemotingBanner_PictureBox"
		Me.DSRemotingBanner_PictureBox.Size = New System.Drawing.Size(408, 85)
		Me.DSRemotingBanner_PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
		Me.DSRemotingBanner_PictureBox.TabIndex = 1
		Me.DSRemotingBanner_PictureBox.TabStop = False
		'
		'TheToolTipProvider
		'
		Me.TheToolTipProvider.AutomaticDelay = 1
		Me.TheToolTipProvider.AutoPopDelay = 30000
		Me.TheToolTipProvider.InitialDelay = 100
		Me.TheToolTipProvider.ReshowDelay = 1
		Me.TheToolTipProvider.ShowAlways = True
		Me.TheToolTipProvider.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info
		Me.TheToolTipProvider.ToolTipTitle = "Help"
		'
		'SimpleMode_ComputerTextBox
		'
		Me.SimpleMode_ComputerTextBox.Location = New System.Drawing.Point(82, 421)
		Me.SimpleMode_ComputerTextBox.Name = "SimpleMode_ComputerTextBox"
		Me.SimpleMode_ComputerTextBox.Size = New System.Drawing.Size(241, 20)
		Me.SimpleMode_ComputerTextBox.TabIndex = 2
		'
		'SimpleMode_ComputerLabel
		'
		Me.SimpleMode_ComputerLabel.AutoSize = True
		Me.SimpleMode_ComputerLabel.Location = New System.Drawing.Point(12, 424)
		Me.SimpleMode_ComputerLabel.Name = "SimpleMode_ComputerLabel"
		Me.SimpleMode_ComputerLabel.Size = New System.Drawing.Size(55, 13)
		Me.SimpleMode_ComputerLabel.TabIndex = 1
		Me.SimpleMode_ComputerLabel.Text = "Computer:"
		'
		'SimpleMode_InfoLabel
		'
		Me.SimpleMode_InfoLabel.AutoSize = True
		Me.SimpleMode_InfoLabel.Location = New System.Drawing.Point(12, 456)
		Me.SimpleMode_InfoLabel.Name = "SimpleMode_InfoLabel"
		Me.SimpleMode_InfoLabel.Size = New System.Drawing.Size(302, 26)
		Me.SimpleMode_InfoLabel.TabIndex = 3
		Me.SimpleMode_InfoLabel.Text = "You will be asked for credentials when you connect." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "DSR will use the ""Interactiv" & _
		  "e"" mode, with the Console session."
		'
		'DSRemotingWindow
		'
		Me.AcceptButton = Me.ConnectButton
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(404, 562)
		Me.Controls.Add(Me.SimpleMode_InfoLabel)
		Me.Controls.Add(Me.SimpleMode_ComputerLabel)
		Me.Controls.Add(Me.SimpleMode_ComputerTextBox)
		Me.Controls.Add(Me.ShowHideOptionsButton)
		Me.Controls.Add(Me.ConnectButton)
		Me.Controls.Add(Me.DSRemotingBanner_PictureBox)
		Me.Controls.Add(Me.TheTabControl)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.MaximumSize = New System.Drawing.Size(420, 600)
		Me.MinimizeBox = False
		Me.Name = "DSRemotingWindow"
		Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "DocScript Remoting"
		Me.TheTabControl.ResumeLayout(False)
		Me.Mode_TabPage.ResumeLayout(False)
		Me.Mode_TabPage.PerformLayout()
		CType(Me.InteractiveIcon_PictureBox, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.ShellIcon_PictureBox, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.HeadlessIcon_PictureBox, System.ComponentModel.ISupportInitialize).EndInit()
		Me.Target_TabPage.ResumeLayout(False)
		Me.GroupBox1.ResumeLayout(False)
		Me.GroupBox1.PerformLayout()
		CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).EndInit()
		Me.HostGroupBox.ResumeLayout(False)
		Me.HostGroupBox.PerformLayout()
		CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).EndInit()
		Me.Shell_TabPage.ResumeLayout(False)
		Me.GroupBox3.ResumeLayout(False)
		Me.GroupBox3.PerformLayout()
		CType(Me.PictureBox8, System.ComponentModel.ISupportInitialize).EndInit()
		Me.Interactive_TabPage.ResumeLayout(False)
		Me.GroupBox4.ResumeLayout(False)
		Me.GroupBox4.PerformLayout()
		CType(Me.InteractiveMode_CustomSessionNumber_NumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupBox2.ResumeLayout(False)
		Me.GroupBox2.PerformLayout()
		CType(Me.PictureBox7, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.DSRemotingBanner_PictureBox, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub

	Friend WithEvents TheTabControl As System.Windows.Forms.TabControl
	Friend WithEvents Mode_TabPage As System.Windows.Forms.TabPage
	Friend WithEvents Target_TabPage As System.Windows.Forms.TabPage
	Friend WithEvents DSRemotingBanner_PictureBox As System.Windows.Forms.PictureBox
	Friend WithEvents SelectModeLabel As System.Windows.Forms.Label
	Friend WithEvents InteractiveMode_RadioButton As System.Windows.Forms.RadioButton
	Friend WithEvents ShellMode_RadioButton As System.Windows.Forms.RadioButton
	Friend WithEvents HeadlessMode_RadioButton As System.Windows.Forms.RadioButton
	Friend WithEvents HeadlessDescriptionLabel As System.Windows.Forms.Label
	Friend WithEvents ConnectButton As System.Windows.Forms.Button
	Friend WithEvents HeadlessIcon_PictureBox As System.Windows.Forms.PictureBox
	Friend WithEvents ShellIcon_PictureBox As System.Windows.Forms.PictureBox
	Friend WithEvents Label1 As System.Windows.Forms.Label
	Friend WithEvents InteractiveIcon_PictureBox As System.Windows.Forms.PictureBox
	Friend WithEvents Label2 As System.Windows.Forms.Label
	Friend WithEvents ShowHideOptionsButton As System.Windows.Forms.Button
	Friend WithEvents HostGroupBox As System.Windows.Forms.GroupBox
	Friend WithEvents TargetComputer_TextBox As System.Windows.Forms.TextBox
	Friend WithEvents Label3 As System.Windows.Forms.Label
	Friend WithEvents PictureBox4 As System.Windows.Forms.PictureBox
	Friend WithEvents TargetUsername_TextBox As System.Windows.Forms.TextBox
	Friend WithEvents Label4 As System.Windows.Forms.Label
	Friend WithEvents TargetPassword_TextBox As System.Windows.Forms.TextBox
	Friend WithEvents Label5 As System.Windows.Forms.Label
	Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
	Friend WithEvents PictureBox5 As System.Windows.Forms.PictureBox
	Friend WithEvents DocScriptCLAs_TextBox As System.Windows.Forms.TextBox
	Friend WithEvents Label7 As System.Windows.Forms.Label
	Friend WithEvents TargetUNCDirectory_TextBox As System.Windows.Forms.TextBox
	Friend WithEvents Shell_TabPage As System.Windows.Forms.TabPage
	Friend WithEvents Interactive_TabPage As System.Windows.Forms.TabPage
	Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
	Friend WithEvents InteractiveMode_PsExecPath_TextBox As System.Windows.Forms.TextBox
	Friend WithEvents InteractiveMode_BrowseForPsExecButton As System.Windows.Forms.Button
	Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
	Friend WithEvents InteractiveMode_UseGUIExecution_CheckBox As System.Windows.Forms.CheckBox
	Friend WithEvents PictureBox6 As System.Windows.Forms.PictureBox
	Friend WithEvents Label6 As System.Windows.Forms.Label
	Friend WithEvents InteractiveMode_UseCustomSession_RadioButton As System.Windows.Forms.RadioButton
	Friend WithEvents InteractiveMode_UseConsoleSession_RadioButton As System.Windows.Forms.RadioButton
	Friend WithEvents InteractiveMode_CustomSessionNumber_NumericUpDown As System.Windows.Forms.NumericUpDown
	Friend WithEvents PictureBox7 As System.Windows.Forms.PictureBox
	Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
	Friend WithEvents ShellMode_PsExecPath_TextBox As System.Windows.Forms.TextBox
	Friend WithEvents ShellMode_BrowseForPsExecButton As System.Windows.Forms.Button
	Friend WithEvents PictureBox8 As System.Windows.Forms.PictureBox
	Public WithEvents TheToolTipProvider As System.Windows.Forms.ToolTip
	Friend WithEvents Label8 As System.Windows.Forms.Label
	Friend WithEvents SimpleMode_ComputerTextBox As System.Windows.Forms.TextBox
	Friend WithEvents SimpleMode_ComputerLabel As System.Windows.Forms.Label
	Friend WithEvents SimpleMode_InfoLabel As System.Windows.Forms.Label
	Friend WithEvents DeleteDSFilesFromRemoteAtEnd_CheckBox As System.Windows.Forms.CheckBox
	Friend WithEvents Label9 As System.Windows.Forms.Label
	Friend WithEvents Label10 As System.Windows.Forms.Label
	Friend WithEvents InteractiveMode_HideConsoleBeforeExecution_CheckBox As System.Windows.Forms.CheckBox
	Friend WithEvents Utility_StartRemoteShellSession_LinkLabel As System.Windows.Forms.LinkLabel
	Friend WithEvents Utility_LaunchNewRFE_LinkLabel As System.Windows.Forms.LinkLabel

End Class