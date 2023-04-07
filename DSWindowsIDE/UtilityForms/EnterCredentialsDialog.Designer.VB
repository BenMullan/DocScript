<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EnterCredentialsDialog
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(EnterCredentialsDialog))
		Me.Label1 = New System.Windows.Forms.Label()
		Me.UsernameTextBox = New System.Windows.Forms.TextBox()
		Me.OkButton = New System.Windows.Forms.Button()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.PasswordTextBox = New System.Windows.Forms.TextBox()
		Me.PictureBox1 = New System.Windows.Forms.PictureBox()
		Me.Label3 = New System.Windows.Forms.Label()
		CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Location = New System.Drawing.Point(85, 38)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(58, 13)
		Me.Label1.TabIndex = 0
		Me.Label1.Text = "Username:"
		'
		'UsernameTextBox
		'
		Me.UsernameTextBox.Location = New System.Drawing.Point(149, 35)
		Me.UsernameTextBox.Name = "UsernameTextBox"
		Me.UsernameTextBox.Size = New System.Drawing.Size(244, 20)
		Me.UsernameTextBox.TabIndex = 1
		'
		'OkButton
		'
		Me.OkButton.Location = New System.Drawing.Point(170, 99)
		Me.OkButton.Name = "OkButton"
		Me.OkButton.Size = New System.Drawing.Size(75, 23)
		Me.OkButton.TabIndex = 2
		Me.OkButton.Text = "&Ok"
		Me.OkButton.UseVisualStyleBackColor = True
		'
		'Label2
		'
		Me.Label2.AutoSize = True
		Me.Label2.Location = New System.Drawing.Point(85, 67)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(56, 13)
		Me.Label2.TabIndex = 0
		Me.Label2.Text = "Password:"
		'
		'PasswordTextBox
		'
		Me.PasswordTextBox.Location = New System.Drawing.Point(149, 64)
		Me.PasswordTextBox.Name = "PasswordTextBox"
		Me.PasswordTextBox.Size = New System.Drawing.Size(244, 20)
		Me.PasswordTextBox.TabIndex = 1
		'
		'PictureBox1
		'
		Me.PictureBox1.Image = Global.DocScript.My.Resources.Resources.Keyword
		Me.PictureBox1.Location = New System.Drawing.Point(12, 24)
		Me.PictureBox1.Name = "PictureBox1"
		Me.PictureBox1.Size = New System.Drawing.Size(65, 75)
		Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
		Me.PictureBox1.TabIndex = 3
		Me.PictureBox1.TabStop = False
		'
		'Label3
		'
		Me.Label3.AutoSize = True
		Me.Label3.Location = New System.Drawing.Point(12, 8)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(326, 13)
		Me.Label3.TabIndex = 0
		Me.Label3.Text = "Specify the credentials with which to make the remote connection..."
		'
		'EnterCredentialsDialog
		'
		Me.AcceptButton = Me.OkButton
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(414, 132)
		Me.ControlBox = False
		Me.Controls.Add(Me.PictureBox1)
		Me.Controls.Add(Me.OkButton)
		Me.Controls.Add(Me.PasswordTextBox)
		Me.Controls.Add(Me.UsernameTextBox)
		Me.Controls.Add(Me.Label2)
		Me.Controls.Add(Me.Label3)
		Me.Controls.Add(Me.Label1)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "EnterCredentialsDialog"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Enter Credentials"
		CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents Label1 As System.Windows.Forms.Label
	Friend WithEvents UsernameTextBox As System.Windows.Forms.TextBox
	Friend WithEvents OkButton As System.Windows.Forms.Button
	Friend WithEvents Label2 As System.Windows.Forms.Label
	Friend WithEvents PasswordTextBox As System.Windows.Forms.TextBox
	Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
	Friend WithEvents Label3 As System.Windows.Forms.Label
End Class
