Public Module DSCompile

	REM DSPipelining\DSCompile
	REM Expected StdIn:		<Program ... />
	REM Provided StdOut:	{An EXE File}

	Public Function Main%(ByVal _CLAs$())
		Try

			If Not (_CLAs.Length = 1) Then Throw New Exception("Provide the name for the output EXE file as the command-line argument.")

			Dim _ProgTreeXMLString$ = Global.System.Console.In.ReadToEnd()
			Dim _ProgTreeXML As System.XML.LINQ.XElement = System.XML.LINQ.XElement.Parse(_ProgTreeXMLString)
			Dim _Program As DocScript.Runtime.Program = DocScript.Runtime.Program.FromProgramTreeXML(_ProgTreeXML, _ExeCxt:=DocScript.Runtime.ExecutionContext.CLIDefault)

			_Program.CompileToEXE(
			 _OutputDirectory:=(New IO.FileInfo(_CLAs.ElementAt(0))).Directory,
			 _FinalExeFile_NameOnly:=(New IO.FileInfo(_CLAs.ElementAt(0))).Name,
			 _VBCExe:=DocScript.Runtime.Program.DefaultVbcExe_ForCurrentSystem,
			 _ILMergeExe:=New IO.FileInfo(IO.Path.Combine(DocScript.CompilerExtentions.UsefulMethods.GetExecutingAssemblyDirectory().FullName, "ILMerge.exe")),
			 _DSCoreDll:=New IO.FileInfo(IO.Path.Combine(DocScript.CompilerExtentions.UsefulMethods.GetExecutingAssemblyDirectory().FullName, "DSCore.dll")),
			 _IcoFile_ForExe:=Nothing,
			 _DeleteIntermediateFiles:=True
			)

			Console.Out.Write("DSCompile: Successfully compiled " & (New IO.FileInfo(_CLAs.ElementAt(0))).FullName)
			Return 0

		Catch _Ex As Exception
			DocScript.CompilerExtentions.UsefulMethods.ConsoleErrorWriteLineInColour("DSCompile Error: " & _Ex.Message, ConsoleColor.Red)
			Return 1
		End Try
	End Function

End Module