Namespace WebParts

	Partial Public NotInheritable Class API

		Public Shared Sub ExecutionSessionASPX(ByRef _Request As HttpRequest, ByRef _Response As HttpResponse)

			DocScript.WebParts.InitialiseWebExecutionEnvironment(_IncommingURL:=_Request.Url.AbsoluteUri)

			Try : _Request.EnsureTheseQueryStringsAreSpecified(_Response, "Action")

				Select Case _Request.QueryString("Action").ToUpper()

					Case "GetExistingExecutionSessions".ToUpper() 'http://localhost:400/API/Interactive/?Action=GetExistingExecutionSessions

						REM QueryStrings:	[]
						REM Returns:		[<ExistingExecutionSesions>]

						Try

							Dim _ExistingExecutionSessions As XElement() = DatabaseInteraction.GetExistingExecutionSessions()
							APIResponse.FromValidRequest({}, {_ExistingExecutionSessions.WrapIn("ExistingExecutionSesions")}).Send(_Response)

						Catch _Ex As Exception : Throw New DSWebException("@Action=GetExistingExecutionSessions", _Ex) : End Try

					Case "PrepareSession".ToUpper()	'http://localhost:400/API/Interactive/?Action=PrepareSession&ProgramName=HelloWorld.DS

						REM QueryStrings:	[ProgramName]
						REM Returns:		[ESID]

						Try : _Request.EnsureTheseQueryStringsAreSpecified(_Response, "ProgramName")

							Dim _ESID$ = DatabaseInteraction.CreateExecutionSession(_Request.QueryString("ProgramName"))
							APIResponse.FromValidRequest({"ESID", _ESID}).Send(_Response)

						Catch _Ex As Exception : Throw New DSWebException("@Action=PrepareSession", _Ex) : End Try

					Case "GetSessionState".ToUpper()   'http://localhost:400/API/Interactive/?Action=GetSessionState&ESID=HELLOW_2KM

						REM QueryStrings:	[ESID]
						REM Returns:		[ESState]

						Try : _Request.EnsureTheseQueryStringsAreSpecified(_Response, "ESID")

							Dim _ESState$ = DatabaseInteraction.GetExecutionSesionState(_Request.QueryString("ESID"))
							APIResponse.FromValidRequest({"ESState", _ESState$}).Send(_Response)

						Catch _Ex As Exception : Throw New DSWebException("@Action=GetSessionState", _Ex) : End Try

					Case "AwaitSessionInitiation".ToUpper()	  'http://localhost:400/API/Interactive/?Action=AwaitSessionInitiation&ESID=DSINTE_GPB

						REM QueryStrings:	[ESID]
						REM Returns:		[...ESIsRunningOrFinished], [...ESTimeStarted]

						Try : _Request.EnsureTheseQueryStringsAreSpecified(_Response, "ESID")

							'Response when the ExeSes's "State" is [Running] OR [Finished]
							WebParts.ExecutionSessionMethods.AwaitSessionInitiation(_Request, _Response)

						Catch _Ex As Exception : Throw New DSWebException("@Action=AwaitSessionInitiation", _Ex) : End Try

					Case "InitiateSession".ToUpper()   'http://localhost:400/API/Interactive/?Action=InitiateSession&ESID=SLEEP5_9AU

						REM QueryStrings:	[ESID]
						REM Returns:		

						Try : _Request.EnsureTheseQueryStringsAreSpecified(_Response, "ESID")

							REM If the Session is already Running, then this is not a valid request
							If DatabaseInteraction.GetExecutionSesionState(_Request.QueryString!ESID).ToUpper() = DBContent.ExeSes_State_Running.ToUpper() _
							 Then APIResponse.FromInvalidRequest("The Execution-Session was already running, so could not be initiated.").Send(_Response)

							With New Process()
								.StartInfo.FileName = DSIConfig.ESWorker_EXEPath
								.StartInfo.Arguments = String.Format("/ESID:""{0}""", _Request.QueryString!ESID)
								.StartInfo.UseShellExecute = True 'ShellExecute() is like typing into the Run dialog. Turning it off, uses CreateProcess() instead.
								.StartInfo.CreateNoWindow = False
								.StartInfo.WindowStyle = ProcessWindowStyle.Minimized 'Don't pop-up the CMD Window when debugging in the current session.
								.Start()
							End With

							APIResponse.FromValidRequest().Send(_Response)

						Catch _Ex As Exception : Throw New DSWebException("@Action=InitiateSession", _Ex) : End Try

					Case "ListenForESEvents".ToUpper()	 'http://localhost:400/API/Interactive/?Action=ListenForESEvents&ESID=SLEEP6_OUD&LogEventsCount=0&OutputEventsCount=0&InputEventsCount=0&CEPsCount=0

						REM QueryStrings:	[ESID], [LogEventsCount], [OutputEventsCount], [InputEventsCount], [CEPsCount]
						REM Returns:		[...ESState], [...ESTimeEnded], [...MissingLogEvents], [...MissingOutputEvents], [...MissingInputEvents], [...MissingCEPs], [...InputIsRequired], [...InputPrompt], [...InputEventID]

						Try : _Request.EnsureTheseQueryStringsAreSpecified(_Response, "ESID", "LogEventsCount", "OutputEventsCount", "InputEventsCount", "CEPsCount")

							WebParts.ExecutionSessionMethods.ListenForESEvents(_Request, _Response)

						Catch _Ex As Exception : Throw New DSWebException("@Action=ListenForESEvents", _Ex) : End Try

					Case "ProvideInputResponse".ToUpper()	'http://localhost:400/API/Interactive/?Action=ProvideInputResponse&ESID=5ITERA_0BN&InputEventID=1&InputResponse=TheNameIsBen

						REM QueryStrings:	[ESID], [InputEventID], [InputResponse]
						REM Returns:		[ResponseWasAccepted]

						Try : _Request.EnsureTheseQueryStringsAreSpecified(_Response, "ESID", "InputEventID", "InputResponse")

							Dim _InputEventID$ = _Request.QueryString!InputEventID
							Dim _ES_InputEvents As XElement() = DatabaseInteraction.GetExecutionSesionInputEvents(_Request.QueryString("ESID"))

							'Ensure there is one InputEvent with ?InputEventID
							If Not _ES_InputEvents.Where(Function(_InputEvent As XElement) _InputEvent.@ID = _InputEventID$).Count = 1 _
							 Then APIResponse.FromInvalidRequest("There was not exactly-one InputEvent with the specified InputEventID " & _InputEventID$.InBrackets()).Send(_Response)

							'If a Response has already been provided for ?InputEventID, then we won't accept it
							If _ES_InputEvents.First(Function(_InputEvent As XElement) _InputEvent.@ID = _InputEventID).@InputResponse <> DBContent.NullValue_Placeholder Then

								LogSystemMessage("An InputResponse for " & _Request.QueryString("ESID") & " was not accepted, because a Response has already been provided for InputEvent " & _InputEventID)
								APIResponse.FromValidRequest({"ResponseWasAccepted", "False"}).Send(_Response)

							Else

								'There is no response hitherto; accept the provided one
								'The Client's ?InputResponse will be "$__NULL__$" if it was an empty string on the client-side.
								'This is because sending an actual empty string would cause the Server to think that
								'No ?InputResponse had been specified with a value.
								'Therefore, we need to replace $__NULL__$ with "", using the If() call
								DatabaseInteraction.AddExecutionSessionInputEvent_ResponsePart(_Request.QueryString!ESID, _Request.QueryString!InputEventID.ExtractUInt32(), If(_Request.QueryString!InputResponse = DBContent.NullValue_Placeholder, "", _Request.QueryString!InputResponse))
								APIResponse.FromValidRequest({"ResponseWasAccepted", "True"}).Send(_Response)

							End If

						Catch _Ex As Exception : Throw New DSWebException("@Action=ProvideInputResponse", _Ex) : End Try

					Case "ListenForInputInterrupts".ToUpper()	'http://localhost:400/API/Interactive/?Action=ListenForInputInterrupts&ESID=5ITERA_0BN&InputEventID=1

						REM QueryStrings:	[ESID], [InputEventID]
						REM Returns:		[...InputHasBeenRespondedTo], [...ESHasFinished]

						REM Eventualities for this API EndPoint:
						'Input is responded-to:			InputHasBeenRespondedTo=True  ESHasFinished=False
						'ES-over, & IE responded-to:	InputHasBeenRespondedTo=True  ESHasFinished=True
						'ES has finished:				InputHasBeenRespondedTo=False ESHasFinished=True
						'InputTimeout occurs first:		(Long-Polling-Timeout)

						Try : _Request.EnsureTheseQueryStringsAreSpecified(_Response, "ESID", "InputEventID")

							WebParts.ExecutionSessionMethods.ListenForInputInterrupts(_Request, _Response)

						Catch _Ex As Exception : Throw New DSWebException("@Action=ListenForInputInterrupts", _Ex) : End Try

					Case "SubmitCEP".ToUpper()	 'http://localhost:400/API/Interactive/?Action=SubmitCEP&ESID=5ITERA_0BN

						REM QueryStrings:	[ESID]
						REM PostKeys:		[JavaScriptToRun]
						REM Returns:		

						Try : _Request.EnsureTheseQueryStringsAreSpecified(_Response, "ESID") : _Request.EnsureThesePostDataKeysAreSpecified(_Response, "JavaScriptToRun")

							DocScript.WebParts.DatabaseInteraction.AddExecutionSessionCEP(_Request.QueryString("ESID").ToUpper(), _Request.Form("JavaScriptToRun"))
							APIResponse.FromValidRequest().Send(_Response)

						Catch _Ex As Exception : Throw New DSWebException("@Action=SubmitCEP", _Ex) : End Try

					Case "ResetSessionToReady".ToUpper() 'http://localhost:400/API/Interactive/?Action=ResetSessionToReady&ESID=1RUNRI_RIW

						REM QueryStrings:	[ESID]
						REM Returns:		

						Try : _Request.EnsureTheseQueryStringsAreSpecified(_Response, "ESID")

							REM SetExecutionSessionState_Ready() will ensure that the ExeSes is currently Finished.
							REM If the ExeSes is already Ready, then the method will log a Warning, and Return.
							DocScript.WebParts.DatabaseInteraction.SetExecutionSessionState_Ready(_Request.QueryString("ESID"))
							APIResponse.FromValidRequest().Send(_Response)

						Catch _Ex As Exception : Throw New DSWebException("@Action=ResetSessionToReady", _Ex) : End Try

					Case "AbortAndDeleteSession".ToUpper() 'http://localhost:400/API/Interactive/?Action=AbortAndDeleteSession&ESID=1RUNRI_RIW

						REM QueryStrings:	[ESID]
						REM Returns:		

						Try : _Request.EnsureTheseQueryStringsAreSpecified(_Response, "ESID")

							REM Ensure that an ExeSes with _ESID exists
							Dim _ESID$ = _Request.QueryString("ESID")
							Dim _ExistantExecutionSessions As XElement() = DatabaseInteraction.GetExistingExecutionSessions()
							If Not _ExistantExecutionSessions.Where(Function(_ExeSes As XElement) _ExeSes.@ESID = _ESID).Count = 1 _
							 Then Throw New DSValidationException("There was not exactly-one ExecutionSession for the specified ESID", _ESID)

							REM If the ExeSes is currently running, then Taskkill it
							For Each _ManagementObject As System.Management.ManagementObject In (New System.Management.ManagementClass("Win32_Process")).GetInstances()
								If _ManagementObject.Item("Name").ToString().ToUpper() = (New IO.FileInfo(DSIConfig.ESWorker_EXEPath)).Name.ToUpper() Then

									REM We're now looking an a DSIExecutionSessionWorker.exe; see if it's the one for our ?ESID
									Dim _ESID_CLA_RegExMatch As System.Text.RegularExpressions.Match = System.Text.RegularExpressions.Regex.Match(_ManagementObject.Item("CommandLine").ToString(), "\/ESID:""[A-Za-z0-9._]+""")
									If _ESID_CLA_RegExMatch.Success Then

										'_ESID_CLA_RegExMatch.Value looks like: /ESID:"ENDLES_UXV"
										If _ESID_CLA_RegExMatch.Value.Split(":"c).ElementAt(1).Replace(""""c, "").ToUpper() = _ESID.ToUpper() Then

											'1) Kill the ESWorker Process
											System.Diagnostics.Process.GetProcessById(CInt(_ManagementObject.Item("ProcessId"))).Kill()

											'2) Set the ES "State" to [Finished]
											DatabaseInteraction.SetExecutionSessionState_Finished(_ESID, "The ESWorker Process was killed by an API request to delete the Execution-Session")

										End If

									Else : Throw New DSValidationException("An DSIExecutionSessionWorker.exe Process had commend-line arguments which didn't match the /ESID:""*"" Regular Expression", "CLAs: " & _ManagementObject.Item("CommandLine").ToString())
									End If

								End If
							Next

							REM If the ExeSes was [Running], it has been Killed
							REM Now, Delete the ES-Record, and its 4 ES-Events Tables from the Database
							DatabaseInteraction.DeleteExecutionSession(_ESID)

							REM If we're here, then all those operations completed successfully;
							APIResponse.FromValidRequest().Send(_Response)

						Catch _Ex As Exception : Throw New DSWebException("@Action=AbortAndDeleteSession", _Ex) : End Try

					Case "DeleteCEP".ToUpper() 'http://localhost:400/API/Get.ASPX?Item=DeleteCEP&ESID=FINITE_66G&CEPID=5

						REM QueryStrings:	[ESID], [CEPID]
						REM Returns:		

						Try : _Request.EnsureTheseQueryStringsAreSpecified(_Response, "ESID", "CEPID")

							DatabaseInteraction.DeleteExecutionSessionCEP(_Request.QueryString("ESID"), _Request.QueryString("CEPID"))
							APIResponse.FromValidRequest().Send(_Response)

						Catch _Ex As Exception : Throw New DSWebException("@Item=DeleteCEP", _Ex) : End Try

					Case Else
						REM An unmatched ?Action was specified...
						APIResponse.FromInvalidRequest("An unrecognised ?Action was specified.").Send(_Response)
				End Select

				'If no response has been sent, then the request hasn't been dealt with properly (e.g. a case didn't APIResponse...Send() anything)
				Throw New DSException("The API-EndPoint did not respond with any data")

			Catch _Ex As Exception
				APIResponse.FromInternalServerError("@ExecutionSessionASPX: " & _Ex.Message).Send(_Response)
			End Try

		End Sub

	End Class

End Namespace







REM Try these to fix the Variables-Not-Clearing problem:
'	- Session.Abandon()
'	- Session.Clear()
'	- https://social.msdn.microsoft.com/Forums/en-US/2e994549-d995-430f-a472-aa78004b5129/what-are-ways-to-make-session-invalidate-or-expire?forum=aspstatemanagement