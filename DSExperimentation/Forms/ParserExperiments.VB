Public Class ParserExperiments

	Public Sub GenerateTokens() Handles ParseButton.Click

		Try

			Console.WriteLine("The Console is Working if you can read this!") : Console.WriteLine()
			DocScript.Logging.LogUtilities.CurrentLogEventHandler = DocScript.Logging.BuiltInLogEventHandlers.GUIDefault

			Dim _Tokens As DocScript.Runtime.Token() = DocScript.Runtime.Parser.GetTokensFromSource(Me.SourceTextBox.Text)

			Me.TokensTextBox.Text = String.Empty : Me.TokenTypesTextBox.Text = String.Empty
			For Each _Token As DocScript.Runtime.Token In _Tokens
				Me.TokensTextBox.Text &= ("_"c & _Token.Value.Replace(vbCrLf, "{LineEnd}") & "_"c & vbCrLf)
				Me.TokenTypesTextBox.Text &= ("["c & _Token.Type.ToString("G") & "]"c & vbCrLf)
			Next

		Catch _Ex As Exception When True Or 5 = 5
			MsgBox("Exception:" & vbCrLf & vbCrLf & _Ex.Message, MsgBoxStyle.Critical, _Ex.GetType().Name)
		End Try

	End Sub

	'SerialisationTests
	Public Shared Sub Button1_Click() Handles Button1.Click
		Dim _Token As New Token("VALUE AS STRING!", Token.TokenType.BooleanLiteral, 12, 44)
		LogMsg(SerializeToXML(Of Token)(_Token))
	End Sub

	Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
		MsgBox(
		 DocScript.Runtime.Parser.GetTokensXML(
		 DocScript.Runtime.Parser.GetTokensFromSource(Me.SourceTextBox.Text)
		 ).ToString()
		)
	End Sub

	Public Sub ProgramFromTokensXML() Handles Button3.Click

		Dim _TokensXMLString$ = DocScript.Runtime.Parser.GetTokensXML(
		 DocScript.Runtime.Parser.GetTokensFromSource(Me.SourceTextBox.Text)
		).ToString()

		Dim _Program As DocScript.Runtime.Program = DocScript.Runtime.Program.FromTokensXML(XElement.Parse(_TokensXMLString), Nothing)

		MsgBox(_Program.ProgramTreeXML.ToString())

	End Sub

	Private Sub ProgramFromTokensXML(sender As System.Object, e As System.EventArgs) Handles Button4.Click

		'<Tokens> <Token ... /> </Tokens>
		Dim _TokensXMLString$ = DocScript.Runtime.Parser.GetTokensXML(
		 DocScript.Runtime.Parser.GetTokensFromSource(Me.SourceTextBox.Text)
		).ToString()

		Dim _ProgramFromTokens As DocScript.Runtime.Program = DocScript.Runtime.Program.FromTokensXML(XElement.Parse(_TokensXMLString), Nothing)

		Dim _ProgTreeXMLString$ = _ProgramFromTokens.ProgramTreeXML.ToString()

		MsgBox(_ProgTreeXMLString)

		Dim _ProgramFromProgTreeXML As DocScript.Runtime.Program = DocScript.Runtime.Program.FromProgramTreeXML(XElement.Parse(_ProgTreeXMLString), Nothing)

		MsgBox(_ProgramFromProgTreeXML.ReconstructedSource)

	End Sub

End Class

Module Parser

	Public Function Parse(ByVal _RawSourceCode$) As Token()

		Dim _DerivedTokens As New List(Of Token)()

		'The unprocessed lines from the Source String:
		Dim _RawSourceLines As String() = _RawSourceCode.Split({vbCrLf}, StringSplitOptions.None).ToArray()
		LogMsg("Loaded in " & _RawSourceLines.Length.ToString() & " Line(s) from Source")

		REM Substitute [Whitespace Lines], and [#Comment Lines] with Empty Lines:
		Dim _FilteredSourceLines As List(Of String) = Parser.ReplaceWhitespaceLinesAndLineCommentsWithEmptyLines(_RawSourceLines)

		_FilteredSourceLines.ForEach(Sub(_Line$) LogMsg("Filtered Source Line: |" & _Line & "|"c))

		REM At this point, we have no [Whitespace Lines] or [#Comment Lines].
		REM Any [Whitespace Lines] or [#Comment Lines] have been substituted with Empty Lines.
		REM This is needed so that we can still accurately report the Line and Column Location of the Tokens.
		REM Next, StringLiterals will be replaced with StringLiteral Indication Tokens (SLITs)

		Dim _ExtractedStringLiterals As New List(Of String)()
		Dim _NumberOfSLITsInserted As UInt16 = 0

		Dim _SLITMatchEvaluator As New System.Text.RegularExpressions.MatchEvaluator(
		 Function(_Match As System.Text.RegularExpressions.Match) As String
			 _NumberOfSLITsInserted += 1US
			 _ExtractedStringLiterals.Add(_Match.Value)	'The SLIT No. is its index in the [_ExtractedStringLiterals] List
			 Return GlobalConstants.SLITPlaceholderPattern.Replace("{Number}", (_ExtractedStringLiterals.Count - 1).ToString())
		 End Function
		)

		For _SourceLineIndex As UInt16 = 0 To CUShort(_FilteredSourceLines.Count - 1) Step +1
			_FilteredSourceLines.Item(_SourceLineIndex) = _
			 System.Text.RegularExpressions.Regex.Replace( _
			  input:=_FilteredSourceLines.Item(_SourceLineIndex),
			  pattern:="""[^""]*""",
			  evaluator:=_SLITMatchEvaluator,
			  options:=System.Text.RegularExpressions.RegexOptions.None
			 )
		Next

		LogMsg("Replaced " & _NumberOfSLITsInserted.ToString() & " String Literals with SLITs")

		REM Now that StringLiterals are removed, Check that all the remaining Characters are valid

		'Function <Number> Main (<String> CLAs)
		'	Output("Hello, World!")
		'	Return 0
		'End-Function


		REM TOKEN LOCATIONS!!!!!

		Dim _WordAccumulator$ = String.Empty

		For _LineNumber As UInt16 = 1 To CUShort(_FilteredSourceLines.Count) Step +1
			For _ColumnNumber As UInt16 = 1 To CUShort(_FilteredSourceLines.Item(_LineNumber - 1).Length) Step +1

				Dim _CurrentCharacter As Char = _FilteredSourceLines.Item(_LineNumber - 1).ToCharArray()(_ColumnNumber - 1)

				LogMsg("Current Character: |" & _CurrentCharacter & "|"c)

				REM If the Char is a WordChar, add it to the _WordAccumulator$
				REM		If that was the last Char of a Line, empty the _WordAccumulator into a [New Token()]
				REM Otherwise:
				REM		If the _WordAccumulator contains some text, empty it and add it's contents as a [New Token()]
				REM		Then just add the Char as a New Token()


				'MAKING SURE WE'RE ALSO SPLITTING AT THE END OF A LINE. Otherwise, a line not ending in a GrammarChar followed by a Line not starting with a GrammarChar will cause no Split to occour.

				'If it's NOT a WordChar (i.e. then it's a GrammarChar), OR this is the last Char on the Line



				If _CurrentCharacter.ToString().MatchesRegEx(GlobalConstants.WordChar_RegExp) Then

					LogMsg("WordChar: |" & _CurrentCharacter & "|"c)

					'It's a WordChar
					_WordAccumulator &= _CurrentCharacter

					'MAKING SURE WE'RE ALSO SPLITTING AT THE END OF A LINE. Otherwise, a line not ending in a GrammarChar followed by a Line not starting with a GrammarChar will cause no Split to occour. So you'd get "ReturnEnd" From a Return followed by an End-Function on the next line.
					If (_ColumnNumber = (_FilteredSourceLines.Item(_LineNumber - 1).Length)) Then
						_DerivedTokens.Add(New Token(_WordAccumulator, Token.TokenType.Unresolved, _LineNumber, CUShort(_ColumnNumber - _WordAccumulator.Length)))
						_WordAccumulator = String.Empty
					End If

				Else
					If Not _WordAccumulator.IsEmpty() Then
						_DerivedTokens.Add(New Token(_WordAccumulator, Token.TokenType.Unresolved, _LineNumber, CUShort(_ColumnNumber - _WordAccumulator.Length)))
						_WordAccumulator = String.Empty
					End If

					_DerivedTokens.Add(New Token(_CurrentCharacter.ToString(), Token.TokenType.Unresolved, _LineNumber, _ColumnNumber))

				End If

			Next
		Next

		REM Remove Null Tokens
		Dim _NonNullTokens As List(Of Token) = _
		(From _Token As Token In _DerivedTokens Where Not _Token.Value.WithLeadingWhiteSpaceRemoved().IsEmpty() Select _Token).ToList()
		LogMsg("Removed " & (_DerivedTokens.Count - _NonNullTokens.Count).ToString() & " Null Token(s)")
		_DerivedTokens = _NonNullTokens

		REM Now that the Segmentation has occoured, we have a series of Tokens with Unresolved TokenTypes.
		REM Next, we must therefore Resolve the TokenTypes...

		LogMsg("Finished Token Generation; Returning " & _DerivedTokens.Count.ToString() & " Token(s)")
		Return _DerivedTokens.ToArray()

	End Function

	Private Function ReplaceWhitespaceLinesAndLineCommentsWithEmptyLines(ByRef _RawSourceLines$()) As List(Of String)

		Dim _ProcessedSourceLines As New List(Of String)()

		Dim _NumberOfCommentLines As UInt16 = 0
		Dim _NumberOfWhitespaceOrEmptyLines As UInt16 = 0

		For Each _RawLine$ In _RawSourceLines

			'The _ProcessedLine will be added to the lines to return (called "_ProcessedSourceLines")
			Dim _ProcessedLine$ = _RawLine

			REM Check if the Line is Empty, or just Whitespace
			If _ProcessedLine.WithLeadingWhiteSpaceRemoved().WithTrailingWhiteSpaceRemoved().IsEmpty() Then
				_NumberOfWhitespaceOrEmptyLines += 1US
				'Instead of the (potentially) Whitespace, substitute in an Empty Line
				_ProcessedSourceLines.Add(String.Empty) : Continue For
			End If

			REM Check if the Line is a #Comment
			If _ProcessedLine.WithLeadingWhiteSpaceRemoved().StartsWith(GlobalConstants.CommentCharacter.ToString()) Then
				_NumberOfCommentLines += 1US
				'Instead of the Comment, substitute in an Empty Line
				_ProcessedSourceLines.Add(String.Empty) : Continue For
			End If

			'If we're here, then the Line wasn't empty, and it wasn't a comment; just keep it as it is
			_ProcessedSourceLines.Add(_ProcessedLine)

		Next

		LogMsg("Ignored " & _NumberOfCommentLines.ToString() & " Comment Line(s)")
		LogMsg("Ignored " & _NumberOfWhitespaceOrEmptyLines.ToString() & " Empty or Whitespace-only Line(s)")

		Return _ProcessedSourceLines

	End Function

End Module

Class [Token]

	Public Enum TokenType As UInt16
		Unresolved = 0
		StringLiteral = 1
		NumericLiteral = 2
		BooleanLiteral = 3
		Keyword = 4
		DataType = 5
		Identifier = 6
		DSOperator = 7
		GrammarChar = 8
		LineEnd = 9
		StatmentEnd = 10
	End Enum

	''' <summary>
	''' Represents the origional location of a Token in the Source
	''' </summary>
	Public Structure TokenLocation

		Public ReadOnly Line As UInt16
		Public ReadOnly Column As UInt16

		Public Sub New(ByVal _Line As UInt16, ByVal _Column As UInt16)
			Me.Line = _Line : Me.Column = _Column
		End Sub

		Public Overrides Function ToString() As String
			Return "[ Line = " & Me.Line.ToString() & ", Column = " & Me.Column.ToString() & " ]"
		End Function

	End Structure

	Public ReadOnly Value As String
	Public ReadOnly Type As TokenType
	Public ReadOnly LocationInSource As TokenLocation

	Public Sub New(ByVal _Value$, ByVal _Type As TokenType, ByVal _LocationLine As UInt16, ByVal _LocationColumn As UInt16)
		Me.Value = _Value
		Me.Type = _Type
		Me.LocationInSource = (New Token.TokenLocation(_LocationLine, _LocationColumn))
	End Sub

	Public Overrides Function ToString() As String
		Return "[ Type = " & Me.Type.ToString() & ", Value = " & Me.Value & ", LocationInSource = " & Me.LocationInSource.ToString() & " ]"
		'Return <Token Type=<%= Me.Type.ToString() %> Value=<%= Me.Value %>/>.ToString()
	End Function

End Class