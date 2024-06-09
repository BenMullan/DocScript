Namespace Language.Instructions.Statements

	''' <summary>Represents E.g. [If (True) ... EndIf]</summary>
	Public Class IfStatement : Implements Language.Instructions.Statements.IStatement

#Region "Declarations specific to this IInstruction"

		Public ConditionExpr As Language.Expressions.IExpression

		'Not Initialised unless there is an Else Clause in the Tokens
		'This means that there is a difference between:
		'	[An IfStatement without the Else Keyword even there]
		'And:
		'	[An IfStatement with an Else Clause, but with no IInstructions therein]
		Protected ElseContents_ As List(Of IInstruction) = Nothing

		''' <summary>The Instructions which are executed if the ConditionExpr resolves to False. If there isn't an Else clause, this is [Nothing] (null)</summary>
		Public ReadOnly Property ElseContents As System.Collections.ObjectModel.ReadOnlyCollection(Of IInstruction)
			Get
				If Me.ElseContents_ Is Nothing Then Return Nothing
				Return New ObjectModel.ReadOnlyCollection(Of IInstruction)(Me.ElseContents_)
			End Get
		End Property

#End Region

#Region "Declarations declared by all IStatements"

		Protected ReadOnly Tokens_ As Runtime.Token()
		Public Property ScopedVariables As Runtime.SymbolTable Implements IStatement.ScopedVariables

		Protected Contents_ As List(Of IInstruction) 'Guarenteed to be Initialised
		''' <summary>The Instructions which are executed if the ConditionExpr resolves to True</summary>
		Public ReadOnly Property Contents As System.Collections.ObjectModel.ReadOnlyCollection(Of IInstruction) Implements IStatement.Contents
			Get
				Return New ObjectModel.ReadOnlyCollection(Of IInstruction)(Me.Contents_)
			End Get
		End Property

#End Region

		''' <summary>Constructs the IInstruction from an existing instance</summary>
		Public Sub New(ByVal _ConditionExpr As Expressions.IExpression, ByVal _Contents As List(Of IInstruction), ByVal _ElseContents As List(Of IInstruction))
			Me.ConditionExpr = _ConditionExpr : Me.Contents_ = _Contents : Me.ElseContents_ = _ElseContents
		End Sub

		''' <summary>Constructs the IInstruction from its Tokens</summary>
		Public Sub New(ByRef _Tokens As Runtime.Token())
			Me.Tokens_ = _Tokens : Me.ProcessTokensToInitialiseFields()
		End Sub

		Public Sub ProcessTokensToInitialiseFields()
			Try : LogLexingMessage("Began constructing an IfStatement...")

				REM Source should look like:
				REM		If (Expr...)
				REM			...
				REM		EndIf

				REM Tokens should look like:
				REM		[Keyword], [GrammarChar], [ExprTokens...], [GrammarChar], [LineEnd], [InstructionTokens...], [Keyword], [LineEnd]

				REM Fields to Initialise:
				REM		ConditionExpr
				REM		Contents_

				REM Ensure that there are enough tokens to construct the IInstruction
				If Me.Tokens_.Count < Runtime.MinimumRequiredTokens(Me.GetType()) Then Throw New DSValidationException(String.Format("The No. Tokens ({0}) was less than the minimum No. Tokens required to construct the Instruction ({1}).", Me.Tokens_.Count.ToString(), Runtime.MinimumRequiredTokens(Me.GetType()).ToString()), "Tokens: " & Me.Tokens_.GetValuesAndLocationsString())

				REM Ensure that the last Token is a {LineEnd}
				If Not Runtime.BuiltInTPVs.LineEnd_TPV.IsSatisfiedBy(Me.Tokens_.Last()) Then Throw New DSUnexpectedTokenException("The Last Token was not a {LineEnd}", Me.Tokens_)

				REM Ensure that the penultimate Token is the StatementEnd
				Runtime.BuiltInTPVs.StatementEnd_If_TPV.EnsureIsSatisfiedBy({Me.Tokens_(Me.Tokens_.Length - 2)})

				REM Ensure that the main TPV herefor is satisfied
				Runtime.BuiltInTPVs.IfStatement_UpToIncOpenBracket_TPV.EnsureIsSatisfiedBy(Me.Tokens_)

				REM On the first line, there should be at least 5 Tokens
				'If ( Expr ) {LineEnd}
				Dim _Statement_FirstLine As Runtime.Token() = Me.Tokens_.UpToAndIncluding(AddressOf Runtime.BuiltInTPVs.LineEnd_TPV.IsSatisfiedBy)
				If Not (_Statement_FirstLine.Length >= 5) Then Throw New DSValidationException("The first line of the Statement was not syntactically-valid, because it didn't contain enough Tokens", Me.Tokens_.GetValuesAndLocationsString())

				REM Ensure that the First Line ends as we expect it to
				If Not _Statement_FirstLine.TakeBlockFromEnd(_Count:=2).SatisfiesTPV(Runtime.BuiltInTPVs.Statement_FirstLineEnding_TPV) Then Throw New DSUnexpectedTokenException("The Statement's First Line should have ended with [)] and [{LineEnd}], but did not.", _Statement_FirstLine)

				REM We now know that there is at least 1 Token for the ConditionExpr
				'Skip() the Keyword and OpeningFunctionBracket, then Take all Tokens upto the [) {LineEnd}]...
				Me.ConditionExpr = Expressions.ConstructExpressionFromTokens(_Statement_FirstLine.Skip(2).ToArray().UpToButExcludingLast(AddressOf Runtime.BuiltInTPVs.ClosingFunctionBracket_TPV.IsSatisfiedBy))

				REM We now need to get the (If) Contents and the ElseContents
				Dim _Contents_And_ElseContents As Tuple(Of Runtime.Token(), Runtime.Token()) = GetContentsAndElseContents_(Me.Tokens_)

				Me.Contents_ = [StatementUtilities].GetStatementContentsFromTokens(_Contents_And_ElseContents.Item1).ToList()

				If _Contents_And_ElseContents.Item2 IsNot Nothing Then _
				 Me.ElseContents_ = [StatementUtilities].GetStatementContentsFromTokens(_Contents_And_ElseContents.Item2).ToList()

				LogLexingMessage("...Finished constructing an IfStatement Object with " & Me.Contents.Count.ToString() & " Child Instruction(s)")
			Catch _Ex As Exception : Throw New DSException("@IfStatement\ProcessTokensToInitialiseFields: " & _Ex.Message, _Ex) : End Try
		End Sub

		''' <summary>If there isn't an Else Clause in the Tokens, then [Item2] of the Tuple is Nothing (null)</summary>
		Protected Shared Function GetContentsAndElseContents_(ByVal _AllTokens As Runtime.Token()) As Tuple(Of Runtime.Token(), Runtime.Token())

			REM Input E.g.
			'	If (True) Then
			'		If (True) Then
			'			Output("0")
			'		Else
			'			If (True) Then
			'				Output("0")
			'			Else
			'				Output("1")
			'			End If
			'		End If
			'	Else
			'		If (True) Then
			'			Output("5")
			'		Else
			'			Output("6")
			'		End If
			'	End If

			REM Output E.g.
			'	Item1 = If ( True ) Then {LineEnd} Output ( "0" ) {LineEnd} ...
			'	Item2 = If ( True ) Then {LineEnd} Output ( "5" ) {LineEnd} ...

			REM The Contents ends (and the ElseContents begins) at the First Else where the DepthCounter is 1

			Dim _IfStatementDepth_Counter As UInt16 = 1	'Initialised to 1, because the ForLoop starts from Tokens that are already inside the FIRST IfStatement (there may, of course, only be this singular IfStatement, with no SubIfStatements inside it...)
			Dim _ContentsTokens As New List(Of Runtime.Token)()

			'From [after the first {LineEnd}] to [before the last EndIf]
			For _TokenIndex% _
			  = _AllTokens.IndexOfFirstWhere(AddressOf [Runtime].BuiltInTPVs.LineEnd_TPV.IsSatisfiedBy) + 1 _
			  To _AllTokens.IndexOfLastWhere(AddressOf [Runtime].BuiltInTPVs.StatementEnd_If_TPV.IsSatisfiedBy) - 1 _
			  Step +1

				If Runtime.BuiltInTPVs.Keyword_If_TPV.IsSatisfiedBy(_AllTokens.ElementAt(_TokenIndex)) Then : _IfStatementDepth_Counter += 1US
				ElseIf Runtime.BuiltInTPVs.Keyword_Else_TPV.IsSatisfiedBy(_AllTokens.ElementAt(_TokenIndex)) Then

					If _IfStatementDepth_Counter = 1 Then

						REM If we're here, then the current Token is the Else for our IfStatement

						'From [After the {LineEnd} which follows the Else] to [before the last EndIf]
						If Not {_AllTokens.ElementAt(_TokenIndex + 1)}.SatisfiesTPV(Runtime.BuiltInTPVs.LineEnd_TPV) Then Throw New DSUnexpectedTokenException("The Token following the Else was not a {LineEnd}", _AllTokens.ElementAt(_TokenIndex + 1))
						Dim _ElseContentsTokens As Runtime.Token() = _AllTokens.Skip(_TokenIndex + 2).ToArray().UpToButExcludingLast(AddressOf [Runtime].BuiltInTPVs.StatementEnd_If_TPV.IsSatisfiedBy)

						LogLexingMessage("Determined that an IfStatement did contain an Else Clause; Returning both the Contents and ElseContent Tokens")
						Return New Tuple(Of Runtime.Token(), Runtime.Token())(_ContentsTokens.ToArray(), _ElseContentsTokens)

					End If

				ElseIf Runtime.BuiltInTPVs.StatementEnd_If_TPV.IsSatisfiedBy(_AllTokens.ElementAt(_TokenIndex)) Then : _IfStatementDepth_Counter -= 1US
				End If

				'We're not up to the Else yet, so the Token is one for the (TruePart) Contents
				_ContentsTokens.Add(_AllTokens.ElementAt(_TokenIndex))

			Next

			'If we're here, then its because we didn't Return in the Else catcher, because there wasn't an Else for the IfStatement.
			'Therefore, we Return [Nothing] for the ElseContentsTokens
			LogLexingMessage("Determined that an IfStatement did not contain an Else Clause; Returning just the Contents Tokens")
			Return New Tuple(Of Runtime.Token(), Runtime.Token())(_ContentsTokens.ToArray(), Nothing)

		End Function

		''' <summary>Any Expressions or Child Instructions executed hereby, are returned in the ExeRes's UpstairsExecutionResults member.</summary>
		Public Function Execute(ByVal _InputSymbolTables As Runtime.SymbolTablesSnapshot) As ExecutionResult Implements IInstruction.Execute

			REM IfStatement Execution Process
			'	- Create the SymbolTable for this IStatement → Me.ScopedVariables
			'	- Create a New SymTblsSnapshot from the _InputSymbolTables, and Me.ScopedVars
			'
			'	- Resolve() Me.ConditionExpr → _Resolved_ConditionExpr
			'	- If _Resolved_ConditionExpr:
			'		- For Each Child IInstruction in Me.Contents:
			'			- Pass in _ModifiedSymTbls to Execute()
			'			- Add ExeRes thereof to UpstairsExecutionResults
			'			- Apply the returned SymTbls to _ModifiedSymTbls
			'			- Check if the _ExeRes's ReturnStatus's CurrentDSFunction_ReturnHasOccured is [True]
			'				- If it is, stop executing Child Instructions, and Copy the value of CurrentDSFunction_ReturnValue to my _ExeRes.
			'	- Else (If Not _Resolved_ConditionExpr):
			'		- For Each Child IInstruction in Me.ElseContents:
			'			- Pass in _ModifiedSymTbls to Execute()
			'			- Add ExeRes thereof to UpstairsExecutionResults
			'			- Apply the returned SymTbls to _ModifiedSymTbls
			'			- Check if the _ExeRes's ReturnStatus's CurrentDSFunction_ReturnHasOccured is [True]
			'				- If it is, stop executing Child Instructions, and Copy the value of CurrentDSFunction_ReturnValue to my _ExeRes.
			'
			'	- Re-apply the Topmost SymTbl's state to Me.ScopedVars
			'	- Add Me.ScopedVars as an Archived UpstairsSymTbl to the _ExeRes

			Try : Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("IfStatement")

				LogExecutionMessage("(IfStatement: Received " & _InputSymbolTables.InnerSymbolTables_Count.ToString() & " Input Symbol Tables. Statement contains " & Me.Contents.Count.ToString() & " Child Instruction(s) in Contents, and " & If(Me.ElseContents IsNot Nothing, Me.ElseContents.Count, 0).ToString() & " Child Instruction(s) in ElseContents)")
				Me.ScopedVariables = New Runtime.SymbolTable("IfStatement")
				LogExecutionMessage("(Generated Local SymbolTable for IfStatement)")
				Dim _Modified_SymTbls As Runtime.SymbolTablesSnapshot = _InputSymbolTables.SnapshotFrom_ThisAndTopmost(_NewTopmost:=Me.ScopedVariables)	'Push this Statement's Locals onto the working SymTbls

				'Iterate this many times
				Dim _Resolved_ConditionExpr As Boolean = Nothing

				REM <Resolve the ConditionExpr>

				'Log
				LogExecutionMessage("Resolving IfStatement ConditionExpr of type " & Me.ConditionExpr.GetType().FullName.InSquares() & "...")

				'Execute()
				Dim _ConditionExpr_Resolution_ExeRes As ExecutionResult = Me.ConditionExpr.Resolve(_Modified_SymTbls)

				'Add as an Upstairs ExeRes
				_ExeRes.UpstairsExecutionResults.Add(New Tuple(Of String, ExecutionResult)("ConditionExpr", _ConditionExpr_Resolution_ExeRes))

				'Update my SymbolTables; 
				_Modified_SymTbls = _ConditionExpr_Resolution_ExeRes.ResultantSymbolTablesState

				'Extract the Value for _ConditionExpr_Result
				_Resolved_ConditionExpr = _ConditionExpr_Resolution_ExeRes.ReturnStatus.IExpression_ResolutionResult.Coerce(Of Variables.DSBoolean)().Value
				LogExecutionMessage("...Resolved IfStatement ConditionExpr to " & _Resolved_ConditionExpr.ToString().InSquares())

				REM </Resolve the ConditionExpr>

				REM !!!!!!!!!!!!!!!!!!!!!!!!!!!!
				REM	    <Contents Execution>
				REM !!!!!!!!!!!!!!!!!!!!!!!!!!!!

				'No Return has occured yet
				_ExeRes.[ReturnStatus].CurrentDSFunction_ReturnHasOccurred = False

				If _Resolved_ConditionExpr Then

					LogExecutionMessage("IfStatement ConditionExpr resolved to True; executing Contents Instructions")

					For Each _ChildInstruction As IInstruction In Me.Contents

						'Log
						LogExecutionMessage("Executing IfStatement Contents Child Instruction of Type " & _ChildInstruction.GetType().Name.InSquares())

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

				Else '_Resolved_ConditionExpr was False

					LogExecutionMessage("IfStatement ConditionExpr resolved to False; executing ElseContents Instructions if existent")

					REM Only Execute() the ElseContents if it exists
					If Me.ElseContents IsNot Nothing Then

						For Each _ChildInstruction As IInstruction In Me.ElseContents

							'Log
							LogExecutionMessage("Executing IfStatement ElseContents Child Instruction of Type " & _ChildInstruction.GetType().Name.InSquares())

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

					End If

				End If

				REM !!!!!!!!!!!!!!!!!!!!!!!!!!!!
				REM	    </Contents Execution>
				REM !!!!!!!!!!!!!!!!!!!!!!!!!!!!

				REM Re-apply the State to Me.ScopedVars
				Me.ScopedVariables = _Modified_SymTbls.Topmost : _ExeRes.Archived_SymbolTable = Me.ScopedVariables

				_ExeRes.StopExecutionTimer_AndFinaliseObject(_Modified_SymTbls.BottomStack)	'Don't return this IStatement's Locals
				LogLexingMessage(String.Format("(...IfStatement produced DSFunction ReturnValue [{0}].", If(_ExeRes.ReturnStatus.CurrentDSFunction_ReturnValue, New Variables.DSString("(No Return Value)")).ToString()), LogEvent.DSEventSeverity.Verbose)
				Return _ExeRes

			Catch _Ex As Exception : Throw New DSException("@IfStatement\Execute: " & _Ex.Message, _Ex) : End Try

		End Function

		''' <summary>Returns an XML Representation of the Instruction, with all Propities and Child Structres included</summary>
		Public Function GetProgramTreeNodeXML() As System.Xml.Linq.XElement Implements IInstruction.GetProgramTreeNodeXML

			Dim _XElementToReturn As XElement = <IfStatement/>

			_XElementToReturn.Add(Me.ConditionExpr.GetExprTreeNodeXML().WrapIn("ConditionExpr"))

			Dim _ContentsXElement As XElement = <Contents/>
			For Each _IInstruction As IInstruction In Me.Contents
				_ContentsXElement.Add(_IInstruction.GetProgramTreeNodeXML())
			Next
			_XElementToReturn.Add(_ContentsXElement)

			Dim _ElseContentsXElement As XElement = <ElseContents/>
			If Me.ElseContents IsNot Nothing Then
				For Each _IInstruction As IInstruction In Me.ElseContents
					_ElseContentsXElement.Add(_IInstruction.GetProgramTreeNodeXML())
				Next
			End If
			_XElementToReturn.Add(_ElseContentsXElement)

			Return _XElementToReturn

		End Function

		''' <summary>Constructs an instance of the IInstruction class from its XML serialasation</summary>
		Public Shared Function FromXML(ByVal _IInstructionXElement As XElement) As IfStatement
			Try

				Dim _ElseContents As List(Of IInstruction) = _
				 StatementUtilities.GetStatementContentsFromXML(_IInstructionXElement.<ElseContents>.First().Elements().ToArray()).ToList()

				Return New IfStatement(
				 _ConditionExpr:=Expressions.ExprUtilities.ConstructExpressionFromXML(_IInstructionXElement.<ConditionExpr>.First().Elements().First()),
				 _Contents:=StatementUtilities.GetStatementContentsFromXML(_IInstructionXElement.<Contents>.First().Elements().ToArray()).ToList(),
				 _ElseContents:=If(_ElseContents.Count = 0, Nothing, _ElseContents)
				)

			Catch _Ex As Exception : Throw New DSException("@IfStatement\FromXML: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>Returns what the Instruction would have looked like in the Source, without an extra LineBreak on the End</summary>
		Public Overrides Function ToString() As String

			REM E.g.
			'	IF (CONDITION_EXPR)
			'		
			'		OUTPUT("H_2 O")
			'	
			'	ELSE
			'		
			'		OUTPUT("H_2 O_2")
			'		
			'	ENDIF

			Return _
			 Constants.Keyword_If & Constants.Space & Constants.OpeningFunctionBracket & Me.ConditionExpr.ToString() & Constants.ClosingFunctionBracket & Constants.LineBreak & _
			 Constants.LineBreak & _
			 ReconstructStatementContentsAsSource(Me.Contents.ToArray()).TabbedInBy(1) & Constants.LineBreak & _
			 Constants.LineBreak & _
			 If(Me.ElseContents Is Nothing, "", Constants.Keyword_Else & Constants.LineBreak & Constants.LineBreak & ReconstructStatementContentsAsSource(Me.ElseContents.ToArray()).TabbedInBy(1) & Constants.LineBreak & Constants.LineBreak) & _
			 Constants.StatementEnd_If

		End Function

	End Class

End Namespace