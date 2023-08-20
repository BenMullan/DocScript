Public Module DSExec

	REM DSPipelining\DSExec
	REM Expected StdIn:		<Program ... />
	REM Provided StdOut:	{DocScript Program's Output}

	Public Function Main%(ByVal _CLAs$())
		Try

			Dim _ProgTreeXMLString$ = Global.System.Console.In.ReadToEnd()
			Dim _ProgTreeXML As System.XML.LINQ.XElement = System.XML.LINQ.XElement.Parse(_ProgTreeXMLString)
			Dim _Program As DocScript.Runtime.Program = DocScript.Runtime.Program.FromProgramTreeXML(_ProgTreeXML, _ExeCxt:=DocScript.Runtime.ExecutionContext.CLIDefault)

			_Program.Run(_CLAs)

			Return 0

		Catch _Ex As Exception
			DocScript.CompilerExtentions.UsefulMethods.ConsoleErrorWriteLineInColour("DSExec Error: " & _Ex.Message, ConsoleColor.Red)
			Return 1
		End Try
	End Function

End Module