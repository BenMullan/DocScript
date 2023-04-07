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
		Me.InteractiveIcon_PictureBox = New System.Windows.Forms.PictureBox()
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
		Me.ShellMode_CustomPSEXECPATH_TextBox = New System.Windows.Forms.TextBox()
		Me.Button3 = New System.Windows.Forms.Button()
		Me.CheckBox2 = New System.Windows.Forms.CheckBox()
		Me.PictureBox8 = New System.Windows.Forms.PictureBox()
		Me.Interactive_TabPage = New System.Windows.Forms.TabPage()
		Me.GroupBox4 = New System.Windows.Forms.GroupBox()
		Me.InteractiveSession_NumericUpDown = New System.Windows.Forms.NumericUpDown()
		Me.RadioButton5 = New System.Windows.Forms.RadioButton()
		Me.RadioButton4 = New System.Windows.Forms.RadioButton()
		Me.Label6 = New System.Windows.Forms.Label()
		Me.PictureBox6 = New System.Windows.Forms.PictureBox()
		Me.CheckBox4 = New System.Windows.Forms.CheckBox()
		Me.GroupBox2 = New System.Windows.Forms.GroupBox()
		Me.InteractiveMode_CustomPSEXECPATH_TextBox = New System.Windows.Forms.TextBox()
		Me.Button2 = New System.Windows.Forms.Button()
		Me.CheckBox3 = New System.Windows.Forms.CheckBox()
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
		CType(Me.InteractiveSession_NumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
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
		Me.Mode_TabPage.Controls.Add(Me.InteractiveIcon_PictureBox)
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
		Me.Label1.Size = New System.Drawing.Size(253, 26)
		Me.Label1.TabIndex = 4
		Me.Label1.Text = "Starts a DSCLI.exe process, and handles command-" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "line interaction to and from it" & _
		  ". Uses PsExec."
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
		Me.TargetUNCDirectory_TextBox.Text = "C$\Windows\DSRemoting\"
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
		Me.GroupBox3.Controls.Add(Me.ShellMode_CustomPSEXECPATH_TextBox)
		Me.GroupBox3.Controls.Add(Me.Button3)
		Me.GroupBox3.Controls.Add(Me.CheckBox2)
		Me.GroupBox3.Controls.Add(Me.PictureBox8)
		Me.GroupBox3.Location = New System.Drawing.Point(6, 6)
		Me.GroupBox3.Name = "GroupBox3"
		Me.GroupBox3.Size = New System.Drawing.Size(360, 78)
		Me.GroupBox3.TabIndex = 0
		Me.GroupBox3.TabStop = False
		Me.GroupBox3.Text = "PsExec"
		'
		'ShellMode_CustomPSEXECPATH_TextBox
		'
		Me.ShellMode_CustomPSEXECPATH_TextBox.Enabled = False
		Me.ShellMode_CustomPSEXECPATH_TextBox.Location = New System.Drawing.Point(57, 43)
		Me.ShellMode_CustomPSEXECPATH_TextBox.Name = "ShellMode_CustomPSEXECPATH_TextBox"
		Me.ShellMode_CustomPSEXECPATH_TextBox.Size = New System.Drawing.Size(266, 20)
		Me.ShellMode_CustomPSEXECPATH_TextBox.TabIndex = 1
		Me.ShellMode_CustomPSEXECPATH_TextBox.Text = "$(DSIDE_Dir)\PsExec.exe"
		'
		'Button3
		'
		Me.Button3.Enabled = False
		Me.Button3.Location = New System.Drawing.Point(329, 41)
		Me.Button3.Name = "Button3"
		Me.Button3.Size = New System.Drawing.Size(25, 23)
		Me.Button3.TabIndex = 2
		Me.Button3.Text = "..."
		Me.Button3.UseVisualStyleBackColor = True
		'
		'CheckBox2
		'
		Me.CheckBox2.AutoSize = True
		Me.CheckBox2.Location = New System.Drawing.Point(57, 19)
		Me.CheckBox2.Name = "CheckBox2"
		Me.CheckBox2.Size = New System.Drawing.Size(144, 17)
		Me.CheckBox2.TabIndex = 0
		Me.CheckBox2.Text = "Use custom PsExec exe:"
		Me.CheckBox2.UseVisualStyleBackColor = True
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
		Me.GroupBox4.Controls.Add(Me.InteractiveSession_NumericUpDown)
		Me.GroupBox4.Controls.Add(Me.RadioButton5)
		Me.GroupBox4.Controls.Add(Me.RadioButton4)
		Me.GroupBox4.Controls.Add(Me.Label6)
		Me.GroupBox4.Controls.Add(Me.PictureBox6)
		Me.GroupBox4.Controls.Add(Me.CheckBox4)
		Me.GroupBox4.Location = New System.Drawing.Point(6, 90)
		Me.GroupBox4.Name = "GroupBox4"
		Me.GroupBox4.Size = New System.Drawing.Size(360, 120)
		Me.GroupBox4.TabIndex = 0
		Me.GroupBox4.TabStop = False
		Me.GroupBox4.Text = "Options"
		'
		'InteractiveSession_NumericUpDown
		'
		Me.InteractiveSession_NumericUpDown.Enabled = False
		Me.InteractiveSession_NumericUpDown.Location = New System.Drawing.Point(164, 85)
		Me.InteractiveSession_NumericUpDown.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
		Me.InteractiveSession_NumericUpDown.Name = "InteractiveSession_NumericUpDown"
		Me.InteractiveSession_NumericUpDown.Size = New System.Drawing.Size(70, 20)
		Me.InteractiveSession_NumericUpDown.TabIndex = 4
		'
		'RadioButton5
		'
		Me.RadioButton5.AutoSize = True
		Me.RadioButton5.Location = New System.Drawing.Point(72, 85)
		Me.RadioButton5.Name = "RadioButton5"
		Me.RadioButton5.Size = New System.Drawing.Size(86, 17)
		Me.RadioButton5.TabIndex = 3
		Me.RadioButton5.Text = "This session:"
		Me.RadioButton5.UseVisualStyleBackColor = True
		'
		'RadioButton4
		'
		Me.RadioButton4.AutoSize = True
		Me.RadioButton4.Checked = True
		Me.RadioButton4.Location = New System.Drawing.Point(72, 62)
		Me.RadioButton4.Name = "RadioButton4"
		Me.RadioButton4.Size = New System.Drawing.Size(123, 17)
		Me.RadioButton4.TabIndex = 2
		Me.RadioButton4.TabStop = True
		Me.RadioButton4.Text = "The Console session"
		Me.RadioButton4.UseVisualStyleBackColor = True
		'
		'Label6
		'
		Me.Label6.AutoSize = True
		Me.Label6.Location = New System.Drawing.Point(57, 46)
		Me.Label6.Name = "Label6"
		Me.Label6.Size = New System.Drawing.Size(47, 13)
		Me.Label6.TabIndex = 1
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
		'CheckBox4
		'
		Me.CheckBox4.AutoSize = True
		Me.CheckBox4.Checked = True
		Me.CheckBox4.CheckState = System.Windows.Forms.CheckState.Checked
		Me.CheckBox4.Location = New System.Drawing.Point(57, 22)
		Me.CheckBox4.Name = "CheckBox4"
		Me.CheckBox4.Size = New System.Drawing.Size(95, 17)
		Me.CheckBox4.TabIndex = 0
		Me.CheckBox4.Text = "GUI Execution"
		Me.CheckBox4.UseVisualStyleBackColor = True
		'
		'GroupBox2
		'
		Me.GroupBox2.Controls.Add(Me.InteractiveMode_CustomPSEXECPATH_TextBox)
		Me.GroupBox2.Controls.Add(Me.Button2)
		Me.GroupBox2.Controls.Add(Me.CheckBox3)
		Me.GroupBox2.Controls.Add(Me.PictureBox7)
		Me.GroupBox2.Location = New System.Drawing.Point(6, 6)
		Me.GroupBox2.Name = "GroupBox2"
		Me.GroupBox2.Size = New System.Drawing.Size(360, 78)
		Me.GroupBox2.TabIndex = 1
		Me.GroupBox2.TabStop = False
		Me.GroupBox2.Text = "PsExec"
		'
		'InteractiveMode_CustomPSEXECPATH_TextBox
		'
		Me.InteractiveMode_CustomPSEXECPATH_TextBox.Enabled = False
		Me.InteractiveMode_CustomPSEXECPATH_TextBox.Location = New System.Drawing.Point(57, 43)
		Me.InteractiveMode_CustomPSEXECPATH_TextBox.Name = "InteractiveMode_CustomPSEXECPATH_TextBox"
		Me.InteractiveMode_CustomPSEXECPATH_TextBox.Size = New System.Drawing.Size(266, 20)
		Me.InteractiveMode_CustomPSEXECPATH_TextBox.TabIndex = 1
		Me.InteractiveMode_CustomPSEXECPATH_TextBox.Text = "$(DSIDE_Dir)\PsExec.exe"
		'
		'Button2
		'
		Me.Button2.Enabled = False
		Me.Button2.Location = New System.Drawing.Point(329, 41)
		Me.Button2.Name = "Button2"
		Me.Button2.Size = New System.Drawing.Size(25, 23)
		Me.Button2.TabIndex = 2
		Me.Button2.Text = "..."
		Me.Button2.UseVisualStyleBackColor = True
		'
		'CheckBox3
		'
		Me.CheckBox3.AutoSize = True
		Me.CheckBox3.Location = New System.Drawing.Point(57, 19)
		Me.CheckBox3.Name = "CheckBox3"
		Me.CheckBox3.Size = New System.Drawing.Size(144, 17)
		Me.CheckBox3.TabIndex = 0
		Me.CheckBox3.Text = "Use custom PsExec exe:"
		Me.CheckBox3.UseVisualStyleBackColor = True
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
		Me.ConnectButton.Location = New System.Drawing.Point(317, 525)
		Me.ConnectButton.Name = "ConnectButton"
		Me.ConnectButton.Size = New System.Drawing.Size(75, 23)
		Me.ConnectButton.TabIndex = 4
		Me.ConnectButton.Text = "&Connect"
		Me.ConnectButton.UseVisualStyleBackColor = True
		'
		'ShowHideOptionsButton
		'
		Me.ShowHideOptionsButton.Location = New System.Drawing.Point(12, 525)
		Me.ShowHideOptionsButton.Name = "ShowHideOptionsButton"
		Me.ShowHideOptionsButton.Size = New System.Drawing.Size(90, 23)
		Me.ShowHideOptionsButton.TabIndex = 5
		Me.ShowHideOptionsButton.Text = "Hide &Options"
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
		Me.TheToolTipProvider.AutomaticDelay = 0
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
		CType(Me.InteractiveSession_NumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
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
	Friend WithEvents InteractiveMode_CustomPSEXECPATH_TextBox As System.Windows.Forms.TextBox
	Friend WithEvents Button2 As System.Windows.Forms.Button
	Friend WithEvents CheckBox3 As System.Windows.Forms.CheckBox
	Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
	Friend WithEvents CheckBox4 As System.Windows.Forms.CheckBox
	Friend WithEvents PictureBox6 As System.Windows.Forms.PictureBox
	Friend WithEvents Label6 As System.Windows.Forms.Label
	Friend WithEvents RadioButton5 As System.Windows.Forms.RadioButton
	Friend WithEvents RadioButton4 As System.Windows.Forms.RadioButton
	Friend WithEvents InteractiveSession_NumericUpDown As System.Windows.Forms.NumericUpDown
	Friend WithEvents PictureBox7 As System.Windows.Forms.PictureBox
	Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
	Friend WithEvents ShellMode_CustomPSEXECPATH_TextBox As System.Windows.Forms.TextBox
	Friend WithEvents Button3 As System.Windows.Forms.Button
	Friend WithEvents CheckBox2 As System.Windows.Forms.CheckBox
	Friend WithEvents PictureBox8 As System.Windows.Forms.PictureBox
	Public WithEvents TheToolTipProvider As System.Windows.Forms.ToolTip
	Friend WithEvents Label8 As System.Windows.Forms.Label
	Friend WithEvents SimpleMode_ComputerTextBox As System.Windows.Forms.TextBox
	Friend WithEvents SimpleMode_ComputerLabel As System.Windows.Forms.Label
	Friend WithEvents SimpleMode_InfoLabel As System.Windows.Forms.Label
	Friend WithEvents DeleteDSFilesFromRemoteAtEnd_CheckBox As System.Windows.Forms.CheckBox

End Class