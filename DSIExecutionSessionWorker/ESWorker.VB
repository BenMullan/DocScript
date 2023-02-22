Public Module ESWorker

	Public Sub HostExecutionSession(ByVal _ESID$)

		REM There is no CheckExecutionSessionExists(_ESID) Function, because a method such as this need only call GetExistingExecutionSessions() once, itself.
		Dim _ExistantExecutionSessions As XElement() = DocScript.WebParts.DatabaseInteraction.GetExistingExecutionSessions()

		REM Ensure that an ExeSes with _ESID exists
		If Not _ExistantExecutionSessions.Where(Function(_ExeSes As XElement) _ExeSes.@ESID = _ESID).Count = 1 _
		 Then Throw New DSValidationException("There was not exactly-one ExecutionSession for the specified ESID", _ESID)

		'At this point:
		'	- We have an ESID
		'	- We have the ExecutionSession for that ESID
		'	- We need to:
		'		- Get the Program for our ExeSes
		'		- Reset the ExeSes (Clear LogEvents, Outputs, and Inputs)
		'		- Set the "State" of our ExeSes to Running, updating the TimeStarted too.
		'		- Interpret that Program's Source, using the ESWorker's ExeCxt

		'↓ Any Exception hence, is to be logged in the DB as the ExitReason for the ExeSes ↓
		Try

			Dim _Source$ = WebParts.DatabaseInteraction.GetUploadedProgramFromESID(_ESID:=_ESID).@Source _
			   .Replace(vbCrLf, vbLf).Replace(vbLf, vbCrLf) 'The JavaScript might not send the source to the API with all LineBreaks as CrLf

			WebParts.DatabaseInteraction.SetExecutionSessionState_Running(_ESID)
			WebParts.DatabaseInteraction.ResetExecutionSession(_ESID) 'Reset the ExeSes (Clear LogEvents, Outputs, and Inputs)

			Dim _Program As New DocScript.Runtime.Program(
			 _Tokens:=DocScript.Runtime.Parser.GetTokensFromSource(_RawSource:=_Source),
			 _ExecutionContext:=ESWorkerExecutionContext_Resources.ESWorkerExecutionContext(_ESID)
			)

			Dim _Program_ExitCode As Int32 = _
			 _Program.Run({}) _
			 .ReturnStatus.Program_ExitCode _
			 .GetValueOrDefault(defaultValue:=DocScript.Runtime.Constants.ProgramExitCode_Default)

			REM Now:
			'	- Change the ExeSes "State" to Finished
			'	- Write the ExitReason into the Table as: ExitedNormally DSExitCode={_Program_ExitCode}
			'	- Exit from this Process, returning 0

			WebParts.DatabaseInteraction.SetExecutionSessionState_Finished(
			 _ESID:=_ESID,
			 _ExitReason:=String.Join(" "c, WebParts.DBContent.ES_ExitReasonFlag_ExitedNormally, WebParts.DBContent.ES_ExitReasonFlag_DSExitCodeEquals & _Program_ExitCode.ToString())
			 )

			UsefulMethods.ConsoleWriteLineInColour("Exiting with ExitCode 0.", ConsoleColor.Green)
			Environment.Exit(exitCode:=0)

		Catch _InterpretationException As Exception

			REM Any Exception caught here, should be logged as the ExitReason for the ExeSes in the Database.
			REM Now:
			'	- Change the ExeSes "State" to Finished
			'	- Write the ExitReason into the Table as: InterpretationFailure B64Exception={//The Exception's Message, in Base64//}
			'	- Exit from this Process, returning 1

			Try
				WebParts.DatabaseInteraction.SetExecutionSessionState_Finished(
				_ESID:=_ESID,
				_ExitReason:=String.Join(" "c, WebParts.DBContent.ES_ExitReasonFlag_InterpretationFailure, WebParts.DBContent.ES_ExitReasonFlag_B64ExceptionEquals & _InterpretationException.Message.PlainText_ToBase64())
			 )
				UsefulMethods.ConsoleWriteLineInColour("Exiting with ExitCode 1. Exception: " & _InterpretationException.Message, ConsoleColor.Red)
				Environment.Exit(exitCode:=1)
			Catch _WriteExceptionToDB_Ex As Exception When True : Throw New DSException("The ESWorker encountered an Interpretation-Exception, but could not Log this into the DataBase. The Interpretation-Exception was: " & _InterpretationException.Message, _InterpretationException) : End Try

		End Try

	End Sub

	''' <summary>
	''' Indicates that LogEvents should not be processed for the time-being, because they are only being caused as a result of attempting to Log a previous Event.
	''' This prevents the infinite loop of [Log an Event] → [Log the fact that you're logging an Event] → ...
	''' </summary>
	Private IgnoreLogEvents_ As Boolean = False

	Public ReadOnly ESWorkerLogEventHandler As New LogEventHandler(
	 Sub(_LogEvent As LogEvent)

		 REM If this plag is [True], then it's because we're currently in the middle of logging another Event. THerefore, don't log any sub-events resultant of attempting to log that event:
		 If IgnoreLogEvents_ _
		   Then Return

		 REM If [System], forward to the WindowsEventLog
		 If _LogEvent.Catagory = LogEvent.DSEventCatagory.System _
		   Then BuiltInLogEventHandlers.WindowsEventLog("I_ESWorker").Invoke(_LogEvent)

		 REM If not [System], only bother with Information, Warning, and Error
		 If Not {LogEvent.DSEventSeverity.Infomation, LogEvent.DSEventSeverity.Warning, LogEvent.DSEventSeverity.Error}.Contains(_LogEvent.Severity) _
		  Then Return

		 REM We are now dealing with a non-[System], Info/Warn/Err; Log in in the LogEvents Table for this ExeSes
		 IgnoreLogEvents_ = True
		 WebParts.DatabaseInteraction.AddExecutionSessionLogEvent(_ESID:=EntryPoint.TargetExeSes_ESID, _LogEvent:=_LogEvent)
		 IgnoreLogEvents_ = False

	 End Sub
	 )

End Module