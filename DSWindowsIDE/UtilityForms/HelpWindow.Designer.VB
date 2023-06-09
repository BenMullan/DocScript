<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class HelpWindow
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(HelpWindow))
		Me.Label1 = New System.Windows.Forms.Label()
		Me.GroupBox1 = New System.Windows.Forms.GroupBox()
		Me.GroupBox2 = New System.Windows.Forms.GroupBox()
		Me.StartPictorialHelp = New System.Windows.Forms.PictureBox()
		Me.GroupBox1.SuspendLayout()
		Me.GroupBox2.SuspendLayout()
		CType(Me.StartPictorialHelp, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label1.Location = New System.Drawing.Point(8, 22)
		Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(526, 221)
		Me.Label1.TabIndex = 0
		Me.Label1.Text = resources.GetString("Label1.Text")
		'
		'GroupBox1
		'
		Me.GroupBox1.Controls.Add(Me.Label1)
		Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.GroupBox1.ForeColor = System.Drawing.SystemColors.InactiveCaptionText
		Me.GroupBox1.Location = New System.Drawing.Point(13, 174)
		Me.GroupBox1.Margin = New System.Windows.Forms.Padding(4)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Padding = New System.Windows.Forms.Padding(4)
		Me.GroupBox1.Size = New System.Drawing.Size(543, 255)
		Me.GroupBox1.TabIndex = 2
		Me.GroupBox1.TabStop = False
		Me.GroupBox1.Text = "General"
		'
		'GroupBox2
		'
		Me.GroupBox2.Controls.Add(Me.StartPictorialHelp)
		Me.GroupBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.GroupBox2.ForeColor = System.Drawing.SystemColors.InactiveCaptionText
		Me.GroupBox2.Location = New System.Drawing.Point(13, 13)
		Me.GroupBox2.Margin = New System.Windows.Forms.Padding(4)
		Me.GroupBox2.Name = "GroupBox2"
		Me.GroupBox2.Padding = New System.Windows.Forms.Padding(4)
		Me.GroupBox2.Size = New System.Drawing.Size(543, 153)
		Me.GroupBox2.TabIndex = 3
		Me.GroupBox2.TabStop = False
		Me.GroupBox2.Text = "Pictorial Help"
		'
		'StartPictorialHelp
		'
		Me.StartPictorialHelp.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
		Me.StartPictorialHelp.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.StartPictorialHelp.Cursor = System.Windows.Forms.Cursors.Hand
		Me.StartPictorialHelp.Image = Global.DocScript.My.Resources.Resources.StartPictorialHelp
		Me.StartPictorialHelp.Location = New System.Drawing.Point(25, 34)
		Me.StartPictorialHelp.Margin = New System.Windows.Forms.Padding(4)
		Me.StartPictorialHelp.Name = "StartPictorialHelp"
		Me.StartPictorialHelp.Size = New System.Drawing.Size(493, 99)
		Me.StartPictorialHelp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
		Me.StartPictorialHelp.TabIndex = 1
		Me.StartPictorialHelp.TabStop = False
		'
		'HelpWindow
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(564, 444)
		Me.Controls.Add(Me.GroupBox2)
		Me.Controls.Add(Me.GroupBox1)
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.ForeColor = System.Drawing.SystemColors.InactiveCaptionText
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.Margin = New System.Windows.Forms.Padding(4)
		Me.MaximizeBox = False
		Me.MaximumSize = New System.Drawing.Size(580, 500)
		Me.MinimizeBox = False
		Me.MinimumSize = New System.Drawing.Size(395, 340)
		Me.Name = "HelpWindow"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "DocScript IDE: Help!"
		Me.GroupBox1.ResumeLayout(False)
		Me.GroupBox1.PerformLayout()
		Me.GroupBox2.ResumeLayout(False)
		CType(Me.StartPictorialHelp, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents Label1 As System.Windows.Forms.Label
	Friend WithEvents StartPictorialHelp As System.Windows.Forms.PictureBox
	Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
	Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
End Class
