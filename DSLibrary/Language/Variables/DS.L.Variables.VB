Namespace Language.Variables

	''' <summary>The Non-Generic Interface for all actual Values of an Expr or Variable</summary>
	Public Interface IDataValue : End Interface

	''' <summary>The Generic Interface for values of an Expr or Variable. Takes in the DotNetType for which the interface is a wrapper</summary>
	Public Interface IDataValue(Of TDotNetType) : Inherits IDataValue
		Property Value As TDotNetType
		ReadOnly Property NullValue As TDotNetType
	End Interface

#Region "IDataValue Implementations"

	''' <summary>Represents a True or False value</summary>
	Public Class DSBoolean : Implements IDataValue(Of Boolean), Runtime.SymbolTable.ISymbolTableValue

		Public Property Value As Boolean Implements IDataValue(Of Boolean).Value

		Public Shared ReadOnly Property InstanceWithNullValue As DSBoolean
			Get
				Return (New DSBoolean(DSBoolean.NullValue_))
			End Get
		End Property

		Private Const NullValue_ As Boolean = False
		Public ReadOnly Property NullValue As Boolean Implements IDataValue(Of Boolean).NullValue
			Get
				Return DSBoolean.NullValue_
			End Get
		End Property

		Public Sub New(ByVal _Value As Boolean)
			Me.Value = _Value
		End Sub

		REM Sideloading Constructor
		Public Shared Function FromToken(ByVal _Token As Runtime.Token) As DSBoolean
			If Not _Token.Type = Runtime.Token.TokenType.BooleanLiteral Then Throw New DSValidationException("A DSBoolean could not be constructed because the specified _Token was not of the type [Runtime.Token.TokenType.BooleanLiteral].", "_Token")
			Select Case _Token.Value.ToUpper()
				Case Language.Constants.BooleanTrueLiteral$ : Return New DSBoolean(True)
				Case Language.Constants.BooleanFalseLiteral$ : Return New DSBoolean(False)
				Case Else : Throw New DocScript.Exceptions.DSValidationException("The BooleanLiteral Token passed to DSBoolean\FromToken() did not have a valid Literal value.", _Token.Value)
			End Select
		End Function

		''' <summary>Returns a string to represent the Literal for the IDataValue</summary>
		Public Overrides Function ToString() As String
			Return Me.Value.ToString()
		End Function

	End Class

	''' <summary>Represents a Number with a Value and a Base. Can be from three different forms of Literal</summary>
	Public Class DSNumber : Implements IDataValue(Of [Double]), Runtime.SymbolTable.ISymbolTableValue

		Protected Const NumericLiteralRegExp_Integer_$ = "^(\d{1,10})$"	'					E.g.	12
		Protected Const NumericLiteralRegExp_Decimal_$ = "^(\d{1,10}\.\d{1,4})$"	'		E.g.	12.9
		Protected Const NumericLiteralRegExp_Based_$ = "^([A-Za-z0-9]{1,10}_\d{1,3})$" '	E.g.	12_3

		Protected Value_ As Double
		''' <summary>The Base-10 Value of the Number</summary>
		Public Property Value As Double Implements IDataValue(Of Double).Value
			Get
				Return Me.Value_
			End Get
			Set(ByVal _NewValue As Double)
				Me.Value_ = _NewValue
			End Set
		End Property

		Public Shared ReadOnly Property InstanceWithNullValue As DSNumber
			Get
				Return (New DSNumber(DSNumber.NullValue_))
			End Get
		End Property

		Private Const NullValue_ As Double = 0.0#
		Public ReadOnly Property NullValue As Double Implements IDataValue(Of Double).NullValue
			Get
				Return DSNumber.NullValue_
			End Get
		End Property

		Public Sub New(ByVal _Value As Double)
			Me.Value = _Value
		End Sub

		REM Sideloading Constructor
		Public Shared Function FromToken(ByVal _Token As Runtime.Token) As DSNumber

			If Not _Token.Type = Runtime.Token.TokenType.NumericLiteral Then Throw New DSValidationException("A DSNumber could not be constructed because the specified _Token was not of the type [Runtime.Token.TokenType.NumericLiteral].", "_Token")

			REM Valid Numeric Literal Forms:
			REM		104
			REM		104.23
			REM		104_5

			If _Token.Value.MatchesRegEx(DSNumber.NumericLiteralRegExp_Integer_) Then

				REM E.g. 12
				Return New DSNumber(CDbl(_Token.Value))

			ElseIf _Token.Value.MatchesRegEx(DSNumber.NumericLiteralRegExp_Decimal_) Then

				REM E.g. 12.9
				Return New DSNumber(CDbl(_Token.Value))

			ElseIf _Token.Value.MatchesRegEx(DSNumber.NumericLiteralRegExp_Based_) Then

				REM E.g. 12_3

				Dim _Value$ = _Token.Value.Split(Language.Constants.BasedNumericLiteral_ValueAndBaseSeperator).ElementAt(0)
				Dim _Base_String$ = _Token.Value.Split(Language.Constants.BasedNumericLiteral_ValueAndBaseSeperator).ElementAt(1)

				If Convert.ToUInt64(_Base_String) > Language.Constants.HighestPermittedNumericLiteralBase Then Throw New DSException(String.Format("The specified Base {0} for the DSNumber is above the Highest Permitted Numeric Literal Base of {1}", _Base_String, Language.Constants.HighestPermittedNumericLiteralBase.ToString()))
				Dim _Base As [Byte] = Convert.ToByte(_Base_String)

				Dim _BasedNumber As New BasedNumberSupport.BasedNumber(_Value, _Base)
				Return New DSNumber(CDbl(_BasedNumber.AsBase10))

			Else : Throw New DSValidationException("The Value of the _Token passed to DSNumber\FromToken() was not of a recognised Numeric Literal Type", _Token.Value)
			End If

		End Function

#Region "DS-TC Utilities"

		''' <summary>
		''' Used in DS-TC for String→Number Conversion
		''' NOTE: This function will automatically strip the Input String of Quotes on either side, if it has them.
		''' </summary>
		Public Shared ReadOnly Property IsValidNumericLiteralString(ByVal _String$) As Boolean
			Get
				Try

					LogLexingMessage(String.Format("Determining if String (in squares) {0} is a valid Numeric Literal", _String.InSquares()))

					_String = DSNumber.StripQuotesIfPresent(_String) 'Fine, because we're ByVal

					Dim _ValidNumericLiteralString_Patterns$() = {
					 DSNumber.NumericLiteralRegExp_Integer_,
					 DSNumber.NumericLiteralRegExp_Decimal_,
					 DSNumber.NumericLiteralRegExp_Based_
					}

					Return _ValidNumericLiteralString_Patterns.Any(Function(_RegExp$) _String.MatchesRegEx(_RegExp))

				Catch _Ex As Exception : Throw New DSException("@IsValidNumericLiteralString: " & _Ex.Message, _Ex) : End Try
			End Get
		End Property

		''' <summary>
		''' Used in DS-TC for String→Number Conversion
		''' NOTE: This function will automatically strip the Input String of Quotes on either side, if it has them.
		''' </summary>
		Public Shared Function FromNumericLiteralString(ByVal _NumericLiteralString$) As DSNumber
			Try

				LogLexingMessage(String.Format("Sideloading DSNumber from Numeric Literal String (in squares) {0}", _NumericLiteralString.InSquares()))

				_NumericLiteralString = DSNumber.StripQuotesIfPresent(_NumericLiteralString) 'Fine, because we're ByVal

				If Not DSNumber.IsValidNumericLiteralString(_NumericLiteralString) Then Throw New DSValidationException("The DSNumber could not be constructed from the _NumericLiteralString, because it was not a valid Numeric Literal String", _NumericLiteralString)
				Return DSNumber.FromToken(New Runtime.Token(_NumericLiteralString, Runtime.Token.TokenType.NumericLiteral, 0, 0))

			Catch _Ex As Exception : Throw New DSException("@FromNumericLiteralString: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>
		''' Automatically strips the Input String of Quotes on either side if it has them both, and if the Input String is *at-least* 3 chars long.
		''' </summary>
		Public Shared Function StripQuotesIfPresent(ByVal _InputString$) As Global.System.String

			LogLexingMessage(String.Format("Determining if this StringLiteral-String needs quotes stripped (in squares): {0}...", _InputString.InSquares()))

			If (_InputString.Length >= 3) AndAlso _InputString.StartsWith(Constants.StringLiteralStartEndChar) AndAlso _InputString.EndsWith(Constants.StringLiteralStartEndChar) Then
				LogLexingMessage(String.Format("...It does; removing first and last Char to leave string of length {0}...", (_InputString.Length - 2).ToString()))
				Return _InputString.RemoveFirstCharacter().RemoveLastCharacter().Output(AddressOf LogLexingMessage, "...Done; stripped quotes to leave:")
			Else
				LogLexingMessage(String.Format("...It does not; returning the String as it is: {0}", _InputString.InSquares()))
				Return _InputString
			End If

		End Function

#End Region

		Public ReadOnly Property AsBasedNumber As DocScript.Language.Variables.BasedNumberSupport.BasedNumber
			Get
				Return New BasedNumber(Me.Value.ToString(), 10)
			End Get
		End Property

		''' <summary>Returns a string to represent the Literal for the IDataValue</summary>
		Public Overrides Function ToString() As String
			Return Me.Value.ToString(format:="G", provider:=System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).Replace("-"c, DocScript.Language.Constants.OperatorChar_NumericalInvertPolarity)
		End Function

	End Class

	''' <summary>Represents a String of Chars</summary>
	Public Class DSString : Implements IDataValue(Of String), Runtime.SymbolTable.ISymbolTableValue

		Public Property Value As [String] Implements IDataValue(Of String).Value

		Public Shared ReadOnly Property InstanceWithNullValue As DSString
			Get
				Return (New DSString(DSString.NullValue_))
			End Get
		End Property

		Private Const NullValue_ As String = ""
		Public ReadOnly Property NullValue As [String] Implements IDataValue(Of String).NullValue
			Get
				Return DSString.NullValue_
			End Get
		End Property

		''' <summary>If the _Value contains the StringLIteralStartEndChar, then instances of this Char will be replaced with the Language.Constants.StringLIteralStartEndChar_DSStringValueReplacementChar</summary>
		Public Sub New(ByVal _Value As String)

			If _Value.Contains(Constants.StringLiteralStartEndChar) Then
				LogLexingMessage(String.Format("The _Value passed to the DSString Constructor contained the StringLiteralStartEndChar ({0}), and will be replaced with the StringLIteralStartEndChar_DSStringValueReplacementChar ({1})", Language.Constants.StringLiteralStartEndChar.ToString(), Language.Constants.StringLIteralStartEndChar_DSStringValueReplacementChar.ToString()), LogEvent.DSEventSeverity.Warning)
				_Value = _Value.Replace(Constants.StringLiteralStartEndChar, Constants.StringLIteralStartEndChar_DSStringValueReplacementChar)
			End If

			Me.Value = _Value

		End Sub

		REM Sideloading Constructor
		Public Shared Function FromToken(ByVal _Token As Runtime.Token) As DSString

			REM We must have a Token of the correct sort
			If Not _Token.Type = Runtime.Token.TokenType.StringLiteral Then Throw New DSValidationException("A DSNumber could not be constructed because the specified _Token was not of the type [Runtime.Token.TokenType.StringLiteral].", "_Token")

			REM There must be at-least 2 chars: ""
			If Not ((_Token.Value.Length >= 2) AndAlso (_Token.Value.StartsWith(Constants.StringLiteralStartEndChar.ToString())) AndAlso (_Token.Value.EndsWith(Constants.StringLiteralStartEndChar.ToString()))) Then Throw New DSValidationException("A DSString could not be constructed because the _Token.Value passed to DSString\FromToken() did not begin and end with a StringLiteralStartEndChar, to make it a valid StringLiteral", _Token.Value)

			REM Skip the leading and trailing "
			Return New DSString(New String(Value:=_Token.Value.ToCharArray().Skip(1).Take(_Token.Value.Length - 2).ToArray()))
			'↑ I'm not using the DSNumber.StripQuotesIfPresent() Function, because that needs the string to be at-least 3 chars long.

		End Function

		''' <summary>Returns a string to represent the Literal for the IDataValue</summary>
		Public Overrides Function ToString() As String
			Return Constants.StringLiteralStartEndChar & Me.Value & Constants.StringLiteralStartEndChar
		End Function

	End Class

	''' <summary>
	''' Represents a List of DSString, DSNumber, or DSBoolean Objects
	''' NOTE: This Class does not Implement IDV(Of TElement) because that would create a member [Value As TElement], and out Value needs to be As List(Of TElement)
	''' </summary>
	Public Class DSArray(Of TElement As {IDataValue, Class}) : Implements IDataValue, Runtime.SymbolTable.ISymbolTableValue

		Public Shared ReadOnly Property InstanceWithNullValue As DSArray(Of TElement)
			Get
				Return (New DSArray(Of TElement)(DSArray(Of TElement).NullValue_.ToArray()))
			End Get
		End Property

		Public ReadOnly Property NullValue As List(Of TElement)
			Get
				Return DSArray(Of TElement).NullValue_
			End Get
		End Property

		Private Shared ReadOnly Property NullValue_ As List(Of TElement)
			Get
				Select Case GetType(TElement)
					Case GetType(DSString) : Return CType(CObj(New List(Of DSString)()), List(Of TElement))
					Case GetType(DSNumber) : Return CType(CObj(New List(Of DSNumber)()), List(Of TElement))
					Case GetType(DSBoolean) : Return CType(CObj(New List(Of DSBoolean)()), List(Of TElement))
					Case Else : Throw New DSValidationException("The DSArray could not return a NullValue, because the Element Type was un-[accounted-for]", GetType(TElement).Name)
				End Select
			End Get
		End Property

		Public Elements As List(Of TElement)

		Public Sub New(ByVal _Elements As TElement())

			'Will this work?
			If GetType(TElement) = GetType(DSArray(Of IDataValue)) Then Throw New DSValidationException("A DSArray cannot have the Element Type DSArray. This would be recursive and DocScript only supports one-dimensional Arrays", "DSArray\New()\TElement")
			If GetType(TElement) = GetType(IDataValue) Then Throw New DSValidationException("A DSArray cannot have the Element Type IDataValue. DocScript only supports Arrays whose elements are all of the same Type", "DSArray\New()\TElement")

			Me.Elements = _Elements.ToList()

		End Sub

		''' <summary>Returns a string to represent the DSArray in the format {Item1, Item2, Item3}</summary>
		Public Overrides Function ToString() As String
			Return "(Length: " & Me.Elements.Count.ToString() & ") " & String.Join(", ", Me.Elements.Select(Of String)(Function(_Element As TElement) _Element.ToString())).InCurlies()
		End Function

	End Class

	''' <summary>Represents the absence of Data (Only valid as a Return Type for Functions)</summary>
	Public Class Void : Implements IDataValue

		Public Sub New()
			Throw New DSInvalidCallException("A Void Object cannot be instanciated. It is only for use as a Placeholder Type", "DocScript\Language\Variables\Void\New()")
		End Sub

	End Class

#End Region

End Namespace