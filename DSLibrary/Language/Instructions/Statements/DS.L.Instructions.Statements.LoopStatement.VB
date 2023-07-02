Namespace Language.Instructions.Statements

	''' <summary>Represents E.g. [Loop (10) ... EndLoop]</summary>
	Public Class LoopStatement : Implements Language.Instructions.Statements.IStatement

#Region "Declarations specific to this IInstruction"

		Public CountExpr As Language.Expressions.IExpression

#End Region

#Region "Declarations declared by all IStatements"

		Protected ReadOnly Tokens_ As Runtime.Token()
		Public Property ScopedVariables As Runtime.SymbolTable Implements IStatement.ScopedVariables

		Protected Contents_ As List(Of IInstruction)
		Public ReadOnly Property Contents As System.Collections.ObjectModel.ReadOnlyCollection(Of IInstruction) Implements IStatement.Contents
			Get
				Return New ObjectModel.ReadOnlyCollection(Of IInstruction)(Me.Contents_)
			End Get
		End Property

#End Region

		''' <summary>Constructs the IInstruction from an existing instance</summary>
		Public Sub New(ByVal _CountExpr As Expressions.IExpression, ByVal _Contents As List(Of IInstruction))
			Me.CountExpr = _CountExpr : Me.Contents_ = _Contents
		End Sub

		''' <summary>Constructs the IInstruction from its Tokens</summary>
		Sub New(ByRef _Tokens As Runtime.Token())
			Me.Tokens_ = _Tokens : Me.ProcessTokensToInitialiseFields()
		End Sub

		Public Sub ProcessTokensToInitialiseFields()
			Try : LogLexingMessage("Began constructing a LoopStatement...")

				REM Source should look like:
				REM		Loop (Expr...)
				REM			...
				REM		EndLoop

				REM Tokens should look like:
				REM		[Keyword], [GrammarChar], [ExprTokens...], [GrammarChar], [LineEnd], [InstructionTokens...], [Keyword], [LineEnd]

				REM Fields to Initialise:
				REM		CountExpr
				REM		Contents_

				REM Ensure that there are enough tokens to construct the IInstruction
				If Me.Tokens_.Count < Runtime.MinimumRequiredTokens(Me.GetType()) Then Throw New DSValidationException(String.Format("The No. Tokens ({0}) was less than the minimum No. Tokens required to construct the Instruction ({1}).", Me.Tokens_.Count.ToString(), Runtime.MinimumRequiredTokens(Me.GetType()).ToString()), "Tokens: " & Me.Tokens_.GetValuesAndLocationsString())

				REM Ensure that the last Token is a {LineEnd}
				If Not Runtime.BuiltInTPVs.LineEnd_TPV.IsSatisfiedBy(Me.Tokens_.Last()) Then Throw New DSUnexpectedTokenException("The Last Token was not a {LineEnd}", Me.Tokens_)

				REM Ensure that the penultimate Token is the StatementEnd
				Runtime.BuiltInTPVs.StatementEnd_Loop_TPV.EnsureIsSatisfiedBy({Me.Tokens_(Me.Tokens_.Length - 2)})

				REM Ensure that the main TPV herefor is satisfied
				Runtime.BuiltInTPVs.LoopStatement_UpToIncOpenBracket_TPV.EnsureIsSatisfiedBy(Me.Tokens_)

				REM On the first line, there should be at least 5 Tokens
				'Loop ( Expr ) {LineEnd}
				Dim _Statement_FirstLine As Runtime.Token() = Me.Tokens_.UpToAndIncluding(AddressOf Runtime.BuiltInTPVs.LineEnd_TPV.IsSatisfiedBy)
				If Not (_Statement_FirstLine.Length >= 5) Then Throw New DSValidationException("The first line of the Statement was not syntactically-valid, because it didn't contain enough Tokens", Me.Tokens_.GetValuesAndLocationsString())

				REM Ensure that the First Line ends as we expect it to
				If Not _Statement_FirstLine.TakeBlockFromEnd(_Count:=2).SatisfiesTPV(Runtime.BuiltInTPVs.Statement_FirstLineEnding_TPV) Then Throw New DSUnexpectedTokenException("The Statement's First Line should have ended with [)] and [{LineEnd}], but did not.", _Statement_FirstLine)

				REM We now know that there is at least 1 Token for the ConditionExpr
				'Skip() the Keyword and OpeningFunctionBracket, then Take all Tokens upto the [) {LineEnd}]...
				Me.CountExpr = Expressions.ConstructExpressionFromTokens(_Statement_FirstLine.Skip(2).ToArray().UpToButExcludingLast(AddressOf Runtime.BuiltInTPVs.ClosingFunctionBracket_TPV.IsSatisfiedBy))

				REM Now get the Contents of the Statement; Skip() the first line, then it's all Tokens up to the very last StatementEnd
				Me.Contents_ = [StatementUtilities].GetStatementContentsFromTokens(Me.Tokens_.Skip(_Statement_FirstLine.Length).ToArray().UpToButExcludingLast(AddressOf Runtime.BuiltInTPVs.StatementEnd_Loop_TPV.IsSatisfiedBy)).ToList()

				LogLexingMessage("...Finished constructing a LoopStatement Object with " & Me.Contents.Count.ToString() & " Child Instruction(s)")
			Catch _Ex As Exception : Throw New DSException("@LoopStatement\ProcessTokensToInitialiseFields: " & _Ex.Message, _Ex) : End Try
		End Sub

		''' <summary>Any Expressions or Child Instructions executed hereby, are returned in the ExeRes's UpstairsExecutionResults member.</summary>
		Public Function Execute(ByVal _InputSymbolTables As Runtime.SymbolTablesSnapshot) As ExecutionResult Implements IInstruction.Execute

			REM LoopStatement Execution Process
			'	- Create the SymbolTable for this IStatement → Me.ScopedVariables
			'	- Create a New SymTblsSnapshot from the _InputSymbolTables, and Me.ScopedVars
			'
			'	- For _LoopStatement_IterationCount = 0 To [Resolve() Me.CountExpr]
			'		- For Each Child IInstruction:
			'			- Pass in _ModifiedSymTbls to Execute()
			'			- Add ExeRes thereof to UpstairsExecutionResults
			'			- Apply the returned SymTbls to _ModifiedSymTbls
			'			- Check if the _ExeRes's ReturnStatus's CurrentDSFunction_ReturnHasOccured is [True]
			'				- If it is, stop executing Child Instructions, and Copy the value of CurrentDSFunction_ReturnValue to my _ExeRes.
			'	- Re-apply the Topmost SymTbl's state to Me.ScopedVars
			'	- Add Me.ScopedVars as an Archived UpstairsSymTbl to the _ExeRes

			Try : Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("LoopStatement")

				LogExecutionMessage("(LoopStatement: Received " & _InputSymbolTables.InnerSymbolTables_Count.ToString() & " Input Symbol Tables. Statement contains " & Me.Contents.Count.ToString() & " Child Instruction(s))")
				Me.ScopedVariables = New Runtime.SymbolTable("LoopStatement")
				LogExecutionMessage("(Generated Local SymbolTable for LoopStatement)")
				Dim _Modified_SymTbls As Runtime.SymbolTablesSnapshot = _InputSymbolTables.SnapshotFrom_ThisAndTopmost(_NewTopmost:=Me.ScopedVariables)	'Push this Statement's Locals onto the working SymTbls

				'Iterate this many times
				Dim _LoopStatement_IterationsToPerformCount As UInt64 = Nothing
				Dim _LoopStatement_PerformedIterationsCount As UInt64 = 0

				REM <Resolve the CountExpr>

				'Log
				LogExecutionMessage("Resolving LoopStatement CountExpr of type " & Me.CountExpr.GetType().FullName.InSquares() & "...")

				'Execute()
				Dim _CountExpr_Resolution_ExeRes As ExecutionResult = Me.CountExpr.Resolve(_Modified_SymTbls)

				'Add as an Upstairs ExeRes
				_ExeRes.UpstairsExecutionResults.Add(New Tuple(Of String, ExecutionResult)("CountExpr", _CountExpr_Resolution_ExeRes))

				'Update my SymbolTables; 
				_Modified_SymTbls = _CountExpr_Resolution_ExeRes.ResultantSymbolTablesState

				'Extract the Value for _LoopStatement_IterationsToPerformCount
				_LoopStatement_IterationsToPerformCount = Convert.ToUInt64(_CountExpr_Resolution_ExeRes.ReturnStatus.IExpression_ResolutionResult.Coerce(Of Variables.DSNumber)().Value)
				LogExecutionMessage("...Resolved LoopStatement CountExpr to " & _LoopStatement_IterationsToPerformCount.ToString().InSquares())

				REM </Resolve the CountExpr>

				REM !!!!!!!!!!!!!!!!!!!!!!!!!!!!
				REM	    <Contents Execution>
				REM !!!!!!!!!!!!!!!!!!!!!!!!!!!!

				'No Return has occured yet
				_ExeRes.[ReturnStatus].CurrentDSFunction_ReturnHasOccurred = False

				For _LoopStatement_PerformedIterationsCount = 0 To CULng(_LoopStatement_IterationsToPerformCount - 1) Step (+1UL)

					REM RESET THE SYMBOL TABLES FOR THIS ITERATION
					If Not _Modified_SymTbls.Topmost.Description = "LoopStatement" Then Throw New DSValidationException("The Statment's Symbols were about to be reset for a new iteration, however the TopMost Modified SymbolTable was not the Statement-Local SymbolTable", "TopMost SymTbl: " & _Modified_SymTbls.Topmost.Description)
					Me.ScopedVariables = _Modified_SymTbls.Topmost
					_Modified_SymTbls.Topmost = New Runtime.SymbolTable("LoopStatement")
					LogExecutionMessage("The LoopStatement's SymbolTables have been dumped to ScopedVariables, and reset for the current (just-started) Iteration")

					For Each _ChildInstruction As IInstruction In Me.Contents

						'Log
						LogExecutionMessage("Executing LoopStatement Child Instruction of Type " & _ChildInstruction.GetType().Name.InSquares())

						'Execute()
						Dim _ChildInstruction_ExeRes As ExecutionResult = _ChildInstruction.Execute(_Modified_SymTbls)

						'Add as an Upstairs ExeRes
						_ExeRes.UpstairsExecutionResults.Add(New Tuple(Of String, ExecutionResult)("ChildInsruction " & _ChildInstruction.GetType().Name.InBrackets(), _ChildInstruction_ExeRes))

						'Update my SymbolTables; 
						_Modified_SymTbls = _ChildInstruction_ExeRes.ResultantSymbolTablesState

						'Work out weather or not to Return from the DSFunction
						If (_ChildInstruction_ExeRes.ReturnStatus.CurrentDSFunction_ReturnHasOccurred <> Nothing) AndAlso (_ChildInstruction_ExeRes.ReturnStatus.CurrentDSFunction_ReturnHasOccurred) Then

							'A Return HAS just occured for the current DSFunction
							_ExeRes.ReturnStatus.CurrentDSFunction_ReturnHasOccurred = True

							'Log
							LogExecutionMessage("A Child Instruction within the LoopStatement, of Type " & _ChildInstruction.GetType().Name.InSquares() & ", raised the Return Flag; no further Child Instructions will be executed")

							'Copy the ReturnValue to this Statement's _ExeRes; pass it DOWN THE STACK
							_ExeRes.ReturnStatus.CurrentDSFunction_ReturnValue = _ChildInstruction_ExeRes.ReturnStatus.CurrentDSFunction_ReturnValue

							'Don't execute any further Contents IInstructions
							Exit For

						End If

					Next

					'If that inner ForEach was just exited due to the DSFunction Reutrn Flag being raised, then we need to Exit this For too:
					If _ExeRes.ReturnStatus.CurrentDSFunction_ReturnHasOccurred Then Exit For

				Next

				REM !!!!!!!!!!!!!!!!!!!!!!!!!!!!
				REM	    </Contents Execution>
				REM !!!!!!!!!!!!!!!!!!!!!!!!!!!!

				REM Re-apply the State to Me.ScopedVars
				Me.ScopedVariables = _Modified_SymTbls.Topmost : _ExeRes.Archived_SymbolTable = Me.ScopedVariables

				_ExeRes.StopExecutionTimer_AndFinaliseObject(_Modified_SymTbls.BottomStack)	'Don't return this IStatement's Locals
				LogLexingMessage(String.Format("(...LoopStatement performed {0} of {1} Iteration(s) and produced DSFunction ReturnValue [{2}].", _LoopStatement_PerformedIterationsCount.ToString(), _LoopStatement_IterationsToPerformCount.ToString(), If(_ExeRes.ReturnStatus.CurrentDSFunction_ReturnValue, New Variables.DSString("(No Return Value)")).ToString()), LogEvent.DSEventSeverity.Verbose)
				Return _ExeRes

			Catch _Ex As Exception : Throw New DSException("@LoopStatement\Execute: " & _Ex.Message, _Ex) : End Try

		End Function

		''' <summary>Returns an XML Representation of the Instruction, with all Propities and Child Structres included</summary>
		Public Function GetProgramTreeNodeXML() As System.Xml.Linq.XElement Implements IInstruction.GetProgramTreeNodeXML

			Dim _XElementToReturn As XElement = <LoopStatement/>

			_XElementToReturn.Add(Me.CountExpr.GetExprTreeNodeXML().WrapIn("CountExpr"))

			Dim _ContentsXElement As XElement = <Contents/>
			For Each _IInstruction As IInstruction In Me.Contents
				_ContentsXElement.Add(_IInstruction.GetProgramTreeNodeXML())
			Next
			_XElementToReturn.Add(_ContentsXElement)

			Return _XElementToReturn

		End Function

		''' <summary>Constructs an instance of the IInstruction class from its XML serialasation</summary>
		Public Shared Function FromXML(ByVal _IInstructionXElement As XElement) As LoopStatement
			Try

				Return New LoopStatement(
				 _CountExpr:=Expressions.ExprUtilities.ConstructExpressionFromXML(_IInstructionXElement.<CountExpr>.First().Elements().First()),
				 _Contents:=StatementUtilities.GetStatementContentsFromXML(_IInstructionXElement.<Contents>.First().Elements().ToArray()).ToList()
				)

			Catch _Ex As Exception : Throw New DSException("@LoopStatement\FromXML: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>Returns what the Instruction would have looked like in the Source, without an extra LineBreak on the End</summary>
		Public Overrides Function ToString() As String

			REM E.g.
			'	LOOP (COUNT_EXPR)
			'		
			'		OUTPUT("H_2 O")
			'		
			'	ENDLOOP

			Return _
			 Constants.Keyword_Loop & Constants.Space & Constants.OpeningFunctionBracket & Me.CountExpr.ToString() & Constants.ClosingFunctionBracket & Constants.LineBreak & _
			 Constants.LineBreak & _
			 ReconstructStatementContentsAsSource(Me.Contents.ToArray()).TabbedInBy(1) & Constants.LineBreak & _
			 Constants.LineBreak & _
			 Constants.StatementEnd_Loop

		End Function

	End Class

End Namespace