Public Class DSCompilationWindow

	Public Const Placeholder_DSIDE_Dir$ = "$(DSIDE_Dir)"
	Public ReadOnly ProgramToCompile As DocScript.Runtime.Program

	Public Sub New(ByVal _Program As DocScript.Runtime.Program)

		REM Initialise the Object
		Me.InitializeComponent()
		Me.ProgramToCompile = _Program

		REM Set the Program Name
		Try
			If IO.File.Exists(_Program.Name) Then
				Me.OutputEXE_FullPath_TextBox.Text = IO.Path.Combine((New IO.FileInfo(Me.ProgramToCompile.Name)).DirectoryName, IO.Path.GetFileNameWithoutExtension(Me.ProgramToCompile.Name)) & ".exe"
				Me.MetaData_ProductName_TextBox.Text = IO.Path.GetFileNameWithoutExtension(Me.ProgramToCompile.Name)
			End If
		Catch : End Try

		REM Add EventHandlers for the File-Browsing-Dialogs
		AddHandler Me.BrowseForEXEOutputFilePath_Button.Click,
			Sub()
				Me.Cursor = Forms.Cursors.WaitCursor : Me.Enabled = False
				Dim _RequestedPath As IO.FileInfo = UsefulMethods.RequestFilePath_WithSaveFileDialog("Save (managed) EXE Output File as...", {New Tuple(Of String, String)("PE Binary Executables", "EXE")}, _InitialDirectory:=If(If(Me.ProgramToCompile.Name IsNot Nothing, (New IO.FileInfo(Me.ProgramToCompile.Name)).Directory.Exists, False), (New IO.FileInfo(Me.ProgramToCompile.Name)).DirectoryName, Nothing))
				Me.OutputEXE_FullPath_TextBox.Text = If(_RequestedPath Is Nothing, "", _RequestedPath.FullName)
				Me.Enabled = True : Me.Cursor = Forms.Cursors.Default
			End Sub

		AddHandler Me.BrowseForIconFilePath_Button.Click,
			Sub()
				Me.Cursor = Forms.Cursors.WaitCursor : Me.Enabled = False
				Dim _RequestedPath As IO.FileInfo = UsefulMethods.RequestFilePath_WithOpenFileDialog("Browse for EXE Icon...", {New Tuple(Of String, String)("Windows Icons", "ICO")}, Environment.GetFolderPath(Environment.SpecialFolder.MyPictures))
				Me.IconFilePath_TextBox.Text = If(_RequestedPath Is Nothing, "", _RequestedPath.FullName)
				Me.Enabled = True : Me.Cursor = Forms.Cursors.Default
			End Sub

		AddHandler Me.BrowseForCustomVBCPath_Button.Click,
			Sub()
				Me.Cursor = Forms.Cursors.WaitCursor : Me.Enabled = False
				Dim _RequestedPath As IO.FileInfo = UsefulMethods.RequestFilePath_WithOpenFileDialog("Browse for custom VBC File...", {New Tuple(Of String, String)("PE Binary Executables", "EXE")}, Environment.GetFolderPath(Environment.SpecialFolder.Windows))
				Me.CustomVBCPath_TextBox.Text = If(_RequestedPath Is Nothing, "", _RequestedPath.FullName)
				Me.Enabled = True : Me.Cursor = Forms.Cursors.Default
			End Sub

	End Sub

	Public Sub InitialiseUIComponents() Handles Me.Shown

		REM Attempt to get the Default ICON Loaded: $(DSIDE_Dir)\DocScriptFile.ICO
		Me.LoadSpecifiedICO_IntoPreviewPictureBox()	'This would happen anyway as Windows Forms sets the hard-coded text of the textbox, then this EventHandler would be called
		Me.ExeType_ComboBox.SelectedIndex = 0

	End Sub

	Public Sub LoadSpecifiedICO_IntoPreviewPictureBox() Handles IconFilePath_TextBox.TextChanged
		Try
			Me.IconPreview_PictureBox.Image = System.Drawing.Image.FromStream(New IO.MemoryStream(IO.File.ReadAllBytes(Me.IconFilePath_TextBox.Text.Replace(Placeholder_DSIDE_Dir, UsefulMethods.GetExecutingAssemblyDirectory().FullName))))
			Me.IconPreviewError_Label.Text = [String].[Empty]
		Catch _Ex As Exception
			Me.IconPreviewError_Label.Text = ("Could not load Icon Preview: " & _Ex.Message).ToCharArray().Intersperse(vbCrLf.ToCharArray(), 40)
			Me.IconPreview_PictureBox.Image = Nothing
		End Try
	End Sub

#Region "Close the form on pressing [Esc]"

	Protected Sub Register_CloseOnEscKey_Handler() Handles Me.Load
		Me.KeyPreview = True
		AddHandler Me.KeyDown, Sub(_Sender As Object, _KeyEventArgs As Forms.KeyEventArgs) If _KeyEventArgs.KeyCode = Forms.Keys.Escape Then Me.Close()
	End Sub

	Protected Overrides Function ProcessDialogKey(ByVal keyData As System.Windows.Forms.Keys) As Boolean
		If (Windows.Forms.Form.ModifierKeys = Forms.Keys.None) AndAlso (keyData = Forms.Keys.Escape) Then
			Me.Close() : Return True
		End If
		Return MyBase.ProcessDialogKey(keyData)
	End Function

#End Region

	Sub ShowAdditionalOptions() Handles ShowAdditionalOptions_LinkLabel.LinkClicked
		Me.Width = Me.MaximumSize.Width : Me.Height = Me.MaximumSize.Height
		Me.CompileButton.Location = New System.Drawing.Point(410, 317)
		Me.MinimumSize = Me.MaximumSize
		Me.ShowAdditionalOptions_LinkLabel.Hide()
	End Sub

	Sub Toggle_CustomVBCPathControls_Enabledness() Handles UseCustomVBCPath_CheckBox.CheckedChanged
		Me.CustomVBCPath_TextBox.Enabled = Me.UseCustomVBCPath_CheckBox.Checked
		Me.BrowseForCustomVBCPath_Button.Enabled = Me.UseCustomVBCPath_CheckBox.Checked
	End Sub

	Public Sub RunCompilation() Handles CompileButton.Click
		Try

			Me.Enabled = False : Me.Cursor = Forms.Cursors.WaitCursor

			REM ILMerge.exe, System.Compiler.dll, and DSCore.dll should exist in the same directory as DSCLI.exe
			If Not {"ILMerge.exe", "System.Compiler.dll", "DSCore.dll"}.All(
			 Function(_FileName$) IO.File.Exists(IO.Path.Combine(UsefulMethods.GetExecutingAssemblyDirectory().FullName, _FileName))
			) Then Throw New DSException("A copy of ILMerge.exe, System.Compiler.dll, and DSCore.dll must exist in the same directory as DSCLI.exe, in order for DSCompilation to function.")

			REM Determine the ExeCxt to use, from the ComboBox
			Select Case Me.ExeType_ComboBox.Text
				Case "Windows Application" : Me.ProgramToCompile.ExecutionContext = Runtime.ExecutionContext.GUIDefault
				Case "Console Application" : Me.ProgramToCompile.ExecutionContext = Runtime.ExecutionContext.CLIDefault
				Case Else : Throw New DSValidationException("The Target Exe Type was unaccounted-for. Only Windows or Console Applications are valid.", Me.ExeType_ComboBox.Text)
			End Select

			REM Apply the MetaData to the Program
			With Me.ProgramToCompile
				.Name = Me.MetaData_ProductName_TextBox.Text
				.Author = Me.MetaData_Author_TextBox.Text
				.Description = Me.MetaData_Description_TextBox.Text
				.Version = Me.MetaData_Version_TextBox.Text
				.CopyrightMessage = Me.MetaData_Copyright_TextBox.Text
			End With

			REM CompileToExe()
			Me.ProgramToCompile _
			.MustNotBeNothing() _
			.CompileToEXE(
			 _OutputDirectory:=(New IO.FileInfo(Me.OutputEXE_FullPath_TextBox.Text.Replace(Placeholder_DSIDE_Dir, UsefulMethods.GetExecutingAssemblyDirectory().FullName))).Directory,
			 _FinalExeFile_NameOnly:=(New IO.FileInfo(Me.OutputEXE_FullPath_TextBox.Text.Replace(Placeholder_DSIDE_Dir, UsefulMethods.GetExecutingAssemblyDirectory().FullName))).Name,
			 _VBCExe:=If(Me.UseCustomVBCPath_CheckBox.Checked, New IO.FileInfo(Me.CustomVBCPath_TextBox.Text.Replace(Placeholder_DSIDE_Dir, UsefulMethods.GetExecutingAssemblyDirectory().FullName)), Runtime.Program.DefaultVbcExe_ForCurrentSystem),
			 _ILMergeExe:=New IO.FileInfo(IO.Path.Combine(UsefulMethods.GetExecutingAssemblyDirectory().FullName, "ILMerge.exe")),
			 _DSCoreDll:=New IO.FileInfo(IO.Path.Combine(UsefulMethods.GetExecutingAssemblyDirectory().FullName, "DSCore.dll")),
			 _IcoFile_ForExe:=New IO.FileInfo(Me.IconFilePath_TextBox.Text.Replace(Placeholder_DSIDE_Dir, UsefulMethods.GetExecutingAssemblyDirectory().FullName)),
			 _DeleteIntermediateFiles:=Not Me.DoNotDeleteIntermediateFiles_CheckBox.Checked
			)

			REM Provide Feedback, and open the exe in explorer
			MsgBox("The DocScript Program compiled successfully", MsgBoxStyle.Information)
			Process.Start("Explorer.exe", "/select,""" & Me.OutputEXE_FullPath_TextBox.Text & """"c)

		Catch _Ex As Exception : MsgBox("The DocScript Source could not be compiled. Reason: " & vbCrLf & _Ex.Message, MsgBoxStyle.Critical)
		Finally : Me.Enabled = True : Me.Cursor = Forms.Cursors.Default : End Try
	End Sub

End Class