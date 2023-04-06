Public Class DSEFormChooser

	Private Sub ExprExpButton_Click() Handles ExprExpButton.Click
		Call (New ExpressionExperiments()).ShowDialog()
	End Sub

	Private Sub ParserExpButton_Click() Handles ParserExpButton.Click
		Call (New ParserExperiments()).ShowDialog()
	End Sub

	Private Sub ProgConstructionButton_Click() Handles ProgConstructionButton.Click
		Call (New ProgramConstruction()).ShowDialog()
	End Sub

	Private Sub InstructionTreesButton_Click() Handles InstructionTreesButton.Click
		Call (New InstructionTrees()).ShowDialog()
	End Sub

	Private Sub SQLExperimentsButton_Click() Handles SQLExperimentsButton.Click
		Call (New SQLExperiments()).ShowDialog()
	End Sub

	Private Sub BIFExperimentsButton_Click() Handles BIFExperimentsButton.Click
		Call (New BIFExperiments()).ShowDialog()
	End Sub

	Private Sub DSRemotingButton_Click(sender As System.Object, e As System.EventArgs) Handles DSRemotingButton.Click
		Call (New RemotingExperiments()).ShowDialog()
	End Sub

End Class