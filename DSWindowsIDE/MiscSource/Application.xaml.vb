REM Application-level events, such as Startup, Exit, and DispatcherUnhandledException can be handled in this file.
Class Application

	REM NOTE: ExamineCLAs_And_SetPassthroughVariables() is called from MainWindow\New()
	Protected Overrides Sub OnStartup(e As System.Windows.StartupEventArgs)
		MyBase.OnStartup(e)
	End Sub

End Class