Public Module DSProgXMLToSource

	REM DSPipelining\DSProgXMLToSource
	REM Expected StdIn:		<Program ... />
	REM Provided StdOut:	FUNCTION <VOID> MAIN () ... ENDFUNCTION

	Public Function Main%(ByVal _CLAs$())
		Try

			Dim _ProgTreeXMLString$ = Global.System.Console.In.ReadToEnd()
			Dim _ProgTreeXML As System.XML.LINQ.XElement = System.XML.LINQ.XElement.Parse(_ProgTreeXMLString)
			Dim _Program As DocScript.Runtime.Program = DocScript.Runtime.Program.FromProgramTreeXML(_ProgTreeXML, _ExeCxt:=Nothing)

			Dim _ReconstructedSource$ = _Program.ReconstructedSource

			Console.Out.Write(_ReconstructedSource)

			Return 0

		Catch _Ex As Exception
			DocScript.CompilerExtentions.UsefulMethods.ConsoleErrorWriteLineInColour("DSProgXMLToSource Error: " & _Ex.Message, ConsoleColor.Red)
			Return 1
		End Try
	End Function

End Module