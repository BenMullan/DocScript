Public Module DSECompilerExtentions

#Region "String Extentions"

	''' <summary>
	''' (MullNet CompilerExtention) Splits the String whenever one of the _CharsAtWhichToSplitString occours, but the _CharAtWhichToSplitString stays in each New String
	''' <example>
	''' "Hello, World; Today" and {","c, ";"c} Becomes {"Hello", ", World", "; Today"}
	''' </example>
	''' </summary>
	''' <returns>The _InputString split at each occourance of a _CharAtWhichToSplitString</returns>
	''' <remarks></remarks>
	<System.Runtime.CompilerServices.Extension()>
	Public Function SplitAtWithoutRemoving(ByVal _InputString$, ByVal _CharsAtWhichToSplitString As Char()) As String()

		Dim _StringsToReturn As New List(Of String)()
		Dim _StringToAddAccumulator$ = String.Empty

		For Each _Char As Char In _InputString.ToCharArray()

			If _CharsAtWhichToSplitString.Contains(_Char) Then

				REM Only add the String to be returned, if it is not empty
				If (Not String.IsNullOrEmpty(_StringToAddAccumulator)) Then _StringsToReturn.Add(_StringToAddAccumulator)
				_StringToAddAccumulator = String.Empty

			Else
				REM The _Char isn't one to split the String at
			End If

			_StringToAddAccumulator &= _Char.ToString()

		Next

		REM Add the Remaining contents of the Accumulator to the output
		_StringsToReturn.Add(_StringToAddAccumulator)

		Return _StringsToReturn.ToArray()

	End Function

	REM 09	=	Horisontal Tab
	REM 10	=	Line Feed
	REM	11	=	Vertical Tab
	REM 12	=	Form Feed
	REM 13	=	Carridge Return
	REM 32	=	Space
	Public ReadOnly WhitespaceChars As Char() = {Chr(9I), Chr(10I), Chr(11I), Chr(12I), Chr(13I), Chr(32I)}

	''' <summary>
	''' (MullNet CompilerExtention) Returns a version of the string with any Spaces, Tabs, or LineBreak Characters which appear at the start of the String removed.
	''' </summary>
	''' <param name="_InputString">The String which may contain Leading Whitespace Characters</param>
	''' <returns>The String without the Leading (preceding) WhiteSpace Characters</returns>
	''' <remarks>Only returns the resultant output; does not modify the Input String. Chars removed (ASCII): 09, 10, 11, 12, 13, 32</remarks>
	<Global.System.Runtime.CompilerServices.Extension()>
	Public Function WithLeadingWhiteSpaceRemoved$(ByVal _InputString$)

		For Each _Char As Char In _InputString$.ToCharArray()

			'If the _Char is one of the ones we want to Remove...
			If WhitespaceChars.Contains(_Char) Then
				'...Then Remove it.
				_InputString.RemoveFirstCharacter()
				REM The above does not modify the origional argument passed to the function at the call, because it is passed as ByVal, so it's only modifying a copy thereof.
			Else
				'Otherwise, We must have hit the start of meaningful Characters in the String, so stop checking.
				'Simply return the String as it is, now that WhiteSpace has been removed.
				Return _InputString
			End If

		Next

		'We could end up here, if:
		'	the Entire _InputString$ was all WhiteSpace,
		'	Or, the _InputString was a String::Empty when passed to the Function,
		'So retrun it, even though it's empty.
		Return _InputString

	End Function

	''' <summary>
	''' (MullNet CompilerExtention) Returns a version of the string with any Spaces, Tabs, or LineBreak Characters which appear at the end of the String removed.
	''' </summary>
	''' <param name="_InputString">The String which may contain Trailing Whitespace Characters</param>
	''' <returns>The String without the Trailing (following) WhiteSpace Characters</returns>
	''' <remarks>Only returns the resultant output; does not modify the Input String. Chars removed (ASCII): 09, 10, 11, 12, 13, 32</remarks>
	<Global.System.Runtime.CompilerServices.Extension()>
	Public Function WithTrailingWhiteSpaceRemoved$(ByVal _InputString$)

		For Each _Char As Char In _InputString$.ToCharArray().Reverse()

			'If the _Char is one of the ones we want to Remove...
			If WhitespaceChars.Contains(_Char) Then
				'...Then Remove it.
				_InputString.RemoveLastCharacter()
				REM The above does not modify the origional argument passed to the function at the call, because it is passed as ByVal, so it's only modifying a copy thereof.
			Else
				'(REMEMBER: this is going from the last Char in the String to the first one)
				'Otherwise, We must have hit the start (end) of meaningful Characters in the String, so stop checking.
				'Simply return the String as it is, now that WhiteSpace has been removed.
				Return _InputString
			End If

		Next

		'We could end up here, if:
		'	the Entire _InputString$ was all WhiteSpace,
		'	Or, the _InputString was a String::Empty when passed to the Function,
		'So retrun it, even though it's empty.
		Return _InputString

	End Function

	''' <summary>
	''' (MullNet CompilerExtention) Removes the First Character from the String
	''' </summary>
	''' <param name="_InputString"></param>
	''' <returns></returns>
	''' <remarks>Both Modifies the Parameter, and returns the post-computation Result</remarks>
	<Global.System.Runtime.CompilerServices.Extension()>
	Public Function RemoveFirstCharacter$(ByRef _InputString$)

		'If the String is Empty, we wouldn't be able to Remove the FirstChar
		If _InputString.IsEmpty() Then Throw New Exception("The First Character could not be removed from the String, because the String is empty.")

		'If we've gotten to here, the String must have at least 1 Character, so Remove the FirstChar
		_InputString = _InputString.Remove(0I, 1I)
		Return _InputString

	End Function

	''' <summary>
	''' (MullNet CompilerExtention) Removes the Last Character from the String
	''' </summary>
	''' <param name="_InputString"></param>
	''' <returns></returns>
	''' <remarks>Both Modifies the Parameter, and returns the post-computation Result</remarks>
	<Global.System.Runtime.CompilerServices.Extension()>
	Public Function RemoveLastCharacter$(ByRef _InputString$)

		'If the String is Empty, we wouldn't be able to Remove the LastChar
		If _InputString.IsEmpty() Then Throw New Exception("The Last Character could not be removed from the String, because the String is empty.")

		'If we've gotten to here, the String must have at least 1 Character, so Remove the LastChar
		_InputString = _InputString.Remove(_InputString.Length - 1, 1I)
		Return _InputString

	End Function

	''' <summary>
	''' (MullNet CompilerExtention) Evaluates weather or not the String is Empty. (WhiteSpace does not count as Empty)
	''' </summary>
	''' <param name="_InputString"></param>
	''' <returns>Retruns True if the String is Empty</returns>
	''' <remarks></remarks>
	<Global.System.Runtime.CompilerServices.Extension()>
	Public Function IsEmpty(ByVal _InputString$) As Boolean
		Return _InputString = Global.System.String.Empty
	End Function

	''' <summary>
	''' (MullNet CompilerExtention) Matches a String against a Regular Expression
	''' </summary>
	''' <param name="_StringToValidate"></param>
	''' <param name="_RegExPattern">The pattern to satisfy</param>
	''' <returns>Returns True if the RegExp is satisfied by the Input String.</returns>
	''' <remarks></remarks>
	<Global.System.Runtime.CompilerServices.Extension()>
	Public Function MatchesRegEx(ByVal _StringToValidate$, ByVal _RegExPattern$) As Boolean
		Return (New Global.System.Text.RegularExpressions.Regex([pattern]:=[_RegExPattern])).IsMatch(_StringToValidate)
	End Function

#End Region

#Region "Serialisation"

	'<Runtime.CompilerServices.Extension()>
	'Public Function GetKVPSerialisation()

	Public Function SerializeToXML(Of _ObjectToSerialiseType As {Class})(ByVal _ObjectToSerialise As _ObjectToSerialiseType) As String

		Dim _XMLSerializer As New Global.System.Xml.Serialization.XmlSerializer(GetType(_ObjectToSerialiseType))
		Dim _StringWriter As New System.IO.StringWriter()
		Dim _XMLTextWriter As New System.Xml.XmlTextWriter(_StringWriter) With {.Formatting = Xml.Formatting.Indented, .Indentation = 1, .IndentChar = Chr(9), .Namespaces = False, .QuoteChar = """"c}

		_XMLSerializer.Serialize(_XMLTextWriter, _ObjectToSerialise)
		Return _StringWriter.ToString()

	End Function

#End Region

End Module