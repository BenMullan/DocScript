Public Module DSLexExpr

	REM DSPipelining\DSLexExpr
	REM Expected StdIn:		<Tokens ... />
	REM Provided StdOut:	<*Expr />

	Public Function Main%(ByVal _CLAs$())
		Try

			Dim _TokensXMLString$ = Global.System.Console.In.ReadToEnd()
			Dim _TokensXML As System.XML.LINQ.XElement = System.XML.LINQ.XElement.Parse(_TokensXMLString)
			Dim _Tokens As DocScript.Runtime.Token() = System.Linq.Enumerable.ToArray(Of DocScript.Runtime.Token)(System.Linq.Enumerable.Select(Of System.XML.LINQ.XElement, DocScript.Runtime.Token)(_TokensXML...<Token>, AddressOf DocScript.Runtime.Token.FromXML))
			Dim _Expression As DocScript.Language.Expressions.IExpression = DocScript.Language.Expressions.ExprUtilities.ConstructExpressionFromTokens(_Tokens)
			Dim _ExpressionXML As System.XML.LINQ.XElement = _Expression.GetExprTreeNodeXML()

			DocScript.CompilerExtentions.XMLObjectExtentions.WriteToConsoleWithXMLColourFormatting(_ExpressionXML)

			Return 0

		Catch _Ex As Exception
			DocScript.CompilerExtentions.UsefulMethods.ConsoleErrorWriteLineInColour("DSLexExpr Error: " & _Ex.Message, ConsoleColor.Red)
			Return 1
		End Try
	End Function

End Module