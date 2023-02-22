<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ResolveExprForm
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ResolveExprForm))
		Me.InputExprLabel = New System.Windows.Forms.Label()
		Me.ExprResultLabel = New System.Windows.Forms.Label()
		Me.OkButton = New System.Windows.Forms.Button()
		Me.SuspendLayout()
		'
		'InputExprLabel
		'
		Me.InputExprLabel.AutoSize = True
		Me.InputExprLabel.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.InputExprLabel.Location = New System.Drawing.Point(5, 6)
		Me.InputExprLabel.Name = "InputExprLabel"
		Me.InputExprLabel.Size = New System.Drawing.Size(85, 13)
		Me.InputExprLabel.TabIndex = 0
		Me.InputExprLabel.Text = "{Input Expr.}"
		'
		'ExprResultLabel
		'
		Me.ExprResultLabel.AutoSize = True
		Me.ExprResultLabel.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(197, Byte), Integer))
		Me.ExprResultLabel.Font = New System.Drawing.Font("Consolas", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.ExprResultLabel.Location = New System.Drawing.Point(100, 34)
		Me.ExprResultLabel.Name = "ExprResultLabel"
		Me.ExprResultLabel.Size = New System.Drawing.Size(154, 24)
		Me.ExprResultLabel.TabIndex = 0
		Me.ExprResultLabel.Text = "(Unresolved)"
		'
		'OkButton
		'
		Me.OkButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.OkButton.Location = New System.Drawing.Point(3, 67)
		Me.OkButton.Name = "OkButton"
		Me.OkButton.Size = New System.Drawing.Size(39, 23)
		Me.OkButton.TabIndex = 1
		Me.OkButton.Text = "&Ok"
		Me.OkButton.UseVisualStyleBackColor = True
		'
		'ResolveExprForm
		'
		Me.AcceptButton = Me.OkButton
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(354, 92)
		Me.Controls.Add(Me.OkButton)
		Me.Controls.Add(Me.ExprResultLabel)
		Me.Controls.Add(Me.InputExprLabel)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.MaximumSize = New System.Drawing.Size(90000, 130)
		Me.MinimizeBox = False
		Me.MinimumSize = New System.Drawing.Size(370, 130)
		Me.Name = "ResolveExprForm"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Resolved Expr."
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents InputExprLabel As System.Windows.Forms.Label
	Friend WithEvents ExprResultLabel As System.Windows.Forms.Label
	Friend WithEvents OkButton As System.Windows.Forms.Button
End Class
