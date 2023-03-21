<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DSCompilationWindow
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DSCompilationWindow))
		Me.Button1 = New System.Windows.Forms.Button()
		Me.GroupBox1 = New System.Windows.Forms.GroupBox()
		Me.ShowAdditionalOptions_LinkLabel = New System.Windows.Forms.LinkLabel()
		Me.IconPreviewError_Label = New System.Windows.Forms.Label()
		Me.IconPreview_PictureBox = New System.Windows.Forms.PictureBox()
		Me.Button3 = New System.Windows.Forms.Button()
		Me.Button2 = New System.Windows.Forms.Button()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.IconFilePath_TextBox = New System.Windows.Forms.TextBox()
		Me.TextBox1 = New System.Windows.Forms.TextBox()
		Me.Label6 = New System.Windows.Forms.Label()
		Me.GroupBox2 = New System.Windows.Forms.GroupBox()
		Me.CustomVBCPath_TextBox = New System.Windows.Forms.TextBox()
		Me.UseCustomVBCPath_CheckBox = New System.Windows.Forms.CheckBox()
		Me.CheckBox1 = New System.Windows.Forms.CheckBox()
		Me.BrowseForCustomVBCPath_Button = New System.Windows.Forms.Button()
		Me.GroupBox3 = New System.Windows.Forms.GroupBox()
		Me.Label5 = New System.Windows.Forms.Label()
		Me.Label3 = New System.Windows.Forms.Label()
		Me.Label4 = New System.Windows.Forms.Label()
		Me.TextBox5 = New System.Windows.Forms.TextBox()
		Me.TextBox3 = New System.Windows.Forms.TextBox()
		Me.TextBox4 = New System.Windows.Forms.TextBox()
		Me.Label7 = New System.Windows.Forms.Label()
		Me.ComboBox1 = New System.Windows.Forms.ComboBox()
		Me.GroupBox1.SuspendLayout()
		CType(Me.IconPreview_PictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupBox2.SuspendLayout()
		Me.GroupBox3.SuspendLayout()
		Me.SuspendLayout()
		'
		'Button1
		'
		Me.Button1.Location = New System.Drawing.Point(197, 345)
		Me.Button1.Name = "Button1"
		Me.Button1.Size = New System.Drawing.Size(75, 23)
		Me.Button1.TabIndex = 1
		Me.Button1.Text = "Compile..."
		Me.Button1.UseVisualStyleBackColor = True
		'
		'GroupBox1
		'
		Me.GroupBox1.Controls.Add(Me.ComboBox1)
		Me.GroupBox1.Controls.Add(Me.ShowAdditionalOptions_LinkLabel)
		Me.GroupBox1.Controls.Add(Me.IconPreviewError_Label)
		Me.GroupBox1.Controls.Add(Me.IconPreview_PictureBox)
		Me.GroupBox1.Controls.Add(Me.Button3)
		Me.GroupBox1.Controls.Add(Me.Button2)
		Me.GroupBox1.Controls.Add(Me.Label7)
		Me.GroupBox1.Controls.Add(Me.Label2)
		Me.GroupBox1.Controls.Add(Me.Label1)
		Me.GroupBox1.Controls.Add(Me.IconFilePath_TextBox)
		Me.GroupBox1.Controls.Add(Me.TextBox1)
		Me.GroupBox1.Controls.Add(Me.Label6)
		Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(446, 219)
		Me.GroupBox1.TabIndex = 2
		Me.GroupBox1.TabStop = False
		Me.GroupBox1.Text = "EXE Options"
		'
		'ShowAdditionalOptions_LinkLabel
		'
		Me.ShowAdditionalOptions_LinkLabel.AutoSize = True
		Me.ShowAdditionalOptions_LinkLabel.Location = New System.Drawing.Point(306, 193)
		Me.ShowAdditionalOptions_LinkLabel.Name = "ShowAdditionalOptions_LinkLabel"
		Me.ShowAdditionalOptions_LinkLabel.Size = New System.Drawing.Size(134, 13)
		Me.ShowAdditionalOptions_LinkLabel.TabIndex = 4
		Me.ShowAdditionalOptions_LinkLabel.TabStop = True
		Me.ShowAdditionalOptions_LinkLabel.Text = "Show Advanced Options..."
		'
		'IconPreviewError_Label
		'
		Me.IconPreviewError_Label.AutoSize = True
		Me.IconPreviewError_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.IconPreviewError_Label.ForeColor = System.Drawing.Color.Red
		Me.IconPreviewError_Label.Location = New System.Drawing.Point(173, 102)
		Me.IconPreviewError_Label.Name = "IconPreviewError_Label"
		Me.IconPreviewError_Label.Size = New System.Drawing.Size(0, 13)
		Me.IconPreviewError_Label.TabIndex = 8
		'
		'IconPreview_PictureBox
		'
		Me.IconPreview_PictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.IconPreview_PictureBox.Location = New System.Drawing.Point(67, 134)
		Me.IconPreview_PictureBox.Name = "IconPreview_PictureBox"
		Me.IconPreview_PictureBox.Size = New System.Drawing.Size(100, 72)
		Me.IconPreview_PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
		Me.IconPreview_PictureBox.TabIndex = 2
		Me.IconPreview_PictureBox.TabStop = False
		'
		'Button3
		'
		Me.Button3.Location = New System.Drawing.Point(355, 55)
		Me.Button3.Name = "Button3"
		Me.Button3.Size = New System.Drawing.Size(60, 23)
		Me.Button3.TabIndex = 3
		Me.Button3.Text = "Browse..."
		Me.Button3.UseVisualStyleBackColor = True
		'
		'Button2
		'
		Me.Button2.Location = New System.Drawing.Point(355, 29)
		Me.Button2.Name = "Button2"
		Me.Button2.Size = New System.Drawing.Size(60, 23)
		Me.Button2.TabIndex = 1
		Me.Button2.Text = "Browse..."
		Me.Button2.UseVisualStyleBackColor = True
		'
		'Label2
		'
		Me.Label2.AutoSize = True
		Me.Label2.Location = New System.Drawing.Point(6, 60)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(31, 13)
		Me.Label2.TabIndex = 6
		Me.Label2.Text = "Icon:"
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Location = New System.Drawing.Point(6, 34)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(55, 13)
		Me.Label1.TabIndex = 5
		Me.Label1.Text = "File name:"
		'
		'IconFilePath_TextBox
		'
		Me.IconFilePath_TextBox.Location = New System.Drawing.Point(67, 57)
		Me.IconFilePath_TextBox.Name = "IconFilePath_TextBox"
		Me.IconFilePath_TextBox.Size = New System.Drawing.Size(282, 20)
		Me.IconFilePath_TextBox.TabIndex = 2
		Me.IconFilePath_TextBox.Text = "$(DSIDE_Dir)\DocScriptFile.ICO"
		'
		'TextBox1
		'
		Me.TextBox1.Location = New System.Drawing.Point(67, 31)
		Me.TextBox1.Name = "TextBox1"
		Me.TextBox1.Size = New System.Drawing.Size(282, 20)
		Me.TextBox1.TabIndex = 0
		'
		'Label6
		'
		Me.Label6.AutoSize = True
		Me.Label6.BackColor = System.Drawing.Color.Transparent
		Me.Label6.Location = New System.Drawing.Point(82, 119)
		Me.Label6.Name = "Label6"
		Me.Label6.Size = New System.Drawing.Size(72, 13)
		Me.Label6.TabIndex = 7
		Me.Label6.Text = "Icon Preview:"
		'
		'GroupBox2
		'
		Me.GroupBox2.Controls.Add(Me.CustomVBCPath_TextBox)
		Me.GroupBox2.Controls.Add(Me.UseCustomVBCPath_CheckBox)
		Me.GroupBox2.Controls.Add(Me.CheckBox1)
		Me.GroupBox2.Controls.Add(Me.BrowseForCustomVBCPath_Button)
		Me.GroupBox2.Location = New System.Drawing.Point(12, 237)
		Me.GroupBox2.Name = "GroupBox2"
		Me.GroupBox2.Size = New System.Drawing.Size(446, 91)
		Me.GroupBox2.TabIndex = 3
		Me.GroupBox2.TabStop = False
		Me.GroupBox2.Text = "Compilation Options"
		'
		'CustomVBCPath_TextBox
		'
		Me.CustomVBCPath_TextBox.Enabled = False
		Me.CustomVBCPath_TextBox.Location = New System.Drawing.Point(227, 46)
		Me.CustomVBCPath_TextBox.Name = "CustomVBCPath_TextBox"
		Me.CustomVBCPath_TextBox.Size = New System.Drawing.Size(122, 20)
		Me.CustomVBCPath_TextBox.TabIndex = 2
		'
		'UseCustomVBCPath_CheckBox
		'
		Me.UseCustomVBCPath_CheckBox.AutoSize = True
		Me.UseCustomVBCPath_CheckBox.Location = New System.Drawing.Point(8, 48)
		Me.UseCustomVBCPath_CheckBox.Name = "UseCustomVBCPath_CheckBox"
		Me.UseCustomVBCPath_CheckBox.Size = New System.Drawing.Size(222, 17)
		Me.UseCustomVBCPath_CheckBox.TabIndex = 1
		Me.UseCustomVBCPath_CheckBox.Text = "Do not auto-detect vbc.exe; use this one:"
		Me.UseCustomVBCPath_CheckBox.UseVisualStyleBackColor = True
		'
		'CheckBox1
		'
		Me.CheckBox1.AutoSize = True
		Me.CheckBox1.Location = New System.Drawing.Point(8, 25)
		Me.CheckBox1.Name = "CheckBox1"
		Me.CheckBox1.Size = New System.Drawing.Size(186, 17)
		Me.CheckBox1.TabIndex = 0
		Me.CheckBox1.Text = "Do not delete intermediate .VB file"
		Me.CheckBox1.UseVisualStyleBackColor = True
		'
		'BrowseForCustomVBCPath_Button
		'
		Me.BrowseForCustomVBCPath_Button.Enabled = False
		Me.BrowseForCustomVBCPath_Button.Location = New System.Drawing.Point(355, 44)
		Me.BrowseForCustomVBCPath_Button.Name = "BrowseForCustomVBCPath_Button"
		Me.BrowseForCustomVBCPath_Button.Size = New System.Drawing.Size(60, 23)
		Me.BrowseForCustomVBCPath_Button.TabIndex = 3
		Me.BrowseForCustomVBCPath_Button.Text = "Browse..."
		Me.BrowseForCustomVBCPath_Button.UseVisualStyleBackColor = True
		'
		'GroupBox3
		'
		Me.GroupBox3.Controls.Add(Me.Label5)
		Me.GroupBox3.Controls.Add(Me.Label3)
		Me.GroupBox3.Controls.Add(Me.Label4)
		Me.GroupBox3.Controls.Add(Me.TextBox5)
		Me.GroupBox3.Controls.Add(Me.TextBox3)
		Me.GroupBox3.Controls.Add(Me.TextBox4)
		Me.GroupBox3.Location = New System.Drawing.Point(470, 12)
		Me.GroupBox3.Name = "GroupBox3"
		Me.GroupBox3.Size = New System.Drawing.Size(362, 185)
		Me.GroupBox3.TabIndex = 0
		Me.GroupBox3.TabStop = False
		Me.GroupBox3.Text = "EXE MetaData"
		'
		'Label5
		'
		Me.Label5.AutoSize = True
		Me.Label5.Location = New System.Drawing.Point(6, 86)
		Me.Label5.Name = "Label5"
		Me.Label5.Size = New System.Drawing.Size(45, 13)
		Me.Label5.TabIndex = 5
		Me.Label5.Text = "Version:"
		'
		'Label3
		'
		Me.Label3.AutoSize = True
		Me.Label3.Location = New System.Drawing.Point(6, 60)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(41, 13)
		Me.Label3.TabIndex = 4
		Me.Label3.Text = "Author:"
		'
		'Label4
		'
		Me.Label4.AutoSize = True
		Me.Label4.Location = New System.Drawing.Point(6, 34)
		Me.Label4.Name = "Label4"
		Me.Label4.Size = New System.Drawing.Size(78, 13)
		Me.Label4.TabIndex = 3
		Me.Label4.Text = "Product Name:"
		'
		'TextBox5
		'
		Me.TextBox5.Location = New System.Drawing.Point(90, 83)
		Me.TextBox5.Name = "TextBox5"
		Me.TextBox5.Size = New System.Drawing.Size(259, 20)
		Me.TextBox5.TabIndex = 2
		Me.TextBox5.Text = "1.0.0.0"
		'
		'TextBox3
		'
		Me.TextBox3.Location = New System.Drawing.Point(90, 57)
		Me.TextBox3.Name = "TextBox3"
		Me.TextBox3.Size = New System.Drawing.Size(259, 20)
		Me.TextBox3.TabIndex = 1
		Me.TextBox3.Text = "DocScript Programmer"
		'
		'TextBox4
		'
		Me.TextBox4.Location = New System.Drawing.Point(90, 31)
		Me.TextBox4.Name = "TextBox4"
		Me.TextBox4.Size = New System.Drawing.Size(259, 20)
		Me.TextBox4.TabIndex = 0
		Me.TextBox4.Text = "DocScript-Compiled Program"
		'
		'Label7
		'
		Me.Label7.AutoSize = True
		Me.Label7.Location = New System.Drawing.Point(6, 86)
		Me.Label7.Name = "Label7"
		Me.Label7.Size = New System.Drawing.Size(34, 13)
		Me.Label7.TabIndex = 6
		Me.Label7.Text = "Type:"
		'
		'ComboBox1
		'
		Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.ComboBox1.FormattingEnabled = True
		Me.ComboBox1.Items.AddRange(New Object() {"Windows Application", "Console Application"})
		Me.ComboBox1.Location = New System.Drawing.Point(67, 83)
		Me.ComboBox1.Name = "ComboBox1"
		Me.ComboBox1.Size = New System.Drawing.Size(282, 21)
		Me.ComboBox1.TabIndex = 9
		'
		'DSCompilationWindow
		'
		Me.AcceptButton = Me.Button1
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(468, 380)
		Me.Controls.Add(Me.GroupBox2)
		Me.Controls.Add(Me.GroupBox3)
		Me.Controls.Add(Me.GroupBox1)
		Me.Controls.Add(Me.Button1)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "DSCompilationWindow"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "DocScript Compilation"
		Me.GroupBox1.ResumeLayout(False)
		Me.GroupBox1.PerformLayout()
		CType(Me.IconPreview_PictureBox, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupBox2.ResumeLayout(False)
		Me.GroupBox2.PerformLayout()
		Me.GroupBox3.ResumeLayout(False)
		Me.GroupBox3.PerformLayout()
		Me.ResumeLayout(False)

	End Sub

	Friend WithEvents Button1 As Forms.Button
	Friend WithEvents GroupBox1 As Forms.GroupBox
	Friend WithEvents Label1 As System.Windows.Forms.Label
	Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
	Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
	Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
	Friend WithEvents Label2 As System.Windows.Forms.Label
	Friend WithEvents IconFilePath_TextBox As System.Windows.Forms.TextBox
	Friend WithEvents Button3 As System.Windows.Forms.Button
	Friend WithEvents Button2 As System.Windows.Forms.Button
	Friend WithEvents IconPreview_PictureBox As System.Windows.Forms.PictureBox
	Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
	Friend WithEvents Label3 As System.Windows.Forms.Label
	Friend WithEvents Label4 As System.Windows.Forms.Label
	Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
	Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
	Friend WithEvents Label5 As System.Windows.Forms.Label
	Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
	Friend WithEvents Label6 As System.Windows.Forms.Label
	Friend WithEvents ShowAdditionalOptions_LinkLabel As System.Windows.Forms.LinkLabel
	Friend WithEvents IconPreviewError_Label As System.Windows.Forms.Label
	Friend WithEvents UseCustomVBCPath_CheckBox As System.Windows.Forms.CheckBox
	Friend WithEvents CustomVBCPath_TextBox As System.Windows.Forms.TextBox
	Friend WithEvents BrowseForCustomVBCPath_Button As System.Windows.Forms.Button
	Friend WithEvents Label7 As System.Windows.Forms.Label
	Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
End Class
