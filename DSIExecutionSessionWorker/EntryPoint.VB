Module EntryPoint

#Region "Instance-specific Data"

	Public TargetExeSes_ESID$ = Nothing 'Initialised by the CLA-Manager

#End Region

	''' <summary>The EntryPoint to the Application</summary>
	''' <param name="_CLAs">The Command-Line Arguments</param>
	''' <returns>0=AllWasFine; 1=ExceptionWasThrown</returns>
	Public Function Main(ByVal _CLAs As String()) As Int32
		Try

			DocScript.Logging.LogUtilities.CurrentLogEventHandler = ESWorker.ESWorkerLogEventHandler

			REM Should be given an ESID like /ESID:"HELLO_AH47"
			Dim _CLAManager As New DocScript.Utilities.CLAManager(
			_CLAs, "DocScript Interactive Execution-Session Worker Executable.",
			{"DSIExecutionSessionWorker.exe /ESID:""DSINTE_GPB"""}, AddressOf Console.WriteLine, True,
			  New Utilities.CLAManager.CLADatum("ESID", "[Datum] The ID of the ExecutionSession to host", True, True, Sub(_Value$) EntryPoint.TargetExeSes_ESID = _Value)
			) : If _CLAManager.HelpDictionaryWasDisplayed Then End

			REM Host the Interpretation of the ExecutionSession's Program
			ESWorker.HostExecutionSession(_ESID:=EntryPoint.TargetExeSes_ESID)

			'That HostExecutionSession() Method should Environment.Exit() with the correct ExitCode.
			'Therefore, if we're here, then that hasn't happerened, so something's gone wrong.
			Throw New DSException("The HostExecutionSession() Method did not cause the ESWorker Process to exit.")

		Catch _Ex As Exception When True
			'If we're here, then an Exception was Thrown that we can't nessecceriarly Log as the ExitReason for the ExeSes
			[UsefulMethods].ConsoleWriteLineInColour("Exception: " & _Ex.Message, ConsoleColor.Red) : Return 1
		End Try
	End Function

End Module