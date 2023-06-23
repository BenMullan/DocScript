Public Class GenerateExprTreeForm

	Public ReadOnly Expression As Language.Expressions.IExpression

	Public Sub New(ByVal _Expr As DocScript.Language.Expressions.IExpression)

		'This call is required by the designer.
		InitializeComponent()

		Me.Expression = _Expr
		'Me.Text = "ExprTree for """ & (Function(_First20ExprChars$) If(_First20ExprChars.Length >= 20, _First20ExprChars & """...", _First20ExprChars & """"c)).Invoke(New String(Me.Expression.ToString().ToCharArray().Take(20).ToArray()))
		Me.Text = "ExprTree for """ & Me.Expression.ToString() & """"c
		Me.RenderExprOntoTreeView()

	End Sub

	Public Sub RenderExprOntoTreeView()
		Try

			Me.ExprTreeView.BeginUpdate()
			Me.ExprTreeView.Nodes.Add(New TreeNode("Expr " & Me.Expression.GetType().Name.InBrackets(), {DocScript.Language.Expressions.ExprUtilities.GetTreeViewNodeFromExpr(Me.Expression)}))
			Me.ExprTreeView.EndUpdate()
			Me.ExprTreeView.ExpandAll()

		Catch _Ex As Exception : Throw New DSException("@RenderExprOntoTreeView: " & _Ex.Message, _Ex) : End Try
	End Sub

	Public Sub ViewXML() Handles ViewXMLToolStripMenuItem.Click
		Try

			REM If Ctrl is being pressed, show the Log Window
			REM If Ctrl + Shift are being pressed, use the CLI Log
			If Form.ModifierKeys = (Keys.Control Or Keys.Shift) Then
				InputExprForm.DocScriptLoggingMode = Logging.BuiltInLogEventHandlers.CLIDefault
			ElseIf Form.ModifierKeys = Keys.Control Then
				InputExprForm.DocScriptLoggingMode = Logging.BuiltInLogEventHandlers.GUIDefault
			Else
				InputExprForm.DocScriptLoggingMode = Logging.BuiltInLogEventHandlers.SilenceAll
			End If

			UsefulMethods.ShowTextInNewWindow("Expr. Tree XML", Me.Expression.GetExprTreeNodeXML().ToTabFormattedString())

		Catch _Ex As Exception
			MsgBox("An Exception was Thrown whilst generating and viewing the Expression XML:" & vbCrLf & vbCrLf & _Ex.Message, MsgBoxStyle.Critical, _Ex.GetType().Name)
		End Try
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