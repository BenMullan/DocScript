Imports DocScript.Runtime.BuiltInFunction, DocScript.Language.Variables, DocScript.Language.Instructions, DocScript.Language.Instructions.Statements

Namespace Runtime

	Partial Public Class StandardBuiltInFunctions

		Private Const SystemBIFs_IdentifierStart_$ = "System_"

#Region "Individual BIF Member Declarations"

#Region "Data-retrival: System_Get*() BIFs"

		Private Const BIF_System_GetTime_Identifier_$ = SystemBIFs_IdentifierStart_ & "GetTime"
		Private Shared ReadOnly BIF_System_GetTime_ As New BuiltInFunction(BIF_System_GetTime_Identifier_$, {}, GetType(DSString), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_System_GetTime_Identifier_$)
			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSString(Global.System.DateTime.Now.ToString("HH:mm:ss"))
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Returns the current Time on the System in the format HH:mm:ss"}

		Private Const BIF_System_GetHostname_Identifier_$ = SystemBIFs_IdentifierStart_ & "GetHostname"
		Private Shared ReadOnly BIF_System_GetHostname_ As New BuiltInFunction(BIF_System_GetHostname_Identifier_$, {}, GetType(DSString), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_System_GetHostname_Identifier_$)
			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSString(My.Computer.Name)
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Returns the Hostname of the current Computer"}

		Private Const BIF_System_GetUsername_Identifier_$ = SystemBIFs_IdentifierStart_ & "GetUsername"
		Private Shared ReadOnly BIF_System_GetUsername_ As New BuiltInFunction(BIF_System_GetUsername_Identifier_$, {}, GetType(DSString), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_System_GetUsername_Identifier_$)
			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSString(Environment.UserName)
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Returns the Username of the current Computer"}

#End Region

		Private Const BIF_System_Beep_Identifier_$ = SystemBIFs_IdentifierStart_ & "Beep"
		Private Shared ReadOnly BIF_System_Beep_ As New BuiltInFunction(BIF_System_Beep_Identifier_$, {}, GetType(Void), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_System_Beep_Identifier_$)
			 Global.System.Console.Beep(frequency:=440, duration:=600)
			 'Global.Microsoft.VisualBasic.Interaction.Beep()
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Sounds a tone through the computer's speaker"}

		Private Const BIF_System_PlayWav_Identifier_$ = SystemBIFs_IdentifierStart_ & "PlayWav"
		Private Shared ReadOnly BIF_System_PlayWav_ As New BuiltInFunction(BIF_System_PlayWav_Identifier_$, {New DSFunction.Parameter("_WavFilePath", GetType(DSString))}, GetType(Void), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_System_PlayWav_Identifier_$)
			 My.Computer.Audio.Play(_Arguments.First().Coerce(Of DSString)().Value, AudioPlayMode.WaitToComplete)
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Plays the _WavFilePath as a Wav File"}

		Private Const BIF_System_Run_Identifier_$ = SystemBIFs_IdentifierStart_ & "Run"
		Private Shared ReadOnly BIF_System_Run_ As New BuiltInFunction(BIF_System_Run_Identifier_$, {New DSFunction.Parameter("_Command", GetType(DSString))}, GetType(Void), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_System_Run_Identifier_$)
			 Global.Microsoft.VisualBasic.Interaction.Shell(_Arguments.First().Coerce(Of DSString)().Value, AppWinStyle.NormalFocus, False, -1)
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Executes the _Command in the Shell"}

#End Region

		''' <summary>System-related Functions such as getting the current Time or executing a Shell Command</summary>
		Public Shared ReadOnly System As DocScript.Runtime.BuiltInFunction() = {
		 StandardBuiltInFunctions.BIF_System_GetTime_,
		 StandardBuiltInFunctions.BIF_System_GetHostname_,
		 StandardBuiltInFunctions.BIF_System_GetUsername_,
		 StandardBuiltInFunctions.BIF_System_Beep_,
		 StandardBuiltInFunctions.BIF_System_PlayWav_,
		 StandardBuiltInFunctions.BIF_System_Run_
		}

	End Class

End Namespace