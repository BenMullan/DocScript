Public Module DSParse

	REM DSPipelining\DSParse
	REM Expected StdIn:		Raw DocScript Source
	REM Provided StdOut:	<Tokens ... />

	Public Function Main%(ByVal _CLAs$())
		Try

			Dim _RawSource$ = Global.System.Console.In.ReadToEnd()
			Dim _Tokens As DocScript.Runtime.Token() = DocScript.Runtime.Parser.GetTokensFromSource(_RawSource)
			Dim _TokensXML As System.XML.LINQ.XElement = DocScript.Runtime.Parser.GetTokensXML(_Tokens)

			Console.Out.WriteLine(_TokensXML.ToString())

			Return 0

		Catch _Ex As Exception
			DocScript.CompilerExtentions.UsefulMethods.ConsoleErrorWriteLineInColour("DSParse Error: " & _Ex.Message, ConsoleColor.Red)
			Return 1
		End Try
	End Function
	
End Module