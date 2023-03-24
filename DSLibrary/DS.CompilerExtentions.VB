#If False Then

Template for CompilerExtentions:

''' <summary>(MullNet CompilerExtention) Returns a version of the _Array wherein all Elements in the _Array which meet the _Condition are replaced with the _Replacement</summary>
<Global.System.Runtime.CompilerServices.Extension()>
Public Function SOMETHING$(ByVal _Array$())
	Try

		Return

	Catch _Ex As Exception : Throw New DSException("@_______________________SOMETHING_______________________ (CompilerExtention): " & _Ex.Message, _Ex) : End Try
End Function

#End If

Namespace CompilerExtentions

	Public Module ObjectExtentions

		''' <summary>
		''' (MullNet CompilerExtention) Ensures that the _Object is of Type _TSoughtType, returning it if it is, and Throwing a DSException if it isn't.
		''' This method is useful for converting an Interface-based Type to a Class-based one.
		''' </summary>
		''' <typeparam name="_TSoughtType">If the _IDataValueObject isn't of this Type, then a DSException is Thrown.</typeparam>
		''' <param name="_Object">The Object to ensure the Type of.</param>
		<System.Runtime.CompilerServices.Extension()>
		Public Function MustBe(Of _TSoughtType)(ByVal _Object As Object) As _TSoughtType

			If Not (TypeOf _Object Is _TSoughtType) Then : Throw New DSException(String.Format("MustBe(Of T): The Object was of Type {0}, but needed to be of Type {1}.", _Object.GetType().Name.InSquares(), GetType(_TSoughtType).Name.InSquares()))
			Else : Return CType(_Object, _TSoughtType)
			End If

		End Function

		''' <summary>(MullNet CompilerExtention) Runs the ToString() of the _Object through the specified _Action</summary>
		<System.Runtime.CompilerServices.Extension()>
		Public Function Output(Of _TInput)(ByVal _Object As _TInput, ByVal _Action As Action(Of String), Optional ByVal _MsgBefore$ = "") As _TInput
			If Not (_MsgBefore = "") Then _MsgBefore &= " "c
			_Action.Invoke(_MsgBefore & _Object.ToString())
			Return _Object
		End Function

		''' <summary>(MullNet CompilerExtention) Ensures that the _Object IsNot Nothing, Throwing a NullReferanceException with the _MessageIfNull if it is</summary>
		<System.Runtime.CompilerServices.Extension()>
		Public Function MustNotBeNothing(Of _TObject)(ByRef _Object As _TObject, Optional ByVal _MsgIfNull$ = "") As _TObject
			If (_Object Is Nothing) Then Throw New NullReferenceException("The Object was [Nothing]. " & _MsgIfNull)
			Return _Object
		End Function

		''' <summary>(MullNet CompilerExtention) Determines if the _Object Is Nothing, Invoke()-ing the _ActionIfNull if it is</summary>
		<System.Runtime.CompilerServices.Extension()>
		Public Function MustNotBeNothing(Of _TObject)(ByRef _Object As _TObject, ByVal _ActionIfNull As action) As _TObject
			If (_Object Is Nothing) Then _ActionIfNull.Invoke()
			Return _Object
		End Function

	End Module

	Public Module StringExtentions

		REM 09	=	Horisontal Tab
		REM 10	=	Line Feed
		REM	11	=	Vertical Tab
		REM 12	=	Form Feed
		REM 13	=	Carridge Return
		REM 32	=	Space
		Public ReadOnly WhitespaceChars As Char() = {Chr(9I), Chr(10I), Chr(11I), Chr(12I), Chr(13I), Chr(32I)}

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
		''' (MullNet CompilerExtention) Removes the First Character from the String.
		''' Both Modifies the Parameter, and returns the post-computation Result.
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
		''' (MullNet CompilerExtention) Removes the First Character from the String.
		''' Only returns the post-computation Result; does NOT modify the input.
		''' </summary>
		''' <param name="_InputString"></param>
		''' <returns></returns>
		''' <remarks>Both Modifies the Parameter, and returns the post-computation Result</remarks>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function WithFirstCharacterRemoved$(ByVal _InputString$)

			'If the String is Empty, we wouldn't be able to Remove the FirstChar
			If _InputString.IsEmpty() Then Throw New Exception("The First Character could not be removed from the String, because the String is empty.")
			Return _InputString.Remove(0I, 1I)

		End Function

		''' <summary>
		''' (MullNet CompilerExtention) Removes the Last Character from the String.
		''' Both Modifies the Parameter, and returns the post-computation Result.
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

		''' <summary>(MullNet CompilerExtention) Returns in the form [_InputString$]</summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function InSquares$(ByRef _InputString$)
			Return ("["c & _InputString & "]"c)
		End Function

		''' <summary>(MullNet CompilerExtention) Returns in the form {_InputString$}</summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function InCurlies$(ByRef _InputString$)
			Return ("{"c & _InputString & "}"c)
		End Function

		''' <summary>(MullNet CompilerExtention) Returns in the form &lt;_InputString$&gt;</summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function InPointies$(ByRef _InputString$)
			Return ("<"c & _InputString & ">"c)
		End Function

		''' <summary>(MullNet CompilerExtention) Returns in the form (_InputString$)</summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function InBrackets$(ByRef _InputString$)
			Return ("("c & _InputString & ")"c)
		End Function

		''' <summary>(MullNet CompilerExtention) Returns a string containing any Digit Chars (0 to 9) which were found in the _InputString$</summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function GetDigitChars$(ByRef _InputString$)
			Return New String(_InputString.Where(AddressOf Char.IsDigit).ToArray())
		End Function

		''' <summary>(MullNet CompilerExtention) Puts _NumberOfTabs Tabs at the start of each vbCrLf-delimited Line</summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function TabbedInBy(ByVal _Lines$, ByVal _NumberOfTabs As Byte) As String

			Const _TabChar As Char = Chr(9)

			Dim _SplitLines$() = _Lines.Split({vbCrLf}, System.StringSplitOptions.None)
			Dim _TabbedInLines_ToReturn$ = ""

			For Each _Line$ In _SplitLines
				_TabbedInLines_ToReturn &= (_TabChar.Repeat(_NumberOfTabs) & _Line & vbCrLf)
			Next

			REM Remove the last vbCrLf from the end, if the Loop iterated at all. vbCrLf is TWO CHARACTERS!
			If _SplitLines.Length >= 1 Then _TabbedInLines_ToReturn.RemoveLastCharacter().RemoveLastCharacter()

			Return _TabbedInLines_ToReturn

		End Function

		''' <summary>(MullNet CompilerExtention) Returns a version of the _InputString, where only chars also appearing in _AllowedChars make it through to the returned string</summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function OnlyAllowTheseChars(ByVal _InputString$, ByVal _AllowedChars As Char()) As String
			Return New String(_InputString.Where(AddressOf _AllowedChars.Contains).ToArray())
		End Function

		''' <summary>(MullNet CompilerExtention) Returns a New String, directly joining in all the _SubsequentParts</summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function Glue(ByVal _InputString$, ByVal ParamArray _SubsequentParts As String()) As String
			Return String.Join("", {_InputString}.Concat(_SubsequentParts).ToArray())
		End Function

		''' <summary>(MullNet CompilerExtention) Computes and Returns the MD5 hash for the _InputString</summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function GetMD5Hash(ByVal _InputString$) As String
			Try

				Return (New Security.Cryptography.MD5CryptoServiceProvider()) _
				 .ComputeHash(System.Text.Encoding.ASCII.GetBytes(_InputString$)) _
				 .Aggregate(Of String, String)(
				   seed:="",
				   func:=Function(_CollectedSoFar$, _CurrentByte As Byte) _CollectedSoFar & _CurrentByte.ToString("x2"),
				   resultSelector:=Function(_FinalString$) _FinalString.ToUpper()
				)

			Catch _Ex As Exception : Throw New DSException("@GetMD5Hash (CompilerExtention): " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>(MullNet CompilerExtention) Computes and Returns the Base64-Representation of the _InputString</summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function PlainText_ToBase64(ByVal _PlainText$) As String
			Try
				Return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(_PlainText$))
			Catch _Ex As Exception : Throw New DSException("@PlainText_ToBase64 (CompilerExtention): " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>(MullNet CompilerExtention) Computes and Returns the PlainText, from a Base64-Encoded _InputString</summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function Base64_ToPlainText(ByVal _Base64$) As String
			Try
				Return System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(_Base64))
			Catch _Ex As Exception : Throw New DSException("@Base64_ToPlainText (CompilerExtention): " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>(MullNet CompilerExtention) Filters for digit-chars, then attempts to parse an UInt32</summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function Extract_UInt32(ByVal _String$) As UInt32
			Try
				Return UInt32.Parse(_String.OnlyAllowTheseChars("0123456789".ToCharArray()))
			Catch _Ex As Exception : Throw New DSException("@Extract_UInt32 (CompilerExtention): " & _Ex.Message, _Ex) : End Try
		End Function

	End Module

	Public Module DateTimeExtentions

		''' <summary>(MullNet CompilerExtention) Gets a short Date and Time String suitable for Logs, in the form "21-07 12:00:08.625"</summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function GetLogSerialisation(ByRef _DateTime As Global.System.DateTime) As String
			Return _DateTime.ToString("dd-MM HH:mm:ss.fff")
		End Function

	End Module

	Public Module ControlExtentions

		''' <summary>
		''' (MullNet CompilerExtention) Invokes the Control if required, and runs the specified _Action
		''' </summary>
		<System.Runtime.CompilerServices.Extension()> _
		Public Sub InvokeIfRequired(ByVal _Control As Windows.Forms.Control, ByVal _Action As Action)
			If _Control.InvokeRequired Then
				_Control.Invoke(_Action)
			Else
				_Action.Invoke()
			End If
		End Sub

		''' <summary>
		''' (MullNet CompilerExtention) Invokes the Control if required, and runs the specified _Action
		''' </summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Sub InvokeIfRequired(ByRef _Control As System.Windows.Controls.Control, ByRef _Action As Action)
			If Not _Control.Dispatcher.CheckAccess() Then
				_Control.Dispatcher.BeginInvoke(_Action)
			Else
				_Action.Invoke()
			End If
		End Sub

		''' <summary>(MullNet CompilerExtention) Returns the Text from the RichTextBox</summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function GetText(ByRef _RichTextBox As Windows.Controls.RichTextBox) As [String]
			Try
				Return New Windows.Documents.TextRange(_RichTextBox.Document.ContentStart, _RichTextBox.Document.ContentEnd).Text
			Catch _Ex As Exception : Throw New DSException("@GetText (CompilerExtention): " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>(MullNet CompilerExtention) Returns the Item1=Line and Item2=Column of the Caret in the RichTextBox</summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function GetCaretLineAndColumn(ByRef _RichTextBox As Windows.Controls.RichTextBox) As Tuple(Of Int32, Int32)
			Try

				'int lineNumber;
				'textBox.CaretPosition.GetLineStartPosition(-int.MaxValue, out lineNumber);
				'int columnNumber = richTextBox.CaretPosition.GetLineStartposition(0).GetOffsetToPosition(richTextBox.CaretPosition);
				'if (lineNumber == 0)
				'    columnNumber--;

				'statusBarLineColumn.Content = string.Format("Line {0}, Column {1}", -lineNumber + 1, columnNumber + 1);

				Dim _LineNumber%
				_RichTextBox.CaretPosition.GetLineStartPosition(-Int32.MaxValue, _LineNumber)

				Dim _ColumnNumber% = _RichTextBox.CaretPosition.GetLineStartPosition(0).GetOffsetToPosition(_RichTextBox.CaretPosition)
				If _LineNumber = 0 Then _ColumnNumber -= 1

				Return New Tuple(Of Int32, Int32)(-_LineNumber + 1, _ColumnNumber + 1)

			Catch _Ex As Exception : Throw New DSException("@GetCaretLineAndColumn (CompilerExtention): " & _Ex.Message, _Ex) : End Try
		End Function

	End Module

	Public Module CollectionTypeExtentions

		''' <summary>
		''' (MullNet CompilerExtention) Returns the last _Count Elements from the _Array
		''' </summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function TakeBlockFromEnd(Of _TElement)(ByRef _Array As _TElement(), ByVal _Count As UInt32) As _TElement()
			Try

				Return _Array.Skip(Math.Max(0, _Array.Count() - CInt(_Count))).ToArray()

			Catch _Ex As Exception : Throw New DSException("@TakeBlockFromEnd (CompilerExtention): " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>
		''' (MullNet CompilerExtention) Returns all elements up to (but excluding) the first element where the _Condition is met.  Throws an Exception if there is not at least one element which meets the condition.
		''' </summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function UpToButExcluding(Of _TElement)(ByRef _Array As _TElement(), ByVal _Condition As Func(Of _TElement, Boolean)) As _TElement()
			Try
				REM Without this check, _IndexOfLastWantedElement would still be 0 when we hit the Return, so we'd return only the first element
				If Not _Array.Any(_Condition) Then Throw New DSException("None of the Elements in the _Array matched the _Condition")

				REM The _Array could be Empty:
				If _Array.Length = 0 Then Return _Array

				Dim _IndexOfLastWantedElement As Int32 = 0

				For _ElementIndex As Int32 = 0 To (_Array.Length - 1) Step +1
					If _Condition.Invoke(_Array(CInt(_ElementIndex))) Then

						'The last wanted element was the previous one
						_IndexOfLastWantedElement = (_ElementIndex - 1) : Exit For

					End If
				Next

				Return _Array.Take(_IndexOfLastWantedElement + 1).ToArray()

			Catch _Ex As Exception : Throw New DSException("@UpToButExcluding (CompilerExtention): " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>
		''' (MullNet CompilerExtention) Returns all elements up to (and including) the first element where the _Condition is met. Throws an Exception if there is not at least one element which meets the condition.
		''' </summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function UpToAndIncluding(Of _TElement)(ByRef _Array As _TElement(), ByVal _Condition As Func(Of _TElement, Boolean)) As _TElement()
			Try
				REM Without this check, _IndexOfLastWantedElement would still be 0 when we hit the Return, so we'd return only the first element
				If Not _Array.Any(_Condition) Then Throw New DSException("None of the Elements in the _Array matched the _Condition")

				REM The _Array could be Empty:
				If _Array.Length = 0 Then Return _Array

				Dim _IndexOfLastWantedElement As UInt32 = 0

				For _ElementIndex% = 0 To (_Array.Length - 1) Step +1
					If _Condition.Invoke(_Array(_ElementIndex)) Then

						'The last wanted element is teh current one
						_IndexOfLastWantedElement = CUInt(_ElementIndex) : Exit For

					End If
				Next

				Return _Array.Take(CInt(_IndexOfLastWantedElement + 1)).ToArray()

			Catch _Ex As Exception : Throw New DSException("@UpToAndIncluding (CompilerExtention): " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>
		''' (MullNet CompilerExtention) Returns all elements up to (and including) the last element where the _Condition is met. Throws an Exception if there is not at least one element which meets the condition.
		''' </summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function UpToAndIncludingLast(Of _TElement)(ByRef _Array As _TElement(), ByVal _Condition As Func(Of _TElement, Boolean)) As _TElement()
			Try
				REM Without this check, _IndexOfLastWantedElement would still be 0 when we hit the Return, so we'd return only the first element
				If Not _Array.Any(_Condition) Then Throw New DSException("None of the Elements in the _Array matched the _Condition")

				REM The _Array could be Empty:
				If _Array.Length = 0 Then Return _Array

				Dim _IndexOfLastWantedElement As UInt32 = CUInt(_Array.IndexOfLastWhere(_Condition))

				Return _Array.Take(CInt(_IndexOfLastWantedElement + 1)).ToArray()

			Catch _Ex As Exception : Throw New DSException("@UpToAndIncludingLast (CompilerExtention): " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>
		''' (MullNet CompilerExtention) Returns all elements up to (but excluding) the last element where the _Condition is met. Throws an Exception if there is not at least one element which meets the condition.
		''' </summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function UpToButExcludingLast(Of _TElement)(ByRef _Array As _TElement(), ByVal _Condition As Func(Of _TElement, Boolean)) As _TElement()
			Try
				REM Without this check, _IndexOfLastWantedElement would still be 0 when we hit the Return, so we'd return only the first element
				If Not _Array.Any(_Condition) Then Throw New DSException("None of the Elements in the _Array matched the _Condition")

				REM The _Array could be Empty:
				If _Array.Length = 0 Then Return _Array

				Dim _IndexOfLastWantedElement As UInt32 = CUInt(_Array.IndexOfLastWhere(_Condition))

				Return _Array.Take(CInt(_IndexOfLastWantedElement)).ToArray()

			Catch _Ex As Exception : Throw New DSException("@UpToAndIncludingLast (CompilerExtention): " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>
		''' (MullNet CompilerExtention) Returns chunks of Elements split by any Element where the _Condition is met. None of the Elements which meet the _Condition will end up in the output groups
		''' </summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function SplitWhere(Of _TElement)(ByRef _Array As _TElement(), ByVal _Condition As Func(Of _TElement, Boolean)) As _TElement()()
			Try

				REM The _Array could be Empty:
				If _Array.Length = 0 Then Return (New List(Of _TElement())).ToArray()

				Dim _ChunksToReturn As New List(Of _TElement())()
				Dim _ChunkAccumulator As New List(Of _TElement)()

				For Each _Element As _TElement In _Array
					If _Condition.Invoke(_Element) Then	'The _Element is one to Split at
						_ChunksToReturn.Add(_ChunkAccumulator.ToArray()) : _ChunkAccumulator.Clear()
					Else 'The _Element is not one to Split at
						_ChunkAccumulator.Add(_Element)
					End If
				Next

				REM Finally, empty the _ChunkAcc. out
				_ChunksToReturn.Add(_ChunkAccumulator.ToArray())

				Return _ChunksToReturn.ToArray()

			Catch _Ex As Exception : Throw New DSException("@SplitWhere (CompilerExtention): " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>
		''' (MullNet CompilerExtention) Returns a version of the _Array wherein all Elements in the _Array which meet the _Condition are replaced with the _Replacement
		''' </summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function ReplaceWhere(Of _TElement)(ByVal _Array As _TElement(), ByVal _Condition As Func(Of _TElement, Boolean), ByVal _Replacement As _TElement) As _TElement()
			Try

				For _Index% = 0 To (_Array.Length - 1) Step +1
					If _Condition.Invoke(_Array(_Index)) Then _Array(_Index) = _Replacement
				Next

				Return _Array

			Catch _Ex As Exception : Throw New DSException("@ReplaceWhere (CompilerExtention): " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>
		''' (MullNet CompilerExtention) Returns the Index of the first element to satisfy the _Condition. Throws an Exception if no elements satisfy the _Condition.
		''' </summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function IndexOfFirstWhere(Of _TElement)(ByVal _Array As _TElement(), ByVal _Condition As Func(Of _TElement, Boolean)) As Int32
			Try

				If Not _Array.Any(_Condition) Then Throw New DSException("None of the Elements in the _Array matched the _Condition")

				For _Index% = 0 To (_Array.Length - 1) Step +1
					If _Condition.Invoke(_Array(_Index)) Then
						Return _Index
					End If
				Next

				Throw New DSException("This Exception should never be seen; At least one element in the _Array must have satisfied the Condition, and yet the a Return didn't occour herebefore.")

			Catch _Ex As Exception : Throw New DSException("@IndexOfFirstWhere (CompilerExtention): " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>
		''' (MullNet CompilerExtention) Returns the Index of the last element to satisfy the _Condition. Throws an Exception if no elements satisfy the _Condition.
		''' </summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function IndexOfLastWhere(Of _TElement)(ByVal _Array As _TElement(), ByVal _Condition As Func(Of _TElement, Boolean)) As Int32
			Try
				'5, 4, 7, 6, 2, 6, 4, 5, 6, 8, 7, 5, 6
				'0  1  2  3  4  5  6  7  8  9  10 11 12
				If Not _Array.Any(_Condition) Then Throw New DSException("None of the Elements in the _Array matched the _Condition")

				For _Index% = (_Array.Length - 1) To 0 Step -1
					If _Condition.Invoke(_Array(_Index)) Then
						Return _Index
					End If
				Next

				Throw New DSException("This Exception should never be seen; At least one element in the _Array must have satisfied the Condition, and yet the a Return didn't occour herebefore.")

			Catch _Ex As Exception : Throw New DSException("@IndexOfLastWhere (CompilerExtention): " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>(MullNet CompilerExtention) Evaluates weather or not each Element in the _Array is unique. In other words, False is Returned if two or more elements are the same.</summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function AllElementsAreUnique(Of _TElement)(ByVal _Array As _TElement()) As Boolean
			Return (New HashSet(Of _TElement)(_Array)).Count = _Array.Length
		End Function

		''' <summary>(MullNet CompilerExtention) Repeats the _Element _Times times.</summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function Repeat(Of _TElement)(ByVal _Element As _TElement, ByVal _Times As UInt64) As _TElement()

			Dim _ElementsToReturn As New List(Of _TElement)()

			For _Index As UInt64 = 0 To Convert.ToUInt64(_Times - 1) Step +1 : _ElementsToReturn.Add(_Element) : Next

			Return _ElementsToReturn.ToArray()

		End Function

		''' <summary>(MullNet CompilerExtention) Gets the ToString() of each Element in the _Array, and serialises these Strings into the form {String1, String2, String3}</summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function GetStandardArraySerialisation(Of _TElement)(ByVal _Array As _TElement(), Optional ByVal _Seperator$ = ", ") As String

			Return String.Join(_Seperator, _Array.Select(Function(_Element As _TElement) If(_Element IsNot Nothing, _Element.ToString(), "(GetStandardArraySerialisation: Nothing)"))).InCurlies()

		End Function

		''' <summary>
		''' (MullNet CompilerExtention) Returns a version of the _Array where the _FillerBlock is inserted after each _HowOften elements.
		''' Does not insert any instances of the _FillerBlock past the end of the _Array's last Element
		''' </summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function Intersperse(Of _TElement)(ByVal _Array As _TElement(), ByVal _FillerBlock As _TElement(), ByVal _HowOften As UInt32) As _TElement()
			Try
				If _HowOften = 0 Then Throw New ArgumentException("_HowOften cannot be 0", "_HowOften")
				If _Array.Length = 0 Then Return _Array

				Dim _InterspersedArray As New List(Of _TElement)()

				For _Index As UInt32 = 0 To CUInt(_Array.Length - 1) Step +1
					If ((_Index Mod _HowOften) = 0) AndAlso (Not (_Index = 0)) Then _InterspersedArray.AddRange(_FillerBlock) 'Insert an instance of the FillerBlock
					_InterspersedArray.Add(_Array.ElementAt(CInt(_Index)))
				Next

				Return _InterspersedArray.ToArray()

			Catch _Ex As Exception : Throw New DSException("@Intersperse (CompilerExtention): " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>(MullNet CompilerExtention) Returns all but the very last Element in the _Array</summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function AllButLast(Of _TElement)(ByVal _Array As _TElement()) As _TElement()
			Try

				Dim _ArrayToReturn As New List(Of _TElement)()

				For _Index% = 0 To (_Array.Length - 2) Step +1
					_ArrayToReturn.Add(_Array.ElementAt(_Index))
				Next

				Return _ArrayToReturn.ToArray()

			Catch _Ex As Exception : Throw New DSException("@AllButLast (CompilerExtention): " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>(MullNet CompilerExtention) Returns all the Keys in the Read-only Dictionary</summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function GetAllKeys(Of _TKey, _TValue)(ByVal _ReadOnlyDictionary As ObjectModel.ReadOnlyCollection(Of KeyValuePair(Of _TKey, _TValue))) As _TKey()
			Try
				Return _ReadOnlyDictionary.Select(Of _TKey)(Function(_KVP As KeyValuePair(Of _TKey, _TValue)) _KVP.Key).ToArray()
			Catch _Ex As Exception : Throw New DSException("@GetAllKeys (CompilerExtention): " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>(MullNet CompilerExtention) Executes the _Action for each Element in the _ReadOnlyCollection(Of _TElement)</summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Sub ForEachItem(Of _TElement)(ByRef _ReadOnlyCollection As ObjectModel.ReadOnlyCollection(Of _TElement), ByVal _Action As Action(Of _TElement))
			Try

				For Each _Element In _ReadOnlyCollection
					_Action.Invoke(_Element)
				Next

			Catch _Ex As Exception : Throw New DSException("@ForEachItem (CompilerExtention): " & _Ex.Message, _Ex) : End Try
		End Sub

		''' <summary>(MullNet CompilerExtention) Returns a value which isn't equal to any of the existing strings, based off of the _Seed.
		''' The returned value will ALWAYS be the same length as the _Seed, but if it is not possible to create a new value within that number of characters, then this method will Throw an Exception.
		''' If the _Seed is empty, and still not contained in the _StringArray, then an Exception will be Thrown.
		''' The SubstitutionChars are 0123456789ABCDEFGHIJKLNMOPQRSTUVWXYZ</summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function GenerateUniqueString(ByRef _StringArray As [String](), ByVal _Seed$) As [String]
			Try
				Dim _SubstitutionChars As Char() = "0123456789ABCDEFGHIJKLNMOPQRSTUVWXYZ".ToCharArray()

				'HELLO
				'0ELLO

				If Not _StringArray.Contains(_Seed) Then Return _Seed
				If String.IsNullOrEmpty(_Seed) Then Throw New DSException("A unique value cannot be found, because the _Seed was empty, and not contained in the _StringArray, and this Function must keep the promise of never returning a value whose length is different to that of the _Seed.")

				'Here, the _Seed is already contained in the _StringArray

				'For each CharIndex of the Seed
				For _SeedChar_Index% = (_Seed.Length - 1) To 0 Step -1

					'Put in each possible SubstitutionChar, and see if that makes the String Unique amoungst the _StringArray
					For Each _SubstitutionChar As Char In _SubstitutionChars
						Dim _Seed_WithSubstitution$ = New String(_Seed.ToCharArray().SubstitutedAtIndex(_SeedChar_Index, _SubstitutionChar))
						If Not _StringArray.Contains(_Seed_WithSubstitution) Then Return _Seed_WithSubstitution
					Next

				Next

				Throw New Exception("No unique value could be generated (whilst maintaining the _Seed length)")

			Catch _Ex As Exception : Throw New DSException("@FindUniqueValue (CompilerExtention): " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>(MullNet CompilerExtention) Returns a version of the _Array wherein the Element at _Index is replaced with the _Substitution</summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function SubstitutedAtIndex(Of _TElement)(ByVal _Array As _TElement(), ByVal _Index As Int32, ByVal _Substitution As _TElement) As _TElement()
			Try
				_Array(_Index) = _Substitution : Return _Array
			Catch _Ex As Exception : Throw New DSException("@SubstitutedAtIndex (CompilerExtention): " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>(MullNet CompilerExtention) Returns each String, in a ToUpper()-ed state</summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function EachToUpper(ByRef _StringArray As [String]()) As [String]()
			Try
				Return _StringArray.Select(Of String)(Function(_String$) _String.ToUpper()).ToArray()
			Catch _Ex As Exception : Throw New DSException("@EachToUpper (CompilerExtention): " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>(MullNet CompilerExtention) Returns a new Array, wherein _NumElementsToRemoveFromEnd have been chopped off of the end of the _Array</summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function RemoveFromEnd(Of _TElement)(ByVal _Array As _TElement(), ByVal _NumElementsToRemoveFromEnd As UInt32) As _TElement()
			Try

				If _NumElementsToRemoveFromEnd > _Array.Length Then Throw New DSException("The Number-of-elements-to-remove-from-the-end-of-the-array " & _NumElementsToRemoveFromEnd.ToString().InBrackets() & " cannot be greater than the number of elements in the array " & _Array.Length.ToString().InBrackets())
				Return _Array.Take(CInt(_Array.Length - _NumElementsToRemoveFromEnd)).ToArray()

			Catch _Ex As Exception : Throw New DSException("@RemoveFromEnd (CompilerExtention): " & _Ex.Message, _Ex) : End Try
		End Function

	End Module

	REM There exists also the HTTPObjectExtentions Module in WebParts
	Public Module WebObjectExtentions

		''' <summary>
		''' (MullNet CompilerExtention) Writes to, and then Ends the Response Stream
		''' </summary>
		''' <remarks></remarks>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Sub WriteAndEnd(ByRef _ResponseObject As Web.HttpResponse, ByVal _String$)

			Try
				_ResponseObject.Write(_String) : _ResponseObject.SafelyEnd()
			Catch _Ex As Exception When True
				_ResponseObject.SafelyEnd()
			End Try

		End Sub

		''' <summary>
		''' (MullNet CompilerExtention) Clears, Writes to, and then Ends the Response Stream
		''' </summary>
		''' <remarks></remarks>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Sub ClearAndWriteAndEnd(ByRef _ResponseObject As Web.HttpResponse, ByVal _String$)

			Try
				_ResponseObject.Clear() : _ResponseObject.Write(_String) : _ResponseObject.SafelyEnd()
			Catch _Ex As Exception When True
				_ResponseObject.SafelyEnd()
			End Try

		End Sub

		''' <summary>
		''' (MullNet CompilerExtention) Ends the Response Stream, without Throwing the "Thread was being aborted" Exception
		''' </summary>
		''' <remarks></remarks>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Sub [SafelyEnd](ByRef _ResponseObject As Web.HttpResponse)

			Web.HttpContext.Current.Response.Flush() 'Sends all currently buffered output to the client.
			Web.HttpContext.Current.Response.SuppressContent = True	'Gets or sets a value indicating whether to send HTTP content to the client.
			Web.HttpContext.Current.ApplicationInstance.CompleteRequest() 'Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.

			Return

		End Sub

		''' <summary>
		''' (MullNet CompilerExtention) Checks that the Specified QueryString is contained in the current Request Object.
		''' </summary>
		''' <param name="_RequestObject"></param>
		''' <remarks></remarks>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function QueryStringIsSpecified(ByRef _RequestObject As Web.HttpRequest, ByVal _QueryStringKey$) As [Boolean]
			Try

				'Return _RequestObject.QueryString.AllKeys.Contains(_QueryStringKey)

				'A case-insensitive version:

				If (_QueryStringKey = Nothing) OrElse (_QueryStringKey Is Nothing) Then Throw New Web.HttpException("The specified _QueryStringKey Argument [Is Nothing].")

				_QueryStringKey = _QueryStringKey.[ToUpper]()

				For Each _Key$ In _RequestObject.QueryString.AllKeys
					If (_Key = Nothing) OrElse (_Key Is Nothing) Then Throw New Exception("At least one QueryString Key was [Nothing]. The QueryString may have ended with an [&] or may not have had an [=Value] after the Key.")
					If _Key.ToUpper() = _QueryStringKey Then Return True
				Next

				Return False

			Catch _Ex As Exception : Throw New DSException("@QueryStringIsSpecified (CompilerExtention): " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>
		''' (MullNet CompilerExtention) Checks that the Specified PostData Item is contained in the current Request Object.
		''' </summary>
		''' <param name="_RequestObject"></param>
		''' <remarks></remarks>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function PostDataItemIsSpecified(ByRef _RequestObject As Web.HttpRequest, ByVal _PostDataItemKey$) As [Boolean]

			'Return _RequestObject.Form.AllKeys.Contains(_PostDataItemKey$)

			'A case-insensitive version:

			_PostDataItemKey = _PostDataItemKey.[ToUpper]()

			For Each _Key$ In _RequestObject.Form.AllKeys
				If _Key.ToUpper() = _PostDataItemKey Then Return True
			Next

			Return False

		End Function

		''' <summary>
		''' (MullNet CompilerExtention) Checks that the Specified QueryString is contained in the current Request Object, and that it has a Value.
		''' </summary>
		''' <param name="_RequestObject"></param>
		''' <remarks></remarks>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function QueryStringIsSpecifiedAndHasValue(ByRef _RequestObject As Web.HttpRequest, ByVal _QueryStringKey$) As [Boolean]

			Return (_RequestObject.QueryStringIsSpecified(_QueryStringKey) AndAlso (Not String.IsNullOrEmpty(_RequestObject.QueryString(_QueryStringKey))))

		End Function

		''' <summary>
		''' (MullNet CompilerExtention) Checks that the Specified PostData Item is contained in the current Request Object, and that it has a Value.
		''' </summary>
		''' <param name="_RequestObject"></param>
		''' <remarks></remarks>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function PostDataItemIsSpecifiedAndHasValue(ByRef _RequestObject As Web.HttpRequest, ByVal _PostDataItemKey$) As [Boolean]

			Return (_RequestObject.PostDataItemIsSpecified(_PostDataItemKey) AndAlso (Not String.IsNullOrEmpty(_RequestObject.Form(_PostDataItemKey$))))

		End Function

	End Module

	Public Module XMLObjectExtentions

		''' <summary>(MullNet CompilerExtention) Returns a version of the _XElement wrapped in an XML Parent Node with name _OuterNodeName$</summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function WrapIn(ByVal _XElement As XElement, ByVal _OuterNodeName$) As XElement
			Try

				Dim _OuterXElement As New XElement(_OuterNodeName) : _OuterXElement.Add(_XElement)
				Return _OuterXElement

			Catch _Ex As Exception : Throw New DSException("@WrapIn (CompilerExtention): " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>(MullNet CompilerExtention) Returns a new XElement which contains the _XElements</summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function WrapIn(ByVal _XElements As XElement(), ByVal _OuterNodeName$) As XElement
			Try

				Dim _OuterXElement As New XElement(_OuterNodeName) : _XElements.ToList().ForEach(AddressOf _OuterXElement.Add)
				Return _OuterXElement

			Catch _Ex As Exception : Throw New DSException("@WrapIn (Array-Version) (CompilerExtention): " & _Ex.Message, _Ex) : End Try
		End Function

	End Module

	Public Module DocScriptObjectExtentions

		''' <summary>(MullNet CompilerExtention) Produces a simple serialisation of the _Tokens with just their Values and Locations in the form [Raw: "A:", WithLocations: {["A" @14,6], [":" @14,7]}]</summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function GetValuesAndLocationsString(ByVal _Tokens As Runtime.Token()) As [String]
			Try

				Return (
				 "Raw: """ & String.Join("", _Tokens.Select(Of String)(Function(_Token As Runtime.Token) _Token.Value)) _
				 & _
				""", WithLocations: " & String.Join(", ", _Tokens.Select(Of String)(Function(_Token As Runtime.Token) (""""c & _Token.Value & """ (@" & _Token.LocationInSource.ToShortString() & ")"c).InSquares())).InCurlies() _
				 ).InSquares().Replace(vbCrLf, "{LineEnd}")

			Catch _Ex As Exception : Throw New DSException("@GetValuesAndLocationsString (CompilerExtention): " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>(MullNet CompilerExtention) Concatenates the Tokens' Values back together</summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function ReconstructIntoSource(ByVal _Tokens As Runtime.Token(), Optional ByVal _BetweenTokensSeperator$ = "") As [String]
			Try

				Return String.Join(_BetweenTokensSeperator, From _Token As Runtime.Token In _Tokens Select _Token.Value)

			Catch _Ex As Exception : Throw New DSException("@GetValuesAndLocationsString (CompilerExtention): " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>(MullNet CompilerExtention) Returns weather or not the _TPV is satisfied by the _Tokens</summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function SatisfiesTPV(ByVal _Tokens As Runtime.Token(), ByVal _TPV As Runtime.TokenPatternValidation.TokensToPatternsValidator) As [Boolean]
			Return _TPV.IsSatisfiedBy(_Tokens)
		End Function

		''' <summary>(MullNet CompilerExtention) Attempts to coerce the _InputIDV into an IDV of type _TSoughtIDV</summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function Coerce(Of _TSoughtIDV As {DocScript.Language.Variables.IDataValue, Class})(ByVal _InputIDV As DocScript.Language.Variables.IDataValue) As _TSoughtIDV
			Return DocScript.Language.Variables.TypeCoercion.CoerceIDV_IntoTargetIDVType(Of _TSoughtIDV)(_InputIDV)
		End Function

	End Module

	''' <summary>
	''' Non-CompilerExtention Useful Methods which need to be accessible without Namespace qualification
	''' Some of these can't be CompilerExtentions E.g. because they take in a ParamArray as the first Argument
	''' </summary>
	Public Module UsefulMethods

		''' <summary>
		''' (MullNet UsefulMethod) Returns a _TElement()() from the Array of _TElement Arrays
		''' </summary>
		Public Function Get2DArray(Of _TElement)(ByVal ParamArray _Array As _TElement()()) As _TElement()()
			Return _Array
		End Function

		''' <summary>(MullNet UsefulMethod) Writes the _Text to the _Console in the specified _ConsoleColour</summary>
		Public Sub ConsoleWriteLineInColour(ByVal _Text$, ByVal _ConsoleColour As ConsoleColor)

			Dim _PreviousConsoleColour As ConsoleColor = Console.ForegroundColor
			Console.ForegroundColor = _ConsoleColour
			Console.WriteLine(_Text)
			Console.ForegroundColor = _PreviousConsoleColour

		End Sub

		''' <summary>(MullNet UsefulMethod) Writes the _Text to the _Console in the specified _ForegroundColour, and using the specified _BackgroundColour</summary>
		Public Sub ConsoleWriteLineInColour(ByVal _Text$, ByVal _ForegroundColour As ConsoleColor, ByVal _BackgroundColour As ConsoleColor)

			Dim _PreviousForegroundColour As ConsoleColor = Console.ForegroundColor
			Dim _PreviousBackgroundColour As ConsoleColor = Console.BackgroundColor

			Console.ForegroundColor = _ForegroundColour
			Console.BackgroundColor = _BackgroundColour
			Console.WriteLine(_Text)

			Console.ForegroundColor = _PreviousForegroundColour
			Console.BackgroundColor = _PreviousBackgroundColour

		End Sub

		''' <summary>(MullNet UsefulMethod) Writes the _Text to the _Console in the specified _ConsoleColour, without a LineBreak on the end</summary>
		''' <remarks>DSCompilation's DSPackage.VB mechanism releis on this method!</remarks>
		Public Sub ConsoleWriteInColour(ByVal _Text$, ByVal _ConsoleColour As ConsoleColor)

			Dim _PreviousConsoleColour As ConsoleColor = Console.ForegroundColor
			Console.ForegroundColor = _ConsoleColour
			Console.Write(_Text)
			Console.ForegroundColor = _PreviousConsoleColour

		End Sub

		''' <summary>(MullNet UsefulMethod) Writes the _Text to the _Console in the specified _ForegroundColour (without a LineBreak on the end), and using the specified _BackgroundColour</summary>
		Public Sub ConsoleWriteInColour(ByVal _Text$, ByVal _ForegroundColour As ConsoleColor, ByVal _BackgroundColour As ConsoleColor)

			Dim _PreviousForegroundColour As ConsoleColor = Console.ForegroundColor
			Dim _PreviousBackgroundColour As ConsoleColor = Console.BackgroundColor

			Console.ForegroundColor = _ForegroundColour
			Console.BackgroundColor = _BackgroundColour
			Console.Write(_Text)

			Console.ForegroundColor = _PreviousForegroundColour
			Console.BackgroundColor = _PreviousBackgroundColour

		End Sub

		''' <summary>
		''' (MullNet UsefulMethod)
		''' Ensures that for each opening component in the _Pairs, there is an instance of the corrosponding closing component.
		''' Also makes sure that the _Pairs components are opened in a ballenced fashion.
		''' E.g. if done with brackets, then "([])" would be valid, whereas "([)]" would not be (even though there are the same number of brackets and squares in the latter).
		''' 
		''' Example Call: ContainsWellBalencedPairs(Of String)({"Open", "Null", "Null", "Close", "Null", "Close"}, New Tuple(Of String, String)("Open", "Close"))
		''' <example>ContainsWellBalencedPairs(Of String)({"Open", "Null", "Null", "Close", "Null", "Close"}, New Tuple(Of String, String)("Open", "Close"))</example>
		''' 
		''' How this Function works:
		''' 'Declare a _TItems Stack
		''' For Each Item In _JustTheRelevantItems
		''' If the Item is an Opening Component (_Pair.Item1) then push it onto stack
		''' If the Item is a Closing Component (_Pair.Item2) then pop from stack and if the popped Item is the matching Opening Component then fine, but otherwise the Items are not balanced
		''' After complete traversal, if there is an Opening Component left in stack then the source is not balanced
		''' </summary>
		''' <typeparam name="_TItems">The DataType of each Item in the _AllItems, and of a component in the _Pairs</typeparam>
		''' <param name="_AllItems">A List of _TItems, not all of which have to be Items seen in the _Pairs. E.g. could be all source token even if you just want to validate the balencedness of brackets.</param>
		''' <param name="_Pairs">The Pairs which have to be balenced. Item1 = the Opening Component E.g. "("; Item2 = the Closing Component E.g. ")"</param>
		''' <returns>Weather or not _AllItems contained well-balenced Pairs</returns>
		''' <remarks>Implemented 24-08-2022 so that Brackets and Statements may make use of it</remarks>
		Public Function ContainsWellBalencedPairs(Of _TItems)(ByVal _AllItems As _TItems(), ByVal ParamArray _Pairs As Tuple(Of _TItems, _TItems)()) As Boolean
			Try

				'Declare a _TItems Stack
				'For Each Item In _JustTheRelevantItems
				'	If the Item is an Opening Component (_Pair.Item1) then push it onto stack
				'	If the Item is a Closing Component (_Pair.Item2) then pop from stack and if the popped Item is the matching Opening Component then fine, but otherwise the Items are not balanced
				'After complete traversal, if there is an Opening Component left in stack then the source is not balanced

				'Contains only Items which are also present in the _Pairs (E.g. only the Brackets [] {} <> out of all the Source Tokens)
				Dim _JustTheRelevantItems As _TItems() = (From _Item As _TItems In _AllItems Where (_Pairs.Any(Function(_Pair As Tuple(Of _TItems, _TItems)) (_Pair.Item1.Equals(_Item)) OrElse (_Pair.Item2.Equals(_Item)))) Select _Item).ToArray()
				Dim _ItemsStack As New Stack(Of _TItems)()
				Dim _NoOpeningComponents, _NoClosingComponents As UInt32

				For Each _Item As _TItems In _JustTheRelevantItems

					'Was advised to do this by VS 2010. Sure.
					Dim _LambdaCopyOf_Item As _TItems = _Item

					REM If we have an Opening Component, Push() it onto the Stack
					If _Pairs.Any(Function(_Pair As Tuple(Of _TItems, _TItems)) _Pair.Item1.Equals(_LambdaCopyOf_Item)) Then

						_ItemsStack.Push(_Item) : _NoOpeningComponents += 1UI

						REM If we have a Closing Component, find out if _ItemsStack.Pop() produces the corrosponding Opening Component
					ElseIf _Pairs.Any(Function(_Pair As Tuple(Of _TItems, _TItems)) _Pair.Item2.Equals(_LambdaCopyOf_Item)) Then

						'Get the Pair which contains the corrosponding Opening Component for out Closing Component
						Dim _Pair_WhereforWeHaveClosingComponent As Tuple(Of _TItems, _TItems) = _
						 _Pairs.First(Function(_Pair As Tuple(Of _TItems, _TItems)) _Pair.Item2.Equals(_LambdaCopyOf_Item))

						'Now see if the Pop() produces the same Opening Component as we have in our Pair
						'(Additionally, if the Stack is empty, then the Items aren't well-balenced, because we just hit a Closing Component which didn't follow a previous corrosponding Opening Component)
						If (_ItemsStack.Count = 0) OrElse (Not _ItemsStack.Pop().Equals(_Pair_WhereforWeHaveClosingComponent.Item1)) Then Return False
						_NoClosingComponents += 1UI

					Else : Throw New DSValidationException("An Item was not recognised as either an Opening or Closing Component", "The _Item's ToString() is: " & _Item.ToString())
					End If

				Next

				REM Now determine if there is anything left on the Stack. The only things on there could ever be Opening Components, because that's all we ever Push()
				If Not (_ItemsStack.Count = 0) Then Return False

				REM If we're here, then all Opening and Closing Components must have been balenced, because we haven't yet Return'd
				LogParsingMessage(String.Format("Determined that {0} relevant Item(s) out of {1} total Item(s) were well-balenced with {2} Opening Component(s) and {3} Closing Component(s) from {4} Pair(s)", _JustTheRelevantItems.Length.ToString(), _AllItems.Length.ToString(), _NoOpeningComponents.ToString(), _NoClosingComponents.ToString(), _Pairs.Length.ToString()), LogEvent.DSEventSeverity.Verbose)
				Return True

			Catch _Ex As Exception : Throw New DSException("@IsStackBalenced(Of " & GetType(_TItems).Name & "): " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>(MullNet UsefulMethod) For Procuring a Console if DS Console Logging has been selected</summary>
		<System.Runtime.InteropServices.DllImport("Kernel32.dll", SetLastError:=True)>
		Public Function AllocConsole() As <System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)> Boolean
		End Function

		''' <summary>(MullNet UsefulMethod) Returns a random String using the Chars 0123456789ABCDEFGHIJKLNMOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz</summary>
		Public Function GetRandomString(Optional ByVal _WantedLength As UInt16 = 8US) As String

			Dim _StringToReturn$ = String.Empty
			Dim _PossibleCharacters As Char() = "0123456789ABCDEFGHIJKLNMOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray()
			Dim _RandomNumber As New Random(Seed:=DateTime.Now.Millisecond)

			For _CurrentCharacter% = 1 To _WantedLength Step +1
				_StringToReturn &= _PossibleCharacters(_RandomNumber.Next(0, _PossibleCharacters.Length - 1))
			Next

			Return _StringToReturn

		End Function

		''' <summary>(MullNet UsefulMethod) Uses an STA Thread (Message-Pumping) to display a SaveFileDialog, and returns the Selected File, or NOTHING.</summary>
		''' <param name="_TargetFileExtentions">E.g. ("Hypertext Markup", "HTML") or ("Plaintext", "TXT"). The [All Files (*.*)] Option is automatically added by this method. Instanciation: New Tuple(Of String, String)("All Files", "*")</param>
		Public Function RequestFilePath_WithSaveFileDialog(ByVal _FileDialog_Title$, ByVal ParamArray _TargetFileExtentions As Tuple(Of String, String)()) As IO.FileInfo
			Try

				Dim _ProvidedFilePath As [String] = Nothing

				REM ↓ E.g.	Hypertext Markup (*.HTML)|*.HTML|All files (*.*)|*.*
				Dim _FileDialog_Filter As [String] = String.Join("|", ( _
				 From _DescriptionAndExtention As Tuple(Of [String], [String])
				 In _TargetFileExtentions.Concat({New Tuple(Of String, String)("All Files", "*")})
				 Select String.Format("{0} (*.{1})|*.{1}", _DescriptionAndExtention.Item1, _DescriptionAndExtention.Item2)
				).ToArray())

				Dim _STAThread As New Threading.Thread(
				Sub()
					Dim _SaveFileDialog As New Windows.Forms.SaveFileDialog() _
					With {.Title = _FileDialog_Title, .Filter$ = _FileDialog_Filter$, .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)}
					If _SaveFileDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then _ProvidedFilePath = _SaveFileDialog.FileName
				End Sub)

				_STAThread.SetApartmentState(Threading.ApartmentState.STA)
				_STAThread.Start()
				_STAThread.Join()

				Return If(_ProvidedFilePath = Nothing, Nothing, New IO.FileInfo(_ProvidedFilePath))

			Catch _Ex As Exception : Throw New DSException("@RequestFolderPath: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>(MullNet UsefulMethod) Returns the directory of the current exe</summary>
		Public Function GetExecutingAssemblyDirectory() As IO.DirectoryInfo

			Dim _CodeBase$ = System.Reflection.Assembly.GetExecutingAssembly().CodeBase 'Looks like: "file:///C:/Windows/assembly/GAC_MSIL/Microsoft.VisualStudio.HostingProcess.Utilities/10.0.0.0__b03f5f7f11d50a3a/Microsoft.VisualStudio.HostingProcess.Utilities.dll"
			Dim _UriBuilder As New UriBuilder(_CodeBase)
			Return New IO.DirectoryInfo(IO.Path.GetDirectoryName(Uri.UnescapeDataString(_UriBuilder.Path)))

		End Function

		''' <summary>(MullNet UsefulMethod) Executes the process, capturing and returning the CLI Output. Returns both the STDOUT and STDERR.</summary>
		Public Function RunProcess_AndReturnCommandLineOutput(ByVal _ProcessImageFile As IO.FileInfo, Optional ByVal _Process_CLAs$ = "") As String
			Try
				With New System.Diagnostics.Process()

					.StartInfo.UseShellExecute = False
					.StartInfo.FileName = _ProcessImageFile.FullName
					.StartInfo.Arguments = _Process_CLAs
					.StartInfo.RedirectStandardOutput = True
					.StartInfo.RedirectStandardError = True

					.Start()
					Dim _ProcessOutput$ = .StandardOutput.ReadToEnd()
					Dim _ErrorOutput$ = .StandardError.ReadToEnd()
					.WaitForExit()

					If String.IsNullOrEmpty(_ProcessOutput) AndAlso (Not String.IsNullOrEmpty(_ErrorOutput)) Then : Return _ErrorOutput
					ElseIf String.IsNullOrEmpty(_ErrorOutput) AndAlso (Not String.IsNullOrEmpty(_ProcessOutput)) Then : Return _ProcessOutput
					ElseIf (Not String.IsNullOrEmpty(_ErrorOutput)) AndAlso (Not String.IsNullOrEmpty(_ProcessOutput)) Then : Return _ProcessOutput & vbCrLf & _ErrorOutput
					ElseIf String.IsNullOrEmpty(_ErrorOutput) AndAlso String.IsNullOrEmpty(_ProcessOutput) Then : Return String.Empty
					Else : Throw New Exception("Unaccounted-for Process Error/Output eventuality")
					End If

				End With
			Catch _Ex As Exception : Throw New DSException("@RunProcess_AndReturnCommandLineOutput: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>(MullNet UsefulMethod) Executes the process, capturing and returning the CLI Output. The ExitCode can be resolved from the input Process Object.</summary>
		Public Function RunProcess_AndReturnCommandLineOutput(ByRef _InputProcess As Process) As [String]
			Try
				With _InputProcess

					.StartInfo.UseShellExecute = False
					.StartInfo.RedirectStandardOutput = True
					.StartInfo.RedirectStandardError = True

					.Start()
					Dim _ProcessOutput$ = .StandardOutput.ReadToEnd()
					Dim _ErrorOutput$ = .StandardError.ReadToEnd()
					.WaitForExit()

					Dim _Process_StdOut_AndStdErr$
					If String.IsNullOrEmpty(_ProcessOutput) AndAlso (Not String.IsNullOrEmpty(_ErrorOutput)) Then : _Process_StdOut_AndStdErr$ = _ErrorOutput
					ElseIf String.IsNullOrEmpty(_ErrorOutput) AndAlso (Not String.IsNullOrEmpty(_ProcessOutput)) Then : _Process_StdOut_AndStdErr$ = _ProcessOutput
					ElseIf (Not String.IsNullOrEmpty(_ErrorOutput)) AndAlso (Not String.IsNullOrEmpty(_ProcessOutput)) Then : _Process_StdOut_AndStdErr$ = _ProcessOutput & vbCrLf & _ErrorOutput
					ElseIf String.IsNullOrEmpty(_ErrorOutput) AndAlso String.IsNullOrEmpty(_ProcessOutput) Then : _Process_StdOut_AndStdErr$ = String.Empty
					Else : Throw New Exception("Unaccounted-for Process Error/Output eventuality")
					End If

					Return _Process_StdOut_AndStdErr

				End With
			Catch _Ex As Exception : Throw New DSException("@RunProcess_AndReturnCommandLineOutput: " & _Ex.Message, _Ex) : End Try
		End Function

	End Module

End Namespace