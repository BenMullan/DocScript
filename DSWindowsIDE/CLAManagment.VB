Public Module CLAManagment

	Public CLAPassthrough_SourceFileToOpen As IO.FileInfo = Nothing
	Public CLAPassthrough_RunWhenReady As Boolean = False
	Public CLAPassthrough_TextToInsertIntoTextEditor As String = Nothing

	Public Sub ExamineCLAs_And_SetPassthroughVariables()
		Try

			Dim _InstanceCLAs$() = Environment.GetCommandLineArgs() 'Includes the path to the binary at [0]

			REM If we have just been passed one file as a CLA, then just open it, instead of bothering with the CLAManager at all...
			If (_InstanceCLAs.Length = 2) AndAlso (New IO.FileInfo(_InstanceCLAs.ElementAt(1))).Exists Then
				CLAManagment.CLAPassthrough_SourceFileToOpen = New IO.FileInfo(_InstanceCLAs.ElementAt(1))
				Return
			End If

			REM We could be given a Source-File like /OpenSourceFile:"X:\HelloWorld.DS"
			Dim _CLAManager As New DocScript.Utilities.CLAManager(
			_InstanceCLAs, "DocScript Windows Integrated-Development-Environment. Create, Execute, and Analyse DocScript Programs.",
			{"DSIDE.exe /OpenSourceFile:""X:\HelloWorld.DS"""}, Sub(_Msg$) CLAManagment.CLAPassthrough_TextToInsertIntoTextEditor = _Msg, False,
			  New Utilities.CLAManager.CLADatum("OpenSourceFile", "[Datum] A path (relative or absolute) to a DocScript Source-File", False, True, Sub(_Value$) CLAManagment.CLAPassthrough_SourceFileToOpen = New IO.FileInfo(_Value)),
			  New Utilities.CLAManager.CLADatum("Run", "[Action] Specifies that the DocScript Program in the file from /OpenSourceFile:<value> should be Run immediately.", False, False, Sub(_Value$) CLAManagment.CLAPassthrough_RunWhenReady = True)
			) With {.UseCommandLineFormatting_ForHelpDictionary = False}

		Catch _Ex As Exception When True
			MsgBox("CLAs Exception: " & _Ex.Message, MsgBoxStyle.Critical)
			Environment.Exit(1)
		End Try
	End Sub

End Module