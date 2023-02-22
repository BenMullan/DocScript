Public Class DSEFormChooser

	Private Sub ExprExpButton_Click() Handles ExprExpButton.Click
		With (New ExpressionExperiments())
			.ShowDialog()
		End With
	End Sub

	Private Sub ParserExpButton_Click() Handles ParserExpButton.Click
		With (New ParserExperiments())
			.ShowDialog()
		End With
	End Sub

	Private Sub ProgConstructionButton_Click() Handles ProgConstructionButton.Click
		With (New ProgramConstruction())
			.ShowDialog()
		End With
	End Sub

	Private Sub InstructionTreesButton_Click() Handles InstructionTreesButton.Click
		With (New InstructionTrees())
			.ShowDialog()
		End With
	End Sub

	Private Sub SQLExperimentsButton_Click() Handles SQLExperimentsButton.Click
		With (New SQLExperiments())
			.ShowDialog()
		End With
	End Sub

	Private Sub BIFExperimentsButton_Click() Handles BIFExperimentsButton.Click
		With (New BIFExperiments())
			.ShowDialog()
		End With
	End Sub

End Class