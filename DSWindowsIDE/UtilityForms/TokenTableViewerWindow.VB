Public Class TokenTableViewerWindow

	Public ReadOnly Tokens As DocScript.Runtime.Token()

	Public Sub New(ByVal _Tokens As Runtime.Token())

		Me.InitializeComponent()
		Me.Tokens = _Tokens

		'Add the records to the Tokens' Table
		For _Index% = 0 To (Me.Tokens.Length - 1) Step +1
			Me.TokensTable_DataGridView.Rows.Add(
			 (_Index + 1).ToString(),
			 Me.Tokens.ElementAt(_Index).Type.ToString("G"),
			 Me.Tokens.ElementAt(_Index).Value,
			 Me.Tokens.ElementAt(_Index).LocationInSource.ToShortString()
			)
		Next

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