Public Class ResolveExprForm

	Public ReadOnly Expression As Language.Expressions.IExpression

	Public Sub New(ByVal _Expr As Language.Expressions.IExpression)

		' This call is required by the designer.
		InitializeComponent()
		AddHandler Me.OkButton.Click, Sub() Me.Close()

		Me.Expression = _Expr
		Me.InputExprLabel.Text = Me.Expression.ToString()

		Me.ResolveExpr_AndShowResult()

	End Sub

	Public Sub ResolveExpr_AndShowResult()

		'Any Exceptions Thrown hence are caught downstairs by the Object which instanciated the Form

		Dim _GlobalSymbolTable_InSnapshot As New DocScript.Runtime.SymbolTablesSnapshot(
		 _BottomStack:=Runtime.SymbolTablesSnapshot.Empty,
		 _Topmost:=DocScript.Runtime.Program.GenerateGlobalSymbolTable(Runtime.ExecutionContext.GUIDefault)
		 )

		Me.ExprResultLabel.Text = Me.Expression _
		 .Resolve(_GlobalSymbolTable_InSnapshot) _
		 .ReturnStatus.IExpression_ResolutionResult _
		 .ToString()

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