<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DSEFormChooser
	Inherits System.Windows.Forms.UserControl

	'UserControl overrides dispose to clean up the component list.
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
		Me.ExprExpButton = New System.Windows.Forms.Button()
		Me.ParserExpButton = New System.Windows.Forms.Button()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.ProgConstructionButton = New System.Windows.Forms.Button()
		Me.InstructionTreesButton = New System.Windows.Forms.Button()
		Me.SQLExperimentsButton = New System.Windows.Forms.Button()
		Me.BIFExperimentsButton = New System.Windows.Forms.Button()
		Me.SuspendLayout()
		'
		'ExprExpButton
		'
		Me.ExprExpButton.BackColor = System.Drawing.Color.LightGray
		Me.ExprExpButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText
		Me.ExprExpButton.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ButtonShadow
		Me.ExprExpButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight
		Me.ExprExpButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
		Me.ExprExpButton.Location = New System.Drawing.Point(8, 33)
		Me.ExprExpButton.Name = "ExprExpButton"
		Me.ExprExpButton.Size = New System.Drawing.Size(131, 23)
		Me.ExprExpButton.TabIndex = 0
		Me.ExprExpButton.Text = "Expression Experiments"
		Me.ExprExpButton.UseVisualStyleBackColor = False
		'
		'ParserExpButton
		'
		Me.ParserExpButton.BackColor = System.Drawing.Color.LightGray
		Me.ParserExpButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText
		Me.ParserExpButton.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ButtonShadow
		Me.ParserExpButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight
		Me.ParserExpButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
		Me.ParserExpButton.Location = New System.Drawing.Point(8, 62)
		Me.ParserExpButton.Name = "ParserExpButton"
		Me.ParserExpButton.Size = New System.Drawing.Size(131, 23)
		Me.ParserExpButton.TabIndex = 1
		Me.ParserExpButton.Text = "Parser Experiments"
		Me.ParserExpButton.UseVisualStyleBackColor = False
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label1.Location = New System.Drawing.Point(36, 11)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(78, 16)
		Me.Label1.TabIndex = 2
		Me.Label1.Text = "DSE Form"
		'
		'ProgConstructionButton
		'
		Me.ProgConstructionButton.BackColor = System.Drawing.Color.LightGray
		Me.ProgConstructionButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText
		Me.ProgConstructionButton.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ButtonShadow
		Me.ProgConstructionButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight
		Me.ProgConstructionButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
		Me.ProgConstructionButton.Location = New System.Drawing.Point(8, 91)
		Me.ProgConstructionButton.Name = "ProgConstructionButton"
		Me.ProgConstructionButton.Size = New System.Drawing.Size(131, 23)
		Me.ProgConstructionButton.TabIndex = 1
		Me.ProgConstructionButton.Text = "Program Construction"
		Me.ProgConstructionButton.UseVisualStyleBackColor = False
		'
		'InstructionTreesButton
		'
		Me.InstructionTreesButton.BackColor = System.Drawing.Color.LightGray
		Me.InstructionTreesButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText
		Me.InstructionTreesButton.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ButtonShadow
		Me.InstructionTreesButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight
		Me.InstructionTreesButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
		Me.InstructionTreesButton.Location = New System.Drawing.Point(8, 120)
		Me.InstructionTreesButton.Name = "InstructionTreesButton"
		Me.InstructionTreesButton.Size = New System.Drawing.Size(131, 23)
		Me.InstructionTreesButton.TabIndex = 3
		Me.InstructionTreesButton.Text = "Instruction Trees"
		Me.InstructionTreesButton.UseVisualStyleBackColor = False
		'
		'SQLExperimentsButton
		'
		Me.SQLExperimentsButton.BackColor = System.Drawing.Color.LightGray
		Me.SQLExperimentsButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText
		Me.SQLExperimentsButton.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ButtonShadow
		Me.SQLExperimentsButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight
		Me.SQLExperimentsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
		Me.SQLExperimentsButton.Location = New System.Drawing.Point(8, 149)
		Me.SQLExperimentsButton.Name = "SQLExperimentsButton"
		Me.SQLExperimentsButton.Size = New System.Drawing.Size(131, 23)
		Me.SQLExperimentsButton.TabIndex = 4
		Me.SQLExperimentsButton.Text = "SQL Experiments"
		Me.SQLExperimentsButton.UseVisualStyleBackColor = False
		'
		'BIFExperimentsButton
		'
		Me.BIFExperimentsButton.BackColor = System.Drawing.Color.LightGray
		Me.BIFExperimentsButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText
		Me.BIFExperimentsButton.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ButtonShadow
		Me.BIFExperimentsButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight
		Me.BIFExperimentsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
		Me.BIFExperimentsButton.Location = New System.Drawing.Point(8, 178)
		Me.BIFExperimentsButton.Name = "BIFExperimentsButton"
		Me.BIFExperimentsButton.Size = New System.Drawing.Size(131, 23)
		Me.BIFExperimentsButton.TabIndex = 5
		Me.BIFExperimentsButton.Text = "BIF Experiments"
		Me.BIFExperimentsButton.UseVisualStyleBackColor = False
		'
		'DSEFormChooser
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.AutoScroll = True
		Me.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
		Me.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.Controls.Add(Me.BIFExperimentsButton)
		Me.Controls.Add(Me.SQLExperimentsButton)
		Me.Controls.Add(Me.InstructionTreesButton)
		Me.Controls.Add(Me.Label1)
		Me.Controls.Add(Me.ProgConstructionButton)
		Me.Controls.Add(Me.ParserExpButton)
		Me.Controls.Add(Me.ExprExpButton)
		Me.Name = "DSEFormChooser"
		Me.Size = New System.Drawing.Size(150, 266)
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents ExprExpButton As System.Windows.Forms.Button
	Friend WithEvents ParserExpButton As System.Windows.Forms.Button
	Friend WithEvents Label1 As System.Windows.Forms.Label
	Friend WithEvents ProgConstructionButton As System.Windows.Forms.Button
	Friend WithEvents InstructionTreesButton As System.Windows.Forms.Button
	Friend WithEvents SQLExperimentsButton As System.Windows.Forms.Button
	Friend WithEvents BIFExperimentsButton As System.Windows.Forms.Button

End Class
