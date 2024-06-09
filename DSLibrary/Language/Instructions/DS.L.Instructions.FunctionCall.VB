Namespace Language.Instructions

	''' <summary>Represents E.g. [GetAge("Ben", 2)]</summary>
	Public Class FunctionCall : Implements DocScript.Language.Instructions.IInstruction

#Region "Declarations specific to this IInstruction"

		Public TargetFunction_Identifier As [String]
		Public Arguments As Language.Expressions.IExpression() 'If there are no Arguments, then this is an Empty Array (not Nothing)

#End Region

		Protected ReadOnly Tokens_ As Runtime.Token()

		''' <summary>Constructs the IInstruction from an existing instance</summary>
		Public Sub New(ByVal _TargetFunction_Identifier$, ByVal _Arguments As Expressions.IExpression())
			Me.TargetFunction_Identifier = _TargetFunction_Identifier : Me.Arguments = _Arguments
		End Sub

		''' <summary>Constructs the IInstruction from its Tokens</summary>
		Public Sub New(ByRef _Tokens As Runtime.Token())
			Me.Tokens_ = _Tokens : Me.ProcessTokensToInitialiseFields()
		End Sub

		Public Sub ProcessTokensToInitialiseFields()
			Try : LogLexingMessage("Began constructing a FunctionCall...")

				REM Source should look like:
				REM		GetAge("Ben", [5 - 2], GetAddr("Ben", 18_10))

				REM Tokens should look like:
				REM		[Identifier], [GrammarChar], [?ExprTokens...Comma...], [GrammarChar], [LineEnd]

				REM Fields to Initialise:
				REM		TargetFunction_Identifier
				REM		Arguments

				REM Ensure that there are enough tokens to construct the IInstruction
				If Me.Tokens_.Count < Runtime.MinimumRequiredTokens(Me.GetType()) Then Throw New DSValidationException(String.Format("The No. Tokens ({0}) was less than the minimum No. Tokens required to construct the Instruction ({1}).", Me.Tokens_.Count.ToString(), Runtime.MinimumRequiredTokens(Me.GetType()).ToString()), "Tokens: " & Me.Tokens_.GetValuesAndLocationsString())

				REM Ensure that the last Token is a {LineEnd}
				If Not Runtime.BuiltInTPVs.LineEnd_TPV.IsSatisfiedBy(Me.Tokens_.Last()) Then Throw New DSUnexpectedTokenException("The Last Token was not a {LineEnd}", Me.Tokens_)

				REM Ensure that the main TPV herefor is satisfied
				Runtime.BuiltInTPVs.FunctionCall_UpToIncOpenBracket_TPV.EnsureIsSatisfiedBy(Me.Tokens_)

				REM The TargetFunction_Identifier should be derivable from the 1st Token
				Me.TargetFunction_Identifier = Me.Tokens_(0).Value

				'We now need to work out how many Arguments there are.
				If Me.Tokens_.Length < 5 Then : Me.Arguments = {}
				Else

					'There are some Arguments
					'We can just piggy-back off of the FunctionCallExpr Logic we've already written in the [Expressions] Namespace..
					'...Which can lex and seperate the Arguments for us.

					Dim _ExprMadeFromFuncCallTokens As Expressions.IExpression = Expressions.ExprUtilities.ConstructExpressionFromTokens(Me.Tokens_)

					'If that Expr isn't of Type FunctionCallExpr, then something has gone wrong!
					If Not _ExprMadeFromFuncCallTokens.GetType() = GetType(Expressions.FunctionCallExpr) Then Throw New DSValidationException("It was detected that at least one argument was present for the FunctionCall, however on Lexing the FunctionCall;s Tokens into an IExpression, the Type of that IExpression did not come back as being a FunctionCallExpr.", "The Expr's Type was " & _ExprMadeFromFuncCallTokens.GetType().Name().InSquares() & ". The Tokens were: " & Me.Tokens_.GetValuesAndLocationsString())

					REM Now get the Arguments from that FunctionCallExpr. That all we need it for.
					Me.Arguments = CType(_ExprMadeFromFuncCallTokens, Expressions.FunctionCallExpr).SubExpressions

				End If

				LogLexingMessage("...Finished constructing a FunctionCall Object to " & Me.TargetFunction_Identifier.InSquares() & " with " & Me.Arguments.Count().ToString() & " Arguments")
			Catch _Ex As Exception : Throw New DSException("@FunctionCall\ProcessTokensToInitialiseFields: " & _Ex.Message, _Ex) : End Try
		End Sub

		''' <summary>Any Expressions or Child Instructions executed hereby, are returned in the ExeRes's UpstairsExecutionResults member.</summary>
		Public Function Execute(ByVal _InputSymbolTables As Runtime.SymbolTablesSnapshot) As ExecutionResult Implements IInstruction.Execute

			REM Differences between a FunctionCall and a FunctionCallExpr
			'	- FunctionCallExpr's Target Function MUST have a non-void ReturnType
			'	- FunctionCallExpr's Target Function MUST produce a ReturnValue

			Return FunctionCall.CallFunctionByName(
			 _InputSymbolTables:=_InputSymbolTables,
			 _TargetFunction_Identifier:=Me.TargetFunction_Identifier,
			 _UnresolvedArguments:=Me.Arguments,
			 _MustProduceReturnValue:=False
			)

		End Function

		''' <summary>Used by FunctionCall and FunctionCallExpr to call a BIF or DSFunction contained in the _InputSymbolTables. Arguments are passed in as UNRESOLVED Expressions.</summary>
		''' <remarks>Sounds rather Win32-y, dosen't it?</remarks>
		Public Shared Function CallFunctionByName(
		  ByVal _InputSymbolTables As Runtime.SymbolTablesSnapshot,
		  ByVal _TargetFunction_Identifier As System.String,
		  ByVal _UnresolvedArguments As Expressions.IExpression(),
		  ByVal _MustProduceReturnValue As System.Boolean
		) As Global.DocScript.Language.Instructions.ExecutionResult

			REM Differences between a [FunctionCall] and a [FunctionCallExpr]
			'	- FunctionCallExpr's Target Function MUST have a non-void ReturnType
			'	- FunctionCallExpr's Target Function MUST produce a ReturnValue

			REM DocScript Function Calling Process
			'	Resolve() all Arguments		(To IDataValues)	(The target DSFunction (or BIF) will perform the Type Coercion)
			'	
			'	Ensure that there is a SymbolTableEntry whose Identifier == _TargetFunction_Identifier... (Worry about ToUpper()ing here...)
			'		...And that the Type of this Entry is for a DSFunction or BIF
			'	Use _InputSymbolTables.GetEntryFromAny() to retreive the DSFunction or BIF, via _TargetFunction_Identifier
			'	Execute() the retreived Function with the Resolved Arguments
			'
			'	If _MustProduceReturnValue:
			'		- Ensure that the ExeRes of the Function Innvocation had a Return Value
			'		- Return this value, as [IExpression_ResolutionResult] (for FunctionCallExpr)

			Try
				'To be returned from this Resolve() call
				Dim _ExeRes As Language.Instructions.ExecutionResult = Instructions.ExecutionResult.New_AndStartExecutionTimer("CallFunctionByName" & _TargetFunction_Identifier.InBrackets())

				'For the returning _ExeRes
				LogExecutionMessage("(CallFunctionByName: Received " & _InputSymbolTables.InnerSymbolTables_Count.ToString() & " Input Symbol Tables)")
				Dim _ModifiedSymTbls As Runtime.SymbolTablesSnapshot = _InputSymbolTables
				Dim _FunctionReturnValue As Variables.IDataValue = Nothing

				'Ensure that the SymbolTables contain an Entry for the target Function
				If Not _InputSymbolTables.IsContainedInAny(_TargetFunction_Identifier) Then Throw New DSNonexistentSymbolException(_TargetFunction_Identifier, "CallFunctionByName() can therefore not continue")
				Dim _TargetFunction_SymTblEntry As Runtime.SymbolTable.ISymbolTableEntry = _InputSymbolTables.GetEntryFromAny(_TargetFunction_Identifier)
				If Not Runtime.SymbolTable.IsFunctionEntry(_TargetFunction_SymTblEntry) Then Throw New DSIncorrectSymbolTableEntryTypeException(_TargetFunction_Identifier, "A DSFunction or BuiltInFunction".InSquares(), _TargetFunction_SymTblEntry.GetType().FullName, "CallFunctionByName() can therefore not continue")

				'Resolve the Arguments (IExpr → IDataValue)
				LogExecutionMessage("Resolving " & _UnresolvedArguments.Length.ToString() & " Argument(s) to IDataValue(s)")
				Dim _ResolvedArguments As New List(Of Variables.IDataValue)()
				For Each _UnresolvedArgument As Expressions.IExpression In _UnresolvedArguments

					REM	_ModifiedSymTbls → [Resolve()] → _ModifiedSymTbls
					Dim _Argument_Resolution_ExeRes As Instructions.ExecutionResult = _UnresolvedArgument.Resolve(_ModifiedSymTbls)
					_ExeRes.UpstairsExecutionResults.Add(New Tuple(Of String, Instructions.ExecutionResult)("Argument", _Argument_Resolution_ExeRes))
					_ModifiedSymTbls = _Argument_Resolution_ExeRes.ResultantSymbolTablesState
					_ResolvedArguments.Add(_Argument_Resolution_ExeRes.ReturnStatus.IExpression_ResolutionResult)

				Next

				'The _TargetFunction_SymTblEntry should be for either a DSFunction or BuiltInFunction
				Select Case _TargetFunction_SymTblEntry.GetType()
					Case GetType(Runtime.SymbolTable.SymbolTableEntry(Of Instructions.Statements.DSFunction))

						Dim _DSFunction As Language.Instructions.Statements.DSFunction = CType(_TargetFunction_SymTblEntry, Runtime.SymbolTable.SymbolTableEntry(Of Instructions.Statements.DSFunction)).Value
						If _MustProduceReturnValue Then If Not Variables.VariableUtilities.IsNonVoidFunctionReturnType(_DSFunction.ReturnType) Then Throw New DSValidationException("The Target DSFunction does not have a non-void Return Type, and cannot therefore produce a value for an Expression", "Target Function's ReturnType: " & _DSFunction.ReturnType.FullName)

						REM Execute the DSFunction, and add the ExeRes as an Upstairs ExeRes
						REM Only pass in the Global SymTbl, just to be clear. The DSFunction dosen't use any of our other SymTbls anyway.
						If (_ModifiedSymTbls.Bottommost.Description <> Runtime.Constants.GlobalSymbolTable_Description) Then Throw New DSValidationException("The DSFunction will not be called because the Bottommost SymbolTable passed into the FunctionCallExpr was not the Global Symbol Table", "Bottommost SymTbl: " & _ModifiedSymTbls.Bottommost.Description.InSquares())
						Dim _DSFunction_ExeRes As Instructions.ExecutionResult = _DSFunction.Run(_InputSymbolTables:=_ModifiedSymTbls.Bottommost.SnapshotContainingJustThis, _Arguments:=_ResolvedArguments.ToArray())
						_ExeRes.UpstairsExecutionResults.Add(New Tuple(Of String, Instructions.ExecutionResult)("TargetDSFunction", _DSFunction_ExeRes))

						'A DSFunction ONLY RETURNS the new GlobalSymTbl State; we don't want to loose all the other SymTbls currently in _ModifiedSymTbls, so we won't assign directly to it
						If (_DSFunction_ExeRes.[ResultantSymbolTablesState].Bottommost.Description <> Runtime.Constants.GlobalSymbolTable_Description) Then Throw New DSValidationException("The DSFunction's Bottommost returned Symbol Table was not the Global Symbol Table", "Bottommost SymTbl: " & _DSFunction_ExeRes.ResultantSymbolTablesState.Bottommost.Description.InSquares())
						_ModifiedSymTbls.Bottommost = _DSFunction_ExeRes.ResultantSymbolTablesState.Bottommost

						'Extract the ReturnValue if there was one
						If _MustProduceReturnValue Then
							If (_DSFunction_ExeRes.ReturnStatus.CurrentDSFunction_ReturnValue Is Nothing) Then Throw New DSValidationException("When executed, the DSFunction had a Return Value of Nothing (null) [nullptr]. A value is needed for the Expression.", "(The Return Value of the DSFunction)")
							_FunctionReturnValue = _DSFunction_ExeRes.ReturnStatus.CurrentDSFunction_ReturnValue
						End If

					Case GetType(Runtime.SymbolTable.SymbolTableEntry(Of Runtime.BuiltInFunction))

						Dim _BIF As Runtime.BuiltInFunction = CType(_TargetFunction_SymTblEntry, Runtime.SymbolTable.SymbolTableEntry(Of Runtime.BuiltInFunction)).Value
						If _MustProduceReturnValue Then If Not Variables.VariableUtilities.IsNonVoidFunctionReturnType(_BIF.ReturnType) Then Throw New DSValidationException("The Target BuiltInFunction does not have a non-void Return Type, and cannot therefore produce a value for an Expression", "Target Function's ReturnType: " & _BIF.ReturnType.FullName)

						REM Execute the BuiltInFunction, and add the ExeRes as an Upstairs ExeRes
						Dim _BIF_ExeRes As Instructions.ExecutionResult = _BIF.Run(_InputSymbolTablesState:=_ModifiedSymTbls, _Arguments:=_ResolvedArguments.ToArray())
						_ExeRes.UpstairsExecutionResults.Add(New Tuple(Of String, Instructions.ExecutionResult)("TargetBIF", _BIF_ExeRes))

						'BIFs are given the privelage of not being restricted in their SymTbl access. We just trust them implicitly.
						_ModifiedSymTbls = _BIF_ExeRes.ResultantSymbolTablesState

						If _MustProduceReturnValue Then
							If (_BIF_ExeRes.ReturnStatus.BuiltInFunction_ReturnValue Is Nothing) Then Throw New DSValidationException("When executed, the BuiltInFunction had a Return Value of Nothing (null) [nullptr]. A value is needed for the Expression.", "(The Return Value of the BuiltInFunction)")
							_FunctionReturnValue = _BIF_ExeRes.ReturnStatus.BuiltInFunction_ReturnValue
						End If

					Case Else : Throw New DSException("Despite passing IsFunctionEntry(), the TargetFunction's SymbolTableEntry was for neither a DSFunction or BIF. It was: " & _TargetFunction_SymTblEntry.GetType().FullName)
				End Select

				_ExeRes.ReturnStatus.IExpression_ResolutionResult = _FunctionReturnValue 'Still Nothing if Not _MustProduceReturnValue
				Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_ModifiedSymTbls)

			Catch _Ex As Exception : Throw New DSException("@CallFunctionByName " & _TargetFunction_Identifier.InBrackets() & ": " & _Ex.Message, _Ex) : End Try

		End Function

		''' <summary>Returns an XML Representation of the Instruction, with all Propities and Child Structres included</summary>
		Public Function GetProgramTreeNodeXML() As System.Xml.Linq.XElement Implements IInstruction.GetProgramTreeNodeXML

			Dim _XElementToReturn As XElement = <FunctionCall TargetFunction_Identifier=<%= Me.TargetFunction_Identifier %>/>

			Dim _ArgumentsXElement As XElement = <Arguments/>
			For Each _Argument As Expressions.IExpression In Me.Arguments
				_ArgumentsXElement.Add(_Argument.GetExprTreeNodeXML())
			Next
			_XElementToReturn.Add(_ArgumentsXElement)

			Return _XElementToReturn

		End Function

		''' <summary>Constructs an instance of the IInstruction class from its XML serialasation</summary>
		Public Shared Function FromXML(ByVal _IInstructionXElement As XElement) As FunctionCall
			Try

				Return New FunctionCall(
				 _TargetFunction_Identifier:=_IInstructionXElement.@TargetFunction_Identifier,
				 _Arguments:=_IInstructionXElement.<Arguments>.First().Elements().Select(Of Expressions.IExpression)(AddressOf Expressions.ExprUtilities.ConstructExpressionFromXML).ToArray()
				)

			Catch _Ex As Exception : Throw New DSException("@FunctionCall\FromXML: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>Returns what the Instruction would have looked like in the Source, without an extra LineBreak on the End</summary>
		Public Overrides Function ToString() As String

			REM E.g.
			'	OUTPUT("Hello, World!")

			Return _
			 Me.TargetFunction_Identifier & Constants.OpeningFunctionBracket & _
			 String.Join(Constants.FunctionArgumentSeperator & Constants.Space, Me.Arguments.Select(Function(_Argument As Expressions.IExpression) _Argument.ToString())) & _
			 Constants.ClosingFunctionBracket

		End Function

	End Class

End Namespace