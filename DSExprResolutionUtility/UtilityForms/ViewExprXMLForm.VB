Public Class ViewExprXMLForm

	Public ReadOnly Expression As DocScript.Language.Expressions.IExpression

	Public Sub New(ByVal _Expr As Language.Expressions.IExpression)

		' This call is required by the designer.
		InitializeComponent()

		Me.Expression = _Expr
		'Me.Text = "Expr XML for """ & (Function(_First20ExprChars$) If(_First20ExprChars.Length >= 20, _First20ExprChars & """...", _First20ExprChars & """"c)).Invoke(New String(Me.Expression.ToString().ToCharArray().Take(20).ToArray()))
		Me.Text = "Expr XML for """ & Me.Expression.ToString() & """"c
		Me.WriteExpressionXMLToTextBox()

	End Sub

	Public Sub WriteExpressionXMLToTextBox()
		Try
			Me.ExprXMLTextBox.Text = Me.Expression.GetExprTreeNodeXML().ToString()
		Catch _Ex As Exception : Throw New DSException("@WriteExpressionXMLToTextBox: " & _Ex.Message, _Ex) : End Try
	End Sub

#Region "Close the form on pressing [Esc]"

	Protected Sub Register_CloseOnEscKey_Handler() Handles Me.Load
		Me.KeyPreview = True
		AddHandler Me.KeyDown, Sub(_Sender As [Object], _KeyEventArgs As Windows.Forms.KeyEventArgs) If _KeyEventArgs.KeyCode = Windows.Forms.Keys.Escape Then Me.Close()
	End Sub

	Protected Overrides Function ProcessDialogKey(keyData As System.Windows.Forms.Keys) As [Boolean]
		If (Windows.Forms.Form.ModifierKeys = Windows.Forms.Keys.None) AndAlso (keyData = Windows.Forms.Keys.Escape) Then
			Me.Close() : Return True
		End If
		Return MyBase.ProcessDialogKey(keyData)
	End Function

#End Region

End Class