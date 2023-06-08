<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RemoteFileExplorerWindow
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RemoteFileExplorerWindow))
		Me.PictureBox1 = New System.Windows.Forms.PictureBox()
		Me.TargetGroupBox = New System.Windows.Forms.GroupBox()
		Me.UpADir_LinkLabel = New System.Windows.Forms.LinkLabel()
		Me.ConnectButton = New System.Windows.Forms.Button()
		Me.Password_TextBox = New System.Windows.Forms.TextBox()
		Me.Username_TextBox = New System.Windows.Forms.TextBox()
		Me.RemoteUNCPath_TextBox = New System.Windows.Forms.TextBox()
		Me.Label3 = New System.Windows.Forms.Label()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.UNCPathLabel = New System.Windows.Forms.Label()
		Me.RemoteFilesGroupBox = New System.Windows.Forms.GroupBox()
		Me.RemoteFilesWebBrowser = New System.Windows.Forms.WebBrowser()
		CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.TargetGroupBox.SuspendLayout()
		Me.RemoteFilesGroupBox.SuspendLayout()
		Me.SuspendLayout()
		'
		'PictureBox1
		'
		Me.PictureBox1.Image = Global.DocScript.My.Resources.Resources.NetFolder
		Me.PictureBox1.Location = New System.Drawing.Point(13, 18)
		Me.PictureBox1.Name = "PictureBox1"
		Me.PictureBox1.Size = New System.Drawing.Size(60, 49)
		Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
		Me.PictureBox1.TabIndex = 0
		Me.PictureBox1.TabStop = False
		'
		'TargetGroupBox
		'
		Me.TargetGroupBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
				  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.TargetGroupBox.Controls.Add(Me.UpADir_LinkLabel)
		Me.TargetGroupBox.Controls.Add(Me.ConnectButton)
		Me.TargetGroupBox.Controls.Add(Me.Password_TextBox)
		Me.TargetGroupBox.Controls.Add(Me.Username_TextBox)
		Me.TargetGroupBox.Controls.Add(Me.RemoteUNCPath_TextBox)
		Me.TargetGroupBox.Controls.Add(Me.Label3)
		Me.TargetGroupBox.Controls.Add(Me.Label2)
		Me.TargetGroupBox.Controls.Add(Me.UNCPathLabel)
		Me.TargetGroupBox.Controls.Add(Me.PictureBox1)
		Me.TargetGroupBox.Location = New System.Drawing.Point(12, 12)
		Me.TargetGroupBox.Name = "TargetGroupBox"
		Me.TargetGroupBox.Size = New System.Drawing.Size(760, 80)
		Me.TargetGroupBox.TabIndex = 1
		Me.TargetGroupBox.TabStop = False
		Me.TargetGroupBox.Text = "Target"
		'
		'UpADir_LinkLabel
		'
		Me.UpADir_LinkLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.UpADir_LinkLabel.AutoSize = True
		Me.UpADir_LinkLabel.Location = New System.Drawing.Point(573, 25)
		Me.UpADir_LinkLabel.Name = "UpADir_LinkLabel"
		Me.UpADir_LinkLabel.Size = New System.Drawing.Size(88, 13)
		Me.UpADir_LinkLabel.TabIndex = 4
		Me.UpADir_LinkLabel.TabStop = True
		Me.UpADir_LinkLabel.Text = "↑ Up a directory"
		Me.UpADir_LinkLabel.Visible = False
		'
		'ConnectButton
		'
		Me.ConnectButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.ConnectButton.Location = New System.Drawing.Point(679, 46)
		Me.ConnectButton.Name = "ConnectButton"
		Me.ConnectButton.Size = New System.Drawing.Size(65, 24)
		Me.ConnectButton.TabIndex = 7
		Me.ConnectButton.Text = "Connect"
		Me.ConnectButton.UseVisualStyleBackColor = True
		'
		'Password_TextBox
		'
		Me.Password_TextBox.Location = New System.Drawing.Point(465, 49)
		Me.Password_TextBox.Name = "Password_TextBox"
		Me.Password_TextBox.Size = New System.Drawing.Size(200, 20)
		Me.Password_TextBox.TabIndex = 6
		Me.Password_TextBox.UseSystemPasswordChar = True
		'
		'Username_TextBox
		'
		Me.Username_TextBox.Location = New System.Drawing.Point(146, 49)
		Me.Username_TextBox.Name = "Username_TextBox"
		Me.Username_TextBox.Size = New System.Drawing.Size(200, 20)
		Me.Username_TextBox.TabIndex = 5
		'
		'RemoteUNCPath_TextBox
		'
		Me.RemoteUNCPath_TextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
				  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.RemoteUNCPath_TextBox.Location = New System.Drawing.Point(146, 23)
		Me.RemoteUNCPath_TextBox.Name = "RemoteUNCPath_TextBox"
		Me.RemoteUNCPath_TextBox.Size = New System.Drawing.Size(519, 20)
		Me.RemoteUNCPath_TextBox.TabIndex = 3
		Me.RemoteUNCPath_TextBox.Text = "\\127.0.0.1\C$\"
		'
		'Label3
		'
		Me.Label3.AutoSize = True
		Me.Label3.Location = New System.Drawing.Point(403, 52)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(56, 13)
		Me.Label3.TabIndex = 2
		Me.Label3.Text = "Password:"
		'
		'Label2
		'
		Me.Label2.AutoSize = True
		Me.Label2.Location = New System.Drawing.Point(82, 52)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(58, 13)
		Me.Label2.TabIndex = 1
		Me.Label2.Text = "Username:"
		'
		'UNCPathLabel
		'
		Me.UNCPathLabel.AutoSize = True
		Me.UNCPathLabel.Location = New System.Drawing.Point(82, 26)
		Me.UNCPathLabel.Name = "UNCPathLabel"
		Me.UNCPathLabel.Size = New System.Drawing.Size(58, 13)
		Me.UNCPathLabel.TabIndex = 0
		Me.UNCPathLabel.Text = "UNC Path:"
		'
		'RemoteFilesGroupBox
		'
		Me.RemoteFilesGroupBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
				  Or System.Windows.Forms.AnchorStyles.Left) _
				  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.RemoteFilesGroupBox.Controls.Add(Me.RemoteFilesWebBrowser)
		Me.RemoteFilesGroupBox.Enabled = False
		Me.RemoteFilesGroupBox.Location = New System.Drawing.Point(12, 98)
		Me.RemoteFilesGroupBox.Name = "RemoteFilesGroupBox"
		Me.RemoteFilesGroupBox.Size = New System.Drawing.Size(760, 402)
		Me.RemoteFilesGroupBox.TabIndex = 0
		Me.RemoteFilesGroupBox.TabStop = False
		Me.RemoteFilesGroupBox.Text = "Remote Files"
		'
		'RemoteFilesWebBrowser
		'
		Me.RemoteFilesWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill
		Me.RemoteFilesWebBrowser.Location = New System.Drawing.Point(3, 16)
		Me.RemoteFilesWebBrowser.MinimumSize = New System.Drawing.Size(20, 20)
		Me.RemoteFilesWebBrowser.Name = "RemoteFilesWebBrowser"
		Me.RemoteFilesWebBrowser.Size = New System.Drawing.Size(754, 383)
		Me.RemoteFilesWebBrowser.TabIndex = 0
		'
		'RemoteFileExplorerWindow
		'
		Me.AcceptButton = Me.ConnectButton
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(784, 512)
		Me.Controls.Add(Me.RemoteFilesGroupBox)
		Me.Controls.Add(Me.TargetGroupBox)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MinimumSize = New System.Drawing.Size(800, 300)
		Me.Name = "RemoteFileExplorerWindow"
		Me.Text = "Remote File Explorer"
		CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.TargetGroupBox.ResumeLayout(False)
		Me.TargetGroupBox.PerformLayout()
		Me.RemoteFilesGroupBox.ResumeLayout(False)
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
	Friend WithEvents TargetGroupBox As System.Windows.Forms.GroupBox
	Friend WithEvents UNCPathLabel As System.Windows.Forms.Label
	Friend WithEvents Label3 As System.Windows.Forms.Label
	Friend WithEvents Label2 As System.Windows.Forms.Label
	Friend WithEvents Password_TextBox As System.Windows.Forms.TextBox
	Friend WithEvents Username_TextBox As System.Windows.Forms.TextBox
	Friend WithEvents RemoteUNCPath_TextBox As System.Windows.Forms.TextBox
	Friend WithEvents ConnectButton As System.Windows.Forms.Button
	Friend WithEvents RemoteFilesGroupBox As System.Windows.Forms.GroupBox
	Friend WithEvents RemoteFilesWebBrowser As System.Windows.Forms.WebBrowser
	Friend WithEvents UpADir_LinkLabel As System.Windows.Forms.LinkLabel
End Class
