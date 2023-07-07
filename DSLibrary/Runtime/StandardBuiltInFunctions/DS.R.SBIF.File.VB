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
			 IO.File.Create(_Arguments.First().Coerce(Of DSString)().Value).Close()
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

		Private Const BIF_File_FullPathFromRelativePath_Identifier_$ = FileBIFs_IdentifierStart_ & "FullPathFromRelativePath"
		Private Shared ReadOnly BIF_File_FullPathFromRelativePath_ As New BuiltInFunction(BIF_File_FullPathFromRelativePath_Identifier_$, {New DSFunction.Parameter("_RelativePath", GetType(DSString))}, GetType(DSString), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_File_FullPathFromRelativePath_Identifier_$)
			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSString((New IO.FileInfo(_Arguments.First().Coerce(Of DSString)().Value)).FullName)
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Determines what the full path to the file is, using the context of the current working directory. The Input can be a fully-resolved file path too; this won't break anything."}

		Private Const BIF_File_Exists_Identifier_$ = FileBIFs_IdentifierStart_ & "Exists"
		Private Shared ReadOnly BIF_File_Exists_ As New BuiltInFunction(BIF_File_Exists_Identifier_$, {New DSFunction.Parameter("_FilePath", GetType(DSString))}, GetType(DSBoolean), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_File_Exists_Identifier_$)
			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSBoolean((New IO.FileInfo(_Arguments.First().Coerce(Of DSString)().Value)).Exists)
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Determines if a file at _FilePath exists."}

		Private Const BIF_File_Copy_Identifier_$ = FileBIFs_IdentifierStart_ & "Copy"
		Private Shared ReadOnly BIF_File_Copy_ As New BuiltInFunction(BIF_File_Copy_Identifier_$, {New DSFunction.Parameter("_SourceFile", GetType(DSString)), New DSFunction.Parameter("_DestinationFile", GetType(DSString))}, GetType(Void), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_File_Copy_Identifier_$)
			 IO.File.Copy(
			  sourceFileName:=_Arguments.ElementAt(0).Coerce(Of DSString)().Value,
			  destFileName:=_Arguments.ElementAt(1).Coerce(Of DSString)().Value,
			  overwrite:=True
			 )
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Copies the _SourceFile to the _DestinationFile. Overwrites the _DestinationFile if it already exists. Use File_Delete() in order to ""Move"" a file."}

#Region "Directory-related"

		Private Const BIF_File_GetDirectoryContents_Identifier_$ = FileBIFs_IdentifierStart_ & "GetDirectoryContents"
		Private Shared ReadOnly BIF_File_GetDirectoryContents_ As New BuiltInFunction(BIF_File_GetDirectoryContents_Identifier_$, {New DSFunction.Parameter("_DirectoryPath", GetType(DSString))}, GetType(DSArray(Of DSString)), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_File_GetDirectoryContents_Identifier_$)

			 '(From _Item As [String] In IO.Directory.GetFileSystemEntries(path:=_Arguments.ElementAt(0).Coerce(Of DSString)().Value) Select New DSString(_Item)).ToArray()
			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSArray(Of DSString)(
			   (New IO.DirectoryInfo(path:=_Arguments.ElementAt(0).Coerce(Of DSString)().Value)).GetFiles().Select(Of DSString)(Function(_File As IO.FileInfo) New DSString(_File.Name)) _
			   .Concat((New IO.DirectoryInfo(path:=_Arguments.ElementAt(0).Coerce(Of DSString)().Value)).GetDirectories().Select(Of DSString)(Function(_Dir As IO.DirectoryInfo) New DSString(_Dir.Name & "\"c))) _
			   .ToArray()
			)

			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Returns the (relative) names of all Files and SubDirectories in the _DirectoryPath, with directories appearing in the form ""DirName\""."}

#End Region

#End Region

		''' <summary>File-related Functions for Reading from, and Writing to, Files on Disk.</summary>
		Public Shared ReadOnly File As DocScript.Runtime.BuiltInFunction() = {
		 StandardBuiltInFunctions.BIF_File_ReadText_,
		 StandardBuiltInFunctions.BIF_File_WriteText_,
		 StandardBuiltInFunctions.BIF_File_Create_,
		 StandardBuiltInFunctions.BIF_File_Delete_,
		 StandardBuiltInFunctions.BIF_File_FullPathFromRelativePath_,
		 StandardBuiltInFunctions.BIF_File_Exists_,
		 StandardBuiltInFunctions.BIF_File_Copy_,
		 StandardBuiltInFunctions.BIF_File_GetDirectoryContents_
		}

	End Class

End Namespace