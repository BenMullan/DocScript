Friend Module ExecutionContextResources

	Public ReadOnly Property ESWorkerExecutionContext(ByVal _ESID$) As DocScript.Runtime.ExecutionContext
		Get
			Return New DocScript.Runtime.ExecutionContext(
			 _RootFolder:=New IO.DirectoryInfo(IO.Directory.GetCurrentDirectory()),
			 _InputDelegate:=ExecutionContextResources.ESWorkerInputDelegate,
			 _OutputDelegate:=ExecutionContextResources.ESWorkerOutputDelegate,
			 _BuiltInFunctions:=ExecutionContext.AllStandardBuiltInFunctions.Concat(DSIBuiltInFunctions.All_DSI_BuiltInFunctions(_ESID)).ToArray()
			)
		End Get
	End Property

	Public ReadOnly ESWorkerInputDelegate As Func(Of String, String) = _
	 Function(_Prompt$) As String

		 REM Write the "InputPrompt" and "TimeSubmitted" to the [{ESID}_Inputs] Table
		 REM Wait a maximum of ESWorker_InputRequest_MaxLifetime, before Throwing a [New DSInputRequestTimedOutException()]

		 Dim _TargetInputEvent_ID$ = WebParts.DatabaseInteraction.AddExecutionSessionInputEvent_RequestPart(EntryPoint.TargetExeSes_ESID, _Prompt)
		 UsefulMethods.ConsoleWriteLineInColour("Wrote InputEvent to DB: " & _Prompt, ConsoleColor.Magenta)

		 Dim _Request_MustEndAt As DateTime = DateTime.Now.Add(WebParts.Resources.ESWorker_InputRequest_MaxLifetime)
		 While (DateTime.Now < _Request_MustEndAt)

			 If (WebParts.DatabaseInteraction.GetExecutionSesionInputEvents(EntryPoint.TargetExeSes_ESID).First(Function(_InputEvent As XElement) _InputEvent.@ID = _TargetInputEvent_ID).@InputResponse <> WebParts.DBContent.NullValue_Placeholder) Then

				 UsefulMethods.ConsoleWriteLineInColour("Detected InputResponse from DB: " & _Prompt, ConsoleColor.Magenta)

				 'Return the InputResponse present in the {ESID}_Inputs Table
				 Return WebParts.DatabaseInteraction.GetExecutionSesionInputEvents(EntryPoint.TargetExeSes_ESID) _
				   .First(Function(_InputEvent As XElement) _InputEvent.@ID = _TargetInputEvent_ID) _
				   .@InputResponse

			 End If

			 'Pause for a few MS, then Check again...
			 Threading.Thread.Sleep(WebParts.Resources.LongPollingRequests_IntervalCheckingDelay)

		 End While

		 'If we're here, then we didn't receive an Input-Response during the 8 Mins, so Throw a [New DSInputRequestTimedOutException()]
		 LogSystemMessage("[The InputEvent was not responded-to] before the ESWorker Max-InpuptRequest-Lifetime occured; Throwing a [New DSInputRequestTimedOutException()]", LogEvent.DSEventSeverity.Warning)
		 Throw New DSInputRequestTimedOutException(WebParts.Resources.ESWorker_InputRequest_MaxLifetime)

	 End Function

	Public ReadOnly ESWorkerOutputDelegate As Action(Of String) = _
	 Sub(_Message$)

		 REM Log the Message in the ExeSes_Outputs Table
		 WebParts.DatabaseInteraction.AddExecutionSessionOutputEvent(EntryPoint.TargetExeSes_ESID, _Message)
		 UsefulMethods.ConsoleWriteLineInColour("Wrote OutputEvent to DB: " & _Message, ConsoleColor.Blue)

	 End Sub

End Module