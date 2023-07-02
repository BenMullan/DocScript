Namespace Runtime

	''' <summary>The Window shown when "Break()" is called from the DocScript Program.</summary>
	Partial Public Class BreakpointActionDialog

		Public ReadOnly ProvidedSymbolTables As DocScript.Runtime.SymbolTablesSnapshot
		Public SymbolTables_StateToReturn As DocScript.Runtime.SymbolTablesSnapshot = Nothing

		Protected Sub New(ByRef _SymTbls As SymbolTablesSnapshot)

			Me.InitializeComponent()
			Me.ProvidedSymbolTables = _SymTbls.MustNotBeNothing("The SymbolTables passed to the BreakpointActionDialog cannot be Nothing (null).")

		End Sub

		''' <summary>Shows the Dialog, and returns only after one of the actions has been performed, or the dialog has been dismissed.</summary>
		Public Shared Function Show_AndReturnModifiedSymTbls(ByVal _InputSymbolTables As SymbolTablesSnapshot) As SymbolTablesSnapshot
			Try

				Dim _NewBreakpointActionDialog As New BreakpointActionDialog(_InputSymbolTables)

				_NewBreakpointActionDialog.ShowDialog()

				If _NewBreakpointActionDialog.SymbolTables_StateToReturn Is Nothing Then Throw New DSException("The Modified-Symbol-Tables-State was not set by the Breakpoint Action")

				Return _NewBreakpointActionDialog.SymbolTables_StateToReturn

			Catch _Ex As Exception : MsgBox(_Ex.Message & vbCrLf & vbCrLf & "The original SymbolTables' State will be returned instead of the modified one.", MsgBoxStyle.Exclamation, "Breakpoint Dialog") : Return _InputSymbolTables : End Try
		End Function

#Region "Close the form on pressing [Esc]. Modified for BreakpointActionDialog."

		Protected Sub Register_CloseOnEscKey_Handler() Handles Me.Load
			Me.KeyPreview = True
			AddHandler Me.KeyDown, Sub(_Sender As [Object], _KeyEventArgs As Windows.Forms.KeyEventArgs) If _KeyEventArgs.KeyCode = Windows.Forms.Keys.Escape Then Me.BPAction_ContinueExec_()
		End Sub

		Protected Overrides Function ProcessDialogKey(keyData As System.Windows.Forms.Keys) As [Boolean]
			If (Windows.Forms.Form.ModifierKeys = Windows.Forms.Keys.None) AndAlso (keyData = Windows.Forms.Keys.Escape) Then
				Me.BPAction_ContinueExec_() : Return True
			End If
			Return MyBase.ProcessDialogKey(keyData)
		End Function

#End Region

#Region "Breakpoint Actions"

		''' <summary>Sets the [SymbolTables_StateToReturn] member</summary>
		Protected Sub BPAction_ViewSymTbls_() Handles ViewSymTblsButton.Click
			Try

				REM Call: Debug_ShowSymbolTables()

				Dim _BIF_ExeRes As Language.Instructions.ExecutionResult = _
				 Global.DocScript.Language.Instructions.FunctionCall.CallFunctionByName(
				  _InputSymbolTables:=Me.ProvidedSymbolTables,
				  _TargetFunction_Identifier:=StandardBuiltInFunctions.BIF_Debug_ShowSymbolTables_Identifier_,
				  _UnresolvedArguments:={},
				  _MustProduceReturnValue:=False
				 )

				Me.SymbolTables_StateToReturn = _BIF_ExeRes.ResultantSymbolTablesState

			Catch _Ex As Exception : Throw New DSException("The SymbolTables cannot be shown: " & _Ex.Message) : End Try
		End Sub

		''' <summary>Sets the [SymbolTables_StateToReturn] member</summary>
		Protected Sub BPAction_CopyToDSLive_() Handles CopyToDSLiveButton.Click
			Try

				REM Call: Debug_StartDSLiveSessionFromCurrentExecutionState()

				Dim _BIF_ExeRes As Language.Instructions.ExecutionResult = _
				 Global.DocScript.Language.Instructions.FunctionCall.CallFunctionByName(
				  _InputSymbolTables:=Me.ProvidedSymbolTables,
				  _TargetFunction_Identifier:=StandardBuiltInFunctions.BIF_Debug_StartDSLiveSessionFromCurrentExecutionState_Identifier_,
				  _UnresolvedArguments:={},
				  _MustProduceReturnValue:=False
				 )

				Me.SymbolTables_StateToReturn = _BIF_ExeRes.ResultantSymbolTablesState

			Catch _Ex As Exception : Throw New DSException("The DSLive session could not be started: " & _Ex.Message) : End Try
		End Sub

		''' <summary>Sets the [SymbolTables_StateToReturn] member</summary>
		Protected Sub BPAction_EditInDSLive_() Handles EditInDSLiveButton.Click
			Try

				Throw New NotImplementedException("BPAction_EditInDSLive_ is not yet implemented")

				REM Call: Debug_EditSymbolTablesWithDSLive()

				'Dim _BIF_ExeRes As Language.Instructions.ExecutionResult = _
				' Global.DocScript.Language.Instructions.FunctionCall.CallFunctionByName(
				'  _InputSymbolTables:=Me.ProvidedSymbolTables,
				'  _TargetFunction_Identifier:=StandardBuiltInFunctions.BIF_Debug_StartDSLiveSessionFromCurrentExecutionState_Identifier_,
				'  _UnresolvedArguments:={},
				'  _MustProduceReturnValue:=False
				' )

				'Me.SymbolTables_StateToReturn = _BIF_ExeRes.ResultantSymbolTablesState

			Catch _Ex As Exception : Throw New DSException("The DSLive session could not be started: " & _Ex.Message) : End Try
		End Sub

		''' <summary>Sets the [SymbolTables_StateToReturn] member</summary>
		Protected Sub BPAction_ContinueExec_() Handles ContinueExecButton.Click
			Me.SymbolTables_StateToReturn = Me.ProvidedSymbolTables
			Me.Close()
		End Sub

#End Region

		Protected Sub IgnoreAllFutureBreakPointsCheckBox_CheckedChanged_() Handles IgnoreAllFutureBreakPointsCheckBox.CheckedChanged
			DocScript.Runtime.StandardBuiltInFunctions.IgnoreBreakpoints = Me.IgnoreAllFutureBreakPointsCheckBox.Checked
		End Sub

	End Class

End Namespace