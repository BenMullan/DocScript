Namespace Language.Expressions

	''' <summary>The Base Interface for all Expression Types (E.g. Implemented by LiteralExpr and VariableExpr)</summary>
	Public Interface IExpression
		Function Resolve(ByVal _InputSymbolTables As Runtime.SymbolTablesSnapshot) As Instructions.ExecutionResult 'ModifiedSymTbls are returned via this ExecutionResult
		Function GetLBLComponentString() As String
		Function GetExprTreeNodeXML() As XElement
	End Interface

	''' <summary>The Base Interface for all Exprs that contain some form of child Exprs (E.g. Implemented by FunctionCallExpr and OperatorExpr, for their Arguments and Operands respectivley)</summary>
	Public Interface ICompoundExpression : Inherits IExpression
		ReadOnly Property SubExpressions As IExpression()
	End Interface

#Region "IExpression (& ICompoundExpression) Implementations"

#Region "LBL Placeholder Exprs"

	''' <summary>The LBL Placeholder for an Operator Literal E.g. +</summary>
	Friend Class LBLOperatorPlaceholder : Implements IExpression

		Public ReadOnly OperatorToken As Runtime.Token

		Public Sub New(ByVal _OperatorToken As Runtime.Token)
			Me.OperatorToken = _OperatorToken
		End Sub

		Public Function Resolve(ByVal _SymbolTables As Runtime.SymbolTablesSnapshot) As Instructions.ExecutionResult Implements IExpression.Resolve
			Throw New DSInvalidCallException("An LBLOperatorPlaceholder cannot be Resolved. The Operator was: " & Me.OperatorToken.Value.InSquares(), "LBLOperatorPlaceholder\Resolve()")
		End Function

		Public Function GetLBLComponentString() As String Implements IExpression.GetLBLComponentString
			Return Me.OperatorToken.Value
		End Function

		Public Function GetExprTreeNodeXML() As System.Xml.Linq.XElement Implements IExpression.GetExprTreeNodeXML
			Throw New DSInvalidCallException("This Method should never be called", "LBLOperatorPlaceholder\GetExprTreeNodeXML()")
		End Function

	End Class

	''' <summary>The LBL Placeholder for the ChildExprs of a []-Bracketed Expr</summary>
	Friend Class LBLBracketedExprPlaceholder : Implements IExpression

		Public ReadOnly Contents As [IExpression]()

		Public Sub New(ByVal _Contents As IExpression())
			Me.Contents = _Contents
		End Sub

		Public Function Resolve(ByVal _SymbolTables As Runtime.SymbolTablesSnapshot) As Instructions.ExecutionResult Implements IExpression.Resolve
			Throw New DSInvalidCallException("An LBLBracketedExprPlaceholder cannot be Resolved", "LBLBracketedExprPlaceholder\Resolve()")
		End Function

		Public Function GetLBLComponentString() As String Implements IExpression.GetLBLComponentString
			'Return Constants.OpeningExpressionBracket & String.Join(" ", Me.Contents.Select(Of String)(Function(_IExpr As IExpression) _IExpr.GetLBLComponentString())) & Constants.ClosingExpressionBracket
			Return Constants.OpeningExpressionBracket & "BracketedExpr (" & Me.Contents.Length.ToString() & " Direct Children)" & Constants.ClosingExpressionBracket
		End Function

		Public Function GetExprTreeNodeXML() As System.Xml.Linq.XElement Implements IExpression.GetExprTreeNodeXML
			Throw New DSInvalidCallException("This Method should never be called", "LBLBracketedExprPlaceholder\GetExprTreeNodeXML()")
		End Function

	End Class

	''' <summary>The LBL Placeholder for the ChildExpr of a FunctionCall</summary>
	Friend Class LBLFunctionCallPlaceholder : Implements IExpression

		Public ReadOnly SourceIdentifierToken As Runtime.Token
		Public ReadOnly Contents As [IExpression]()

		Public Sub New(ByVal _SourceIdentifierToken As Runtime.Token, ByVal _Contents As IExpression())
			Me.SourceIdentifierToken = _SourceIdentifierToken : Me.Contents = _Contents
		End Sub

		Public Function GetLBLComponentString() As String Implements IExpression.GetLBLComponentString
			Return "[FunctionCall to " & Me.SourceIdentifierToken.Value & " (" & Me.Contents.Length.ToString() & " Direct Children)]"
			'Return Me.SourceIdentifierToken.Value & Constants.OpeningFunctionBracket & String.Join(" ", Me.Contents.Select(Of String)(Function(_IExpr As IExpression) _IExpr.GetLBLComponentString())) & Constants.ClosingFunctionBracket
		End Function

		Public Function Resolve(ByVal _SymbolTables As Runtime.SymbolTablesSnapshot) As Instructions.ExecutionResult Implements IExpression.Resolve
			Throw New DSInvalidCallException("An LBLFunctionCallPlaceholder cannot be Resolved", "LBLFunctionCallPlaceholder\Resolve()")
		End Function

		Public Function GetExprTreeNodeXML() As System.Xml.Linq.XElement Implements IExpression.GetExprTreeNodeXML
			Throw New DSInvalidCallException("This Method should never be called", "LBLFunctionCallPlaceholder\GetExprTreeNodeXML()")
		End Function

	End Class

	''' <summary>Represents a Comma (,) in a Linear Bracketed Level</summary>
	Friend Class LBLArgumentSeperatorPlaceholder : Implements IExpression

		Public ReadOnly SourceToken As Runtime.Token

		Public Sub New(ByVal _SourceToken As Runtime.Token)
			Me.SourceToken = _SourceToken
		End Sub

		Public Function GetLBLComponentString() As String Implements IExpression.GetLBLComponentString
			Return ","
		End Function

		Public Function Resolve(ByVal _SymbolTables As Runtime.SymbolTablesSnapshot) As Instructions.ExecutionResult Implements IExpression.Resolve
			Throw New DSInvalidCallException("An LBLArgumentSeperatorPlaceholder cannot be Resolved", "LBLArgumentSeperatorPlaceholder\Resolve()")
		End Function

		Public Function GetExprTreeNodeXML() As System.Xml.Linq.XElement Implements IExpression.GetExprTreeNodeXML
			Throw New DSInvalidCallException("This Method should never be called", "LBLArgumentSeperatorPlaceholder\GetExprTreeNodeXML()")
		End Function

	End Class

#End Region

#Region "IOT Placeholder Exprs"

	''' <summary>An object of this type is substituted into the LBL in CollapseToIOT_()</summary>
	Friend Class ScannedComponentIndicator : Implements IExpression

		Public ReadOnly LooseOpExpr_Number As UInt16

		Public Sub New(ByVal _LooseOpExpr_Number As UInt16)
			Me.LooseOpExpr_Number = _LooseOpExpr_Number
		End Sub

		Public Function GetLBLComponentString() As String Implements IExpression.GetLBLComponentString
			Throw New DSInvalidCallException("This Method should never be called", "ScannedComponentIndicator\GetLBLComponentString()")
		End Function

		Public Function Resolve(ByVal _SymbolTables As Runtime.SymbolTablesSnapshot) As Instructions.ExecutionResult Implements IExpression.Resolve
			Throw New DSInvalidCallException("This Method should never be called", "ScannedComponentIndicator\Resolve()")
		End Function

		Public Function GetExprTreeNodeXML() As System.Xml.Linq.XElement Implements IExpression.GetExprTreeNodeXML
			Throw New DSInvalidCallException("This Method should never be called", "ScannedComponentIndicator\GetExprTreeNodeXML()")
		End Function

	End Class

#End Region

	''' <summary>Represents a DSString, DSNumber, or DSBoolean Literal</summary>
	Public Class LiteralExpr(Of TLiteral As {Variables.IDataValue, Class}) : Implements IExpression

		Public ReadOnly LiteralValue As TLiteral
		Public ReadOnly SourceToken As Runtime.Token

		Public Sub New(ByVal _LiteralValue As TLiteral, Optional ByVal _SourceToken As Runtime.Token = Nothing)
			If Not Variables.VariableUtilities.IsNonVoidFunctionReturnType(GetType(TLiteral)) Then Throw New Exception("The specified TLiteral is not a valid non-void IDataValue Type")
			Me.SourceToken = _SourceToken : Me.LiteralValue = _LiteralValue
		End Sub

		Public Function Resolve(ByVal _InputSymbolTables As Runtime.SymbolTablesSnapshot) As Instructions.ExecutionResult Implements IExpression.Resolve

			REM LiteralExpr Resolution Process
			'	We have the LiteralValue already stored as an IDataValue; return [it wrapped in an ExeRes].

			Try
				'To be returned from this Resolve() call
				'There are no _ModifiedSymTbls because we do not need to touch them at all...
				Dim _ExeRes As Language.Instructions.ExecutionResult = Instructions.ExecutionResult.New_AndStartExecutionTimer("LiteralExpr (Of " & Variables.VariableUtilities.GetDataTypeString_FromDSVariableType(GetType(TLiteral)) & ")"c)

				LogExecutionMessage("(LiteralExpr: Received " & _InputSymbolTables.InnerSymbolTables_Count.ToString() & " Input Symbol Tables)")

				_ExeRes.ReturnStatus.IExpression_ResolutionResult = Me.LiteralValue
				Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_InputSymbolTables)

			Catch _Ex As Exception : Throw New DSException("LiteralExpr (Of " & Variables.VariableUtilities.GetDataTypeString_FromDSVariableType(GetType(TLiteral)) & "): " & _Ex.Message, _Ex) : End Try

		End Function

		Public Function GetLBLComponentString() As String Implements IExpression.GetLBLComponentString
			Return LiteralValue.ToString()
		End Function

		''' <summary>Returns what the Expression would have looked like in the Source</summary>
		Public Overrides Function ToString() As String
			Return Me.LiteralValue.ToString()
		End Function

		''' <summary>Returns a serialised form of all the Data for the Expression, as needed to form a graphical Expression Tree</summary>
		Public Function GetExprTreeNodeXML() As System.Xml.Linq.XElement Implements IExpression.GetExprTreeNodeXML
			Return <LiteralExpr LiteralType=<%= Variables.GetDataTypeString_FromDSVariableType(GetType(TLiteral)) %> LiteralValue=<%= Me.ToString() %>/>
		End Function

	End Class

	''' <summary>Represents an Expr which is the result of reading a Variable's value from a SymbolTable</summary>
	Public Class VariableExpr : Implements IExpression

		Public ReadOnly Identifier$
		Public ReadOnly SourceToken As Runtime.Token 'Initialised to Nothing if not passed to Constructor

		Public Sub New(ByVal _Identifier$, Optional ByVal _SourceToken As Runtime.Token = Nothing)
			Me.SourceToken = _SourceToken
			Me.Identifier = _Identifier
		End Sub

		Public Function Resolve(ByVal _InputSymbolTables As Runtime.SymbolTablesSnapshot) As Instructions.ExecutionResult Implements IExpression.Resolve

			REM VariableExpr Resolution Process
			'	Identify the SymTblEntry with Me.Identifier in the _InputSymTbls
			'	Ensure that this Entry is for a Variable (instead of a Function)
			'	Extract the IDV from the Entry, and Return this

			Try
				'To be returned from this Resolve() call
				'There are no _ModifiedSymTbls because we are only *reading* thence
				Dim _ExeRes As Language.Instructions.ExecutionResult = Instructions.ExecutionResult.New_AndStartExecutionTimer("VariableExpr" & Me.Identifier.InBrackets())

				LogExecutionMessage("(VariableExpr: Received " & _InputSymbolTables.InnerSymbolTables_Count.ToString() & " Input Symbol Tables)")

				'Ensure that the SymbolTables contain an Entry for the target Variable
				If Not _InputSymbolTables.IsContainedInAny(Me.Identifier) Then Throw New DSNonexistentSymbolException(Me.Identifier, "The VariableExpr can therefore not be resolved")
				Dim _TargetVariable_SymTblEntry As Runtime.SymbolTable.ISymbolTableEntry = _InputSymbolTables.GetEntryFromAny(Me.Identifier)
				If Not Runtime.SymbolTable.IsVariableEntry(_TargetVariable_SymTblEntry) Then Throw New DSIncorrectSymbolTableEntryTypeException(Me.Identifier, "An IDataValue Variable Type".InSquares(), _TargetVariable_SymTblEntry.GetType().FullName, "The VariableExpr can therefore not be resolved")

				_ExeRes.ReturnStatus.IExpression_ResolutionResult = Runtime.SymbolTable.ExtractVariableValue_FromSymbolTableEntry(_TargetVariable_SymTblEntry, Me.Identifier)

				Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_InputSymbolTables)
			Catch _Ex As Exception : Throw New DSException("@FunctionCallExpr " & Me.Identifier.InBrackets() & ": " & _Ex.Message, _Ex) : End Try

		End Function

		Public Function GetLBLComponentString() As String Implements IExpression.GetLBLComponentString
			Return Me.Identifier
		End Function

		''' <summary>Returns what the Expression would have looked like in the Source</summary>
		Public Overrides Function ToString() As String
			Return Me.Identifier
		End Function

		''' <summary>Returns a serialised form of all the Data for the Expression, as needed to form a graphical Expression Tree</summary>
		Public Function GetExprTreeNodeXML() As System.Xml.Linq.XElement Implements IExpression.GetExprTreeNodeXML
			Return <VariableExpr Identifier=<%= Me.ToString() %>/>
		End Function

	End Class

	''' <summary>Holds the Arguments and Identifier of a Function, ready for this to be called during Resolve()</summary>
	Public Class FunctionCallExpr : Implements ICompoundExpression

		''' <summary>The Identifier of the target DSFunction or BIF</summary>
		Public ReadOnly TargetFunction_Identifier$

		''' <summary>The UNRESOLVED Arguments to apply to the Target Function at CallTime. SymbolTables are required to Resolve() these IExprs.</summary>
		Protected ReadOnly Arguments_ As IExpression()

		Public ReadOnly Property SubExpressions As IExpression() Implements ICompoundExpression.SubExpressions
			Get
				Return Me.Arguments_
			End Get
		End Property

		''' <summary>_Identifier is the Identifier of the target DSFunction or BIF</summary>
		Public Sub New(ByVal _TargetFunction_Identifier$, ByVal _Arguments As IExpression())

			'We can only lookup how many Arguments the corrosponding DSFunction wants when we have the Symbol Table
			'Therefore, this validation must occour later

			Me.TargetFunction_Identifier = _TargetFunction_Identifier : Me.Arguments_ = _Arguments

		End Sub

		Public Function Resolve(ByVal _InputSymbolTables As Runtime.SymbolTablesSnapshot) As Instructions.ExecutionResult Implements IExpression.Resolve

			REM Differences between a FunctionCall and a FunctionCallExpr
			'	- FunctionCallExpr's Target Function MUST have a non-void ReturnType
			'	- FunctionCallExpr's Target Function MUST produce a ReturnValue

			Return Instructions.FunctionCall.CallFunctionByName(
			 _InputSymbolTables:=_InputSymbolTables,
			 _TargetFunction_Identifier:=Me.TargetFunction_Identifier,
			 _UnresolvedArguments:=Me.SubExpressions,
			 _MustProduceReturnValue:=True
			)

		End Function

		Public Function GetLBLComponentString() As String Implements IExpression.GetLBLComponentString
			Throw New DocScript.Exceptions.DSInvalidCallException("A FunctionCallExpr cannot appear in a LBL", "FunctionCallExpr\GetLBLComponentString()")
		End Function

		''' <summary>Returns what the Expression would have looked like in the Source</summary>
		Public Overrides Function ToString() As String
			Return Me.TargetFunction_Identifier & Constants.OpeningFunctionBracket & String.Join(", ", Me.SubExpressions.Select(Of String)(Function(_Argument As IExpression) _Argument.ToString())) & Constants.ClosingFunctionBracket
		End Function

		''' <summary>Returns a serialised form of all the Data for the Expression, as needed to form a graphical Expression Tree</summary>
		Public Function GetExprTreeNodeXML() As System.Xml.Linq.XElement Implements IExpression.GetExprTreeNodeXML

			Dim _XElementToReturn As XElement = <FunctionCallExpr Identifier=<%= Me.TargetFunction_Identifier %>/>

			For Each _Argument As IExpression In Me.SubExpressions
				_XElementToReturn.Add(_Argument.GetExprTreeNodeXML())
			Next

			Return _XElementToReturn

		End Function

	End Class

	''' <summary>Holds the Operands and OperatorLiteral of a DSOperator</summary>
	Public Class OperatorExpr : Implements ICompoundExpression

		Public ReadOnly OperatorChar As Char	REM E.g. "&"c

		Protected ReadOnly Operands_ As IExpression()
		Public ReadOnly Property SubExpressions As IExpression() Implements ICompoundExpression.SubExpressions
			Get
				Return Me.Operands_
			End Get
		End Property

		Public Sub New(ByVal _OperatorChar As Char, ByVal _Operands As IExpression())

			REM Ensure that there is a DSOperator for the specified _OperatorChar
			If Not Operators.DSOperators.ContainsKey(_OperatorChar) Then Throw New DSValidationException("The OperatorExpr could not be constructed because the specified OperatorChar does not appear in the DSOperators Dictionary", "Operator Char: " & _OperatorChar)

			REM There must be two operands for a Binary Operator...
			If Operators.OperatorUtilities.IsBinaryOperator(_OperatorChar) Then If Not _Operands.Length = 2 Then Throw New DSValidationException("The No. Operands specified to a New OperatorExpr() was " & _Operands.Length.ToString() & " when it should have been two, because the Operator " & _OperatorChar.ToString().InSquares() & " is a Binary one", "OperatorExpr\New()\_Operands")

			REM ...And one for a Unary Operator
			If Operators.OperatorUtilities.IsUnaryOperator(_OperatorChar) Then If Not _Operands.Length = 1 Then Throw New DSValidationException("The No. Operands specified to a New OperatorExpr() was " & _Operands.Length.ToString() & " when it should have been one, because the Operator " & _OperatorChar.ToString().InSquares() & " is a Unary one", "OperatorExpr\New()\_Operands")

			Me.OperatorChar = _OperatorChar : Me.Operands_ = _Operands

		End Sub

		Public Function GetLBLComponentString() As String Implements IExpression.GetLBLComponentString
			Throw New DocScript.Exceptions.DSInvalidCallException("An OperatorExpr cannot appear in a Linear Bracketed Level", "OperatorExpr\GetLBLComponentString()")
		End Function

		Public Function Resolve(ByVal _InputSymbolTables As Runtime.SymbolTablesSnapshot) As Instructions.ExecutionResult Implements IExpression.Resolve

			REM DocScript OperatorExpr Resolution Process
			'	The Constructor has already checked that our (ReadOnly) OperatorChar exists in the DSOperators Dictionary
			'	
			'	Resolve() First Operand		(To an IDataValue)
			'	Resolve() Second Operand	(To an IDataValue)	(Only if BinaryOperator)
			'	
			'	Find my DSOperator in the DSOperators Dictionary
			'	Resolve() each Operand (from an IExpression to an IDataValue)
			'	Pass in the one|two IDataValues we have for Operands
			'	Return the Result, wrapped in an IDataValue

			Try
				'To be returned from this Resolve() call
				Dim _ExeRes As Language.Instructions.ExecutionResult = Instructions.ExecutionResult.New_AndStartExecutionTimer("OperatorExpr (DSOperator\" & Me.OperatorChar & ")"c)

				'For the returning _ExeRes
				LogExecutionMessage("(OperatorExpr: Received " & _InputSymbolTables.InnerSymbolTables_Count.ToString() & " Input Symbol Tables)")
				Dim _ModifiedSymTbls As Runtime.SymbolTablesSnapshot = _InputSymbolTables
				Dim _OperationResult As Variables.IDataValue

				'Get the DSOperator at our OperatorChar
				Dim _Target_DSOperator As Language.Expressions.Operators.DSOperator = DocScript.Language.Expressions.Operators.DSOperators.Item(Me.OperatorChar)

				If Operators.IsBinaryOperator(Me.OperatorChar) Then

					Dim _Operation As Operators.BinaryOperator.BinaryOperation = CType(_Target_DSOperator, Operators.BinaryOperator).Operation

					REM	_ModifiedSymTbls → [OperandOne_Resolve()] → _ModifiedSymTbls
					Dim _OperandOne_Resolution_ExeRes As Instructions.ExecutionResult = Me.SubExpressions(0).Resolve(_ModifiedSymTbls)
					_ExeRes.UpstairsExecutionResults.Add(New Tuple(Of String, Instructions.ExecutionResult)("Operand [0]", _OperandOne_Resolution_ExeRes))
					_ModifiedSymTbls = _OperandOne_Resolution_ExeRes.ResultantSymbolTablesState

					REM	_ModifiedSymTbls → [OperandTwo_Resolve()] → _ModifiedSymTbls
					Dim _OperandTwo_Resolution_ExeRes As Instructions.ExecutionResult = Me.SubExpressions(1).Resolve(_ModifiedSymTbls)
					_ExeRes.UpstairsExecutionResults.Add(New Tuple(Of String, Instructions.ExecutionResult)("Operand [1]", _OperandTwo_Resolution_ExeRes))
					_ModifiedSymTbls = _OperandTwo_Resolution_ExeRes.ResultantSymbolTablesState

					LogExecutionMessage("Executing Operation for DSOperator" & Me.OperatorChar.ToString().InSquares() & "with Resolved Operands " & _OperandOne_Resolution_ExeRes.ReturnStatus.IExpression_ResolutionResult.ToString().InSquares() & " and " & _OperandTwo_Resolution_ExeRes.ReturnStatus.IExpression_ResolutionResult.ToString().InSquares())

					REM No SymTbls are pased in here; the Operands are ALREADY RESOLVED, and OperatorDelegates never need to access the SymbolTable (they're just simple actions like * or &)
					_OperationResult = _Operation.Invoke(
					 _OperandOne:=_OperandOne_Resolution_ExeRes.ReturnStatus.IExpression_ResolutionResult,
					 _OperandTwo:=_OperandTwo_Resolution_ExeRes.ReturnStatus.IExpression_ResolutionResult
					)

				ElseIf Operators.IsUnaryOperator(Me.OperatorChar) Then

					Dim _Operation As Operators.UnaryOperator.UnaryOperation = CType(_Target_DSOperator, Operators.UnaryOperator).Operation

					REM	_ModifiedSymTbls → [OperandOne_Resolve()] → _ModifiedSymTbls
					Dim _OperandOne_Resolution_ExeRes As Instructions.ExecutionResult = Me.SubExpressions(0).Resolve(_ModifiedSymTbls)
					_ExeRes.UpstairsExecutionResults.Add(New Tuple(Of String, Instructions.ExecutionResult)("Operand [0]", _OperandOne_Resolution_ExeRes))
					_ModifiedSymTbls = _OperandOne_Resolution_ExeRes.ResultantSymbolTablesState

					LogExecutionMessage("Executing Operation for DSOperator" & Me.OperatorChar.ToString().InSquares() & "with Resolved Operand " & _OperandOne_Resolution_ExeRes.ReturnStatus.IExpression_ResolutionResult.ToString().InSquares())

					REM No SymTbls are pased in here; the Operands are ALREADY RESOLVED, and OperatorDelegates never need to access the SymbolTable (they're just simple actions like * or &)
					_OperationResult = _Operation.Invoke(_Operand:=_OperandOne_Resolution_ExeRes.ReturnStatus.IExpression_ResolutionResult)

				Else : Throw New DSValidationException("The Operator was not recognised as a valid Unary or Binary Operator", "OperatorChar: " & Me.OperatorChar)
				End If

				_ExeRes.ReturnStatus.IExpression_ResolutionResult = _OperationResult
				Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_ModifiedSymTbls)
			Catch _Ex As Exception : Throw New DSOperatorExecutionException("@DSOperators" & Me.OperatorChar & ": " & _Ex.Message, {Me.SubExpressions(0).ToString(), If(Me.SubExpressions.Length < 2, "(No Second Operand)", Me.SubExpressions(1).ToString())}, _Ex) : End Try

		End Function

		''' <summary>Returns what the Expression would have looked like in the Source</summary>
		Public Overrides Function ToString() As String

			If Operators.OperatorUtilities.IsBinaryOperator(Me.OperatorChar) Then
				'[Operand0 * Operand1]
				Return Language.Constants.OpeningExpressionBracket & Me.SubExpressions(0).ToString() & Language.Constants.Space & Me.OperatorChar & Language.Constants.Space & Me.SubExpressions(1).ToString() & Language.Constants.ClosingExpressionBracket
			ElseIf Operators.OperatorUtilities.IsUnaryOperator(Me.OperatorChar) Then
				'[~ Operand0]
				Return Language.Constants.OpeningExpressionBracket & Me.OperatorChar & Language.Constants.Space & Me.SubExpressions(0).ToString() & Language.Constants.ClosingExpressionBracket
			Else : Throw New DSException("The OperatorExpr was neither a Unary or Binary Operator. The OperatorChar was: " & Me.OperatorChar.ToString().InSquares())
			End If

		End Function

		''' <summary>Returns a serialised form of all the Data for the Expression, as needed to form a graphical Expression Tree</summary>
		Public Function GetExprTreeNodeXML() As System.Xml.Linq.XElement Implements IExpression.GetExprTreeNodeXML

			Dim _XElementToReturn As XElement = <OperatorExpr OperatorChar=<%= Me.OperatorChar.ToString() %>/>

			For Each _Argument As IExpression In Me.SubExpressions
				_XElementToReturn.Add(_Argument.GetExprTreeNodeXML())
			Next

			Return _XElementToReturn

		End Function

	End Class

#End Region

End Namespace