Namespace Runtime

	''' <summary>Contains Constants and Methods for the first stage in the DocScript Interpretation Process</summary>
	Public Module Parser

#Region "Parser Constants"

		Private Const WordChar_RegExp_$ = "^[0-9a-zA-Z_\$@\.]$"	'$ Needed for SLITs e.g. $SLIT_0$; @ Needed for Array Types e.g. <String@>; . Needed for some Numeric Literals e.g. 1255.2292
		Private Const SLIT_PlaceholderPattern_$ = "$SLIT_{Number}$"	'{Number} is later String.Replace()'d
		Private Const SLIT_RegExp_$ = "\$SLIT_\d{1,5}\$"
		Private Const StringLiteral_RegExp_$ = """[^""]*"""
		Private Const LineEnd_TokenTypeValue_$ = Language.Constants.LineBreak

		Private Const ValidIdentifierChars_$ = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz_"
		Private Const ValidNumericChars_$ = "0123456789." 'This isn't refering to NumericLiterals
		Private Const ValidOperatorChars_$ = ":=&¬'|¦+-*/^%~"
		Private Const ValidGrammarChars_$ = "()[]<>,"
		Private Const ValidSLITStageTokenChars_$ = (ValidIdentifierChars_ & ValidOperatorChars_ & ValidGrammarChars_ & ValidNumericChars_ & LineEnd_TokenTypeValue_$ & "$@")

		'↓ These are checked-for AFTER SLIT-Insertion; DS String Literals CAN contain these chars
		Private ReadOnly ProblematicChars_ForAutomaticRemoval_$ = {
		 0, 160, 173, 847, 8192, 8193, 8194, 8195, 8196, 8197, 8198, 8199, 8200, 8201, 8202, 8203, 8204, 8205, 8206, 8207, 8239, 8287, 8288, 8289, 8290, 8291, 8292, 65279
		}.Select(Of [Char])(AddressOf ChrW).ToArray()

		REM A Token should be ToUpper()ed before being checked against these
		Private ReadOnly TokenRegExpToTokenType_Table_ As New Dictionary(Of String, Token.TokenType)() From {
		 {"^(""[^""]*"")$", Token.TokenType.StringLiteral},
		 {"^((\d{1,10}(\.\d{1,4})?)|[A-Za-z0-9]{1,10}_\d{1,3})$", Token.TokenType.NumericLiteral},
		 {"^((TRUE)|(FALSE))$", Token.TokenType.BooleanLiteral},
		 {"^((IF)|(ELSE)|(WHILE)|(LOOP)|(RETURN)|(FUNCTION))$", Token.TokenType.Keyword},
		 {"^(END((IF)|(WHILE)|(LOOP)|(FUNCTION)))$", Token.TokenType.StatmentEnd},
		 {"^(((STRING)|(NUMBER)|(BOOLEAN))@?|(VOID))$", Token.TokenType.DataType},
		 {"^(_?[A-Z]+[A-Z_]*)$", Token.TokenType.Identifier},
		 {"^(:|=|&|¬|'|\||¦|\+|\-|\*|/|\^|%|~)$", Token.TokenType.DSOperator},
		 {"^(\(|\)|\[|\]|<|>|\,)$", Token.TokenType.GrammarChar},
		 {"^(\r\n)$", Token.TokenType.LineEnd}
		}

		Public Function GetRegExp_For_TokenType(ByVal _TokenType As Token.TokenType) As [String]
			Try : If Not TokenRegExpToTokenType_Table_.ContainsValue(_TokenType) Then Throw New DSValidationException("The TokenType did not exist in the TokenTypeToRegExp Table", _TokenType.ToString())
				Return TokenRegExpToTokenType_Table_.Where(Function(_KVP As KeyValuePair(Of String, Token.TokenType)) _KVP.Value = _TokenType).First().Key
			Catch _Ex As Exception : Throw New DSException("@GetRegExp_For_TokenType: " & _Ex.Message, _Ex) : End Try
		End Function

#End Region

		''' <summary>Performs the Segmentation and TokenTypeIdentification to generate Tokens from some raw DocScript Source</summary>
		Public Function GetTokensFromSource(ByVal _RawSource$) As DocScript.Runtime.Token()
			Try

				LogParsingMessage("Began Parsing...", LogEvent.DSEventSeverity.Infomation)

				REM ╔═════════════════════════════════════════════╗
				REM ║          DocScript Parsing Process          ║
				REM ╚═════════════════════════════════════════════╝

				REM 1) Initialisation
				'		- Ensure all LineBreaks are valid (CrLf)
				'		- Load in Lines of Source

				REM 2) Segmentation
				'		- Blank out any #Comment or Whitespace Lines
				'		- Ensure nothing already exists in the Source which matches the SLIT RegExp
				'		- Substitute StringLiterals with SLITs e.g. $SLIT_0$
				'		- Generate Segmented Tokens:
				'			- (For Each Line, and For Each Character thereof, evaluate if it's a WordChar or SplitAtChar...)
				'		- Remove any Null Tokens (Whitespace, etc...) (...Except from LineEnd Tokens)
				'		- Check for, and automatically remove, any remaining problematic characters (E.g. zero-width no-break spaces)
				'		- Ensure all remaining characters are valid (E.g. No SpeechMarks) (Best to do this now as we have the [TokenLocation]s)
				'		- Replace any SLITs with their original StringLiterals

				REM 3) Classification
				'		- For Each Token, attempt to match it to a RegExp for its TokenType
				'		- Ensure all Bracket usage is balenced (Best to do this now as we have the TokenTypes for easy GrammarChar filtering)
				'		- Ensure all Statement Openings and Closings are balenced (Function to EndFunction, etc...)

				REM		[_RawSourceLines] → [_CleanSourceLines]		→	 [_SegmentedTokens] → [_NonNullTokens] → [_CleansedTokens] → [_TokensWithStringLiterals] → [_ClassifiedTokens]

				If Not AllLineBreaksAreValid_(_RawSource) Then Throw New DSValidationException("At least one LineBreak in the Source was invalid. All LineBreaks in a DocScript Program must be [CrLf].", "(The Unprocessed Source)")

				'The unprocessed Lines from the Source:
				Dim _RawSourceLines As String() = _RawSource.Split({vbCrLf}, StringSplitOptions.None).ToArray()
				LogParsingMessage("Loaded in " & _RawSourceLines.Length.ToString() & " Line(s) from Source")

				Dim _CleanSourceLines As String() = Parser.BlankOutUnnecessarySourceLines_(_RawSourceLines)

				REM At this point, Any [Whitespace Lines] or [#Comment Lines] have been substituted with Empty Lines.
				REM This is needed so that we can still accurately report the Line and Column Location of the Tokens.
				REM Next, StringLiterals will be replaced with StringLiteral Indication Tokens (SLITs)

				Dim _SLITMatches As Text.RegularExpressions.MatchCollection = (New System.Text.RegularExpressions.Regex(Parser.SLIT_RegExp_)).Matches(String.Join(vbCrLf, _CleanSourceLines))
				LogParsingMessage("Detected " & _SLITMatches.Count.ToString() & " prematurely-present SLIT(s) in Source")
				If _SLITMatches.Count > 0 Then Throw New DSValidationException("At least one instance of a SLIT Placeholder appeared in the Source before any had been inserted by the Parser. This is not permitted.", _SLITMatches.Item(0).Value)

				'This Function Call takes in the Lines ByRef (and modifies them) and returns the SLIT Table
				Dim _ExtractedStringLiterals As String() = Parser.ReplaceStringLiteralsWithSLITs_(_CleanSourceLines)

				REM Generate the Segmentation Tokens
				'This SegmentCleanSourceIntoTokens() call needs access to the SLIT Table, so that it can accurately fill in the Column for the TokenPositions. Without this, it would do all horozontal positions relative to the length of a SLIT, instead of the string *represented by* the SLIT.
				Dim _SegmentedTokens As Runtime.Token() = Parser.SegmentCleanSourceIntoTokens_(_CleanSourceLines, _ExtractedStringLiterals)

				REM Remove Null Tokens
				Dim _NonNullTokens As List(Of Token) = _
				(From _Token As Token In _SegmentedTokens Where ((_Token.Type = Token.TokenType.LineEnd) OrElse (Not _Token.Value.WithLeadingWhiteSpaceRemoved().IsEmpty())) Select _Token).ToList()
				LogParsingMessage("Ignored " & (_SegmentedTokens.Count - _NonNullTokens.Count).ToString() & " Null Token(s)")

				REM Check for, and automatically remove, any remaining problematic characters. Then, ensure all remaining chars are valid.
				Dim _CleansedTokens As List(Of Token) = Parser.RemoveProblematicChars_(_NonNullTokens)

				REM Replace any SLITs with their origional Values
				Dim _TokensWithStringLiterals As List(Of Token) = Parser.ReplaceSLITsWithStringLiterals_(_CleansedTokens, _ExtractedStringLiterals)

				REM Now that the Segmentation has occoured, we have a series of Tokens with Unresolved TokenTypes.
				REM Next, we must therefore Resolve the TokenTypes...
				Dim _ClassifiedTokens As Token() = Parser.GetTypedTokensFromUnclassifiedOnes_(_TokensWithStringLiterals)

				REM Ensure that brackets and statements are well-balenced in their opening- and closing-components
				Parser.EnsurePairedComponentsAreWellBalenced_(_ClassifiedTokens)

				_ClassifiedTokens.ToList().ForEach(Sub(_Token As Runtime.Token) LogDebugMessage("Parser Final Classified Token: " & _Token.ToString(), LogEvent.DSEventCatagory.Parsing))
				LogParsingMessage("...Finished Parsing; Returning " & _ClassifiedTokens.Count.ToString() & " Classified Token(s)", LogEvent.DSEventSeverity.Infomation)

				Return _ClassifiedTokens

			Catch _Ex As Exception : Throw New DSException("@GetTokensFromSource: " & _Ex.Message, _Ex) : End Try
		End Function

#Region "(Private) Parser Internal Utility Methods"

		''' <summary>This function attempts to proove that only Proper CrLf LineBreaks have been used</summary>
		Private ReadOnly Property AllLineBreaksAreValid_(ByVal _RawSource$) As Boolean
			Get
				Try

					REM This function attempts to proove that only Proper CrLf LineBreaks have been used
					REM Therefore, we will check that the number of [Cr occourances] is equal to the number of [Lf occourances]

					Dim _NumberOfCarridgeReturns As Int32 = _RawSource.ToCharArray().Where(Function(_Char As Char) _Char = Chr(13)).Count()
					Dim _NumberOfLineFeeds As Int32 = _RawSource.ToCharArray().Where(Function(_Char As Char) _Char = Chr(10)).Count()

					LogParsingMessage(String.Format("Checking LineBreaks validity: Detected {0} CarridgeReturn(s) and {1} LineFeed(s). Result: {2}", _NumberOfCarridgeReturns, _NumberOfLineFeeds, IIf(_NumberOfCarridgeReturns = _NumberOfLineFeeds, "Valid", "Invalid")))

					Return (_NumberOfCarridgeReturns = _NumberOfLineFeeds)

				Catch _Ex As Exception : Throw New DSException("@AllLineBreaksAreValid_: " & _Ex.Message, _Ex) : End Try
			End Get
		End Property

		''' <summary>Replaces [#Comment Lines] and [Whitespace-only Lines] with [Empty Strings]</summary>
		Private Function BlankOutUnnecessarySourceLines_(ByRef _RawSourceLines$()) As String()
			Try

				Dim _ProcessedSourceLinesToReturn As New List(Of String)()

				Dim _NumberOfCommentLines As UInt16 = 0
				Dim _NumberOfWhitespaceOrEmptyLines As UInt16 = 0

				For Each _RawLine$ In _RawSourceLines

					'The _ProcessedLine will be added to the _ProcessedSourceLinesToReturn
					Dim _ProcessedLine$ = _RawLine

					REM Check if the Line is Empty, or just Whitespace; Instead of the (potentially) Whitespace, substitute in an Empty Line
					If _ProcessedLine.WithLeadingWhiteSpaceRemoved().WithTrailingWhiteSpaceRemoved().IsEmpty() Then
						_NumberOfWhitespaceOrEmptyLines += 1US : _ProcessedSourceLinesToReturn.Add(String.Empty) : Continue For
					End If

					REM Check if the Line is a #Comment; Instead of the Comment, substitute in an Empty Line
					If _ProcessedLine.WithLeadingWhiteSpaceRemoved().StartsWith(DocScript.Language.Constants.CommentCharacter.ToString()) Then
						_NumberOfCommentLines += 1US : _ProcessedSourceLinesToReturn.Add(String.Empty) : Continue For
					End If

					'If we're here, then the Line wasn't empty, and it wasn't a comment; just keep it as it is
					_ProcessedSourceLinesToReturn.Add(_ProcessedLine)
					LogDebugMessage("Processed (blanked-out) Line: [" & _ProcessedLine & "]"c, LogEvent.DSEventCatagory.Parsing)

				Next

				LogParsingMessage("Ignored " & _NumberOfCommentLines.ToString() & " Comment Line(s)")
				LogParsingMessage("Ignored " & _NumberOfWhitespaceOrEmptyLines.ToString() & " Empty or Whitespace-only Line(s)")

				Return _ProcessedSourceLinesToReturn.ToArray()

			Catch _Ex As Exception : Throw New DSException("@BlankOutUnnecessarySourceLines_: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>Replaces any StringLiterals with SLITs. The Origional String Literals are stored in the Array returned by this Method</summary>
		''' <returns>Returns the SLITs Array</returns>
		Private Function ReplaceStringLiteralsWithSLITs_(ByRef _SourceLines As String()) As String()
			Try

				Dim _ExtractedStringLiteralsToReturn As New List(Of String)()
				Dim _NumberOfSLITsInserted As UInt16 = 0

				Dim _StringLiteralMatchEvaluator As New System.Text.RegularExpressions.MatchEvaluator(
				 Function(_Match As System.Text.RegularExpressions.Match) As String

					 REM The _Match looks like:				"Origional String Literal"
					 REM That needs to be replaced with:	$SLIT_0$

					 _NumberOfSLITsInserted += 1US
					 _ExtractedStringLiteralsToReturn.Add(_Match.Value)	'The SLIT No. is its index in the [_ExtractedStringLiterals] List

					 LogDebugMessage("Detected and replaced a StringLiteral with a SLIT. StringLiteral = [" & _Match.Value & "], SLIT No. = [" & (_ExtractedStringLiteralsToReturn.Count - 1).ToString() & "]"c, LogEvent.DSEventCatagory.Parsing)
					 Return [Parser].SLIT_PlaceholderPattern_.Replace("{Number}", (_ExtractedStringLiteralsToReturn.Count - 1).ToString())

				 End Function
				)

				For _SourceLineIndex As UInt16 = 0 To CUShort(_SourceLines.Count - 1) Step +1
					_SourceLines(_SourceLineIndex) = _
					  System.Text.RegularExpressions.Regex.Replace( _
					   input:=_SourceLines(_SourceLineIndex),
					   pattern:=Parser.StringLiteral_RegExp_,
					   evaluator:=_StringLiteralMatchEvaluator,
					   options:=System.Text.RegularExpressions.RegexOptions.None
					 )
				Next

				LogParsingMessage("Replaced " & _NumberOfSLITsInserted.ToString() & " String Literal(s) with SLITs")

				Return _ExtractedStringLiteralsToReturn.ToArray()

			Catch _Ex As Exception : Throw New DSException("@ReplaceStringLiteralsWithSLITs_: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>Splits the _CleanSourceLines up into Tokens, each of which leave this Function with an "Unresolved" TokenType</summary>
		Private Function SegmentCleanSourceIntoTokens_(ByVal _CleanSourceLines$(), ByVal _ExtractedStringLiterals As String()) As Token()
			Try

				REM Example Input
				'Function <Number> Main (<String> CLAs)
				'	Output("Hello, World!")
				'	Return 0
				'End-Function

				REM In order to accurately report the Token Locations, we must calcuate the Column reletave to the length of the strings represented by the SLITs, instead of the SLITs themselves
				Dim _WordLengthEvaluator As Func(Of [String], UInt16) = _
				   Function(_Word$) As UInt16
					   Try
						   'If the _Word is a SLIT, extract the {Number} from the SLIT, and return SLIT_Table.Item({Number}).Length
						   'Otherwise, just return the _Word.Length

						   If _Word.MatchesRegEx(Parser.SLIT_RegExp_) Then
							   Return Convert.ToUInt16(_ExtractedStringLiterals(Convert.ToInt32(_Word.GetDigitChars())).Length)
						   Else
							   Return Convert.ToUInt16(_Word.Length)
						   End If

					   Catch _Ex As Exception : Throw New DSException("@_WordLengthEvaluator: " & _Ex.Message, _Ex) : End Try
				   End Function

				Dim _DerivedTokensToReturn As New List(Of Token)()
				Dim _WordAccumulator$ = String.Empty

				For _LineNumber As UInt16 = 1 To CUShort(_CleanSourceLines.Length) Step +1

					REM Every time after a Token is added, this is incremented by the length of that token. It is needed because the length of a SLIT is different to the length of the string represented by the SLIT.
					Dim _PreviousToken_FinishedOnColumn As UInt16 = 1US

					For _ColumnNumber As UInt16 = 1 To CUShort(_CleanSourceLines(_LineNumber - 1).Length) Step +1

						Dim _CurrentCharacter As Char = _CleanSourceLines(_LineNumber - 1).ToCharArray()(_ColumnNumber - 1)

						REM If the Char is a WordChar, add it to the _WordAccumulator$
						REM		If that was the last Char of a Line, empty the _WordAccumulator into a [New Token()] and add a [LineEnd Token]
						REM Otherwise:
						REM		If the _WordAccumulator contains some text, empty it and add it's contents as a [New Token()]
						REM		Then just add the Char as a New Token()

						If _CurrentCharacter.ToString().MatchesRegEx(Parser.WordChar_RegExp_) Then

							_WordAccumulator &= _CurrentCharacter

							'We need to Split at the end of each line too, in case the last Char of the line isn't a non-WordChar. Otherwise, a line ending in a WordChar followed by a Line starting with a WordChar will cause no Split to occour. E.g. You'd get "ReturnEnd" From a "Return" followed by an "End-Function" on the next line.
							If (_ColumnNumber = (_CleanSourceLines(_LineNumber - 1).Length)) Then

								_DerivedTokensToReturn.Add(New Token(_WordAccumulator, Token.TokenType.Unresolved, _LineNumber, _PreviousToken_FinishedOnColumn))
								_PreviousToken_FinishedOnColumn += _WordLengthEvaluator.Invoke(_WordAccumulator)
								_WordAccumulator = String.Empty


								'We also need to add a LineEnd Token
								_DerivedTokensToReturn.Add(New Token(Parser.LineEnd_TokenTypeValue_$, Token.TokenType.LineEnd, _LineNumber, _PreviousToken_FinishedOnColumn))
								_PreviousToken_FinishedOnColumn += 1US 'Don't actually need this line because nothing should come after the LineEnd

							End If

						Else
							'We've just hit a non-WordChar; Empty the collected Word out as a Token...
							If Not _WordAccumulator.IsEmpty() Then
								_DerivedTokensToReturn.Add(New Token(_WordAccumulator, Token.TokenType.Unresolved, _LineNumber, _PreviousToken_FinishedOnColumn))
								_PreviousToken_FinishedOnColumn += _WordLengthEvaluator.Invoke(_WordAccumulator)
								_WordAccumulator = String.Empty
							End If

							'...And Add() the non-WordChar as a Token of it's own too:
							_DerivedTokensToReturn.Add(New Token(_CurrentCharacter.ToString(), Token.TokenType.Unresolved, _LineNumber, _PreviousToken_FinishedOnColumn))
							_PreviousToken_FinishedOnColumn += 1US 'Because the _CurrentCharacter is always 1 in length

							'We may also need to add a LineEnd Token
							If (_ColumnNumber = (_CleanSourceLines(_LineNumber - 1).Length)) Then
								_DerivedTokensToReturn.Add(New Token(Parser.LineEnd_TokenTypeValue_$, Token.TokenType.LineEnd, _LineNumber, _PreviousToken_FinishedOnColumn))
								_PreviousToken_FinishedOnColumn += 1US 'Don't actually need this line because nothing should come after the LineEnd
							End If

						End If

					Next
				Next

				LogParsingMessage("Segmented Clean Source into " & _DerivedTokensToReturn.Count & " Token(s)")
				Return _DerivedTokensToReturn.ToArray()

			Catch _Ex As Exception : Throw New DSException("@SegmentCleanSourceIntoTokens_: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>
		''' Takes in Tokens which may Problematic chars in their values, and removes these chars.
		''' Then, removes any tokens which are part of newly-visable #Comment lines (which were previously undetected, because of a leading problematic-char).
		''' Then, ensures that all remaining characters in all Tokens' Values are valid.
		''' </summary>
		Private Function RemoveProblematicChars_(ByRef _TokensWithSLITs As List(Of Token)) As List(Of Token)
			Try

				REM Check for, and automatically remove, any remaining problematic characters
				LogParsingMessage("Checking if there are any problematic characters, which can be automatically removed")
				Dim _InerrantTokens As List(Of Token) = _TokensWithSLITs.Select(Of Token)(
				 Function(_Token As Token) As Token

					 'The character must be removed, and not just replaced with a space. This is because spaces are already entirely gone at this point; they were used for the segmentation, but no Tokens have spaces in their values at this point.
					 Dim _CleansedTokenValue$ = String.Join("",
					 _Token.Value.ToCharArray().Select(Of [String])(AddressOf Char.ToString).ToArray().ReplaceWhere(AddressOf Parser.ProblematicChars_ForAutomaticRemoval_.Contains, "")
					 )

					 'If there were some replaced chars, then this deserves attention
					 If (_CleansedTokenValue <> _Token.Value) Then LogParsingMessage("The DS-Source contained a problematic (illegal) character within the token at location " & _Token.LocationInSource.ToString() & ". The Parser has automatically removed this character.", LogEvent.DSEventSeverity.Warning)

					 'Change only the .Value
					 Return New Token(
					   _Value:=_CleansedTokenValue$,
					   _Type:=_Token.Type, _LocationLine:=_Token.LocationInSource.Line, _LocationColumn:=_Token.LocationInSource.Column
					 )

				 End Function
				).ToList()

				REM At this point, there could be Tokens within _InerrantTokens, which have a .[Value] of an empty string.
				REM This is because the Value would have comprised EXCLUSIVELY ProblematicChars, which were all removed.
				REM Therefore, we should remove any Tokens whose values are "".
				_InerrantTokens = _
				  (From [_Token] As Token In _InerrantTokens Where ((_Token.Type = Token.TokenType.LineEnd) OrElse (Not _Token.Value.WithLeadingWhiteSpaceRemoved().IsEmpty())) Select [_Token]).ToList()

				REM At this point, a problematic char could just have been removed from the start of a #Comment line.
				REM This would now expose the #Comment line, and the # (0x23) would be detected as an illegal char in the following Parser stages.
				REM Therefore, we need to remove any token-sequences starting with a [{LineEnd}][#], up to the next [{LineEnd}].
				REM The reason there must be a [{LineEnd}] at the start, is because we would otherwise also be removing comments that start mid-way through a line, which isn't valid in DocScript.
				REM For this reason ↑, we also need to add a [{LineEnd}] onto the start, in case the first char is a [#].
				_InerrantTokens = {New Token(Parser.LineEnd_TokenTypeValue_, Token.TokenType.LineEnd, 0, 0)}.Concat(_InerrantTokens).ToList()
				Dim _NonCommentLineTokens As New List(Of Token)()
				For _TokenIndex% = 0 To (_InerrantTokens.Count - 1) Step +1

					'If we're currently looking at a [#]...
					If _InerrantTokens.ElementAt(_TokenIndex).Value.StartsWith(Language.Constants.CommentCharacter) AndAlso (_InerrantTokens.ElementAt(_TokenIndex - 1).Type = Token.TokenType.LineEnd) Then

						'...Then find the Index of the very next [{LineEnd}] after the [#]...
						Dim _IndexOf_NextLineEnd As Int32 = _TokenIndex + _InerrantTokens.Skip(_TokenIndex).ToArray().IndexOfFirstWhere(Function(_Token As Token) (_Token.Type = Token.TokenType.LineEnd) AndAlso (_Token.Value = Parser.LineEnd_TokenTypeValue_))
						'(The LineEnds do have their TokenTypes at this point, because of SegmentCleanSourceIntoTokens_()) ↑
						'(We have to Skip() _TokenIndex, so that we only find {LineEnd}s AFTER the [#]...) ↑
						'(...But we have to add it back on, because otherwise, we'd have an incorrect index relative to the whole _InerrantTokens array) ↑

						'...And skip reading the tokens herebetween
						_TokenIndex = _IndexOf_NextLineEnd

					Else

						'Here, we're not looking at a [#]; treat the Token as a lexically-significant one
						_NonCommentLineTokens.Add(_InerrantTokens.ElementAt(_TokenIndex))

					End If

				Next

				REM Now remove the [{LineEnd}] from earlier
				If Not _NonCommentLineTokens.First().Type = Token.TokenType.LineEnd Then Throw New DSException("The first non-comment-line Token did not have a value of {LineEnd}. It was: " & _NonCommentLineTokens.First().ToString())
				_NonCommentLineTokens = _NonCommentLineTokens.Skip(1).ToList()

				REM Ensure all remaining characters - accross ALL Tokens' Values - are valid...
				LogParsingMessage("Checking that all Characters in the post-SLIT-stage non-comment-line Tokens are valid")
				For Each _Token As Token In _NonCommentLineTokens
					If Not _Token.Value.ToCharArray().All(Function(_Char As Char) Parser.ValidSLITStageTokenChars_.ToCharArray().Contains(_Char)) Then _
					 Throw New DSValidationException("An Invalid Character was found in the Source within a Token located at " & _Token.LocationInSource.ToString() & ". Character codes: " & _Token.Value.ToCharArray().Select(Of Int32)(AddressOf AscW).ToArray().GetStandardArraySerialisation() & vbCrLf & "Note that some invalid characters are invisible such as a zero width no-break space. Use a UniCode Loopup website to determine what the errant character is, from the character code.", _Token.Value)
				Next

				Return _NonCommentLineTokens

			Catch _Ex As Exception : Throw New DSException("@RemoveProblematicChars_: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>Takes in some Tokens which may have SLITs in them, and returns a version of that List(Of Token) which has the origional StringLiterals again</summary>
		Private Function ReplaceSLITsWithStringLiterals_(ByRef _TokensWithSLITs As List(Of Token), ByVal _PreviouslyExtractedStringLiterals As String()) As List(Of Token)
			Try

				LogParsingMessage("Began Resubstituting SLITs with StringLiterals...")

				Dim _TokensWithStringLiteralsToReturn As New List(Of Runtime.Token)()
				Dim _NumberOfSLITsRemoved As UInt16 = 0

				Dim _SLITMatchEvaluator As New System.Text.RegularExpressions.MatchEvaluator(
				 Function(_Match As System.Text.RegularExpressions.Match) As String

					 REM The _Match looks like:				$SLIT_0$
					 REM That needs to be replaced with:	"Origional String Literal"

					 _NumberOfSLITsRemoved += 1US
					 Dim _SLITNumber As UInt32 = Convert.ToUInt32(_Match.Value.GetDigitChars())

					 LogDebugMessage("Detected and replaced a SLIT with a StringLiteral. SLIT No. = [" & _SLITNumber.ToString() & "], StringLiteral = [" & _PreviouslyExtractedStringLiterals(CInt(_SLITNumber)) & "]", LogEvent.DSEventCatagory.Parsing)
					 Return _PreviouslyExtractedStringLiterals(CInt(_SLITNumber))

				 End Function
				)

				For Each _TokenWithSLIT As Token In _TokensWithSLITs
					_TokensWithStringLiteralsToReturn.Add( _
					 New Token(_value:= _
					  System.Text.RegularExpressions.Regex.Replace( _
					   input:=_TokenWithSLIT.Value,
					   pattern:=Parser.SLIT_RegExp_,
					   evaluator:=_SLITMatchEvaluator,
					   options:=System.Text.RegularExpressions.RegexOptions.None
					  ),
					  _Type:=Token.TokenType.Unresolved,
					  _LocationLine:=_TokenWithSLIT.LocationInSource.Line,
					  _LocationColumn:=_TokenWithSLIT.LocationInSource.Column
					 )
					)
				Next

				REM On leaving that Loop, the number of [_TokensWithSLITs] and [_TokensWithStringLiteralsToReturn] should be equal
				LogParsingMessage(String.Format("Ensuring No. SLIT-containing Token(s) ({0}) is equal to No. StringLiteral-containing one(s) ({1}). Result: {2}", _TokensWithSLITs.Count.ToString(), _TokensWithStringLiteralsToReturn.Count.ToString(), IIf(_TokensWithSLITs.Count = _TokensWithStringLiteralsToReturn.Count, "True (Valid)", "False (Invalid)")), LogEvent.DSEventSeverity.Debug)
				If Not (_TokensWithSLITs.Count = _TokensWithStringLiteralsToReturn.Count) Then Throw New DSValidationException("The number of [SLIT-containing Tokens] was not equal to the number of [StringLiteral-containing Tokens] after the StringLiteral-Insertion Loop. This means that not all String Literals were successfully resubstituted.", "[_TokensWithStringLiteralsToReturn]")

				LogParsingMessage("...Replaced " & _NumberOfSLITsRemoved.ToString() & " SLIT(s) with StringLiteral(s)")
				Return _TokensWithStringLiteralsToReturn

			Catch _Ex As Exception : Throw New DSException("@ReplaceSLITsWithStringLiterals_: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>Takes in the Tokens where [Type = Unresolved] and classifies the Tokens with the RegExpToTokenType_Table</summary>
		Private Function GetTypedTokensFromUnclassifiedOnes_(ByRef _UnclassifiedTokens As List(Of Token)) As Token()
			Try

				LogParsingMessage("Began Classification stage of Parsing...")

				Dim _TypedTokensToReturn As New List(Of Token)()

				For Each _UnclassifiedToken As Token In _UnclassifiedTokens

					Dim _CurrentToken_HasBeenClassified As Boolean = False

					For Each _RegExpToTokenType As KeyValuePair(Of String, Token.TokenType) In Parser.TokenRegExpToTokenType_Table_

						If _UnclassifiedToken.Value.ToUpper().MatchesRegEx(_RegExpToTokenType.Key) Then
							_TypedTokensToReturn.Add(New Token(_UnclassifiedToken.Value, _RegExpToTokenType.Value, _UnclassifiedToken.LocationInSource.Line, _UnclassifiedToken.LocationInSource.Column))
							_CurrentToken_HasBeenClassified = True
							Exit For
						End If

					Next

					REM If we're here, then we've just exited the RegExp-Matching Loop
					REM This could be because:
					REM		1) We hit the [Exit For] when a Match Occoured, or:
					REM		2) We treid all the RegExps, and none of them fit
					REM	We can determine which eventuality is the case by checking the _CurrentToken_HasBeenClassified Variable

					If Not _CurrentToken_HasBeenClassified Then Throw New DSValidationException("A Token could not be classified with a TokenType. Its Location in the Source is " & _UnclassifiedToken.LocationInSource.ToString(), _UnclassifiedToken.Value)

				Next

				REM On leaving that Loop, we should now have the same number of tokens in [_TypedTokensToReturn] as in [_UnclassifiedTokens].
				REM Ensure that this is indeed the case

				LogParsingMessage(String.Format("...Ensuring No. Unclassified Token(s) ({0}) is equal to No. Classified one(s) ({1}). Result: {2}", _UnclassifiedTokens.Count.ToString(), _TypedTokensToReturn.Count.ToString(), IIf(_TypedTokensToReturn.Count = _UnclassifiedTokens.Count, "True (Valid)", "False (Invalid)")), LogEvent.DSEventSeverity.Debug)
				If Not (_TypedTokensToReturn.Count = _UnclassifiedTokens.Count) Then Throw New DSValidationException("The number of [Type-Assigned Tokens] was not equal to the number of [Unclassified Tokens] after the Token Classification Loop. This means that not all input Tokens were successfully classified.", "[_TypedTokensToReturn]")

				Return _TypedTokensToReturn.ToArray()

			Catch _Ex As Exception : Throw New DSException("@GetTypedTokensFromUnclassifiedOnes_: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>Ensures that all brackets () [] and &lt;&gt;, and statements (Function, If, Etc...) and are well-balenced</summary>
		Private Sub EnsurePairedComponentsAreWellBalenced_(ByRef _ClassifiedTokens As Token())
			Try

				REM Ensure that all brackets () [] and <> are balenced legally-ordered
				LogLexingMessage("Ensuring that all Brackets Opened, are Closed in the correct order")
				If Not DocScript.CompilerExtentions.UsefulMethods.ContainsWellBalencedPairs(Of Char)(
				 (From _Token As Runtime.Token In _ClassifiedTokens Select _Token.Value.ToCharArray().First()).ToArray(),
				 New Tuple(Of Char, Char)(Language.Constants.OpeningFunctionBracket, Language.Constants.ClosingFunctionBracket),
				 New Tuple(Of Char, Char)(Language.Constants.OpeningExpressionBracket, Language.Constants.ClosingExpressionBracket),
				 New Tuple(Of Char, Char)(Language.Constants.OpeningDataTypeBracket, Language.Constants.ClosingDataTypeBracket)
				) Then Throw New DSValidationException("The Brackets were not balenced and legally-ordered in the Classified Tokens", "_ClassifiedTokens")

				REM Ensure all StatementOpenings (Function, If, Etc...) and StatementClosings (EndFunction, EndIf, Etc...) are balenced and legally-ordered
				LogLexingMessage("Ensuring that all Statements Opened, are Closed in the correct order")
				If Not DocScript.CompilerExtentions.UsefulMethods.ContainsWellBalencedPairs(Of String)(
				 (From _Token As Runtime.Token In _ClassifiedTokens Where (BuiltInTPVs.StatementOpening_TPV.IsSatisfiedBy(_Token) OrElse BuiltInTPVs.StatementClosing_TPV.IsSatisfiedBy(_Token)) Select _Token.Value.ToUpper()).ToArray(),
				 New Tuple(Of String, String)(Language.Constants.Keyword_Function, Language.Constants.StatementEnd_Function),
				 New Tuple(Of String, String)(Language.Constants.Keyword_If, Language.Constants.StatementEnd_If),
				 New Tuple(Of String, String)(Language.Constants.Keyword_Loop, Language.Constants.StatementEnd_Loop),
				 New Tuple(Of String, String)(Language.Constants.Keyword_While, Language.Constants.StatementEnd_While)
				) Then Throw New DSValidationException("The StatementOpenings (Function, If, While, Loop) were not well-balenced with the StatementClosings (EndFunction, EndIf, EndWhile, EndLoop)", "(Source Tokens)") 'I know that I should have used the Language.Constants.* instead of the Keyword and StatementEnds in this Exception-Message-String. Get over it.

			Catch _Ex As Exception : Throw New DSException("@EnsurePairedComponentsAreWellBalenced_: " & _Ex.Message, _Ex) : End Try
		End Sub

#End Region

#Region "(Public) Parser Utility Methods"

		''' <summary>
		''' Makes the source valid for within a single line or inside string literal; replaces [LineBreaks (CrLf)] with [;], and replaces ["] with [`].
		''' </summary>
		Public Function FlattenSource(ByVal _Source_ContainingLineBreaksAndQuotes$) As [String]
			Return _Source_ContainingLineBreaksAndQuotes _
			 .Replace(Language.Constants.LineBreak, Language.Constants.LineBreak_SubstitutionChar_ForFlattenedSource) _
			 .Replace(Language.Constants.StringLiteralStartEndChar, Language.Constants.StringLIteralStartEndChar_DSStringValueReplacementChar)
		End Function

		''' <summary>
		''' Takes in flattened source, and expands it; replaces [;] with [LineBreaks (CrLf)], and replaces [`] with ["].
		''' For example, DocScript-Live uses this method, so the line [Output("Ben");Output("Ryan")] becomes two seperate lines.
		''' </summary>
		Public Function UnflattenSource(ByVal _Source_ContainingSemiColonsAndTicks$) As [String]
			Return _Source_ContainingSemiColonsAndTicks _
			 .Replace(Language.Constants.LineBreak_SubstitutionChar_ForFlattenedSource, Language.Constants.LineBreak) _
			 .Replace(Language.Constants.StringLIteralStartEndChar_DSStringValueReplacementChar, Language.Constants.StringLiteralStartEndChar)
		End Function

#End Region

	End Module

End Namespace