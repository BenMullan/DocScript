'To preclude having to type Language.Expressions and Language.Variables and Language.Instructions
Imports DocScript.Language.Expressions, DocScript.Language.Variables, DocScript.Language.Instructions

Namespace Runtime

	REM Possible Optimisations to implement:
	'	- [DONE] Constant Expression (compile-time) Evaluation (e.g. [5 + 4] → [9])
	'	- [DONE] Bitwise-shift instead of power-of-two multiplications (e.g. [Age * 8] → [Maths_Bitshift_Left(Age, 3)])		'Same could be done for Po2 Division
	'	- Branchless-ing
	'	- Redundant call/statement removal
	'	- Constant Substitution (instead of variable reference)
	'	- [DONE] Unreachable code elimination
	'	- Loop unrolling (e.g. if there are only two iterations)
	'	- Recursion to Iteration

	REM Interesting, but not relevant for DS:
	'	- Optimising the linking order, to reduce paging for larger programs
	'	- Struct-padding for memory alignment

	''' <summary>Contains resources for optimising a DocScript Program to make it run more efficiently</summary>
	Public Module Optimisation

		''' <summary>
		''' The reason this is a Class, and not just methods in a Module, is because the PerformedOptimisationsTally is specific to the optimisation of a given Component (Expression, IInstruction, or Program).
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

			Public ReadOnly PerformedOptimisationsTally As New Dictionary(Of OptimisationAction, UInt64)() From {
			 {OptimisationAction.ConstantExpressionEvaluation, 0},
			 {OptimisationAction.BitShiftForPowerOfTwoMultiplication, 0},
			 {OptimisationAction.ConversionToBranchless, 0},
			 {OptimisationAction.RedundantCallOrStatementRemoval, 0},
			 {OptimisationAction.ConstToLiteralSubstitution, 0},
			 {OptimisationAction.UnreachableCodeElimination, 0},
			 {OptimisationAction.LoopUnrolling, 0},
			 {OptimisationAction.RecursionToIteration, 0}
			}

			Public ReadOnly Property PerformedOptimisationsReport As [String]
				Get

					Dim _MaxOptimisationActionStringLength% = _
					 Me.PerformedOptimisationsTally.Keys.Select(Of Int32)(Function(_OptimisationAction As OptimisationAction) _OptimisationAction.ToString("G").CamelCaseToSpacedString().Length).Max()

					Return _
					 "---------- Optimisation Report (Tally) ----------" & vbCrLf & _
					 [String].Join(vbCrLf, Me.PerformedOptimisationsTally.ToArray().Select(Of [String])(
					  Function(_OptimisationAction_ToPerformedFlag As KeyValuePair(Of OptimisationAction, UInt64)) As [String]
						  Return (_OptimisationAction_ToPerformedFlag.Key.ToString("G").CamelCaseToSpacedString() & ":").PadRight(totalWidth:=_MaxOptimisationActionStringLength + 6, paddingChar:=" "c) & _OptimisationAction_ToPerformedFlag.Value
					  End Function
					 ).ToArray())

					REM Example Output:
					'	---------- Optimisation Report (Tally) ----------
					'	Constant Expression Evaluation:                1
					'	Bit Shift For Power Of Two Multiplication:     0
					'	Conversion To Branchless:                      0
					'	Redundant Call Or Statement Removal:           0
					'	Const To Literal Substitution:                 0
					'	Unreachable Code Elimination:                  0
					'	Loop Unrolling:                                0
					'	Recursion To Iteration:                        0

				End Get
			End Property

			''' <summary>Returns an optimised version of _Program</summary>
			Public Function OptimiseProgram(ByVal _UnoptimisedProgram As Runtime.Program) As Runtime.Program
				Try : LogLexingMessage("Optimising this Program: " & _UnoptimisedProgram.ToString())

					REM Replace each GlobalVarDec Expression with an optimised version
					'(For some reason, AddressOf dosen't work here)
					Dim _OptimisedGlobalVarDecs As VariableDeclaration() = _
					 Me.ApplyExpressionOptimisationToStatementContents_(_UnoptimisedProgram.GlobalVarDecs.ToArray()) _
					.Select(Of VariableDeclaration)(Function(_IInstruction As IInstruction) _IInstruction.MustBe(Of VariableDeclaration)()).ToArray()

					REM Replace each DSFunction with an optimised version
					Dim _OptimisedDSFunctions As Statements.DSFunction() = _
					 _UnoptimisedProgram.Functions.Select(Of Statements.DSFunction)(Function(_UnoptimisedDSFunction As Statements.DSFunction) New Statements.DSFunction(
					 _Identifier:=_UnoptimisedDSFunction.Identifier,
					 _ReturnType:=_UnoptimisedDSFunction.ReturnType,
					 _Parameters:=_UnoptimisedDSFunction.Parameters.ToArray(),
					 _Contents:=Me.OptimiseStatementContents(_UnoptimisedDSFunction.Contents.ToArray()).ToList()
					)).ToArray()

					LogLexingMessage("...Finished optimising Program. Report: " & Me.PerformedOptimisationsReport, LogEvent.DSEventSeverity.Infomation)

					REM Return a New Program(), with the Optimisations in-place
					Return New Program(_OptimisedGlobalVarDecs, _OptimisedDSFunctions, _UnoptimisedProgram.ExecutionContext) With {
					 .HasBeenOptimised = True,
					 .Name = _UnoptimisedProgram.Name, .Author = _UnoptimisedProgram.Author, .CopyrightMessage = _UnoptimisedProgram.CopyrightMessage,
					 .Description = _UnoptimisedProgram.Description, .Version = _UnoptimisedProgram.Version
					}

				Catch _Ex As Exception : Throw New DSException("@OptimiseProgram: " & _Ex.Message, _Ex) : End Try
			End Function

#Region "IInstruction Optimisation"

			''' <summary>
			''' Applies the following optimisations:
			''' 	- Removal of unreachable instructions
			''' 	- (Expression-based optimisation on any Expressions contained within the _IInstructions)
			''' </summary>
			Public Function OptimiseStatementContents(ByVal _IInstructions As IInstruction()) As IInstruction()
				Try : LogLexingMessage("Optimising Statement Contents comprising " & _IInstructions.Length.ToString() & " IInstructions...")

					_IInstructions = Me.RemoveUnreachableIInstructions_(_IInstructions)
					_IInstructions = Me.ApplyExpressionOptimisationToStatementContents_(_IInstructions)

					LogLexingMessage("...Finished optimising StatementContents. Report: " & Me.PerformedOptimisationsReport)
					Return _IInstructions

				Catch _Ex As Exception : Throw New DSException("@OptimiseStatementContents: " & _Ex.Message, _Ex) : End Try
			End Function

			''' <summary>Recursively puts any IExpressions contained in the Instructions through OptimiseExpression()</summary>
			Private Function ApplyExpressionOptimisationToStatementContents_(ByVal _UnoptimisedIInstructions As IInstruction()) As IInstruction()
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
								 _Contents:=Me.ApplyExpressionOptimisationToStatementContents_(CType(_IInstruction, Statements.IfStatement).Contents.ToArray()).ToList(),
								 _ElseContents:=If(CType(_IInstruction, Statements.IfStatement).ElseContents Is Nothing, Nothing, Me.ApplyExpressionOptimisationToStatementContents_(CType(_IInstruction, Statements.IfStatement).ElseContents.ToArray()).ToList())
								))

							Case GetType(Statements.LoopStatement)
								_OptimisedIInstructions.Add(New Statements.LoopStatement(
								 _CountExpr:=Me.OptimiseExpression(CType(_IInstruction, Statements.LoopStatement).CountExpr),
								 _Contents:=Me.ApplyExpressionOptimisationToStatementContents_(CType(_IInstruction, Statements.LoopStatement).Contents.ToArray()).ToList()
								))

							Case GetType(Statements.WhileStatement)
								_OptimisedIInstructions.Add(New Statements.WhileStatement(
								 _ConditionExpr:=Me.OptimiseExpression(CType(_IInstruction, Statements.WhileStatement).ConditionExpr),
								 _Contents:=Me.ApplyExpressionOptimisationToStatementContents_(CType(_IInstruction, Statements.WhileStatement).Contents.ToArray()).ToList()
								))

							Case Else : Throw New DSValidationException("The Statement-Contents Instruction could not be optimised because its Type was unaccounted-for", _IInstruction.GetType().FullName)
						End Select
					Next

					Return _OptimisedIInstructions.ToArray()

				Catch _Ex As Exception : Throw New DSException("@OptimiseStatementContents_: " & _Ex.Message, _Ex) : End Try
			End Function

			''' <summary>Recursively optimises the Contents of an IStatement. This is not an OptimisationAction itself, but passes the IInstructions through other OptimisationActions.</summary>
			Private Function RemoveUnreachableIInstructions_(ByVal _IInstructions As IInstruction()) As IInstruction()
				Try

					REM Only deal with IInstructions which are reachable
					Dim _ReachableInstructions As IInstruction() = _IInstructions
					If (_IInstructions.OfType(Of ReturnToCaller)().Count() > 0) _
					 AndAlso (_IInstructions.IndexOfFirstWhere(Function(_IInstruction As IInstruction) TypeOf _IInstruction Is ReturnToCaller) < (_IInstructions.Length - 1)) _
					Then _ReachableInstructions = _IInstructions.UpToAndIncluding(Function(_IInstruction As IInstruction) TypeOf _IInstruction Is ReturnToCaller)

					REM Determine if any Instructions have been skiped
					If _ReachableInstructions.Count() < _IInstructions.Count() Then
						LogLexingMessage((_IInstructions.Count() - _ReachableInstructions.Count()).ToString() & " Unreachable Instruction(s) will be removed")
						Me.PerformedOptimisationsTally.Item(OptimisationAction.UnreachableCodeElimination) += 1UL
					End If

					REM The _ReachableInstructions may contain some IStatement Instructions.
					REM These should have their Contents/ElseContents put through RemoveUnreachableIInstructions_() before being returned.
					REM All the Instructions to return will be added to _IInstructionsToReturn

					Dim _IInstructionsToReturn As New List(Of IInstruction)()

					For Each _IInstruction As IInstruction In _ReachableInstructions
						If _IInstruction.ImplementsInterface(Of Statements.IStatement)() Then

							REM It's an IStatement with child Instructions; Add() it, with all its child Instructions put through RemoveUnreachableIInstructions_()
							Dim _SubStatementInstructions_WithUnreachableOnesRemoved As IInstruction() = _
							 Me.RemoveUnreachableIInstructions_(CType(_IInstruction, Statements.IStatement).Contents.ToArray())

							Select Case _IInstruction.GetType()

								Case GetType(Statements.IfStatement)
									_IInstructionsToReturn.Add(New Statements.IfStatement(
									 _ConditionExpr:=CType(_IInstruction, Statements.IfStatement).ConditionExpr,
									 _Contents:=Me.RemoveUnreachableIInstructions_(CType(_IInstruction, Statements.IfStatement).Contents.ToArray()).ToList(),
									 _ElseContents:=If(CType(_IInstruction, Statements.IfStatement).ElseContents Is Nothing, Nothing, Me.RemoveUnreachableIInstructions_(CType(_IInstruction, Statements.IfStatement).ElseContents.ToArray()).ToList())
									))

								Case GetType(Statements.LoopStatement)
									_IInstructionsToReturn.Add(New Statements.LoopStatement(
									 _CountExpr:=CType(_IInstruction, Statements.LoopStatement).CountExpr,
									 _Contents:=Me.RemoveUnreachableIInstructions_(CType(_IInstruction, Statements.LoopStatement).Contents.ToArray()).ToList()
									))

								Case GetType(Statements.WhileStatement)
									_IInstructionsToReturn.Add(New Statements.WhileStatement(
									 _ConditionExpr:=CType(_IInstruction, Statements.WhileStatement).ConditionExpr,
									 _Contents:=Me.RemoveUnreachableIInstructions_(CType(_IInstruction, Statements.WhileStatement).Contents.ToArray()).ToList()
									))

								Case Else : Throw New DSValidationException("The IStatement Type was unaccounted-for", _IInstruction.GetType().FullName)
							End Select

						Else

							REM It's not an IStatement and dosen't contain child Instructions; Add() it as it is
							_IInstructionsToReturn.Add(_IInstruction)

						End If
					Next

					Return _IInstructionsToReturn.ToArray()

				Catch _Ex As Exception : Throw New DSException("@RemoveUnreachableIInstructions_: " & _Ex.Message, _Ex) : End Try
			End Function

#End Region

#Region "Expression Optimisation"

			''' <summary>
			''' Applies the following optimisations:
			''' 	- Constant Expression Evaluation (e.g. [5 + 4] → [9])
			''' 	- Bitwise-shift instead of power-of-two multiplications (e.g. [Age * 8] → [Maths_Bitshift_Left(Age, 3)])
			''' </summary>
			Public Function OptimiseExpression(ByVal _InputExpression As IExpression) As IExpression
				Try : LogLexingMessage("Optimising Expression: " & _InputExpression.ToString() & "...")

					_InputExpression = Me.SubstituteConstantExpr_WithEvaluationThereof_(_InputExpression)
					_InputExpression = Me.SubstitutePowerOfTwoMultiplication_WithBitshift_(_InputExpression)

					LogLexingMessage("...Finished optimising Expression. Report: " & Me.PerformedOptimisationsReport)
					Return _InputExpression

				Catch _Ex As Exception : Throw New DSException("@OptimiseExpression: " & _Ex.Message, _Ex) : End Try
			End Function

			''' <summary>Converts Multiplications by powers of two into BitShifts where possible</summary>
			Private Function SubstitutePowerOfTwoMultiplication_WithBitshift_(ByVal _InputExpression As IExpression) As IExpression
				Try

					REM Examples:
					'	(There will never be two constants; this will have been removed by the CompileTimeEval stage)
					'	Age * 8			Maths_Bitshift_Left(Age, 3)
					'	F(9 * 2) * 4	Maths_Bitshift_Left(F(Maths_Bitshift_Left(9, 1)), 2)

					If (TypeOf _InputExpression Is OperatorExpr) AndAlso _
					 (CType(_InputExpression, OperatorExpr).SubExpressions.Count() = 2) AndAlso _
					 (TypeOf CType(_InputExpression, OperatorExpr).SubExpressions.ElementAt(1) Is LiteralExpr(Of DSNumber)) AndAlso _
					 ((CType(CType(_InputExpression, OperatorExpr).SubExpressions.ElementAt(1), LiteralExpr(Of DSNumber)).LiteralValue.Value Mod 1) = 0) AndAlso _
					 (CType(CType(_InputExpression, OperatorExpr).SubExpressions.ElementAt(1), LiteralExpr(Of DSNumber)).LiteralValue.Value > 1) AndAlso _
					 CType(CType(_InputExpression, OperatorExpr).SubExpressions.ElementAt(1), LiteralExpr(Of DSNumber)).LiteralValue.Value.ToString().ExtractUInt32().IsPowerOfTwo() _
					Then

						REM We now know that:
						'	- We have an OperatorExpr...
						'	- ...With two operands...
						'	- ...And the second operand is:
						'		- A DSNumber...
						'		- ...With an integral value...
						'		- ...Which is greater than one...
						'		- ...And a Power-of-two (Po2)

						LogLexingMessage("This Expression will be optimised to a Bitshift operation, from ↓" & vbCrLf & _InputExpression.ToString())
						Me.PerformedOptimisationsTally.Item(OptimisationAction.BitShiftForPowerOfTwoMultiplication) += 1US

						REM Therefore, we can replace it with a FunctionCallExpr to the Maths_Bitshift_Left() BIF
						Return New FunctionCallExpr(
						 _TargetFunction_Identifier:=DocScript.Runtime.StandardBuiltInFunctions.BIF_Maths_Bitshift_Left_Identifier,
						 _Arguments:={
						  CType(_InputExpression, OperatorExpr).SubExpressions.ElementAt(0),
						  New LiteralExpr(Of DSNumber)(New DSNumber(System.Math.Log(CType(CType(_InputExpression, OperatorExpr).SubExpressions.ElementAt(1), LiteralExpr(Of DSNumber)).LiteralValue.Value.ToString().ExtractUInt32(), 2)))
						 }
						)

					Else

						REM The current _InputExpression didn't meet the nessecary criteria, but if it has Children, then they might

						'Only ICompoundExpressions have children
						If Not _InputExpression.ImplementsInterface(Of ICompoundExpression)() Then Return _InputExpression

						Dim _SubExprs_SubstitutedWithBitshiftsWherePossible As IExpression() = CType(_InputExpression, ICompoundExpression) _
						 .SubExpressions.Select(AddressOf Me.SubstitutePowerOfTwoMultiplication_WithBitshift_).ToArray()

						REM Now re-assemble those Sub-Expressions into an ICompoundExpression to return
						Select Case _InputExpression.GetType()
							Case GetType(FunctionCallExpr) : Return New FunctionCallExpr(CType(_InputExpression, FunctionCallExpr).TargetFunction_Identifier, _SubExprs_SubstitutedWithBitshiftsWherePossible)
							Case GetType(OperatorExpr) : Return New OperatorExpr(CType(_InputExpression, OperatorExpr).OperatorChar, _SubExprs_SubstitutedWithBitshiftsWherePossible)
							Case Else : Throw New DSValidationException("The ICompoundExpression Type was unaccounted-for. The resolved-where-possible SubExpressions cannot be re-assembled into their parent Expression", _InputExpression.GetType().FullName)
						End Select

					End If

				Catch _Ex As Exception : Throw New DSException("@SubstitutePowerOfTwoMultiplication_WithBitshift_: " & _Ex.Message, _Ex) : End Try
			End Function

			''' <summary>Performs Compile-time Evaluation where possible</summary>
			Private Function SubstituteConstantExpr_WithEvaluationThereof_(ByVal _InputExpression As IExpression) As IExpression
				Try

					REM Examples:
					'	5 + [4 - 2]				7
					'	F(5) ' False			(No Change)
					'	F(5 + 4)				F(9)

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
						Me.PerformedOptimisationsTally.Item(OptimisationAction.ConstantExpressionEvaluation) += 1UL

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

				Catch _Ex As Exception : Throw New DSException("@SubstituteConstantExpr_WithEvaluationThereof_: " & _Ex.Message, _Ex) : End Try
			End Function

#End Region

		End Class

	End Module

End Namespace