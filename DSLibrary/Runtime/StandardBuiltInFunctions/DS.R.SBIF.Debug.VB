Imports DocScript.Runtime.BuiltInFunction, DocScript.Language.Variables, DocScript.Language.Instructions, DocScript.Language.Instructions.Statements

Namespace Runtime

	Partial Public Class StandardBuiltInFunctions

		Private Const DebugBIFs_IdentifierStart_$ = "Debug_"

#Region "Individual BIF Member Declarations"

		Private Const BIF_Debug_SymbolExists_Identifier_$ = DebugBIFs_IdentifierStart_ & "SymbolExists"
		Private Shared ReadOnly BIF_Debug_SymbolExists_ As New BuiltInFunction(BIF_Debug_SymbolExists_Identifier_$, {New DSFunction.Parameter("_SymbolName", GetType(DSString))}, GetType(DSBoolean), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_Debug_SymbolExists_Identifier_$)
			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSBoolean(_SymTbls.IsContainedInAny(_Arguments.First().Coerce(Of DSString)().Value))
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Returns True, if ANY of the SymbolTables in the current scope contain an Entry with the identifier of _SymbolName"}

		Private Const BIF_Debug_GetVisibleSymbols_Identifier_$ = DebugBIFs_IdentifierStart_ & "GetVisibleSymbols"
		Private Shared ReadOnly BIF_Debug_GetVisibleSymbols_ As New BuiltInFunction(BIF_Debug_GetVisibleSymbols_Identifier_$, {}, GetType(DSArray(Of DSString)), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_Debug_GetVisibleSymbols_Identifier_$)

			 Dim _AllSymbolIdentifiers_AsDSStrings As New List(Of DSString)()
			 For Each _SymbolTable As SymbolTable In _SymTbls.SymbolTables_ReadOnlyCopy
				 For Each _Entry As KeyValuePair(Of String, SymbolTable.ISymbolTableEntry) In _SymbolTable.Entries_ReadOnlyCopy
					 _AllSymbolIdentifiers_AsDSStrings.Add(New DSString(_Entry.Key))
				 Next
			 Next

			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSArray(Of DSString)(_AllSymbolIdentifiers_AsDSStrings.ToArray())
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Returns all of the identifiers from currently-visible SymbolTable Entries"}

		Private Const BIF_Debug_ShowSymbolTables_Identifier_$ = DebugBIFs_IdentifierStart_ & "ShowSymbolTables"
		Private Shared ReadOnly BIF_Debug_ShowSymbolTables_ As New BuiltInFunction(BIF_Debug_ShowSymbolTables_Identifier_$, {}, GetType(Void), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_Debug_ShowSymbolTables_Identifier_$)

			 If Not Environment.UserInteractive Then Throw New DSException("Debug_ShowSymbolTables() cannot be called in non-UserInteractive mode.")
			 Dim _SymTblsExplorerWindow As New DocScript.Runtime.SymbolTableExplorerWindow(_SymbolTables:=_SymTbls) : _SymTblsExplorerWindow.ShowDialog()

			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Shows a graphical Window with a tab for the entries of each currently-visiable SymbolTable. Interrupts continous execution until the Windows is closed. This BIF only works in User-Interactive Mode."}

		Private Const BIF_Debug_GetStackTrace_Identifier_$ = DebugBIFs_IdentifierStart_ & "GetStackTrace"
		Private Shared ReadOnly BIF_Debug_GetStackTrace_ As New BuiltInFunction(BIF_Debug_GetStackTrace_Identifier_$, {}, GetType(DSString), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_Debug_GetStackTrace_Identifier_$)
			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSString(Global.System.Environment.StackTrace)
			 Return (_ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls))

		 End Function)
		) With {.[Description] = "Returns the DocScript-Runtime's Stack Trace. This includes all the behind-the-scenes Functions responsible for Interpreting DocScript Source."}

#End Region

		''' <summary>DocScript Language Utilities</summary>
		Public Shared ReadOnly Debug As DocScript.Runtime.BuiltInFunction() = {
		 StandardBuiltInFunctions.BIF_Debug_SymbolExists_,
		 StandardBuiltInFunctions.BIF_Debug_GetVisibleSymbols_,
		 StandardBuiltInFunctions.BIF_Debug_ShowSymbolTables_,
		 StandardBuiltInFunctions.BIF_Debug_GetStackTrace_
		}

	End Class

End Namespace