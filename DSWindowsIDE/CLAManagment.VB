Public Module CLAManagment

	Public CLAPassthrough_SourceFileToOpen As IO.FileInfo = Nothing
	Public CLAPassthrough_RunWhenReady As [Boolean] = False
	Public CLAPassthrough_TextToInsertIntoTextEditor As [String] = Nothing

	REM NOTE: ExamineCLAs_And_SetPassthroughVariables() is called from MainWindow\New()
	Public Sub ExamineCLAs_And_SetPassthroughVariables()
		Try

			REM ↓ Includes the path to the binary at [0]
			Dim _InstanceCLAs$() = Environment.GetCommandLineArgs()

			REM ↓ If we have just been passed one file as a CLA, then just open it, instead of bothering with the CLAManager at all...
			If (_InstanceCLAs.Length = 2) AndAlso [UsefulMethods].Result_OrFalseOnException(Function() (New IO.FileInfo(_InstanceCLAs.ElementAt(1))).Exists) Then
				CLAManagment.CLAPassthrough_SourceFileToOpen = New IO.FileInfo(_InstanceCLAs.ElementAt(1))
				Return
			End If

			REM ↓ We could be given a Source-File like /OpenSourceFile:"X:\HelloWorld.DS"
			Dim _CLAManager As New DocScript.Utilities.CLAManager(
			_InstanceCLAs, "DocScript Windows Integrated-Development-Environment. Create, Execute, and Analyse DocScript Programs. Note that for compatibility reasons, source files can be passed to DSIDE.exe in the form DSIDE.exe {PathToSourceFile}. However, the /Key:Value CLAs provide more options.",
			{"DSIDE.exe /OpenSourceFile:""X:\HelloWorld.DS"""}, Sub(_Msg$) CLAManagment.CLAPassthrough_TextToInsertIntoTextEditor = _Msg, False,
			  New Utilities.CLAManager.CLADatum("OpenSourceFile", "[Datum] A path (relative or absolute) to a DocScript Source-File", False, True, Sub(_Value$) CLAManagment.CLAPassthrough_SourceFileToOpen = New IO.FileInfo(_Value)),
			  New Utilities.CLAManager.CLADatum("OpenSourceString", "[Datum] A string of flattened DocScript Source to load into the Text-Editor. Use ` for "", and ; for {LineBreak}. In this instance only, you can also use \\\\ to represent [TAB].", False, True, Sub(_Value$) CLAManagment.CLAPassthrough_TextToInsertIntoTextEditor = DocScript.Runtime.Parser.UnflattenSource(_Value).Replace("\\\\", [vbTab])),
			  New Utilities.CLAManager.CLADatum("Run", "[Action] Specifies that the DocScript Program in the file from /OpenSourceFile:<value> should be Run immediately. This is not valid for /OpenSourceString.", False, False, Sub(_Value$) CLAManagment.CLAPassthrough_RunWhenReady = True)
			) With {.UseCommandLineFormatting_ForHelpDictionary = False}

		Catch _Ex As Exception When True
			MsgBox("CLAs Exception: " & _Ex.Message, MsgBoxStyle.Critical)
			Environment.Exit(1)
		End Try
	End Sub

End Module