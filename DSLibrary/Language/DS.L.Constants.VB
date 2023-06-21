Namespace Language

	''' <summary>An Uninheritable, Uninstanciatable Class holding Language-Level Constants for DocScript. See Also: DocScript.Runtime.Constants</summary>
	Public NotInheritable Class Constants

		'Make this Class uninstanciatable from outside
		Private Sub New()
		End Sub

		REM Misc.
		Public Const CommentCharacter As Char = "#"c									'Only valid at the start of a line
		Public Const AssignmentOperator As Char = ":"c									'Not valid as an Expr. Operator
		Public Const Space As Char = " "c
		Public Const LineBreak As String = [vbCrLf]
		Public Const LineBreak_SubstitutionChar_ForFlattenedSource As Char = ";"c		'Used by Parser.FlattenSource

		REM GrammarChars
		Public Const OpeningFunctionBracket As Char = "("c
		Public Const ClosingFunctionBracket As Char = ")"c
		Public Const OpeningExpressionBracket As Char = "["c
		Public Const ClosingExpressionBracket As Char = "]"c
		Public Const OpeningDataTypeBracket As Char = "<"c
		Public Const ClosingDataTypeBracket As Char = ">"c
		Public Const FunctionArgumentSeperator As Char = ","c

		REM Keywords
		Public Const Keyword_If$ = "IF"
		Public Const Keyword_Else$ = "ELSE"
		Public Const Keyword_While$ = "WHILE"
		Public Const Keyword_Loop$ = "LOOP"
		Public Const Keyword_Return$ = "RETURN"
		Public Const Keyword_Function$ = "FUNCTION"
		Public Shared ReadOnly AllKeywords$() = {Keyword_If, Keyword_Else, Keyword_While, Keyword_Loop, Keyword_Return, Keyword_Function}

		REM StatementEnds
		Public Const StatementEndPrefix$ = "END"
		Public Const StatementEnd_If$ = StatementEndPrefix & Keyword_If
		Public Const StatementEnd_While$ = StatementEndPrefix & Keyword_While
		Public Const StatementEnd_Loop$ = StatementEndPrefix & Keyword_Loop
		Public Const StatementEnd_Function$ = StatementEndPrefix & Keyword_Function
		Public Shared ReadOnly AllStatementEnds$() = {StatementEnd_If, StatementEnd_While, StatementEnd_Loop, StatementEnd_Function}

		REM Operators
		Public Const OperatorChar_EqualityComparrison As Char = "="c
		Public Const OperatorChar_StringConcatenation As Char = "&"c
		Public Const OperatorChar_LogicalNot As Char = "¬"c
		Public Const OperatorChar_LogicalAnd As Char = "'"c
		Public Const OperatorChar_LogicalOr As Char = "|"c
		Public Const OperatorChar_LogicalXOr As Char = "¦"c
		Public Const OperatorChar_NumericalAddition As Char = "+"c
		Public Const OperatorChar_NumericalSubtraction As Char = "-"c
		Public Const OperatorChar_NumericalMultiplication As Char = "*"c
		Public Const OperatorChar_NumericalDivision As Char = "/"c
		Public Const OperatorChar_NumericalExponentiation As Char = "^"c
		Public Const OperatorChar_NumericalModulo As Char = "%"c
		Public Const OperatorChar_NumericalInvertPolarity As Char = "~"c

		REM DataTypes
		Public Const DataType_Void$ = "VOID"
		Public Const DataType_String$ = "STRING"
		Public Const DataType_Number$ = "NUMBER"
		Public Const DataType_Boolean$ = "BOOLEAN"
		Public Const ArrayModifierChar As Char = "@"c
		Public Const DataType_StringArray$ = DataType_String & ArrayModifierChar
		Public Const DataType_NumberArray$ = DataType_Number & ArrayModifierChar
		Public Const DataType_BooleanArray$ = DataType_Boolean & ArrayModifierChar
		Public Shared ReadOnly AllDataTypes$() = {DataType_Void, DataType_String, DataType_Number, DataType_Boolean, DataType_StringArray, DataType_NumberArray, DataType_BooleanArray}
		Public Shared ReadOnly AllNonArrayDataTypes$() = {DataType_Void, DataType_String, DataType_Number, DataType_Boolean}

		REM Literals
		Public Shared ReadOnly BasedNumericLiteralGraphemes As Char() = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray()
		Public Shared ReadOnly HighestPermittedNumericLiteralBase As Byte = CByte(BasedNumericLiteralGraphemes.Length)
		Public Const BasedNumericLiteral_ValueAndBaseSeperator As Char = "_"c
		Public Const BooleanTrueLiteral$ = "TRUE"
		Public Const BooleanFalseLiteral$ = "FALSE"
		Public Const StringLiteralStartEndChar As Char = """"c
		Public Const StringLIteralStartEndChar_DSStringValueReplacementChar As Char = "`"c		'A DSString's Value will contain this instead of "

	End Class

End Namespace