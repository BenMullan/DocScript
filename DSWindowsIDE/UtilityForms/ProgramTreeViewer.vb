Imports System.Windows.Forms

Public Class ProgramTreeViewer

	Public ReadOnly Program As DocScript.Runtime.Program

	Public Sub New(ByVal _Program As DocScript.Runtime.Program)

		' This call is required by the designer.
		Me.InitializeComponent()

		Me.Program = _Program

		Me.TheProgramTreeView.BeginUpdate()
		Me.TheProgramTreeView.Nodes.Clear()
		Me.TheProgramTreeView.Nodes.Add(ProgramTreeViewer.GenerateProgramTreeNode(Me.Program))
		Me.TheProgramTreeView.EndUpdate()
		Me.TheProgramTreeView.ExpandAll()

	End Sub

	Public Shared Function GenerateProgramTreeNode(ByVal _Program As DocScript.Runtime.Program) As System.Windows.Forms.TreeNode
		Try

			Dim _ProgramRootNode As New TreeNode("Program") With {.BackColor = System.Drawing.Color.FromArgb(192, 255, 192), .ImageKey = "DocScriptFile"}
			Dim _GlobalVarDecsNode As New TreeNode("GlobalVarDecs") With {.BackColor = System.Drawing.Color.FromArgb(192, 192, 255), .ImageKey = "VariableDeclaration"}
			Dim _FunctionsNode As New TreeNode("DSFunctions") With {.BackColor = System.Drawing.Color.FromArgb(255, 192, 255), .ImageKey = "DSFunction"}

			'Global Var Decs
			For Each _VarDec As DocScript.Language.Instructions.VariableDeclaration In _Program.GlobalVarDecs
				_GlobalVarDecsNode.Nodes.Add(ProgramTreeViewer.GenerateTreeNodeForIInstruction(_VarDec))
			Next

			'Functions
			For Each _Function As Language.Instructions.Statements.DSFunction In _Program.Functions
				_FunctionsNode.Nodes.Add(ProgramTreeViewer.GenerateTreeNodeForIStatement_(_Function))
			Next

			_ProgramRootNode.Nodes.Add(_GlobalVarDecsNode) : _ProgramRootNode.Nodes.Add(_FunctionsNode)
			Return _ProgramRootNode

		Catch _Ex As Exception : Throw New DSException("@GenerateProgramTreeNode: " & _Ex.Message, _Ex) : End Try
	End Function

	Protected Shared Function GenerateTreeNodeForIStatement_(ByVal _IStatement As DocScript.Language.Instructions.Statements.IStatement) As System.Windows.Forms.TreeNode

		Dim _NodeToReturn As New TreeNode(Text:=_IStatement.GetType().Name.InSquares() & " "c & _IStatement.ToString().Split({vbCrLf}, StringSplitOptions.None).ElementAt(0)) With {
		 .ImageKey = _IStatement.GetType.Name,
		 .SelectedImageKey = _IStatement.GetType.Name,
		.ToolTipText = _IStatement.ToString()
		}

		'Add the ElseContents it it's an IfStatement
		If _IStatement.GetType() = GetType(Language.Instructions.Statements.IfStatement) Then

			Dim _IfStatement As Language.Instructions.Statements.IfStatement = _IStatement.MustBe(Of Language.Instructions.Statements.IfStatement)()

			'Normal Contents
			Dim _IfContents_TreeNode As New TreeNode(Text:="(If Contents)") With {.ImageKey = "IfStatement", .SelectedImageKey = "IfStatement"}
			For Each _ChildInstr As Language.Instructions.IInstruction In _IfStatement.Contents
				If _ChildInstr.GetType().GetInterfaces().Contains(GetType(Language.Instructions.Statements.IStatement)) Then
					_IfContents_TreeNode.Nodes.Add(ProgramTreeViewer.GenerateTreeNodeForIStatement_(CType(_ChildInstr, Language.Instructions.Statements.IStatement)))
				ElseIf _ChildInstr.GetType().GetInterfaces().Contains(GetType(Language.Instructions.IInstruction)) Then
					_IfContents_TreeNode.Nodes.Add(ProgramTreeViewer.GenerateTreeNodeForIInstruction(_ChildInstr))
				Else : Throw New DSException("Unrecognised Child Instruction Interface Implementation")
				End If
			Next
			_NodeToReturn.Nodes.Add(_IfContents_TreeNode)

			'ElseContents
			If _IfStatement.ElseContents IsNot Nothing Then
				Dim _ElseContents_TreeNode As New TreeNode(Text:="(Else Contents)") With {.ImageKey = "IfStatement", .SelectedImageKey = "IfStatement"}
				For Each _ChildInstr As Language.Instructions.IInstruction In _IfStatement.ElseContents
					If _ChildInstr.GetType().GetInterfaces().Contains(GetType(Language.Instructions.Statements.IStatement)) Then
						_ElseContents_TreeNode.Nodes.Add(ProgramTreeViewer.GenerateTreeNodeForIStatement_(CType(_ChildInstr, Language.Instructions.Statements.IStatement)))
					ElseIf _ChildInstr.GetType().GetInterfaces().Contains(GetType(Language.Instructions.IInstruction)) Then
						_ElseContents_TreeNode.Nodes.Add(ProgramTreeViewer.GenerateTreeNodeForIInstruction(_ChildInstr))
					Else : Throw New DSException("Unrecognised Child Instruction Interface Implementation")
					End If
				Next
				_NodeToReturn.Nodes.Add(_ElseContents_TreeNode)
			End If

			Return _NodeToReturn

		End If

		'For non-IfStatements
		For Each _ChildInstr As Language.Instructions.IInstruction In _IStatement.Contents
			If _ChildInstr.GetType().GetInterfaces().Contains(GetType(Language.Instructions.Statements.IStatement)) Then
				_NodeToReturn.Nodes.Add(ProgramTreeViewer.GenerateTreeNodeForIStatement_(CType(_ChildInstr, Language.Instructions.Statements.IStatement)))
			ElseIf _ChildInstr.GetType().GetInterfaces().Contains(GetType(Language.Instructions.IInstruction)) Then
				_NodeToReturn.Nodes.Add(ProgramTreeViewer.GenerateTreeNodeForIInstruction(_ChildInstr))
			Else : Throw New DSException("Unrecognised Child Instruction Interface Implementation")
			End If
		Next

		Return _NodeToReturn

	End Function

	Protected Shared Function GenerateTreeNodeForIInstruction(ByVal _IInstruction As DocScript.Language.Instructions.IInstruction) As System.Windows.Forms.TreeNode

		Dim _NodeToReturn As New TreeNode(Text:=_IInstruction.GetType().Name.InSquares() & " "c & _IInstruction.ToString()) With {
		   .ImageKey = _IInstruction.GetType.Name,
		   .SelectedImageKey = _IInstruction.GetType.Name,
		   .ToolTipText = _IInstruction.ToString()
		}

		Return _NodeToReturn

	End Function

	Public Sub ShowProgramXML() Handles ViewXMLToolStripMenuItem.Click

		Dim _SimpleTextForm As New Windows.Forms.Form() With {.Text = "Program XML", .Width = 640, .Height = 768, .Icon = Nothing, .ShowIcon = False, .ShowInTaskbar = False}
		_SimpleTextForm.Controls.Add(New Windows.Forms.TextBox() With {.Multiline = True, .Dock = DockStyle.Fill, .Text = Me.Program.ProgramTreeXML.ToString().Replace("  ", "    "), .ReadOnly = True})

		REM Could do something like this in the future, to show the XML with Syntax Highlighting
		'_SimpleTextForm.Controls.Add(New Windows.Forms.WebBrowser() With {.Dock = DockStyle.Fill, .DocumentText = Me.Program.ProgramTreeXML.ToString().Replace("  ", "    ")})

		Call (_SimpleTextForm).Show()

	End Sub

#Region "Close the form on pressing [Esc]"

	Protected Sub Register_CloseOnEscKey_Handler() Handles Me.Load
		Me.KeyPreview = True
		AddHandler Me.KeyDown, Sub(_Sender As Object, _KeyEventArgs As Forms.KeyEventArgs) If _KeyEventArgs.KeyCode = Forms.Keys.Escape Then Me.Close()
	End Sub

	Protected Overrides Function ProcessDialogKey(keyData As System.Windows.Forms.Keys) As Boolean
		If (Windows.Forms.Form.ModifierKeys = Forms.Keys.None) AndAlso (keyData = Forms.Keys.Escape) Then
			Me.Close() : Return True
		End If
		Return MyBase.ProcessDialogKey(keyData)
	End Function

#End Region

End Class