Public Class EnterCredentialsDialog

	''' <summary>Item1 = Username; Item2 = Password</summary>
	Public Shared Function Show_AndGetCredentials() As Tuple(Of String, String)

		Dim _Dialog As New EnterCredentialsDialog()
		_Dialog.ShowDialog() : _Dialog.Focus() : _Dialog.Activate() : _Dialog.BringToFront()
		Return New Tuple(Of String, String)(_Dialog.UsernameTextBox.Text, _Dialog.PasswordTextBox.Text)

	End Function

	Public Sub DismissDialog_EnsuringRequiredFieldsAreSpecified() Handles OkButton.Click
		If {Me.UsernameTextBox.Text, Me.PasswordTextBox.Text}.Contains(String.Empty) _
		 Then MsgBox("Please provide both a Username and Password", MsgBoxStyle.Information) _
		 Else Me.Close()
	End Sub

End Class