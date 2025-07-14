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
        Me.CompileButton = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.ExeType_ComboBox = New System.Windows.Forms.ComboBox()
        Me.ShowAdditionalOptions_LinkLabel = New System.Windows.Forms.LinkLabel()
        Me.IconPreviewError_Label = New System.Windows.Forms.Label()
        Me.IconPreview_PictureBox = New System.Windows.Forms.PictureBox()
        Me.BrowseForIconFilePath_Button = New System.Windows.Forms.Button()
        Me.BrowseForEXEOutputFilePath_Button = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.IconFilePath_TextBox = New System.Windows.Forms.TextBox()
        Me.OutputEXE_FullPath_TextBox = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.CustomVBCPath_TextBox = New System.Windows.Forms.TextBox()
        Me.UseCustomVBCPath_CheckBox = New System.Windows.Forms.CheckBox()
        Me.DoNotDeleteIntermediateFiles_CheckBox = New System.Windows.Forms.CheckBox()
        Me.BrowseForCustomVBCPath_Button = New System.Windows.Forms.Button()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.MetaData_Copyright_TextBox = New System.Windows.Forms.TextBox()
        Me.MetaData_Description_TextBox = New System.Windows.Forms.TextBox()
        Me.MetaData_Version_TextBox = New System.Windows.Forms.TextBox()
        Me.MetaData_Author_TextBox = New System.Windows.Forms.TextBox()
        Me.MetaData_ProductName_TextBox = New System.Windows.Forms.TextBox()
        Me.GroupBox1.SuspendLayout()
        CType(Me.IconPreview_PictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'CompileButton
        '
        Me.CompileButton.Location = New System.Drawing.Point(361, 465)
        Me.CompileButton.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.CompileButton.Name = "CompileButton"
        Me.CompileButton.Size = New System.Drawing.Size(138, 42)
        Me.CompileButton.TabIndex = 3
        Me.CompileButton.Text = "&Compile..."
        Me.CompileButton.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.ExeType_ComboBox)
        Me.GroupBox1.Controls.Add(Me.ShowAdditionalOptions_LinkLabel)
        Me.GroupBox1.Controls.Add(Me.IconPreviewError_Label)
        Me.GroupBox1.Controls.Add(Me.IconPreview_PictureBox)
        Me.GroupBox1.Controls.Add(Me.BrowseForIconFilePath_Button)
        Me.GroupBox1.Controls.Add(Me.BrowseForEXEOutputFilePath_Button)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.IconFilePath_TextBox)
        Me.GroupBox1.Controls.Add(Me.OutputEXE_FullPath_TextBox)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Location = New System.Drawing.Point(22, 22)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.GroupBox1.Size = New System.Drawing.Size(818, 404)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "EXE Options"
        '
        'ExeType_ComboBox
        '
        Me.ExeType_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ExeType_ComboBox.FormattingEnabled = True
        Me.ExeType_ComboBox.Items.AddRange(New Object() {"Windows Application", "Console Application"})
        Me.ExeType_ComboBox.Location = New System.Drawing.Point(123, 153)
        Me.ExeType_ComboBox.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.ExeType_ComboBox.Name = "ExeType_ComboBox"
        Me.ExeType_ComboBox.Size = New System.Drawing.Size(514, 32)
        Me.ExeType_ComboBox.TabIndex = 9
        '
        'ShowAdditionalOptions_LinkLabel
        '
        Me.ShowAdditionalOptions_LinkLabel.AutoSize = True
        Me.ShowAdditionalOptions_LinkLabel.Location = New System.Drawing.Point(561, 356)
        Me.ShowAdditionalOptions_LinkLabel.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.ShowAdditionalOptions_LinkLabel.Name = "ShowAdditionalOptions_LinkLabel"
        Me.ShowAdditionalOptions_LinkLabel.Size = New System.Drawing.Size(244, 25)
        Me.ShowAdditionalOptions_LinkLabel.TabIndex = 10
        Me.ShowAdditionalOptions_LinkLabel.TabStop = True
        Me.ShowAdditionalOptions_LinkLabel.Text = "Show Advanced Options..."
        '
        'IconPreviewError_Label
        '
        Me.IconPreviewError_Label.AutoSize = True
        Me.IconPreviewError_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.IconPreviewError_Label.ForeColor = System.Drawing.Color.Red
        Me.IconPreviewError_Label.Location = New System.Drawing.Point(348, 220)
        Me.IconPreviewError_Label.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.IconPreviewError_Label.Name = "IconPreviewError_Label"
        Me.IconPreviewError_Label.Size = New System.Drawing.Size(0, 25)
        Me.IconPreviewError_Label.TabIndex = 4
        '
        'IconPreview_PictureBox
        '
        Me.IconPreview_PictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.IconPreview_PictureBox.Location = New System.Drawing.Point(123, 244)
        Me.IconPreview_PictureBox.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.IconPreview_PictureBox.Name = "IconPreview_PictureBox"
        Me.IconPreview_PictureBox.Size = New System.Drawing.Size(180, 130)
        Me.IconPreview_PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.IconPreview_PictureBox.TabIndex = 2
        Me.IconPreview_PictureBox.TabStop = False
        '
        'BrowseForIconFilePath_Button
        '
        Me.BrowseForIconFilePath_Button.Location = New System.Drawing.Point(651, 102)
        Me.BrowseForIconFilePath_Button.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.BrowseForIconFilePath_Button.Name = "BrowseForIconFilePath_Button"
        Me.BrowseForIconFilePath_Button.Size = New System.Drawing.Size(110, 42)
        Me.BrowseForIconFilePath_Button.TabIndex = 8
        Me.BrowseForIconFilePath_Button.Text = "Browse..."
        Me.BrowseForIconFilePath_Button.UseVisualStyleBackColor = True
        '
        'BrowseForEXEOutputFilePath_Button
        '
        Me.BrowseForEXEOutputFilePath_Button.Location = New System.Drawing.Point(651, 54)
        Me.BrowseForEXEOutputFilePath_Button.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.BrowseForEXEOutputFilePath_Button.Name = "BrowseForEXEOutputFilePath_Button"
        Me.BrowseForEXEOutputFilePath_Button.Size = New System.Drawing.Size(110, 42)
        Me.BrowseForEXEOutputFilePath_Button.TabIndex = 6
        Me.BrowseForEXEOutputFilePath_Button.Text = "Browse..."
        Me.BrowseForEXEOutputFilePath_Button.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(11, 159)
        Me.Label7.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(102, 25)
        Me.Label7.TabIndex = 2
        Me.Label7.Text = "Exe Type:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(11, 111)
        Me.Label2.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(55, 25)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Icon:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(11, 63)
        Me.Label1.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(103, 25)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "File name:"
        '
        'IconFilePath_TextBox
        '
        Me.IconFilePath_TextBox.Location = New System.Drawing.Point(123, 105)
        Me.IconFilePath_TextBox.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.IconFilePath_TextBox.Name = "IconFilePath_TextBox"
        Me.IconFilePath_TextBox.Size = New System.Drawing.Size(514, 29)
        Me.IconFilePath_TextBox.TabIndex = 7
        Me.IconFilePath_TextBox.Text = "$(DSIDE_Dir)\DocScriptFile.ICO"
        '
        'OutputEXE_FullPath_TextBox
        '
        Me.OutputEXE_FullPath_TextBox.Location = New System.Drawing.Point(123, 57)
        Me.OutputEXE_FullPath_TextBox.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.OutputEXE_FullPath_TextBox.Name = "OutputEXE_FullPath_TextBox"
        Me.OutputEXE_FullPath_TextBox.Size = New System.Drawing.Size(514, 29)
        Me.OutputEXE_FullPath_TextBox.TabIndex = 5
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Location = New System.Drawing.Point(150, 216)
        Me.Label6.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(129, 25)
        Me.Label6.TabIndex = 3
        Me.Label6.Text = "Icon Preview:"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.CustomVBCPath_TextBox)
        Me.GroupBox2.Controls.Add(Me.UseCustomVBCPath_CheckBox)
        Me.GroupBox2.Controls.Add(Me.DoNotDeleteIntermediateFiles_CheckBox)
        Me.GroupBox2.Controls.Add(Me.BrowseForCustomVBCPath_Button)
        Me.GroupBox2.Location = New System.Drawing.Point(862, 378)
        Me.GroupBox2.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Padding = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.GroupBox2.Size = New System.Drawing.Size(761, 168)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Compilation Options"
        '
        'CustomVBCPath_TextBox
        '
        Me.CustomVBCPath_TextBox.Enabled = False
        Me.CustomVBCPath_TextBox.Location = New System.Drawing.Point(416, 85)
        Me.CustomVBCPath_TextBox.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.CustomVBCPath_TextBox.Name = "CustomVBCPath_TextBox"
        Me.CustomVBCPath_TextBox.Size = New System.Drawing.Size(209, 29)
        Me.CustomVBCPath_TextBox.TabIndex = 0
        '
        'UseCustomVBCPath_CheckBox
        '
        Me.UseCustomVBCPath_CheckBox.AutoSize = True
        Me.UseCustomVBCPath_CheckBox.Location = New System.Drawing.Point(15, 89)
        Me.UseCustomVBCPath_CheckBox.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.UseCustomVBCPath_CheckBox.Name = "UseCustomVBCPath_CheckBox"
        Me.UseCustomVBCPath_CheckBox.Size = New System.Drawing.Size(393, 29)
        Me.UseCustomVBCPath_CheckBox.TabIndex = 3
        Me.UseCustomVBCPath_CheckBox.Text = "Do not auto-detect vbc.exe; use this one:"
        Me.UseCustomVBCPath_CheckBox.UseVisualStyleBackColor = True
        '
        'DoNotDeleteIntermediateFiles_CheckBox
        '
        Me.DoNotDeleteIntermediateFiles_CheckBox.AutoSize = True
        Me.DoNotDeleteIntermediateFiles_CheckBox.Location = New System.Drawing.Point(15, 46)
        Me.DoNotDeleteIntermediateFiles_CheckBox.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.DoNotDeleteIntermediateFiles_CheckBox.Name = "DoNotDeleteIntermediateFiles_CheckBox"
        Me.DoNotDeleteIntermediateFiles_CheckBox.Size = New System.Drawing.Size(303, 29)
        Me.DoNotDeleteIntermediateFiles_CheckBox.TabIndex = 2
        Me.DoNotDeleteIntermediateFiles_CheckBox.Text = "Do not delete intermediate files"
        Me.DoNotDeleteIntermediateFiles_CheckBox.UseVisualStyleBackColor = True
        '
        'BrowseForCustomVBCPath_Button
        '
        Me.BrowseForCustomVBCPath_Button.Enabled = False
        Me.BrowseForCustomVBCPath_Button.Location = New System.Drawing.Point(640, 81)
        Me.BrowseForCustomVBCPath_Button.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.BrowseForCustomVBCPath_Button.Name = "BrowseForCustomVBCPath_Button"
        Me.BrowseForCustomVBCPath_Button.Size = New System.Drawing.Size(110, 42)
        Me.BrowseForCustomVBCPath_Button.TabIndex = 1
        Me.BrowseForCustomVBCPath_Button.Text = "Browse..."
        Me.BrowseForCustomVBCPath_Button.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Label9)
        Me.GroupBox3.Controls.Add(Me.Label8)
        Me.GroupBox3.Controls.Add(Me.Label5)
        Me.GroupBox3.Controls.Add(Me.Label3)
        Me.GroupBox3.Controls.Add(Me.Label4)
        Me.GroupBox3.Controls.Add(Me.MetaData_Copyright_TextBox)
        Me.GroupBox3.Controls.Add(Me.MetaData_Description_TextBox)
        Me.GroupBox3.Controls.Add(Me.MetaData_Version_TextBox)
        Me.GroupBox3.Controls.Add(Me.MetaData_Author_TextBox)
        Me.GroupBox3.Controls.Add(Me.MetaData_ProductName_TextBox)
        Me.GroupBox3.Location = New System.Drawing.Point(862, 22)
        Me.GroupBox3.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Padding = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.GroupBox3.Size = New System.Drawing.Size(761, 342)
        Me.GroupBox3.TabIndex = 2
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "EXE MetaData"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(11, 255)
        Me.Label9.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(102, 25)
        Me.Label9.TabIndex = 4
        Me.Label9.Text = "Copyright:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(11, 207)
        Me.Label8.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(115, 25)
        Me.Label8.TabIndex = 3
        Me.Label8.Text = "Description:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(11, 159)
        Me.Label5.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(85, 25)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Version:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(11, 111)
        Me.Label3.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(76, 25)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Author:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(11, 63)
        Me.Label4.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(142, 25)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Product Name:"
        '
        'MetaData_Copyright_TextBox
        '
        Me.MetaData_Copyright_TextBox.Location = New System.Drawing.Point(174, 249)
        Me.MetaData_Copyright_TextBox.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.MetaData_Copyright_TextBox.Name = "MetaData_Copyright_TextBox"
        Me.MetaData_Copyright_TextBox.Size = New System.Drawing.Size(527, 29)
        Me.MetaData_Copyright_TextBox.TabIndex = 9
        Me.MetaData_Copyright_TextBox.Text = "DS (c) 2023"
        '
        'MetaData_Description_TextBox
        '
        Me.MetaData_Description_TextBox.Location = New System.Drawing.Point(174, 201)
        Me.MetaData_Description_TextBox.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.MetaData_Description_TextBox.Name = "MetaData_Description_TextBox"
        Me.MetaData_Description_TextBox.Size = New System.Drawing.Size(527, 29)
        Me.MetaData_Description_TextBox.TabIndex = 8
        Me.MetaData_Description_TextBox.Text = "Compiled DS Program"
        '
        'MetaData_Version_TextBox
        '
        Me.MetaData_Version_TextBox.Location = New System.Drawing.Point(174, 153)
        Me.MetaData_Version_TextBox.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.MetaData_Version_TextBox.Name = "MetaData_Version_TextBox"
        Me.MetaData_Version_TextBox.Size = New System.Drawing.Size(527, 29)
        Me.MetaData_Version_TextBox.TabIndex = 7
        Me.MetaData_Version_TextBox.Text = "1.0.0.0"
        '
        'MetaData_Author_TextBox
        '
        Me.MetaData_Author_TextBox.Location = New System.Drawing.Point(174, 105)
        Me.MetaData_Author_TextBox.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.MetaData_Author_TextBox.Name = "MetaData_Author_TextBox"
        Me.MetaData_Author_TextBox.Size = New System.Drawing.Size(527, 29)
        Me.MetaData_Author_TextBox.TabIndex = 6
        Me.MetaData_Author_TextBox.Text = "DocScript Programmer"
        '
        'MetaData_ProductName_TextBox
        '
        Me.MetaData_ProductName_TextBox.Location = New System.Drawing.Point(174, 57)
        Me.MetaData_ProductName_TextBox.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.MetaData_ProductName_TextBox.Name = "MetaData_ProductName_TextBox"
        Me.MetaData_ProductName_TextBox.Size = New System.Drawing.Size(527, 29)
        Me.MetaData_ProductName_TextBox.TabIndex = 5
        Me.MetaData_ProductName_TextBox.Text = "DocScript-Compiled Program"
        '
        'DSCompilationWindow
        '
        Me.AcceptButton = Me.CompileButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(860, 602)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.CompileButton)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(1648, 690)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(869, 546)
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

	Friend WithEvents CompileButton As Forms.Button
	Friend WithEvents GroupBox1 As Forms.GroupBox
	Friend WithEvents Label1 As System.Windows.Forms.Label
	Friend WithEvents OutputEXE_FullPath_TextBox As System.Windows.Forms.TextBox
	Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
	Friend WithEvents DoNotDeleteIntermediateFiles_CheckBox As System.Windows.Forms.CheckBox
	Friend WithEvents Label2 As System.Windows.Forms.Label
	Friend WithEvents IconFilePath_TextBox As System.Windows.Forms.TextBox
	Friend WithEvents BrowseForIconFilePath_Button As System.Windows.Forms.Button
	Friend WithEvents BrowseForEXEOutputFilePath_Button As System.Windows.Forms.Button
	Friend WithEvents IconPreview_PictureBox As System.Windows.Forms.PictureBox
	Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
	Friend WithEvents Label3 As System.Windows.Forms.Label
	Friend WithEvents Label4 As System.Windows.Forms.Label
	Friend WithEvents MetaData_Author_TextBox As System.Windows.Forms.TextBox
	Friend WithEvents MetaData_ProductName_TextBox As System.Windows.Forms.TextBox
	Friend WithEvents Label5 As System.Windows.Forms.Label
	Friend WithEvents MetaData_Version_TextBox As System.Windows.Forms.TextBox
	Friend WithEvents Label6 As System.Windows.Forms.Label
	Friend WithEvents ShowAdditionalOptions_LinkLabel As System.Windows.Forms.LinkLabel
	Friend WithEvents IconPreviewError_Label As System.Windows.Forms.Label
	Friend WithEvents UseCustomVBCPath_CheckBox As System.Windows.Forms.CheckBox
	Friend WithEvents CustomVBCPath_TextBox As System.Windows.Forms.TextBox
	Friend WithEvents BrowseForCustomVBCPath_Button As System.Windows.Forms.Button
	Friend WithEvents Label7 As System.Windows.Forms.Label
	Friend WithEvents ExeType_ComboBox As System.Windows.Forms.ComboBox
	Friend WithEvents Label9 As Forms.Label
	Friend WithEvents Label8 As Forms.Label
	Friend WithEvents MetaData_Copyright_TextBox As Forms.TextBox
	Friend WithEvents MetaData_Description_TextBox As Forms.TextBox
End Class
