Imports DocScript.Runtime.BuiltInFunction, DocScript.Language.Variables, DocScript.Language.Instructions, DocScript.Language.Instructions.Statements

Public Module DSIBuiltInFunctions

	Private Const DSIBIFs_IdentifierStart_$ = "DSI_"

#Region "Individual BIF Member Declarations"

	Private Const BIF_DSI_InjectCEP_Identifier_$ = DSIBIFs_IdentifierStart_ & "InjectCEP"
	Private ReadOnly Property BIF_DSI_InjectCEP_(ByVal _ESID$) As BuiltInFunction
		Get

			Return New BuiltInFunction(BIF_DSI_InjectCEP_Identifier_, {New DSFunction.Parameter("_JavaScriptToRun", GetType(DSString))}, GetType(Void), New BuiltInFunctionDelegate(
			  Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

				  Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("DSI-BIF\" & BIF_DSI_InjectCEP_Identifier_)
				  WebParts.DatabaseInteraction.AddExecutionSessionCEP(_ESID, _Arguments.First().Coerce(Of DSString)().Value)
				  Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

			  End Function)
			 ) With {.[Description] = "Injects the _JavaScriptToRun as a CEP, into the current Execution-Session"}

		End Get
	End Property

	Private Const BIF_DSI_GetCurrentESID_Identifier_$ = DSIBIFs_IdentifierStart_ & "GetCurrentESID"
	Private ReadOnly Property BIF_DSI_GetCurrentESID_(ByVal _ESID$) As BuiltInFunction
		Get

			Return New BuiltInFunction(BIF_DSI_GetCurrentESID_Identifier_, {}, GetType(DSString), New BuiltInFunctionDelegate(
			  Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

				  Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("DSI-BIF\" & BIF_DSI_GetCurrentESID_Identifier_)
				  _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSString(_ESID)
				  Return (_ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls))

			  End Function)
			 ) With {.[Description] = "Returns the ID of the current Execution-Session"}

		End Get
	End Property

	Private Const BIF_DSI_MsgBox_Identifier_$ = DSIBIFs_IdentifierStart_ & "MsgBox"
	Private ReadOnly Property BIF_DSI_MsgBox_(ByVal _ESID$) As BuiltInFunction
		Get

			Return New BuiltInFunction(BIF_DSI_MsgBox_Identifier_, {New DSFunction.Parameter("_MessageText", GetType(DSString))}, GetType(Void), New BuiltInFunctionDelegate(
			  Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

				  Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("DSI-BIF\" & BIF_DSI_MsgBox_Identifier_)
				  WebParts.DatabaseInteraction.AddExecutionSessionCEP(
				   _ESID,
				   "DSI.MsgBox(`" & _Arguments.First().Coerce(Of DSString)().Value & "`);"
				  )
				  Return (_ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls))

			  End Function)
			 ) With {.[Description] = "Injects a CEP to call DSI.MsgBox() in client-side JavaScript"}

		End Get
	End Property

#End Region

	''' <summary>DocScript Interactive BIFs - accessible via calling DSI_{BIFName}() in the DocScript DSI Program Source.</summary>
	Public ReadOnly Property [All_DSI_BuiltInFunctions](ByVal _ESID$) As DocScript.Runtime.BuiltInFunction()
		Get
			Return {
			 Global.DocScript.Interactive.ESWorker.DSIBuiltInFunctions.BIF_DSI_InjectCEP_(_ESID),
			 Global.DocScript.Interactive.ESWorker.DSIBuiltInFunctions.BIF_DSI_GetCurrentESID_(_ESID),
			 Global.DocScript.Interactive.ESWorker.DSIBuiltInFunctions.BIF_DSI_MsgBox_(_ESID)
			}
		End Get
	End Property

End Module