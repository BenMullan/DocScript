Public Class DSCompilationWindow

	Public Const Placeholder_DSIDE_Dir$ = "$(DSIDE_Dir)"

	Public Sub InitialiseUI() Handles Me.Shown

		REM Attempt to get the Default ICON Loaded: $(DSIDE_Dir)\DocScriptFile.ICO
		Me.LoadSpecifiedICO_IntoPreviewPictureBox()

	End Sub

	Private Sub IconFilePath_TextBox_TextChanged() Handles IconFilePath_TextBox.TextChanged
		Me.LoadSpecifiedICO_IntoPreviewPictureBox()
	End Sub

	Public Sub LoadSpecifiedICO_IntoPreviewPictureBox()
		Try
			Me.IconPreview_PictureBox.Image = System.Drawing.Image.FromStream(New IO.MemoryStream(IO.File.ReadAllBytes(Me.IconFilePath_TextBox.Text.Replace(Placeholder_DSIDE_Dir, UsefulMethods.GetExecutingAssemblyDirectory().FullName))))
			Me.IconPreviewError_Label.Text = [String].[Empty]
		Catch _Ex As Exception
			Me.IconPreviewError_Label.Text = ("Could not load Icon Preview: " & _Ex.Message).ToCharArray().Intersperse(vbCrLf.ToCharArray(), 40)
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
		Me.Width = 855
		Me.ShowAdditionalOptions_LinkLabel.Enabled = False
	End Sub

	Sub Toggle_CustomVBCPathControls_Enabledness() Handles UseCustomVBCPath_CheckBox.CheckedChanged
		Me.CustomVBCPath_TextBox.Enabled = Me.UseCustomVBCPath_CheckBox.Checked
		Me.BrowseForCustomVBCPath_Button.Enabled = Not Me.UseCustomVBCPath_CheckBox.Checked
	End Sub

End Class