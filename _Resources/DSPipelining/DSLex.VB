Public Module DSLex

	REM DSPipelining\DSLex
	REM Expected StdIn:		<Tokens ... />
	REM Provided StdOut:	<Program ... />

	Public Function Main%(ByVal _CLAs$())
		Try

			Dim _TokensXMLString$ = Global.System.Console.In.ReadToEnd()
			Dim _TokensXML As System.XML.LINQ.XElement = System.XML.LINQ.XElement.Parse(_TokensXMLString)
			Dim _Program As DocScript.Runtime.Program = DocScript.Runtime.Program.FromTokensXML(_TokensXML, _ExeCxt:=Nothing)
			Dim _ProgramTreeXML As System.XML.LINQ.XElement = _Program.ProgramTreeXML

			DocScript.CompilerExtentions.XMLObjectExtentions.WriteToConsoleWithXMLColourFormatting(_ProgramTreeXML)

			Return 0

		Catch _Ex As Exception
			DocScript.CompilerExtentions.UsefulMethods.ConsoleErrorWriteLineInColour("DSLex Error: " & _Ex.Message, ConsoleColor.Red)
			Return 1
		End Try
	End Function

End Module