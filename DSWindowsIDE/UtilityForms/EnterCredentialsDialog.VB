Public Class EnterCredentialsDialog

#Region "Shared Members"

	''' <summary>Item1 = Username; Item2 = Password</summary>
	Public Shared Function Show_AndGetCredentials() As Tuple(Of String, String)

		Dim _Dialog As New EnterCredentialsDialog() : _Dialog.ShowDialog()
		Return New Tuple(Of [String], [String])(_Dialog.UsernameTextBox.Text, _Dialog.PasswordTextBox.Text)

	End Function

#End Region

#Region "Instance Members"

	Protected WindowClose_IsInitiatedByOkButton_ As [Boolean] = False

	REM Seize focus from the ActionInProgressDialog
	Public Sub MakeDialogFocused() Handles Me.[Shown]
		Me.Focus() : Me.Activate() : Me.BringToFront()
	End Sub

	Public Sub DismissDialog_EnsuringRequiredFieldsAreSpecified() Handles OkButton.Click
		If {Me.UsernameTextBox.Text, Me.PasswordTextBox.Text}.Contains(String.Empty) Then
			MsgBox("Please provide both a Username and Password", MsgBoxStyle.Information)
		Else
			Me.WindowClose_IsInitiatedByOkButton_ = True
			Me.Close()
		End If
	End Sub

	'Prevent Alt + F4
	Protected Overrides Sub OnFormClosing(ByVal _FormClosingEventArgs As System.Windows.Forms.FormClosingEventArgs)

		REM If the [&Ok] Button has been pressed, then do in fact close
		If Me.WindowClose_IsInitiatedByOkButton_ Then Return

		REM Otherwise, stay open
		_FormClosingEventArgs.[Cancel] = True
		MyBase.OnFormClosing(_FormClosingEventArgs)

	End Sub

	Protected Sub ShowOrHidePasswordString_DependingOnCheckboxValue_() Handles ShowPassword_CheckBox.CheckedChanged
		Me.PasswordTextBox.UseSystemPasswordChar = Not Me.ShowPassword_CheckBox.Checked
	End Sub

#End Region

End Class