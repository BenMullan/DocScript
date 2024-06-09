Namespace Language.Instructions

	''' <summary>The Base Interface for all DocScript Instructions (E.g. Implemented by VariableDeclaration and ReturnToCaller)</summary>
	Public Interface IInstruction
		'ReadOnly Property Tokens As Runtime.Token()
		Function Execute(ByVal _InputSymTblsState As DocScript.Runtime.SymbolTablesSnapshot) As ExecutionResult
		Function GetProgramTreeNodeXML() As XElement
	End Interface

	''' <summary>Represents the result of a call to Execute() of an IInstruction, or Run() on a DSFunction. Contains data such as weather or not to Return.</summary>
	Public Class ExecutionResult

		''' <summary>Represents data about weather or not to Return to the Caller of a DSFunction, and weather or not there is an associated value for the Return for a DSFunction or a BuiltInFunction.</summary>
		Public Class ReturnStatusData

			Public CurrentDSFunction_ReturnHasOccurred As Boolean = False			'Set by a ReturnToCaller in .Execute()
			Public CurrentDSFunction_ReturnValue As Variables.IDataValue = Nothing	'Set by a ReturnToCaller; passed up and out of the DSFunction
			Public BuiltInFunction_ReturnValue As Variables.IDataValue = Nothing	'Set by a BIF's [Action] Delegate
			Public IExpression_ResolutionResult As Variables.IDataValue = Nothing	'Set by an IExpression in .Resolve()
			Public Program_ExitCode As Int32? = Nothing								'Set by a Program in Run()

			''' <summary>
			''' Returns a (Non-Standard) KVP-Serialisation of the Object
			''' </summary>
			Public Overrides Function ToString() As String

				Dim _Serialisation$ = ""

				REM Only return members which aren't Null or False
				If Me.CurrentDSFunction_ReturnHasOccurred Then _Serialisation &= "CurrentDSFunction_ReturnHasOccurred = [True]; "
				If Me.CurrentDSFunction_ReturnValue IsNot Nothing Then _Serialisation &= "CurrentDSFunction_ReturnValue = [" & Me.CurrentDSFunction_ReturnValue.ToString() & "]; "
				If Me.BuiltInFunction_ReturnValue IsNot Nothing Then _Serialisation &= "BuiltInFunction_ReturnValue = [" & Me.BuiltInFunction_ReturnValue.ToString() & "]; "
				If Me.IExpression_ResolutionResult IsNot Nothing Then _Serialisation &= "IExpression_ResolutionResult = [" & Me.IExpression_ResolutionResult.ToString() & "]; "
				If Me.Program_ExitCode IsNot Nothing Then _Serialisation &= "Program_ExitCode = [" & Me.Program_ExitCode.ToString() & "];"

				Return If(_Serialisation = "", "(N/A)", _Serialisation)

			End Function

		End Class

#Region "ExecutionResult \ Propities"

		'Not ReadOnly, because its members are to be set with an [Inline With] Clause
		Public ReturnStatus As New ReturnStatusData()

		Protected ResultantSymbolTablesState_ As DocScript.Runtime.SymbolTablesSnapshot = Nothing 'Initialised in StopExecutionTimer_AndFinaliseObject()
		Public ReadOnly Property ResultantSymbolTablesState As DocScript.Runtime.SymbolTablesSnapshot
			Get
				Return Me.ResultantSymbolTablesState_
			End Get
		End Property

		Protected ExecutionTime_Stopwatch_ As New System.Diagnostics.Stopwatch
		Protected ExecutionTimeMS_ As UInt64 = Nothing 'We can actually write to this one from StopExecutionTimer_AndFinaliseObject()
		Public ReadOnly Property ExecutionTimeMS As UInt64
			Get
				Return Me.ExecutionTimeMS_
			End Get
		End Property

		''' <summary>E.g. "IInstr\IfStatement" or "BIF\Input". A Description of the Object being Executed.</summary>
		Public ReadOnly ExecuteeDescription$

		''' <summary>
		''' Item1 = ExeRes Description relative to current Execution (E.g. "Operand" or "Child Instruction");
		''' Item2 = The actual ExeRes which occured upstairs of the current Execution StackFrame.
		''' Holds ExecutionResults which occured upstairs of the current Execution - E.g. the Contents Instructions of a Statement, or the OperandResolutions of a OperatorExpr.
		''' </summary>
		Public UpstairsExecutionResults As New List(Of Tuple(Of String, ExecutionResult))()

		''' <summary>Used by IStatements to store an archived copy of their ScopedVars</summary>
		Public Archived_SymbolTable As Runtime.SymbolTable = Nothing

#End Region

		Protected Sub New(ByVal _ExecuteeDescription$) 'The Class cannot be Instanciated from outside by means of the Constructor.
			Me.ExecuteeDescription = _ExecuteeDescription
			Me.ExecutionTime_Stopwatch_.Start()
			LogExecutionMessage("Started Executing or Resolving " & Me.ExecuteeDescription.InSquares() & "...")
		End Sub

		''' <summary>
		''' How to use ExecutionResult:
		''' ---------------------------
		''' Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\Output")
		''' (Do IInstruction processing...)
		''' Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_ModifiedSymTbls)
		''' </summary>
		Public Shared Function New_AndStartExecutionTimer(ByVal _ExecuteeDescription$) As ExecutionResult
			Return (New ExecutionResult(_ExecuteeDescription)) 'Starts the Stopwatch for that Instance
		End Function

		''' <summary>The .ReturnStatus is initialised to New ReturnStatusData(); use an Inline With to set its members</summary>
		''' <param name="_ResultantSymbolTablesState">For IStatements, this should NOT contain the ScopedVariables member</param>
		Public Function StopExecutionTimer_AndFinaliseObject(ByRef _ResultantSymbolTablesState As Runtime.SymbolTablesSnapshot) As ExecutionResult
			Me.ExecutionTime_Stopwatch_.Stop()
			Me.ExecutionTimeMS_ = Convert.ToUInt64(Me.ExecutionTime_Stopwatch_.ElapsedMilliseconds)
			Me.ResultantSymbolTablesState_ = _ResultantSymbolTablesState.MustNotBeNothing("The _ResultantSymbolTablesState cannot be [Nothing]")
			LogExecutionMessage("...Finished Executing " & Me.ExecuteeDescription.InSquares() & " in " & Me.ExecutionTimeMS.ToString() & "ms, with " & Me.UpstairsExecutionResults.Count.ToString() & " Upstairs ExecutionResult(s) and " & Me.ResultantSymbolTablesState.InnerSymbolTables_Count.ToString() & " Symbol Table(s) being returned downstairs.")
			Return Me
		End Function

	End Class

End Namespace