Namespace Exceptions

	''' <summary>The Base Execption Class whence all other DocScript Exceptions derive.</summary>
	Public Class DSException : Inherits System.Exception

		''' <summary>Attempts to Log the Exception Message, indicating weather this occoured successfully or not in the Message Property</summary>
		Public Sub New(ByVal _Message$)
			MyBase.New(
			DSException.FormatMessage(
			   (Function(_LogSubmissionResult As Logging.LogEventSubmissionResult) As String
					Return If(_LogSubmissionResult.WasSuccessfull, ("(Logged) " & _Message), ("(Unlogged: " & _LogSubmissionResult.GeneratedException.Message & ") " & _Message))
				End Function).Invoke(Logging.SubmitLogEvent("Exception: " & _Message, LogEvent.DSEventSeverity.Error, LogEvent.DSEventCatagory.Unspecified))
			 )
			)
		End Sub

		''' <summary>Attempts to Log the Exception Message, indicating weather this occoured successfully or not in the Message Property</summary>
		Public Sub New(ByVal _Message$, ByVal _InnerException As System.Exception)
			MyBase.New(
			  DSException.FormatMessage(
			  (Function(_LogSubmissionResult As Logging.LogEventSubmissionResult) As String
				   Return If(_LogSubmissionResult.WasSuccessfull, ("(Logged) " & _Message), ("(Unlogged: " & _LogSubmissionResult.GeneratedException.Message & ") " & _Message))
			   End Function).Invoke(Logging.SubmitLogEvent("Exception: " & _Message, LogEvent.DSEventSeverity.Error, LogEvent.DSEventCatagory.Unspecified))
			  ),
			  _InnerException
			)
		End Sub

		''' <summary>Inserts a LineBreak after each Colon (:), if this hasn't already been done.</summary>
		Public Shared Function FormatMessage$(ByVal _PotentiallyUnformattedMessage$)
			'If _PotentiallyUnformattedMessage.Contains(":" & vbCrLf) Then Return _PotentiallyUnformattedMessage
			'Return _PotentiallyUnformattedMessage.Replace(":", ":" & vbCrLf)
			Return _PotentiallyUnformattedMessage.Replace(":", vbCrLf)
		End Function

	End Class

	''' <summary>Used for Input and Argument Validation</summary>
	Public Class DSValidationException : Inherits DocScript.Exceptions.DSException

		Public ReadOnly InvalidContent As String

		''' <summary>Calls the Constructor for DSException which attempts to Log the Message</summary>
		Public Sub New(ByVal _Message$, ByVal _InvalidContent$)
			MyBase.New((GetType(DSValidationException).Name.InSquares()) & " "c & _Message & ". Invalid Content (in []): " & _InvalidContent.InSquares() & "."c)
			Me.InvalidContent = _InvalidContent
		End Sub

	End Class

	''' <summary>Used for Token Validation</summary>
	Public Class DSTokenValidationException : Inherits DocScript.Exceptions.DSException

		Public ReadOnly InvalidTokens As Runtime.Token()

		''' <summary>Calls the Constructor for DSException which attempts to Log the Message</summary>
		Public Sub New(ByVal _Message$, ByVal _InvalidTokens As Runtime.Token())
			MyBase.New((GetType(DSTokenValidationException).Name.InSquares()) & " "c & _Message & ". The Invalid Tokens were: " & String.Join(", ", _InvalidTokens.Select(Of String)(Function(_Token As Runtime.Token) _Token.ToString())).InCurlies() & "."c)
			Me.InvalidTokens = _InvalidTokens
		End Sub

	End Class

	''' <summary>Used for Null Referances and other Objects with should be set to a non-null value (but aren't)</summary>
	Public Class DSNullException : Inherits DocScript.Exceptions.DSException

		Public ReadOnly NullObjectName As String

		''' <summary>Calls the Constructor for DSException which attempts to Log the Message</summary>
		Public Sub New(ByVal _Message$, ByVal _NullObjectName$)
			MyBase.New((GetType(DSNullException).Name.InSquares()) & " "c & _Message & ". The Null Object Name was: " & _NullObjectName.InSquares() & ".")
			Me.NullObjectName = _NullObjectName
		End Sub

	End Class

	''' <summary>Used when an call is made to a method that should never be called (e.g. one implemented from an interface that isn't needed on the implementer object)</summary>
	Public Class DSInvalidCallException : Inherits DocScript.Exceptions.DSException

		Public ReadOnly InvalidMethodName As String

		''' <summary>Calls the Constructor for DSException which attempts to Log the Message</summary>
		Public Sub New(ByVal _Message$, ByVal _InvalidMethodName$)
			MyBase.New((GetType(DSInvalidCallException).Name.InSquares()) & " "c & _Message & ". The Invalid Method's Name was: " & _InvalidMethodName.InSquares() & ".")
			Me.InvalidMethodName = _InvalidMethodName
		End Sub

	End Class

	''' <summary>Used when the order of the LBL is invalid in an Expression</summary>
	Public Class DSLBLValidationException : Inherits DocScript.Exceptions.DSException

		Public ReadOnly InvalidLBLComponents As DocScript.Language.Expressions.IExpression()
		Public ReadOnly RuleBroken As String

		''' <summary>Calls the Constructor for DSException which attempts to Log the Message</summary>
		Public Sub New(ByVal _Message$, ByVal _RuleBroken$, ByVal _InvalidLBLComponents As DocScript.Language.Expressions.IExpression())
			MyBase.New((GetType(DSLBLValidationException).Name.InSquares()) & " "c & _Message & ". The Invalid Pattern Type was: " & _RuleBroken.InSquares() & ". The Invalid LBL Components were: " & String.Join(", ", _InvalidLBLComponents.Select(Of String)(Function(_LBLComponent As Language.Expressions.IExpression) _LBLComponent.GetLBLComponentString())).InCurlies() & "."c)
			Me.RuleBroken = _RuleBroken : Me.InvalidLBLComponents = _InvalidLBLComponents
		End Sub

	End Class

	''' <summary>Used when an OperatorExpr.Resolve() call Throws an Exception</summary>
	Public Class DSOperatorExecutionException : Inherits DocScript.Exceptions.DSException

		'Public ReadOnly DSOperator As DocScript.Language.Expressions.Operators.DSOperator
		Public ReadOnly Operands$()

		''' <summary>Calls the Constructor for DSException which attempts to Log the Message</summary>
		Public Sub New(ByVal _Message$, ByVal _Operands$(), ByVal _InnerException As Exception)
			MyBase.New((GetType(DSOperatorExecutionException).Name.InSquares()) & " An Exception was throw whilst executing the Operator. The Operands (each in []) were: " & String.Join(", ", _Operands$.Select(Of String)(Function(_Operand$) _Operand.InSquares())).InCurlies() & ". " & _Message, _InnerException)
			Me.Operands = _Operands
		End Sub

	End Class

	''' <summary>Used when the [Action] of a BuiltInFunction Throws an Exception</summary>
	Public Class DSBuiltInFunctionException : Inherits DocScript.Exceptions.DSException

		Public ReadOnly TargetBuiltInFunction_Identifier$

		''' <summary>Calls the Constructor for DSException which attempts to Log the Message</summary>
		Public Sub New(ByVal _TargetBuiltInFunction_Identifier$, ByVal _InnerException As System.Exception)
			MyBase.New((GetType(DocScript.Exceptions.DSBuiltInFunctionException).Name.InSquares()) & " An Exception was Thrown from the BuiltInFunction with Identifier " & _TargetBuiltInFunction_Identifier.InSquares() & ": " & _InnerException.Message, _InnerException)
			Me.TargetBuiltInFunction_Identifier = _TargetBuiltInFunction_Identifier
		End Sub


	End Class

	''' <summary>Used when a Token (or several Tokens) appears in an unexpected place in the source</summary>
	Public Class DSUnexpectedTokenException : Inherits DocScript.Exceptions.DSException

		Public ReadOnly CulpritTokens As Runtime.Token()

		''' <summary>Calls the Constructor for DSException which attempts to Log the Message</summary>
		Public Sub New(ByVal _Message$, ByVal _CulpritToken As Runtime.Token)
			MyBase.New((GetType(DSUnexpectedTokenException).Name.InSquares()) & " "c & _Message & ". The culprit Token was: " & _CulpritToken.ToString() & "."c)
			Me.CulpritTokens = {_CulpritToken}
		End Sub

		''' <summary>Calls the Constructor for DSException which attempts to Log the Message</summary>
		Public Sub New(ByVal _Message$, ByVal _CulpritTokens As Runtime.Token())
			MyBase.New((GetType(DSUnexpectedTokenException).Name.InSquares()) & " "c & _Message & ". The culprit Tokens were: " & _CulpritTokens.GetValuesAndLocationsString() & "."c)
			Me.CulpritTokens = _CulpritTokens
		End Sub

	End Class

	''' <summary>Used when a Token appears in an unexpected place in the source</summary>
	Public Class DSMissingTokenException : Inherits DocScript.Exceptions.DSException

		Public ReadOnly TokenWhichShouldHaveBeenThere As Runtime.Token

		''' <summary>Calls the Constructor for DSException which attempts to Log the Message</summary>
		Public Sub New(ByVal _Message$, ByVal _TokenWhichShouldHaveBeenThere As Runtime.Token)
			MyBase.New((GetType(DSMissingTokenException).Name.InSquares()) & " "c & _Message & ". The Token which should have been present was: " & _TokenWhichShouldHaveBeenThere.ToString() & "."c)
			Me.TokenWhichShouldHaveBeenThere = _TokenWhichShouldHaveBeenThere
		End Sub

	End Class

	''' <summary>Used when a Token Array does not satisfy an Array of [TokenPattern]s</summary>
	Public Class DSTokenPatternValidationException : Inherits DocScript.Exceptions.DSException

		Public ReadOnly CulpritTokens As Runtime.Token()
		Public ReadOnly Related_TokensToPatternsValidator As DocScript.Runtime.TokenPatternValidation.TokensToPatternsValidator

		''' <summary>Calls the Constructor for DSException which attempts to Log the Message</summary>
		Public Sub New(ByVal _Message$, ByVal _Related_TokensToPatternsValidator As DocScript.Runtime.TokenPatternValidation.TokensToPatternsValidator, ByVal _CulpritTokens As Runtime.Token())
			MyBase.New((GetType(DSTokenPatternValidationException).Name.InSquares()) & " "c & _Message & ".The Expected Pattern of Tokens was: " & _Related_TokensToPatternsValidator.ToString() & ".The culprit Tokens were: " & String.Join(", ", _CulpritTokens.Select(Of String)(Function(_Token As Runtime.Token) _Token.Value)).InCurlies() & "."c)
			Me.CulpritTokens = _CulpritTokens
			Me.Related_TokensToPatternsValidator = _Related_TokensToPatternsValidator
		End Sub

	End Class

	''' <summary>Used when an Entry with the specified Identifier does not exist in a SymbolTable</summary>
	Public Class DSNonexistentSymbolException : Inherits DocScript.Exceptions.DSException

		Public ReadOnly NonexistentSymbol_Identifier$

		''' <summary>Calls the Constructor for DSException which attempts to Log the Message</summary>
		Public Sub New(ByVal _NonexistentSymbol_Identifier$, Optional ByVal _Message$ = "")
			MyBase.New((GetType(DSNonexistentSymbolException).Name.InSquares()) & " No Entry existed within the SymbolTable(s) with an identifier (in squares) of " & _NonexistentSymbol_Identifier.InSquares() & ". " & _Message.InBrackets())
			Me.NonexistentSymbol_Identifier = _NonexistentSymbol_Identifier
		End Sub

	End Class

	''' <summary>Used when an Entry with the specified Identifier already exists in a SymbolTable</summary>
	Public Class DSDuplicateSymbolException : Inherits DocScript.Exceptions.DSException

		Public ReadOnly DuplicateSymbol_Identifier$

		''' <summary>Calls the Constructor for DSException which attempts to Log the Message</summary>
		Public Sub New(ByVal _DuplicateSymbol_Identifier$, Optional ByVal _Message$ = "Unspecified Reason")
			MyBase.New((GetType(DSDuplicateSymbolException).Name.InSquares()) & " An Entry already existed in the SymbolTable(s) with an identifier (in squares) of " & _DuplicateSymbol_Identifier.InSquares() & ". " & _Message.InBrackets())
			Me.DuplicateSymbol_Identifier = _DuplicateSymbol_Identifier
		End Sub

	End Class

	''' <summary>Used when an Entry with the specified Identifier does not exist in a SymbolTable</summary>
	Public Class DSIncorrectSymbolTableEntryTypeException : Inherits DocScript.Exceptions.DSException

		Public ReadOnly NonexistentSymbol_Identifier$, _Expected_EntryType$, _Actual_EntryType$

		''' <summary>Calls the Constructor for DSException which attempts to Log the Message</summary>
		Public Sub New(ByVal _TargetSymbol_Identifier$, ByVal _ExpectedEntryType$, ByVal _ActualEntryType$, Optional ByVal _Message$ = "")
			MyBase.New((GetType(DSIncorrectSymbolTableEntryTypeException).Name.InSquares()) & " The SymbolTableEntry for (in squares) " & _TargetSymbol_Identifier.InSquares() & " was expected to be of Entry Type " & _ExpectedEntryType & ", but was found at Runtime to be of Type " & _ActualEntryType & ". " & _Message.InBrackets())
			Me.NonexistentSymbol_Identifier = _TargetSymbol_Identifier : Me._Expected_EntryType = _ExpectedEntryType : Me._Actual_EntryType = _ActualEntryType
		End Sub

	End Class

	''' <summary>Used for DS-TC, when an Input IDV cannot reasonably produce an Output IDV of a given IDV-Type</summary>
	Public Class DSRefusedToCoerceToTypeException : Inherits DocScript.Exceptions.DSException

		Public ReadOnly InputIDV As DocScript.Language.Variables.IDataValue, Target_IDVType$

		''' <summary>Calls the Constructor for DSException which attempts to Log the Message</summary>
		Public Sub New(ByVal _InputIDV As DocScript.Language.Variables.IDataValue, ByVal _Target_IDVType$, Optional ByVal _Reason$ = "(Unspecified)")
			MyBase.New((GetType(DSRefusedToCoerceToTypeException).Name.InSquares()) & " DocScript Type Coercion refused to produce an IDV of type " & _Target_IDVType.InSquares() & " from an Input IDV of type " & _InputIDV.GetType().FullName.InSquares() & " with value " & _InputIDV.ToString() & ". Reason: " & _Reason)
			Me.InputIDV = _InputIDV : Me.Target_IDVType = _Target_IDVType
		End Sub

	End Class

	''' <summary>Used when a Function declares itself to return a value, but dosen't.</summary>
	Public Class DSNoReturnValueException : Inherits DocScript.Exceptions.DSException

		Public ReadOnly TargetFunction_Identifier$

		''' <summary>Calls the Constructor for DSException which attempts to Log the Message</summary>
		Public Sub New(ByVal _TargetFunction_Identifier$, Optional ByVal _Message$ = "")
			MyBase.New((GetType(DSNoReturnValueException).Name.InSquares()) & " The Function with Identifier " & _TargetFunction_Identifier.InSquares() & " declared itself to have a Return Value, however - when executed - no Value was returned. " & _Message)
			Me.TargetFunction_Identifier = _TargetFunction_Identifier
		End Sub

	End Class

	''' <summary>Used when the DS_ThrowError(_Msg) BuiltInFunction is called from within DocScript Source</summary>
	Public Class DSSourceGeneratedException : Inherits DocScript.Exceptions.DSException

		''' <summary>Calls the Constructor for DSException which attempts to Log the Message</summary>
		Public Sub New(ByVal _Message$)
			MyBase.New((GetType(DSSourceGeneratedException).Name.InSquares()) & " An Error was Thrown from within the DocScript Program: " & _Message)
		End Sub

	End Class

#Region "Exceptions for use in other DocScript Projects"

	''' <summary>Used when an API Logical Branch encounters an Exception which must be caught by an unstream Catch Statement. DOES NOT INHERIT FROM DSException.</summary>
	Public Class DSWebException : Inherits System.Exception

		Public ReadOnly SourceLocationMessage$ 'E.g. @ExprTreeFromSourceExpr

		''' <summary>Constructs a New DocScript WebException Object, attempting to Log the Message</summary>
		''' <param name="_SourceLocationMessage">E.g. @ExprTreeFromSourceExpr</param>
		Public Sub New(ByVal _SourceLocationMessage$, ByVal _InnerException As Exception)

			MyBase.New(
			 (Function(_LogSubmissionResult As Logging.LogEventSubmissionResult) As String
				  Return (GetType(DSWebException).Name.InSquares()) & " "c & _SourceLocationMessage & ": " & If(_LogSubmissionResult.WasSuccessfull, ("(Logged) " & _InnerException.GetType().Name.InSquares() & " "c & _InnerException.Message), ("(Unlogged: " & _LogSubmissionResult.GeneratedException.Message & ") " & _InnerException.GetType().Name.InSquares() & " "c & _InnerException.Message))
			  End Function).Invoke(Logging.SubmitLogEvent((GetType(DSWebException).Name.InSquares()) & " "c & _SourceLocationMessage & ": " & _InnerException.GetType().Name.InSquares() & " "c & _InnerException.Message, LogEvent.DSEventSeverity.Error, LogEvent.DSEventCatagory.System)),
			 _InnerException
			)

		End Sub

	End Class

	''' <summary>Used when an Input-Response is not provided within 8 Mins of it being requested by the ESWorker</summary>
	Public Class DSInputRequestTimedOutException : Inherits DocScript.Exceptions.DSException

		''' <summary>Calls the Constructor for DSException which attempts to Log the Message</summary>
		Public Sub New(ByVal _TimeWaitedForResponse As TimeSpan)
			MyBase.New((GetType(DSInputRequestTimedOutException).Name.InSquares()) & " An Input-Response was not provided within " & _TimeWaitedForResponse.TotalSeconds.ToString() & " second(s) of the Input-Request being submitted to the Database. The DocScript Program could not continue without an Input Response.")
		End Sub

	End Class

#End Region

#Region "CLAManager Exceptions"

	''' <summary>Thrown when a Command-Line Argument is mandatory, but not specified. Does NOT Inherit DSException.</summary>
	Public Class DSCLANotSpecifiedException : Inherits System.Exception

		Public Sub New(ByVal _Message$)
			MyBase.New("[DSCLANotSpecifiedException] " & _Message)
		End Sub

	End Class

	''' <summary>Thrown when a Command-Line Argument is specified, but without a mandatory associated Value in the form /Key:Value or /Key:"Value". Does NOT Inherit DSException.</summary>
	Public Class DSCLAValueNotSpecifiedException : Inherits System.Exception
		Public Sub New(ByVal _Message$)
			MyBase.New("[DSCLAValueNotSpecifiedException] " & _Message)
		End Sub
	End Class

#End Region

End Namespace