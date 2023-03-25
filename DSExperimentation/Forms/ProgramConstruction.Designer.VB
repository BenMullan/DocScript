<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ProgramConstruction
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
		Dim TreeNode1 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Global Variable Declarations")
		Dim TreeNode2 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Functions")
		Dim TreeNode3 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Program", New System.Windows.Forms.TreeNode() {TreeNode1, TreeNode2})
		Me.SourceTextBox = New System.Windows.Forms.TextBox()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.ConstructProgButton = New System.Windows.Forms.Button()
		Me.ProgramTreeView = New System.Windows.Forms.TreeView()
		Me.Button1 = New System.Windows.Forms.Button()
		Me.ProgramXMLTextBox = New System.Windows.Forms.TextBox()
		Me.Button2 = New System.Windows.Forms.Button()
		Me.Button3 = New System.Windows.Forms.Button()
		Me.GSSTButton = New System.Windows.Forms.Button()
		Me.ROTButton = New System.Windows.Forms.Button()
		Me.TIBButton = New System.Windows.Forms.Button()
		Me.VAButton = New System.Windows.Forms.Button()
		Me.RTCButton = New System.Windows.Forms.Button()
		Me.FCButton = New System.Windows.Forms.Button()
		Me.IOLWButton = New System.Windows.Forms.Button()
		Me.TheDseFormChooser = New DSExperimentation.DSEFormChooser()
		Me.SSFRTButton = New System.Windows.Forms.Button()
		Me.Button4 = New System.Windows.Forms.Button()
		Me.SuspendLayout()
		'
		'SourceTextBox
		'
		Me.SourceTextBox.AutoCompleteCustomSource.AddRange(New String() {"Function", "If", "While", "Loop", "Else", "Return", "String", "Number", "Boolean"})
		Me.SourceTextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
		Me.SourceTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
		Me.SourceTextBox.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.SourceTextBox.Location = New System.Drawing.Point(12, 25)
		Me.SourceTextBox.Multiline = True
		Me.SourceTextBox.Name = "SourceTextBox"
		Me.SourceTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
		Me.SourceTextBox.Size = New System.Drawing.Size(533, 146)
		Me.SourceTextBox.TabIndex = 0
		Me.SourceTextBox.Text = "#TestProgram.DS" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "<String> Name : ""Ben"" & "" "" & ""Mullan""" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "<Number@> Ages" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Functi" &
	"on <String> GetName ()" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "EndFunction"
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Location = New System.Drawing.Point(231, 7)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(94, 13)
		Me.Label1.TabIndex = 1
		Me.Label1.Text = "DocScript Source:"
		'
		'ConstructProgButton
		'
		Me.ConstructProgButton.Location = New System.Drawing.Point(551, 243)
		Me.ConstructProgButton.Name = "ConstructProgButton"
		Me.ConstructProgButton.Size = New System.Drawing.Size(158, 23)
		Me.ConstructProgButton.TabIndex = 3
		Me.ConstructProgButton.Text = "&Construct Program"
		Me.ConstructProgButton.UseVisualStyleBackColor = True
		'
		'ProgramTreeView
		'
		Me.ProgramTreeView.Location = New System.Drawing.Point(12, 177)
		Me.ProgramTreeView.Name = "ProgramTreeView"
		TreeNode1.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
		TreeNode1.Name = "GlobalVarDecs"
		TreeNode1.Text = "Global Variable Declarations"
		TreeNode2.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
		TreeNode2.Name = "Functions"
		TreeNode2.Text = "Functions"
		TreeNode3.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
		TreeNode3.Checked = True
		TreeNode3.Name = "Program"
		TreeNode3.Text = "Program"
		TreeNode3.ToolTipText = "The Root of the DocScript Program"
		Me.ProgramTreeView.Nodes.AddRange(New System.Windows.Forms.TreeNode() {TreeNode3})
		Me.ProgramTreeView.Size = New System.Drawing.Size(533, 152)
		Me.ProgramTreeView.TabIndex = 4
		'
		'Button1
		'
		Me.Button1.Location = New System.Drawing.Point(614, 451)
		Me.Button1.Name = "Button1"
		Me.Button1.Size = New System.Drawing.Size(95, 23)
		Me.Button1.TabIndex = 5
		Me.Button1.Text = "< IsSatisfiedBy()"
		Me.Button1.UseVisualStyleBackColor = True
		'
		'ProgramXMLTextBox
		'
		Me.ProgramXMLTextBox.Location = New System.Drawing.Point(12, 335)
		Me.ProgramXMLTextBox.Multiline = True
		Me.ProgramXMLTextBox.Name = "ProgramXMLTextBox"
		Me.ProgramXMLTextBox.ReadOnly = True
		Me.ProgramXMLTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
		Me.ProgramXMLTextBox.Size = New System.Drawing.Size(533, 139)
		Me.ProgramXMLTextBox.TabIndex = 6
		Me.ProgramXMLTextBox.Text = "(No Program Constructed)"
		'
		'Button2
		'
		Me.Button2.Location = New System.Drawing.Point(551, 422)
		Me.Button2.Name = "Button2"
		Me.Button2.Size = New System.Drawing.Size(57, 52)
		Me.Button2.TabIndex = 7
		Me.Button2.Text = "SplitWhere()"
		Me.Button2.UseVisualStyleBackColor = True
		'
		'Button3
		'
		Me.Button3.Location = New System.Drawing.Point(614, 422)
		Me.Button3.Name = "Button3"
		Me.Button3.Size = New System.Drawing.Size(95, 23)
		Me.Button3.TabIndex = 8
		Me.Button3.Text = "BalencedFiltering"
		Me.Button3.UseVisualStyleBackColor = True
		'
		'GSSTButton
		'
		Me.GSSTButton.Location = New System.Drawing.Point(551, 393)
		Me.GSSTButton.Name = "GSSTButton"
		Me.GSSTButton.Size = New System.Drawing.Size(158, 23)
		Me.GSSTButton.TabIndex = 9
		Me.GSSTButton.Text = "GetSubStatementTokens"
		Me.GSSTButton.UseVisualStyleBackColor = True
		'
		'ROTButton
		'
		Me.ROTButton.Location = New System.Drawing.Point(551, 364)
		Me.ROTButton.Name = "ROTButton"
		Me.ROTButton.Size = New System.Drawing.Size(74, 23)
		Me.ROTButton.TabIndex = 10
		Me.ROTButton.Text = "Repeat(Of T)"
		Me.ROTButton.UseVisualStyleBackColor = True
		'
		'TIBButton
		'
		Me.TIBButton.Location = New System.Drawing.Point(631, 364)
		Me.TIBButton.Name = "TIBButton"
		Me.TIBButton.Size = New System.Drawing.Size(78, 23)
		Me.TIBButton.TabIndex = 11
		Me.TIBButton.Text = "TabbedInBy"
		Me.TIBButton.UseVisualStyleBackColor = True
		'
		'VAButton
		'
		Me.VAButton.BackgroundImage = Global.DSExperimentation.My.Resources.Resources._31082022_Example_ExprTree
		Me.VAButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
		Me.VAButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup
		Me.VAButton.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
		Me.VAButton.Image = Global.DSExperimentation.My.Resources.Resources._31082022_Example_ExprTree
		Me.VAButton.ImageAlign = System.Drawing.ContentAlignment.BottomLeft
		Me.VAButton.Location = New System.Drawing.Point(551, 335)
		Me.VAButton.Name = "VAButton"
		Me.VAButton.Size = New System.Drawing.Size(75, 23)
		Me.VAButton.TabIndex = 12
		Me.VAButton.Text = "VarAssign"
		Me.VAButton.UseVisualStyleBackColor = True
		'
		'RTCButton
		'
		Me.RTCButton.BackgroundImage = Global.DSExperimentation.My.Resources.Resources._31082022_Example_ExprTree
		Me.RTCButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
		Me.RTCButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup
		Me.RTCButton.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
		Me.RTCButton.Image = Global.DSExperimentation.My.Resources.Resources._31082022_Example_ExprTree
		Me.RTCButton.Location = New System.Drawing.Point(634, 335)
		Me.RTCButton.Name = "RTCButton"
		Me.RTCButton.Size = New System.Drawing.Size(75, 23)
		Me.RTCButton.TabIndex = 13
		Me.RTCButton.Text = "Return"
		Me.RTCButton.UseVisualStyleBackColor = True
		'
		'FCButton
		'
		Me.FCButton.BackgroundImage = Global.DSExperimentation.My.Resources.Resources._31082022_Example_ExprTree
		Me.FCButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
		Me.FCButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup
		Me.FCButton.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
		Me.FCButton.Image = Global.DSExperimentation.My.Resources.Resources._31082022_Example_ExprTree
		Me.FCButton.ImageAlign = System.Drawing.ContentAlignment.BottomRight
		Me.FCButton.Location = New System.Drawing.Point(551, 306)
		Me.FCButton.Name = "FCButton"
		Me.FCButton.Size = New System.Drawing.Size(75, 23)
		Me.FCButton.TabIndex = 14
		Me.FCButton.Text = "FuncCall"
		Me.FCButton.UseVisualStyleBackColor = True
		'
		'IOLWButton
		'
		Me.IOLWButton.Location = New System.Drawing.Point(551, 272)
		Me.IOLWButton.Name = "IOLWButton"
		Me.IOLWButton.Size = New System.Drawing.Size(37, 23)
		Me.IOLWButton.TabIndex = 15
		Me.IOLWButton.Text = "IOLW"
		Me.IOLWButton.UseVisualStyleBackColor = True
		'
		'TheDseFormChooser
		'
		Me.TheDseFormChooser.AutoScroll = True
		Me.TheDseFormChooser.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
		Me.TheDseFormChooser.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.TheDseFormChooser.Location = New System.Drawing.Point(551, 25)
		Me.TheDseFormChooser.Name = "TheDseFormChooser"
		Me.TheDseFormChooser.Size = New System.Drawing.Size(158, 212)
		Me.TheDseFormChooser.TabIndex = 2
		'
		'SSFRTButton
		'
		Me.SSFRTButton.Location = New System.Drawing.Point(594, 272)
		Me.SSFRTButton.Name = "SSFRTButton"
		Me.SSFRTButton.Size = New System.Drawing.Size(115, 23)
		Me.SSFRTButton.TabIndex = 16
		Me.SSFRTButton.Text = "SubStaFromRemaining"
		Me.SSFRTButton.UseVisualStyleBackColor = True
		'
		'Button4
		'
		Me.Button4.Location = New System.Drawing.Point(662, 301)
		Me.Button4.Name = "Button4"
		Me.Button4.Size = New System.Drawing.Size(47, 23)
		Me.Button4.TabIndex = 15
		Me.Button4.Text = "Dialog"
		Me.Button4.UseVisualStyleBackColor = True
		'
		'ProgramConstruction
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(721, 486)
		Me.Controls.Add(Me.SSFRTButton)
		Me.Controls.Add(Me.Button4)
		Me.Controls.Add(Me.IOLWButton)
		Me.Controls.Add(Me.FCButton)
		Me.Controls.Add(Me.RTCButton)
		Me.Controls.Add(Me.VAButton)
		Me.Controls.Add(Me.TIBButton)
		Me.Controls.Add(Me.ROTButton)
		Me.Controls.Add(Me.GSSTButton)
		Me.Controls.Add(Me.Button3)
		Me.Controls.Add(Me.Button2)
		Me.Controls.Add(Me.ProgramXMLTextBox)
		Me.Controls.Add(Me.Button1)
		Me.Controls.Add(Me.ProgramTreeView)
		Me.Controls.Add(Me.ConstructProgButton)
		Me.Controls.Add(Me.TheDseFormChooser)
		Me.Controls.Add(Me.Label1)
		Me.Controls.Add(Me.SourceTextBox)
		Me.Name = "ProgramConstruction"
		Me.Text = "ProgramConstruction"
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents SourceTextBox As System.Windows.Forms.TextBox
	Friend WithEvents Label1 As System.Windows.Forms.Label
	Friend WithEvents TheDseFormChooser As DSExperimentation.DSEFormChooser
	Friend WithEvents ConstructProgButton As System.Windows.Forms.Button
	Friend WithEvents ProgramTreeView As System.Windows.Forms.TreeView
	Friend WithEvents Button1 As System.Windows.Forms.Button
	Friend WithEvents ProgramXMLTextBox As System.Windows.Forms.TextBox
	Friend WithEvents Button2 As System.Windows.Forms.Button
	Friend WithEvents Button3 As System.Windows.Forms.Button
	Friend WithEvents GSSTButton As System.Windows.Forms.Button
	Friend WithEvents ROTButton As System.Windows.Forms.Button
	Friend WithEvents TIBButton As System.Windows.Forms.Button
	Friend WithEvents VAButton As System.Windows.Forms.Button
	Friend WithEvents RTCButton As System.Windows.Forms.Button
	Friend WithEvents FCButton As System.Windows.Forms.Button
	Friend WithEvents IOLWButton As System.Windows.Forms.Button
	Friend WithEvents SSFRTButton As System.Windows.Forms.Button
	Friend WithEvents Button4 As Button
End Class
