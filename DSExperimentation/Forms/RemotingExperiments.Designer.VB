<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RemotingExperiments
	Inherits System.Windows.Forms.Form

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
		Me.Button1 = New System.Windows.Forms.Button()
		Me.TargetPCTextBox = New System.Windows.Forms.TextBox()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.UsernameTextBox = New System.Windows.Forms.TextBox()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.PasswordTextBox = New System.Windows.Forms.TextBox()
		Me.Label3 = New System.Windows.Forms.Label()
		Me.CommandTextBox = New System.Windows.Forms.TextBox()
		Me.Label4 = New System.Windows.Forms.Label()
		Me.RichTextBox1 = New System.Windows.Forms.RichTextBox()
		Me.DseFormChooser1 = New DSExperimentation.DSEFormChooser()
		Me.GroupBox1 = New System.Windows.Forms.GroupBox()
		Me.GroupBox2 = New System.Windows.Forms.GroupBox()
		Me.LocalFileTextBox = New System.Windows.Forms.TextBox()
		Me.Label5 = New System.Windows.Forms.Label()
		Me.RemoteFileTextBox = New System.Windows.Forms.TextBox()
		Me.Label8 = New System.Windows.Forms.Label()
		Me.AuthAndCopyButton = New System.Windows.Forms.Button()
		Me.GroupBox3 = New System.Windows.Forms.GroupBox()
		Me.GroupBox1.SuspendLayout()
		Me.GroupBox2.SuspendLayout()
		Me.GroupBox3.SuspendLayout()
		Me.SuspendLayout()
		'
		'Button1
		'
		Me.Button1.Location = New System.Drawing.Point(19, 87)
		Me.Button1.Name = "Button1"
		Me.Button1.Size = New System.Drawing.Size(625, 23)
		Me.Button1.TabIndex = 1
		Me.Button1.Text = "WMI Create Remote Process"
		Me.Button1.UseVisualStyleBackColor = True
		'
		'TargetPCTextBox
		'
		Me.TargetPCTextBox.Location = New System.Drawing.Point(111, 25)
		Me.TargetPCTextBox.Name = "TargetPCTextBox"
		Me.TargetPCTextBox.Size = New System.Drawing.Size(533, 20)
		Me.TargetPCTextBox.TabIndex = 2
		Me.TargetPCTextBox.Text = "192.168.1.85"
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Location = New System.Drawing.Point(16, 28)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(89, 13)
		Me.Label1.TabIndex = 3
		Me.Label1.Text = "Target Computer:"
		'
		'UsernameTextBox
		'
		Me.UsernameTextBox.Location = New System.Drawing.Point(82, 19)
		Me.UsernameTextBox.Name = "UsernameTextBox"
		Me.UsernameTextBox.Size = New System.Drawing.Size(562, 20)
		Me.UsernameTextBox.TabIndex = 2
		Me.UsernameTextBox.Text = ".\Administrator"
		'
		'Label2
		'
		Me.Label2.AutoSize = True
		Me.Label2.Location = New System.Drawing.Point(11, 22)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(58, 13)
		Me.Label2.TabIndex = 3
		Me.Label2.Text = "Username:"
		'
		'PasswordTextBox
		'
		Me.PasswordTextBox.Location = New System.Drawing.Point(82, 45)
		Me.PasswordTextBox.Name = "PasswordTextBox"
		Me.PasswordTextBox.Size = New System.Drawing.Size(562, 20)
		Me.PasswordTextBox.TabIndex = 2
		Me.PasswordTextBox.Text = "PASS123"
		'
		'Label3
		'
		Me.Label3.AutoSize = True
		Me.Label3.Location = New System.Drawing.Point(11, 48)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(56, 13)
		Me.Label3.TabIndex = 3
		Me.Label3.Text = "Password:"
		'
		'CommandTextBox
		'
		Me.CommandTextBox.Location = New System.Drawing.Point(111, 52)
		Me.CommandTextBox.Name = "CommandTextBox"
		Me.CommandTextBox.Size = New System.Drawing.Size(533, 20)
		Me.CommandTextBox.TabIndex = 2
		Me.CommandTextBox.Text = "C:\Resources\DocScript\ProofOfExecution.exe"
		'
		'Label4
		'
		Me.Label4.AutoSize = True
		Me.Label4.Location = New System.Drawing.Point(16, 55)
		Me.Label4.Name = "Label4"
		Me.Label4.Size = New System.Drawing.Size(80, 13)
		Me.Label4.TabIndex = 3
		Me.Label4.Text = "Command Line:"
		'
		'RichTextBox1
		'
		Me.RichTextBox1.Location = New System.Drawing.Point(12, 368)
		Me.RichTextBox1.Name = "RichTextBox1"
		Me.RichTextBox1.Size = New System.Drawing.Size(815, 79)
		Me.RichTextBox1.TabIndex = 4
		Me.RichTextBox1.Text = "C:\Resources\SysInternals\PsExec.exe -i 0 dialer.exe" & Global.Microsoft.VisualBasic.ChrW(10) & "C:\Resources\SysInternals\Ps" & _
		  "Exec.exe -i 0 C:\Resources\DocScript\PlayWav.exe /Wav:""C:\Resources\DocScript\Ru" & _
		  "leBritannia.wav"""
		'
		'DseFormChooser1
		'
		Me.DseFormChooser1.AutoScroll = True
		Me.DseFormChooser1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
		Me.DseFormChooser1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.DseFormChooser1.Location = New System.Drawing.Point(12, 12)
		Me.DseFormChooser1.Name = "DseFormChooser1"
		Me.DseFormChooser1.Size = New System.Drawing.Size(150, 340)
		Me.DseFormChooser1.TabIndex = 0
		'
		'GroupBox1
		'
		Me.GroupBox1.Controls.Add(Me.Button1)
		Me.GroupBox1.Controls.Add(Me.Label4)
		Me.GroupBox1.Controls.Add(Me.TargetPCTextBox)
		Me.GroupBox1.Controls.Add(Me.Label1)
		Me.GroupBox1.Controls.Add(Me.CommandTextBox)
		Me.GroupBox1.Location = New System.Drawing.Point(168, 227)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(659, 125)
		Me.GroupBox1.TabIndex = 5
		Me.GroupBox1.TabStop = False
		Me.GroupBox1.Text = "WMI Execute"
		'
		'GroupBox2
		'
		Me.GroupBox2.Controls.Add(Me.AuthAndCopyButton)
		Me.GroupBox2.Controls.Add(Me.LocalFileTextBox)
		Me.GroupBox2.Controls.Add(Me.Label8)
		Me.GroupBox2.Controls.Add(Me.Label5)
		Me.GroupBox2.Controls.Add(Me.RemoteFileTextBox)
		Me.GroupBox2.Location = New System.Drawing.Point(168, 98)
		Me.GroupBox2.Name = "GroupBox2"
		Me.GroupBox2.Size = New System.Drawing.Size(659, 123)
		Me.GroupBox2.TabIndex = 6
		Me.GroupBox2.TabStop = False
		Me.GroupBox2.Text = "Copy File"
		'
		'LocalFileTextBox
		'
		Me.LocalFileTextBox.Location = New System.Drawing.Point(108, 21)
		Me.LocalFileTextBox.Name = "LocalFileTextBox"
		Me.LocalFileTextBox.Size = New System.Drawing.Size(536, 20)
		Me.LocalFileTextBox.TabIndex = 2
		Me.LocalFileTextBox.Text = "F:\DSRemotingSession_03042023_135503_ScriptAK47.DS"
		'
		'Label5
		'
		Me.Label5.AutoSize = True
		Me.Label5.Location = New System.Drawing.Point(13, 52)
		Me.Label5.Name = "Label5"
		Me.Label5.Size = New System.Drawing.Size(66, 13)
		Me.Label5.TabIndex = 3
		Me.Label5.Text = "Remote File:"
		'
		'RemoteFileTextBox
		'
		Me.RemoteFileTextBox.Location = New System.Drawing.Point(108, 49)
		Me.RemoteFileTextBox.Name = "RemoteFileTextBox"
		Me.RemoteFileTextBox.Size = New System.Drawing.Size(536, 20)
		Me.RemoteFileTextBox.TabIndex = 2
		Me.RemoteFileTextBox.Text = "\\192.168.1.85\C$\Windows\DSRemoting\DSRemotingSession_03042023_135503_ScriptAK47" & _
		  ".DS"
		'
		'Label8
		'
		Me.Label8.AutoSize = True
		Me.Label8.Location = New System.Drawing.Point(13, 24)
		Me.Label8.Name = "Label8"
		Me.Label8.Size = New System.Drawing.Size(55, 13)
		Me.Label8.TabIndex = 3
		Me.Label8.Text = "Local File:"
		'
		'AuthAndCopyButton
		'
		Me.AuthAndCopyButton.Location = New System.Drawing.Point(16, 84)
		Me.AuthAndCopyButton.Name = "AuthAndCopyButton"
		Me.AuthAndCopyButton.Size = New System.Drawing.Size(628, 23)
		Me.AuthAndCopyButton.TabIndex = 1
		Me.AuthAndCopyButton.Text = "Authenticate and Copy File"
		Me.AuthAndCopyButton.UseVisualStyleBackColor = True
		'
		'GroupBox3
		'
		Me.GroupBox3.Controls.Add(Me.PasswordTextBox)
		Me.GroupBox3.Controls.Add(Me.Label2)
		Me.GroupBox3.Controls.Add(Me.UsernameTextBox)
		Me.GroupBox3.Controls.Add(Me.Label3)
		Me.GroupBox3.Location = New System.Drawing.Point(168, 12)
		Me.GroupBox3.Name = "GroupBox3"
		Me.GroupBox3.Size = New System.Drawing.Size(659, 80)
		Me.GroupBox3.TabIndex = 7
		Me.GroupBox3.TabStop = False
		Me.GroupBox3.Text = "Credentials for Remote Computer"
		'
		'RemotingExperiments
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(839, 463)
		Me.Controls.Add(Me.GroupBox3)
		Me.Controls.Add(Me.RichTextBox1)
		Me.Controls.Add(Me.GroupBox2)
		Me.Controls.Add(Me.GroupBox1)
		Me.Controls.Add(Me.DseFormChooser1)
		Me.Name = "RemotingExperiments"
		Me.Text = "RemotingExperiments"
		Me.GroupBox1.ResumeLayout(False)
		Me.GroupBox1.PerformLayout()
		Me.GroupBox2.ResumeLayout(False)
		Me.GroupBox2.PerformLayout()
		Me.GroupBox3.ResumeLayout(False)
		Me.GroupBox3.PerformLayout()
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents DseFormChooser1 As DSExperimentation.DSEFormChooser
	Friend WithEvents Button1 As System.Windows.Forms.Button
	Friend WithEvents TargetPCTextBox As System.Windows.Forms.TextBox
	Friend WithEvents Label1 As System.Windows.Forms.Label
	Friend WithEvents UsernameTextBox As System.Windows.Forms.TextBox
	Friend WithEvents Label2 As System.Windows.Forms.Label
	Friend WithEvents PasswordTextBox As System.Windows.Forms.TextBox
	Friend WithEvents Label3 As System.Windows.Forms.Label
	Friend WithEvents CommandTextBox As System.Windows.Forms.TextBox
	Friend WithEvents Label4 As System.Windows.Forms.Label
	Friend WithEvents RichTextBox1 As System.Windows.Forms.RichTextBox
	Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
	Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
	Friend WithEvents AuthAndCopyButton As System.Windows.Forms.Button
	Friend WithEvents LocalFileTextBox As System.Windows.Forms.TextBox
	Friend WithEvents Label8 As System.Windows.Forms.Label
	Friend WithEvents Label5 As System.Windows.Forms.Label
	Friend WithEvents RemoteFileTextBox As System.Windows.Forms.TextBox
	Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
End Class
