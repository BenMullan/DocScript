<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InstructionTrees
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
		Dim TreeNode1 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("FunctionCall")
		Dim TreeNode2 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("IfStatement", New System.Windows.Forms.TreeNode() {TreeNode1})
		Dim TreeNode3 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("ReturnToCaller")
		Dim TreeNode4 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Function", New System.Windows.Forms.TreeNode() {TreeNode2, TreeNode3})
		Dim TreeNode5 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Program", New System.Windows.Forms.TreeNode() {TreeNode4})
		Me.ProgramTreeView = New System.Windows.Forms.TreeView()
		Me.SuspendLayout()
		'
		'ProgramTreeView
		'
		Me.ProgramTreeView.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
		Me.ProgramTreeView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.ProgramTreeView.Dock = System.Windows.Forms.DockStyle.Fill
		Me.ProgramTreeView.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.ProgramTreeView.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
		Me.ProgramTreeView.HotTracking = True
		Me.ProgramTreeView.Indent = 30
		Me.ProgramTreeView.ItemHeight = 24
		Me.ProgramTreeView.Location = New System.Drawing.Point(0, 0)
		Me.ProgramTreeView.Name = "ProgramTreeView"
		TreeNode1.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
		TreeNode1.Name = "Node3"
		TreeNode1.Text = "FunctionCall"
		TreeNode2.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
		TreeNode2.Name = "Node2"
		TreeNode2.Text = "IfStatement"
		TreeNode3.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
		TreeNode3.Name = "Node4"
		TreeNode3.Text = "ReturnToCaller"
		TreeNode4.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
		TreeNode4.Name = "Node1"
		TreeNode4.Text = "Function"
		TreeNode5.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
		TreeNode5.Name = "Node0"
		TreeNode5.Text = "Program"
		Me.ProgramTreeView.Nodes.AddRange(New System.Windows.Forms.TreeNode() {TreeNode5})
		Me.ProgramTreeView.Size = New System.Drawing.Size(436, 305)
		Me.ProgramTreeView.TabIndex = 1
		'
		'InstructionTrees
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(436, 305)
		Me.Controls.Add(Me.ProgramTreeView)
		Me.Name = "InstructionTrees"
		Me.Text = "InstructionTrees"
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents ProgramTreeView As System.Windows.Forms.TreeView
End Class
