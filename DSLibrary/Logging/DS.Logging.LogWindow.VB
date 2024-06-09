Namespace Logging

	Partial Public Class LogWindow

		Protected Shared ReadOnly EventSeverityIcons_ As New Dictionary(Of DocScript.Logging.LogEvent.DSEventSeverity, System.Drawing.Icon)() From {
		 {LogEvent.DSEventSeverity.Error, Drawing.SystemIcons.Error},
		 {LogEvent.DSEventSeverity.Warning, Drawing.SystemIcons.Warning},
		 {LogEvent.DSEventSeverity.Infomation, Drawing.SystemIcons.Information},
		 {LogEvent.DSEventSeverity.Verbose, Drawing.SystemIcons.Application},
		 {LogEvent.DSEventSeverity.Debug, Drawing.SystemIcons.Shield}
		}

		Protected Shared ReadOnly EventSeveritySounds_ As New Dictionary(Of DocScript.Logging.LogEvent.DSEventSeverity, System.Media.SystemSound)() From {
		 {LogEvent.DSEventSeverity.Error, Media.SystemSounds.Hand},
		 {LogEvent.DSEventSeverity.Warning, Media.SystemSounds.Asterisk},
		 {LogEvent.DSEventSeverity.Infomation, Media.SystemSounds.Exclamation},
		 {LogEvent.DSEventSeverity.Verbose, Media.SystemSounds.Question},
		 {LogEvent.DSEventSeverity.Debug, Media.SystemSounds.Beep}
		}

		REM Item1 = Frequency; Item2 = Duration
		Protected Shared ReadOnly EventSeverityBeepArguments_ As New Dictionary(Of DocScript.Logging.LogEvent.DSEventSeverity, Tuple(Of Int32, Int32))() From {
		  {LogEvent.DSEventSeverity.Error, New Tuple(Of Int32, Int32)(65, 1500)},
		  {LogEvent.DSEventSeverity.Warning, New Tuple(Of Int32, Int32)(131, 800)},
		  {LogEvent.DSEventSeverity.Infomation, New Tuple(Of Int32, Int32)(262, 100)},
		  {LogEvent.DSEventSeverity.Verbose, New Tuple(Of Int32, Int32)(392, 50)},
		  {LogEvent.DSEventSeverity.Debug, New Tuple(Of Int32, Int32)(523, 25)}
		 }

		Public LogEvents As New List(Of DocScript.Logging.LogEvent)()

		''' <summary>The _SessionName$ appears in the Window Title, to distinguish it from other active LogWindows</summary>
		Public Sub New(Optional ByVal _SessionName$ = "")

			REM Initialisation
			Me.InitializeComponent()
			If Not [String].IsNullOrEmpty(_SessionName) Then Me.Text &= " (" & _SessionName.Take(40).ToArray() & ")"
			Me.ShowDebugSeverityCheckBox.Checked = DocScript.Logging.ProcessDebugEvents

			REM Event Handlers
			AddHandler Me.SelectAllToolStripMenuItem.Click, Sub() Me.LogEventsDataGridView.SelectAll()
			AddHandler Me.ExitToolStripMenuItem.Click, Sub() Me.Close()
			AddHandler Me.DeleteAllToolStripMenuItem.Click, Sub() Me.LogEventsDataGridView.Rows.Clear()

		End Sub

		''' <summary>Adds the _LogEvent to the DataGridView of the Form</summary>
		Public Sub AddLogEvent(ByVal _LogEvent As DocScript.Logging.LogEvent)

			REM Unconditionally keep a copy of the Event, in case if should later be saved to disk
			Me.LogEvents.Add(_LogEvent)

			REM Only add the event if the CheckBox for its Severity is Checked
			If Not LogEventShouldBeShown_(_LogEvent) Then Return

			REM Add the LogEvent to the DataGridView:
			Me.LogEventsDataGridView.Rows.Add(Me.LogEvents.Count.ToString(), LogWindow.EventSeverityIcons_.Item(_LogEvent.Severity), _LogEvent.Timestamp.GetLogSerialisation(), _LogEvent.Severity.ToString("G"), _LogEvent.Catagory.ToString("G"), _LogEvent.Message)

			REM Update the [Count: *] Text:
			Me.EventCountLabel.Text = ("Shown: " & Me.LogEventsDataGridView.Rows.Count.ToString())

			REM Scroll to the bottom of the DataGridView:
			Me.LogEventsDataGridView.FirstDisplayedScrollingRowIndex = (Me.LogEventsDataGridView.RowCount - 1)

			REM Force the Form to raise its Win32 Paint Event:
			Me.Refresh()

			REM Play the corrosponding sound if the CheckBox herefor is Checked
			'If Me.PlaySoundCheckBox.Checked Then My.Computer.Audio.PlaySystemSound(EventSeveritySounds_.Item(_LogEvent.Severity))
			If Me.PlaySoundCheckBox.Checked Then Console.Beep(EventSeverityBeepArguments_.Item(_LogEvent.Severity).Item1, EventSeverityBeepArguments_.Item(_LogEvent.Severity).Item2)

		End Sub

		Protected ReadOnly Property LogEventShouldBeShown_(ByVal _LogEvent As LogEvent) As Boolean
			Get

				REM Get the first CheckBox whose [Text] = _LogEvent.Severity, and return whether or not it's Checked
				Return CType(Me.Controls.Find("Show" & _LogEvent.Severity.ToString("G") & "SeverityCheckBox", False).First(), System.Windows.Forms.CheckBox).Checked

			End Get
		End Property

		Private Sub Set_ProcessDebugMessages_State() Handles ShowDebugSeverityCheckBox.CheckedChanged
			DocScript.Logging.LogUtilities.ProcessDebugEvents = Me.ShowDebugSeverityCheckBox.Checked
			MsgBox("The [ProcessDebugEvents] Flag has been changed. Any Debug Events submitted henceforth will be " & If(Me.ShowDebugSeverityCheckBox.Checked, "shown.", "hidden."), MsgBoxStyle.Information, "Debug Events Show/Hide")
		End Sub

		Public Sub GenerateExampleLogEvents() Handles GenerateExampleLogEventsToolStripMenuItem.Click
			Me.AddLogEvent(New LogEvent("Example LogEvent", LogEvent.DSEventSeverity.Error, LogEvent.DSEventCatagory.Execution))
			Me.AddLogEvent(New LogEvent("Example LogEvent", LogEvent.DSEventSeverity.Warning, LogEvent.DSEventCatagory.Lexing))
			Me.AddLogEvent(New LogEvent("Example LogEvent", LogEvent.DSEventSeverity.Infomation, LogEvent.DSEventCatagory.Parsing))
			Me.AddLogEvent(New LogEvent("Example LogEvent", LogEvent.DSEventSeverity.Verbose, LogEvent.DSEventCatagory.System))
			Me.AddLogEvent(New LogEvent("Example LogEvent", LogEvent.DSEventSeverity.Debug, LogEvent.DSEventCatagory.Unspecified))
		End Sub

		Public Sub SaveLogEventsAs() Handles SaveToolStripMenuItem.Click
			Try

				Dim _FileToSaveInto As IO.FileInfo = UsefulMethods.RequestFilePath_WithSaveFileDialog("Save LogEvents as...", {New Tuple(Of String, String)("Log Files", "LOG")})

				If _FileToSaveInto IsNot Nothing Then

					My.Computer.FileSystem.WriteAllText(
					 file:=_FileToSaveInto.FullName,
					 text:=String.Join(vbCrLf, Me.LogEvents.Select(Of [String])(Function(_Event As LogEvent) String.Format("Log: [{0}]{4}[Stage: {1}]{4}[{2}]{4}{3}", _Event.Timestamp.GetLogSerialisation(), _Event.Catagory.ToString(), _Event.Severity.ToString(), _Event.Message, vbTab))),
					 append:=False
					   )

					Process.Start(_FileToSaveInto.FullName)

				End If

			Catch _Ex As Exception
				MsgBox("The LogEvents could not be saved." & vbCrLf & _Ex.Message, MsgBoxStyle.Critical)
			End Try
		End Sub

#Region "Close the form on pressing [Esc]"

		Protected Sub Register_CloseOnEscKey_Handler() Handles Me.Load
			Me.KeyPreview = True
			AddHandler Me.KeyDown, Sub(_Sender As [Object], _KeyEventArgs As Windows.Forms.KeyEventArgs) If _KeyEventArgs.KeyCode = Windows.Forms.Keys.Escape Then Me.Close()
		End Sub

		Protected Overrides Function ProcessDialogKey(keyData As System.Windows.Forms.Keys) As [Boolean]
			If (Windows.Forms.Form.ModifierKeys = Windows.Forms.Keys.None) AndAlso (keyData = Windows.Forms.Keys.Escape) Then
				Me.Close() : Return True
			End If
			Return MyBase.ProcessDialogKey(keyData)
		End Function

#End Region

	End Class

End Namespace