Public Module DSResolveExpr

	REM DSPipelining\DSResolveExpr
	REM Expected StdIn:		<*Expr />
	REM Provided StdOut:	{The Resolved Expression, Coerced into a String}

	Public Function Main%(ByVal _CLAs$())
		Try

			Dim _ExprXMLString$ = Global.System.Console.In.ReadToEnd()
			Dim _ExprXML As System.XML.LINQ.XElement = System.XML.LINQ.XElement.Parse(_ExprXMLString)
			Dim _Expr As DocScript.Language.Expressions.IExpression = DocScript.Language.Expressions.ExprUtilities.ConstructExpressionFromXML(_ExprXML)
			Dim _ResolvedExpr$ = _Expr.Resolve(DocScript.Runtime.SymbolTablesSnapshot.Empty).ReturnStatus.IExpression_ResolutionResult.ToString()

			Console.Out.Write(_ResolvedExpr)

			Return 0

		Catch _Ex As Exception
			DocScript.CompilerExtentions.UsefulMethods.ConsoleErrorWriteLineInColour("DSResolveExpr Error: " & _Ex.Message, ConsoleColor.Red)
			Return 1
		End Try
	End Function

End Module