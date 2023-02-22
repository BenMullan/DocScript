<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ParserExperiments
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
		Me.SourceTextBox = New System.Windows.Forms.TextBox()
		Me.TokensTextBox = New System.Windows.Forms.TextBox()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.ParseButton = New System.Windows.Forms.Button()
		Me.Button1 = New System.Windows.Forms.Button()
		Me.TokenTypesTextBox = New System.Windows.Forms.TextBox()
		Me.Label3 = New System.Windows.Forms.Label()
		Me.Label4 = New System.Windows.Forms.Label()
		Me.DseFormChooser1 = New DSExperimentation.DSEFormChooser()
		Me.SuspendLayout()
		'
		'SourceTextBox
		'
		Me.SourceTextBox.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.SourceTextBox.Location = New System.Drawing.Point(12, 35)
		Me.SourceTextBox.Multiline = True
		Me.SourceTextBox.Name = "SourceTextBox"
		Me.SourceTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
		Me.SourceTextBox.Size = New System.Drawing.Size(240, 429)
		Me.SourceTextBox.TabIndex = 0
		Me.SourceTextBox.Text = "#HelloWorld.DS" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Function <Void> Main ()" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    Output(""Hello, World!"")" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    Return" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "EndFunction"
		'
		'TokensTextBox
		'
		Me.TokensTextBox.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.TokensTextBox.Location = New System.Drawing.Point(285, 35)
		Me.TokensTextBox.Multiline = True
		Me.TokensTextBox.Name = "TokensTextBox"
		Me.TokensTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
		Me.TokensTextBox.Size = New System.Drawing.Size(180, 429)
		Me.TokensTextBox.TabIndex = 0
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Location = New System.Drawing.Point(84, 13)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(44, 13)
		Me.Label1.TabIndex = 1
		Me.Label1.Text = "Source:"
		'
		'Label2
		'
		Me.Label2.AutoSize = True
		Me.Label2.Location = New System.Drawing.Point(352, 13)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(46, 13)
		Me.Label2.TabIndex = 1
		Me.Label2.Text = "Tokens:"
		'
		'ParseButton
		'
		Me.ParseButton.Location = New System.Drawing.Point(234, 488)
		Me.ParseButton.Name = "ParseButton"
		Me.ParseButton.Size = New System.Drawing.Size(75, 23)
		Me.ParseButton.TabIndex = 2
		Me.ParseButton.Text = "Parse"
		Me.ParseButton.UseVisualStyleBackColor = True
		'
		'Button1
		'
		Me.Button1.Location = New System.Drawing.Point(12, 488)
		Me.Button1.Name = "Button1"
		Me.Button1.Size = New System.Drawing.Size(75, 23)
		Me.Button1.TabIndex = 3
		Me.Button1.Text = "Serialise"
		Me.Button1.UseVisualStyleBackColor = True
		'
		'TokenTypesTextBox
		'
		Me.TokenTypesTextBox.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.TokenTypesTextBox.Location = New System.Drawing.Point(471, 35)
		Me.TokenTypesTextBox.Multiline = True
		Me.TokenTypesTextBox.Name = "TokenTypesTextBox"
		Me.TokenTypesTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
		Me.TokenTypesTextBox.Size = New System.Drawing.Size(161, 429)
		Me.TokenTypesTextBox.TabIndex = 0
		'
		'Label3
		'
		Me.Label3.AutoSize = True
		Me.Label3.Location = New System.Drawing.Point(510, 13)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(73, 13)
		Me.Label3.TabIndex = 1
		Me.Label3.Text = "Token Types:"
		'
		'Label4
		'
		Me.Label4.AutoSize = True
		Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label4.Location = New System.Drawing.Point(253, 225)
		Me.Label4.Name = "Label4"
		Me.Label4.Size = New System.Drawing.Size(34, 25)
		Me.Label4.TabIndex = 1
		Me.Label4.Text = "→"
		'
		'DseFormChooser1
		'
		Me.DseFormChooser1.AutoScroll = True
		Me.DseFormChooser1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
		Me.DseFormChooser1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.DseFormChooser1.Location = New System.Drawing.Point(638, 12)
		Me.DseFormChooser1.Name = "DseFormChooser1"
		Me.DseFormChooser1.Size = New System.Drawing.Size(146, 146)
		Me.DseFormChooser1.TabIndex = 4
		'
		'ParserExperiments
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(793, 523)
		Me.Controls.Add(Me.DseFormChooser1)
		Me.Controls.Add(Me.Button1)
		Me.Controls.Add(Me.ParseButton)
		Me.Controls.Add(Me.Label3)
		Me.Controls.Add(Me.Label2)
		Me.Controls.Add(Me.Label1)
		Me.Controls.Add(Me.TokenTypesTextBox)
		Me.Controls.Add(Me.TokensTextBox)
		Me.Controls.Add(Me.SourceTextBox)
		Me.Controls.Add(Me.Label4)
		Me.Name = "ParserExperiments"
		Me.Text = "ParserExperiments"
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents SourceTextBox As System.Windows.Forms.TextBox
	Friend WithEvents TokensTextBox As System.Windows.Forms.TextBox
	Friend WithEvents Label1 As System.Windows.Forms.Label
	Friend WithEvents Label2 As System.Windows.Forms.Label
	Friend WithEvents ParseButton As System.Windows.Forms.Button
	Friend WithEvents Button1 As System.Windows.Forms.Button
	Friend WithEvents TokenTypesTextBox As System.Windows.Forms.TextBox
	Friend WithEvents Label3 As System.Windows.Forms.Label
	Friend WithEvents Label4 As System.Windows.Forms.Label
	Friend WithEvents DseFormChooser1 As DSExperimentation.DSEFormChooser
End Class
