Public Class InputExprForm

	Public Property DocScriptLoggingMode As DocScript.Logging.LogUtilities.LogEventHandler
		Get
			Return DocScript.Logging.LogUtilities.CurrentLogEventHandler
		End Get
		Set(ByVal _NewLoggingMode As DocScript.Logging.LogUtilities.LogEventHandler)
			If _NewLoggingMode = Logging.BuiltInLogEventHandlers.CLIDefault Then UsefulMethods.AllocConsole()
			DocScript.Logging.LogUtilities.CurrentLogEventHandler = _NewLoggingMode
		End Set
	End Property

	Public Sub InitialiseDocScriptAndUIComponents() Handles Me.Shown

		REM DocScript Components:
		DocScript.Logging.LogUtilities.ProcessDebugEvents = False
		Me.DocScriptLoggingMode = Logging.BuiltInLogEventHandlers.SilenceAll

		REM User Interface Components:
		Me.ToolTipAllocator.SetToolTip(Me.ResolveButton, "Ctrl + Click to show Log" & vbCrLf & "Ctrl + Shift + Click to show the Command-Line Log")
		Me.ToolTipAllocator.SetToolTip(Me.GenerateExprTreeButton, "Ctrl + Click to show Log" & vbCrLf & "Ctrl + Shift + Click to show the Command-Line Log")
		Me.ToolTipAllocator.SetToolTip(Me.ExprInputTextBox, "E.g. ""5 + 4""")

		Me.Enabled = True
		Me.ExprInputTextBox.Focus()

	End Sub

	Public Sub GenerateExprTree() Handles GenerateExprTreeButton.Click
		Try

			REM If Ctrl is being pressed, show the Log Window
			REM If Ctrl + Shift are being pressed, use the CLI Log
			If Form.ModifierKeys = (Keys.Control Or Keys.Shift) Then
				Me.DocScriptLoggingMode = Logging.BuiltInLogEventHandlers.CLIDefault
			ElseIf Form.ModifierKeys = Keys.Control Then
				Me.DocScriptLoggingMode = Logging.BuiltInLogEventHandlers.GUIDefault
			Else
				Me.DocScriptLoggingMode = Logging.BuiltInLogEventHandlers.SilenceAll
			End If

			Dim _NewExprTreeForm As New GenerateExprTreeForm(Language.Expressions.ExprUtilities.ConstructExpressionFromTokens(Runtime.Parser.GetTokensFromSource(Me.ExprInputTextBox.Text)))
			_NewExprTreeForm.Show()

		Catch _Ex As Exception
			MsgBox("An Exception was Thrown whilst generating the Expression Tree:" & vbCrLf & vbCrLf & _Ex.Message, MsgBoxStyle.Critical, _Ex.GetType().Name)
		End Try
	End Sub

	Protected Sub ResolveExpr() Handles ResolveButton.Click
		Try

			REM If Ctrl is being pressed, show the Log Window
			REM If Ctrl + Shift are being pressed, use the CLI Log
			If Form.ModifierKeys = (Keys.Control Or Keys.Shift) Then
				Me.DocScriptLoggingMode = Logging.BuiltInLogEventHandlers.CLIDefault
			ElseIf Form.ModifierKeys = Keys.Control Then
				Me.DocScriptLoggingMode = Logging.BuiltInLogEventHandlers.GUIDefault
			Else
				Me.DocScriptLoggingMode = Logging.BuiltInLogEventHandlers.SilenceAll
			End If

			Dim _NewExprResolutionForm As New ResolveExprForm(Language.Expressions.ExprUtilities.ConstructExpressionFromTokens(Runtime.Parser.GetTokensFromSource(Me.ExprInputTextBox.Text)))
			_NewExprResolutionForm.Show()

		Catch _Ex As Exception
			MsgBox("An Exception was Thrown whilst resolving the Expression:" & vbCrLf & vbCrLf & _Ex.Message, MsgBoxStyle.Critical, _Ex.GetType().Name)
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

	Public Sub ShowOptimisedExpression() Handles OptimiseLinkLabel.LinkClicked
		Try
			Dim _ComponentOptimiser As New DocScript.Runtime.Optimisation.ComponentOptimiser()
			UsefulMethods.ShowTextInNewWindow("Optimised Expr.", _ComponentOptimiser.OptimiseExpression(Language.Expressions.ExprUtilities.ConstructExpressionFromTokens(Runtime.Parser.GetTokensFromSource(Me.ExprInputTextBox.Text))).ToString() & vbCrLf & vbCrLf & _ComponentOptimiser.PerformedOptimisationsReport)
		Catch _Ex As Exception
			MsgBox("An Exception was Thrown whilst resolving the Expression:" & vbCrLf & vbCrLf & _Ex.Message, MsgBoxStyle.Critical, _Ex.GetType().Name)
		End Try
	End Sub

End Class