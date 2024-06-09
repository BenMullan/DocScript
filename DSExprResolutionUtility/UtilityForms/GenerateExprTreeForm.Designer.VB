<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GenerateExprTreeForm
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
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(GenerateExprTreeForm))
		Me.ExprTreeView = New System.Windows.Forms.TreeView()
		Me.TheMenuStrip = New System.Windows.Forms.MenuStrip()
		Me.ViewXMLToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
		Me.ToolTipAllocator = New System.Windows.Forms.ToolTip(Me.components)
		Me.TheMenuStrip.SuspendLayout()
		Me.SuspendLayout()
		'
		'ExprTreeView
		'
		Me.ExprTreeView.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
		Me.ExprTreeView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.ExprTreeView.Dock = System.Windows.Forms.DockStyle.Fill
		Me.ExprTreeView.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.ExprTreeView.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
		Me.ExprTreeView.HotTracking = True
		Me.ExprTreeView.Indent = 30
		Me.ExprTreeView.ItemHeight = 24
		Me.ExprTreeView.Location = New System.Drawing.Point(0, 24)
		Me.ExprTreeView.Name = "ExprTreeView"
		Me.ExprTreeView.Size = New System.Drawing.Size(284, 418)
		Me.ExprTreeView.TabIndex = 0
		Me.ToolTipAllocator.SetToolTip(Me.ExprTreeView, "The Expression Tree")
		'
		'TheMenuStrip
		'
		Me.TheMenuStrip.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
		Me.TheMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ViewXMLToolStripMenuItem})
		Me.TheMenuStrip.Location = New System.Drawing.Point(0, 0)
		Me.TheMenuStrip.Name = "TheMenuStrip"
		Me.TheMenuStrip.ShowItemToolTips = True
		Me.TheMenuStrip.Size = New System.Drawing.Size(284, 24)
		Me.TheMenuStrip.TabIndex = 1
		Me.TheMenuStrip.Text = "MenuStrip1"
		'
		'ViewXMLToolStripMenuItem
		'
		Me.ViewXMLToolStripMenuItem.Name = "ViewXMLToolStripMenuItem"
		Me.ViewXMLToolStripMenuItem.Size = New System.Drawing.Size(71, 20)
		Me.ViewXMLToolStripMenuItem.Text = "View &XML"
		Me.ViewXMLToolStripMenuItem.ToolTipText = "Ctrl + Click to show Log"
		'
		'ToolTipAllocator
		'
		Me.ToolTipAllocator.ShowAlways = True
		Me.ToolTipAllocator.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info
		Me.ToolTipAllocator.ToolTipTitle = "Info"
		'
		'GenerateExprTreeForm
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(284, 442)
		Me.Controls.Add(Me.ExprTreeView)
		Me.Controls.Add(Me.TheMenuStrip)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MainMenuStrip = Me.TheMenuStrip
		Me.Name = "GenerateExprTreeForm"
		Me.Text = "View ExprTree"
		Me.TheMenuStrip.ResumeLayout(False)
		Me.TheMenuStrip.PerformLayout()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents TheMenuStrip As System.Windows.Forms.MenuStrip
	Friend WithEvents ViewXMLToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
	Friend WithEvents ToolTipAllocator As System.Windows.Forms.ToolTip
	Friend WithEvents ExprTreeView As System.Windows.Forms.TreeView
End Class
