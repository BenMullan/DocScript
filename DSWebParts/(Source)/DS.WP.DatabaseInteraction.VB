Namespace WebParts

	Public Module DatabaseInteraction

		''' <summary>Gets the "ProgramName" and "TimeUploaded" from the UploadedPrograms Table</summary>
		Public Function GetUploadedProgramsWithoutSource() As XElement()
			Try

				Dim _SQLQueryRunner As New DocScript.Utilities.SQLQueryRunner(DSIConfig.DocScriptSQLServer)
				_SQLQueryRunner.ExecuteQuery(SQLQueries_.Select_UploadedProgramsWithoutSource)

				'Select the "ProgramName" column (field) from each row (record) returned
				Return _SQLQueryRunner.QueryOutputDataTable.Select() _
				 .Select(Of XElement)(Function(_Row As DataRow) <UploadedProgram ProgramName=<%= _Row.Item("ProgramName").ToString() %> TimeUploaded=<%= _Row.Item("TimeUploaded").ToString() %>/>) _
				 .ToArray()

			Catch _Ex As Exception : Throw New DSException("@GetUploadedProgramsWithoutSource: " & _Ex.Message, _Ex) : End Try
		End Function

		'Piggy-backs off of GetUploadedProgramsWithoutSource()
		Private ReadOnly Property UploadedPrograms_Names As String()
			Get
				Return DatabaseInteraction.GetUploadedProgramsWithoutSource() _
				 .Select(Of String)(Function(_UploadedProgram As XElement) _UploadedProgram.@ProgramName) _
				 .ToArray()
			End Get
		End Property

		'Not actually used anywhere; just exists to partner the _Names Property...
		Private ReadOnly Property UploadedPrograms_TimesUploaded As String()
			Get
				Return DatabaseInteraction.GetUploadedProgramsWithoutSource() _
				 .Select(Of String)(Function(_UploadedProgram As XElement) _UploadedProgram.@TimesUploaded) _
				 .ToArray()
			End Get
		End Property

		''' <summary>Ensures an UploadedProgram with _ProgramName exists. Gets the "Source" from the UploadedPrograms Table</summary>
		Public Function GetUploadedProgramSource(ByVal _ProgramName$) As String
			Try

				REM Ensure an UploadedProgram with _ProgramName exists
				If Not DatabaseInteraction.UploadedPrograms_Names.EachToUpper().Contains(_ProgramName.ToUpper()) _
				 Then Throw New DSValidationException("An UploadedProgram does not exist with the specified _ProgramName", "Program-Name: " & _ProgramName)

				Dim _SQLQueryRunner As New DocScript.Utilities.SQLQueryRunner(DSIConfig.DocScriptSQLServer)
				_SQLQueryRunner.ExecuteQuery(SQLQueries_.Select_UploadedProgramSource, New SqlClient.SqlParameter("TheProgramName", SqlDbType.VarChar) With {.Value = _ProgramName})

				'Select the "Source" column (field) from the First() row (record) returned
				Return _SQLQueryRunner.QueryOutputDataTable.Select().First().Item(columnName:="Source").ToString()

			Catch _Ex As Exception : Throw New DSException("@GetUploadedProgramSource: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>Handles the clensing and duplicate-name detection. Returns the ProgramSavedAsName</summary>
		Public Function AddUploadedProgram(ByVal _ProgramName$, ByVal _ProgramSource$) As [String]
			Try

				REM [_ProgramName] → [_Clensed_ProgramName] → [_Unique_ProgramName]
				Dim _Clensed_ProgramName$ = _ProgramName.ToUpper().OnlyAllowTheseChars(DBContent.StandardDBIdentifierChars).Take(DBContent.Max_ProgramNameLength).ToArray()

				REM _Clensed_ProgramName$ could be Empty
				If String.IsNullOrEmpty(_Clensed_ProgramName) Then _Clensed_ProgramName = DBContent.GenerateRandomProgramName()

				REM _Clensed_ProgramName could be equal to an existing Program Name
				Dim _Unique_ProgramName$ = _
				 DatabaseInteraction.UploadedPrograms_Names _
				 .EachToUpper().ToArray() _
				 .GenerateUniqueString(_Seed:=_Clensed_ProgramName)

				Dim _SQLQueryRunner As New DocScript.Utilities.SQLQueryRunner(DSIConfig.DocScriptSQLServer)
				_SQLQueryRunner.ExecuteQuery(
				 SQLQueries_.Insert_UploadedProgram,
				 New SqlClient.SqlParameter("TheProgramName", SqlDbType.VarChar) With {.Value = _Unique_ProgramName},
				 New SqlClient.SqlParameter("TheSource", SqlDbType.NVarChar) With {.Value = _ProgramSource}
				)

				Return _Unique_ProgramName 'That's the name the DataBase was actually told to save the Source under

			Catch _Ex As Exception : Throw New DSException("@AddUploadedProgram: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>Returns the XElements to represent the existing ESs</summary>
		Public Function GetExistingExecutionSessions() As [XElement]()
			Try

				Dim _SQLQueryRunner As New DocScript.Utilities.SQLQueryRunner(DSIConfig.DocScriptSQLServer)
				_SQLQueryRunner.ExecuteQuery(SQLQueries_.Select_AllExecutionSessions)

				Return _SQLQueryRunner.QueryOutputDataTable.Select() _
				 .Select(Of XElement)(
				   Function(_Row As DataRow) As XElement

					   'Return <ExecutionSession ESID=<%= _Row.Item("ESID").ToString() %>/>

					   Dim _XElement As New XElement("ExecutionSession")
					   For Each _Key$ In {"ESID", "ProgramName", "TimeStarted", "TimeEnded", "State", "ExitReason"}
						   _XElement.Add(New XAttribute(_Key, If(IsDBNull(_Row.Item(columnName:=_Key)), DBContent.NullValue_Placeholder$, _Row.Item(columnName:=_Key).ToString()))) : Next
					   Return _XElement

				   End Function
				).ToArray()

			Catch _Ex As Exception : Throw New DSException("@GetExistingExecutionSessions: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>Returns the ESID of the created ExeSes. Ensures that a Program Exists with the specified _ProgramName</summary>
		Public Function CreateExecutionSession(ByVal _ProgramName$) As String
			Try

				REM Ensure that a Program Exists with the specified _ProgramName
				If Not DatabaseInteraction.UploadedPrograms_Names.Where(Function(_ExistingProgramName$) _ExistingProgramName$.ToUpper() = _ProgramName.ToUpper()).Count = 1 _
					 Then Throw New DSValidationException("There was not exactly one UploadedProgram record with the specified _ProgramName", _ProgramName)

				Dim _ESID$ = _
				 DatabaseInteraction.GetExistingExecutionSessions() _
				 .Select(Of String)(Function(_ExeSes As XElement) _ExeSes.@ESID).ToArray() _
				 .GenerateUniqueString(_Seed:=(_ProgramName.Take(6).ToArray() & "_"c & UsefulMethods.GetRandomString(_WantedLength:=3)).ToUpper())	  'E.g. HELLOW_AH4

				REM Create the Record in [ExecutionSessions]
				Dim _SQLQueryRunner As New DocScript.Utilities.SQLQueryRunner(DSIConfig.DocScriptSQLServer)
				_SQLQueryRunner.ExecuteQuery(
				 SQLQueries_.Insert_NewExecutionSession,
				  New SqlClient.SqlParameter("TheESID", SqlDbType.VarChar) With {.Value = _ESID},
				  New SqlClient.SqlParameter("TheProgramName", SqlDbType.VarChar) With {.Value = _ProgramName.ToUpper()}
				)

				REM Create each of the Four tables required for the ExeSes
				_SQLQueryRunner.ExecuteQuery(
				 SQLQueries_.Create_ExecutionSession_OutputsTable(_ESID) & _
				 SQLQueries_.Create_ExecutionSession_InputsTable(_ESID) & _
				 SQLQueries_.Create_ExecutionSession_LogEventsTable(_ESID) & _
				 SQLQueries_.Create_ExecutionSession_CEPsTable(_ESID)
				)

				Return _ESID

			Catch _Ex As Exception : Throw New DSException("@CreateExecutionSession: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>Returns Ready|Running|Finished. Ensures that an ES exists with the specified _ESID</summary>
		Public Function GetExecutionSesionState(ByVal _ESID$) As String
			Try

				REM There is no CheckExecutionSessionExists(_ESID) Function, because a method such as this need only call GetExistingExecutionSessions() once, itself.
				Dim _ExistantExecutionSessions As XElement() = DatabaseInteraction.GetExistingExecutionSessions()

				REM Ensure that an ExeSes with _ESID exists
				If Not _ExistantExecutionSessions.Where(Function(_ExeSes As XElement) _ExeSes.@ESID = _ESID).Count = 1 _
				 Then Throw New DSValidationException("There was not exactly one ExecutionSession for the specified ESID", _ESID)

				REM Return the @State of the relevant ExeSes
				Return _ExistantExecutionSessions.Where(Function(_ExeSes As XElement) _ExeSes.@ESID = _ESID).First().@State

			Catch _Ex As Exception : Throw New DSException("@GetExecutionSesionState: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>Returns the Source, of the Program referenced by the ExeSes with _ESID</summary>
		Public Function GetUploadedProgramFromESID(ByVal _ESID$) As XElement
			Try

				Dim _ExistantExecutionSessions As XElement() = DatabaseInteraction.GetExistingExecutionSessions()

				REM Ensure that an ExeSes with _ESID exists
				If Not _ExistantExecutionSessions.Where(Function(_ExeSes As XElement) _ExeSes.@ESID = _ESID).Count = 1 _
				 Then Throw New DSValidationException("There was not exactly-one ExecutionSession for the specified ESID", _ESID)

				REM Return the UploadedProgram, WHERE the ProgramName = ExeSes.ProgramName
				Dim _ProgramName_OfTargetExeSes$ = _ExistantExecutionSessions.First(Function(_ExeSes As XElement) _ExeSes.@ESID = _ESID).@ProgramName
				Dim _SQLQueryRunner As New DocScript.Utilities.SQLQueryRunner(DSIConfig.DocScriptSQLServer)
				_SQLQueryRunner.ExecuteQuery(SQLQueries_.Select_UploadedProgram_WhereProgramName, New SqlClient.SqlParameter("TheProgramName", SqlDbType.VarChar) With {.Value = _ProgramName_OfTargetExeSes})
				If Not _SQLQueryRunner.QueryOutputDataTable.Rows.Count = 1 Then Throw New DSValidationException("There was not exactly-one Program with the Execution-Session's ProgramName", "ProgramName: " & _ProgramName_OfTargetExeSes)

				Return _SQLQueryRunner.QueryOutputDataTable.Select() _
				 .Select(Of XElement)(Function(_Row As DataRow) <UploadedProgram ProgramName=<%= _Row.Item(columnName:="ProgramName").ToString() %> TimeUploaded=<%= _Row.Item(columnName:="TimeUploaded").ToString() %> Source=<%= _Row.Item(columnName:="Source").ToString() %>/>) _
				 .First()

			Catch _Ex As Exception : Throw New DSException("@GetProgramFromESID: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>Automatically updates the TimeStarted</summary>
		Public Sub SetExecutionSessionState_Running(ByVal _ESID$)
			Try

				REM Ensure that an ExeSes with _ESID exists
				Dim _ExistantExecutionSessions As XElement() = DatabaseInteraction.GetExistingExecutionSessions()
				If Not _ExistantExecutionSessions.Where(Function(_ExeSes As XElement) _ExeSes.@ESID = _ESID).Count = 1 _
			  Then Throw New DSValidationException("There was not exactly-one ExecutionSession for the specified ESID", _ESID)

				REM Update the "State", and "TimeStarted"
				Dim _SQLQueryRunner As New DocScript.Utilities.SQLQueryRunner(DSIConfig.DocScriptSQLServer)
				_SQLQueryRunner.ExecuteQuery(SQLQueries_.Update_ExecutionSessionState_Running, New SqlClient.SqlParameter("TheESID", SqlDbType.VarChar) With {.Value = _ESID})

			Catch _Ex As Exception : Throw New DSException("@SetExecutionSessionState_Running: " & _Ex.Message, _Ex) : End Try
		End Sub

		''' <summary>Automatically updates the TimeEnded</summary>
		Public Sub SetExecutionSessionState_Finished(ByVal _ESID$, ByVal _ExitReason$)
			Try

				REM Ensure that an ExeSes with _ESID exists
				Dim _ExistantExecutionSessions As XElement() = DatabaseInteraction.GetExistingExecutionSessions()
				If Not _ExistantExecutionSessions.Where(Function(_ExeSes As XElement) _ExeSes.@ESID = _ESID).Count = 1 _
				 Then Throw New DSValidationException("There was not exactly-one ExecutionSession for the specified ESID", _ESID)

				REM Update the "State", and "TimeEnded" and "ExitReason"
				Dim _SQLQueryRunner As New DocScript.Utilities.SQLQueryRunner(DSIConfig.DocScriptSQLServer)
				_SQLQueryRunner.ExecuteQuery(SQLQueries_.Update_ExecutionSessionState_Finished, New SqlClient.SqlParameter("TheESID", SqlDbType.VarChar) With {.Value = _ESID}, New SqlClient.SqlParameter("TheExitReason", SqlDbType.VarChar) With {.Value = _ExitReason})

			Catch _Ex As Exception : Throw New DSException("@SetExecutionSessionState_Finished: " & _Ex.Message, _Ex) : End Try
		End Sub

		''' <summary>Ensures that the ExeSes is currently [Finished] (or already Ready)</summary>
		Public Sub SetExecutionSessionState_Ready(ByVal _ESID$)
			Try

				REM Ensure that an ExeSes with _ESID exists
				Dim _ExistantExecutionSessions As XElement() = DatabaseInteraction.GetExistingExecutionSessions()
				If Not _ExistantExecutionSessions.Where(Function(_ExeSes As XElement) _ExeSes.@ESID = _ESID).Count = 1 _
				 Then Throw New DSValidationException("There was not exactly-one ExecutionSession for the specified ESID", _ESID)

				REM If the ExeSes is already [Ready], then Return
				If _ExistantExecutionSessions.Where(Function(_ExeSes As XElement) _ExeSes.@ESID = _ESID).@State.ToUpper() = DBContent.ExeSes_State_Ready.ToUpper() Then
					LogSystemMessage("A request was made to Reset the Execution-Session (" & _ESID & ") to the [Ready] State, but it is already in this State.", LogEvent.DSEventSeverity.Warning)
					Return
				End If

				REM Ensure that the ExeSes is currently [Finished]
				If _ExistantExecutionSessions.Where(Function(_ExeSes As XElement) _ExeSes.@ESID = _ESID).@State.ToUpper() <> DBContent.ExeSes_State_Finished.ToUpper() _
					Then Throw New DSValidationException("The Execution-Session with ESID " & _ESID & " is not in the Finished State, and so cannot be Reset to Ready", "It's State was: " & _ExistantExecutionSessions.Where(Function(_ExeSes As XElement) _ExeSes.@ESID = _ESID).@State)

				REM Update the "State"
				REM The TimeStarted and TimeEnded will remain those of the previous execution of the ExeSes
				Dim _SQLQueryRunner As New DocScript.Utilities.SQLQueryRunner(DSIConfig.DocScriptSQLServer)
				_SQLQueryRunner.ExecuteQuery(SQLQueries_.Update_ExecutionSessionState_Ready, New SqlClient.SqlParameter("TheESID", SqlDbType.VarChar) With {.Value = _ESID})

			Catch _Ex As Exception : Throw New DSException("@SetExecutionSessionState_Ready: " & _Ex.Message, _Ex) : End Try
		End Sub

		''' <summary>Handles the seperation of the LogEvent's Members</summary>
		Public Sub AddExecutionSessionLogEvent(ByVal _ESID$, ByVal _LogEvent As DocScript.Logging.LogEvent)
			Try

				REM Ensure that an ExeSes with _ESID exists
				Dim _ExistantExecutionSessions As XElement() = DatabaseInteraction.GetExistingExecutionSessions()
				If Not _ExistantExecutionSessions.Where(Function(_ExeSes As XElement) _ExeSes.@ESID = _ESID).Count = 1 _
				 Then Throw New DSValidationException("There was not exactly-one ExecutionSession for the specified ESID", _ESID)

				Dim _SQLQueryRunner As New DocScript.Utilities.SQLQueryRunner(DSIConfig.DocScriptSQLServer)
				_SQLQueryRunner.ExecuteQuery(
				 SQLQueries_.Insert_ExecutionSessionLogEvent(_ESID),
				 New SqlClient.SqlParameter("TheMessage", SqlDbType.NVarChar) With {.Value = _LogEvent.Message},
				 New SqlClient.SqlParameter("TheSeverity", SqlDbType.VarChar) With {.Value = _LogEvent.Severity.ToString("G")},
				 New SqlClient.SqlParameter("TheCategory", SqlDbType.VarChar) With {.Value = _LogEvent.Catagory.ToString("G")}
				)

			Catch _Ex As Exception : Throw New DSException("@AddExecutionSessionLogEvent: " & _Ex.Message, _Ex) : End Try
		End Sub

		''' <summary>Inserts into the {ESID}_Outputs Table</summary>
		Public Sub AddExecutionSessionOutputEvent(ByVal _ESID$, ByVal _Message$)
			Try

				REM Ensure that an ExeSes with _ESID exists
				Dim _ExistantExecutionSessions As XElement() = DatabaseInteraction.GetExistingExecutionSessions()
				If Not _ExistantExecutionSessions.Where(Function(_ExeSes As XElement) _ExeSes.@ESID = _ESID).Count = 1 _
				 Then Throw New DSValidationException("There was not exactly-one ExecutionSession for the specified ESID", _ESID)

				Dim _SQLQueryRunner As New DocScript.Utilities.SQLQueryRunner(DSIConfig.DocScriptSQLServer)
				_SQLQueryRunner.ExecuteQuery(
				 SQLQueries_.Insert_ExecutionSessionOutputEvent(_ESID),
				 New SqlClient.SqlParameter("TheOutputMessage", SqlDbType.NVarChar) With {.Value = _Message}
				)

			Catch _Ex As Exception : Throw New DSException("@AddExecutionSessionOutputEvent: " & _Ex.Message, _Ex) : End Try
		End Sub

		''' <summary>Inserts into the {ESID}_CEPs Table</summary>
		Public Sub AddExecutionSessionCEP(ByVal _ESID$, ByVal _JavaScriptToRun$)
			Try

				REM Ensure that an ExeSes with _ESID exists
				Dim _ExistantExecutionSessions As XElement() = DatabaseInteraction.GetExistingExecutionSessions()
				If Not _ExistantExecutionSessions.Where(Function(_ExeSes As XElement) _ExeSes.@ESID = _ESID).Count = 1 _
				 Then Throw New DSValidationException("There was not exactly-one ExecutionSession for the specified ESID", _ESID)

				Dim _SQLQueryRunner As New DocScript.Utilities.SQLQueryRunner(DSIConfig.DocScriptSQLServer)
				_SQLQueryRunner.ExecuteQuery(
				 SQLQueries_.Insert_ExecutionSessionCEP(_ESID),
				 New SqlClient.SqlParameter("TheJavaScriptToRun", SqlDbType.NVarChar) With {.Value = _JavaScriptToRun}
				)

			Catch _Ex As Exception : Throw New DSException("@AddExecutionSessionCEP: " & _Ex.Message, _Ex) : End Try
		End Sub

		''' <summary>Inserts into the {ESID}_Inputs Table. Returns the ID of the newly-inserted InputEvent.</summary>
		Public Function AddExecutionSessionInputEvent_RequestPart(ByVal _ESID$, ByVal _Prompt$) As String
			Try

				REM Ensure that an ExeSes with _ESID exists
				Dim _ExistantExecutionSessions As XElement() = DatabaseInteraction.GetExistingExecutionSessions()
				If Not _ExistantExecutionSessions.Where(Function(_ExeSes As XElement) _ExeSes.@ESID = _ESID).Count = 1 _
				 Then Throw New DSValidationException("There was not exactly-one ExecutionSession for the specified ESID", _ESID)

				Dim _SQLQueryRunner As New DocScript.Utilities.SQLQueryRunner(DSIConfig.DocScriptSQLServer)
				_SQLQueryRunner.ExecuteQuery(
				 SQLQueries_.Insert_ExecutionSessionInputEvent_AndSelectLatestID(_ESID),
				 New SqlClient.SqlParameter("TheInputPrompt", SqlDbType.NVarChar) With {.Value = _Prompt}
				)

				Return _SQLQueryRunner.QueryOutputDataTable.Rows.Item(index:=0).Item(columnName:="ID").ToString()

			Catch _Ex As Exception : Throw New DSException("@AddExecutionSessionInputEvent_RequestPart: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>Blind UPDATE Statement; does NOT ensure that a Response hasn't already been specified. Automatically updates the Responded-to Time</summary>
		Public Sub AddExecutionSessionInputEvent_ResponsePart(ByVal _ESID$, ByVal _InputEventID As UInt32, ByVal _InputResponse$)
			Try

				REM Ensure that an ExeSes with _ESID exists
				Dim _ExistantExecutionSessions As XElement() = DatabaseInteraction.GetExistingExecutionSessions()
				If Not _ExistantExecutionSessions.Where(Function(_ExeSes As XElement) _ExeSes.@ESID = _ESID).Count = 1 _
				 Then Throw New DSValidationException("There was not exactly-one ExecutionSession for the specified ESID", _ESID)

				REM Update the "InputResponse", and "TimeStarted"
				Dim _SQLQueryRunner As New DocScript.Utilities.SQLQueryRunner(DSIConfig.DocScriptSQLServer)
				_SQLQueryRunner.ExecuteQuery(SQLQueries_.Update_ExecutionSession_InputResponse(_ESID), New SqlClient.SqlParameter("TheInputResponse", SqlDbType.NVarChar) With {.Value = _InputResponse}, New SqlClient.SqlParameter("TheInputEventID", SqlDbType.Int) With {.Value = _InputEventID.ToString()})

			Catch _Ex As Exception : Throw New DSException("@AddExecutionSessionInputEvent_ResponsePart: " & _Ex.Message, _Ex) : End Try
		End Sub

		''' <summary>Returns each &lt;OutputEvent/&gt; as an XElement with "ID", "TimeSubmitted", and "OutputMessage"</summary>
		Public Function GetExecutionSesionOutputEvents(ByVal _ESID$) As XElement()
			Try

				REM Ensure that an ExeSes with _ESID exists
				If Not DatabaseInteraction.GetExistingExecutionSessions().Where(Function(_ExeSes As XElement) _ExeSes.@ESID = _ESID).Count = 1 _
				  Then Throw New DSValidationException("There was not exactly one ExecutionSession for the specified ESID", _ESID)

				Dim _SQLQueryRunner As New DocScript.Utilities.SQLQueryRunner(DSIConfig.DocScriptSQLServer)
				_SQLQueryRunner.ExecuteQuery(SQLQueries_.Select_AllExecutionSession_TableRecords(_ESID, DBContent.TableName_ES_Outputs))

				Return _SQLQueryRunner.QueryOutputDataTable.Select() _
				 .Select(Of XElement)(
				   Function(_Row As DataRow) As XElement

					   Dim _XElement As New XElement("OutputEvent")
					   For Each _Key$ In {"ID", "TimeSubmitted", "OutputMessage"}
						   _XElement.Add(New XAttribute(_Key, If(IsDBNull(_Row.Item(columnName:=_Key)), DBContent.NullValue_Placeholder$, _Row.Item(columnName:=_Key).ToString()))) : Next
					   Return _XElement

				   End Function
				).ToArray()

			Catch _Ex As Exception : Throw New DSException("@GetExecutionSesionOutputEvents: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>Returns each &lt;InputEvent/&gt; as an XElement with "ID", "TimeSubmitted", "InputPrompt", "InputResponse", and "RespondedToTime"</summary>
		Public Function GetExecutionSesionInputEvents(ByVal _ESID$) As XElement()
			Try

				REM Ensure that an ExeSes with _ESID exists
				If Not DatabaseInteraction.GetExistingExecutionSessions().Where(Function(_ExeSes As XElement) _ExeSes.@ESID = _ESID).Count = 1 _
				  Then Throw New DSValidationException("There was not exactly one ExecutionSession for the specified ESID", _ESID)

				Dim _SQLQueryRunner As New DocScript.Utilities.SQLQueryRunner(DSIConfig.DocScriptSQLServer)
				_SQLQueryRunner.ExecuteQuery(SQLQueries_.Select_AllExecutionSession_TableRecords(_ESID, DBContent.TableName_ES_Inputs))

				Return _SQLQueryRunner.QueryOutputDataTable.Select() _
				 .Select(Of XElement)(
				   Function(_Row As DataRow) As XElement

					   Dim _XElement As New XElement("InputEvent")
					   For Each _Key$ In {"ID", "TimeSubmitted", "InputPrompt", "InputResponse", "RespondedToTime"}
						   _XElement.Add(New XAttribute(_Key, If(IsDBNull(_Row.Item(columnName:=_Key)), DBContent.NullValue_Placeholder$, _Row.Item(columnName:=_Key).ToString()))) : Next
					   Return _XElement

				   End Function
				).ToArray()

			Catch _Ex As Exception : Throw New DSException("@GetExecutionSesionInputEvents: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>Returns each &lt;LogEvent/&gt; as an XElement with "ID", "TimeSubmitted", "Message", "Severity", and "Category"</summary>
		Public Function GetExecutionSesionLogEvents(ByVal _ESID$) As XElement()
			Try

				REM Ensure that an ExeSes with _ESID exists
				If Not DatabaseInteraction.GetExistingExecutionSessions().Where(Function(_ExeSes As XElement) _ExeSes.@ESID = _ESID).Count = 1 _
				  Then Throw New DSValidationException("There was not exactly one ExecutionSession for the specified ESID", _ESID)

				Dim _SQLQueryRunner As New DocScript.Utilities.SQLQueryRunner(DSIConfig.DocScriptSQLServer)
				_SQLQueryRunner.ExecuteQuery(SQLQueries_.Select_AllExecutionSession_TableRecords(_ESID, DBContent.TableName_ES_LogEvents))

				Return _SQLQueryRunner.QueryOutputDataTable.Select() _
				 .Select(Of XElement)(
				   Function(_Row As DataRow) As XElement

					   Dim _XElement As New XElement("LogEvent")
					   For Each _Key$ In {"ID", "TimeSubmitted", "Message", "Severity", "Category"}
						   _XElement.Add(New XAttribute(_Key, If(IsDBNull(_Row.Item(columnName:=_Key)), DBContent.NullValue_Placeholder$, _Row.Item(columnName:=_Key).ToString()))) : Next
					   Return _XElement

				   End Function
				).ToArray()

			Catch _Ex As Exception : Throw New DSException("@GetExecutionSesionLogEvents: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>Returns each &lt;CEP/&gt; as an XElement with "ID", "TimeSubmitted", and "JavaScriptToRun"</summary>
		Public Function GetExecutionSesionCEPs(ByVal _ESID$) As XElement()
			Try

				REM Ensure that an ExeSes with _ESID exists
				If Not DatabaseInteraction.GetExistingExecutionSessions().Where(Function(_ExeSes As XElement) _ExeSes.@ESID = _ESID).Count = 1 _
				  Then Throw New DSValidationException("There was not exactly one ExecutionSession for the specified ESID", _ESID)

				Dim _SQLQueryRunner As New DocScript.Utilities.SQLQueryRunner(DSIConfig.DocScriptSQLServer)
				_SQLQueryRunner.ExecuteQuery(SQLQueries_.Select_AllExecutionSession_TableRecords(_ESID, DBContent.TableName_ES_CEPs))

				Return _SQLQueryRunner.QueryOutputDataTable.Select() _
				 .Select(Of XElement)(
				   Function(_Row As DataRow) As XElement

					   Dim _XElement As New XElement("CEP")
					   For Each _Key$ In {"ID", "TimeSubmitted", "JavaScriptToRun"}
						   _XElement.Add(New XAttribute(_Key, If(IsDBNull(_Row.Item(columnName:=_Key)), DBContent.NullValue_Placeholder$, _Row.Item(columnName:=_Key).ToString()))) : Next
					   Return _XElement

				   End Function
				).ToArray()

			Catch _Ex As Exception : Throw New DSException("@GetExecutionSesionCEPs: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>Clears LogEvents, Outputs, and Inputs</summary>
		Public Sub ResetExecutionSession(ByVal _ESID$)
			Try

				REM Ensure that an ExeSes with _ESID exists
				If Not DatabaseInteraction.GetExistingExecutionSessions().Where(Function(_ExeSes As XElement) _ExeSes.@ESID = _ESID).Count = 1 _
				  Then Throw New DSValidationException("There was not exactly one ExecutionSession for the specified ESID", _ESID)

				Dim _SQLQueryRunner As New DocScript.Utilities.SQLQueryRunner(DSIConfig.DocScriptSQLServer)
				_SQLQueryRunner.ExecuteQuery(
				 SQLQueries_.Delete_AllExecutionSession_TableRecords(_ESID, DBContent.TableName_ES_LogEvents) & _
				 SQLQueries_.Delete_AllExecutionSession_TableRecords(_ESID, DBContent.TableName_ES_Outputs) & _
				 SQLQueries_.Delete_AllExecutionSession_TableRecords(_ESID, DBContent.TableName_ES_Inputs)
				)

			Catch _Ex As Exception : Throw New DSException("@ResetExecutionSession: " & _Ex.Message, _Ex) : End Try
		End Sub

		''' <summary>Removes the ES's Record in the ExecutionSessions Table, and Deletes the 4 Events' Tables for the ES</summary>
		Public Sub DeleteExecutionSession(ByVal _ESID$)
			Try

				REM Ensure that an ExeSes with _ESID exists
				If Not DatabaseInteraction.GetExistingExecutionSessions().Where(Function(_ExeSes As XElement) _ExeSes.@ESID = _ESID).Count = 1 _
				  Then Throw New DSValidationException("There was not exactly-one ExecutionSession for the specified ESID", _ESID)

				Dim _SQLQueryRunner As New DocScript.Utilities.SQLQueryRunner(DSIConfig.DocScriptSQLServer)

				REM Remove the ES's Record in the ExecutionSessions Table
				_SQLQueryRunner.ExecuteQuery(SQLQueries_.Delete_ExecutionSessionRecord, New SqlClient.SqlParameter("TheESID", SqlDbType.VarChar) With {.Value = _ESID})

				REM Delete the 4 Events' Tables for the ES
				_SQLQueryRunner.ExecuteQuery(
				 SQLQueries_.Drop_ExecutionSession_Table(_ESID, DBContent.TableName_ES_Outputs) & _
				 SQLQueries_.Drop_ExecutionSession_Table(_ESID, DBContent.TableName_ES_Inputs) & _
				 SQLQueries_.Drop_ExecutionSession_Table(_ESID, DBContent.TableName_ES_LogEvents) & _
				 SQLQueries_.Drop_ExecutionSession_Table(_ESID, DBContent.TableName_ES_CEPs)
				)

			Catch _Ex As Exception : Throw New DSException("@DeleteExecutionSession: " & _Ex.Message, _Ex) : End Try
		End Sub

		''' <summary>Removes the Program's Record in the UploadedPrograms Table</summary>
		Public Sub DeleteUploadedProgram(ByVal _ProgramName$)
			Try

				REM Ensure an UploadedProgram with _ProgramName exists
				If Not DatabaseInteraction.UploadedPrograms_Names.EachToUpper().Contains(_ProgramName.ToUpper()) _
				 Then Throw New DSValidationException("An UploadedProgram does not exist with the specified _ProgramName", "Program-Name: " & _ProgramName)

				REM Remove the Program's Record in the UploadedPrograms Table
				Dim _SQLQueryRunner As New DocScript.Utilities.SQLQueryRunner(DSIConfig.DocScriptSQLServer)
				_SQLQueryRunner.ExecuteQuery(SQLQueries_.Delete_UploadedProgramRecord$, New SqlClient.SqlParameter("TheProgramName", SqlDbType.VarChar) With {.Value = _ProgramName$})

			Catch _Ex As Exception : Throw New DSException("@DeleteUploadedProgram: " & _Ex.Message, _Ex) : End Try
		End Sub

		''' <summary>Removes the CEP's Record in the {_ESID}_CEPs Table</summary>
		Public Sub DeleteExecutionSessionCEP(ByVal _ESID$, ByVal _CEPID$)
			Try

				REM Ensure that an ExeSes with _ESID exists
				If Not DatabaseInteraction.GetExistingExecutionSessions().Where(Function(_ExeSes As XElement) _ExeSes.@ESID = _ESID).Count = 1 _
				  Then Throw New DSValidationException("There was not exactly-one ExecutionSession for the specified ESID", _ESID)

				REM Remove the CEP's Record in the {_ESID}_CEPs Table
				Dim _SQLQueryRunner As New DocScript.Utilities.SQLQueryRunner(DSIConfig.DocScriptSQLServer)
				_SQLQueryRunner.ExecuteQuery(SQLQueries_.Delete_ExecutionSession_CEP(_ESID), New SqlClient.SqlParameter("TheID", SqlDbType.Int) With {.Value = _CEPID.ExtractUInt32()})

			Catch _Ex As Exception : Throw New DSException("@DeleteExecutionSessionCEP: " & _Ex.Message, _Ex) : End Try
		End Sub

#Region "DBInteraction Resources"

		Public Class DBContent

			Public Const NullValue_Placeholder$ = "$__NULL__$"

			Public Const TableName_UploadedPrograms$ = "UploadedPrograms"
			Public Const TableName_ExecutionSessions$ = "ExecutionSessions"
			Public Const TableName_ES_Outputs$ = "Outputs"
			Public Const TableName_ES_Inputs$ = "Inputs"
			Public Const TableName_ES_LogEvents$ = "LogEvents"
			Public Const TableName_ES_CEPs$ = "CEPs" 'Client-Execution-Packages

			Public Const Max_ProgramNameLength As Byte = 50	'It's actually a VARCHAR(100), but this helps to keep it all reasonable

			''' <summary>Used when clensing incomming Identifiers for the DSI-DB</summary>
			Public Shared ReadOnly StandardDBIdentifierChars As Char() = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz_.".ToCharArray()

			Public Const _
			 ExeSes_State_Ready$ = "Ready",
			 ExeSes_State_Running$ = "Running",
			 ExeSes_State_Finished$ = "Finished"

			Public Const _
			 ES_ExitReasonFlag_ExitedNormally$ = "ExitedNormally",
			 ES_ExitReasonFlag_DSExitCodeEquals$ = "DSExitCode=",
			 ES_ExitReasonFlag_InterpretationFailure$ = "InterpretationFailure",
			ES_ExitReasonFlag_B64ExceptionEquals$ = "B64Exception="

			Public Shared Function GenerateRandomProgramName() As String
				Return "DS_" & UsefulMethods.GetRandomString(_WantedLength:=7).ToUpper()
			End Function

		End Class

		Private NotInheritable Class SQLQueries_

			''' <summary>SQLParameters needed: @TheProgramName, @TheSource</summary>
			Public Shared ReadOnly Insert_UploadedProgram$ = String.Format("INSERT INTO [{0}].[dbo].[{1}] (""ProgramName"", ""Source"", ""TimeUploaded"") VALUES (@TheProgramName, @TheSource, CURRENT_TIMESTAMP);", DSIConfig.DocScriptDatabaseName, DBContent.TableName_UploadedPrograms)

			''' <summary>SQLParameters needed: @TheESID, @TheProgramName</summary>
			Public Shared ReadOnly Insert_NewExecutionSession$ = String.Format("INSERT INTO [{0}].[dbo].[{1}] (""ESID"", ""ProgramName"", ""State"") VALUES (@TheESID, @TheProgramName, '{2}');", DSIConfig.DocScriptDatabaseName, DBContent.TableName_ExecutionSessions, DBContent.ExeSes_State_Ready)

			''' <summary>SQLParameters needed: @TheMessage, @TheSeverity, @TheCategory</summary>
			Public Shared ReadOnly Property Insert_ExecutionSessionLogEvent$(ByVal _ESID$)
				Get
					Return Glue("INSERT INTO [", DSIConfig.DocScriptDatabaseName, "].[dbo].[", _ESID, "_", DBContent.TableName_ES_LogEvents, "] (""TimeSubmitted"", ""Message"", ""Severity"", ""Category"") VALUES (CURRENT_TIMESTAMP, @TheMessage, @TheSeverity, @TheCategory);")
				End Get
			End Property

			''' <summary>SQLParameters needed: @TheOutputMessage</summary>
			Public Shared ReadOnly Property Insert_ExecutionSessionOutputEvent$(ByVal _ESID$)
				Get
					Return Glue("INSERT INTO [", DSIConfig.DocScriptDatabaseName, "].[dbo].[", _ESID, "_", DBContent.TableName_ES_Outputs, "] (""TimeSubmitted"", ""OutputMessage"") VALUES (CURRENT_TIMESTAMP, @TheOutputMessage);")
				End Get
			End Property

			''' <summary>SQLParameters needed: @TheJavaScriptToRun</summary>
			Public Shared ReadOnly Property Insert_ExecutionSessionCEP$(ByVal _ESID$)
				Get
					Return Glue("INSERT INTO [", DSIConfig.DocScriptDatabaseName, "].[dbo].[", _ESID, "_", DBContent.TableName_ES_CEPs, "] (""TimeSubmitted"", ""JavaScriptToRun"") VALUES (CURRENT_TIMESTAMP, @TheJavaScriptToRun);")
				End Get
			End Property

			''' <summary>SQLParameters needed: @TheInputPrompt</summary>
			Public Shared ReadOnly Property Insert_ExecutionSessionInputEvent_AndSelectLatestID$(ByVal _ESID$)
				Get
					Return Glue("INSERT INTO [", DSIConfig.DocScriptDatabaseName, "].[dbo].[", _ESID, "_", DBContent.TableName_ES_Inputs, "] (""TimeSubmitted"", ""InputPrompt"") VALUES (CURRENT_TIMESTAMP, @TheInputPrompt); SELECT SCOPE_IDENTITY() AS ""ID"";")
				End Get
			End Property

			''' <summary>SQLParameters needed: @TheInputResponse, @TheInputEventID</summary>
			Public Shared ReadOnly Property Update_ExecutionSession_InputResponse$(ByVal _ESID$)
				Get
					Return ("UPDATE [" & DSIConfig.DocScriptDatabaseName & "].[dbo].[" & _ESID & "_"c & DBContent.TableName_ES_Inputs & "] SET ""InputResponse"" = @TheInputResponse, ""RespondedToTime"" = CURRENT_TIMESTAMP WHERE ""ID"" = @TheInputEventID;")
				End Get
			End Property

			''' <summary>SQLParameters needed: </summary>
			Public Shared ReadOnly Property Create_ExecutionSession_OutputsTable$(ByVal _ESID$)
				Get
					Return String.Format("CREATE TABLE [{0}].[dbo].[{1}_{2}] ([ID] [int] IDENTITY(1,1) NOT NULL, [TimeSubmitted] [datetime] NOT NULL, [OutputMessage] [nvarchar](max) NOT NULL);", DSIConfig.DocScriptDatabaseName, _ESID, DBContent.TableName_ES_Outputs)
				End Get
			End Property

			''' <summary>SQLParameters needed: </summary>
			Public Shared ReadOnly Property Create_ExecutionSession_InputsTable$(ByVal _ESID$)
				Get
					Return String.Format("CREATE TABLE [{0}].[dbo].[{1}_{2}] ([ID] [int] IDENTITY(1,1) NOT NULL, [TimeSubmitted] [datetime] NOT NULL, [InputPrompt] [nvarchar](max) NOT NULL, [InputResponse] [nvarchar](max) NULL, [RespondedToTime] [datetime] NULL);", DSIConfig.DocScriptDatabaseName, _ESID, DBContent.TableName_ES_Inputs)
				End Get
			End Property

			''' <summary>SQLParameters needed: </summary>
			Public Shared ReadOnly Property Create_ExecutionSession_LogEventsTable$(ByVal _ESID$)
				Get
					Return String.Format("CREATE TABLE [{0}].[dbo].[{1}_{2}] ([ID] [int] IDENTITY(1,1) NOT NULL, [TimeSubmitted] [datetime] NOT NULL, [Message] [nvarchar](max) NOT NULL, [Severity] [varchar](100) NOT NULL, [Category] [varchar](100) NOT NULL);", DSIConfig.DocScriptDatabaseName, _ESID, DBContent.TableName_ES_LogEvents)
				End Get
			End Property

			''' <summary>SQLParameters needed: </summary>
			Public Shared ReadOnly Property Create_ExecutionSession_CEPsTable$(ByVal _ESID$)
				Get
					Return String.Format("CREATE TABLE [{0}].[dbo].[{1}_{2}] ([ID] [int] IDENTITY(1,1) NOT NULL, [TimeSubmitted] [datetime] NOT NULL, [JavaScriptToRun] [nvarchar](max) NOT NULL);", DSIConfig.DocScriptDatabaseName, _ESID, DBContent.TableName_ES_CEPs)
				End Get
			End Property

			''' <summary>SQLParameters needed: </summary>
			Public Shared ReadOnly Property Select_AllExecutionSession_TableRecords$(ByVal _ESID$, ByVal _EventsTableName$)
				Get
					Return String.Format("SELECT * FROM [{0}].[dbo].[{1}_{2}];", DSIConfig.DocScriptDatabaseName, _ESID, _EventsTableName$)
				End Get
			End Property

			''' <summary>SQLParameters needed: </summary>
			Public Shared ReadOnly Property Delete_AllExecutionSession_TableRecords$(ByVal _ESID$, ByVal _EventsTableName$)
				Get
					Return String.Format("DELETE FROM [{0}].[dbo].[{1}_{2}];", DSIConfig.DocScriptDatabaseName, _ESID, _EventsTableName$)
				End Get
			End Property

			''' <summary>SQLParameters needed: @TheID</summary>
			Public Shared ReadOnly Property Delete_ExecutionSession_CEP$(ByVal _ESID$)
				Get
					Return String.Format("DELETE FROM [{0}].[dbo].[{1}_{2}] WHERE ""ID"" = @TheID;", DSIConfig.DocScriptDatabaseName, _ESID, DBContent.TableName_ES_CEPs)
				End Get
			End Property

			''' <summary>SQLParameters needed: @TheESID</summary>
			Public Shared ReadOnly Delete_ExecutionSessionRecord$ = _
			 String.Format("DELETE FROM [{0}].[dbo].[{1}] WHERE ""ESID"" = @TheESID;", DSIConfig.DocScriptDatabaseName, DBContent.TableName_ExecutionSessions)

			''' <summary>SQLParameters needed: @TheProgramName</summary>
			Public Shared ReadOnly Delete_UploadedProgramRecord$ = _
			  String.Format("DELETE FROM [{0}].[dbo].[{1}] WHERE ""ProgramName"" = @TheProgramName;", DSIConfig.DocScriptDatabaseName, DBContent.TableName_UploadedPrograms)

			''' <summary>SQLParameters needed: </summary>
			Public Shared ReadOnly Property Drop_ExecutionSession_Table$(ByVal _ESID$, ByVal _EventsTableName$)
				Get
					Return String.Format("DROP TABLE [{0}].[dbo].[{1}_{2}];", DSIConfig.DocScriptDatabaseName, _ESID, _EventsTableName$)
				End Get
			End Property

			''' <summary>SQLParameters needed: </summary>
			Public Shared ReadOnly Select_UploadedProgramsWithoutSource$ = _
			  String.Format("SELECT ""ProgramName"", ""TimeUploaded"" FROM [{0}].[dbo].[{1}];", DSIConfig.DocScriptDatabaseName, DBContent.TableName_UploadedPrograms)

			''' <summary>SQLParameters needed: @TheProgramName</summary>
			Public Shared ReadOnly Select_UploadedProgramSource$ = _
			 String.Format("SELECT ""Source"" FROM [{0}].[dbo].[{1}] WHERE ""ProgramName"" = @TheProgramName;", DSIConfig.DocScriptDatabaseName, DBContent.TableName_UploadedPrograms)

			''' <summary>SQLParameters needed: </summary>
			Public Shared ReadOnly Select_AllExecutionSessions$ = _
			 String.Format("SELECT * FROM [{0}].[dbo].[{1}];", DSIConfig.DocScriptDatabaseName, DBContent.TableName_ExecutionSessions)

			''' <summary>SQLParameters needed: </summary>
			Public Shared ReadOnly Select_UploadedProgram_WhereProgramName$ = _
			 Glue("SELECT * FROM [", DSIConfig.DocScriptDatabaseName, "].[dbo].[", DBContent.TableName_UploadedPrograms, "] WHERE ""ProgramName"" = @TheProgramName;")

			''' <summary>SQLParameters needed: @TheESID</summary>
			Public Shared ReadOnly Update_ExecutionSessionState_Running$ = _
			 ("UPDATE [" & DSIConfig.DocScriptDatabaseName & "].[dbo].[" & DBContent.TableName_ExecutionSessions & "] SET ""State"" = '" & DBContent.ExeSes_State_Running & "', ""TimeStarted"" = CURRENT_TIMESTAMP WHERE ""ESID"" = @TheESID;")

			''' <summary>SQLParameters needed: @TheESID, @TheExitReason</summary>
			Public Shared ReadOnly Update_ExecutionSessionState_Finished$ = _
			 ("UPDATE [" & DSIConfig.DocScriptDatabaseName & "].[dbo].[" & DBContent.TableName_ExecutionSessions & "] SET ""State"" = '" & DBContent.ExeSes_State_Finished & "', ""TimeEnded"" = CURRENT_TIMESTAMP, ""ExitReason"" = @TheExitReason WHERE ""ESID"" = @TheESID;")

			''' <summary>SQLParameters needed: @TheESID</summary>
			Public Shared ReadOnly Update_ExecutionSessionState_Ready$ = _
			 ("UPDATE [" & DSIConfig.DocScriptDatabaseName & "].[dbo].[" & DBContent.TableName_ExecutionSessions & "] SET ""State"" = '" & DBContent.ExeSes_State_Ready & "' WHERE ""ESID"" = @TheESID;")

		End Class

#End Region

	End Module

End Namespace