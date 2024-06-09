Namespace Language.Instructions

	''' <summary>Represents E.g. [Return "Ben"]</summary>
	Public Class ReturnToCaller : Implements DocScript.Language.Instructions.IInstruction

#Region "Declarations specific to this IInstruction"

		''' <summary>If there wasn't an associated Expr in the source, then this is Nothing (null)</summary>
		Public ReturnValueExpr As Expressions.IExpression = Nothing

#End Region

		Protected ReadOnly Tokens_ As Runtime.Token()

		''' <summary>Constructs the IInstruction from an existing instance</summary>
		Public Sub New(ByVal _ReturnValueExpr As Expressions.IExpression)
			Me.ReturnValueExpr = _ReturnValueExpr
		End Sub

		''' <summary>Constructs the IInstruction from its Tokens</summary>
		Public Sub New(ByRef _Tokens As Runtime.Token())
			Me.Tokens_ = _Tokens : Me.ProcessTokensToInitialiseFields()
		End Sub

		Public Sub ProcessTokensToInitialiseFields()
			Try : LogLexingMessage("Began constructing a ReturnToCaller Object...")

				REM Source should look like:
				REM		Return
				REM		Return "Expression"

				REM Tokens should look like:
				REM		[Keyword], [LineEnd]
				REM		[Keyword], [ExprTokens...], [LineEnd]

				REM Fields to Initialise:
				REM		ReturnValueExpr		(Can be Nothing)

				REM Ensure that there are enough tokens to construct the IInstruction
				If Me.Tokens_.Count < Runtime.TokenPatternValidation.MinimumRequiredTokens.Item(Me.GetType()) Then Throw New DSValidationException(String.Format("The No. Tokens ({0}) was less than the minimum No. Tokens required to construct the Instruction ({1}).", Me.Tokens_.Count.ToString(), Runtime.MinimumRequiredTokens(Me.GetType())), "Tokens: " & Me.Tokens_.GetValuesAndLocationsString())

				REM Ensure that the last Token is a {LineEnd}
				If Not Runtime.BuiltInTPVs.LineEnd_TPV.IsSatisfiedBy(Me.Tokens_.Last()) Then Throw New DSUnexpectedTokenException("The Last Token was not a {LineEnd}", Me.Tokens_)

				REM Ensure that the main TPV herefor is satisfied
				Runtime.BuiltInTPVs.Keyword_Return_TPV.EnsureIsSatisfiedBy(Me.Tokens_)

				REM Only Initialise the ReturnValueExpr, if there is such an Expr in the Source Tokens	
				REM Tokens after Token 0 (1 onwards...) should form the AssignmentExpr, up to the {LineEnd}
				If Me.Tokens_.Length >= 3 Then Me.ReturnValueExpr = Expressions.ConstructExpressionFromTokens(Me.Tokens_.Skip(1).ToArray().UpToButExcluding(AddressOf Runtime.BuiltInTPVs.LineEnd_TPV.IsSatisfiedBy))

				LogLexingMessage("...Finished constructing a ReturnToCaller Object, with" & If(Me.ReturnValueExpr Is Nothing, "out", "") & " a ReturnValue Expr")
			Catch _Ex As Exception : Throw New DSException("@ReturnToCaller\ProcessTokensToInitialiseFields: " & _Ex.Message, _Ex) : End Try
		End Sub

		''' <summary>Any Expressions or Child Instructions executed hereby, are returned in the ExeRes's UpstairsExecutionResults member.</summary>
		Public Function Execute(ByVal _InputSymbolTables As Runtime.SymbolTablesSnapshot) As ExecutionResult Implements IInstruction.Execute
			Try : Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("ReturnToCaller")

				LogExecutionMessage("(ReturnToCaller: Received " & _InputSymbolTables.InnerSymbolTables_Count.ToString() & " Input Symbol Tables)")
				Dim _Modified_SymTbls As Runtime.SymbolTablesSnapshot = _InputSymbolTables

				REM ReturnToCaller Execution Process:
				'	- Raise the CurrentDSFunction_ReturnHasOccured flag
				'	- If Me.ReturnValueExpr IsNot Nothing, then CurrentDSFunction_ReturnValue = Me.ReturnValueExpr.Resolve()

				_ExeRes.ReturnStatus.CurrentDSFunction_ReturnHasOccurred = True

				If (Me.ReturnValueExpr IsNot Nothing) Then

					'Resolve()
					Dim _ReturnValueExpr_Resolution_ExeRes As ExecutionResult = ReturnValueExpr.Resolve(_Modified_SymTbls)

					'Add as an Upstairs ExeRes
					_ExeRes.UpstairsExecutionResults.Add(New Tuple(Of String, ExecutionResult)("ReturnValueExpr", _ReturnValueExpr_Resolution_ExeRes))

					'Update my SymbolTables
					_Modified_SymTbls = _ReturnValueExpr_Resolution_ExeRes.ResultantSymbolTablesState

					'Extract the Resolved Expr
					_ExeRes.ReturnStatus.CurrentDSFunction_ReturnValue = _ReturnValueExpr_Resolution_ExeRes.ReturnStatus.[IExpression_ResolutionResult]
					LogExecutionMessage("ReturnToCaller: Return Value Expr has type " & _ExeRes.ReturnStatus.CurrentDSFunction_ReturnValue.GetType().FullName.InSquares())

				End If

				Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_Modified_SymTbls)
			Catch _Ex As Exception : Throw New DSException("@VariableDeclaration\Execute: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>Returns an XML Representation of the Instruction, with all Propities and Child Structres included</summary>
		Public Function GetProgramTreeNodeXML() As System.Xml.Linq.XElement Implements IInstruction.GetProgramTreeNodeXML

			Dim _XElementToReturn As XElement = <ReturnToCaller/>
			_XElementToReturn.Add(If(Me.ReturnValueExpr Is Nothing, <ReturnValueExpr/>, Me.ReturnValueExpr.GetExprTreeNodeXML().WrapIn("ReturnValueExpr")))

			Return _XElementToReturn

		End Function

		''' <summary>Constructs an instance of the IInstruction class from its XML serialasation</summary>
		Public Shared Function FromXML(ByVal _IInstructionXElement As XElement) As ReturnToCaller
			Try

				Return New ReturnToCaller(
				 _ReturnValueExpr:=If(_IInstructionXElement.<ReturnValueExpr>.First().Elements().Count() = 1, Expressions.ExprUtilities.ConstructExpressionFromXML(_IInstructionXElement.<ReturnValueExpr>.First().Elements().First()), Nothing)
				)

			Catch _Ex As Exception : Throw New DSException("@ReturnToCaller\FromXML: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>Returns what the Instruction would have looked like in the Source, without an extra LineBreak on the End</summary>
		Public Overrides Function ToString() As String

			REM E.g.
			'	RETURN 3

			Return _
			 Constants.Keyword_Return & If(Me.ReturnValueExpr Is Nothing, "", Constants.Space & Me.ReturnValueExpr.ToString())

		End Function

	End Class

End Namespace