Public Module DSOptimise

	REM DSPipelining\DSOptimise
	REM Expected StdIn:		<Program ... />
	REM Provided StdOut:	<Program ... />

	Public Function Main%(ByVal _CLAs$())
		Try

			Dim _ProgTreeXMLString$ = Global.System.Console.In.ReadToEnd()
			Dim _ProgTreeXML As System.XML.LINQ.XElement = System.XML.LINQ.XElement.Parse(_ProgTreeXMLString)
			Dim _Program As DocScript.Runtime.Program = DocScript.Runtime.Program.FromProgramTreeXML(_ProgTreeXML, _ExeCxt:=Nothing)

			Dim _OptimisedProgram As DocScript.Runtime.Program = (New DocScript.Runtime.Optimisation.ComponentOptimiser()).OptimiseProgram(_Program)
			Dim _OptimisedProgramTreeXML As System.XML.LINQ.XElement = _OptimisedProgram.ProgramTreeXML

			DocScript.CompilerExtentions.XMLObjectExtentions.WriteToConsoleWithXMLColourFormatting(_OptimisedProgramTreeXML)

			Return 0

		Catch _Ex As Exception
			DocScript.CompilerExtentions.UsefulMethods.ConsoleErrorWriteLineInColour("DSOptimise Error: " & _Ex.Message, ConsoleColor.Red)
			Return 1
		End Try
	End Function

End Module