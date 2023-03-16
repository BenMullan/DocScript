Namespace Utilities

	''' <summary>Runs a Query on an SQL Server, returning the DataTable resulting from the Query</summary>
	Public Class SQLQueryRunner

		Public Enum SQLServerAuthenticationType As [Byte]
			WindowsAuthentication = 0
			SQLServerAuthentication = 1
		End Enum

		REM Misc. Constants
		Public Const OutputTableName$ = "SQLQueryRunner_Output"

		REM ConnectionString Constants
		Public Const ConStr_ConnectionTimeout$ = "15" 'Default:	15
		Public Const ConStr_ConnectionLifetime$ = "0" 'Default:	0
		Public Const ConStr_MinPoolSize$ = "0" 'Default:		0
		Public Const ConStr_MaxPoolSize$ = "100" 'Default:		100
		Public Const ConStr_Pooling$ = "true" 'Default:			true

		REM Per-instance Server & Database details
		Public ReadOnly SQLServerAuthenticationMode As SQLQueryRunner.SQLServerAuthenticationType
		Public ReadOnly SQLServerName$
		Public ReadOnly SQLServerUserName$
		Public ReadOnly SQLServerUserPassword$

		Protected QueryOutputDataTable_ As DataTable = Nothing
		''' <summary>The Table filled with the resultant output of the previous SQL Query. If no queries have been made yet, then it is Nothing/null.</summary>
		Public ReadOnly Property QueryOutputDataTable As Global.System.Data.DataTable
			Get
				Return Me.QueryOutputDataTable_
			End Get
		End Property

		Public Event QueryWatch_QueryOutputChanged As Action(Of SqlClient.SqlNotificationEventArgs)

		''' <summary>CONSTRUCTOR FOR SQL SERVER AUTHENTICATION. Readies the Object with the specified arguments, but does not initiate any form of connection with the SQL Server at this point.</summary>
		Public Sub New(ByVal _SQLServerName$, ByVal _SQLServerUserName$, ByVal _SQLServerUserPassword$)

			Me.SQLServerAuthenticationMode = SQLServerAuthenticationType.SQLServerAuthentication

			Me.SQLServerName = _SQLServerName
			Me.SQLServerUserName = _SQLServerUserName
			Me.SQLServerUserPassword = _SQLServerUserPassword

		End Sub

		''' <summary>CONSTRUCTOR FOR WINDOWS AUTHENTICATION. Readies the Object with the specified arguments, but does not initiate any form of connection with the SQL Server at this point.</summary>
		Public Sub New(ByVal _SQLServerName$)

			Me.SQLServerAuthenticationMode = SQLServerAuthenticationType.WindowsAuthentication

			Me.SQLServerName = _SQLServerName

		End Sub

		''' <summary>Constructs the ConnectionString based on the current values of Fields in the Class. E.g. there are different ConnectionString Formats for Windows Authentication vs SQL Server Authentication.</summary>
		Public ReadOnly Property ConnectionString As String
			Get

				Select Case Me.SQLServerAuthenticationMode

					Case SQLServerAuthenticationType.SQLServerAuthentication : Return String.Format(
					 "Server={0};User Id={1};Password={2};Connection Timeout={3};Connection Lifetime={4};Min Pool Size={5};Max Pool Size={6};Pooling={7};",
					 Me.SQLServerName, Me.SQLServerUserName, Me.SQLServerUserPassword, SQLQueryRunner.ConStr_ConnectionTimeout, SQLQueryRunner.ConStr_ConnectionLifetime, SQLQueryRunner.ConStr_MinPoolSize, SQLQueryRunner.ConStr_MaxPoolSize, SQLQueryRunner.ConStr_Pooling)

					Case SQLServerAuthenticationType.WindowsAuthentication : Return String.Format(
					 "Server={0};Integrated Security=True;Connection Timeout={1};Connection Lifetime={2};Min Pool Size={3};Max Pool Size={4};Pooling={5};",
					 Me.SQLServerName, SQLQueryRunner.ConStr_ConnectionTimeout, SQLQueryRunner.ConStr_ConnectionLifetime, SQLQueryRunner.ConStr_MinPoolSize, SQLQueryRunner.ConStr_MaxPoolSize, SQLQueryRunner.ConStr_Pooling)

					Case Else : Throw New DSValidationException("The SQLServerAuthenticationMode was not accounted-for", Me.SQLServerAuthenticationMode.ToString())
				End Select

			End Get
		End Property

		''' <summary>
		''' Connects to the SQL Server, executes the T-SQL _Query, and returns the resultant DataTable.
		''' Example Call:
		''' _SQLQueryRunner.ExecuteQuery("SELECT * FROM [TestingDB].[dbo].[People] WHERE ""Name"" = @TargetName;", New SqlClient.SqlParameter() With {.ParameterName = "TargetName", .Value = "Ben", .DbType = DbType.String})
		''' </summary>
		''' <param name="_Query">The T-SQL Query to execute, e.g. "SELECT * FROM [DatabaseName].[dbo].[TableName];"</param>
		''' <param name="_SQLParameters">(Optional) An array of SQLParameters, which are added to the connection object prior to executing the SQLCommand. They can be referanced in the _Query in the form @ParameterName. Each Parameter ought to have a ParameterName, Value, and DbType</param>
		''' <returns>The DataTable filled with the output from the Command.</returns>
		''' <remarks>There is potential for a blank DataTable to be returned here.</remarks>
		Public Function ExecuteQuery(ByVal _Query$, ByVal ParamArray _SQLParameters As SqlClient.SqlParameter()) As DataTable
			Try
				LogSystemMessage("Executing Query on SQL Server " & Me.SQLServerName.InSquares() & ": " & _Query.InSquares(), LogEvent.DSEventSeverity.Verbose)

				REM Order of Operations:
				'	Instanciate SQLConnection
				'	Instanciate SQLCommand
				'	Open() SQLConnection
				'
				'	Instanciate DataAdaptor
				'	DataAdaptor.Fill(DataTable)
				'
				'	Close() SQLConnection
				'	Dispose() DataAdaptor

				Dim _SQLConnection As New Global.System.Data.SqlClient.SqlConnection(Me.ConnectionString)
				Dim _SQLCommand As New Global.System.Data.SqlClient.SqlCommand(_Query, _SQLConnection)

				'Load the Parameters into the SQL Command
				For Each Param As SqlClient.SqlParameter In _SQLParameters : _SQLCommand.Parameters.Add(Param) : Next

				_SQLConnection.Open()

				Dim _DataAdaptor As New Global.System.Data.SqlClient.SqlDataAdapter(_SQLCommand)

				'Fill() actually executes the SQL Query on the Server
				Me.QueryOutputDataTable_ = New DataTable()
				_DataAdaptor.Fill(Me.QueryOutputDataTable_)

				_SQLConnection.Close()
				_DataAdaptor.Dispose()
				_SQLConnection.Dispose()
				_SQLCommand.Dispose()

				Return Me.QueryOutputDataTable

			Catch _Ex As Exception : Throw New DSException("@SQLQueryRunner\ExecuteQuery: " & _Ex.Message & ". The Query was: " & _Query.InSquares(), _Ex) : End Try
		End Function

#Region "QueryWatch Logic - For detecting a change in the result of an SQL Query"

		''' <summary>
		''' Begins listening for a change in the Output of the specified _Query. When a change occurs, the QueryWatch_QueryOutputChanged Event is Raised.
		''' 
		''' Example Call:
		''' Dim _SQLQueryRunner As New DocScript.Utilities.SQLQueryRunner(_SQLServerName:=&quot;MNLT01\SQLEXPRESS&quot;)
		''' _SQLQueryRunner.BeginQueryWatch(_DatabaseName:=&quot;QueryWatchTest&quot;, _Query:=&quot;SELECT &quot;&quot;Name&quot;&quot; FROM [dbo].[People];&quot;)
		''' AddHandler _SQLQueryRunner.QueryWatch_QueryOutputChanged, Sub(_EventArgs As SqlClient.SqlNotificationEventArgs) MsgBox(&quot;Type: &quot; &amp; _EventArgs.Type.ToString())
		''' </summary>
		''' <param name="_DatabaseName">Needed because a Queue will be registered with the ServiceBroker of this Database. The specified Database MUST have its Service Broker ENABLED. To enable the Service Broker: ALTER DATABASE [DBName] SET ENABLE_BROKER;</param>
		''' <param name="_Query">E.g. SELECT "Name" FROM [dbo].[People];</param>
		''' <param name="_SQLParameters">Any Parameters to be used in the SQL Command, accessible from the _Query in the format @ParameterName</param>
		''' <remarks>
		''' Check Broker is enabled with "SELECT name, is_broker_enabled FROM sys.databases;"
		''' The database user must have the SUBSCRIBE QUERY NOTIFICATIONS permission
		''' 
		''' _Query cannot contain *, and must use fully-qualified Tablenames instead of just the Table Name on its own.
		''' INVALID:	SELECT * FROM Messages;
		''' VALID:		SELECT ID, Message FROM dbo.Messages;
		''' </remarks>
		Public Sub BeginQueryWatch(ByVal _DatabaseName$, ByVal _Query$, ByVal ParamArray _SQLParameters As SqlClient.SqlParameter())
			Try
				LogSystemMessage("Registering QueryWatch for Database " & _DatabaseName.InSquares() & " on SQL Server " & Me.SQLServerName.InSquares() & " from Workstation " & My.Computer.Name.InSquares() & " for Query " & _Query.InSquares(), LogEvent.DSEventSeverity.Verbose)

				'_Query cannot contain *, and must use fully-qualified Tablenames ([dbo].[TableName]) instead of just the Table Name on its own (TableName).
				If _Query.Contains("*"c) OrElse (Not _Query.StartsWith("SELECT")) Then Throw New DSValidationException("The _Query must start with SELECT, and cannot contain the * WildCard", _Query)

				'Initialise the Data needed by RegisterSQLDependency()
				Me.QW_ConnectionStringWithInitialCatalog_ = "Initial Catalog=" & _DatabaseName & ";"c & Me.ConnectionString
				Me.QW_Query_ = _Query
				Me.QW_DatabaseName_ = _DatabaseName
				Me.QW_SQLParameters_ = _SQLParameters

				'Begin accepting SqlDependency Registrations on the SQL Server
				Global.System.Data.SqlClient.SqlDependency.Start(Me.QW_ConnectionStringWithInitialCatalog_)

				'Register the SQLDependancy. Once a change occurs, the QueryWatch_ChangeOccuredHandler_() Handler will be run, which will re-subscribe to the event.
				RegisterSQLDependency_()

			Catch _Ex As Exception : Throw New DSException("@SQLQueryRunner\BeginQueryWatch: " & _Ex.Message & ". The Query was: " & _Query.InSquares(), _Ex) : End Try
		End Sub

		REM SQL Connection Data set by BeginQueryWatch, and used within RegisterSQLDependency_() and QueryWatch_ChangeOccuredHandler_()
		Protected QW_ConnectionStringWithInitialCatalog_$
		Protected QW_Query_$
		Protected QW_DatabaseName_$
		Protected QW_SQLParameters_ As SqlClient.SqlParameter()

		Protected Sub RegisterSQLDependency_()

			Dim _SQLConnection As New Global.System.Data.SqlClient.SqlConnection(Me.QW_ConnectionStringWithInitialCatalog_)
			Dim _SQLCommand As New Global.System.Data.SqlClient.SqlCommand(Me.QW_Query_, _SQLConnection)
			For Each Param As SqlClient.SqlParameter In Me.QW_SQLParameters_ : _SQLCommand.Parameters.Add(Param) : Next

			_SQLCommand.Notification = Nothing

			Dim _SQLDependancy As New SqlClient.SqlDependency(_SQLCommand)
			AddHandler _SQLDependancy.OnChange, New SqlClient.OnChangeEventHandler(AddressOf Me.QueryWatch_ChangeOccuredHandler_)

			_SQLConnection.Open()
			_SQLCommand.ExecuteReader(CommandBehavior.CloseConnection)

			_SQLConnection.Dispose()
			_SQLCommand.Dispose()

		End Sub

		Protected Sub QueryWatch_ChangeOccuredHandler_(ByVal _Sender As Object, ByVal _EventArgs As SqlClient.SqlNotificationEventArgs)

			'This call dosen't work for some reason with the GUIDefault LogEventHandler, because of a Thread-access/EventHandlerAsync oddity. It works perfectly for CLIDefault though.
			LogSystemMessage("Query Output changed for Query " & Me.QW_Query_.InSquares() & " in Database " & Me.QW_DatabaseName_.InSquares() & " with Info " & _EventArgs.Info.ToString().InSquares() & " and Source " & _EventArgs.Source.ToString().InSquares() & " and Type " & _EventArgs.Type.ToString().InSquares())

			RaiseEvent QueryWatch_QueryOutputChanged(_EventArgs)

			REM We have to Unsubscribe from the Event, and then execute the Query again (which will re-subscribe the Handler to the Event...)
			Dim _SQLDependency As SqlClient.SqlDependency = TryCast(_Sender, SqlClient.SqlDependency)
			RemoveHandler _SQLDependency.OnChange, AddressOf Me.QueryWatch_ChangeOccuredHandler_

			Me.RegisterSQLDependency_()

		End Sub

#End Region

	End Class

End Namespace