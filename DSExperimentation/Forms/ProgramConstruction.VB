Public Class ProgramConstruction

	'Used as a Demo for the Coursework DocX
	Sub PiecingItAllTogether()

		'Log via the Default LogWindow
		DocScript.Logging.LogUtilities.CurrentLogEventHandler = DocScript.Logging.BuiltInLogEventHandlers.GUIDefault

		'Raw
		Dim _Source As [String] = "..."

		'Parse
		Dim _Tokens As DocScript.Runtime.Token() = DocScript.Runtime.GetTokensFromSource(_Source)

		'Lex
		Dim _Program As New DocScript.Runtime.Program(_Tokens, DocScript.Runtime.ExecutionContext.GUIDefault)

		'Execute
		_Program.Run({})

	End Sub

	Public Sub ConstructProgram() Handles ConstructProgButton.Click
		Try

			InitialiseDocScriptStuff()

			Dim _Program As DocScript.Runtime.Program = DocScript.Runtime.Program.FromSource(Me.SourceTextBox.Text, DocScript.Runtime.ExecutionContext.GUIDefault)

			Me.ProgramTreeView.BeginUpdate()
			Me.ProgramTreeView.Nodes.Clear()

			Dim _ProgramRootNode As New TreeNode("Program") With {.BackColor = Color.FromArgb(192, 255, 192)}
			Dim _GlobalVarDecsNode As New TreeNode("GlobalVarDecs") With {.BackColor = Color.FromArgb(192, 192, 255)}
			Dim _FunctionsNode As New TreeNode("Functions") With {.BackColor = Color.FromArgb(255, 192, 255)}

			'Global Var Decs
			For Each _VarDec As DocScript.Language.Instructions.VariableDeclaration In _Program.GlobalVarDecs
				_GlobalVarDecsNode.Nodes.Add(New TreeNode(Text:=
					 DocScript.Language.Constants.OpeningDataTypeBracket & DocScript.Language.Variables.VariableUtilities.GetDataTypeString_FromDSVariableType(_VarDec.DataType) & DocScript.Language.Constants.ClosingDataTypeBracket & " " & _VarDec.Identifier & If(_VarDec.AssignmentExpr Is Nothing, " (Unsssigned)", " : " & _VarDec.AssignmentExpr.ToString())
					 ))
			Next

			'Functions
			For Each _Function As Language.Instructions.Statements.DSFunction In _Program.Functions
				_FunctionsNode.Nodes.Add(New TreeNode(Text:=
				   Language.Constants.Keyword_Function & " "c & DocScript.Language.Constants.OpeningDataTypeBracket & DocScript.Language.Variables.VariableUtilities.GetDataTypeString_FromDSVariableType(_Function.ReturnType) & DocScript.Language.Constants.ClosingDataTypeBracket & " "c & _Function.Identifier & " (" & _Function.Parameters.Count.ToString() & " Parameters) (" & _Function.Contents.Count.ToString() & " Instructions)"
				  ))
			Next

			_ProgramRootNode.Nodes.Add(_GlobalVarDecsNode) : _ProgramRootNode.Nodes.Add(_FunctionsNode)
			Me.ProgramTreeView.Nodes.Add(_ProgramRootNode)

			Me.ProgramTreeView.EndUpdate()
			Me.ProgramTreeView.ExpandAll()

			Me.ProgramXMLTextBox.Text = _Program.ProgramTreeXML.ToString()

		Catch _Ex As Exception When True : MsgBox("Exception: " & vbCrLf & vbCrLf & _Ex.Message, MsgBoxStyle.Critical, _Ex.GetType().FullName) : End Try
	End Sub

	Private Sub Button1_Click() Handles Button1.Click

		InitialiseDocScriptStuff()
		MsgBox("OpeningDataTypeBracket_TPV IsSatisfied by <" & vbCrLf & DocScript.Runtime.BuiltInTPVs.OpeningDataTypeBracket_TPV.IsSatisfiedBy({New DocScript.Runtime.Token("<", DocScript.Runtime.Token.TokenType.GrammarChar, 0, 0)}))

	End Sub

	Public Sub InitialiseDocScriptStuff() 'Handles Me.Shown
		Console.WriteLine("(ProgramConstruction) The Console is Working if you can read this!") : Console.WriteLine()
		DocScript.Logging.ProcessDebugEvents = False
		DocScript.Logging.LogUtilities.CurrentLogEventHandler = DocScript.Logging.BuiltInLogEventHandlers.CLIDefault
	End Sub

	Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
		InitialiseDocScriptStuff()
		For Each _Item As [String]() In {"2", "0", "1", "2", "2", "3", "4", "5", "6", "2"}.SplitWhere(Function(_String$) _String = "2")
			MsgBox("Length: " & _Item.Length & vbCrLf & String.Join("|", _Item))
		Next
	End Sub

	Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
		InitialiseDocScriptStuff()

		MsgBox("IsBalenced: " & DocScript.CompilerExtentions.UsefulMethods.ContainsWellBalencedPairs(Of String)(
	   {"Open", "Null", "Dull", "kfjsdhf", "Null", "Close"},
	   New Tuple(Of String, String)("Open", "Close")
		 ))

	End Sub

	Private Sub GSSTButton_Click() Handles GSSTButton.Click

		InitialiseDocScriptStuff()

		Dim _SubStatementType As Type
		Select Case InputBox("Provide SubStatement Type", My.Computer.Name, "IfStatement")
			Case "IfStatement" : _SubStatementType = GetType(Global.DocScript.Language.Instructions.Statements.IfStatement)
			Case "WhileStatement" : _SubStatementType = GetType(Global.DocScript.Language.Instructions.Statements.WhileStatement)
			Case "LoopStatement" : _SubStatementType = GetType(Global.DocScript.Language.Instructions.Statements.LoopStatement)
			Case Else : Throw New DSException("Unrecognised SubStatement Type (From DSExperimentation)")
		End Select

		'Me.ProgramXMLTextBox.Text = DocScript.Language.Instructions.Statements.GetSubStatementTokens_(_SubStatementType, DocScript.Runtime.GetTokensFromSource(Me.SourceTextBox.Text)).GetValuesAndLocationsString().Replace("{LineEnd}", vbCrLf)

	End Sub


	Private Sub ROTButton_Click() Handles ROTButton.Click
		InitialiseDocScriptStuff()
		MsgDebug("*"c.Repeat(92).Length.ToString())
	End Sub

	Private Sub TIBButton_Click() Handles TIBButton.Click

		InitialiseDocScriptStuff()

		Dim _String$ = "Line1" & vbCrLf & "Line2" & vbCrLf & "Line3" & vbCrLf & "Line4" & vbCrLf & "Line5" & vbCrLf & "Line6"

		MsgDebug("NonTabbed" & vbCrLf & _String.TabbedInBy(3) & vbCrLf & "NonTabbed")

	End Sub

	Private Sub VAButton_Click() Handles VAButton.Click

		InitialiseDocScriptStuff()

		Dim _VariableAssignment As New Global.DocScript.Language.Instructions.VariableAssignment(DocScript.Runtime.GetTokensFromSource(Me.SourceTextBox.Text))

	End Sub

	Private Sub RTCButton_Click() Handles RTCButton.Click

		InitialiseDocScriptStuff()

		Dim _ReturnToCaller As New Global.DocScript.Language.Instructions.ReturnToCaller(DocScript.Runtime.Parser.GetTokensFromSource(Me.SourceTextBox.Text))
		MsgDebug(_ReturnToCaller.GetProgramTreeNodeXML().ToString())

	End Sub

	Private Sub FCButton_Click() Handles FCButton.Click

		InitialiseDocScriptStuff()

		Dim _FunctionCall As New Global.DocScript.Language.Instructions.FunctionCall(DocScript.Runtime.Parser.GetTokensFromSource(Me.SourceTextBox.Text))
		MsgDebug(_FunctionCall.GetProgramTreeNodeXML().ToString())
		MsgDebug(_FunctionCall.ToString())

	End Sub

	Private Sub IOLWButton_Click() Handles IOLWButton.Click

		'MsgDebug(Global.DocScript.CompilerExtentions.IndexOfLastWhere(Of System.Int32)({5, 4, 7, 6, 2, 6, 4, 5, 6, 8, 7, 5, 6}, Function(_Element%) _Element = 5).ToString())
		MsgDebug({5, 4, 7, 6, 2, 6, 4, 5, 6, 8, 7, 5, 6}.IndexOfLastWhere(Function(_Element%) _Element = 4).ToString())

	End Sub

	Private Sub SSFRTButton_Click() Handles SSFRTButton.Click

		InitialiseDocScriptStuff()
		MsgDebug("Disabled, because the method is Private again.")
		'Me.ProgramXMLTextBox.Text = DocScript.Language.Instructions.Statements.StatementUtilities.GetSubStatementFromRemainingTokens_(DocScript.Runtime.Parser.GetTokensFromSource(Me.SourceTextBox.Text)).Item1.ToString()

	End Sub

	Private Sub IOLWButton_Click(sender As Object, e As EventArgs) Handles IOLWButton.Click, Button4.Click
		Dim _Dialog As New NewDialog()
		_Dialog.ShowDialog()
	End Sub

	Public Class NewDialog : Inherits CommonDialog

		Public Overrides Sub Reset()

		End Sub

		Protected Overrides Function RunDialog(hwndOwner As IntPtr) As Boolean
			Return False
		End Function

	End Class

End Class