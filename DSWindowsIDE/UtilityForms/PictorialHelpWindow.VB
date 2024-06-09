Public Class PictorialHelpWindow

	Public Sub LoadImages() Handles Me.Shown
		Try

			'Just in case...
			Me.PicturesTabControl.TabPages.Clear()

			REM Generate a new TabPage for each Bitmap in Resources, whose name starts with "__"
			For Each _Resource As DictionaryEntry In My.Resources.ResourceManager.GetResourceSet(culture:=System.Globalization.CultureInfo.CurrentCulture, createIfNotExists:=False, tryParents:=True).Cast(Of DictionaryEntry)().OrderBy(Of [String])(Function(_ResourceKVP As DictionaryEntry) _ResourceKVP.Key.ToString())
				If (_Resource.Value.GetType() Is GetType(System.Drawing.Bitmap)) AndAlso (_Resource.Key.ToString().StartsWith("__")) Then

					REM Create the [New TabPage()]
					Me.PicturesTabControl.TabPages.Add(
					  New Windows.Forms.TabPage(_Resource.Key.ToString().WithFirstCharacterRemoved().WithFirstCharacterRemoved()) With {.Name = _Resource.Key.ToString()}
					)

					REM Add the PictureBox thereon
					Me.PicturesTabControl.TabPages.Item(key:=_Resource.Key.ToString()).Controls.Add(
					  New Windows.Forms.PictureBox() With {.Image = CType(_Resource.Value, System.Drawing.Bitmap), .Dock = Forms.DockStyle.Fill, .SizeMode = Forms.PictureBoxSizeMode.Zoom}
					)

				End If
			Next

		Catch _Ex As Exception : MsgBox("On loading the TabPages for the Images: " & _Ex.Message, MsgBoxStyle.Critical, "DS Pictorial Help") : End Try
	End Sub

#Region "Close the form on pressing [Esc]"

	Protected Sub Register_CloseOnEscKey_Handler() Handles Me.Load
		Me.KeyPreview = True
		AddHandler Me.KeyDown, Sub(_Sender As Object, _KeyEventArgs As Forms.KeyEventArgs) If _KeyEventArgs.KeyCode = Forms.Keys.Escape Then Me.Close()
	End Sub

	Protected Overrides Function ProcessDialogKey(keyData As System.Windows.Forms.Keys) As Boolean
		If (Windows.Forms.Form.ModifierKeys = Forms.Keys.None) AndAlso (keyData = Forms.Keys.Escape) Then
			Me.Close() : Return True
		End If
		Return MyBase.ProcessDialogKey(keyData)
	End Function

#End Region

End Class