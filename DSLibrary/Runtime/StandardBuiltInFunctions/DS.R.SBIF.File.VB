Imports DocScript.Runtime.BuiltInFunction, DocScript.Language.Variables, DocScript.Language.Instructions, DocScript.Language.Instructions.Statements

Namespace Runtime

	Partial Public Class StandardBuiltInFunctions

		Private Const FileBIFs_IdentifierStart_$ = "File_"

#Region "Individual BIF Member Declarations"

		Private Const BIF_File_ReadText_Identifier_$ = FileBIFs_IdentifierStart_ & "ReadText"
		Private Shared ReadOnly BIF_File_ReadText_ As New BuiltInFunction(BIF_File_ReadText_Identifier_$, {New DSFunction.Parameter("_FilePath", GetType(DSString))}, GetType(DSString), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_File_ReadText_Identifier_$)
			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSString(My.Computer.FileSystem.ReadAllText(_Arguments.First().Coerce(Of DSString)().Value))
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Returns all Text from inside the specified File"}

		Private Const BIF_File_WriteText_Identifier_$ = FileBIFs_IdentifierStart_ & "WriteText"
		Private Shared ReadOnly BIF_File_WriteText_ As New BuiltInFunction(BIF_File_WriteText_Identifier_$, {New DSFunction.Parameter("_FilePath", GetType(DSString)), New DSFunction.Parameter("_Text", GetType(DSString)), New DSFunction.Parameter("_Append", GetType(DSBoolean))}, GetType(Void), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_File_WriteText_Identifier_$)
			 My.Computer.FileSystem.WriteAllText(_Arguments.ElementAt(0).Coerce(Of DSString)().Value, _Arguments.ElementAt(1).Coerce(Of DSString)().Value, _Arguments.ElementAt(2).Coerce(Of DSBoolean)().Value)
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Writes the _Text to the File at _FilePath"}

		Private Const BIF_File_Create_Identifier_$ = FileBIFs_IdentifierStart_ & "Create"
		Private Shared ReadOnly BIF_File_Create_ As New BuiltInFunction(BIF_File_Create_Identifier_$, {New DSFunction.Parameter("_FilePath", GetType(DSString))}, GetType(Void), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_File_Create_Identifier_$)
			 IO.File.Create(_Arguments.First().Coerce(Of DSString)().Value)
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Creates the File at _FilePath"}

		Private Const BIF_File_Delete_Identifier_$ = FileBIFs_IdentifierStart_ & "Delete"
		Private Shared ReadOnly BIF_File_Delete_ As New BuiltInFunction(BIF_File_Delete_Identifier_$, {New DSFunction.Parameter("_FilePath", GetType(DSString))}, GetType(Void), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_File_Delete_Identifier_$)
			 IO.File.Delete(_Arguments.First().Coerce(Of DSString)().Value)
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Deletes the File at _FilePath"}

#End Region

		''' <summary>File-related Functions for Reading from, and Writing to, Files on Disk.</summary>
		Public Shared ReadOnly File As DocScript.Runtime.BuiltInFunction() = {
		 StandardBuiltInFunctions.BIF_File_ReadText_,
		 StandardBuiltInFunctions.BIF_File_WriteText_,
		 StandardBuiltInFunctions.BIF_File_Create_,
		 StandardBuiltInFunctions.BIF_File_Delete_
		}

	End Class

End Namespace