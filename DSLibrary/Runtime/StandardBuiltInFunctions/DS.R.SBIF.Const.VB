Imports DocScript.Runtime.BuiltInFunction, DocScript.Language.Variables, DocScript.Language.Instructions, DocScript.Language.Instructions.Statements

Namespace Runtime

	Partial Public Class StandardBuiltInFunctions

		Private Const ConstBIFs_IdentifierStart_$ = "Const_"

#Region "Individual BIF Member Declarations"

#Region "Misc. Constants"

		Private Const BIF_Const_CrLf_Identifier_$ = ConstBIFs_IdentifierStart_ & "CrLf"
		Private Shared ReadOnly BIF_Const_CrLf_ As New BuiltInFunction(BIF_Const_CrLf_Identifier_, {}, GetType(DSString), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_Const_CrLf_Identifier_)
			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSString(vbCrLf)
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Returns a CarriageReturn followed by a LineFeed (for Print and Display Functions)"}

		Private Const BIF_Const_Tab_Identifier_$ = ConstBIFs_IdentifierStart_ & "Tab"
		Private Shared ReadOnly BIF_Const_Tab_ As New BuiltInFunction(BIF_Const_Tab_Identifier_, {}, GetType(DSString), New BuiltInFunctionDelegate(
		   Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			   Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_Const_Tab_Identifier_)
			   _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSString(vbTab)
			   Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		   End Function)
		) With {.[Description] = "Returns a Tab character (for Print and Display Functions)"}

#End Region

#End Region

		''' <summary>DocScript Constants - accessible via calling Const_Name()</summary>
		Public Shared ReadOnly [Const] As DocScript.Runtime.BuiltInFunction() = {
		   StandardBuiltInFunctions.BIF_Const_CrLf_,
		   StandardBuiltInFunctions.BIF_Const_Tab_
		}

	End Class

End Namespace