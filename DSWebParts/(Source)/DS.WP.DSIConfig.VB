Namespace WebParts

	Public Module DSIConfig

#Region "DSInteractive.config Setting-Retrival Propities"

		Public ReadOnly Property DocScriptDatabaseName As String
			Get
				Try : Return System.Configuration.ConfigurationManager.AppSettings.Item("DocScriptDatabaseName").MustNotBeNothing("The DocScriptDatabaseName was not successfully read from the XML Configuration File.")
				Catch _Ex As Exception : Throw New DSException("@DocScriptDatabaseName: " & _Ex.Message, _Ex) : End Try
			End Get
		End Property

		Public ReadOnly Property DocScriptSQLServer As String
			Get
				Try : Return System.Configuration.ConfigurationManager.AppSettings.Item("DocScriptSQLServer").MustNotBeNothing("The DocScriptSQLServer was not successfully read from the XML Configuration File.")
				Catch _Ex As Exception : Throw New DSException("@DocScriptSQLServer: " & _Ex.Message, _Ex) : End Try
			End Get
		End Property

		Public ReadOnly Property ESWorker_EXEPath As String
			Get
				Try : Return (New IO.DirectoryInfo(HttpContext.Current.Server.MapPath("~"))).FullName & System.Configuration.ConfigurationManager.AppSettings.Item("ESWorker_EXEPath").MustNotBeNothing("The ESWorker_EXEPath was not successfully read from the XML Configuration File.")
				Catch _Ex As Exception : Throw New DSException("@ESWorker_EXEPath: " & _Ex.Message, _Ex) : End Try
			End Get
		End Property

#End Region

	End Module

End Namespace