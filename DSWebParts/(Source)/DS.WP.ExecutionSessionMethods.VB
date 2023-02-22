Namespace WebParts

	Public Module ExecutionSessionMethods

		''' <summary>Waits for a maximum of 120 seconds for the ExeSes to start, otherwise Responds with a LongPollingTimeout.
		''' Response when the ExeSes's "State" is [Running] OR [Finished].</summary>
		Public Sub AwaitSessionInitiation(ByRef _Request As HttpRequest, ByRef _Response As HttpResponse)

			REM There is no CheckExecutionSessionExists(_ESID) Function, because a method such as this need only call GetExistingExecutionSessions() once, itself.
			Dim _ExistantExecutionSessions As XElement() = DocScript.WebParts.DatabaseInteraction.GetExistingExecutionSessions()
			Dim _TargetExeSes_ESID$ = _Request.QueryString("ESID")

			REM Ensure that an ExeSes with ?ESID exists
			If Not _ExistantExecutionSessions.Where(Function(_ExeSes As XElement) _ExeSes.@ESID = _TargetExeSes_ESID).Count = 1 _
			 Then Throw New DSValidationException("There was not exactly-one ExecutionSession for the specified ESID", _TargetExeSes_ESID$)

			REM Check the "State" of the ExeSes:
			'	- Ready:	Wait around here, until it starts
			'	- Running:	Return straight away
			'	- Finished:	Return an Error (Cannot wait for the ExeSes to start)
			Dim _TargetExeSes As XElement = _ExistantExecutionSessions.First(Function(_ExeSes As XElement) _ExeSes.@ESID = _TargetExeSes_ESID)
			Select Case _TargetExeSes.@State
				Case DBContent.ExeSes_State_Ready 'Continue as below...
				Case DBContent.ExeSes_State_Running, DBContent.ExeSes_State_Finished
					APIResponse.FromValidRequest({"ESIsRunningOrFinished", "True"}, {"ESTimeStarted", _ExistantExecutionSessions.First(Function(_ExeSes As XElement) _ExeSes.@ESID = _TargetExeSes_ESID).@TimeStarted}).Send(_Response)
				Case Else : Throw New DSValidationException("The ExecutionSession's State was not recognised", _TargetExeSes.@State)
			End Select

			'At this point, we should begin listening for a change in the "State" of the ExeSes
			'We Respond to the Client, when either [the "State" changes], or [the Timeout is exceeded]

			Dim _Request_MustEndAt As DateTime = DateTime.Now.Add(WebParts.Resources.LongPollingRequests_MaxLifetime)

			While (DateTime.Now < _Request_MustEndAt)

				'Check the "State" of [our target ExeSes]
				Select Case DatabaseInteraction.GetExecutionSesionState(_TargetExeSes_ESID)
					Case DBContent.ExeSes_State_Running, DBContent.ExeSes_State_Finished

						'Respond, saying that the ExeSes has begun; it may actually have already finished
						_ExistantExecutionSessions = DocScript.WebParts.DatabaseInteraction.GetExistingExecutionSessions()
						Dim _ExeSes_TimeStarted$ = _ExistantExecutionSessions.First(Function(_ExeSes As XElement) _ExeSes.@ESID = _TargetExeSes_ESID).@TimeStarted
						LogSystemMessage("The ExecutionSession " & _TargetExeSes_ESID.InBrackets() & " was [Running] or [Finished] before the Long-Polling Max-Lifetime elapsed; Responding with the ExeSes's TimeStarted", LogEvent.DSEventSeverity.Infomation)
						APIResponse.FromValidRequest({"ESIsRunningOrFinished", "True"}, {"ESTimeStarted", _ExeSes_TimeStarted}).Send(_Response)

					Case DBContent.ExeSes_State_Ready
						'Fine; we'll just keep checking...
					Case Else
						'Not fine; the ExeSes may have Finished before we saw it Start
						APIResponse.FromInternalServerError("The ExecutionSession's State was not [Ready] or [Running] or [Finished], when checked the interval-checking loop.").Send(_Response)
				End Select

				'Pause for a few MS, then Check again...
				Threading.Thread.Sleep(WebParts.Resources.LongPollingRequests_IntervalCheckingDelay)

			End While

			'If we're here, then we didn't Respond during the loop, so the _Request_MustEndAt was reached
			LogSystemMessage("The ExecutionSession (".Glue(_TargetExeSes_ESID, ") did not start before the Long-Polling Max-Lifetime elapsed; Responding with LongPollingTimedOut=True"), LogEvent.DSEventSeverity.Infomation)
			APIResponse.FromLongPollingTimeout().Send(_Response)

		End Sub

		''' <summary>Waits for a maximum of 120 seconds for an ES-Event to befall, otherwise Responds with a LongPollingTimeout</summary>
		Public Sub ListenForESEvents(ByRef _Request As HttpRequest, ByRef _Response As HttpResponse)

			'(Copied from API)
			REM QueryStrings:	[ESID], [LogEventsCount], [OutputEventsCount], [InputEventsCount], [CEPsCount]
			REM Returns:		[...ESState], [...ESTimeEnded], [...MissingLogEvents], [...MissingOutputEvents], [...MissingInputEvents], [...MissingCEPs], [...InputIsRequired], [...InputPrompt], [...InputEventID]

			Dim _ESID$ = _Request.QueryString!ESID,
			_Client_LogEventsCount As UInt32 = _Request.QueryString("LogEventsCount").Extract_UInt32(),
			_Client_OutputEventsCount As UInt32 = _Request.QueryString("OutputEventsCount").Extract_UInt32(),
			_Client_InputEventsCount As UInt32 = _Request.QueryString("InputEventsCount").Extract_UInt32(),
			_Client_CEPsCount As UInt32 = _Request.QueryString("CEPsCount").Extract_UInt32()

			Dim _ExistantExecutionSessions As XElement() = DocScript.WebParts.DatabaseInteraction.GetExistingExecutionSessions()

			REM Ensure that an ExeSes with ?ESID exists
			If Not _ExistantExecutionSessions.Where(Function(_ExeSes As XElement) _ExeSes.@ESID = _ESID).Count = 1 _
			  Then Throw New DSValidationException("There was not exactly-one ExecutionSession for the specified ESID", _ESID)

			Dim _Target_ExeSes As XElement = _ExistantExecutionSessions.First(Function(_ExeSes As XElement) _ExeSes.@ESID = _ESID)

			REM If the Session is [Ready], then this is not a valid request
			If _Target_ExeSes.@State.ToUpper() = DBContent.ExeSes_State_Ready.ToUpper() _
			   Then APIResponse.FromInvalidRequest("The Execution-Session was in the [Ready] state, but ES-Events can only be listened for when an Execution-Session is Running, or Finished (with the latter, this is only to pull-down an archived copy the Events).").Send(_Response)

			REM If the ?OutputEventsCount is greater than the number of OutputEvents, then the Client is lying.
			If (_Client_OutputEventsCount > DatabaseInteraction.GetExecutionSesionOutputEvents(_ESID).Length) _
			   Then APIResponse.FromInvalidRequest("The ?OutputEventsCount (" & _Client_OutputEventsCount.ToString() & ") was invalid, or greater than the number of actual Output-Events of the Execution-Session (" & DatabaseInteraction.GetExecutionSesionOutputEvents(_ESID).Length.ToString() & "), which is impossible.").Send(_Response)

			REM If the ?InputEventsCount is greater than the number of InputEvents, then the Client is lying.
			REM This needs to compare against only the COMPLETED Input Events
			Dim _InputEvent_HasNullInputResponse As Func(Of XElement, Boolean) = Function(_InputEvent As XElement) _InputEvent.@InputResponse = DBContent.NullValue_Placeholder
			Dim _InputEventsCountValidation_ExeSes_AllInputEvents As XElement() = DatabaseInteraction.GetExecutionSesionInputEvents(_ESID)
			Dim _InputEventsCountValidation_ExeSes_CompletedInputEvents As XElement() = If(_InputEventsCountValidation_ExeSes_AllInputEvents.Any(_InputEvent_HasNullInputResponse), _InputEventsCountValidation_ExeSes_AllInputEvents.UpToButExcluding(_InputEvent_HasNullInputResponse), _InputEventsCountValidation_ExeSes_AllInputEvents)
			If (_Client_InputEventsCount > _InputEventsCountValidation_ExeSes_CompletedInputEvents.Length) _
			Then APIResponse.FromInvalidRequest("The ?InputEventsCount (" & _Client_InputEventsCount.ToString() & ") was invalid, or greater than the number of COMPLETED (responded-to) Input-Events of the Execution-Session (" & _InputEventsCountValidation_ExeSes_CompletedInputEvents.Length.ToString() & "), which is impossible.").Send(_Response)

			REM If the ?LogEventsCount is greater than the number of LogEvents, then the Client is lying.
			If (_Client_LogEventsCount > DatabaseInteraction.GetExecutionSesionLogEvents(_ESID).Length) _
			   Then APIResponse.FromInvalidRequest("The ?LogEventsCount (" & _Client_LogEventsCount.ToString() & ") was invalid, or greater than the number of actual Log-Events of the Execution-Session (" & DatabaseInteraction.GetExecutionSesionLogEvents(_ESID).Length.ToString() & "), which is impossible.").Send(_Response)

			REM If the ?CEPsCount is greater than the number of CEPs, then the Client is lying.
			If (_Client_CEPsCount > DatabaseInteraction.GetExecutionSesionCEPs(_ESID).Length) _
			   Then APIResponse.FromInvalidRequest("The ?CEPsCount (" & _Client_CEPsCount.ToString() & ") was invalid, or greater than the number of actual CEPs of the Execution-Session (" & DatabaseInteraction.GetExecutionSesionCEPs(_ESID).Length.ToString() & "), which is impossible.").Send(_Response)

			'It is the responsibility of the API, to ensure that if the ExeSes is Finished, then InputIsRequired must not be True
			If (_Target_ExeSes.@State.ToUpper() = DBContent.ExeSes_State_Finished.ToUpper()) AndAlso _InputEventsCountValidation_ExeSes_AllInputEvents.Any(_InputEvent_HasNullInputResponse) _
			  Then APIResponse.FromInternalServerError("The Execution-Session is [Finished], but there was at least one InputEvent with a NULL InputResponse. This may be because the ExeSes terminated owing to an InputResponse-Timeout. The DSI Server cannot serve the response because the Client would attempt to ask the User for Input, despite the session being over.").Send(_Response)

			REM Every LongPollingRequests_IntervalCheckingDelay, check if:
			'	- The ExeSes's "State" is finished
			'	- There is a record in the ESID_Inputs Table, where the [InputResponse] is NULL (Input is required)
			'	- The ExeSes's OutputEventCount is greater than ?OutputEventsCount
			'	- The ExeSes's InputEventCount is greater than ?InputEventsCount
			'	- The ExeSes's CEPsCount is greater than ?CEPsCount
			REM If any of these conditions are met, then:
			'	- Send off a Response, with [...ESState], [...ESTimeEnded], [...MissingLogEvents], [...MissingOutputEvents], [...MissingInputEvents], [...MissingCEPs], [...InputIsRequired], [...InputPrompt], [...InputEventID]

			Dim _Request_MustEndAt As DateTime = DateTime.Now.Add(WebParts.Resources.LongPollingRequests_MaxLifetime)

			While (DateTime.Now < _Request_MustEndAt)

				REM For the ↓ [The ExeSes's InputEventCount is greater than ?InputEventsCount] ↓ Check, we actually don't need to worry about
				REM ...weather we're counting-up any Records with NULL InputResponses, because the ↓ [There is a record in the ESID_Inputs Table, where the [InputResponse] is NULL (Input is required)] ↓
				REM ...check would be evaluated to True first, but if we've gotten to the ESInputEvents-More-Than-ClientKnows check, then it must have been false.
				REM We never want to send accross any non-complete InputEvents to the Client, because the Client would count these in its next ?InputEventsCount, which would
				REM ...result in the Server (here) not asking the Client for a Response to the latest InputEvent

				If (
				  (DatabaseInteraction.GetExecutionSesionState(_ESID).ToUpper() = DBContent.ExeSes_State_Finished.ToUpper()) _
				  OrElse (DatabaseInteraction.GetExecutionSesionInputEvents(_ESID).Any(_InputEvent_HasNullInputResponse)) _
				  OrElse (DatabaseInteraction.GetExecutionSesionOutputEvents(_ESID).Length > _Client_OutputEventsCount) _
				  OrElse (DatabaseInteraction.GetExecutionSesionInputEvents(_ESID).Length > _Client_InputEventsCount) _
				  OrElse (DatabaseInteraction.GetExecutionSesionCEPs(_ESID).Length > _Client_CEPsCount)
				) Then

					'We only what to Respond with Completed-Input-Events; ones with NULLs for "InputResponse" should NEVER be send
					Dim _ExeSes_AllInputEvents As XElement() = DatabaseInteraction.GetExecutionSesionInputEvents(_ESID)
					Dim _ExeSes_CompletedInputEvents As XElement() = If( _
					  _ExeSes_AllInputEvents.Any(_InputEvent_HasNullInputResponse),
					  _ExeSes_AllInputEvents.UpToButExcluding(_InputEvent_HasNullInputResponse),
					  _ExeSes_AllInputEvents
					)

					APIResponse.FromValidRequest(
					 _ResponseContent_KVPs:=Get2DArray(
					  {"ESState", DatabaseInteraction.GetExecutionSesionState(_ESID)},
					  {"ESTimeEnded", DatabaseInteraction.GetExistingExecutionSessions().First(Function(_ExeSes As XElement) _ExeSes.@ESID = _ESID).@TimeEnded},
					  {"InputIsRequired", _ExeSes_AllInputEvents.Any(_InputEvent_HasNullInputResponse).ToString()},
					  {"InputPrompt", If(_ExeSes_AllInputEvents.Any(_InputEvent_HasNullInputResponse), _ExeSes_AllInputEvents.First(_InputEvent_HasNullInputResponse).@InputPrompt, "")},
					  {"InputEventID", If(_ExeSes_AllInputEvents.Any(_InputEvent_HasNullInputResponse), _ExeSes_AllInputEvents.First(_InputEvent_HasNullInputResponse).@ID, "")}
					   ),
					 _ResponseContent_XMLChildren:={
					   (Function(_AllLogEvents As XElement()) _AllLogEvents.TakeBlockFromEnd(CUInt(_AllLogEvents.Length - _Client_LogEventsCount)).WrapIn("MissingLogEvents")).Invoke(DatabaseInteraction.GetExecutionSesionLogEvents(_ESID)),
					   (Function(_AllOutputEvents As XElement()) _AllOutputEvents.TakeBlockFromEnd(CUInt(_AllOutputEvents.Length - _Client_OutputEventsCount)).WrapIn("MissingOutputEvents")).Invoke(DatabaseInteraction.GetExecutionSesionOutputEvents(_ESID)),
					   _ExeSes_CompletedInputEvents.TakeBlockFromEnd(CUInt(_ExeSes_CompletedInputEvents.Length - _Client_InputEventsCount)).WrapIn("MissingInputEvents"),
					   (Function(_AllCEPs As XElement()) _AllCEPs.TakeBlockFromEnd(CUInt(_AllCEPs.Length - _Client_CEPsCount)).WrapIn("MissingCEPs")).Invoke(DatabaseInteraction.GetExecutionSesionCEPs(_ESID))
					 }
					 ).Send(_Response)

				End If

				'Pause for a few MS, then Check again...
				Threading.Thread.Sleep(WebParts.Resources.LongPollingRequests_IntervalCheckingDelay)

			End While

			'If we're here, then we didn't Respond during the loop, so the _Request_MustEndAt was reached
			LogSystemMessage("The ExecutionSession (".Glue(_ESID, ") did not raise any Events before the Long-Polling Max-Lifetime elapsed; Responding with LongPollingTimedOut=True"), LogEvent.DSEventSeverity.Infomation)
			APIResponse.FromLongPollingTimeout().Send(_Response)

		End Sub

		''' <summary>Waits for a maximum of 120 seconds for an InputResponse to be provided, otherwise Responds with a LongPollingTimeout</summary>
		Public Sub ListenForInputInterrupts(ByRef _Request As HttpRequest, ByRef _Response As HttpResponse)

			Dim _ESID$ = _Request.QueryString!ESID,
			 _InputEventID$ = _Request.QueryString!InputEventID

			REM Ensure that an ExeSes with ?ESID exists, and that it is not [Ready]
			If Not DatabaseInteraction.GetExistingExecutionSessions().Where(Function(_ExeSes As XElement) (_ExeSes.@ESID = _ESID) AndAlso (_ExeSes.@State.ToUpper() <> DBContent.ExeSes_State_Ready.ToUpper())).Count = 1 _
			 Then APIResponse.FromInvalidRequest("There was not exactly-one ExecutionSession for the specified ESID, or the Execution-Session had a ""State"" of Ready. InputInterrupts can only be listened-for when the ""State"" is [Running] (and actually also [Finished], but that'll Respond back instantly saying ESHasFinished=True).").Send(_Response)

			'Ensure there is one InputEvent with ?InputEventID
			If Not DatabaseInteraction.GetExecutionSesionInputEvents(_ESID).Where(Function(_InputEvent As XElement) _InputEvent.@ID = _InputEventID$).Count = 1 _
			 Then APIResponse.FromInvalidRequest("There was not exactly-one InputEvent with the specified InputEventID " & _InputEventID$.InBrackets()).Send(_Response)

			REM Eventualities for this API EndPoint:
			'Input is responded-to:			InputHasBeenRespondedTo=True  ESHasFinished=False
			'ES-over, & IE responded-to:	InputHasBeenRespondedTo=True  ESHasFinished=True
			'ES has finished:				InputHasBeenRespondedTo=False ESHasFinished=True
			'InputTimeout occurs first:		(Long-Polling-Timeout)

			REM Every LongPollingRequests_IntervalCheckingDelay, check if:
			'	- The "InputResponse" for {_InputEventID$} is NOT-NULL
			'	- The ExeSes's "State" is Finished (in which case, the InputTimeout was exceeded before a response was provided, or the IE was responded-to, and then the ES finished before this loop noticed that the IE became responded-to)
			REM If any of these conditions are met, then:
			'	- Send off a Response, with [...InputHasBeenRespondedTo] and [...ESHasFinished]

			Dim _Request_MustEndAt As DateTime = DateTime.Now.Add(WebParts.Resources.LongPollingRequests_MaxLifetime)
			While (DateTime.Now < _Request_MustEndAt)

				If (
				  (DatabaseInteraction.GetExecutionSesionInputEvents(_ESID).First(Function(_InputEvent As XElement) _InputEvent.@ID = _InputEventID$).@InputResponse <> DBContent.NullValue_Placeholder) _
				  OrElse (DatabaseInteraction.GetExistingExecutionSessions().Where(Function(_ExeSes As XElement) _ExeSes.@ESID = _ESID).@State.ToUpper() = DBContent.ExeSes_State_Finished.ToUpper())
				) Then

					APIResponse.FromValidRequest(
					 {"InputHasBeenRespondedTo", (DatabaseInteraction.GetExecutionSesionInputEvents(_ESID).First(Function(_InputEvent As XElement) _InputEvent.@ID = _InputEventID$).@InputResponse <> DBContent.NullValue_Placeholder).ToString()},
					 {"ESHasFinished", (DatabaseInteraction.GetExistingExecutionSessions().Where(Function(_ExeSes As XElement) _ExeSes.@ESID = _ESID).@State.ToUpper() = DBContent.ExeSes_State_Finished.ToUpper()).ToString()}
					).Send(_Response)

				End If

				'Pause for a few MS, then Check again...
				Threading.Thread.Sleep(WebParts.Resources.LongPollingRequests_IntervalCheckingDelay)

			End While

			'If we're here, then we didn't Respond during the loop, so the _Request_MustEndAt was reached
			LogSystemMessage("[The InputEvent was not responded-to] and [The ES didn't become Finished] before the Long-Polling Timeout occured; responding with LongPollingTimedOut=True", LogEvent.DSEventSeverity.Infomation)
			APIResponse.FromLongPollingTimeout().Send(_Response)

		End Sub

	End Module

End Namespace