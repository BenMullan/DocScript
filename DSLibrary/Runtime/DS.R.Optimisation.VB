'To preclude having to type Language.Expressions and Language.Variables and Language.Instructions
Imports DocScript.Language.Expressions, DocScript.Language.Variables, DocScript.Language.Instructions

Namespace Runtime

	REM Possible Optimisations to implement:
	'	- Constant Expression (compile-time) Evaluation (e.g. [5 + 4] → [9])
	'	- Bitwise-shift instead of power-of-two multiplications (e.g. [14 * 8] → [14 << 3])
	'	- Branchless-ing
	'	- Redundant call/statement removal
	'	- Constant Substitution (instead of variable reference)
	'	- Unreachable code elimination
	'	- Loop unrolling (e.g. if there are only two iterations)

	REM Interesting, but not relevant for DS: 
	'	- Optimising the linking order, to reduce paging for larger programs
	'	- Struct-padding for memory alignment

	''' <summary>Contains resources for optimising a DocScript Program to make it run more efficiently</summary>
	Public Module Optimisation

		''' <summary>
		''' The reason this is a Class and not just methods in a Module, is because the PerformedOptimisations are specific to the optimisation of a given Component (Expression, IInstruction, or Program).
		''' If the PerformedOptimisations dictionary was just floating in the Module, then the tracking of which optimisations have occured would break in a multi-threaded scenario.
		''' </summary>
		Public Class ComponentOptimiser

			Public Enum OptimisationAction As [Byte]
				ConstantExpressionEvaluation
				BitShiftForPowerOfTwoMultiplication
				ConversionToBranchless
				RedundantCallOrStatementRemoval
				ConstToLiteralSubstitution
				UnreachableCodeElimination
				LoopUnrolling
				RecursionToIteration
			End Enum

			Public ReadOnly PerformedOptimisations As New Dictionary(Of OptimisationAction, [Boolean])() From {
			 {OptimisationAction.ConstantExpressionEvaluation, False},
			 {OptimisationAction.BitShiftForPowerOfTwoMultiplication, False},
			 {OptimisationAction.ConversionToBranchless, False},
			 {OptimisationAction.RedundantCallOrStatementRemoval, False},
			 {OptimisationAction.ConstToLiteralSubstitution, False},
			 {OptimisationAction.UnreachableCodeElimination, False},
			 {OptimisationAction.LoopUnrolling, False},
			 {OptimisationAction.RecursionToIteration, False}
			}

			Public ReadOnly Property PerformedOptimisationsReport As [String]
				Get

					Return _
					 "---------- Optimisation Report ----------" & vbCrLf & _
					 String.Join(vbCrLf, Me.PerformedOptimisations.ToArray().Select(Of [String])(
					  Function(_OptimisationAction_ToPerformedFlag As KeyValuePair(Of OptimisationAction, [Boolean])) (_OptimisationAction_ToPerformedFlag.Key.ToString("G").CamelCaseToSpacedString() & ":").PadRight(totalWidth:=Me.PerformedOptimisations.Keys.Select(Of Int32)(Function(_OptimisationAction As OptimisationAction) _OptimisationAction.ToString("G").CamelCaseToSpacedString().Length + 6).Max()) & If(_OptimisationAction_ToPerformedFlag.Value, "Performed", "Not Applicable")
					 ).ToArray())

				End Get
			End Property

			''' <summary>Returns an optimised version of _Program</summary>
			Public Function OptimiseProgram(ByVal _UnoptimisedProgram As Runtime.Program) As Runtime.Program
				Try

					REM Replace each GlobalVarDec Expression with an optimised version
					'(For some reason, AddressOf dosen't work here)
					Dim _OptimisedGlobalVarDecs As VariableDeclaration() = _
					 Me.OptimiseStatementContents_(_UnoptimisedProgram.GlobalVarDecs.ToArray()) _
					.Select(Of VariableDeclaration)(Function(_IInstruction As IInstruction) _IInstruction.MustBe(Of VariableDeclaration)()).ToArray()

					REM Replace each DSFunction with an optimised version
					Dim _OptimisedDSFunctions As Statements.DSFunction() = _
					 _UnoptimisedProgram.Functions.Select(Of Statements.DSFunction)(Function(_UnoptimisedDSFunction As Statements.DSFunction) New Statements.DSFunction(
					 _Identifier:=_UnoptimisedDSFunction.Identifier,
					 _ReturnType:=_UnoptimisedDSFunction.ReturnType,
					 _Parameters:=_UnoptimisedDSFunction.Parameters.ToArray(),
					 _Contents:=Me.OptimiseStatementContents_(_UnoptimisedDSFunction.Contents.ToArray()).ToList()
					)).ToArray()

					LogLexingMessage("...Finished optimising Program. Report: " & Me.PerformedOptimisationsReport)

					REM Return a New Program(), with the Optimisations in-place
					Return New Program(_OptimisedGlobalVarDecs, _OptimisedDSFunctions, _UnoptimisedProgram.ExecutionContext) With {
					 .HasBeenOptimised = True,
					 .Name = _UnoptimisedProgram.Name, .Author = _UnoptimisedProgram.Author, .CopyrightMessage = _UnoptimisedProgram.CopyrightMessage,
					 .Description = _UnoptimisedProgram.Description, .Version = _UnoptimisedProgram.Version
					}

				Catch _Ex As Exception : Throw New DSException("@OptimiseProgram: " & _Ex.Message, _Ex) : End Try
			End Function

			''' <summary>Recursively optimises the Contents of an IStatement</summary>
			Private Function OptimiseStatementContents_(ByVal _UnoptimisedIInstructions As IInstruction()) As IInstruction()
				Try

					Dim _OptimisedIInstructions As New List(Of IInstruction)()

					For Each _IInstruction As IInstruction In _UnoptimisedIInstructions
						Select Case _IInstruction.GetType()

							Case GetType(VariableDeclaration)
								_OptimisedIInstructions.Add(New VariableDeclaration(
								 _Identifier:=CType(_IInstruction, VariableDeclaration).Identifier,
								 _DataType:=CType(_IInstruction, VariableDeclaration).DataType,
								 _AssignmentExpr:=If(CType(_IInstruction, VariableDeclaration).AssignmentExpr Is Nothing, Nothing, Me.OptimiseExpression(CType(_IInstruction, VariableDeclaration).AssignmentExpr))
								))

							Case GetType(VariableAssignment)
								_OptimisedIInstructions.Add(New VariableAssignment(
								 _TargetVariable_Identifier:=CType(_IInstruction, VariableAssignment).TargetVariable_Identifier,
								  _AssignmentExpr:=If(CType(_IInstruction, VariableAssignment).AssignmentExpr Is Nothing, Nothing, Me.OptimiseExpression(CType(_IInstruction, VariableAssignment).AssignmentExpr))
								))

							Case GetType(FunctionCall)
								_OptimisedIInstructions.Add(New FunctionCall(
								 _TargetFunction_Identifier:=CType(_IInstruction, FunctionCall).TargetFunction_Identifier,
								  _Arguments:=CType(_IInstruction, FunctionCall).Arguments.Select(Of IExpression)(AddressOf Me.OptimiseExpression).ToArray()
								))

							Case GetType(ReturnToCaller)
								_OptimisedIInstructions.Add(New ReturnToCaller(
								  _ReturnValueExpr:=If(CType(_IInstruction, ReturnToCaller).ReturnValueExpr Is Nothing, Nothing, Me.OptimiseExpression(CType(_IInstruction, ReturnToCaller).ReturnValueExpr))
								 ))

							Case GetType(Statements.IfStatement)
								_OptimisedIInstructions.Add(New Statements.IfStatement(
								 _ConditionExpr:=Me.OptimiseExpression(CType(_IInstruction, Statements.IfStatement).ConditionExpr),
								 _Contents:=Me.OptimiseStatementContents_(CType(_IInstruction, Statements.IfStatement).Contents.ToArray()).ToList(),
								 _ElseContents:=If(CType(_IInstruction, Statements.IfStatement).ElseContents Is Nothing, Nothing, Me.OptimiseStatementContents_(CType(_IInstruction, Statements.IfStatement).ElseContents.ToArray()).ToList())
								))

							Case GetType(Statements.LoopStatement)
								_OptimisedIInstructions.Add(New Statements.LoopStatement(
								 _CountExpr:=Me.OptimiseExpression(CType(_IInstruction, Statements.LoopStatement).CountExpr),
								 _Contents:=Me.OptimiseStatementContents_(CType(_IInstruction, Statements.IfStatement).Contents.ToArray()).ToList()
								))

							Case GetType(Statements.WhileStatement)
								_OptimisedIInstructions.Add(New Statements.WhileStatement(
								 _ConditionExpr:=Me.OptimiseExpression(CType(_IInstruction, Statements.WhileStatement).ConditionExpr),
								 _Contents:=Me.OptimiseStatementContents_(CType(_IInstruction, Statements.WhileStatement).Contents.ToArray()).ToList()
								))

							Case Else : Throw New DSValidationException("The Statement-Contents Instruction could not be optimised because its Type was unaccounted-for", _IInstruction.GetType().FullName)
						End Select
					Next

					Return _OptimisedIInstructions.ToArray()

				Catch _Ex As Exception : Throw New DSException("@OptimiseStatementContents_: " & _Ex.Message, _Ex) : End Try
			End Function

#Region "Expression Optimisation"

			''' <summary>
			''' Applies the following optimisations:
			''' 	- Constant Expression Evaluation (e.g. [5 + 4] → [9])
			''' 	- *
			''' </summary>
			Public Function OptimiseExpression(ByVal _InputExpression As IExpression) As IExpression
				Try : LogLexingMessage("Optimising Expression: " & _InputExpression.ToString() & "...")

					REM If the Expression is an OperatorExpr, with all it's operands of type LiteralExpr, then it can undergo compile-time-evaluation optimisation
					_InputExpression = Me.SubstituteConstantExpr_WithEvaluationThereof_(_InputExpression)

					LogLexingMessage("...Finished optimising Expression. Report: " & Me.PerformedOptimisationsReport)
					OptimiseExpression = _InputExpression

				Catch _Ex As Exception : Throw New DSException("@OptimiseExpression: " & _Ex.Message, _Ex) : End Try
			End Function

			Private Function SubstituteConstantExpr_WithEvaluationThereof_(ByVal _InputExpression As IExpression) As IExpression
				Try

					REM We must be passed one of these: LiteralExpr, VariableExpr, FunctionCallExpr, OperatorExpr
					If Not Language.Expressions.ExprUtilities.ExprTree_PermittedIExprTypes.Contains(_InputExpression.GetType()) _
					 Then Throw New DSValidationException("The _InputExpression's Type was not valid for the optimisation method", _InputExpression.GetType().FullName)

					REM If it's a LiteralExpr or VariableExpr, then it can't be optimised any further
					If {GetType(VariableExpr), GetType(LiteralExpr(Of DSBoolean)), GetType(LiteralExpr(Of DSNumber)), GetType(LiteralExpr(Of DSString))} _
					 .Contains(_InputExpression.GetType()) Then Return _InputExpression

					REM At this point, the InputExpr should be an ICompoundExpr (FunctionCallExpr or OperatorExpr)
					If Not _InputExpression.ImplementsInterface(Of ICompoundExpression)() Then Throw New DSValidationException("Despite not being a LiteralExpr or VariableExpr, the _InputExpression does not implement ICompoundExpression", _InputExpression.GetType().FullName)

					REM We don't even need to work out weather this is an OperatorExpr or whatever;
					REM ...We'll just try to Resolve() it, and if that works, then it *is* compile-time-resolvable.
					Try

						Dim _ResolvedExpr As IDataValue = _InputExpression.Resolve(SymbolTablesSnapshot.Empty).ReturnStatus _
						.IExpression_ResolutionResult.MustNotBeNothing("The Expression was resolvable at compile-time, but resolved to [Nothing]. Expression: " & vbCrLf & _InputExpression.ToString())

						LogLexingMessage("The Input Expression was successfully resolved from ↓" & vbCrLf & _InputExpression.ToString() & "to ↓" & vbCrLf & _ResolvedExpr.ToString())
						Me.PerformedOptimisations.Item(OptimisationAction.ConstantExpressionEvaluation) = True

						Select Case _ResolvedExpr.GetType()
							Case GetType(DSString) : Return New LiteralExpr(Of DSString)(CType(_ResolvedExpr, DSString))
							Case GetType(DSNumber) : Return New LiteralExpr(Of DSNumber)(CType(_ResolvedExpr, DSNumber))
							Case GetType(DSBoolean) : Return New LiteralExpr(Of DSBoolean)(CType(_ResolvedExpr, DSBoolean))
							Case Else : Throw New DSValidationException("The resolved expression did not have a valid literal-compatible type", _ResolvedExpr.GetType().FullName)
						End Select

					Catch _Ex As Exception

						REM If we're here, then although the Expression isn't compile-time-resolvable, its Sub-Expressions might still be
						Dim _SubExprs_ResolvedWherePossible As IExpression() = CType(_InputExpression, ICompoundExpression) _
						 .SubExpressions.Select(AddressOf Me.SubstituteConstantExpr_WithEvaluationThereof_).ToArray()

						REM Now re-assemble those Sub-Expressions into an ICompoundExpression to return
						Select Case _InputExpression.GetType()
							Case GetType(FunctionCallExpr) : Return New FunctionCallExpr(CType(_InputExpression, FunctionCallExpr).TargetFunction_Identifier, _SubExprs_ResolvedWherePossible)
							Case GetType(OperatorExpr) : Return New OperatorExpr(CType(_InputExpression, OperatorExpr).OperatorChar, _SubExprs_ResolvedWherePossible)
							Case Else : Throw New DSValidationException("The ICompoundExpression Type was unaccounted-for. The resolved-where-possible SubExpressions cannot be re-assembled into their parent Expression", _InputExpression.GetType().FullName)
						End Select

					End Try

					REM Examples:
					'	5 + [4 - 2]				7
					'	F(5) ' False			(No Change)
					'	F(5 + 4)				F(9)

				Catch _Ex As Exception : Throw New DSException("@SubstituteConstantExpr_WithEvaluationThereof_: " & _Ex.Message, _Ex) : End Try
			End Function

#End Region

		End Class

	End Module

End Namespace