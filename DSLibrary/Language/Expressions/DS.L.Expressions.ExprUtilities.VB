Namespace Language.Expressions

	REM Exmaple Expressions:
	REM		"Hello"
	REM		23_4
	REM		tRuE
	REM		"Hello" & ", World!"
	REM		"Hello, " & GetName()
	REM		"Hello, " & GetFullName("Ben " & "Mullan")
	REM		ToString([5 + ~4]) & "9" & ToString(¬True¦False)
	REM		[5_12 + 6 - 7.4 ^ 3 ^ ~G()] > ~ 10110101_2
	REM		[4 + 5] * 9												(Discovered as bug 24-10-2022) (Fix: Added OperatorExpr as a permitted Type for stage 1 of IOTCollapsing)

	''' <summary>Contains Expression Construction Resources</summary>
	Public Module ExprUtilities

#Region "Expression Constants"

		Private ReadOnly Expr_PermittedTokenTypes_ As Runtime.Token.TokenType() = {Runtime.Token.TokenType.BooleanLiteral, Runtime.Token.TokenType.DSOperator, Runtime.Token.TokenType.GrammarChar, Runtime.Token.TokenType.Identifier, Runtime.Token.TokenType.NumericLiteral, Runtime.Token.TokenType.StringLiteral}
		Private ReadOnly LBL_PermittedIExprTypes_ As Type() = {GetType(LBLArgumentSeperatorPlaceholder), GetType(LBLBracketedExprPlaceholder), GetType(LBLFunctionCallPlaceholder), GetType(LBLOperatorPlaceholder), GetType(LiteralExpr(Of Variables.DSBoolean)), GetType(LiteralExpr(Of Variables.DSNumber)), GetType(LiteralExpr(Of Variables.DSString)), GetType(VariableExpr)}
		Private ReadOnly ExprTree_PermittedIExprTypes_ As Type() = {GetType(FunctionCallExpr), GetType(OperatorExpr), GetType(LiteralExpr(Of Variables.DSBoolean)), GetType(LiteralExpr(Of Variables.DSNumber)), GetType(LiteralExpr(Of Variables.DSString)), GetType(VariableExpr)}
		Private ReadOnly IOTCollapsing_PostRecursiveFuncCallAndBracketedExprCollapse_PermittedIExprTypes_ As Type() = {GetType(LiteralExpr(Of Variables.DSBoolean)), GetType(LiteralExpr(Of Variables.DSNumber)), GetType(LiteralExpr(Of Variables.DSString)), GetType(VariableExpr), GetType(FunctionCallExpr), GetType(LBLOperatorPlaceholder), GetType(OperatorExpr)}

#End Region

		''' <summary>Constructs a Tree for an Expression, from the Tokens which make up that Expr.</summary>
		Public Function ConstructExpressionFromTokens(ByRef _RawTokens As Runtime.Token()) As IExpression
			Try

				LogLexingMessage("Beginning the construction of an Expression from Tokens...")

				REM ╔═════════════════════════════════════════════╗
				REM ║  DocScript Expr. Tree Construction Process  ║
				REM ╚═════════════════════════════════════════════╝

				REM 1) Initial Validation
				'		- Ensure _RawTokens is not Empty
				'		- Reassign _RawTokens to not end in a [LineEnd] Token if it currently does
				'		- Ensure _RawTokens all have a permitted TokenType
				'		- Ensure each opening bracket "(" or "[" has a corrosponding closing bracket...
				'			... (even though we know the brackets for the source *as a whole* are balenced)

				REM 2) LBL Production
				'		- Produce the Top-Level LBL (Linear Bracketed Level)
				'		- Simplify this LBL into an unambigous form. E.g. [[9]] → 9
				'		- Validate this simplified LBL to ensure that the expression is well-formed

				REM 3) IOT Collapsing
				'		- Identify the Indexes and Prescedances, of Operators in LBL
				'		- Order this OperatorsList by the Presecedance and Associativity of the operators
				'		- Starting with the highest-prescedance Operator, collapse the LBL into IOTs (Intermediate Operator Trees)
				'		- Assemble these IOTs into the RootTreeNode, via the SCIs (Scanned Component Indicators) of each LooseOperatorExpr

				REM		[_RawTokens] → [_TopLevelLBL] → [_SimplifiedLBL] → [_ExprTreeRoot]

				Call PerformInitialValidationOnRawTokens_(_RawTokens) 'Throws Exceptions if anything's out-of-order

				Dim _TopLevelLBL As IExpression() = ProduceLBL_(_RawTokens).Item2

				Dim _SimplifiedLBL As IExpression() = SimplifyLBL_(_TopLevelLBL) : LogLexingMessage("The Top-Level LBL was simplified from " & _TopLevelLBL.Length.ToString() & " into " & _SimplifiedLBL.Length.ToString() & " LBL Component(s)")

				REM Ensure that the simplification hasn't made the Expression Empty:
				If (_SimplifiedLBL.Length = 0) Then Throw New DSValidationException("After simplifying the LBL, it was found to be empty", "ConstructExpressionFromTokens()\_SimplifiedLBL")

				'Throws any exceptions if something is found to be invalid
				ValidateLBL_(_SimplifiedLBL) : LogLexingMessage("All LBL Components was found to be Valid and well-formed")
				_SimplifiedLBL.ToList().ForEach(Sub(_LBLComponent As IExpression) LogDebugMessage("Simplified & Validated LBL Component: " & _LBLComponent.GetLBLComponentString(), LogEvent.DSEventCatagory.Lexing))

				Dim _ExprTreeRoot As IExpression = CollapseToIOT_(_SimplifiedLBL)

				LogLexingMessage("...Finished constructing an Expression from Tokens; returning Root IExpression of type " & _ExprTreeRoot.GetType().Name.InSquares())
				Return _ExprTreeRoot

			Catch _Ex As Exception : Throw New DSException("@ConstructExpressionFromTokens: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>Constructs a WinForms TreeView Node from a constructed IExpression</summary>
		Public Function GetTreeViewNodeFromExpr(ByRef _Expr As IExpression) As Global.System.Windows.Forms.TreeNode
			Try

				LogLexingMessage("Constructing a WindowsForms TreeView Node from Expression """ & (Function(_First20ExprChars$) If(_First20ExprChars.Length >= 20, _First20ExprChars & """...", _First20ExprChars & """"c)).Invoke(New String(_Expr.ToString().ToCharArray().Take(20).ToArray())))

				Static _ExprTreeNodeColours_ As New Dictionary(Of Type, Drawing.Color)() From {
				 {GetType(LiteralExpr(Of Variables.DSString)), Drawing.Color.FromArgb(128, 128, 255)},
				 {GetType(LiteralExpr(Of Variables.DSNumber)), Drawing.Color.FromArgb(128, 128, 255)},
				 {GetType(LiteralExpr(Of Variables.DSBoolean)), Drawing.Color.FromArgb(128, 128, 255)},
				 {GetType(VariableExpr), Drawing.Color.FromArgb(255, 192, 128)},
				 {GetType(FunctionCallExpr), Drawing.Color.FromArgb(255, 128, 255)},
				 {GetType(OperatorExpr), Drawing.Color.FromArgb(128, 255, 128)}
				}

				Dim _TreeNodeToReturn As Windows.Forms.TreeNode

				Select Case _Expr.GetType()
					Case GetType(LiteralExpr(Of Variables.DSString)), GetType(LiteralExpr(Of Variables.DSNumber)), GetType(LiteralExpr(Of Variables.DSBoolean))
						_TreeNodeToReturn = New Windows.Forms.TreeNode(_Expr.ToString()) With {.BackColor = _ExprTreeNodeColours_.Item(_Expr.GetType())}
					Case GetType(VariableExpr)
						_TreeNodeToReturn = New Windows.Forms.TreeNode(_Expr.ToString()) With {.BackColor = _ExprTreeNodeColours_.Item(_Expr.GetType())}
					Case GetType(FunctionCallExpr)

						Dim _Expr_AsFunctionCallExpr As FunctionCallExpr = CType(_Expr, FunctionCallExpr)
						_TreeNodeToReturn = New Windows.Forms.TreeNode(_Expr_AsFunctionCallExpr.Identifier & Constants.OpeningFunctionBracket & If(_Expr_AsFunctionCallExpr.SubExpressions.Length = 0, "", "...") & Constants.ClosingFunctionBracket) With {.BackColor = _ExprTreeNodeColours_.Item(_Expr.GetType())}

						For Each _Argument As IExpression In _Expr_AsFunctionCallExpr.SubExpressions
							_TreeNodeToReturn.Nodes.Add(GetTreeViewNodeFromExpr(_Argument))
						Next

					Case GetType(OperatorExpr)

						Dim _Expr_AsOperatorExpr As OperatorExpr = CType(_Expr, OperatorExpr)
						_TreeNodeToReturn = New Windows.Forms.TreeNode(_Expr_AsOperatorExpr.OperatorChar.ToString()) With {.BackColor = _ExprTreeNodeColours_.Item(_Expr.GetType())}

						For Each _Operand As IExpression In _Expr_AsOperatorExpr.SubExpressions
							_TreeNodeToReturn.Nodes.Add(GetTreeViewNodeFromExpr(_Operand))
						Next

					Case Else : Throw New DSValidationException("The _Expr had an unrecognised Type of " & _Expr.GetType().FullName.InBrackets(), "_Expr")
				End Select

				Return _TreeNodeToReturn

			Catch _Ex As Exception : Throw New DSException("@GetTreeViewNodeFromExpr: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>
		''' Constructs a Tree for an Expression, from an XML Expression Serialisation, as produced by IExpression.GetExprTreeNodeXML().
		''' The XElement passed in should have one of these Tag Names: OperatorExpr, LiteralExpr, FunctionCallExpr, VariableExpr.
		''' </summary>
		Public Function ConstructExpressionFromXML(ByVal _ExpressionXML As XElement) As IExpression
			Try

				Select Case _ExpressionXML.Name.ToString().ToUpper()
					Case "LITERALEXPR"
						Select Case _ExpressionXML.@LiteralType
							Case "STRING" : Return New LiteralExpr(Of Variables.DSString)(New Variables.DSString(_ExpressionXML.@LiteralValue.WithFirstCharacterRemoved().WithLastCharacterRemoved()))
							Case "NUMBER" : Return New LiteralExpr(Of Variables.DSNumber)(New Variables.DSNumber(Convert.ToDouble(_ExpressionXML.@LiteralValue)))
							Case "BOOLEAN" : Return New LiteralExpr(Of Variables.DSBoolean)(New Variables.DSBoolean(_ExpressionXML.@LiteralValue.ToUpper() = Boolean.TrueString.ToUpper()))
							Case Else : Throw New DSValidationException("The LiteralExpr's Data-Type was unaccounted-for", _ExpressionXML.@LiteralType)
						End Select
					Case "VARIABLEEXPR" : Return New VariableExpr(_ExpressionXML.@Identifier)
					Case "OPERATOREXPR"
						Return New OperatorExpr(
						 _OperatorChar:=_ExpressionXML.@OperatorChar.ToCharArray().First(),
						 _Operands:=If(_ExpressionXML.Elements().Count() = 1,
						  {ExprUtilities.ConstructExpressionFromXML(_ExpressionXML.Elements().ElementAt(0))},
						  {ExprUtilities.ConstructExpressionFromXML(_ExpressionXML.Elements().ElementAt(0)), ExprUtilities.ConstructExpressionFromXML(_ExpressionXML.Elements().ElementAt(1))}
						 )
						)
					Case "FUNCTIONCALLEXPR"
						Return New FunctionCallExpr(
						 _Identifier:=_ExpressionXML.@Identifier,
						 _Arguments:=_ExpressionXML.Elements().Select(Of IExpression)(AddressOf ExprUtilities.ConstructExpressionFromXML).ToArray()
						)
					Case Else : Throw New DSValidationException("The _ExpressionXML's Tag Name was unaccounted-for", _ExpressionXML.Name.ToString())
				End Select

			Catch _Ex As Exception : Throw New DSException("@ConstructExpressionFromXML: " & _Ex.Message, _Ex) : End Try
		End Function

#Region "Expression Utility Functions"

		''' <summary>Ensures that the Tokens are of permitted TokenTypes, and that there are equal numbers of opening and closing Brackets.</summary>
		Private Sub PerformInitialValidationOnRawTokens_(ByRef _RawTokens As Runtime.Token())
			Try

				LogLexingMessage("Ensuring _RawTokens Array passed to ConstructExpressionFromTokens() is not empty")
				If _RawTokens.Length = 0 Then Throw New DSValidationException("The _RawTokens Array argument passed to ConstructExpressionFromTokens() was empty. An Expression cannot be constructed from no Tokens", "_RawTokens")

				LogLexingMessage(IIf(_RawTokens.Last().Type = Runtime.Token.TokenType.LineEnd, "The ending Token in the _RawTokens is a [LineEnd] and will be removed", "The ending token in the _RawTokens was not a [LineEnd]. It does not need to be removed.").ToString())
				If _RawTokens.Last().Type = Runtime.Token.TokenType.LineEnd Then _RawTokens = _RawTokens.Take(_RawTokens.Length - 1).ToArray() : If _RawTokens.Length = 0 Then Throw New DSValidationException("After removing a trailing [LineEnd] Token, there were no Tokens left in the _RawTokens passed to ConstructExpressionFromTokens()", "_RawTokens")

				LogLexingMessage("Ensuring all RawTokens passed to ConstructExpressionFromTokens() are of permitted TokenTypes")
				If Not _RawTokens.All(Function(_Token As Runtime.Token) Expr_PermittedTokenTypes_.Contains(_Token.Type)) Then Throw New DSValidationException("The Expression could not be constructed because at least one of the Tokens did not have a valid TokenType", "_RawTokens. The Tokens Were: {" & String.Join(", ", _RawTokens.Select(Function(_Token As Runtime.Token) _Token.ToString())) & "}"c)

				'The Well-Balenced Bracket Algorithm should have already dealt with this in the Parser; this is just an additional check
				Dim _NumberOfOpeningFunctionBrackets% = (_RawTokens.Where(Function(_Token As Runtime.Token) _Token.Value = Language.Constants.OpeningFunctionBracket)).Count()
				Dim _NumberOfClosingFunctionBrackets% = (_RawTokens.Where(Function(_Token As Runtime.Token) _Token.Value = Language.Constants.ClosingFunctionBracket)).Count()
				LogLexingMessage(String.Format("Ensuring No. OpeningFunctionBrackets ({0}) is equal to No. ClosingFunctionBrackets ({1}). Result: {2}", _NumberOfOpeningFunctionBrackets.ToString(), _NumberOfClosingFunctionBrackets.ToString(), IIf(_NumberOfOpeningFunctionBrackets = _NumberOfClosingFunctionBrackets, "True (Valid)", "False (Invalid)")))
				If Not (_NumberOfOpeningFunctionBrackets = _NumberOfClosingFunctionBrackets) Then Throw New DSValidationException(String.Format("The No. OpeningFunctionBrackets ({0}) is not equal to No. ClosingFunctionBrackets ({1}) in an Expression. The expression cannot therefore be syntactically-valid", _NumberOfOpeningFunctionBrackets.ToString(), _NumberOfClosingFunctionBrackets.ToString()), "_RawTokens. The Tokens Were: {" & String.Join(", ", _RawTokens.Select(Function(_Token As Runtime.Token) _Token.ToString())) & "}"c)

				'The Well-Balenced Bracket Algorithm should have already dealt with this in the Parser; this is just an additional check
				Dim _NumberOfOpeningExpressionBrackets% = (_RawTokens.Where(Function(_Token As Runtime.Token) _Token.Value = Language.Constants.OpeningExpressionBracket)).Count()
				Dim _NumberOfClosingExpressionBrackets% = (_RawTokens.Where(Function(_Token As Runtime.Token) _Token.Value = Language.Constants.ClosingExpressionBracket)).Count()
				LogLexingMessage(String.Format("Ensuring No. OpeningExpressionBrackets ({0}) is equal to No. ClosingExpressionBrackets ({1}). Result: {2}", _NumberOfOpeningExpressionBrackets.ToString(), _NumberOfClosingExpressionBrackets.ToString(), IIf(_NumberOfOpeningExpressionBrackets = _NumberOfClosingExpressionBrackets, "True (Valid)", "False (Invalid)")))
				If Not (_NumberOfOpeningExpressionBrackets = _NumberOfClosingExpressionBrackets) Then Throw New DSValidationException(String.Format("The No. OpeningExpressionBrackets ({0}) is not equal to No. ClosingExpressionBrackets ({1}) in an Expression. The expression cannot therefore be syntactically-valid", _NumberOfOpeningExpressionBrackets.ToString(), _NumberOfClosingExpressionBrackets.ToString()), "_RawTokens. The Tokens Were: {" & String.Join(", ", _RawTokens.Select(Function(_Token As Runtime.Token) _Token.ToString())) & "}"c)

				LogLexingMessage("Ensuring that there are no AssignmentOperator (" & Constants.AssignmentOperator & ") occourances in the Expression Tokens")
				If _RawTokens.Any(AddressOf Runtime.BuiltInTPVs.AssignmentOperator_TPV.IsSatisfiedBy) Then Throw New DSUnexpectedTokenException("An Assignment Operator was found in the Expression. This operator is an instruction (assignment) and does not produce an expressive value.", _RawTokens.First(AddressOf Runtime.BuiltInTPVs.AssignmentOperator_TPV.IsSatisfiedBy))

				Return

			Catch _Ex As Exception : Throw New DSException("@PerformInitialValidationOnRawTokens_: " & _Ex.Message, _Ex) : End Try
		End Sub

		''' <summary>Returns [the Index of the Last Token read by that call], and [the LBL generated from the Tokens read]</summary>
		Private Function ProduceLBL_(ByRef _Tokens As Runtime.Token(), Optional ByVal _TokensStartAt% = 0) As Tuple(Of Int32, IExpression())
			Try

				LogLexingMessage("Entering ProduceLBL_() call with " & _Tokens.Length.ToString() & " Total Token(s), starting at Token " & _TokensStartAt.ToString())

				REM E.g. Input:		5	+ 9		- [GetAge() ^ 1101_2]	* Take(Age, Name)
				REM E.g. LBL:		Lit	+ Lit	- [BracketedExpr]		* [FunctionCallExpr]

				Dim _LBLToReturn As New List(Of IExpression)()

				'This is needed outside of the ForLoop, so is being declared here
				Dim _TokenIndex%

				'This Call might start from e.g. TokenIndex 12. The call reports which TokkenIndex it got up to before returning, via Item1 of the Tuple
				For _TokenIndex% = _TokensStartAt To (_Tokens.Length - 1) Step +1
					Select Case _Tokens(_TokenIndex).Type
						Case Runtime.Token.TokenType.NumericLiteral : _LBLToReturn.Add(New LiteralExpr(Of Variables.DSNumber)(Variables.DSNumber.FromToken(_Tokens(_TokenIndex)), _Tokens(_TokenIndex)))
						Case Runtime.Token.TokenType.StringLiteral : _LBLToReturn.Add(New LiteralExpr(Of Variables.DSString)(Variables.DSString.FromToken(_Tokens(_TokenIndex)), _Tokens(_TokenIndex)))
						Case Runtime.Token.TokenType.BooleanLiteral : _LBLToReturn.Add(New LiteralExpr(Of Variables.DSBoolean)(Variables.DSBoolean.FromToken(_Tokens(_TokenIndex)), _Tokens(_TokenIndex)))
						Case Runtime.Token.TokenType.DSOperator : _LBLToReturn.Add(New LBLOperatorPlaceholder(_Tokens(_TokenIndex)))
						Case Runtime.Token.TokenType.GrammarChar

							REM Should either be [ or ] or ) or ,

							Select Case _Tokens(_TokenIndex).Value
								Case Constants.OpeningExpressionBracket

									'Produce the LBL for the Expr inside the Brackets

									'Start Parsing from the next Token after the [
									LogLexingMessage("Making recursive BracketedExpr ProduceLBL_() Call, which will start at Index " & (_TokenIndex + 1) & " of " & _Tokens.Length & " Total Token(s)")
									Dim _BracketedExprLBL As Tuple(Of Int32, IExpression()) = ProduceLBL_(_Tokens, _TokenIndex + 1)

									'We have returned to us: [the Index of the Last Token read by that call] and [the LBL generated from the Tokens read]
									'The Last Token read in the call should be a ]. We want to continue reading from the TOKEN THEREAFTER:
									_TokenIndex = (_BracketedExprLBL.Item1 + 1)
									'We need to add all the LBL from the BracketedExpr to this call's LBL:
									_LBLToReturn.Add(New LBLBracketedExprPlaceholder(_BracketedExprLBL.Item2))

									'The next iteration of the FoorLoop should now be looking at the Next Token after the Closing Bracket

								Case Constants.ClosingExpressionBracket

									'If all is well, we can only be here because a Call of this method (beneath the current call on the CallStack) requested that this ...
									'... Call produce the LBL for the contents of a Bracketed Expr.. Therefore, we have just hit the end of that Bracketed Expr., and ...
									'... should now return to the previous StackFrame, reporting [the Index of the Last Token read]
									LogLexingMessage("Returning from recursive BracketedExpr ProduceLBL_() Call, having collected " & _LBLToReturn.Count.ToString() & " LBL Component(s), and having read up to " & (_TokenIndex - 1) & " of " & _Tokens.Length & " Total Token(s)")
									Return (New Tuple(Of Int32, IExpression())(_TokenIndex - 1, _LBLToReturn.ToArray()))

								Case Constants.ClosingFunctionBracket

									'If all is well, we can only be here because a Call of this method (beneath the current call on the CallStack) requested that this ...
									'... Call produce the LBL for the contents of a FuncitonCall Expr.. Therefore, we have just hit the end of that FuncitonCall Expr., and ...
									'... should now return to the previous StackFrame, reporting [the Index of the Last Token read]
									LogLexingMessage("Returning from recursive FunctionCall ProduceLBL_() Call, having collected " & _LBLToReturn.Count.ToString() & " LBL Component(s), and having read up to " & (_TokenIndex - 1) & " of " & _Tokens.Length & " Total Token(s)")
									Return (New Tuple(Of Int32, IExpression())(_TokenIndex - 1, _LBLToReturn.ToArray()))

								Case Constants.FunctionArgumentSeperator

									REM Add a Placeholder for a comma (,)
									_LBLToReturn.Add(New LBLArgumentSeperatorPlaceholder(_Tokens(_TokenIndex)))

								Case Else : Throw New DSException("A GrammarChar Token passed to ProduceLBL_() had an unrecognised value of " & _Tokens(_TokenIndex).Value.InSquares() & ". The Token's Location was " & _Tokens(_TokenIndex).LocationInSource.ToString())
							End Select

						Case Runtime.Token.TokenType.Identifier

							REM Should either be a Variable or FunctionCall, in which case it'll be followed by a (

							'The first half of the AndAlso is to account for eventuallity where we're already on the last Token
							If (_Tokens.ElementAtOrDefault(_TokenIndex + 1) IsNot Nothing) AndAlso (_Tokens(_TokenIndex + 1).Value = Constants.OpeningFunctionBracket) Then

								'It's should be a FunctionCall
								'Produce the LBL for inside the FunctionCall ( and ) brackets

								'Start Parsing from the next Token after the (		- Bear in mind we're currently on the Identifier for the Function, not the (
								LogLexingMessage("Making recursive FunctionCall ProduceLBL_() Call, which will start at Index " & (_TokenIndex + 2) & " of " & _Tokens.Length & " Total Token(s)")
								Dim _FunctionCallExprLBL As Tuple(Of Int32, IExpression()) = ProduceLBL_(_Tokens, _TokenIndex + 2)

								'We have returned to us: [the Index of the Last Token read by that call] and [the LBL generated from the Tokens read]
								'We need to add all the LBL from the BracketedExpr to this call's LBL:
								_LBLToReturn.Add(New LBLFunctionCallPlaceholder(_Tokens(_TokenIndex), _FunctionCallExprLBL.Item2))
								'The Last Token read in the call should be a ). We want to continue reading from the TOKEN THEREAFTER:
								_TokenIndex = (_FunctionCallExprLBL.Item1 + 1)


								'The next iteration of the FoorLoop should now be looking at the Next Token after the Closing Bracket

							Else

								'It should just be a Variable
								_LBLToReturn.Add(New VariableExpr(_Tokens(_TokenIndex).Value, _Tokens(_TokenIndex)))

							End If

						Case Else : Throw New DSException("The LBL could not be produced because a Token was found with an irrelevant Type. It was: " & _Tokens(_TokenIndex).ToString())
					End Select
				Next

				'We should only be here if this is the Base ProduceLBL() call. Ensure we've read all the Tokens
				If Not (_TokenIndex = _Tokens.Length) Then Throw New DSException(String.Format("After reaching the end of the Base ProduceLBL_() call, the _TokenIndex ({0}) was not equal to the total number of origional Input Tokens ({1}). Therefore, not all the Tokens have been read. (Which is a Problem.)", _TokenIndex.ToString(), _Tokens.Length.ToString()))

				LogLexingMessage("Finished Base ProduceLBL_() call. Returning " & _LBLToReturn.Count.ToString() & " LBL Component(s)")
				Return (New Tuple(Of Int32, IExpression())(_TokenIndex, _LBLToReturn.ToArray()))

			Catch _Ex As Exception : Throw New DSException("@ProduceLBL_: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>Removes and one-child BracketedExprs, recursively inside BracketedExprs and FunctionCalls too</summary>
		Private Function SimplifyLBL_(ByVal _LBL As IExpression()) As IExpression()
			Try

				REM		[9]			→	9
				REM		[[[[9]]]]	→	9
				REM		[9 + 7]		→	9 + 7
				REM		[[[R + H]]]	→	R + H
				REM		F([9])		→	F(9)
				REM		F([[[9]]])	→	F(9)
				REM		[F([9])]	→	F(9)
				REM		F([9], 7)	→	F(9, 7)
				REM		["NaN" ^ 6]	→	["Name" ^ 6]

				'E.g.		5 + [9] * GetAge([[[6]]]) / [Age ^ ~[3]]
				'Becomes:	5 + 9 * GetAge(6) / [Age ^ ~3]

				REM It's possible that the whole of the LBL is in []s when it enters this Function. This won't be corrected in the first Loop because the [] could contain multiple Children
				REM	E.g.		[5 + [9] * GetAge([[[6]]]) / [Age ^ ~[3]]]
				REM	Should be:	 5 + [9] * GetAge([[[6]]]) / [Age ^ ~[3]]
				Do While (_LBL.Length = 1) AndAlso (_LBL.First().GetType() = GetType(Expressions.LBLBracketedExprPlaceholder))
					_LBL = CType(_LBL.First(), LBLBracketedExprPlaceholder).Contents
				Loop

				REM For Each Bracketed Level:
				REM		Replace any [LBLBracketedExprPlaceholder]s where there is only one LBLComp. inside with the contents thereof, until there are no [] with only one child
				REM		If instead there are multiple LBLComponents inside either [] or F(), then recursively call this function with the contents of those Brackets (excluding the Brackets themselves)...
				REM			...And replace those LBLComponents with the ones returned by the call, still inside either the [] or F() when added back in

				REM This first loop Simplifies the current Linear Bracketed Level

				'As long as Any() of the LBLComponents are [LBLBracketedExprPlaceholder]s with just one LBLComp. inside...
				While _LBL.Any(Function(_LBLComponent As IExpression) (_LBLComponent.GetType() = GetType(Expressions.LBLBracketedExprPlaceholder)) AndAlso (CType(_LBLComponent, LBLBracketedExprPlaceholder).Contents.Length = 1))
					For _LBLComponentIndex% = 0 To (_LBL.Length - 1) Step +1

						'If the current LBLComp. is an [LBLBracketedExprPlaceholder]s with just one LBLComp. inside, then replace it with its contents...
						If ((_LBL.ElementAt(_LBLComponentIndex).GetType() = GetType(Expressions.LBLBracketedExprPlaceholder)) AndAlso (CType(_LBL.ElementAt(_LBLComponentIndex), LBLBracketedExprPlaceholder).Contents.Length = 1)) Then

							LogLexingMessage("Detected a redundant one-child BracketedExpr in Top-Level LBL; replacing it with its contents")
							_LBL(_LBLComponentIndex) = CType(_LBL.ElementAt(_LBLComponentIndex), LBLBracketedExprPlaceholder).Contents.ElementAt(0)

						Else 'No need to alter the Component in the _LBL Array
						End If

					Next
				End While

				'The Example now looks like:	5 + 9 * GetAge([[[6]]]) / [Age ^ ~[3]]

				REM This second Loop recursively calls, to simplify and Child Linear Bracketed Levels

				For _LBLComponentIndex% = 0 To (_LBL.Length - 1) Step +1

					'If the current LBLComp. is a [] with multiple children, then replace its contents with the SimplifyLBL_(thereof)...
					If ((_LBL.ElementAt(_LBLComponentIndex).GetType() = GetType(Expressions.LBLBracketedExprPlaceholder)) AndAlso (CType(_LBL.ElementAt(_LBLComponentIndex), LBLBracketedExprPlaceholder).Contents.Length > 1)) Then

						LogLexingMessage("Making recursive SimplifyLBL_() call for BracketedExpr Contents")
						_LBL(_LBLComponentIndex) = (New LBLBracketedExprPlaceholder(SimplifyLBL_(CType(_LBL.ElementAt(_LBLComponentIndex), LBLBracketedExprPlaceholder).Contents)))

						'Alternatively, If the current LBLComp. is a F() with multiple children, then replace its contents with the SimplifyLBL_(thereof)...
					ElseIf (_LBL.ElementAt(_LBLComponentIndex).GetType() = GetType(Expressions.LBLFunctionCallPlaceholder)) Then

						LogLexingMessage("Making recursive SimplifyLBL_() call for FunctionCall Contents")
						_LBL(_LBLComponentIndex) = (New LBLFunctionCallPlaceholder(CType(_LBL.ElementAt(_LBLComponentIndex), LBLFunctionCallPlaceholder).SourceIdentifierToken, SimplifyLBL_(CType(_LBL.ElementAt(_LBLComponentIndex), LBLFunctionCallPlaceholder).Contents)))

					Else 'No need to alter the Component in the _LBL Array
					End If

				Next

				Return _LBL

			Catch _Ex As Exception : Throw New DSException("@SimplifyLBL_: " & _Ex.Message, _Ex) : End Try
		End Function

		'''<summary>Ensures there are no occourances of Illegal patterns in the LBL</summary>
		Private Sub ValidateLBL_(ByVal _LBL As IExpression())
			Try

				REM Ensure that some LBL has been passed in:
				'If (_LBL.Length = 0) Then Throw New DSValidationException("The _LBL passed to ValidateLBL_() was found to be empty", "ValidateLBL_()\_LBL")
				'We can't do this ^^^ here because a recursive call could be made which contains no contents.
				'However, if there are no items in the LBL, then the subsequent logic won't work, so we should just return
				If _LBL.Length = 0 Then
					LogDebugMessage("The LBL passed to ValidateLBL_() was empty. Returning straight awas as there is nothing to validate", LogEvent.DSEventCatagory.Lexing)
					Return
				End If

				REM The valid TokenTypes for an Expr are:
				REM		{BooleanLiteral, DSOperator, GrammarChar, Identifier, NumericLiteral, StringLiteral}
				REM	However, the only IExpression Types we should see inside a LBL are:
				REM		{}

				REM Ensure we have only these IEXPRESSION TYPES:
				If Not _LBL.All(Function(_LBLComp As IExpression) LBL_PermittedIExprTypes_.Contains(_LBLComp.GetType())) Then Throw New DSValidationException("At least one of the LBL Components passed to ValidateLBL_() was not of a permitted LBL Component Type", "The LBL Component Types were: {" & String.Join(", ", _LBL.Select(Of String)(Function(_LBLComp As IExpression) _LBLComp.GetType().Name)) & "}")

				REM Certian Expressions are not valid:
				REM		6 * * 6			(Two consecutive [BinaryOperator]s)	(This is valid though: ~~~~~3)
				REM		6 ~ ^ 6			(This is valid though: 6 ^ ~ 6)
				REM		F(23,,"W")
				REM		[]
				REM		F() F()
				REM		[A] [B]
				REM		F(H) []
				REM		[] F(H)
				REM		Ident Ident
				REM		True True
				REM		732_2 33.9990
				REM		"Lit" "Lit"
				REM		[] True
				REM		F() True
				REM		[] Ident
				REM		F() Ident
				REM		[] 7
				REM		Etc...

				REM Therefore, certian patterns are not valid in an Expression:

				REM Invalid Patterns:
				REM		{Comma} {Comma}
				REM		{Empty-BracketedExpr}
				REM		{BinaryOperator} {BinaryOperator}
				REM		{UnaryOperator} {BinaryOperator}
				REM		{*Literal | Variable} {UnaryOperator}
				REM		{*Literal | Variable} {*Literal | Variable}
				REM		{*Literal | Variable} {FunctionCall | BracketedExpr}
				REM		{FunctionCall | BracketedExpr} {*Literal | Variable}
				REM		{FunctionCall | BracketedExpr} {FunctionCall | BracketedExpr}

				Dim _IsLiteralOrVariable As Func(Of IExpression, Boolean) = _
				 Function(_LBLComponent As IExpression) As Boolean
					 Return {GetType(VariableExpr), GetType(LiteralExpr(Of Variables.DSBoolean)), GetType(LiteralExpr(Of Variables.DSNumber)), GetType(LiteralExpr(Of Variables.DSString))}.Contains(_LBLComponent.GetType())
				 End Function

				Dim _IsFunctionCallOrBracketedExpr As Func(Of IExpression, Boolean) = _
				 Function(_LBLComponent As IExpression) As Boolean
					 Return {GetType(LBLFunctionCallPlaceholder), GetType(LBLBracketedExprPlaceholder)}.Contains(_LBLComponent.GetType())
				 End Function

				'These return True if the Pattern matches (so True if the LBL is invalid)
				Dim _InvalidPatternDetectors As New Dictionary(Of String, Func(Of IExpression, IExpression, Boolean))() From {
				 {"[An Argument Seperator (" & Constants.FunctionArgumentSeperator & ")] followed by [another Argument Seperator]", Function(_A As IExpression, _B As IExpression) ((_A.GetType() = GetType(LBLArgumentSeperatorPlaceholder)) AndAlso (_B.GetType() = GetType(LBLArgumentSeperatorPlaceholder)))},
				 {"An Empty Bracketed Expr.", Function(_A As IExpression, _B As IExpression) {_A, _B}.Any(Function(_LBLComp As IExpression) (_LBLComp.GetType() = GetType(LBLBracketedExprPlaceholder)) AndAlso (CType(_LBLComp, LBLBracketedExprPlaceholder).Contents.Length = 0))},
				 {"Two Binary Operators next to each other", Function(_A As IExpression, _B As IExpression) ((_A.GetType() = GetType(LBLOperatorPlaceholder)) AndAlso Operators.OperatorUtilities.IsBinaryOperator(CType(_A, LBLOperatorPlaceholder).OperatorToken.Value.First())) AndAlso ((_B.GetType() = GetType(LBLOperatorPlaceholder)) AndAlso Operators.OperatorUtilities.IsBinaryOperator(CType(_B, LBLOperatorPlaceholder).OperatorToken.Value.First()))},
				 {"[A Unary Operator] followed by [a Binary Operator]", Function(_A As IExpression, _B As IExpression) ((_A.GetType() = GetType(LBLOperatorPlaceholder)) AndAlso Operators.OperatorUtilities.IsUnaryOperator(CType(_A, LBLOperatorPlaceholder).OperatorToken.Value.First())) AndAlso ((_B.GetType() = GetType(LBLOperatorPlaceholder)) AndAlso Operators.OperatorUtilities.IsBinaryOperator(CType(_B, LBLOperatorPlaceholder).OperatorToken.Value.First()))},
				 {"[A Literal or Variable] followed by [another Literal or Variable]", Function(_A As IExpression, _B As IExpression) (_IsLiteralOrVariable.Invoke(_A) AndAlso _IsLiteralOrVariable.Invoke(_B))},
				 {"[A Literal or Variable] followed by [a Function Call or Bracketed Expr.]", Function(_A As IExpression, _B As IExpression) (_IsLiteralOrVariable.Invoke(_A) AndAlso _IsFunctionCallOrBracketedExpr.Invoke(_B))},
				 {"[A Literal or Variable] followed by [a Unary Operator]", Function(_A As IExpression, _B As IExpression) (_IsLiteralOrVariable.Invoke(_A) AndAlso ((_B.GetType() = GetType(LBLOperatorPlaceholder)) AndAlso Operators.OperatorUtilities.IsUnaryOperator(CType(_B, LBLOperatorPlaceholder).OperatorToken.Value.First())))},
				 {"[A Function Call or Bracketed Expr.] followed by [a Literal or Variable]", Function(_A As IExpression, _B As IExpression) (_IsLiteralOrVariable.Invoke(_B) AndAlso _IsFunctionCallOrBracketedExpr.Invoke(_A))},
				 {"[A Function Call or Bracketed Expr.] followed by [a Function Call or Bracketed Expr.]", Function(_A As IExpression, _B As IExpression) (_IsFunctionCallOrBracketedExpr.Invoke(_A) AndAlso _IsFunctionCallOrBracketedExpr.Invoke(_B))}
				}

				LogLexingMessage("Checking for Invalid Patterns in " & _LBL.Length.ToString() & " LBL Component(s)")

				REM Because the following ForLoop will only run if the Expr contians at least 2 LBLComps, we need to check if there is only one LBLComp., and do some edge-case validation thereon
				If _LBL.Length = 1 Then

					REM In this eventuality where there is only one LBL Component in the _LBL, there are only these things to check for:
					REM		1) That the LBLComp. isn't an Operator
					REM		2) That the LBLComp. isn't an Empty BracketedExpr
					REM		3) That the LBLComp. isn't an ArgumentSeperator (,)

					If _LBL.First().GetType() = GetType(LBLOperatorPlaceholder) Then Throw New DSLBLValidationException("A length-one LBL Component was Invalid", "An Operator on its own", {_LBL.First()})
					If (_LBL.First().GetType() = GetType(LBLBracketedExprPlaceholder)) AndAlso (CType(_LBL.First(), LBLBracketedExprPlaceholder).Contents.Length = 0) Then Throw New DSLBLValidationException("A length-one LBL Component was Invalid", "An empty Bracketed Expr.", {_LBL.First()})
					If (_LBL.First().GetType() = GetType(LBLArgumentSeperatorPlaceholder)) Then Throw New DSLBLValidationException("A length-one LBL Component was Invalid", "An Argument Seperator (" & Constants.FunctionArgumentSeperator & ") on its own.", {_LBL.First()})

				End If

				REM Put the current _LBL through the InvalidPatternDetectors
				For _LBLComponentIndex% = 0 To (_LBL.Length - 2) Step (+1)
					For Each _InvalidPatternDetector As KeyValuePair(Of String, Func(Of IExpression, IExpression, Boolean)) In _InvalidPatternDetectors
						If _InvalidPatternDetector.Value.Invoke(_LBL(_LBLComponentIndex), _LBL(_LBLComponentIndex + 1)) Then _
						 Throw New DSLBLValidationException("The LBL contained an Invalid Pattern", _InvalidPatternDetector.Key, {_LBL(_LBLComponentIndex), _LBL(_LBLComponentIndex + 1)})
					Next
				Next

				REM Also ensure that the _LBL dosen't begin or end with a Binary Operator, and dosen't end with a Unary Operator...
				If (_LBL.First().GetType() = GetType(LBLOperatorPlaceholder)) AndAlso Operators.OperatorUtilities.IsBinaryOperator(CType(_LBL.First(), LBLOperatorPlaceholder).OperatorToken.Value.First()) Then Throw New DSLBLValidationException("An LBL Component was Invalid in its location", "A Binary Operator at the start of the LBL", {_LBL.First()})
				If (_LBL.Last().GetType() = GetType(LBLOperatorPlaceholder)) AndAlso Operators.OperatorUtilities.IsBinaryOperator(CType(_LBL.Last(), LBLOperatorPlaceholder).OperatorToken.Value.First()) Then Throw New DSLBLValidationException("An LBL Component was Invalid in its location", "A Binary Operator at the end of the LBL", {_LBL.Last()})
				If (_LBL.Last().GetType() = GetType(LBLOperatorPlaceholder)) AndAlso Operators.OperatorUtilities.IsUnaryOperator(CType(_LBL.Last(), LBLOperatorPlaceholder).OperatorToken.Value.First()) Then Throw New DSLBLValidationException("An LBL Component was Invalid in its location", "A Unary Operator at the end of the LBL", {_LBL.Last()})

				REM ...And that it dosen't begin or end with an Argument Seperator (,)
				If (_LBL.First().GetType() = GetType(LBLArgumentSeperatorPlaceholder)) Then Throw New DSLBLValidationException("An LBL Component was Invalid in its location", "An Argument Seperator at the start of the LBL", {_LBL.Last()})
				If (_LBL.Last().GetType() = GetType(LBLArgumentSeperatorPlaceholder)) Then Throw New DSLBLValidationException("An LBL Component was Invalid in its location", "An Argument Seperator at the end of the LBL", {_LBL.Last()})

				REM Now that the current Linear Bracketed Level has been validated, validate the contents of any BracketedExprs or FunctionCalls within it:
				For Each _LBLComponent As IExpression In _LBL '.Where(_IsFunctionCallOrBracketedExpr)
					If (_LBLComponent.GetType() = GetType(LBLBracketedExprPlaceholder)) Then
						ValidateLBL_(CType(_LBLComponent, LBLBracketedExprPlaceholder).Contents)
					ElseIf (_LBLComponent.GetType() = GetType(LBLFunctionCallPlaceholder)) Then
						ValidateLBL_(CType(_LBLComponent, LBLFunctionCallPlaceholder).Contents)
					End If
				Next

			Catch _Ex As Exception : Throw New DSException("@ValidateLBL_: " & _Ex.Message, _Ex) : End Try
		End Sub

		'''<summary>Used by the CollapseToIOT_() Method to order the OperatorTuples</summary>
		Private Class OperatorTupleComparer : Implements Global.System.Collections.Generic.IComparer(Of Tuple(Of Int32, Byte))

			Public ReadOnly ReferanceCopyOf_ValidatedLBL As IExpression() 'Used to get the OperatorChar of the Operators to make sure Unary Operators go first. Passed hither in the Constructor

			Public Sub New(ByVal _ValidatedLBL_RefCopy As IExpression())
				Me.ReferanceCopyOf_ValidatedLBL = _ValidatedLBL_RefCopy
			End Sub

			Public Function Compare(ByVal _X As System.Tuple(Of Integer, Byte), ByVal _Y As System.Tuple(Of Integer, Byte)) As Int32 Implements System.Collections.Generic.IComparer(Of System.Tuple(Of Integer, Byte)).Compare
				Try

					'Return Value:			(From MS .NET Docs)
					'Int32: A signed integer that indicates the relative values of x and y:
					'	- If less than 0, x is less than y.
					'	- If 0, x equals y.
					'	- If greater than 0, x is greater than y.

					REM Firstly order by the Prescedance
					REM Then by Index

					'Item1 = Index; Item2 = Prescedance
					'Highest Prescedance must be first in the List
					'Lowest Index must be first in the List (when Prescedance is the same)

					REM HOWEVER:
					REM		All Unary Operators have an equally-high prescedance, which is higher than the Prescedance of any of the Binary Operators.
					REM		The Unary Operators must always go first regardless...
					REM		...And they must be in order of Highest to Lowest LBL Index.

					Dim _ReturnValue As Int32

					Dim _X_OperatorPlaceholder As LBLOperatorPlaceholder = CType(Me.ReferanceCopyOf_ValidatedLBL.ElementAt(_X.Item1), LBLOperatorPlaceholder)
					Dim _Y_OperatorPlaceholder As LBLOperatorPlaceholder = CType(Me.ReferanceCopyOf_ValidatedLBL.ElementAt(_Y.Item1), LBLOperatorPlaceholder)

					REM If _X and _Y are both Unary Operators, then the one with the highest LBL Index must go first
					If Operators.IsUnaryOperator(_X_OperatorPlaceholder.OperatorToken.Value.First()) AndAlso Operators.IsUnaryOperator(_Y_OperatorPlaceholder.OperatorToken.Value.First()) Then
						_ReturnValue = If(_X.Item1 > _Y.Item1, -1, If(_X.Item1 = _Y.Item1, 0, +1))

						REM If just _X is a Unary Operator, then it must go first
					ElseIf Operators.IsUnaryOperator(_X_OperatorPlaceholder.OperatorToken.Value.First()) Then
						_ReturnValue = -1

						REM If just _Y is a Unary Operator, then it must go first
					ElseIf Operators.IsUnaryOperator(_Y_OperatorPlaceholder.OperatorToken.Value.First()) Then
						_ReturnValue = +1

						REM Otherwise, neither _X or _Y are Unary Operators; Return based on Highest Prescedance and Left-to-Right Associativity
					Else

						If _X.Item2 < _Y.Item2 Then : _ReturnValue = +1
						ElseIf _X.Item2 = _Y.Item2 Then : _ReturnValue = If(_X.Item1 < _Y.Item1, -1, If(_X.Item1 = _Y.Item1, 0, +1))
						ElseIf _X.Item2 > _Y.Item2 Then : _ReturnValue = -1
						Else : Throw New DSException("_X was not LessThan, EqualTo, or GreaterThan _Y. X was " & _X.ToString() & ". Y was " & _Y.ToString())
						End If

					End If

					'MsgBox("Returning " & _ReturnValue.ToString() & vbCrLf & "_X: " & _X.ToString() & vbCrLf & "_Y:" & _Y.ToString(), , "OperatorTuple Comparer")
					Return _ReturnValue

				Catch _Ex As Exception : Throw New DSException("@OperatorTupleComparer\Compare: " & _Ex.Message, _Ex) : End Try
			End Function

		End Class

		'''<summary>Removes LBL-Only IExprs and forms the Expr. Tree and adds in the [OperatorExpr]s and [FunctionCallExpr]s</summary>
		Private Function CollapseToIOT_(ByVal _ValidatedLBL As IExpression()) As IExpression
			Try
				LogLexingMessage("Began Collapsing LBL into Intermediate Operator Tree (IOT)")

				REM Input could just look like: "0123456789"

				REM	E.g. Input:		Lit + Lit	 - LBLBracketedExpr * LBLFunctionCall
				REM	E.g. Output-0:	OperatorExpr - LBLBracketedExpr * LBLFunctionCall
				REM	E.g. Output-1:	OperatorExpr					* LBLFunctionCall
				REM	E.g. Output-2:	OperatorExpr

				REM 1) Replace any LBLBracketedExprs or LBLFunctionCallPlaceholders with the proper Expr. Types herefor
				For _LBLComponentIndex% = 0 To (_ValidatedLBL.Length - 1) Step +1
					If _ValidatedLBL(_LBLComponentIndex).GetType() = GetType(LBLBracketedExprPlaceholder) Then

						REM Replace \w an Expr (Tree)
						_ValidatedLBL(_LBLComponentIndex) = CollapseToIOT_(CType(_ValidatedLBL(_LBLComponentIndex), LBLBracketedExprPlaceholder).Contents)

					ElseIf _ValidatedLBL(_LBLComponentIndex).GetType() = GetType(LBLFunctionCallPlaceholder) Then

						REM Replace \w a FunctionCallExpr
						Dim _ArgumentLBLs As IExpression()() = CType(_ValidatedLBL(_LBLComponentIndex), LBLFunctionCallPlaceholder).Contents.SplitWhere(Function(_LBLComp As IExpression) _LBLComp.GetType() = GetType(LBLArgumentSeperatorPlaceholder)).Where(Function(_ArgumentLBL As IExpression()) _ArgumentLBL.Length > 0).ToArray()
						Dim _IOTCollapsedArguments As IExpression() = (From _ArgumentLBL As IExpression() In _ArgumentLBLs Select CollapseToIOT_(_ArgumentLBL)).ToArray()

						'We could be passing in a zero-length array in here for the Arguments (which is fine)
						_ValidatedLBL(_LBLComponentIndex) = New FunctionCallExpr(CType(_ValidatedLBL(_LBLComponentIndex), LBLFunctionCallPlaceholder).SourceIdentifierToken.Value, _IOTCollapsedArguments)

					End If
				Next

				REM At this point, the _ValidatedLBL should only contain these IExpression Types:
				REM		{LiteralExpr, VariableExpr, FunctionCallExpr, LBLOperatorPlaceholder}
				REM		CHANGE MADE 24-10-2022: An OperatorExpr is also allowed here (?)
				For Each _LBLComp As IExpression In _ValidatedLBL
					If Not IOTCollapsing_PostRecursiveFuncCallAndBracketedExprCollapse_PermittedIExprTypes_.Contains(_LBLComp.GetType()) Then Throw New DSValidationException("After the recursive FunctionCall and BracketedExpr Collapsing, there was an LBL Component remaining which didn't have a permitted Type for this stage", "Its Type was: " & _LBLComp.GetType().Name.InSquares())
				Next

				REM We should not have a zero-length _ValidatedLBL at this point
				If _ValidatedLBL.Length = 0 Then Throw New DSValidationException("The length of the LBL passed into CollapseToIOT_() was 0", "CollapseToIOT_()\_ValidatedLBL")

				REM If there aren't any LBLOperatorPlaceholders, then there should only be one IExpr left, and it's okay to return this
				If _ValidatedLBL.Where(Function(_LBLComp As IExpression) _LBLComp.GetType() = GetType(LBLOperatorPlaceholder)).Count = 0 Then
					If Not _ValidatedLBL.Length = 1 Then
						Throw New DSLBLValidationException("Despite there being no LBLOperatorPlaceholders, the _ValidatedLBL was greater than 1 in Length", "(Not Applicable)", _ValidatedLBL)
					Else
						Return _ValidatedLBL.First()
					End If
				End If

				'If the No. LBLOperatorPlaceholders == the Length of the LBL, then we have only been passed in Operators, which is very wrong:
				If _ValidatedLBL.Where(Function(_LBLComp As IExpression) _LBLComp.GetType() = GetType(LBLOperatorPlaceholder)).Count = _ValidatedLBL.Length Then Throw New DSLBLValidationException("The No. LBLOperatorPlaceholders == the Length of the LBL. This means that all the LBL Components must have been operators, which is invalid", "(Not Applicable)", _ValidatedLBL)

				REM 2) Here, we know that there are MoreThan 1 LBLComponents, so there is at least one which is an Operator (and at least one which isn't) (E.g. [~1] or [¬True])
				REM		...Therefore, the next thing to do is to collapse the LBL down from any LBLOperatorPlaceholders to OperatorExprs
				REM To do this:
				REM		- Determine the Indexes of OperatorPlaceholders in the LBL, and the Prescedances which corrospond to the operators being represented. Put these data into the OperatorTuples.
				REM		- Order these OperatorTuples by Prescedance, and then Associativity (left to right)

				'The UInt16 is the Operator's Index in the LBL; The Byte is the Operators Prescedance
				'Item1 = Index; Item2 = Prescedance
				Dim _Unsorted_OperatorsInLBL As New List(Of Tuple(Of Int32, Byte))()

				For _LBLComponentIndex% = 0 To (_ValidatedLBL.Length - 1) Step +1
					If _ValidatedLBL(_LBLComponentIndex).GetType() = GetType(LBLOperatorPlaceholder) Then _
					 _Unsorted_OperatorsInLBL.Add(New Tuple(Of Int32, Byte)(_LBLComponentIndex, Operators.DSOperators.Item(CType(_ValidatedLBL(_LBLComponentIndex), LBLOperatorPlaceholder).OperatorToken.Value.First()).Precedence))
				Next
				LogLexingMessage("Collected " & _Unsorted_OperatorsInLBL.Count.ToString() & " OperatorTuple(s) from LBL; Ordering by Prescedance and Associativity...")

				'Sort the list by Prescedance and Associativity (Lowest Index first where prescedance is the same)
				Dim _Sorted_OperatorsInLBL As List(Of Tuple(Of Int32, Byte)) = _Unsorted_OperatorsInLBL.OrderBy(Of Tuple(Of Int32, Byte))(keySelector:=Function(_OperatorTuple As Tuple(Of Int32, Byte)) _OperatorTuple, comparer:=New OperatorTupleComparer(_ValidatedLBL)).ToList()
				LogDebugMessage("Finished Ordering OperatorTuple(s):" & String.Join(", ", _Sorted_OperatorsInLBL.Select(Of String)(Function(_OperatorTuple As Tuple(Of Int32, Byte)) (CType(_ValidatedLBL(_OperatorTuple.Item1), LBLOperatorPlaceholder).OperatorToken.Value & ", " & _OperatorTuple.ToString()).InSquares())), LogEvent.DSEventCatagory.Lexing)

				REM 3) Now the LBL Components Array will be gradually filled with Scanned Component Indicators (SCIs) as the ExprTree is constructed

				Dim _LooseOperatorExprs As New List(Of OperatorExpr)() 'Collects the OperatorExprs before they are assembled into the Tree

				For _OperatorTupleIndex% = 0 To (_Sorted_OperatorsInLBL.Count - 1) Step +1 'Incedentially, the _OperatorTupleIndex% will always be equal to the next avaliable Index in _LooseOperatorExprs

					Dim _CurrentLBLOptrPlcHldr_IndexInLBL% = _Sorted_OperatorsInLBL(_OperatorTupleIndex).Item1
					Dim _CurrentLBLOptrPlcHldr As LBLOperatorPlaceholder = CType(_ValidatedLBL(_CurrentLBLOptrPlcHldr_IndexInLBL%), LBLOperatorPlaceholder)

					REM Add a Loose OperatorExpr for the OperatorTuple

					If Operators.IsBinaryOperator(_CurrentLBLOptrPlcHldr.OperatorToken.Value.First()) Then

						_LooseOperatorExprs.Add(New OperatorExpr(_CurrentLBLOptrPlcHldr.OperatorToken.Value.First(), {_ValidatedLBL(_CurrentLBLOptrPlcHldr_IndexInLBL - 1), _ValidatedLBL(_CurrentLBLOptrPlcHldr_IndexInLBL% + 1)}))

						REM Now replace the OperatorPlaceholder and Operands that we just used, with [SCIs whose OpEx_Number is _OperatorTupleIndex%]
						REM If either of the Operands we're replacing are themselves SCI's however, then all SCIs with that OpEx_Number need to be replaced with [SCIs whose OpEx_Number is _OperatorTupleIndex%]

						Dim _SCI_ForThisOperatorExpr As New ScannedComponentIndicator(CUShort(_OperatorTupleIndex))

						REM Firstly: the OperatorPlaceholder itself...
						_ValidatedLBL(_CurrentLBLOptrPlcHldr_IndexInLBL) = _SCI_ForThisOperatorExpr

						REM ...Now: If the 1st Operand was an SCI...
						If _ValidatedLBL(_CurrentLBLOptrPlcHldr_IndexInLBL - 1).GetType() = GetType(ScannedComponentIndicator) Then
							REM ... Then replace all occourances of that Operand SCI with the SCI for the OperatorExpr we've just constructed...
							Dim _SCIToReplace_Number As UInt16 = CType(_ValidatedLBL(_CurrentLBLOptrPlcHldr_IndexInLBL - 1), ScannedComponentIndicator).LooseOpExpr_Number
							_ValidatedLBL = _ValidatedLBL.ReplaceWhere(Function(_LBLComp As IExpression) ((_LBLComp.GetType() = GetType(ScannedComponentIndicator)) AndAlso (CType(_LBLComp, ScannedComponentIndicator).LooseOpExpr_Number = _SCIToReplace_Number)), _SCI_ForThisOperatorExpr)
						Else
							REM ...Otherwise, just replace the 1st Operand with the SCI for the OperatorExpr we've just constructed
							_ValidatedLBL(_CurrentLBLOptrPlcHldr_IndexInLBL - 1) = _SCI_ForThisOperatorExpr
						End If

						REM If the 2nd Operand was an SCI...
						If _ValidatedLBL(_CurrentLBLOptrPlcHldr_IndexInLBL + 1).GetType() = GetType(ScannedComponentIndicator) Then
							REM ... Then replace all occourances of that Operand SCI with the SCI for the OperatorExpr we've just constructed...
							Dim _SCIToReplace_Number As UInt16 = CType(_ValidatedLBL(_CurrentLBLOptrPlcHldr_IndexInLBL + 1), ScannedComponentIndicator).LooseOpExpr_Number
							_ValidatedLBL = _ValidatedLBL.ReplaceWhere(Function(_LBLComp As IExpression) ((_LBLComp.GetType() = GetType(ScannedComponentIndicator)) AndAlso (CType(_LBLComp, ScannedComponentIndicator).LooseOpExpr_Number = _SCIToReplace_Number)), _SCI_ForThisOperatorExpr)
						Else
							REM ...Otherwise, just replace the 2nd Operand with the SCI for the OperatorExpr we've just constructed
							_ValidatedLBL(_CurrentLBLOptrPlcHldr_IndexInLBL + 1) = _SCI_ForThisOperatorExpr
						End If

					ElseIf Operators.IsUnaryOperator(_CurrentLBLOptrPlcHldr.OperatorToken.Value.First()) Then

						_LooseOperatorExprs.Add(New OperatorExpr(_CurrentLBLOptrPlcHldr.OperatorToken.Value.First(), {_ValidatedLBL(_CurrentLBLOptrPlcHldr_IndexInLBL% + 1)}))

						REM Now replace the OperatorPlaceholder and Operand that we just used, with [SCIs whose OpEx_Number is _OperatorTupleIndex%]
						REM If the Operand we're replacing is itself an SCI however, then all SCIs with that OpEx_Number need to be replaced with [SCIs whose OpEx_Number is _OperatorTupleIndex%]

						Dim _SCI_ForThisOperatorExpr As New ScannedComponentIndicator(CUShort(_OperatorTupleIndex))

						REM Firstly: the OperatorPlaceholder itself...
						_ValidatedLBL(_CurrentLBLOptrPlcHldr_IndexInLBL) = _SCI_ForThisOperatorExpr

						REM ...Now: If the Operand was an SCI...
						If _ValidatedLBL(_CurrentLBLOptrPlcHldr_IndexInLBL + 1).GetType() = GetType(ScannedComponentIndicator) Then
							REM ... Then replace all occourances of that Operand SCI with the SCI for the OperatorExpr we've just constructed...
							Dim _SCIToReplace_Number As UInt16 = CType(_ValidatedLBL(_CurrentLBLOptrPlcHldr_IndexInLBL + 1), ScannedComponentIndicator).LooseOpExpr_Number
							_ValidatedLBL = _ValidatedLBL.ReplaceWhere(Function(_LBLComp As IExpression) ((_LBLComp.GetType() = GetType(ScannedComponentIndicator)) AndAlso (CType(_LBLComp, ScannedComponentIndicator).LooseOpExpr_Number = _SCIToReplace_Number)), _SCI_ForThisOperatorExpr)
						Else
							REM ...Otherwise, just replace the 1st Operand with the SCI for the OperatorExpr we've just constructed
							_ValidatedLBL(_CurrentLBLOptrPlcHldr_IndexInLBL + 1) = _SCI_ForThisOperatorExpr
						End If

					Else : Throw New DSValidationException("An Operator within the _ValidatedLBL passed to CollapseToIOT_() was not recognised as a valid Expression Binary or Unary Operator", _CurrentLBLOptrPlcHldr.OperatorToken.Value & "(TokenLocation: " & _CurrentLBLOptrPlcHldr.OperatorToken.LocationInSource.ToString() & ")"c)
					End If

					REM At this point, the Current OperatorPlaceholer and it's Operand(s) have been replaced with SCIs which point to [the constructed OperandExpr in the _LooseOperatorExprs]

				Next

				LogLexingMessage("Finished populating _LooseOperatorExprs...")

				'Takes in an OpEx whose Operands could be SCIs, and returns the OpEx with those SCIs resolved
				Dim _GetPureOperatorExpr As Func(Of OperatorExpr, OperatorExpr) = _
				 Function(_OperatorExpr As OperatorExpr) As OperatorExpr

					 Dim _OperatorExprToReturn_Operands As New List(Of IExpression)()

					 For Each _Operand As IExpression In _OperatorExpr.SubExpressions
						 If _Operand.GetType() = GetType(ScannedComponentIndicator) Then
							 REM Resolve the SCI...
							 _OperatorExprToReturn_Operands.Add(_GetPureOperatorExpr.Invoke(_LooseOperatorExprs(CType(_Operand, ScannedComponentIndicator).LooseOpExpr_Number)))
						 Else
							 REM Just add the Operand as it is; it's already "resolved"...
							 _OperatorExprToReturn_Operands.Add(_Operand)
						 End If
					 Next

					 Return New OperatorExpr(_OperatorExpr.OperatorChar, _OperatorExprToReturn_Operands.ToArray())

				 End Function

				REM The Last item in the _LooseOperatorExprs is the root of the Tree
				Dim _TreeRoot As OperatorExpr = _GetPureOperatorExpr.Invoke(_LooseOperatorExprs.Last())

				'If for some reason the _TreeRoot is nothing, then something has gone wrong
				If (_TreeRoot Is Nothing) Then Throw New DSValidationException("The _TreeRoot was Nothing at the end of CollapseToIOT_() when it should have had a value", "_TreeRoot")

				LogLexingMessage("...Returning Root OperatorExpr from CollapseToIOT_() call")
				Return _TreeRoot

			Catch _Ex As Exception : Throw New DSException("@CollapseToIOT_: " & _Ex.Message, _Ex) : End Try
		End Function

#End Region

	End Module

End Namespace