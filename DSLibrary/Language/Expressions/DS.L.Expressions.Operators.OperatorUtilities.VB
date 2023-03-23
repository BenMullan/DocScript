Imports DocScript.Language.Variables 'For IDataValue
Namespace Language.Expressions.Operators

	''' <summary>Contains the DSOperators Declaration, and several Utility Methods</summary>
	Public Module OperatorUtilities

		Public ReadOnly Property IsUnaryOperator(ByVal _OperatorChar As Char) As Boolean
			Get
				Try
					If Not DSOperators.ContainsKey(_OperatorChar) Then Throw New DSValidationException("Could not determine if [" & _OperatorChar.ToString() & "] is a Unary Operator because it is not contained in the DSOperators Dictionary", _OperatorChar.ToString())
					Return (DSOperators.Item(_OperatorChar).GetType() = GetType(UnaryOperator))
				Catch _Ex As Exception : Throw New DSException("@IsUnaryOperator: " & _Ex.Message, _Ex) : End Try
			End Get
		End Property

		Public ReadOnly Property IsBinaryOperator(ByVal _OperatorChar As Char) As Boolean
			Get
				Try
					If Not DSOperators.ContainsKey(_OperatorChar) Then Throw New DSValidationException("Could not determine if [" & _OperatorChar.ToString() & "] is a Binary Operator because it is not contained in the DSOperators Dictionary", _OperatorChar.ToString())
					Return (DSOperators.Item(_OperatorChar).GetType() = GetType(BinaryOperator))
				Catch _Ex As Exception : Throw New DSException("@IsBinaryOperator: " & _Ex.Message, _Ex) : End Try
			End Get
		End Property

#Region "Individual DSOperator Definitions"

#Region "Misc. DSOperators"

		Private ReadOnly DSOperator_EqualityComparrison_ As New BinaryOperator(_FriendlyName:="Equality Comparison", _Precedence:=1,
		_Operation:=
		Function(_A As IDataValue, _B As IDataValue)
			LogExecutionMessage("Equality Comparrison Operator: Comparing " & _A.Coerce(Of DSString)().Value.InSquares() & " to " & _B.Coerce(Of DSString)().Value.InSquares() & ". Result: " & (_A.Coerce(Of DSString)().Value = _B.Coerce(Of DSString)().Value).ToString() & ". DSBoolean threrfor Value: " & (New DSBoolean(_A.Coerce(Of DSString)().Value = _B.Coerce(Of DSString)().Value).Value).ToString(), LogEvent.DSEventSeverity.Debug)
			Return New DSBoolean(_A.Coerce(Of DSString)().Value = _B.Coerce(Of DSString)().Value)
		End Function
		)

#End Region

#Region "String* DSOperators"

		Private ReadOnly DSOperator_StringConcatenation_ As New BinaryOperator(_FriendlyName:="String Concatenation", _Precedence:=2,
		_Operation:=Function(_A As IDataValue, _B As IDataValue) New DSString(_A.Coerce(Of DSString)().Value & _B.Coerce(Of DSString)().Value)
		)

#End Region

#Region "Logical* DSOperators"

		Private ReadOnly DSOperator_LogicalNot_ As New UnaryOperator(_FriendlyName:="Logical Not", _Precedence:=8,
		_Operation:=Function(_A As IDataValue) New DSBoolean(Not _A.Coerce(Of DSBoolean)().Value)
		)

		Private ReadOnly DSOperator_LogicalAnd_ As New BinaryOperator(_FriendlyName:="Logical And", _Precedence:=7,
		_Operation:=Function(_A As IDataValue, _B As IDataValue) New DSBoolean(_A.Coerce(Of DSBoolean)().Value AndAlso _B.Coerce(Of DSBoolean)().Value)
		)

		Private ReadOnly DSOperator_LogicalOr_ As New BinaryOperator(_FriendlyName:="Logical Or", _Precedence:=5,
		_Operation:=Function(_A As IDataValue, _B As IDataValue) New DSBoolean(_A.Coerce(Of DSBoolean)().Value OrElse _B.Coerce(Of DSBoolean)().Value)
		)

		Private ReadOnly DSOperator_LogicalXOr_ As New BinaryOperator(_FriendlyName:="Logical XOr", _Precedence:=6,
		_Operation:=Function(_A As IDataValue, _B As IDataValue) New DSBoolean(_A.Coerce(Of DSBoolean)().Value Xor _B.Coerce(Of DSBoolean)().Value)
		)

#End Region

#Region "Numerical* DSOperators"

		Private ReadOnly DSOperator_NumericalAddition_ As New BinaryOperator(_FriendlyName:="Numerical Addition", _Precedence:=4,
		_Operation:=Function(_A As IDataValue, _B As IDataValue) New DSNumber(_A.Coerce(Of DSNumber)().Value + _B.Coerce(Of DSNumber)().Value)
		)

		Private ReadOnly DSOperator_NumericalSubtraction_ As New BinaryOperator(_FriendlyName:="Numerical Subtraction", _Precedence:=4,
		_Operation:=Function(_A As IDataValue, _B As IDataValue) New DSNumber(_A.Coerce(Of DSNumber)().Value - _B.Coerce(Of DSNumber)().Value)
		)

		Private ReadOnly DSOperator_NumericalMultiplication_ As New BinaryOperator(_FriendlyName:="Numerical Multiplication", _Precedence:=3,
		_Operation:=Function(_A As IDataValue, _B As IDataValue) New DSNumber(_A.Coerce(Of DSNumber)().Value * _B.Coerce(Of DSNumber)().Value)
		)

		Private ReadOnly DSOperator_NumericalDivision_ As New BinaryOperator(_FriendlyName:="Numerical Division", _Precedence:=3,
		_Operation:=Function(_A As IDataValue, _B As IDataValue) New DSNumber(_A.Coerce(Of DSNumber)().Value / _B.Coerce(Of DSNumber)().Value)
		)

		Private ReadOnly DSOperator_NumericalExponentiation_ As New BinaryOperator(_FriendlyName:="Numerical Exponentiation", _Precedence:=3,
		_Operation:=Function(_A As IDataValue, _B As IDataValue) New DSNumber(_A.Coerce(Of DSNumber)().Value ^ _B.Coerce(Of DSNumber)().Value)
		)

		Private ReadOnly DSOperator_NumericalModulo_ As New BinaryOperator(_FriendlyName:="Numerical Modulo", _Precedence:=3,
		_Operation:=Function(_A As IDataValue, _B As IDataValue) New DSNumber(_A.Coerce(Of DSNumber)().Value Mod _B.Coerce(Of DSNumber)().Value)
		)

		Private ReadOnly DSOperator_NumericalInvertPolarity_ As New UnaryOperator(_FriendlyName:="Numerical Invert Polarity", _Precedence:=8,
		_Operation:=Function(_A As IDataValue) New DSNumber(-_A.Coerce(Of DSNumber)().Value)
		)

#End Region

#End Region

		''' <summary>Maps the OperatorChars (as defined in the Language-level Constants) to their Binary- and Unary-Operator(s)</summary>
		Public ReadOnly DSOperators As New Dictionary(Of Char, DSOperator)() From {
		 {Constants.OperatorChar_EqualityComparrison, OperatorUtilities.DSOperator_EqualityComparrison_},
		 {Constants.OperatorChar_StringConcatenation, OperatorUtilities.DSOperator_StringConcatenation_},
		 {Constants.OperatorChar_LogicalNot, OperatorUtilities.DSOperator_LogicalNot_},
		 {Constants.OperatorChar_LogicalAnd, OperatorUtilities.DSOperator_LogicalAnd_},
		 {Constants.OperatorChar_LogicalOr, OperatorUtilities.DSOperator_LogicalOr_},
		 {Constants.OperatorChar_LogicalXOr, OperatorUtilities.DSOperator_LogicalXOr_},
		 {Constants.OperatorChar_NumericalAddition, OperatorUtilities.DSOperator_NumericalAddition_},
		 {Constants.OperatorChar_NumericalSubtraction, OperatorUtilities.DSOperator_NumericalSubtraction_},
		 {Constants.OperatorChar_NumericalMultiplication, OperatorUtilities.DSOperator_NumericalMultiplication_},
		 {Constants.OperatorChar_NumericalDivision, OperatorUtilities.DSOperator_NumericalDivision_},
		 {Constants.OperatorChar_NumericalExponentiation, OperatorUtilities.DSOperator_NumericalExponentiation_},
		 {Constants.OperatorChar_NumericalModulo, OperatorUtilities.DSOperator_NumericalModulo_},
		 {Constants.OperatorChar_NumericalInvertPolarity, OperatorUtilities.DSOperator_NumericalInvertPolarity_}
		}

	End Module

End Namespace