Namespace Logging

	''' <summary>Represents an Event which has occoured and is to be logged. An Object of this Class is passed to SubmitLogEvent() when it needs to be logged.</summary>
	Public Class LogEvent

		''' <summary>Error = The program could not continue. Warning = The program can continue partially. Infomation = Just an FYI. Verbose = Lots of data regarding the program's component-level processes. Debug = Annoyingly-frequent messages concerning each iteration of even the most insignificant loops. Debug messages are only processed when the Boolean Flag [ProcessDebugEvents] is True</summary>
		Public Enum DSEventSeverity As Byte
			Debug = 5
			Verbose = 4
			Infomation = 3
			Warning = 2
			[Error] = 1
		End Enum

		''' <summary>Represents the Catagory, or stage in the Interpretation Process (Parsing, Lexing, or Execution) pertaining to which a LogEvent was generated.</summary>
		Public Enum DSEventCatagory As Byte
			Unspecified = 0
			Parsing = 1
			Lexing = 2
			Execution = 3
			System = 4
		End Enum

		Public ReadOnly Message As String
		Public ReadOnly Timestamp As DateTime
		Public ReadOnly Severity As DSEventSeverity
		Public ReadOnly Catagory As DSEventCatagory

		Public Sub New(ByVal _Message$, ByVal _Severity As DSEventSeverity, ByVal _Catagory As DSEventCatagory)
			Me.Message = _Message : Me.Timestamp = DateTime.UtcNow : Me.Severity = _Severity : Me.Catagory = _Catagory
		End Sub

		''' <summary>Returns a KVPSerialised String to represent the Data of the current Object</summary>
		Public Overrides Function ToString() As String
			Return DocScript.Utilities.KVPSerialisation.GetKVPString({"Message", Me.Message}, {"Timestamp", Me.Timestamp.GetLogSerialisation()}, {"Severity", Me.Severity.ToString()}, {"Catagory", Me.Catagory.ToString()})
		End Function

	End Class

	Public Module LogUtilities

		''' <summary>Points to a Method to run when a LogEvent is Submitted. Can be set to one of the BuiltInLogEventHandlers. Default: SilenceAll</summary>
		Public CurrentLogEventHandler As LogEventHandler = BuiltInLogEventHandlers.SilenceAll

		''' <summary>The template for a Method to process a submitted LogEvent</summary>
		Public Delegate Sub LogEventHandler(ByVal _Event As DocScript.Logging.LogEvent)

		''' <summary>LogEvents where [Severity=Debug] are only processed when this flag is set to [True]. This is to improve performance, as there are a very large number of Debug Messages, and they are submitted at an annoyingly-high frequency.</summary>
		Public ProcessDebugEvents As Boolean = False

		''' <summary>Invokes the CurrentLogEventHandler with the specified Data for the LogEvent. Also checks that the CurrentLogEventHandler has been initialised with a Delegate.</summary>
		Public Function SubmitLogEvent(ByVal _Message$, ByVal _Severity As LogEvent.DSEventSeverity, ByVal _Catagory As LogEvent.DSEventCatagory) As LogEventSubmissionResult

			REM LogEvents where [Severity=Debug] are only processed when this flag is set to [True]. This is to improve performance, as there are a very large number of Debug Messages, and they are submitted at an annoyingly-high frequency.
			If (Not ProcessDebugEvents) AndAlso (_Severity = LogEvent.DSEventSeverity.Debug) Then Return (New LogEventSubmissionResult(_Success:=False, _GeneratedException:=(New Exception("The Event had Severity [Debug], and the [ProcessDebugEvents] flag was set to False."))))

			REM We can't throw a DSNullException if there isn't a CurrentLogEventHandler, because the instanciation of this Exception Type will attempt to Log its message, which it can't do because there isn't an Initialised Logger Delegate, which it will attempt to Log, but can't ... etc recursively forever and ever...
			If (Global.DocScript.Logging.LogUtilities.CurrentLogEventHandler = Nothing) Then Throw (New System.NullReferenceException("A Message could not be logged because the [CurrentLogEventHandler] has not been initialised with a value. The _Message which should have been logged is: " & _Message.InSquares()))

			REM Now we know that there is a LoggerDelegate specified, attempt to Invoke() it:
			Try
				Global.DocScript.Logging.LogUtilities.CurrentLogEventHandler.Invoke(New LogEvent(_Message, _Severity, _Catagory))
				Return (New LogEventSubmissionResult(_Success:=True))
			Catch _Ex As Exception
				Return (New LogEventSubmissionResult(_Success:=False, _GeneratedException:=_Ex))
			End Try

		End Function

#Region "QuickLog Methods"

		''' <summary>This is a QuickLog Method</summary>
		Public Sub LogDebugMessage(ByVal _Message$, ByVal _Catagory As LogEvent.DSEventCatagory)
			SubmitLogEvent(_Message, LogEvent.DSEventSeverity.Debug, _Catagory)
		End Sub

		''' <summary>This is a QuickLog Method</summary>
		Public Sub LogParsingMessage(ByVal _Message$, Optional ByVal _Severity As LogEvent.DSEventSeverity = LogEvent.DSEventSeverity.Verbose)
			SubmitLogEvent(_Message, _Severity, LogEvent.DSEventCatagory.Parsing)
		End Sub

		''' <summary>This is a QuickLog Method</summary>
		Public Sub LogLexingMessage(ByVal _Message$, Optional ByVal _Severity As LogEvent.DSEventSeverity = LogEvent.DSEventSeverity.Verbose)
			SubmitLogEvent(_Message, _Severity, LogEvent.DSEventCatagory.Lexing)
		End Sub

		''' <summary>This is a QuickLog Method</summary>
		Public Sub LogExecutionMessage(ByVal _Message$, Optional ByVal _Severity As LogEvent.DSEventSeverity = LogEvent.DSEventSeverity.Verbose)
			SubmitLogEvent(_Message, _Severity, LogEvent.DSEventCatagory.Execution)
		End Sub

		''' <summary>This is a QuickLog Method</summary>
		Public Sub LogSystemMessage(ByVal _Message$, Optional ByVal _Severity As LogEvent.DSEventSeverity = LogEvent.DSEventSeverity.Verbose)
			SubmitLogEvent(_Message, _Severity, LogEvent.DSEventCatagory.System)
		End Sub

#End Region

		''' <summary>Contains a series of pre-defined ways of handling a LogEvent</summary>
		Public NotInheritable Class BuiltInLogEventHandlers

			REM Make this Class uninstanciatable from outside
			Private Sub New()
			End Sub

#Region "CLI LogEvent Handler"

			Private Shared ReadOnly EventSeverityConsoleColours_ As New Dictionary(Of LogEvent.DSEventSeverity, ConsoleColor)() From {
			 {LogEvent.DSEventSeverity.Debug, ConsoleColor.Cyan},
			 {LogEvent.DSEventSeverity.Verbose, ConsoleColor.DarkCyan},
			 {LogEvent.DSEventSeverity.Infomation, ConsoleColor.Yellow},
			 {LogEvent.DSEventSeverity.Warning, ConsoleColor.DarkYellow},
			 {LogEvent.DSEventSeverity.Error, ConsoleColor.DarkRed}
			}

			''' <summary>Outputs the LogEvents to the Console in different Colours depending on the Event's Severity</summary>
			Public Shared ReadOnly CLIDefault As New LogEventHandler(
			 Sub(_Event As LogEvent)

				 Dim _PreviousConsoleColour_Foreground As ConsoleColor = Console.ForegroundColor
				 Dim _PreviousConsoleColour_Background As ConsoleColor = Console.BackgroundColor

				 Console.ForegroundColor = EventSeverityConsoleColours_.Item(_Event.Severity)
				 Console.BackgroundColor = ConsoleColor.Gray

				 Console.WriteLine(String.Format("Log: [{0}] [Stage: {1}] [{2}] {3}", _Event.Timestamp.GetLogSerialisation(), _Event.Catagory.ToString(), _Event.Severity.ToString(), _Event.Message))

				 Console.ForegroundColor = _PreviousConsoleColour_Foreground
				 Console.BackgroundColor = _PreviousConsoleColour_Background

				 Return
			 End Sub
			)

#End Region

#Region "GUI LogEvent Handler"

			Private Shared LogWindow_ As Logging.LogWindow = Nothing

			''' <summary>Outputs the LogEvents to a Built-in LogWindow, which is run in a seperate Thread</summary>
			Public Shared ReadOnly GUIDefault As New LogEventHandler(
			  Sub(_Event As LogEvent)

				  REM If this is the first time a Log call is being made, Initialise the LogWindow in a new Thread:
				  If (LogWindow_ Is Nothing) Then
					  LogWindow_ = New LogWindow()
				  End If

				  LogWindow_.AddLogEvent(_Event)
				  LogWindow_.Show()

			  End Sub
			)

#End Region

#Region "MsgBox LogEvent Handler"

			''' <summary>Outputs each LogEvent to a MsgBox</summary>
			Public Shared ReadOnly MsgBoxPerEvent As New LogEventHandler(
			 Sub(_Event As LogEvent)
				 MsgBox("LogEvent:" & vbCrLf & vbCrLf & _Event.ToString(), MsgBoxStyle.Information, "DocScript Log")
			 End Sub
			)

#End Region

#Region "TextFile LogEvent Handler"

			''' <summary>Outputs each LogEvent to the _LogFile</summary>
			Public Shared ReadOnly Property TextFile(ByVal _LogFile As IO.FileInfo) As LogEventHandler
				Get
					Return New LogEventHandler(
					 Sub(_Event As LogEvent)

						 If Not _LogFile.Exists Then _LogFile.Create()

						 My.Computer.FileSystem.WriteAllText(
						 file:=_LogFile.FullName,
						 text:=String.Format(vbCrLf & "Log: [{0}]{4}[Stage: {1}]{4}[{2}]{4}{3}", _Event.Timestamp.GetLogSerialisation(), _Event.Catagory.ToString(), _Event.Severity.ToString(), _Event.Message, vbTab),
						 append:=True
						)

					 End Sub
					)

				End Get
			End Property

#End Region

#Region "WindowsEventLog LogEvent Handler"

			''' <summary>Writes each LogEvent to the Windows-Event-Log</summary>
			Public Shared ReadOnly Property WindowsEventLog(ByVal _Log_SubName$) As LogEventHandler
				Get
					Return (New LogEventHandler(
					 Sub(_Event As LogEvent)

						 Dim _DocScript_EventSource_And_LogName$ = "DS_" & _Log_SubName

						 If Not EventLog.SourceExists(source:=_DocScript_EventSource_And_LogName$) _
						   Then EventLog.CreateEventSource(New EventSourceCreationData(source:=_DocScript_EventSource_And_LogName$, logName:=_DocScript_EventSource_And_LogName$))

						 Dim _WindowsEventLog As New System.Diagnostics.EventLog(logName:=_DocScript_EventSource_And_LogName$, machineName:=".", source:=_DocScript_EventSource_And_LogName$)

						 _WindowsEventLog.WriteEntry(
						  message:=String.Format(vbCrLf & "Log: [{0}]{4}[Stage: {1}]{4}[{2}]{4}{3}", _Event.Timestamp.GetLogSerialisation(), _Event.Catagory.ToString(), _Event.Severity.ToString(), _Event.Message, vbTab),
						  type:=If(_Event.Severity = LogEvent.DSEventSeverity.Error, EventLogEntryType.Error, If(_Event.Severity = LogEvent.DSEventSeverity.Warning, EventLogEntryType.Warning, EventLogEntryType.Information)),
						  eventID:=1,
						  category:=_Event.Catagory
						)

						 _WindowsEventLog.Dispose()

					 End Sub
					)).MustNotBeNothing("The WindowsEventLog LogEventHandler could not be created, and returned Nothing")

				End Get
			End Property

#End Region

#Region "SilenceAll LogEvent Handler"

			''' <summary>Does nothing with each LogEvent</summary>
			Public Shared ReadOnly SilenceAll As New LogEventHandler(
			 Sub(_Event As LogEvent)
			 End Sub
			)

#End Region

		End Class

		''' <summary>Holds Data concerning the success of submitting a LogEvent</summary>
		Public Structure LogEventSubmissionResult

			Public ReadOnly WasSuccessfull As Boolean
			Public GeneratedException As System.Exception

			Public Sub New(ByVal _Success As Boolean, Optional ByVal _GeneratedException As Exception = Nothing)
				Me.WasSuccessfull = _Success : Me.GeneratedException = _GeneratedException
			End Sub

		End Structure

	End Module

End Namespace