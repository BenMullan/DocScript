Public Module GetText

	REM DSPipelining\GetText
	REM Expected StdIn:		(None)
	REM Provided StdOut:	{Text from File specified in _CLAs[0]}

	Public Function Main%(ByVal _CLAs$())
		Try

			If Not (_CLAs.Length = 1) Then Throw New Exception("Provide the path to a Text-based file as the command-line argument.")

			Dim _FileToRead As New System.IO.FileInfo(_CLAs.ElementAt(0))
			Dim _Text$ = My.Computer.FileSystem.ReadAllText(_FileToRead.FullName)

			Console.Out.Write(_Text)

			Return 0

		Catch _Ex As Exception
			DocScript.CompilerExtentions.UsefulMethods.ConsoleErrorWriteLineInColour("GetText Error: " & _Ex.Message, ConsoleColor.Red)
			Return 1
		End Try
	End Function

End Module