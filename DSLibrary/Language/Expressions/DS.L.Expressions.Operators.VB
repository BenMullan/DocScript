'Try Statements in Operation Delegates - Check for Overflow and report Operands (\w TokenLocations?) in _Ex.Message
'Return [IDataValue]s instead of [LiteralExpr]s

REM About DocScript Operators
'----------------------------
'Operators only ever READ from the Symbol Tables.
'However, an Operand such as a FunctionCall may need to write to the Global Symbol Table

'Whereas an IExpression represents an UNRESOLVED Tree which *can* produce a value...
'...an IDataValue represents a RESOLVED Datum which is itself a value.
'IExpressions DON'T have a DocScript DataType, whereas IDataValues DO.

'DSOperator Objects are just definitions of what a certain Operator does;
'	they are not representations of instances of an operator in source (or a program)
'They are refered to by OperatorExpr, when Resolve() is called.



'Question:
'	The Arguments to a Function (DS or BIF) need to be resolved in the same scope as the FunctionCall, BEFORE the target Function is executed.
'	Is it ever the case that an Operand to a DSOperator will be put in a context where is it unresolvable, because the operand referances inaccessible Symbols?
'	Do the operands of a DSOperator need to be resolved before executing the Operation?
'	Is there any advantage to defering the operand resolution?

'You could have:
'	5 - [_Age + 14 * GetNumber(H * _H)]
'	ConstructPerson(GetNameFromID(_InputID + ~GetOffset(23&23)), GetAgeFromID(_InputID + ~Offset))

'Answer 27112022:
'	There is no obvious advantage to defering the Operand Resolution to within the Operation Delegates;
'	The Operands should therefore be passed as IDataValues instead of IExpressions.
'	Aditionally, OperatorDelegates do NOT need to return an ExecutionResult, because:
'		- The SymbolTables are never modified; no IExpression resolution occurs within the OperationDelegates
'		- The OperatorExpr which invokes a given OperatorDelegate will do the Execution Timing; the OperatorDelegate needn't do it too.

Imports DocScript.Language.Variables 'For IDataValue
Namespace Language.Expressions.Operators

	''' <summary>The Base Class for UnaryOperator and BinaryOperator</summary>
	Public MustInherit Class DSOperator

		Public ReadOnly FriendlyName As String		'E.g. "Concatenation"
		Public ReadOnly Precedence As Byte			'0 to 255	(Highest = FirstExecuted)

		Public Sub New(ByVal _FriendlyName$, ByVal _Precedence As Byte)
			Me.FriendlyName = _FriendlyName : Me.Precedence = _Precedence
		End Sub

	End Class

	''' <summary>Represents an Operator with one Operand</summary>
	Public Class UnaryOperator : Inherits DSOperator

		REM No SymbolTables need to be passed in, because no IExpression Resolution occurs inside, and no SymbolTables are returned
		Public Delegate Function UnaryOperation(ByVal _Operand As IDataValue) As IDataValue

		Public ReadOnly Operation As UnaryOperation

		Public Sub New(ByVal _FriendlyName$, ByVal _Precedence As System.Byte, ByVal _Operation As UnaryOperation)
			MyBase.New(_FriendlyName, _Precedence) : Me.Operation = _Operation
		End Sub

	End Class

	''' <summary>Represents an Operator with two Operands</summary>
	Public Class BinaryOperator : Inherits DSOperator

		REM No SymbolTables need to be passed in, because no IExpression Resolution occurs inside, and no SymbolTables are returned
		Public Delegate Function BinaryOperation(ByVal _OperandOne As IDataValue, ByVal _OperandTwo As IDataValue) As IDataValue

		Public Operation As BinaryOperation

		Public Sub New(ByVal _FriendlyName$, ByVal _Precedence As System.Byte, ByVal _Operation As BinaryOperation)
			MyBase.New(_FriendlyName, _Precedence) : Me.Operation = _Operation
		End Sub

	End Class

End Namespace