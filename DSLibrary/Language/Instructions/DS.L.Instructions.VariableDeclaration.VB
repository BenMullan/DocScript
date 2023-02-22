Namespace Language.Instructions

	''' <summary>Represents E.g. [&lt;String&gt; Name : "Ben"]</summary>
	Public Class VariableDeclaration : Implements DocScript.Language.Instructions.IInstruction

#Region "Declarations specific to this IInstruction"

		Public DataType As Type	'An IDataValue-based Type
		Public Identifier As [String]
		Public AssignmentExpr As Language.Expressions.IExpression = Nothing

#End Region

		Protected ReadOnly Tokens_ As Runtime.Token()

		''' <summary>Constructs the IInstruction from its Tokens</summary>
		Sub New(ByRef _Tokens As Runtime.Token())
			Me.Tokens_ = _Tokens : Me.ProcessTokensToInitialiseFields()
		End Sub

		Public Sub ProcessTokensToInitialiseFields()
			Try : LogLexingMessage("Began constructing a VariableDeclaration...")

				REM Source should look like:
				REM		<String> Name
				REM		<Boolean@> _Pixels : GetImageRow(0)

				REM Tokens should look like:
				REM		[GrammarChar], [DataType], [GrammarChar], [Identifier], [LineEnd]
				REM		[GrammarChar], [DataType], [GrammarChar], [Identifier], [ExprTokens...], [LineEnd]

				REM Fields to Initialise:
				REM		DataType
				REM		Identifier
				REM		AssignmentExpr	(Can be Nothing)

				REM Ensure that there are enough tokens to construct the IInstruction
				If Me.Tokens_.Count < Runtime.TokenPatternValidation.MinimumRequiredTokens.Item(Me.GetType()) _
				 Then Throw New DSValidationException(String.Format("The No. Tokens ({0}) was less than the minimum No. Tokens required to construct the Instruction ({1}).", Me.Tokens_.Count.ToString(), Runtime.MinimumRequiredTokens(Me.GetType())), "Tokens: " & Me.Tokens_.GetValuesAndLocationsString())

				REM Ensure that the last Token is a {LineEnd}
				If Not Runtime.BuiltInTPVs.LineEnd_TPV.IsSatisfiedBy(Me.Tokens_.Last()) _
				 Then Throw New DSUnexpectedTokenException("The Last Token was not a {LineEnd}", Me.Tokens_)

				REM Ensure that the main TPV herefor is satisfied
				Runtime.BuiltInTPVs.VariableDeclaration_UpToIncIdentifier_TPV.EnsureIsSatisfiedBy(Me.Tokens_)

				REM The DataType should be derivable from the 2nd Token
				Me.DataType = DocScript.Language.Variables.VariableUtilities.GetDSVariableType_FromDataTypeString(Me.Tokens_(1).Value)

				REM The Identifier should be derivable from the 4th Token
				Me.Identifier = Me.Tokens_(3).Value

				REM If there is an AssignmentExpr, derive it from all Tokens after the 5th one (6th onwards...)
				If Me.Tokens_.Length > 5 Then 'There should be an AssignmentExpr

					'There should be at least 7 Tokens
					'<String> Name : "Ben" {LineEnd}
					If Not (Me.Tokens_.Length >= 7) _
					  Then Throw New DSUnexpectedTokenException("Despite there being [more Tokens than the number needed for a VariableDeclaration without an Assignment Expression], there were not at least 7 Tokens, which is the minimum number required for a VariableDeclaration with an AssignmentExpr", Me.Tokens_)

					'Token 4 should be the Assignment Operator
					If Not Runtime.BuiltInTPVs.AssignmentOperator_TPV.IsSatisfiedBy(Me.Tokens_(4)) _
					 Then Throw New DSUnexpectedTokenException("Despite there being [more Tokens than the number needed for a VariableDeclaration without an Assignment Expression], the 5th Token (Index 4) was not an Assignment Operator " & Constants.AssignmentOperator.ToString().InBrackets(), Me.Tokens_)

					'Tokens after Token 4 (5 onwards...) should form the AssignmentExpr, up to the {LineEnd}
					Me.AssignmentExpr = Expressions.ConstructExpressionFromTokens(
					 Me.Tokens_ _
					 .Skip(5).ToArray() _
					 .UpToButExcluding(AddressOf Runtime.BuiltInTPVs.LineEnd_TPV.IsSatisfiedBy)
					)

				End If

				LogLexingMessage("...Finished constructing a VariableDeclaration Object for " & Language.Constants.OpeningDataTypeBracket & Variables.VariableUtilities.GetDataTypeString_FromDSVariableType(Me.DataType) & Language.Constants.ClosingDataTypeBracket & " "c & Me.Identifier.InSquares())
			Catch _Ex As Exception : Throw New DSException("@VariableDeclaration\ProcessTokensToInitialiseFields: " & _Ex.Message, _Ex) : End Try
		End Sub

		''' <summary>Any Expressions or Child Instructions executed hereby, are returned in the ExeRes's UpstairsExecutionResults member.</summary>
		Public Function Execute(ByVal _InputSymbolTables As Runtime.SymbolTablesSnapshot) As ExecutionResult Implements IInstruction.Execute
			Try : Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("VariableDeclaration " & Me.Identifier.InBrackets())

				LogExecutionMessage("(VariableDeclaration: Received " & _InputSymbolTables.InnerSymbolTables_Count.ToString() & " Input Symbol Tables)")
				Dim _Modified_SymTbls As Runtime.SymbolTablesSnapshot = _InputSymbolTables

				REM VariableDeclaration Execution Process:
				'	- Resolve() Me.AssignmentExpr if this isn't Nothing
				'	- Coerce() this resolved Expr into Me.DataType
				'	- Add a new SymTbl Entry with [the Resolved Me.AssignmentExpr], or [My DataType's IDV's NullValue]

				'↓ ONLY Initialised, *if* Me.AssignmentExpr IsNot Nothing
				Dim _Resolved_AssignmentExpr As Variables.IDataValue = Nothing

				If Me.AssignmentExpr IsNot Nothing Then

					'Resolve()
					Dim _AssignmentExpr_Resolution_ExeRes As ExecutionResult = _
					 Me.AssignmentExpr.Resolve(_Modified_SymTbls)

					'Add as an Upstairs ExeRes
					_ExeRes.UpstairsExecutionResults.Add(New Tuple(Of String, ExecutionResult)("AssignmentExpr", _AssignmentExpr_Resolution_ExeRes))

					'Update my SymbolTables
					_Modified_SymTbls = _AssignmentExpr_Resolution_ExeRes.ResultantSymbolTablesState

					'Extract the Resolved Expr
					_Resolved_AssignmentExpr = _AssignmentExpr_Resolution_ExeRes.ReturnStatus.IExpression_ResolutionResult

					'Coerce() the Resolved Expr into Me.DataType (i.e. The <Number> or <String@> that was typed in the Source)
					LogExecutionMessage("VariableDeclaration for " & Me.Identifier.InSquares() & ": Declared DataType is " & Me.DataType.FullName.InSquares() & ", and Resolved AssignmentExpr is of type " & _Resolved_AssignmentExpr.GetType().FullName.InSquares())
					_Resolved_AssignmentExpr = Variables.CoerceIDV_IntoTargetIDVType_NoGenericModifier(_Resolved_AssignmentExpr, Me.DataType)

				End If

				REM Procure a SymbolTableEntry(Of TheCorrectType)
				REM In the GetSymbolTableEntry_FromIDataValue Function, if the _InputIDV Is Nothing, then an InstanceWillNullValue IDV will be used for Me.DataType:
				Dim _GeneratedSymTblEntry As Runtime.SymbolTable.ISymbolTableEntry = _
				 Runtime.SymbolTable.GetSymbolTableEntry_FromIDataValue(_InputIDV:=_Resolved_AssignmentExpr, _NullValueType_IfInputIDVIsNothing:=Me.DataType)

				REM Add an Entry into the Topmost SymbolTable, for Me.Identifier
				_Modified_SymTbls.AddEntryToToTopmost(Me.Identifier, _GeneratedSymTblEntry)

				Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_Modified_SymTbls)
			Catch _Ex As Exception : Throw New DSException("@VariableDeclaration\Execute: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>Returns an XML Representation of the Instruction, with all Propities and Child Structres included</summary>
		Public Function GetProgramTreeNodeXML() As System.Xml.Linq.XElement Implements IInstruction.GetProgramTreeNodeXML

			Dim _XElementToReturn As XElement = <VariableDeclaration DataType=<%= Variables.GetDataTypeString_FromDSVariableType(Me.DataType) %> Identifier=<%= Me.Identifier %>/>
			_XElementToReturn.Add(If(Me.AssignmentExpr Is Nothing, <AssignmentExpr/>, Me.AssignmentExpr.GetExprTreeNodeXML().WrapIn("AssignmentExpr")))

			Return _XElementToReturn

		End Function

		''' <summary>Returns what the Instruction would have looked like in the Source, without an extra LineBreak on the End</summary>
		Public Overrides Function ToString() As String

			REM E.g.
			'	<STRING> NAME : "Ben"

			Return _
			 Constants.OpeningDataTypeBracket & Variables.VariableUtilities.GetDataTypeString_FromDSVariableType(Me.DataType) & Constants.ClosingDataTypeBracket & Constants.Space & Me.Identifier & If(Me.AssignmentExpr Is Nothing, "", Constants.Space & Constants.AssignmentOperator & Constants.Space & Me.AssignmentExpr.ToString())

		End Function

	End Class

End Namespace