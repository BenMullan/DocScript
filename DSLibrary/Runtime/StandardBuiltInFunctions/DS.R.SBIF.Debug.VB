Imports DocScript.Runtime.BuiltInFunction, DocScript.Language.Variables, DocScript.Language.Instructions, DocScript.Language.Instructions.Statements

Namespace Runtime

	Partial Public Class StandardBuiltInFunctions

		Private Const DebugBIFs_IdentifierStart_$ = "Debug_"

#Region "Individual BIF Member Declarations"

		''' <summary>Needs to be publicly visible for DSIDE's [Insert Breakpoint] Button</summary>
		Public Const BIF_Break_Identifier_ = "Break"

		''' <summary>The DSIDE BIF to pause execution and e.g. inspect the current variables. In the DebugBIF's File, but not a member of the Debug Namespace</summary>
		''' <remarks>Does NOT start with "Debug_"</remarks>
		Private Shared ReadOnly BIF_Break_ As New Runtime.BuiltInFunction(BIF_Break_Identifier_$, {}, GetType(Void), New Runtime.BuiltInFunction.BuiltInFunctionDelegate(
		 Function(_SymTbls As Runtime.SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As Language.Instructions.ExecutionResult

			 Dim _ExeRes As Language.Instructions.ExecutionResult = Language.Instructions.ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_Break_Identifier_$)

			 REM The BreakpointActionDialog may return modified SymTbls, e.g. from the [Edit in DSLive] feature
			 _SymTbls = DocScript.Runtime.BreakpointActionDialog.Show_AndReturnModifiedSymTbls(_SymTbls)

			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Shows the BreakPoint-Action-Dialog, and only continues execution when this Dialog is dismissed"}

#Region "SymbolTable-Related"

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

		''' <summary>Needs to be publically visible for the BreakpointActionDialog</summary>
		Public Const BIF_Debug_ShowSymbolTables_Identifier_$ = DebugBIFs_IdentifierStart_ & "ShowSymbolTables"
		Private Shared ReadOnly BIF_Debug_ShowSymbolTables_ As New BuiltInFunction(BIF_Debug_ShowSymbolTables_Identifier_$, {}, GetType(Void), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_Debug_ShowSymbolTables_Identifier_$)

			 If Not Environment.UserInteractive Then Throw New DSException("Debug_ShowSymbolTables() cannot be called in non-UserInteractive mode.")
			 Dim _SymTblsExplorerWindow As New DocScript.Runtime.SymbolTableExplorerWindow(_SymbolTables:=_SymTbls) : _SymTblsExplorerWindow.ShowDialog()

			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Shows a graphical Window with a tab for the entries of each currently-visiable SymbolTable. Interrupts continous execution until the Windows is closed. This BIF only works in User-Interactive Mode."}

#End Region

		''' <summary>Needs to be publically visible for the BreakpointActionDialog</summary>
		Public Const BIF_Debug_StartDSLiveSessionFromCurrentExecutionState_Identifier_$ = DebugBIFs_IdentifierStart_ & "StartDSLiveSessionFromCurrentExecutionState"
		Private Shared ReadOnly BIF_Debug_StartDSLiveSessionFromCurrentExecutionState_ As New BuiltInFunction(BIF_Debug_StartDSLiveSessionFromCurrentExecutionState_Identifier_$, {}, GetType(Void), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_Debug_StartDSLiveSessionFromCurrentExecutionState_Identifier_$)

			 REM Save the _SymTbls to an XML File in AppData
			 Dim _SymTblsXMLFile_FullPath$ = IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DocScript", "DSSymbolTableStates_" & UsefulMethods.GetRandomString(6) & ".XML")
			 My.Computer.FileSystem.WriteAllText(_SymTblsXMLFile_FullPath$, _SymTbls.ToXML().ToTabFormattedString(), append:=False)

			 REM Start DSLive with the SymTbls XML
			 Process.Start(UsefulMethods.GetExecutingAssemblyDirectory.FileHereIn("DSCLI.exe").FullName, "/Live /DSLiveSymbolTablesFile:""" & _SymTblsXMLFile_FullPath$ & """"c)

			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Copies the current SymbolTables' States (including all variables and Functions) into a new DSLive Session. This allows immediate expressions to be resolved based on the current Execution State. The SymbolTables are saved to a small File in AppData\Roaming\."}

		Private Const BIF_Debug_GetStackTrace_Identifier_$ = DebugBIFs_IdentifierStart_ & "GetStackTrace"
		Private Shared ReadOnly BIF_Debug_GetStackTrace_ As New BuiltInFunction(BIF_Debug_GetStackTrace_Identifier_$, {}, GetType(DSString), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_Debug_GetStackTrace_Identifier_$)
			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSString(Global.System.Environment.StackTrace)
			 Return (_ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls))

		 End Function)
		) With {.[Description] = "Returns the DocScript-Runtime's Stack Trace. This includes all the behind-the-scenes Functions responsible for Interpreting DocScript Source."}

		Private Const BIF_Debug_GetBIFDescription_Identifier_$ = DebugBIFs_IdentifierStart_ & "GetBIFDescription"
		Private Shared ReadOnly BIF_Debug_GetBIFDescription_ As New BuiltInFunction(BIF_Debug_GetBIFDescription_Identifier_$, {New DSFunction.Parameter("_BIFIdentifier", GetType(DSString))}, GetType(DSString), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 '1) Initialise
			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_Debug_GetBIFDescription_Identifier_$)
			 Dim _TargetBIF_Identifier$ = _Arguments.First().Coerce(Of DSString)().Value

			 '2) Ensure that the SymbolTables contain an Entry for the target BIFunction
			 If Not _SymTbls.IsContainedInAny(_TargetBIF_Identifier) Then Throw New DSNonexistentSymbolException(_TargetBIF_Identifier, "The BIF Description can not be retreived")
			 Dim _TargetBIF_SymTblEntry As Runtime.SymbolTable.ISymbolTableEntry = _SymTbls.GetEntryFromAny(_TargetBIF_Identifier)
			 If Not (_TargetBIF_SymTblEntry.GetType() = GetType(Runtime.SymbolTable.SymbolTableEntry(Of Runtime.BuiltInFunction))) Then Throw New DSIncorrectSymbolTableEntryTypeException(_TargetBIF_Identifier, "A BuiltInFunction".InSquares(), _TargetBIF_SymTblEntry.GetType().FullName, "The BIF Description can not be retreived")

			 '3) Return the Description for the specified BIF
			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSString(CType(_TargetBIF_SymTblEntry, Runtime.SymbolTable.SymbolTableEntry(Of Runtime.BuiltInFunction)).Value.Description)
			 Return (_ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls))

		 End Function)
		) With {.[Description] = "Returns the Description for the Built-in Function with an identifier of _BIFIdentifier."}

#End Region

		''' <summary>DocScript Debugging Utilities for SymbolTables, BIFs, and more.</summary>
		Public Shared ReadOnly Debug As DocScript.Runtime.BuiltInFunction() = {
		 StandardBuiltInFunctions.BIF_Break_,
		 StandardBuiltInFunctions.BIF_Debug_SymbolExists_,
		 StandardBuiltInFunctions.BIF_Debug_GetVisibleSymbols_,
		 StandardBuiltInFunctions.BIF_Debug_ShowSymbolTables_,
		 StandardBuiltInFunctions.BIF_Debug_StartDSLiveSessionFromCurrentExecutionState_,
		 StandardBuiltInFunctions.BIF_Debug_GetStackTrace_,
		 StandardBuiltInFunctions.BIF_Debug_GetBIFDescription_
		}

	End Class

End Namespace