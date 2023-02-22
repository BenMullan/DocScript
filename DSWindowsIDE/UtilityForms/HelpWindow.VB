Public Class HelpWindow

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

	Public Sub ShowPictorialHelpWindow() Handles StartPictorialHelp.Click
		Call (New PictorialHelpWindow).Show(Me)
	End Sub

	Private Sub ShowPictorialHelpWindow(sender As System.Object, e As System.EventArgs) Handles StartPictorialHelp.Click

	End Sub
End Class