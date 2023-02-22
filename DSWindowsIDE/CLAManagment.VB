'In Application.g.vb, put this as the first line of the EntryPoint Main():
'CLAManagment.SetPassthroughVariables_FromCLAs()

Public Module CLAManagment

	Public CLAPassthrough_SourceFileToOpen As IO.FileInfo = Nothing
	Public CLAPassthrough_RunWhenReady As Boolean = False
	Public CLAPassthrough_CLAHelpDictionaryText As String = Nothing

	Public Sub ExamineCLAs_And_SetPassthroughVariables()
		Try

			REM We could be given a Source-File like /OpenSourceFile:"X:\HelloWorld.DS"
			Dim _CLAManager As New DocScript.Utilities.CLAManager(
			Environment.GetCommandLineArgs(), "DocScript Windows Integrated-Development-Environment. Create, Execute, and Analyse DocScript Programs.",
			{"DSIDE.exe /OpenSourceFile:""X:\HelloWorld.DS"""}, Sub(_Msg$) CLAManagment.CLAPassthrough_CLAHelpDictionaryText = _Msg, False,
			  New Utilities.CLAManager.CLADatum("OpenSourceFile", "[Datum] A path (relative or absolute) to a DocScript Source-File", False, True, Sub(_Value$) CLAManagment.CLAPassthrough_SourceFileToOpen = New IO.FileInfo(_Value)),
			  New Utilities.CLAManager.CLADatum("Run", "[Action] Specifies that the DocScript Program in the file from /OpenSourceFile:<value> should be Run immediately.", False, False, Sub(_Value$) CLAManagment.CLAPassthrough_RunWhenReady = True)
			) With {.UseCommandLineFormatting_ForHelpDictionary = False}

			'If _CLAManager.HelpDictionaryWasDisplayed Then End

		Catch _Ex As Exception When True
			[UsefulMethods].ConsoleWriteLineInColour("CLAs Exception: " & _Ex.Message, ConsoleColor.Red) : Environment.Exit(1)
		End Try
	End Sub

End Module