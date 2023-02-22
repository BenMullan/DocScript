<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ViewExprXMLForm
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
		Me.ExprXMLTextBox = New System.Windows.Forms.TextBox()
		Me.SuspendLayout()
		'
		'ExprXMLTextBox
		'
		Me.ExprXMLTextBox.Dock = System.Windows.Forms.DockStyle.Fill
		Me.ExprXMLTextBox.Location = New System.Drawing.Point(0, 0)
		Me.ExprXMLTextBox.Multiline = True
		Me.ExprXMLTextBox.Name = "ExprXMLTextBox"
		Me.ExprXMLTextBox.ReadOnly = True
		Me.ExprXMLTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
		Me.ExprXMLTextBox.Size = New System.Drawing.Size(504, 342)
		Me.ExprXMLTextBox.TabIndex = 0
		'
		'ViewExprXMLForm
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(504, 342)
		Me.Controls.Add(Me.ExprXMLTextBox)
		Me.Name = "ViewExprXMLForm"
		Me.ShowIcon = False
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "View Expression XML"
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents ExprXMLTextBox As System.Windows.Forms.TextBox
End Class
