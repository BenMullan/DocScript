Imports DocScript.Language.Instructions

Namespace Runtime

	''' <summary>Contains logic for validating a list of Tokens against a Pattern</summary>
	Public Module TokenPatternValidation

		''' <summary>Represents a Pattern against which a single Token is validated</summary>
		Public Structure TokenPattern

			Public ReadOnly TokenType As Runtime.Token.TokenType
			Public ReadOnly TokenValueRegExp As [String]

			Public Sub New(ByVal _TokenType As Runtime.Token.TokenType, ByVal _TokenValueRegExp$)
				Me.TokenType = _TokenType : Me.TokenValueRegExp = _TokenValueRegExp
			End Sub

			Public Function IsSatisfiedBy(ByVal _Token As Runtime.Token) As Boolean
				Dim _WasSatisfied As Boolean = ((_Token.Type = Me.TokenType) AndAlso (_Token.Value.ToUpper().MatchesRegEx(Me.TokenValueRegExp)))
				'LogDebugMessage("Determined if Token " & {_Token}.GetValuesAndLocationsString() & " satisfies Pattern " & Me.ToString() & ". Result: " & _WasSatisfied.ToString(), LogEvent.DSEventCatagory.Lexing)
				Return _WasSatisfied
			End Function

			''' <summary>Returns in the form E.g. [Keyword, /Function/]</summary>
			Public Overrides Function ToString() As String
				REM [Keyword, /Function/]
				Return (Me.TokenType.ToString() & ", /" & Me.TokenValueRegExp & "/"c).InSquares()
			End Function

		End Structure

		''' <summary>Represents a List of [TokenPattern]s against which an Array of Tokens can be validated</summary>
		Public Class TokensToPatternsValidator

			Public ReadOnly Patterns As TokenPattern()

			Public Sub New(ByVal ParamArray _Patterns As TokenPattern())
				Me.Patterns = _Patterns
			End Sub

			''' <summary>Throws any Exceptions to provide verbose output if a Token Array does not satisfy the Patterns</summary>
			Public Sub EnsureIsSatisfiedBy(ByVal _Tokens As Runtime.Token(), Optional ByVal _MustBeAsManyTokensAsPatterns As Boolean = False)
				Try

					REM E.g.
					REM	{Keyword, /Function/}, {GrammarChar, /</}, {DataType, /.*/}, {GrammarChar, />/}, {Identifier, /.*/}, {GrammarChar, /(/}, {GrammarChar, /)/}
					REM		Function				<					String				>					GetName				(					)

					LogLexingMessage("Ensuring TokensToPatternsValidator " & Me.ToString() & " is satisfied by Token Array " & _Tokens.ReconstructIntoSource(" ").InSquares() & "...")

					If _MustBeAsManyTokensAsPatterns AndAlso (Not (_Tokens.Length = Me.Patterns.Length)) Then Throw New DSTokenPatternValidationException("The Tokens do not match the Patterns because the number of Tokens " & _Tokens.Length.ToString().InBrackets() & " is not equal to the number of Patterns " & Me.Patterns.Length.ToString().InBrackets() & " and _MustBeAsManyTokensAsPatterns was True so they had to be equal.", Me, _Tokens)

					REM There must be AT LEAST as many Tokens as there are Patterns; there could be more Tokens than patterns, but not the other way around.
					If Not (_Tokens.Length >= Me.Patterns.Length) Then Throw New DSTokenPatternValidationException("The Tokens do not match the Patterns because the number of Tokens " & _Tokens.Length.ToString().InBrackets() & " is less than the number of Patterns " & Me.Patterns.Length.ToString().InBrackets(), Me, _Tokens)

					REM Here, we know that there is one Token for each Pattern. We must now ensure that each Token satisfies its corosponding Pattern.
					For _PatternIndex% = 0 To (Me.Patterns.Length - 1) Step +1
						If Not Me.Patterns.ElementAt(_PatternIndex).IsSatisfiedBy(_Tokens.ElementAt(_PatternIndex)) Then Throw New DSTokenPatternValidationException("The Pattern " & Me.Patterns.ElementAt(_PatternIndex).ToString() & " is not satisfied by the Token at position " & _Tokens.ElementAt(_PatternIndex).LocationInSource.ToString() & " with value " & _Tokens.ElementAt(_PatternIndex).Value, Me, _Tokens)
					Next

					REM Here, we know that all Patterns were satisfied by a corosponding Token

					LogLexingMessage("...TokensToPatternsValidator was successfully satisfied by Tokens")

				Catch _Ex As Exception : Throw New DSException("@EnsureIsSatisfiedBy: " & _Ex.Message, _Ex) : End Try
			End Sub

			''' <summary>Returns True if the Tokens match the Patterns in the current TPV</summary>
			Public Function IsSatisfiedBy(ByVal _Tokens As Runtime.Token(), Optional ByVal _MustBeAsManyTokensAsPatterns As Boolean = False) As Boolean
				Try

					'LogLexingMessage("Determining if TokensToPatternsValidator " & Me.ToString() & " is satisfied by Token Array " & _Tokens.GetValuesAndLocationsString(), LogEvent.DSEventSeverity.Debug)

					If _MustBeAsManyTokensAsPatterns AndAlso (Not (_Tokens.Length = Me.Patterns.Length)) Then Return False

					REM There must be AT LEAST as many Tokens as there are Patterns; there could be more Tokens than patterns, but not the other way around.
					If Not (_Tokens.Length >= Me.Patterns.Length) Then Return False

					REM Here, we know that there is one Token for each Pattern. We must now ensure that each Token satisfies its corosponding Pattern.
					For _PatternIndex% = 0 To (Me.Patterns.Length - 1) Step +1
						If Not Me.Patterns.ElementAt(_PatternIndex).IsSatisfiedBy(_Tokens.ElementAt(_PatternIndex)) Then Return False
					Next

					Return True

				Catch _Ex As Exception : Throw New DSException("@IsSatisfiedBy: " & _Ex.Message, _Ex) : End Try
			End Function

			''' <summary>Returns True if the Token matches the Patterns in the current TPV</summary>
			Public Function IsSatisfiedBy(ByVal _Token As Runtime.Token) As Boolean
				Return Me.IsSatisfiedBy({_Token})
			End Function

			''' <summary>Returns the TokenValidators herein</summary>
			Public Overrides Function ToString() As String
				REM {[Keyword, /Function/], [GrammarChar, /</], [DataType, /.*/], [GrammarChar, />/], [GrammarChar, /(/], [GrammarChar, /)/]}
				Return String.Join(", ", Me.Patterns.Select(Of String)(Function(_TokenPattern As TokenPattern) _TokenPattern.ToString())).InCurlies()
			End Function

		End Class

		''' <summary>A Map of IInstruction to the Minimum No. Tokens required to construct one of those Instructions</summary>
		Public ReadOnly MinimumRequiredTokens As New Dictionary(Of Type, System.Byte)() From {
		   {GetType(FunctionCall), 4},
		   {GetType(ReturnToCaller), 2},
		   {GetType(VariableAssignment), 4},
		   {GetType(VariableDeclaration), 5},
		   {GetType(Statements.DSFunction), 10},
		   {GetType(Statements.IfStatement), 7},
		   {GetType(Statements.LoopStatement), 7},
		   {GetType(Statements.WhileStatement), 7}
		  }

		'FunctionCall			S() {LineEnd}												4
		'ReturnToCaller			Return {LineEnd}											2
		'VariableAssignment		A : B {LineEnd}												4
		'VariableDeclaration	<String> A {LineEnd}										5
		'
		'DSFunction				Function <Void> S () {LineEnd} EndFunction {LineEnd}		10
		'IfStatement			If (A) {LineEnd} EndIf {LineEnd}							7
		'LoopStatement			Loop (A) {LineEnd} EndLoop {LineEnd}						7
		'WhileStatement			While (A) {LineEnd} EndWhile {LineEnd}						7

		''' <summary>IInstruction Tokens to Patterns Validators (TPVs)</summary>
		Public Class BuiltInTPVs

			REM TPV RegExps don't have to be UPPERCASE, because they're ToUpper()'d before a comparrison occours in TokenPattern\IsSatisfiedBy()

#Region "Utility TPVs"

			''' <summary>Validates [{Cr,Lf}]</summary>
			Public Shared ReadOnly LineEnd_TPV As New TokensToPatternsValidator(
			New TokenPattern(Runtime.Token.TokenType.LineEnd, ".*")
			) 'Lots of things rely on this.Patterns.ElementAt(0) being the {LineEnd} Pattern

			''' <summary>Validates [&lt;]</summary>
			Public Shared ReadOnly OpeningDataTypeBracket_TPV As New TokensToPatternsValidator(
			New TokenPattern(Runtime.Token.TokenType.GrammarChar, "\"c & Language.Constants.OpeningDataTypeBracket)
			)

			''' <summary>Validates [:]</summary>
			Public Shared ReadOnly AssignmentOperator_TPV As New TokensToPatternsValidator(
			New TokenPattern(Runtime.Token.TokenType.DSOperator, Language.Constants.AssignmentOperator)
			)

			''' <summary>Validates [(]</summary>
			Public Shared ReadOnly OpeningFunctionBracket_TPV As New TokensToPatternsValidator(
			New TokenPattern(Runtime.Token.TokenType.GrammarChar, "\"c & Language.Constants.OpeningFunctionBracket)
			)

			''' <summary>Validates [)]</summary>
			Public Shared ReadOnly ClosingFunctionBracket_TPV As New TokensToPatternsValidator(
			New TokenPattern(Runtime.Token.TokenType.GrammarChar, "\"c & Language.Constants.ClosingFunctionBracket)
			)

			''' <summary>Validates [,]</summary>
			Public Shared ReadOnly FunctionArgumentSeperator_Comma_TPV As New TokensToPatternsValidator(
			New TokenPattern(Runtime.Token.TokenType.GrammarChar, "\"c & Language.Constants.FunctionArgumentSeperator)
			)

#Region "Statement TPVs"

			''' <summary>Validates [Function]</summary>
			Public Shared ReadOnly Keyword_Function_TPV As New TokensToPatternsValidator(
			New TokenPattern(Runtime.Token.TokenType.Keyword, Language.Constants.Keyword_Function)
			)

			''' <summary>Validates [EndFunction]</summary>
			Public Shared ReadOnly StatementEnd_Function_TPV As New TokensToPatternsValidator(
			New TokenPattern(Runtime.Token.TokenType.StatmentEnd, Language.Constants.StatementEnd_Function)
			)

			''' <summary>Validates [If]</summary>
			Public Shared ReadOnly Keyword_If_TPV As New TokensToPatternsValidator(
			New TokenPattern(Runtime.Token.TokenType.Keyword, Language.Constants.Keyword_If)
			)

			''' <summary>Validates [Else]</summary>
			Public Shared ReadOnly Keyword_Else_TPV As New TokensToPatternsValidator(
			New TokenPattern(Runtime.Token.TokenType.Keyword, Language.Constants.Keyword_Else)
			)

			''' <summary>Validates [EndIf]</summary>
			Public Shared ReadOnly StatementEnd_If_TPV As New TokensToPatternsValidator(
			New TokenPattern(Runtime.Token.TokenType.StatmentEnd, Language.Constants.StatementEnd_If)
			)

			''' <summary>Validates [Loop]</summary>
			Public Shared ReadOnly Keyword_Loop_TPV As New TokensToPatternsValidator(
			New TokenPattern(Runtime.Token.TokenType.Keyword, Language.Constants.Keyword_Loop)
			)

			''' <summary>Validates [EndLoop]</summary>
			Public Shared ReadOnly StatementEnd_Loop_TPV As New TokensToPatternsValidator(
			New TokenPattern(Runtime.Token.TokenType.StatmentEnd, Language.Constants.Keyword_Loop)
			)

			''' <summary>Validates [While]</summary>
			Public Shared ReadOnly Keyword_While_TPV As New TokensToPatternsValidator(
			New TokenPattern(Runtime.Token.TokenType.Keyword, Language.Constants.Keyword_While)
			)

			''' <summary>Validates [EndWhile]</summary>
			Public Shared ReadOnly StatementEnd_While_TPV As New TokensToPatternsValidator(
			New TokenPattern(Runtime.Token.TokenType.StatmentEnd, Language.Constants.StatementEnd_While)
			)

			''' <summary>Validates [Function], [If], [Loop], and [While]</summary>
			Public Shared ReadOnly StatementOpening_TPV As New TokensToPatternsValidator(
			New TokenPattern(Runtime.Token.TokenType.Keyword, "^"c & (Language.Constants.Keyword_Function.InBrackets() & "|"c & Language.Constants.Keyword_If.InBrackets() & "|"c & Language.Constants.Keyword_Loop.InBrackets() & "|"c & Language.Constants.Keyword_While.InBrackets()).InBrackets() & "$"c)
			)

			''' <summary>Validates [EndFunction], [EndIf], [EndLoop], and [EndWhile]</summary>
			Public Shared ReadOnly StatementClosing_TPV As New TokensToPatternsValidator(
			New TokenPattern(Runtime.Token.TokenType.StatmentEnd, "^"c & (Language.Constants.StatementEnd_Function.InBrackets() & "|"c & Language.Constants.StatementEnd_If.InBrackets() & "|"c & Language.Constants.StatementEnd_Loop.InBrackets() & "|"c & Language.Constants.StatementEnd_While.InBrackets()).InBrackets() & "$"c)
			)

			''' <summary>Validates [If], [Loop], and [While]</summary>
			Public Shared ReadOnly SubStatementOpening_TPV As New TokensToPatternsValidator(
			New TokenPattern(Runtime.Token.TokenType.Keyword, "^"c & (Language.Constants.Keyword_If.InBrackets() & "|"c & Language.Constants.Keyword_Loop.InBrackets() & "|"c & Language.Constants.Keyword_While.InBrackets()).InBrackets() & "$"c)
			)

			''' <summary>Validates [EndIf], [EndLoop], and [EndWhile]</summary>
			Public Shared ReadOnly SubStatementClosing_TPV As New TokensToPatternsValidator(
			New TokenPattern(Runtime.Token.TokenType.StatmentEnd, "^"c & (Language.Constants.StatementEnd_If.InBrackets() & "|"c & Language.Constants.StatementEnd_Loop.InBrackets() & "|"c & Language.Constants.StatementEnd_While.InBrackets()).InBrackets() & "$"c)
			)

			''' <summary>Validates [) {LineEnd}]</summary>
			Public Shared ReadOnly Statement_FirstLineEnding_TPV As New TokensToPatternsValidator(
			New TokenPattern(Runtime.Token.TokenType.GrammarChar, "\"c & Language.Constants.ClosingFunctionBracket)
			)

#End Region

#End Region

#Region "IInstruction TPVs"

			''' <summary>Validates E.g. [Function &lt;String&gt; GetName]</summary>
			Public Shared ReadOnly DSFunction_UpToIncIdentifier_TPV As New TokensToPatternsValidator(
			New TokenPattern(Runtime.Token.TokenType.Keyword, Language.Constants.Keyword_Function),
			New TokenPattern(Runtime.Token.TokenType.GrammarChar, "\"c & Language.Constants.OpeningDataTypeBracket),
			New TokenPattern(Runtime.Token.TokenType.DataType, Parser.GetRegExp_For_TokenType(Token.TokenType.DataType)),
			New TokenPattern(Runtime.Token.TokenType.GrammarChar, "\"c & Language.Constants.ClosingDataTypeBracket),
			New TokenPattern(Runtime.Token.TokenType.Identifier, Parser.GetRegExp_For_TokenType(Token.TokenType.Identifier))
			)

			''' <summary>Validates E.g. [Function &lt;String&gt; GetName () {LineEnd}]</summary>
			Public Shared ReadOnly Parameterless_DSFunction_TPV As New TokensToPatternsValidator(
			DSFunction_UpToIncIdentifier_TPV.Patterns.Concat(
			 {
			  New TokenPattern(Runtime.Token.TokenType.GrammarChar, "\"c & Language.Constants.OpeningFunctionBracket),
			  New TokenPattern(Runtime.Token.TokenType.GrammarChar, "\"c & Language.Constants.ClosingFunctionBracket),
			  LineEnd_TPV.Patterns.ElementAt(0)
			 }
			).ToArray()
			)

			''' <summary>Validates E.g. [&lt;String@&gt; Names]</summary>
			Public Shared ReadOnly Parameter_TPV As New TokensToPatternsValidator(
			New TokenPattern(Runtime.Token.TokenType.GrammarChar, "\"c & Language.Constants.OpeningDataTypeBracket),
			New TokenPattern(Runtime.Token.TokenType.DataType, Parser.GetRegExp_For_TokenType(Token.TokenType.DataType)),
			New TokenPattern(Runtime.Token.TokenType.GrammarChar, "\"c & Language.Constants.ClosingDataTypeBracket),
			New TokenPattern(Runtime.Token.TokenType.Identifier, Parser.GetRegExp_For_TokenType(Token.TokenType.Identifier))
			)

			''' <summary>Validates [If (]</summary>
			Public Shared ReadOnly IfStatement_UpToIncOpenBracket_TPV As New TokensToPatternsValidator(
			New TokenPattern(Runtime.Token.TokenType.Keyword, Language.Constants.Keyword_If),
			New TokenPattern(Runtime.Token.TokenType.GrammarChar, "\"c & Language.Constants.OpeningFunctionBracket)
			)

			''' <summary>Validates [While (]</summary>
			Public Shared ReadOnly WhileStatement_UpToIncOpenBracket_TPV As New TokensToPatternsValidator(
			New TokenPattern(Runtime.Token.TokenType.Keyword, Language.Constants.Keyword_While),
			New TokenPattern(Runtime.Token.TokenType.GrammarChar, "\"c & Language.Constants.OpeningFunctionBracket)
			)

			''' <summary>Validates [Loop (]</summary>
			Public Shared ReadOnly LoopStatement_UpToIncOpenBracket_TPV As New TokensToPatternsValidator(
			New TokenPattern(Runtime.Token.TokenType.Keyword, Language.Constants.Keyword_Loop),
			New TokenPattern(Runtime.Token.TokenType.GrammarChar, "\"c & Language.Constants.OpeningFunctionBracket)
			)

			''' <summary>Validates E.g. [&lt;String@&gt; NameTwo]</summary>
			Public Shared ReadOnly VariableDeclaration_UpToIncIdentifier_TPV As New TokensToPatternsValidator(
			New TokenPattern(Runtime.Token.TokenType.GrammarChar, "\"c & Language.Constants.OpeningDataTypeBracket),
			New TokenPattern(Runtime.Token.TokenType.DataType, Parser.GetRegExp_For_TokenType(Token.TokenType.DataType)),
			New TokenPattern(Runtime.Token.TokenType.GrammarChar, "\"c & Language.Constants.ClosingDataTypeBracket),
			New TokenPattern(Runtime.Token.TokenType.Identifier, Parser.GetRegExp_For_TokenType(Token.TokenType.Identifier))
			)

			''' <summary>Validates E.g. [GetName(]</summary>
			Public Shared ReadOnly FunctionCall_UpToIncOpenBracket_TPV As New TokensToPatternsValidator(
			   New TokenPattern(Runtime.Token.TokenType.Identifier, Parser.GetRegExp_For_TokenType(Token.TokenType.Identifier)),
			   New TokenPattern(Runtime.Token.TokenType.GrammarChar, "\"c & Language.Constants.OpeningFunctionBracket)
			)

			''' <summary>Validates [Return]</summary>
			Public Shared ReadOnly Keyword_Return_TPV As New TokensToPatternsValidator(
			   New TokenPattern(Token.TokenType.Keyword, Language.Constants.Keyword_Return)
			)

			''' <summary>Validates E.g. [Name :]</summary>
			Public Shared ReadOnly VariableAssignmet_TPV As New TokensToPatternsValidator(
			New TokenPattern(Runtime.Token.TokenType.Identifier, Parser.GetRegExp_For_TokenType(Token.TokenType.Identifier)),
			AssignmentOperator_TPV.Patterns.ElementAt(0)
			)

#End Region

		End Class

	End Module

End Namespace