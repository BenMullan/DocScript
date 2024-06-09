<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ExpressionExperiments
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
		Me.InputExprTextBox = New System.Windows.Forms.TextBox()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.OutputLabel = New System.Windows.Forms.Label()
		Me.ResolveButton = New System.Windows.Forms.Button()
		Me.OutputTextBox = New System.Windows.Forms.TextBox()
		Me.DseFormChooser1 = New DSExperimentation.DSEFormChooser()
		Me.StringToNumberCoercionButton = New System.Windows.Forms.Button()
		Me.SuspendLayout()
		'
		'InputExprTextBox
		'
		Me.InputExprTextBox.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.InputExprTextBox.Location = New System.Drawing.Point(106, 6)
		Me.InputExprTextBox.Name = "InputExprTextBox"
		Me.InputExprTextBox.Size = New System.Drawing.Size(493, 23)
		Me.InputExprTextBox.TabIndex = 0
		Me.InputExprTextBox.Text = "[4 + 5] - 9"
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Location = New System.Drawing.Point(12, 9)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(88, 13)
		Me.Label1.TabIndex = 1
		Me.Label1.Text = "Input Expression:"
		'
		'OutputLabel
		'
		Me.OutputLabel.AutoSize = True
		Me.OutputLabel.Location = New System.Drawing.Point(12, 59)
		Me.OutputLabel.Name = "OutputLabel"
		Me.OutputLabel.Size = New System.Drawing.Size(42, 13)
		Me.OutputLabel.TabIndex = 2
		Me.OutputLabel.Text = "Output:"
		'
		'ResolveButton
		'
		Me.ResolveButton.Location = New System.Drawing.Point(321, 39)
		Me.ResolveButton.Name = "ResolveButton"
		Me.ResolveButton.Size = New System.Drawing.Size(75, 23)
		Me.ResolveButton.TabIndex = 3
		Me.ResolveButton.Text = "Resolve"
		Me.ResolveButton.UseVisualStyleBackColor = True
		'
		'OutputTextBox
		'
		Me.OutputTextBox.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.OutputTextBox.Location = New System.Drawing.Point(12, 75)
		Me.OutputTextBox.Multiline = True
		Me.OutputTextBox.Name = "OutputTextBox"
		Me.OutputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
		Me.OutputTextBox.Size = New System.Drawing.Size(587, 304)
		Me.OutputTextBox.TabIndex = 4
		'
		'DseFormChooser1
		'
		Me.DseFormChooser1.AutoScroll = True
		Me.DseFormChooser1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
		Me.DseFormChooser1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.DseFormChooser1.Location = New System.Drawing.Point(605, 6)
		Me.DseFormChooser1.Name = "DseFormChooser1"
		Me.DseFormChooser1.Size = New System.Drawing.Size(146, 146)
		Me.DseFormChooser1.TabIndex = 5
		'
		'StringToNumberCoercionButton
		'
		Me.StringToNumberCoercionButton.Location = New System.Drawing.Point(605, 167)
		Me.StringToNumberCoercionButton.Name = "StringToNumberCoercionButton"
		Me.StringToNumberCoercionButton.Size = New System.Drawing.Size(144, 23)
		Me.StringToNumberCoercionButton.TabIndex = 6
		Me.StringToNumberCoercionButton.Text = "StringToNumberCoercion"
		Me.StringToNumberCoercionButton.UseVisualStyleBackColor = True
		'
		'ExpressionExperiments
		'
		Me.AcceptButton = Me.ResolveButton
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(761, 391)
		Me.Controls.Add(Me.StringToNumberCoercionButton)
		Me.Controls.Add(Me.DseFormChooser1)
		Me.Controls.Add(Me.OutputTextBox)
		Me.Controls.Add(Me.ResolveButton)
		Me.Controls.Add(Me.OutputLabel)
		Me.Controls.Add(Me.Label1)
		Me.Controls.Add(Me.InputExprTextBox)
		Me.Name = "ExpressionExperiments"
		Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "ExpressionExperiments"
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents InputExprTextBox As System.Windows.Forms.TextBox
	Friend WithEvents Label1 As System.Windows.Forms.Label
	Friend WithEvents OutputLabel As System.Windows.Forms.Label
	Friend WithEvents ResolveButton As System.Windows.Forms.Button
	Friend WithEvents OutputTextBox As System.Windows.Forms.TextBox
	Friend WithEvents DseFormChooser1 As DSExperimentation.DSEFormChooser
	Friend WithEvents StringToNumberCoercionButton As System.Windows.Forms.Button
End Class
