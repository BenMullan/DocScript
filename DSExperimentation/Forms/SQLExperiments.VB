Public Class SQLExperiments

	Private Sub ExecQueryButton_Click() Handles ExecQueryButton.Click
		Try

			'Log via the Default LogWindow
			DocScript.Logging.LogUtilities.CurrentLogEventHandler = DocScript.Logging.BuiltInLogEventHandlers.CLIDefault

			'Instanciate the SQL Runner
			Dim _SQLQueryRunner As New DocScript.Utilities.SQLQueryRunner(Me.SQLServerTextBox.Text)
			_SQLQueryRunner.ExecuteQuery(Me.SQLQueryTextBox.Text,
			  New SqlClient.SqlParameter() With {.ParameterName = "TestParam", .Value = "TestParamValue", .DbType = DbType.String}
			)

			'Copy the SQL Query output to the DataGridView on the Form
			Me.SQLOutput_DataGridView.DataSource = _SQLQueryRunner.QueryOutputDataTable

		Catch _Ex As Exception When True : MsgBox(_Ex.Message, MsgBoxStyle.Critical) : End Try
	End Sub

	Private Sub QueryWatchButton_Click() Handles QueryWatchButton.Click
		Try
			'Log via the Default LogWindow
			DocScript.Logging.LogUtilities.CurrentLogEventHandler = DocScript.Logging.BuiltInLogEventHandlers.CLIDefault

			Console.WriteLine("Console is working if you can read this!")
			DocScript.Logging.LogSystemMessage("DocScript CLI Logging is working if you can read this!")

			'Instanciate the SQL Runner
			Dim _SQLQueryRunner As New DocScript.Utilities.SQLQueryRunner(_SQLServerName:=Me.SQLServerTextBox.Text)

			'Begin the QueryWatch
			_SQLQueryRunner.BeginQueryWatch(_DatabaseName:=Me.QueryWatchDBTextBox.Text, _Query:=Me.SQLQueryTextBox.Text)

			'Add an Event Handler for a changed Query result
			AddHandler _SQLQueryRunner.QueryWatch_QueryOutputChanged, Sub(_EventArgs As SqlClient.SqlNotificationEventArgs) Beep()

		Catch _Ex As Exception When True : MsgBox(_Ex.Message, MsgBoxStyle.Critical) : End Try
	End Sub

	Private Sub ExecTenButton_Click() Handles ExecTenButton.Click

		'Log via the Default LogWindow
		DocScript.Logging.LogUtilities.CurrentLogEventHandler = DocScript.Logging.BuiltInLogEventHandlers.CLIDefault

		'Instanciate the SQL Runner
		Dim _SQLQueryRunner As New DocScript.Utilities.SQLQueryRunner(Me.SQLServerTextBox.Text)

		'Run the Query 10 times
		For _Index% = 0 To 10 Step +1
			_SQLQueryRunner.ExecuteQuery(Me.SQLQueryTextBox.Text)
			DocScript.Logging.LogSystemMessage("Executed iteration " & _Index.ToString("X"))
		Next

		'Copy the SQL Query output to the DataGridView on the Form
		Me.SQLOutput_DataGridView.DataSource = _SQLQueryRunner.QueryOutputDataTable

	End Sub

	Private Sub TBFEButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBFEButton.Click
		MsgBox(String.Join(", ", {1, 2, 3, 4, 5, 6, 7, 8, 9}.TakeBlockFromEnd(10)).InCurlies())
	End Sub
End Class