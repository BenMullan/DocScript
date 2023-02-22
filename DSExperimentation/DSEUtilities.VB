Module DSEUtilities

	Public Sub LogMsg(ByVal _Message$)

		MsgBox("LogMsg is disabled, but was asked to log this message: " & _Message)

		'LogWindow.LogEntriesListBox.Items.Add(_Message)
		'LogWindow.Show()
		'LogWindow.LogEntriesListBox.SelectedIndex = LogWindow.LogEntriesListBox.Items.Count - 1
		'LogWindow.Refresh()
	End Sub

End Module