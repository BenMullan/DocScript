Imports DocScript.Runtime.BuiltInFunction, DocScript.Language.Variables, DocScript.Language.Instructions, DocScript.Language.Instructions.Statements

Namespace Runtime

	Partial Public Class StandardBuiltInFunctions

		Private Const DSBIFs_IdentifierStart_$ = "DS_"

#Region "Individual BIF Member Declarations"

#Region "Misc. DS BIFs"

		Private Const BIF_DS_Sleep_Identifier_$ = DSBIFs_IdentifierStart_ & "Sleep"
		Private Shared ReadOnly BIF_DS_Sleep_ As New BuiltInFunction(BIF_DS_Sleep_Identifier_$, {New DSFunction.Parameter("_MilliSeconds", GetType(DSNumber))}, GetType(Void), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_DS_Sleep_Identifier_$)
			 Global.System.Threading.Thread.Sleep(Convert.ToInt32(_Arguments.First().Coerce(Of DSNumber)().Value))
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Interrupts continous execution for a duration of _MilliSeconds"}

		Private Const BIF_DS_ThrowError_Identifier_$ = DSBIFs_IdentifierStart_ & "ThrowError"
		Private Shared ReadOnly BIF_DS_ThrowError_ As New BuiltInFunction(BIF_DS_ThrowError_Identifier_$, {New DSFunction.Parameter("_ErrorMsg", GetType(DSString))}, GetType(Void), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_DS_ThrowError_Identifier_$)
			 Throw New DSSourceGeneratedException(_Arguments.First().Coerce(Of DSString)().Value)
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls) '(Never gets called)

		 End Function)
		) With {.[Description] = "Stops execution immediately, Throwing a New DSSourceGeneratedException with the specified _ErrorMsg"}

		Private Const BIF_DS_Interpret_Identifier_$ = DSBIFs_IdentifierStart_ & "Interpret"
		Private Shared ReadOnly BIF_DS_Interpret_ As New BuiltInFunction(BIF_DS_Interpret_Identifier_$, {New DSFunction.Parameter("_Source", GetType(DSString))}, GetType(Void), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_DS_Interpret_Identifier_$)
			 Dim _Modified_SymTbls As Runtime.SymbolTablesSnapshot = _SymTbls

			 Dim _Tokens As Runtime.Token() = Parser.GetTokensFromSource(Parser.UnflattenSource(_Arguments.First().Coerce(Of DSString)().Value))
			 Dim _Instructions As Language.Instructions.IInstruction() = Language.Instructions.Statements.GetStatementContentsFromTokens(_Tokens)

			 'Just in case multiple Instructions were specified...
			 For Each _Instruction As Language.Instructions.IInstruction In _Instructions
				 Dim _Instruction_ExeRes As Language.Instructions.ExecutionResult = _Instruction.Execute(_Modified_SymTbls)
				 _ExeRes.UpstairsExecutionResults.Add(New Tuple(Of String, ExecutionResult)("Lambda " & _Instruction.GetType().Name.InBrackets(), _Instruction_ExeRes))
				 _Modified_SymTbls = _Instruction_ExeRes.ResultantSymbolTablesState
			 Next

			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_Modified_SymTbls)

		 End Function)
		) With {.[Description] = "Runs _Source as statement-level DocScript source, in the context of the current Symbol-Tables. Seperate lines with ; and use ` for SpeeachMarks ("")"}

		Private Const BIF_DS_InterpretTry_Identifier_$ = DSBIFs_IdentifierStart_ & "InterpretTry"
		Private Shared ReadOnly BIF_DS_InterpretTry_ As New BuiltInFunction(BIF_DS_InterpretTry_Identifier_$, {New DSFunction.Parameter("_Source", GetType(DSString))}, GetType(DSBoolean), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_DS_InterpretTry_Identifier_$)
			 Dim _Modified_SymTbls As Runtime.SymbolTablesSnapshot = _SymTbls

			 Try

				 Dim _Tokens As Runtime.Token() = Parser.GetTokensFromSource(Parser.UnflattenSource(_Arguments.First().Coerce(Of DSString)().Value))
				 Dim _Instructions As Language.Instructions.IInstruction() = Language.Instructions.Statements.GetStatementContentsFromTokens(_Tokens)

				 'Just in case multiple Instructions were specified...
				 For Each _Instruction As Language.Instructions.IInstruction In _Instructions
					 Dim _Instruction_ExeRes As Language.Instructions.ExecutionResult = _Instruction.Execute(_Modified_SymTbls)
					 _ExeRes.UpstairsExecutionResults.Add(New Tuple(Of String, ExecutionResult)("Lambda " & _Instruction.GetType().Name.InBrackets(), _Instruction_ExeRes))
					 _Modified_SymTbls = _Instruction_ExeRes.ResultantSymbolTablesState
				 Next

				 LogExecutionMessage("The DocScript run within DS_InterpretTry() did not throw an Exception; the BIF will Return True", LogEvent.DSEventSeverity.Infomation)
				 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSBoolean(True)

			 Catch _Ex As Exception
				 LogExecutionMessage("The DocScript run within DS_InterpretTry() threw an Exception; the BIF will Return False. The Exception's Message was: " & _Ex.Message, LogEvent.DSEventSeverity.Warning)
				 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSBoolean(False)
			 End Try

			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_Modified_SymTbls)

		 End Function)
		) With {.[Description] = "Runs _Source as statement-level DocScript source, in the context of the current Symbol-Tables. If an exception is thrown, this Function returns False. If all is well, it returns True. Seperate lines with ; and use ` for SpeeachMarks ("")"}

		Private Const BIF_DS_InterpretProgram_Identifier_$ = DSBIFs_IdentifierStart_ & "InterpretProgram"
		Private Shared ReadOnly BIF_DS_InterpretProgram_ As New BuiltInFunction(BIF_DS_InterpretProgram_Identifier_$, {New DSFunction.Parameter("_ProgramSource", GetType(DSString)), New DSFunction.Parameter("_CLAs", GetType(DSString)), New DSFunction.Parameter("_ExeCxtMode", GetType(DSString))}, GetType(DSNumber), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_DS_InterpretProgram_Identifier_$)

			 Dim _ExeCxtToUse As Runtime.ExecutionContext
			 Select Case _Arguments.ElementAt(2).Coerce(Of DSString)().Value.ToUpper()
				 Case "GUI" : _ExeCxtToUse = ExecutionContext.GUIDefault
				 Case "CLI" : _ExeCxtToUse = ExecutionContext.CLIDefault
				 Case Else : Throw New DSValidationException("The _ExeCxtMode was not recognised as a DocScript ExecutionContext. It must be either GUI or CLI", "Specified ExeCxt-Mode: " & _Arguments.ElementAt(2).Coerce(Of DSString)().Value.ToUpper())
			 End Select

			 Dim _Program As New DocScript.Runtime.Program(
			  _Tokens:=DocScript.Runtime.Parser.GetTokensFromSource(_RawSource:=Parser.UnflattenSource(_Arguments.ElementAt(0).Coerce(Of DSString)().Value)),
			  _ExecutionContext:=_ExeCxtToUse
			)

			 Dim _Program_ExeRes As Language.Instructions.ExecutionResult = _Program.Run(_Arguments.ElementAt(1).Coerce(Of DSString)().Value.Split(" "c))
			 Dim _ExitCode As Int32 = _Program_ExeRes.ReturnStatus.Program_ExitCode.GetValueOrDefault(defaultValue:=DocScript.Runtime.Constants.ProgramExitCode_Default)

			 _ExeRes.UpstairsExecutionResults.Add(New Tuple(Of String, ExecutionResult)("DocScript Program", _Program_ExeRes))
			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSNumber(_ExitCode)
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Runs _ProgramSource as a complete DS Program (with a Main Function), returning the ExitCode of that DS Program. The _CLAs are the space-delimited command-line arguments to the DocScript Program, and the _ExeCxtMode must be either ""GUI"" or ""CLI"". Seperate lines with ; and use ` for SpeeachMarks ("")"}

#End Region

#Region "DS_String BIFs"

		Private Const BIF_DS_String_Split_Identifier_$ = DSBIFs_IdentifierStart_ & "String_Split"
		Private Shared ReadOnly BIF_DS_String_Split_ As New BuiltInFunction(BIF_DS_String_Split_Identifier_$, {New DSFunction.Parameter("_String", GetType(DSString)), New DSFunction.Parameter("_SplitAtSequence", GetType(DSString))}, GetType(DSArray(Of DSString)), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_DS_String_Split_Identifier_$)

			 Dim _ArrayToReturn As String() = _
			 _Arguments.ElementAt(0).Coerce(Of DSString)().Value _
			 .Split({_Arguments.ElementAt(1).Coerce(Of DSString)().Value}, Global.System.StringSplitOptions.None)

			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSArray(Of DSString)((From _Item As String In _ArrayToReturn Select New DSString(_Item)).ToArray())

			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Splits the _String at each occurance of the _SplitAtSequence of chars, returning <String@>"}

		Private Const BIF_DS_String_Replace_Identifier_$ = DSBIFs_IdentifierStart_ & "String_Replace"
		Private Shared ReadOnly BIF_DS_String_Replace_ As New BuiltInFunction(BIF_DS_String_Replace_Identifier_$, {New DSFunction.Parameter("_String", GetType(DSString)), New DSFunction.Parameter("_SubstringToBeReplaced", GetType(DSString)), New DSFunction.Parameter("_Replacement", GetType(DSString))}, GetType(DSString), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_DS_String_Replace_Identifier_$)

			 Dim _ArrayToReturn As String() = _
			 _Arguments.ElementAt(0).Coerce(Of DSString)().Value _
			 .Split({_Arguments.ElementAt(1).Coerce(Of DSString)().Value}, Global.System.StringSplitOptions.None)

			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSString(
			  _Arguments.ElementAt(0).Coerce(Of DSString)().Value.Replace(oldValue:=_Arguments.ElementAt(1).Coerce(Of DSString)().Value, newValue:=_Arguments.ElementAt(2).Coerce(Of DSString)().Value)
			 )

			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Replaces all instances of the _SubstringToBeReplaced that occur within the _String, with the _Replacement"}

		Private Const BIF_DS_String_Length_Identifier_$ = DSBIFs_IdentifierStart_ & "String_Length"
		Private Shared ReadOnly BIF_DS_String_Length_ As New BuiltInFunction(BIF_DS_String_Length_Identifier_$, {New DSFunction.Parameter("_String", GetType(DSString))}, GetType(DSNumber), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_DS_String_Length_Identifier_$)
			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSNumber(_Arguments.First().Coerce(Of DSString)().Value.Length)
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Determines the number of Characters in the String"}

		Private Const BIF_DS_String_At_Identifier_$ = DSBIFs_IdentifierStart_ & "String_At"
		Private Shared ReadOnly BIF_DS_String_At_ As New BuiltInFunction(BIF_DS_String_At_Identifier_$, {New DSFunction.Parameter("_String", GetType(DSString)), New DSFunction.Parameter("_Index", GetType(DSNumber))}, GetType(DSString), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Try

				 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_DS_String_At_Identifier_$)

				 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = _
				 New DSString(
				  _Arguments.ElementAt(0).Coerce(Of DSString)().Value.ToCharArray().ElementAt(
				  Convert.ToInt32(_Arguments.ElementAt(1).Coerce(Of DSNumber)().Value)
				 ).ToString()
				)

				 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

			 Catch _Ex As Exception
				 Throw New DSException(_Ex.Message & " The _String Length was " & _Arguments.ElementAt(0).Coerce(Of DSString)().Value.Length.ToString().InSquares() & ", and the _Index was " & Convert.ToInt32(_Arguments.ElementAt(1).Coerce(Of DSNumber)().Value).ToString().InSquares(), _Ex)
			 End Try

		 End Function)
		) With {.[Description] = "Returns the Character at _Index in the _String"}

		Private Const BIF_DS_String_Contains_Identifier_$ = DSBIFs_IdentifierStart_ & "String_Contains"
		Private Shared ReadOnly BIF_DS_String_Contains_ As New BuiltInFunction(BIF_DS_String_Contains_Identifier_$, {New DSFunction.Parameter("_WholeString", GetType(DSString)), New DSFunction.Parameter("_SubString", GetType(DSString))}, GetType(DSBoolean), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_DS_String_Contains_Identifier_$)
			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSBoolean(_Arguments.ElementAt(0).Coerce(Of DSString)().Value.Contains(_Arguments.ElementAt(1).Coerce(Of DSString)().Value))
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Determines if the _WholeString contains the _SubString within it"}

		Private Const BIF_DS_String_If_Identifier_$ = DSBIFs_IdentifierStart_ & "String_If"
		Private Shared ReadOnly BIF_DS_String_If_ As New BuiltInFunction(BIF_DS_String_If_Identifier_$, {New DSFunction.Parameter("_ConditionExpr", GetType(DSBoolean)), New DSFunction.Parameter("_TruePart", GetType(DSString)), New DSFunction.Parameter("_FalsePart", GetType(DSString))}, GetType(DSString), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_DS_String_If_Identifier_$)

			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = If(
			  _Arguments.ElementAt(0).Coerce(Of DSBoolean)().Value,
			  _Arguments.ElementAt(1).Coerce(Of DSString)(),
			  _Arguments.ElementAt(2).Coerce(Of DSString)()
			 )

			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Returns _TruePart if _ConditionExpr evaluates to True, otherwise returns _FalsePart"}

		Private Const BIF_DS_String_GetRandom_Identifier_$ = DSBIFs_IdentifierStart_ & "String_GetRandom"
		Private Shared ReadOnly BIF_DS_String_GetRandom_ As New BuiltInFunction(BIF_DS_String_GetRandom_Identifier_$, {New DSFunction.Parameter("_SoughtLength", GetType(DSNumber))}, GetType(DSString), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_DS_String_GetRandom_Identifier_$)

			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSString(
			 Global.DocScript.CompilerExtentions.UsefulMethods.GetRandomString(
				_WantedLength:=Global.System.Convert.ToUInt16(_Arguments.First().Coerce(Of DSNumber)().Value)
			   )
			  )

			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Returns a random string of length _SoughtLength chars"}

#End Region

#Region "DS_Number BIFs"

		Private Const BIF_DS_Number_ToBase_Identifier_$ = DSBIFs_IdentifierStart_ & "Number_ToBase"
		Private Shared ReadOnly BIF_DS_Number_ToBase_ As New BuiltInFunction(BIF_DS_Number_ToBase_Identifier_$, {New DSFunction.Parameter("_InputNumber", GetType(DSNumber)), New DSFunction.Parameter("_TargetBase", GetType(DSNumber))}, GetType(DSString), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_DS_Number_ToBase_Identifier_$)

			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSString(_Arguments.ElementAt(0).Coerce(Of DSNumber)().AsBasedNumber.ToBase(Convert.ToByte(_Arguments.ElementAt(1).Coerce(Of DSNumber)().Value)).Value)

			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Returns the _InputNumber in the _TargetBase, as a String"}

		Private Const BIF_DS_Number_FromExprString_Identifier_$ = DSBIFs_IdentifierStart_ & "Number_FromExprString"
		Private Shared ReadOnly BIF_DS_Number_FromExprString_ As New BuiltInFunction(BIF_DS_Number_FromExprString_Identifier_$, {New DSFunction.Parameter("_ExpressionString", GetType(DSNumber))}, GetType(DSNumber), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_DS_Number_FromExprString_Identifier_$)
			 Dim _Modified_SymTbls As Runtime.SymbolTablesSnapshot = _SymTbls

			 Dim _ExprString_ExeRes As ExecutionResult = Language.Expressions.ConstructExpressionFromTokens(Parser.GetTokensFromSource(_Arguments.First().Coerce(Of DSString)().Value)).Resolve(_Modified_SymTbls)
			 _ExeRes.UpstairsExecutionResults.Add(New Tuple(Of String, ExecutionResult)("Numeric Expression String", _ExprString_ExeRes))
			 _Modified_SymTbls = _ExprString_ExeRes.ResultantSymbolTablesState

			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = _ExprString_ExeRes.ReturnStatus.IExpression_ResolutionResult.Coerce(Of DSNumber)()
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Interprets the _ExpressionString as a DocScript Expression, Resolving it to <Number>"}

		Private Const BIF_DS_Number_GetRandom_Identifier_$ = DSBIFs_IdentifierStart_ & "Number_GetRandom"
		Private Shared ReadOnly BIF_DS_Number_GetRandom_ As New BuiltInFunction(BIF_DS_Number_GetRandom_Identifier_$, {New DSFunction.Parameter("_LowerBound", GetType(DSNumber)), New DSFunction.Parameter("_UpperBound", GetType(DSNumber))}, GetType(DSNumber), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_DS_Number_GetRandom_Identifier_$)
			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSNumber((New System.Random(DateTime.Now.Millisecond)).Next(minValue:=CInt(_Arguments.ElementAt(0).Coerce(Of DSNumber)().Value) - 1, maxValue:=CInt(_Arguments.ElementAt(1).Coerce(Of DSNumber)().Value)) + 1)
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Returns a random integral number, guaranteed to be no lower than the _LowerBound, and no higher than the _UpperBound"}

#End Region

#Region "DS_Boolean BIFs"

		Private Const BIF_DS_Boolean_Repeat_Identifier_$ = DSBIFs_IdentifierStart_ & "Boolean_Repeat"
		Private Shared ReadOnly BIF_DS_Boolean_Repeat_ As New BuiltInFunction(BIF_DS_Boolean_Repeat_Identifier_$, {New DSFunction.Parameter("_BooleanDatum", GetType(DSBoolean)), New DSFunction.Parameter("_Times", GetType(DSNumber))}, GetType(DSArray(Of DSBoolean)), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_DS_Boolean_Repeat_Identifier_$)

			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue =
			 New DSArray(Of DSBoolean)(Enumerable.Repeat(Of DSBoolean)(element:=_Arguments.ElementAt(0).Coerce(Of DSBoolean)(), count:=CInt(_Arguments.ElementAt(1).Coerce(Of DSNumber)().Value)).ToArray())

			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Returns the _BooleanDatum, repeated _Times times"}

#End Region

#Region "DS_BooleanArray BIFs"

		Private Const BIF_DS_BooleanArray_Length_Identifier_$ = DSBIFs_IdentifierStart_ & "BooleanArray_Length"
		Private Shared ReadOnly BIF_DS_BooleanArray_Length_ As New BuiltInFunction(BIF_DS_BooleanArray_Length_Identifier_$, {New DSFunction.Parameter("_BooleanArray", GetType(DSArray(Of DSBoolean)))}, GetType(DSNumber), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_DS_BooleanArray_Length_Identifier_$)
			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSNumber(_Arguments.ElementAt(0).Coerce(Of DSArray(Of DSBoolean))().Elements.Count)
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Returns the number of elements in the _BooleanArray"}

		Private Const BIF_DS_BooleanArray_At_Identifier_$ = DSBIFs_IdentifierStart_ & "BooleanArray_At"
		Private Shared ReadOnly BIF_DS_BooleanArray_At_ As New BuiltInFunction(BIF_DS_BooleanArray_At_Identifier_$, {New DSFunction.Parameter("_Array", GetType(DSArray(Of DSBoolean))), New DSFunction.Parameter("_Index", GetType(DSNumber))}, GetType(DSBoolean), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Try

				 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_DS_BooleanArray_At_Identifier_$)

				 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = _
				_Arguments.ElementAt(0).Coerce(Of DSArray(Of DSBoolean))() _
				.Elements.ElementAt(
				 Convert.ToInt32(_Arguments.ElementAt(1).Coerce(Of DSNumber)().Value)
				 )

				 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

			 Catch _Ex As Exception
				 Throw New DSException(_Ex.Message & " The _Array Length was " & _Arguments.ElementAt(0).Coerce(Of DSString)().Value.Length.ToString().InSquares() & ", and the _Index was " & Convert.ToInt32(_Arguments.ElementAt(1).Coerce(Of DSNumber)().Value).ToString().InSquares(), _Ex)
			 End Try

		 End Function)
		) With {.[Description] = "Returns the element at _Index in the _Array"}

#Region "LINQ-Style Methods"

		Private Const BIF_DS_BooleanArray_ForEach_Identifier_$ = DSBIFs_IdentifierStart_ & "BooleanArray_ForEach"
		Private Shared ReadOnly BIF_DS_BooleanArray_ForEach_ As New BuiltInFunction(BIF_DS_BooleanArray_ForEach_Identifier_$, {New DSFunction.Parameter("_Array", GetType(DSArray(Of DSBoolean))), New DSFunction.Parameter("_Action", GetType(DSString))}, GetType(Void), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_DS_BooleanArray_ForEach_Identifier_$)
			 Dim _Modified_SymTbls As Runtime.SymbolTablesSnapshot = _SymTbls.SnapshotFrom_ThisAndTopmost(_NewTopmost:=New Runtime.SymbolTable("LambdaForEach_PreIterations"))

			 REM Parse and Lex the _Action
			 REM For Each element in _Array:
			 REM	Define a new SymbolTable for _Item and _Index
			 REM	Execute the Action-Instructions with the new SymTblsSnpSht
			 REM	Add as an upstairs ExeRes
			 REM	Reapply the SymTbls
			 REM Return WITHOUT the SymTbls which held _Item and _Index

			 Dim _Array As DSArray(Of DSBoolean) = _Arguments.ElementAt(0).Coerce(Of DSArray(Of DSBoolean))()

			 Dim _Tokens As Runtime.Token() = Parser.GetTokensFromSource(Parser.UnflattenSource(_Arguments.ElementAt(1).Coerce(Of DSString)().Value))
			 Dim _Instructions As Language.Instructions.IInstruction() = Language.Instructions.Statements.GetStatementContentsFromTokens(_Tokens)

			 For _Element_Index% = 0 To (_Array.Elements.Count - 1) Step +1

				 'Set the Topmost SymTbl to have the _Item and _Index
				 _Modified_SymTbls.Topmost = New Runtime.SymbolTable("LambdaForEach_Iteration" & _Element_Index.ToString())
				 _Modified_SymTbls.AddEntryToToTopmost("_Item", Runtime.SymbolTable.GetSymbolTableEntry_FromIDataValue(_InputIDV:=_Array.Elements.ElementAt(_Element_Index).MustNotBeNothing("The Element at index " & _Element_Index.ToString() & " in _Array, was Nothing"), _NullValueType_IfInputIDVIsNothing:=Nothing))
				 _Modified_SymTbls.AddEntryToToTopmost("_Index", Runtime.SymbolTable.GetSymbolTableEntry_FromIDataValue(_InputIDV:=New DSNumber(_Element_Index%), _NullValueType_IfInputIDVIsNothing:=Nothing))

				 'Just in case multiple Instructions were specified...
				 For Each _Instruction As Language.Instructions.IInstruction In _Instructions
					 Dim _Instruction_ExeRes As Language.Instructions.ExecutionResult = _Instruction.Execute(_Modified_SymTbls)
					 _ExeRes.UpstairsExecutionResults.Add(New Tuple(Of String, ExecutionResult)("Iteration" & _Element_Index.ToString() & " ActionInstruction " & _Instruction.GetType().Name.InBrackets(), _Instruction_ExeRes))
					 _Modified_SymTbls = _Instruction_ExeRes.ResultantSymbolTablesState
				 Next

			 Next

			 _ExeRes.Archived_SymbolTable = _Modified_SymTbls.Topmost
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_Modified_SymTbls.BottomStack)	'Don't return the SymTbl which was for _Item and _Index

		 End Function)
		) With {.[Description] = "Interprets the _Action for each Element of the _Array. A new SymbolTable is created for _Item and _Index. Use ; for CrLf and ` for "". (i.e. DS Flattened Source is supported herein)"}

		Private Const BIF_DS_BooleanArray_ReplaceAt_Identifier_$ = DSBIFs_IdentifierStart_ & "BooleanArray_ReplaceAt"
		Private Shared ReadOnly BIF_DS_BooleanArray_ReplaceAt_ As New BuiltInFunction(BIF_DS_BooleanArray_ReplaceAt_Identifier_$, {New DSFunction.Parameter("_InputArray", GetType(DSArray(Of DSBoolean))), New DSFunction.Parameter("_Index", GetType(DSNumber)), New DSFunction.Parameter("_NewItem", GetType(DSBoolean))}, GetType(DSArray(Of DSBoolean)), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_DS_BooleanArray_ReplaceAt_Identifier_$)

			 Dim _InputArray As DSArray(Of DSBoolean) = _Arguments.ElementAt(0).Coerce(Of DSArray(Of DSBoolean))()
			 _InputArray.Elements.Item(index:=CInt(_Arguments.ElementAt(1).Coerce(Of DSNumber)().Value)) = _Arguments.ElementAt(2).Coerce(Of DSBoolean)()
			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = _InputArray

			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Returns a Boolean@, based off of the _InputArray, but where the Item at _Index is replaced with the _NewItem"}

		Private Const BIF_DS_BooleanArray_SelectStrings_Identifier_$ = DSBIFs_IdentifierStart_ & "BooleanArray_SelectStrings"
		Private Shared ReadOnly BIF_DS_BooleanArray_SelectStrings_ As New BuiltInFunction(BIF_DS_BooleanArray_SelectStrings_Identifier_$, {New DSFunction.Parameter("_Array", GetType(DSArray(Of DSBoolean))), New DSFunction.Parameter("_SelectionExpr", GetType(DSString))}, GetType(DSArray(Of DSString)), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_DS_BooleanArray_SelectStrings_Identifier_$)
			 Dim _Modified_SymTbls As Runtime.SymbolTablesSnapshot = _SymTbls.SnapshotFrom_ThisAndTopmost(_NewTopmost:=New Runtime.SymbolTable("LambdaSelectString_PreIterations"))

			 REM Parse and Lex the _SelectionExpr
			 REM For Each element in _Array:
			 REM	Define a new SymbolTable for _Item and _Index
			 REM	Execute the Expression with the new SymTblsSnpSht
			 REM		Add {the IDV produced from this resolution} to _SelectedDSStrings
			 REM	Add as an upstairs ExeRes
			 REM	Reapply the SymTbls
			 REM Return WITHOUT the SymTbls which held _Item and _Index

			 Dim _Array As DSArray(Of DSBoolean) = _Arguments.ElementAt(0).Coerce(Of DSArray(Of DSBoolean))()
			 Dim _SelectedDSStrings As New List(Of DSString)()

			 Dim _Tokens As Runtime.Token() = Parser.GetTokensFromSource(Parser.UnflattenSource(_Arguments.ElementAt(1).Coerce(Of DSString)().Value))
			 Dim _SelectionExpr As Language.Expressions.IExpression = Language.Expressions.ExprUtilities.ConstructExpressionFromTokens(_Tokens)

			 For _Element_Index% = 0 To (_Array.Elements.Count - 1) Step +1

				 'Set the Topmost SymTbl to have the _Item and _Index
				 _Modified_SymTbls.Topmost = New Runtime.SymbolTable("LambdaSelectString_Iteration" & _Element_Index.ToString())
				 _Modified_SymTbls.AddEntryToToTopmost("_Item", Runtime.SymbolTable.GetSymbolTableEntry_FromIDataValue(_InputIDV:=_Array.Elements.ElementAt(_Element_Index).MustNotBeNothing("The Element at index " & _Element_Index.ToString() & " in _Array, was Nothing"), _NullValueType_IfInputIDVIsNothing:=Nothing))
				 _Modified_SymTbls.AddEntryToToTopmost("_Index", Runtime.SymbolTable.GetSymbolTableEntry_FromIDataValue(_InputIDV:=New DSNumber(_Element_Index), _NullValueType_IfInputIDVIsNothing:=Nothing))

				 'Push the evaluation of the _SelectionExpr to the _SelectedDSStrings
				 Dim _Expression_ExeRes As Language.Instructions.ExecutionResult = _SelectionExpr.Resolve(_Modified_SymTbls)
				 _ExeRes.UpstairsExecutionResults.Add(New Tuple(Of String, ExecutionResult)("Iteration" & _Element_Index.ToString() & " SelectionExpression " & _SelectionExpr.GetType().Name.InBrackets(), _Expression_ExeRes))
				 _Modified_SymTbls = _Expression_ExeRes.ResultantSymbolTablesState
				 _SelectedDSStrings.Add(_Expression_ExeRes.ReturnStatus.IExpression_ResolutionResult.Coerce(Of DSString)())

			 Next

			 _ExeRes.Archived_SymbolTable = _Modified_SymTbls.Topmost
			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSArray(Of DSString)(_SelectedDSStrings.ToArray())
			 Return (_ExeRes.StopExecutionTimer_AndFinaliseObject(_Modified_SymTbls.BottomStack)) 'Don't return the SymTbl which was for _Item and _Index

		 End Function)
		) With {.[Description] = "Interprets the _SelectionExpr for each Element of the _Array, returning a String@ for the result of each Resolution of the _SelectionExpr. A new SymbolTable is created for _Item and _Index. Use ; for CrLf and ` for ""."}

#End Region

#End Region

#Region "DS_StringArray BIFs"

		Private Const BIF_DS_StringArray_Append_Identifier_$ = DSBIFs_IdentifierStart_ & "StringArray_Append"
		Private Shared ReadOnly BIF_DS_StringArray_Append_ As New BuiltInFunction(BIF_DS_StringArray_Append_Identifier_$, {New DSFunction.Parameter("_ExistingStringArray", GetType(DSArray(Of DSString))), New DSFunction.Parameter("_NewItem", GetType(DSString))}, GetType(DSArray(Of DSString)), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_DS_StringArray_Append_Identifier_$)

			 Dim _ArrayToReturn As New DSArray(Of DSString)(
			  _Arguments.ElementAt(0).Coerce(Of DSArray(Of DSString))().Elements _
			  .Concat({_Arguments.ElementAt(1).Coerce(Of DSString)()}) _
			  .ToArray()
			 )

			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = _ArrayToReturn

			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Returns a new <String@> with the items of the _ExistingStringArray, but with the _NewItem appended"}

		Private Const BIF_DS_StringArray_At_Identifier_$ = DSBIFs_IdentifierStart_ & "StringArray_At"
		Private Shared ReadOnly BIF_DS_StringArray_At_ As New BuiltInFunction(BIF_DS_StringArray_At_Identifier_$, {New DSFunction.Parameter("_Array", GetType(DSArray(Of DSString))), New DSFunction.Parameter("_Index", GetType(DSNumber))}, GetType(DSString), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Try

				 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_DS_StringArray_At_Identifier_$)

				 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = _
				_Arguments.ElementAt(0).Coerce(Of DSArray(Of DSString))() _
				.Elements.ElementAt(
				 Convert.ToInt32(_Arguments.ElementAt(1).Coerce(Of DSNumber)().Value)
				 )

				 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

			 Catch _Ex As Exception
				 Throw New DSException(_Ex.Message & " The _Array Length was " & _Arguments.ElementAt(0).Coerce(Of DSString)().Value.Length.ToString().InSquares() & ", and the _Index was " & Convert.ToInt32(_Arguments.ElementAt(1).Coerce(Of DSNumber)().Value).ToString().InSquares(), _Ex)
			 End Try

		 End Function)
		) With {.[Description] = "Returns the element at _Index in the _Array"}

		Private Const BIF_DS_StringArray_Length_Identifier_$ = DSBIFs_IdentifierStart_ & "StringArray_Length"
		Private Shared ReadOnly BIF_DS_StringArray_Length_ As New BuiltInFunction(BIF_DS_StringArray_Length_Identifier_$, {New DSFunction.Parameter("_Array", GetType(DSArray(Of DSString)))}, GetType(DSNumber), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_DS_StringArray_Length_Identifier_$)
			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSNumber(_Arguments.ElementAt(0).Coerce(Of DSArray(Of DSString))().Elements.Count)
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Returns the number of elements in the _Array"}

		Private Const BIF_DS_StringArray_EachToNumber_Identifier_$ = DSBIFs_IdentifierStart_ & "StringArray_EachToNumber"
		Private Shared ReadOnly BIF_DS_StringArray_EachToNumber_ As New BuiltInFunction(BIF_DS_StringArray_EachToNumber_Identifier_$, {New DSFunction.Parameter("_Array", GetType(DSArray(Of DSString)))}, GetType(DSArray(Of DSNumber)), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult


			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_DS_StringArray_EachToNumber_Identifier_$)
			 Dim _Modified_SymTbls As Runtime.SymbolTablesSnapshot = _SymTbls

			 Dim _DSNumbers_FromStringArray As New List(Of DSNumber)()
			 For Each _DSString As DSString In _Arguments.ElementAt(0).Coerce(Of DSArray(Of DSString))().Elements

				 Dim _DSString_ToDSNumber_ExeRes As ExecutionResult = Language.Expressions.ConstructExpressionFromTokens(Parser.GetTokensFromSource(_DSString.Value)).Resolve(_Modified_SymTbls)
				 _ExeRes.UpstairsExecutionResults.Add(New Tuple(Of String, ExecutionResult)("Numeric String Expression", _DSString_ToDSNumber_ExeRes))
				 _Modified_SymTbls = _DSString_ToDSNumber_ExeRes.ResultantSymbolTablesState
				 _DSNumbers_FromStringArray.Add(_DSString_ToDSNumber_ExeRes.ReturnStatus.IExpression_ResolutionResult.Coerce(Of DSNumber)())

			 Next

			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSArray(Of DSNumber)(_DSNumbers_FromStringArray.ToArray())
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_Modified_SymTbls)

		 End Function)
		) With {.[Description] = "Returns a Number@ from each element in the String@"}

		Private Const BIF_DS_StringArray_Serialise_Identifier_$ = DSBIFs_IdentifierStart_ & "StringArray_Serialise"
		Private Shared ReadOnly BIF_DS_StringArray_Serialise_ As New BuiltInFunction(BIF_DS_StringArray_Serialise_Identifier_$, {New DSFunction.Parameter("_Array", GetType(DSArray(Of DSString))), New DSFunction.Parameter("_SeperatorString", GetType(DSString))}, GetType(DSString), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_DS_StringArray_Serialise_Identifier_$)

			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSString(
			  String.Join(_Arguments.ElementAt(1).Coerce(Of DSString)().Value, _Arguments.ElementAt(0).Coerce(Of DSArray(Of DSString))().Elements.Select(Of String)(Function(_DSString As DSString) _DSString.Value).ToArray())
			)

			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Returns a String with each element in the _Array being seperated by the _SeperatorString"}

#Region "LINQ-Style Methods"

		Private Const BIF_DS_StringArray_ForEach_Identifier_$ = DSBIFs_IdentifierStart_ & "StringArray_ForEach"
		Private Shared ReadOnly BIF_DS_StringArray_ForEach_ As New BuiltInFunction(BIF_DS_StringArray_ForEach_Identifier_$, {New DSFunction.Parameter("_Array", GetType(DSArray(Of DSString))), New DSFunction.Parameter("_Action", GetType(DSString))}, GetType(Void), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_DS_StringArray_ForEach_Identifier_$)
			 Dim _Modified_SymTbls As Runtime.SymbolTablesSnapshot = _SymTbls.SnapshotFrom_ThisAndTopmost(_NewTopmost:=New Runtime.SymbolTable("LambdaForEach_PreIterations"))

			 REM Parse and Lex the _Action
			 REM For Each element in _Array:
			 REM	Define a new SymbolTable for _Item and _Index
			 REM	Execute the Action-Instructions with the new SymTblsSnpSht
			 REM	Add as an upstairs ExeRes
			 REM	Reapply the SymTbls
			 REM Return WITHOUT the SymTbls which held _Item and _Index

			 Dim _Array As DSArray(Of DSString) = _Arguments.ElementAt(0).Coerce(Of DSArray(Of DSString))()

			 Dim _Tokens As Runtime.Token() = Parser.GetTokensFromSource(Parser.UnflattenSource(_Arguments.ElementAt(1).Coerce(Of DSString)().Value))
			 Dim _Instructions As Language.Instructions.IInstruction() = Language.Instructions.Statements.GetStatementContentsFromTokens(_Tokens)

			 For _Element_Index% = 0 To (_Array.Elements.Count - 1) Step +1

				 'Set the Topmost SymTbl to have the _Item and _Index
				 _Modified_SymTbls.Topmost = New Runtime.SymbolTable("LambdaForEach_Iteration" & _Element_Index.ToString())
				 _Modified_SymTbls.AddEntryToToTopmost("_Item", Runtime.SymbolTable.GetSymbolTableEntry_FromIDataValue(_InputIDV:=_Array.Elements.ElementAt(_Element_Index).MustNotBeNothing("The Element at index " & _Element_Index.ToString() & " in _Array, was Nothing"), _NullValueType_IfInputIDVIsNothing:=Nothing))
				 _Modified_SymTbls.AddEntryToToTopmost("_Index", Runtime.SymbolTable.GetSymbolTableEntry_FromIDataValue(_InputIDV:=New DSNumber(_Element_Index%), _NullValueType_IfInputIDVIsNothing:=Nothing))

				 'Just in case multiple Instructions were specified...
				 For Each _Instruction As Language.Instructions.IInstruction In _Instructions
					 Dim _Instruction_ExeRes As Language.Instructions.ExecutionResult = _Instruction.Execute(_Modified_SymTbls)
					 _ExeRes.UpstairsExecutionResults.Add(New Tuple(Of String, ExecutionResult)("Iteration" & _Element_Index.ToString() & " ActionInstruction " & _Instruction.GetType().Name.InBrackets(), _Instruction_ExeRes))
					 _Modified_SymTbls = _Instruction_ExeRes.ResultantSymbolTablesState
				 Next

			 Next

			 _ExeRes.Archived_SymbolTable = _Modified_SymTbls.Topmost
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_Modified_SymTbls.BottomStack)	'Don't return the SymTbl which was for _Item and _Index

		 End Function)
		) With {.[Description] = "Interprets the _Action for each Element of the _Array. A new SymbolTable is created for _Item and _Index. Use ; for CrLf and ` for ""."}

		Private Const BIF_DS_StringArray_Where_Identifier_$ = DSBIFs_IdentifierStart_ & "StringArray_Where"
		Private Shared ReadOnly BIF_DS_StringArray_Where_ As New BuiltInFunction(BIF_DS_StringArray_Where_Identifier_$, {New DSFunction.Parameter("_Array", GetType(DSArray(Of DSString))), New DSFunction.Parameter("_FilterExpr", GetType(DSString))}, GetType(DSArray(Of DSString)), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_DS_StringArray_Where_Identifier_$)
			 Dim _Modified_SymTbls As Runtime.SymbolTablesSnapshot = _SymTbls.SnapshotFrom_ThisAndTopmost(_NewTopmost:=New Runtime.SymbolTable("LambdaWhere_PreIterations"))

			 REM Parse and Lex the _Action
			 REM For Each element in _Array:
			 REM	Define a new SymbolTable for _Item and _Index
			 REM	Execute the Expression with the new SymTblsSnpSht
			 REM		If it resolves to True, add the Element to 
			 REM	Add as an upstairs ExeRes
			 REM	Reapply the SymTbls
			 REM Return WITHOUT the SymTbls which held _Item and _Index

			 Dim _Array As DSArray(Of DSString) = _Arguments.ElementAt(0).Coerce(Of DSArray(Of DSString))()
			 Dim _MatchingItems As New List(Of DSString)()

			 Dim _Tokens As Runtime.Token() = Parser.GetTokensFromSource(Parser.UnflattenSource(_Arguments.ElementAt(1).Coerce(Of DSString)().Value))
			 Dim _Expression As Language.Expressions.IExpression = Language.Expressions.ExprUtilities.ConstructExpressionFromTokens(_Tokens)

			 For _Element_Index% = 0 To (_Array.Elements.Count - 1) Step +1

				 'Set the Topmost SymTbl to have the _Item and _Index
				 _Modified_SymTbls.Topmost = New Runtime.SymbolTable("LambdaWhere_Iteration" & _Element_Index.ToString())
				 _Modified_SymTbls.AddEntryToToTopmost("_Item", Runtime.SymbolTable.GetSymbolTableEntry_FromIDataValue(_InputIDV:=_Array.Elements.ElementAt(_Element_Index).MustNotBeNothing("The Element at index " & _Element_Index.ToString() & " in _Array, was Nothing"), _NullValueType_IfInputIDVIsNothing:=Nothing))
				 _Modified_SymTbls.AddEntryToToTopmost("_Index", Runtime.SymbolTable.GetSymbolTableEntry_FromIDataValue(_InputIDV:=New DSNumber(_Element_Index%), _NullValueType_IfInputIDVIsNothing:=Nothing))

				 'Push the current Item, if the _Expression resolves to TRUE
				 Dim _Expression_ExeRes As Language.Instructions.ExecutionResult = _Expression.Resolve(_Modified_SymTbls)
				 _ExeRes.UpstairsExecutionResults.Add(New Tuple(Of String, ExecutionResult)("Iteration" & _Element_Index.ToString() & " FilterExpression " & _Expression.GetType().Name.InBrackets(), _Expression_ExeRes))
				 _Modified_SymTbls = _Expression_ExeRes.ResultantSymbolTablesState
				 If _Expression_ExeRes.ReturnStatus.IExpression_ResolutionResult.Coerce(Of DSBoolean)().Value Then _MatchingItems.Add(_Array.Elements.ElementAt(_Element_Index))

			 Next

			 _ExeRes.Archived_SymbolTable = _Modified_SymTbls.Topmost
			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSArray(Of DSString)(_MatchingItems.ToArray())
			 Return (_ExeRes.StopExecutionTimer_AndFinaliseObject(_Modified_SymTbls.BottomStack)) 'Don't return the SymTbl which was for _Item and _Index

		 End Function)
		) With {.[Description] = "Interprets the _FilterExpr for each Element of the _Array, returning only those elements which cause the _FilterExpr to resolve to True. A new SymbolTable is created for _Item and _Index. Use ; for CrLf and ` for ""."}

#End Region

#End Region

#Region "DS_NumberArray BIFs"

		Private Const BIF_DS_NumberArray_Append_Identifier_$ = DSBIFs_IdentifierStart_ & "NumberArray_Append"
		Private Shared ReadOnly BIF_DS_NumberArray_Append_ As New BuiltInFunction(BIF_DS_NumberArray_Append_Identifier_$, {New DSFunction.Parameter("_ExistingNumberArray", GetType(DSArray(Of DSNumber))), New DSFunction.Parameter("_NewItem", GetType(DSNumber))}, GetType(DSArray(Of DSNumber)), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_DS_NumberArray_Append_Identifier_$)

			 Dim _ArrayToReturn As New DSArray(Of DSNumber)(
			  _Arguments.ElementAt(0).Coerce(Of DSArray(Of DSNumber))().Elements _
			  .Concat({_Arguments.ElementAt(1).Coerce(Of DSNumber)()}) _
			  .ToArray()
			 )

			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = _ArrayToReturn

			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Returns a new <Number@> with the items of the _ExistingNumberArray, but with the _NewItem appended"}

		Private Const BIF_DS_NumberArray_At_Identifier_$ = DSBIFs_IdentifierStart_ & "NumberArray_At"
		Private Shared ReadOnly BIF_DS_NumberArray_At_ As New BuiltInFunction(BIF_DS_NumberArray_At_Identifier_$, {New DSFunction.Parameter("_Array", GetType(DSArray(Of DSNumber))), New DSFunction.Parameter("_Index", GetType(DSNumber))}, GetType(DSNumber), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Try

				 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_DS_NumberArray_At_Identifier_$)

				 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = _
				_Arguments.ElementAt(0).Coerce(Of DSArray(Of DSNumber))() _
				.Elements.ElementAt(
				 Convert.ToInt32(_Arguments.ElementAt(1).Coerce(Of DSNumber)().Value)
				 )

				 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

			 Catch _Ex As Exception
				 Throw New DSException(_Ex.Message & " The _Array Length was " & _Arguments.ElementAt(0).Coerce(Of DSString)().Value.Length.ToString().InSquares() & ", and the _Index was " & Convert.ToInt32(_Arguments.ElementAt(1).Coerce(Of DSNumber)().Value).ToString().InSquares(), _Ex)
			 End Try

		 End Function)
		) With {.[Description] = "Returns the element at _Index in the _Array"}

		Private Const BIF_DS_NumberArray_Length_Identifier_$ = DSBIFs_IdentifierStart_ & "NumberArray_Length"
		Private Shared ReadOnly BIF_DS_NumberArray_Length_ As New BuiltInFunction(BIF_DS_NumberArray_Length_Identifier_$, {New DSFunction.Parameter("_Array", GetType(DSArray(Of DSNumber)))}, GetType(DSNumber), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_DS_NumberArray_Length_Identifier_$)
			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSNumber(_Arguments.ElementAt(0).Coerce(Of DSArray(Of DSNumber))().Elements.Count)
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Returns the number of elements in the _Array"}

		Private Const BIF_DS_NumberArray_Serialise_Identifier_$ = DSBIFs_IdentifierStart_ & "NumberArray_Serialise"
		Private Shared ReadOnly BIF_DS_NumberArray_Serialise_ As New BuiltInFunction(BIF_DS_NumberArray_Serialise_Identifier_$, {New DSFunction.Parameter("_Array", GetType(DSArray(Of DSNumber))), New DSFunction.Parameter("_SeperatorString", GetType(DSString))}, GetType(DSString), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_DS_NumberArray_Serialise_Identifier_$)

			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSString(
			   String.Join(_Arguments.ElementAt(1).Coerce(Of DSString)().Value, _Arguments.ElementAt(0).Coerce(Of DSArray(Of DSNumber))().Elements.Select(Of String)(Function(_DSNumber As DSNumber) _DSNumber.Value.ToString()).ToArray())
			   )

			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Returns a String with each element in the _Array being seperated by the _SeperatorString"}

#Region "LINQ-Style Methods"

		Private Const BIF_DS_NumberArray_Last_Identifier_$ = DSBIFs_IdentifierStart_ & "NumberArray_Last"
		Private Shared ReadOnly BIF_DS_NumberArray_Last_ As New BuiltInFunction(BIF_DS_NumberArray_Last_Identifier_$, {New DSFunction.Parameter("_Array", GetType(DSArray(Of DSNumber)))}, GetType(DSNumber), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_DS_NumberArray_Last_Identifier_$)
			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = _Arguments.ElementAt(0).Coerce(Of DSArray(Of DSNumber))().Elements.Last()
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Returns the Last element in the _Array"}

#End Region

#End Region

#End Region

		''' <summary>DocScript Language Utilities</summary>
		Public Shared ReadOnly DS As DocScript.Runtime.BuiltInFunction() = {
		 StandardBuiltInFunctions.BIF_DS_Sleep_,
		 StandardBuiltInFunctions.BIF_DS_ThrowError_,
		 StandardBuiltInFunctions.BIF_DS_Interpret_,
		 StandardBuiltInFunctions.BIF_DS_InterpretTry_,
		 StandardBuiltInFunctions.BIF_DS_InterpretProgram_,
		 StandardBuiltInFunctions.BIF_DS_String_Split_,
		 StandardBuiltInFunctions.BIF_DS_String_Replace_,
		 StandardBuiltInFunctions.BIF_DS_String_Length_,
		 StandardBuiltInFunctions.BIF_DS_String_At_,
		 StandardBuiltInFunctions.BIF_DS_String_Contains_,
		 StandardBuiltInFunctions.BIF_DS_String_If_,
		 StandardBuiltInFunctions.BIF_DS_String_GetRandom_,
		 StandardBuiltInFunctions.BIF_DS_Number_ToBase_,
		 StandardBuiltInFunctions.BIF_DS_Number_FromExprString_,
		 StandardBuiltInFunctions.BIF_DS_Number_GetRandom_,
		 StandardBuiltInFunctions.BIF_DS_Boolean_Repeat_,
		 StandardBuiltInFunctions.BIF_DS_BooleanArray_Length_,
		 StandardBuiltInFunctions.BIF_DS_BooleanArray_At_,
		 StandardBuiltInFunctions.BIF_DS_BooleanArray_ForEach_,
		 StandardBuiltInFunctions.BIF_DS_BooleanArray_ReplaceAt_,
		 StandardBuiltInFunctions.BIF_DS_BooleanArray_SelectStrings_,
		 StandardBuiltInFunctions.BIF_DS_StringArray_Append_,
		 StandardBuiltInFunctions.BIF_DS_StringArray_At_,
		 StandardBuiltInFunctions.BIF_DS_StringArray_Length_,
		 StandardBuiltInFunctions.BIF_DS_StringArray_EachToNumber_,
		 StandardBuiltInFunctions.BIF_DS_StringArray_Serialise_,
		 StandardBuiltInFunctions.BIF_DS_StringArray_ForEach_,
		 StandardBuiltInFunctions.BIF_DS_StringArray_Where_,
		 StandardBuiltInFunctions.BIF_DS_NumberArray_Append_,
		 StandardBuiltInFunctions.BIF_DS_NumberArray_At_,
		 StandardBuiltInFunctions.BIF_DS_NumberArray_Length_,
		 StandardBuiltInFunctions.BIF_DS_NumberArray_Serialise_,
		 StandardBuiltInFunctions.BIF_DS_NumberArray_Last_
		}

	End Class

End Namespace