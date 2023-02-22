Imports System.Windows.Forms

Public Class ExeResExplorerWindow

	Public ReadOnly ExecutionResult As DocScript.Language.Instructions.ExecutionResult

	Public Sub New(ByVal _ExeRes As Language.Instructions.ExecutionResult)

		' This call is required by the designer.
		InitializeComponent()

		Me.ExecutionResult = _ExeRes

		Me.TheRootExeResTreeView.BeginUpdate()
		Me.TheRootExeResTreeView.Nodes.Clear()
		Me.TheRootExeResTreeView.Nodes.Add(ExeResExplorerWindow.GenerateExeResTreeNode(Me.ExecutionResult))
		Me.TheRootExeResTreeView.EndUpdate()
		Me.TheRootExeResTreeView.ExpandAll()

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

	Public Shared Function GenerateExeResTreeNode(ByVal _ExeRes As DocScript.Language.Instructions.ExecutionResult) As System.Windows.Forms.TreeNode
		Try

			REM Root
			Dim _ExeResRootNode As New TreeNode(_ExeRes.ExecuteeDescription) With {.BackColor = System.Drawing.Color.FromArgb(192, 255, 192), .ImageKey = "ExeRes"}

			REM ExecutionTimeMS
			_ExeResRootNode.Nodes.Add(New TreeNode("Execution-Time: " & _ExeRes.ExecutionTimeMS.ToString() & "ms") With {.BackColor = System.Drawing.Color.FromArgb(192, 192, 255), .ImageKey = "TimeMS"})

			REM Archived SymTbl
			If _ExeRes.Archived_SymbolTable IsNot Nothing _
			 Then _ExeResRootNode.Nodes.Add(New TreeNode("Archived Symbol-Table """ & _ExeRes.Archived_SymbolTable.Description & """ (" & _ExeRes.Archived_SymbolTable.Entries_ReadOnlyCopy.Count.ToString() & " Entries)") With {.BackColor = System.Drawing.Color.FromArgb(255, 192, 255), .ImageKey = "SymTbl"})

			REM ReturnStatus
			If _ExeRes.ReturnStatus IsNot Nothing _
			  Then _ExeResRootNode.Nodes.Add(New TreeNode("Return-Status: " & _ExeRes.ReturnStatus.ToString()) With {.BackColor = System.Drawing.Color.FromArgb(255, 192, 192), .ImageKey = "ReturnStatus"})

			REM Upstairs ExeRess
			If _ExeRes.UpstairsExecutionResults IsNot Nothing Then
				For Each _UpstairsExeRes As Tuple(Of String, Language.Instructions.ExecutionResult) In _ExeRes.UpstairsExecutionResults

					Dim _ChildExeRes_Node As TreeNode = ExeResExplorerWindow.GenerateExeResTreeNode(_UpstairsExeRes.Item2)
					_ChildExeRes_Node.Text = _UpstairsExeRes.Item1 & ": " & _ChildExeRes_Node.Text
					_ExeResRootNode.Nodes.Add(_ChildExeRes_Node)

				Next
			End If
			
			Return _ExeResRootNode

		Catch _Ex As Exception : Throw New DSException("@GenerateExeResTreeNode: " & _Ex.Message, _Ex) : End Try
	End Function

End Class