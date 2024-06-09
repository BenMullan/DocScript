Namespace WebParts

	Public Module Resources

		Public ReadOnly LongPollingRequests_MaxLifetime As TimeSpan = TimeSpan.FromSeconds(60)						'1 Min		60s
		Public ReadOnly LongPollingRequests_IntervalCheckingDelay As TimeSpan = TimeSpan.FromMilliseconds(100)		'1 Second	1000ms
		Public ReadOnly ESWorker_InputRequest_MaxLifetime As TimeSpan = TimeSpan.FromSeconds(480)                   '8 Mins		480s

		Public Const ESParticipant_Role_Observer$ = "OBSERVER"
		Public Const ESParticipant_Role_Responder$ = "RESPONDER"
		Public Const ESParticipant_Role_Controller$ = "CONTROLLER"
		Public ReadOnly ESParticipant_ValidRoles$() = {ESParticipant_Role_Observer, ESParticipant_Role_Responder, ESParticipant_Role_Controller}

		''' <summary>Prepares the LogEventHandler</summary>
		Public Sub InitialiseWebExecutionEnvironment(ByVal _IncommingURL$)
			Try

				Global.DocScript.Logging.ProcessDebugEvents = False
				Global.DocScript.Logging.CurrentLogEventHandler = DocScript.Logging.BuiltInLogEventHandlers.WindowsEventLog("Interactive")
				Global.DocScript.Logging.LogSystemMessage("Initialised Web Execution-Environment for incomming URL: " & _IncommingURL.InSquares(), LogEvent.DSEventSeverity.Verbose)

			Catch _Ex As Exception : Throw New DSException("@InitialiseWebExecutionEnvironment: " & _Ex.Message, _Ex) : End Try
		End Sub

	End Module

End Namespace