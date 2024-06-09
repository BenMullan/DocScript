Imports DocScript.Runtime.BuiltInFunction, DocScript.Language.Variables, DocScript.Language.Instructions, DocScript.Language.Instructions.Statements


'Can we have a mechanism for BIFs accessing the Program's ExeCxt for the RootFolder etc...
'?
'Dim _RootFolder$ = Runtime.Program.Current.ExecutionContext.RootFolder


Namespace Runtime

	REM This is the Original Declaration of the Class; it has the Private Constructor and XMLDocumentation
	''' <summary>Provides useful Logic built-in to DocScript</summary>
	Partial Public NotInheritable Class StandardBuiltInFunctions

		'Make this Class uninstanciatable from outside
		Private Sub New()
		End Sub

		Private Const MathsBIFs_IdentifierStart_$ = "Maths_"

#Region "Individual BIF Member Declarations"

		Private Const BIF_Maths_Round_Identifier_$ = MathsBIFs_IdentifierStart_ & "Round"
		Private Shared ReadOnly BIF_Maths_Round_ As New BuiltInFunction(BIF_Maths_Round_Identifier_$, {New DSFunction.Parameter("_Input", GetType(DSNumber))}, GetType(DSNumber), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_Maths_Round_Identifier_)
			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSNumber(Math.Round(_Arguments.First().Coerce(Of DSNumber)().Value))
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Rounds the _Input to the nearest Integer"}

		Private Const BIF_Maths_GetPi_Identifier_$ = MathsBIFs_IdentifierStart_ & "GetPi"
		Private Shared ReadOnly BIF_Maths_GetPi_ As New BuiltInFunction(BIF_Maths_GetPi_Identifier_$, {}, GetType(DSNumber), New BuiltInFunctionDelegate(
		   Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			   Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_Maths_GetPi_Identifier_)
			   _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSNumber(Math.PI)
			   Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		   End Function)
		) With {.[Description] = "Gets the Value of Pi: " & Math.PI.ToString()}

#Region "Common Maths Operators"

		Private Const BIF_Maths_LessThan_Identifier_$ = MathsBIFs_IdentifierStart_ & "LessThan"
		Private Shared ReadOnly BIF_Maths_LessThan_ As New BuiltInFunction(BIF_Maths_LessThan_Identifier_$, {New DSFunction.Parameter("_A", GetType(DSNumber)), New DSFunction.Parameter("_B", GetType(DSNumber))}, GetType(DSBoolean), New BuiltInFunctionDelegate(
		Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_Maths_LessThan_Identifier_)

			_ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSBoolean(
			   _Arguments.ElementAt(0).Coerce(Of DSNumber)().Value < _Arguments.ElementAt(1).Coerce(Of DSNumber)().Value
			 )

			Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		End Function)
		) With {.[Description] = "Determines weather or not _A is less than _B"}

		Private Const BIF_Maths_GreaterThan_Identifier_$ = MathsBIFs_IdentifierStart_ & "GreaterThan"
		Private Shared ReadOnly BIF_Maths_GreaterThan_ As New BuiltInFunction(BIF_Maths_GreaterThan_Identifier_$, {New DSFunction.Parameter("_A", GetType(DSNumber)), New DSFunction.Parameter("_B", GetType(DSNumber))}, GetType(DSBoolean), New BuiltInFunctionDelegate(
		Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_Maths_GreaterThan_Identifier_)

			_ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSBoolean(
			  _Arguments.ElementAt(0).Coerce(Of DSNumber)().Value > _Arguments.ElementAt(1).Coerce(Of DSNumber)().Value
			)

			Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		End Function)
		) With {.[Description] = "Determines weather or not _A is greater than _B"}

		Public Const BIF_Maths_Bitshift_Left_Identifier$ = MathsBIFs_IdentifierStart_ & "Bitshift_Left"	'Public because it's used by DS.R.Optimisation
		Private Shared ReadOnly BIF_Maths_Bitshift_Left_ As New BuiltInFunction(BIF_Maths_Bitshift_Left_Identifier$, {New DSFunction.Parameter("_Input", GetType(DSNumber)), New DSFunction.Parameter("_ShiftCount", GetType(DSNumber))}, GetType(DSNumber), New BuiltInFunctionDelegate(
		Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_Maths_Bitshift_Left_Identifier)

			_ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSNumber(
			  CULng(_Arguments.ElementAt(0).Coerce(Of DSNumber)().Value) << CUShort(_Arguments.ElementAt(1).Coerce(Of DSNumber)().Value)
			)

			Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		End Function)
		) With {.[Description] = "Returns _Input shifted left by _ShiftCount places"}

		Private Const BIF_Maths_Bitshift_Right_Identifier_$ = MathsBIFs_IdentifierStart_ & "Bitshift_Right"
		Private Shared ReadOnly BIF_Maths_Bitshift_Right_ As New BuiltInFunction(BIF_Maths_Bitshift_Right_Identifier_$, {New DSFunction.Parameter("_Input", GetType(DSNumber)), New DSFunction.Parameter("_ShiftCount", GetType(DSNumber))}, GetType(DSNumber), New BuiltInFunctionDelegate(
		Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_Maths_Bitshift_Right_Identifier_)

			_ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSNumber(
			  CULng(_Arguments.ElementAt(0).Coerce(Of DSNumber)().Value) >> CUShort(_Arguments.ElementAt(1).Coerce(Of DSNumber)().Value)
			)

			Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		End Function)
		) With {.[Description] = "Returns _Input shifted right by _ShiftCount places"}

#End Region

#End Region

		''' <summary>Maths-related Functions such as Rounding, getting Pi, or finding the maximum of two numbers</summary>
		Public Shared ReadOnly Maths As DocScript.Runtime.BuiltInFunction() = {
		 StandardBuiltInFunctions.BIF_Maths_Round_,
		 StandardBuiltInFunctions.BIF_Maths_GetPi_,
		 StandardBuiltInFunctions.BIF_Maths_LessThan_,
		 StandardBuiltInFunctions.BIF_Maths_GreaterThan_,
		 StandardBuiltInFunctions.BIF_Maths_Bitshift_Left_,
		 StandardBuiltInFunctions.BIF_Maths_Bitshift_Right_
		}

	End Class

End Namespace