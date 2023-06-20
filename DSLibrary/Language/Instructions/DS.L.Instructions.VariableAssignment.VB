Namespace Language.Instructions

	''' <summary>Represents E.g. [Name : "Ben"]</summary>
	Public Class VariableAssignment : Implements DocScript.Language.Instructions.IInstruction

#Region "Declarations specific to this IInstruction"

		Public TargetVariable_Identifier As [String]
		Public AssignmentExpr As Language.Expressions.IExpression

#End Region

		Protected ReadOnly Tokens_ As Runtime.Token()

		''' <summary>For internal use only; used by FromXML()</summary>
		Protected Sub New()
		End Sub

		''' <summary>Constructs the IInstruction from its Tokens</summary>
		Public Sub New(ByRef _Tokens As Runtime.Token())
			Me.Tokens_ = _Tokens : Me.ProcessTokensToInitialiseFields()
		End Sub

		Public Sub ProcessTokensToInitialiseFields()
			Try : LogLexingMessage("Began constructing a VariableAssignment...")

				REM Source should look like:
				REM		Age : [5 * 6]

				REM Tokens should look like:
				REM		[Identifier], [DSOperator], [ExprTokens...], [LineEnd]

				REM Fields to Initialise:
				REM		TargetVariable_Identifier
				REM		AssignmentExpr

				REM Ensure that there are enough tokens to construct the IInstruction
				If Me.Tokens_.Count < Runtime.TokenPatternValidation.MinimumRequiredTokens.Item(Me.GetType()) Then Throw New DSValidationException(String.Format("The No. Tokens ({0}) was less than the minimum No. Tokens required to construct the Instruction ({1}).", Me.Tokens_.Count.ToString(), Runtime.MinimumRequiredTokens(Me.GetType())), "Tokens: " & Me.Tokens_.GetValuesAndLocationsString())

				REM Ensure that the last Token is a {LineEnd}
				If Not Runtime.BuiltInTPVs.LineEnd_TPV.IsSatisfiedBy(Me.Tokens_.Last()) Then Throw New DSUnexpectedTokenException("The Last Token was not a {LineEnd}", Me.Tokens_)

				REM Ensure that the main TPV herefor is satisfied
				Runtime.BuiltInTPVs.VariableAssignmet_TPV.EnsureIsSatisfiedBy(Me.Tokens_)

				REM The TargetVariable_Identifier should be derivable from the 1st Token
				Me.TargetVariable_Identifier = Me.Tokens_(0).Value

				REM Tokens after Token 1 (2 onwards...) should form the AssignmentExpr, up to the {LineEnd}
				Me.AssignmentExpr = Expressions.ConstructExpressionFromTokens(Me.Tokens_.Skip(2).ToArray().UpToButExcluding(AddressOf Runtime.BuiltInTPVs.LineEnd_TPV.IsSatisfiedBy))

				LogLexingMessage("...Finished constructing a VariableAssignment Object for " & Me.TargetVariable_Identifier.InSquares())
			Catch _Ex As Exception : Throw New DSException("@VariableAssignment\ProcessTokensToInitialiseFields: " & _Ex.Message, _Ex) : End Try
		End Sub

		''' <summary>Any Expressions or Child Instructions executed hereby, are returned in the ExeRes's UpstairsExecutionResults member.</summary>
		Public Function Execute(ByVal _InputSymbolTables As Runtime.SymbolTablesSnapshot) As ExecutionResult Implements IInstruction.Execute
			Try : Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("VariableAssignment " & Me.TargetVariable_Identifier.InBrackets())

				LogExecutionMessage("(VariableAssignment: Received " & _InputSymbolTables.InnerSymbolTables_Count.ToString() & " Input Symbol Tables)")
				Dim _Modified_SymTbls As Runtime.SymbolTablesSnapshot = _InputSymbolTables

				REM VariableAssignment Execution Process:
				'	- Ensure that there exists an entry with Me.TargetVariable_Identifier in the _InputSymbolTables
				'	- Resolve() Me.AssignmentExpr, to [the current IDV Type of the SymTblEntry with Me.TargetVariable_Identifier]
				'	- Update it's Value to [the Resolved Me.AssignmentExpr]

				If Not _Modified_SymTbls.IsContainedInAny(Me.TargetVariable_Identifier) Then Throw New DSNonexistentSymbolException(Me.TargetVariable_Identifier, "The VariableAssignment can therefore not occur")

				'Resolve() the AssignmentExpr
				Dim _AssignmentExpr_Resolution_ExeRes As ExecutionResult = Me.AssignmentExpr.Resolve(_Modified_SymTbls)

				'Add as an Upstairs ExeRes
				_ExeRes.UpstairsExecutionResults.Add(New Tuple(Of String, ExecutionResult)("AssignmentExpr", _AssignmentExpr_Resolution_ExeRes))

				'Update my SymbolTables
				_Modified_SymTbls = _AssignmentExpr_Resolution_ExeRes.ResultantSymbolTablesState

				'Extract the Resolved Expr
				Dim _Resolved_AssignmentExpr As Variables.IDataValue = _AssignmentExpr_Resolution_ExeRes.ReturnStatus.IExpression_ResolutionResult

				'Coerce() the _Resolved_AssignmentExpr into [the current IDV Type of the SymTblEntry with Me.TargetVariable_Identifier]
				Dim _CurrentSymTblEntry_IDVType As Type = Runtime.SymbolTable.ExtractVariableValue_FromSymbolTableEntry(_Modified_SymTbls.GetEntryFromAny(Me.TargetVariable_Identifier)).GetType()
				LogExecutionMessage("VariableAssignment for " & Me.TargetVariable_Identifier.InSquares() & ": Current SymbolTable Entry is of type " & _CurrentSymTblEntry_IDVType.FullName.InSquares() & ", and Resolved AssignmentExpr is of type " & _Resolved_AssignmentExpr.GetType().FullName.InSquares())
				_Resolved_AssignmentExpr = Variables.CoerceIDV_IntoTargetIDVType_NoGenericModifier(_Resolved_AssignmentExpr, _CurrentSymTblEntry_IDVType)

				'Update the SymTbl Entry
				_Modified_SymTbls.UpdateEntryInAny(Me.TargetVariable_Identifier, CType(_Resolved_AssignmentExpr, Runtime.SymbolTable.ISymbolTableValue))

				Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_Modified_SymTbls)
			Catch _Ex As Exception : Throw New DSException("@VariableDeclaration\Execute: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>Returns an XML Representation of the Instruction, with all Propities and Child Structres included</summary>
		Public Function GetProgramTreeNodeXML() As System.Xml.Linq.XElement Implements IInstruction.GetProgramTreeNodeXML

			Dim _XElementToReturn As XElement = <VariableAssignment TargetVariable_Identifier=<%= Me.TargetVariable_Identifier %>/>
			_XElementToReturn.Add(Me.AssignmentExpr.GetExprTreeNodeXML().WrapIn("AssignmentExpr"))

			Return _XElementToReturn

		End Function

		''' <summary>Constructs an instance of the IInstruction class from its XML serialasation</summary>
		Public Shared Function FromXML(ByVal _IInstructionXElement As XElement) As VariableAssignment
			Try

				Return New VariableAssignment() With {
				 .TargetVariable_Identifier = _IInstructionXElement.@TargetVariable_Identifier,
				 .AssignmentExpr = Expressions.ExprUtilities.ConstructExpressionFromXML(_IInstructionXElement.<AssignmentExpr>.First().Elements().First())
				}

			Catch _Ex As Exception : Throw New DSException("@VariableAssignment\FromXML: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>Returns what the Instruction would have looked like in the Source, without an extra LineBreak on the End</summary>
		Public Overrides Function ToString() As String

			REM E.g.
			'	NAME : "Ben"

			Return _
			 Me.TargetVariable_Identifier & Constants.Space & Constants.AssignmentOperator & Constants.Space & Me.AssignmentExpr.ToString()

		End Function

	End Class

End Namespace