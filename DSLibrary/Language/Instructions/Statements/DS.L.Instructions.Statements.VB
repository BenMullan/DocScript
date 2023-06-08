Imports DocScript.Runtime.BuiltInTPVs

Namespace Language.Instructions.Statements

	''' <summary>The Base Interface for all Statement Instructions in DocScript (E.g. Implemented by DSFunction and IfStatement)</summary>
	Public Interface IStatement : Inherits IInstruction
		Property ScopedVariables As Runtime.SymbolTable
		ReadOnly Property Contents As ObjectModel.ReadOnlyCollection(Of IInstruction)
	End Interface

	Public Module StatementUtilities

#Region "Statement Contents Lexing Logic"

		REM E.g. Input:
		REM		<String> Name : "Ben"
		REM		If (Name = GetName(0))
		REM			Output("Was Name 0")
		REM		EndIf

		REM E.g. Output:
		REM		[VariableDeclaration]
		REM		[IfStatement] (1 Child Instruction)
		REM Left-over Tokens: NONE


		REM E.g. Input:
		REM				Output("Was Name 0")
		REM			EndWhile
		REM		EndIf

		REM E.g. Output:
		REM		[FunctionCall]
		REM Left-over Tokens: EndIf


		REM The valid Possible lines are:
		REM		Output("Hello, World!")
		REM		Return
		REM		Return GetTypeOfNumber(11100001_2)
		REM		Name : "Ben"
		REM		<String> Name
		REM		<String> Name : "Ben"
		'
		REM		If (True ' False)
		REM		Loop (10)
		REM		While (True)
		'
		REM		EndIf
		REM		EndLoop
		REM		EndWhile

		''' <summary>The Lexing Function to construct the contents of a DSFunction, IfStatement, WhileStatement, or LoopStatement</summary>
		Public Function GetStatementContentsFromTokens(ByVal _StatementContentsTokens As Runtime.Token()) As IInstruction()
			Try : LogLexingMessage("Entering ConstructStatementContentsFromTokens() call with " & _StatementContentsTokens.Length.ToString() & " Token(s)...")

				'If there are no Tokens, then the Statement is empty (which is useless for the program, but fine...)
				If _StatementContentsTokens.Length = 0 Then
					LogLexingMessage("...The Statement contains no Tokens and is Empty") : Return (New List(Of IInstruction)()).ToArray()
				End If

				REM Ensure the last Token is a {LineEnd}
				If Not {_StatementContentsTokens.Last()}.SatisfiesTPV(LineEnd_TPV) Then Throw New DSMissingTokenException("The _Tokens did not end in a {LineEnd}. The Tokens were: " & _StatementContentsTokens.GetValuesAndLocationsString(), New Runtime.Token(vbCrLf, Runtime.Token.TokenType.LineEnd, 0, 0))

				'Collects the IInstructions which are children of the current call
				Dim _DerivedInstructions As New List(Of IInstruction)()

				REM _TokenIndex means: The Index of the NEXT TOKEN TO LOOK AT
				For _TokenIndex% = 0 To (_StatementContentsTokens.Length - 1) Step +0 'The Incrementation is performed inside the Loop, not by this header

					REM Could be:
					'	If
					'	While
					'	Loop
					'	<
					'	VariableName
					'	FunctionName
					'	Return

					'	EndIf
					'	EndWhile
					'	EndLoop

					REM Whenever a StatementEnd is encountered, the current call must return, along with [all the derived instructions] and [all remaining Tokens]

					'E.g.	While ( True ) {LineEnd}
					Dim _CurrentLine As Runtime.Token() = _StatementContentsTokens.Skip(_TokenIndex).ToArray().UpToAndIncluding(AddressOf Runtime.BuiltInTPVs.LineEnd_TPV.IsSatisfiedBy)
					'MsgDebug("CurrentLine: " & _CurrentLine.GetStandardArraySerialisation(vbCrLf & vbCrLf))

					Select Case True

						Case FunctionCall_UpToIncOpenBracket_TPV.IsSatisfiedBy(_CurrentLine) : _DerivedInstructions.Add(New FunctionCall(_CurrentLine))					'FunctionCall			Output("Hello, World!")
						Case Keyword_Return_TPV.IsSatisfiedBy(_CurrentLine) : _DerivedInstructions.Add(New ReturnToCaller(_CurrentLine))								'ReturnToCaller			Return GetTypeOfNumber(11100001_2)
						Case VariableAssignmet_TPV.IsSatisfiedBy(_CurrentLine) : _DerivedInstructions.Add(New VariableAssignment(_CurrentLine))							'VariableAssignment		Name : "Ben"
						Case VariableDeclaration_UpToIncIdentifier_TPV.IsSatisfiedBy(_CurrentLine) : _DerivedInstructions.Add(New VariableDeclaration(_CurrentLine))	'VariableDeclaration	<String> Name : "Ben"

						Case SubStatementOpening_TPV.IsSatisfiedBy(_CurrentLine)

							'IfStatement	If (True ' False)
							'LoopStatement	Loop (10)
							'WhileStatement	While (True)

							REM We've just hit the beginning of another statement.
							REM We don't know where it ends, so we pass ALL REMAINING TOKENS into GetSubStatementFromRemainingTokens_(), which constructs the IStatement Object for us, and returns the left-over Tokens that we need to process in this Function.
							REM "ALL REMAINING TOKENS" means [_RemainingTokens minus the Tokens we've processed already]

							Dim _SubStatement_IStatement_And_LeftOverTokens As Tuple(Of IStatement, Runtime.Token()) = GetSubStatementFromRemainingTokens_(_StatementContentsTokens.Skip(_TokenIndex).ToArray())

							'Add the Statement Object
							_DerivedInstructions.Add(_SubStatement_IStatement_And_LeftOverTokens.Item1)

							'Set the next Iteration of the ForLoop to start from the Tokens after those of the Statement we've just added
							'_StatementContentsTokens = _SubStatement_IStatement_And_LeftOverTokens.Item2
							'_TokenIndex = 0

							'The next Token to look at is the first one after the Statement we've just dealt with.
							'We want to INCREMENT the _TokenIndex, by the NUMBER OF TOKENS IN THE STATEMENT.
							'This can be calculated by subtracting [the number of Tokens which occur after that Statement] from [the total number of Tokens wthat were orrigionally passed into this function].
							'In heinzsight, it may have been better to just have the GetSubStatementFromRemainingTokens_() Method return [the number of Tokens in the just-constructed Statement]...

							'**ChangeLog AMENDMENT 05122022: += Assignment changed to = assignment.
							_TokenIndex = (_StatementContentsTokens.Length - _SubStatement_IStatement_And_LeftOverTokens.Item2.Length)
							'MsgDebug("Just added a " & _DerivedInstructions.Last().GetType().Name & "; RemainingTokens:" & vbCrLf & _SubStatement_IStatement_And_LeftOverTokens.Item2.GetStandardArraySerialisation(vbCrLf))

						Case Else : Throw New DSUnexpectedTokenException("The syntax of the following inner-statement line is invalid", _CurrentLine)
					End Select

					REM Now we need to increment the _TokenIndex pointer by the number of Tokens in [the line just-processed]...
					REM	...Unless the [Instruction just added] was a Statement, in which case the _TokenIndex ought to be left at 0, so that we start from the first post-Statement Token
					If Not SubStatementOpening_TPV.IsSatisfiedBy(_CurrentLine) Then _TokenIndex += _CurrentLine.Length

				Next

				LogLexingMessage("...Returning from ConstructStatementContentsFromTokens() call, having derived " & _DerivedInstructions.Count.ToString() & " Instruction(s)")

				'There are no left-over Tokens, because the ↑ForLoop↑ would have covered them all (?)

				Return (_DerivedInstructions.ToArray())

			Catch _Ex As Exception : Throw New DSException("@GetStatementContentsFromTokens: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>Returns [1] The first possible IStatement which could be constructed, and [2] The Left-over Tokens which followed [the Tokens of the constructed IStatement Object]</summary>
		Private Function GetSubStatementFromRemainingTokens_(ByRef _AllRemainingTokens As Runtime.Token()) As Tuple(Of IStatement, Runtime.Token())
			Try

				REM E.g. Input [↓]:
				REM		If (True)
				REM			While (AnotherFileExists())
				REM				Output("File: " & GetNextFile())
				REM			EndWhile
				REM			If (¬ AnotherFileExists())
				REM				Output("All Files Processed")
				REM			EndIf
				REM		EndIf
				REM		If (False | True)
				REM			SayHello()
				REM		EndIf
				REM		<String> Name : "Ben"
				REM		Input(Name & " needs some Input")

				REM E.g. Output [↓]:
				REM		[IfStatement] (2 Children)
				REM	Left-over Tokens: If ( False | True ) {LineEnd} SayHello ( ) {LineEnd} EndIf {LineEnd} < String > Name : "Ben" {LineEnd} Input ( Name & " needs some Input" ) {LineEnd}

				'The Input to this Function must start with one of the Tokens [If], [While], or [Loop]
				If Not SubStatementOpening_TPV.IsSatisfiedBy(_AllRemainingTokens.First()) Then Throw New DSUnexpectedTokenException("The SubStatement must start with one of [If], [While], or [Loop].", _AllRemainingTokens.First())

				'In order to construct the IStatement Object, we need all its Tokens.
				'What we've been passed (via _AllRemainingTokens), however, may contain tokens which are for something AFTER Statement we're constructing.
				'These are the LEFT-OVER TOKENS, and we'll return them in Item2 of the returned Tuple

				'The Tokens for the current SubStatement
				Dim _SubStatementTokens As Runtime.Token()					'Value is assigned in the following SelectCase [↓]

				'The Statement Object contructed *from* those Tokens
				Dim _ConstructedSubStatement_ToReturn As IStatement			'Value is assigned in the following SelectCase [↓]

				'The Tokens which aren't needed for the Statement Object
				Dim _LeftOverTokens_ToReturn As Runtime.Token()				'Value is assigned after the following SelectCase [↓]

				REM Work out which type of IStatement we're constructing
				Select Case True

					Case Keyword_If_TPV.IsSatisfiedBy(_AllRemainingTokens.First())
						_SubStatementTokens = GetSubStatementTokens_(GetType(IfStatement), _AllRemainingTokens)
						_ConstructedSubStatement_ToReturn = New IfStatement(_SubStatementTokens)

					Case Keyword_While_TPV.IsSatisfiedBy(_AllRemainingTokens.First())
						_SubStatementTokens = GetSubStatementTokens_(GetType(WhileStatement), _AllRemainingTokens)
						_ConstructedSubStatement_ToReturn = New WhileStatement(_SubStatementTokens)

					Case Keyword_Loop_TPV.IsSatisfiedBy(_AllRemainingTokens.First())
						_SubStatementTokens = GetSubStatementTokens_(GetType(LoopStatement), _AllRemainingTokens)
						_ConstructedSubStatement_ToReturn = New LoopStatement(_SubStatementTokens)

					Case Else : Throw New DSUnexpectedTokenException("The SubStatement Instruction Type could not be determined", _AllRemainingTokens.First())
				End Select

				_LeftOverTokens_ToReturn = _AllRemainingTokens.TakeBlockFromEnd(CUInt(_AllRemainingTokens.Length - _SubStatementTokens.Length))
				Return New Tuple(Of IStatement, Runtime.Token())(_ConstructedSubStatement_ToReturn, _LeftOverTokens_ToReturn)

				LogLexingMessage(String.Format("Constructed SubStatement of Type {0} from {1} Total Remaining Tokens, with {2} left-over Tokens", _ConstructedSubStatement_ToReturn.GetType().Name.InSquares(), _AllRemainingTokens.Length.ToString(), _LeftOverTokens_ToReturn.Length.ToString()))
			Catch _Ex As Exception : Throw New DSException("@GetSubStatementFromRemainingTokens_: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>Takes in some Tokens which begin with a Statement, and returns only the Tokens for that Statement.</summary>
		''' <param name="_AllRemainingTokens">Must start with a Statement-Opening Keyword</param>
		''' <param name="_IStatementType">Either IfStatement, WhileStatement, or LoopStatement</param>
		Private Function GetSubStatementTokens_(ByVal _IStatementType As Type, ByVal _AllRemainingTokens As Runtime.Token()) As Runtime.Token()
			Try

				'[↓] An Exception will be Thrown if an attempt is made to access the dictionary with a Key which it dosen't contain
				Static _IStatementType_To_TPVsTherefor_Dictionary_ As New Dictionary(Of Type, Tuple(Of Runtime.TokensToPatternsValidator, Runtime.TokensToPatternsValidator))() From {
				 {GetType(IfStatement), New Tuple(Of Runtime.TokensToPatternsValidator, Runtime.TokensToPatternsValidator)(Keyword_If_TPV, StatementEnd_If_TPV)},
				 {GetType(WhileStatement), New Tuple(Of Runtime.TokensToPatternsValidator, Runtime.TokensToPatternsValidator)(Keyword_While_TPV, StatementEnd_While_TPV)},
				 {GetType(LoopStatement), New Tuple(Of Runtime.TokensToPatternsValidator, Runtime.TokensToPatternsValidator)(Keyword_Loop_TPV, StatementEnd_Loop_TPV)}
				}

				REM The Statement Ends at the First RelevantStatementEnd where the DepthCounter is equal to 1
				'	Set a DepthCounter to 0
				'	Every time we hit an Opening for our Statement Type, DepthCounter += 1
				'	Every time we hit a Closing for our Statement Type, if the DepthCounter is 1, then we've hit the end of the Statement's Tokens. Otherwise, DepthCounter -= 1

				Dim _SubStatementsTokens_ToReturn As New List(Of Runtime.Token)

				Dim _RelevantStatementType_OpeningTPV As Runtime.TokenPatternValidation.TokensToPatternsValidator = _IStatementType_To_TPVsTherefor_Dictionary_.Item(_IStatementType).Item1
				Dim _RelevantStatementType_ClosingTPV As Runtime.TokenPatternValidation.TokensToPatternsValidator = _IStatementType_To_TPVsTherefor_Dictionary_.Item(_IStatementType).Item2

				Dim _RelevantStatementType_DepthCounter As Int32 = 0

				For _TokenIndex% = 0 To (_AllRemainingTokens.Length - 1) Step +1 _
				: _SubStatementsTokens_ToReturn.Add(_AllRemainingTokens(_TokenIndex))

					If _RelevantStatementType_OpeningTPV.IsSatisfiedBy(_AllRemainingTokens(_TokenIndex)) Then : _RelevantStatementType_DepthCounter += 1
					ElseIf _RelevantStatementType_ClosingTPV.IsSatisfiedBy(_AllRemainingTokens(_TokenIndex)) Then

						If _RelevantStatementType_DepthCounter = 1 Then

							'We're currently on the StatementEnd at which our Statement's Tokens end.

							'We just need the {LineEnd} on the end.
							If Not LineEnd_TPV.IsSatisfiedBy(_AllRemainingTokens(_TokenIndex + 1)) Then Throw New DSUnexpectedTokenException("A LineEnd should have been present following the end of the Statement's Tokens", _AllRemainingTokens(_TokenIndex + 1))

							LogLexingMessage("Determined that a SubStatement posessed " & _SubStatementsTokens_ToReturn.Count.ToString() & " Tokens")
							Return _SubStatementsTokens_ToReturn.Concat({_AllRemainingTokens(_TokenIndex + 1)}).ToArray()

						Else : _RelevantStatementType_DepthCounter -= 1
						End If

					End If

				Next

				'If we haven't returned yet, then something has gone wrong
				Throw New DSException("[SHOULD NEVER SEE] The end of the Statement was not successfully identified, so the Function did not return where it should have (which is [from inside the StatementDepth-determination loop])")

			Catch _Ex As Exception : Throw New DSException("@GetSubStatementTokens_: " & _Ex.Message, _Ex) : End Try
		End Function

#End Region

		''' <summary>
		''' Recursivly reconstructs the IInstructions as DocScript Source.
		''' Keywords all become UPPERCASE, and Tabs are inserted according to SubStatement Depth
		''' <para>
		''' (actually that's not done herein, but rather, in the ToString() Methods of the Statement Classes...)
		''' ((...But the return value from this function has the same effect applied...))
		''' (((...Because this function just calls the ToString() method on each IInstruction, and concatenates them with some ancilliary formatting...)))
		''' ((((...In the form of a {LineBreak} betwixt each IInstruciton.ToString().))))
		''' </para>
		''' </summary>
		Public Function ReconstructStatementContentsAsSource(ByRef _StatementContents As IInstruction()) As String

			REM E.g. Input:
			'	[IfStatement] (2 Children)

			REM E.g. Output:
			'	IF (CONDITION)
			'
			'		OUTPUT("Hello, World!")
			'		NAME = INPUT("Enter Name:")
			'
			'	ENDIF

			Dim _SourceToReturn$ = Language.Constants.LineBreak	'This is so that the Statement has a LineBreak AFTER its header. (The IStatement's ToString() itself will add one for AFTER the contents).

			For Each _Instruction As IInstruction In _StatementContents
				_SourceToReturn &= _Instruction.ToString() & Language.Constants.LineBreak
			Next

			REM Remove the last LineBreak, if the Loop iterated at all
			If _StatementContents.Length >= 1 Then _SourceToReturn = _SourceToReturn.WithTrailingWhiteSpaceRemoved()

			Return _SourceToReturn

		End Function

	End Module

End Namespace