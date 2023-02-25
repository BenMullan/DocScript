<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BIFExperiments
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BIFExperiments))
		Me.InputTestButton = New System.Windows.Forms.Button()
		Me.Button1 = New System.Windows.Forms.Button()
		Me.DseFormChooser1 = New DSExperimentation.DSEFormChooser()
		Me.Button2 = New System.Windows.Forms.Button()
		Me.Button3 = New System.Windows.Forms.Button()
		Me.SuspendLayout()
		'
		'InputTestButton
		'
		Me.InputTestButton.Location = New System.Drawing.Point(168, 12)
		Me.InputTestButton.Name = "InputTestButton"
		Me.InputTestButton.Size = New System.Drawing.Size(168, 23)
		Me.InputTestButton.TabIndex = 1
		Me.InputTestButton.Text = "Input(""Enter your Name:"")"
		Me.InputTestButton.UseVisualStyleBackColor = True
		'
		'Button1
		'
		Me.Button1.Location = New System.Drawing.Point(168, 41)
		Me.Button1.Name = "Button1"
		Me.Button1.Size = New System.Drawing.Size(123, 23)
		Me.Button1.TabIndex = 2
		Me.Button1.Text = "LambdaBIF.Identifier"
		Me.Button1.UseVisualStyleBackColor = True
		'
		'DseFormChooser1
		'
		Me.DseFormChooser1.AutoScroll = True
		Me.DseFormChooser1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
		Me.DseFormChooser1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.DseFormChooser1.Location = New System.Drawing.Point(12, 12)
		Me.DseFormChooser1.Name = "DseFormChooser1"
		Me.DseFormChooser1.Size = New System.Drawing.Size(150, 224)
		Me.DseFormChooser1.TabIndex = 0
		'
		'Button2
		'
		Me.Button2.Location = New System.Drawing.Point(297, 41)
		Me.Button2.Name = "Button2"
		Me.Button2.Size = New System.Drawing.Size(107, 23)
		Me.Button2.TabIndex = 3
		Me.Button2.Text = "ExeRes Is Nothing"
		Me.Button2.UseVisualStyleBackColor = True
		'
		'Button3
		'
		Me.Button3.Location = New System.Drawing.Point(207, 142)
		Me.Button3.Name = "Button3"
		Me.Button3.Size = New System.Drawing.Size(188, 23)
		Me.Button3.TabIndex = 4
		Me.Button3.Text = "WindowsEventLog Test"
		Me.Button3.UseVisualStyleBackColor = True
		'
		'BIFExperiments
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(660, 247)
		Me.Controls.Add(Me.Button3)
		Me.Controls.Add(Me.Button2)
		Me.Controls.Add(Me.Button1)
		Me.Controls.Add(Me.InputTestButton)
		Me.Controls.Add(Me.DseFormChooser1)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.Name = "BIFExperiments"
		Me.Text = "BuiltInFunction Experiments"
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents DseFormChooser1 As DSExperimentation.DSEFormChooser
	Friend WithEvents InputTestButton As System.Windows.Forms.Button
	Friend WithEvents Button1 As System.Windows.Forms.Button
	Friend WithEvents Button2 As System.Windows.Forms.Button
	Friend WithEvents Button3 As System.Windows.Forms.Button
End Class
