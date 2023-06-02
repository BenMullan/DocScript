<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ProgramTreeViewer : Inherits System.Windows.Forms.Form

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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ProgramTreeViewer))
		Me.TheProgramTreeView = New System.Windows.Forms.TreeView()
		Me.ProgramTreeImageList = New System.Windows.Forms.ImageList(Me.components)
		Me.TheMenuStrip = New System.Windows.Forms.MenuStrip()
		Me.ViewXMLToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
		Me.ReconstructToSourceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
		Me.TheMenuStrip.SuspendLayout()
		Me.SuspendLayout()
		'
		'TheProgramTreeView
		'
		Me.TheProgramTreeView.BackColor = System.Drawing.Color.WhiteSmoke
		Me.TheProgramTreeView.Dock = System.Windows.Forms.DockStyle.Fill
		Me.TheProgramTreeView.ImageIndex = 0
		Me.TheProgramTreeView.ImageList = Me.ProgramTreeImageList
		Me.TheProgramTreeView.ItemHeight = 26
		Me.TheProgramTreeView.Location = New System.Drawing.Point(0, 24)
		Me.TheProgramTreeView.Name = "TheProgramTreeView"
		Me.TheProgramTreeView.SelectedImageIndex = 0
		Me.TheProgramTreeView.Size = New System.Drawing.Size(584, 338)
		Me.TheProgramTreeView.TabIndex = 0
		'
		'ProgramTreeImageList
		'
		Me.ProgramTreeImageList.ImageStream = CType(resources.GetObject("ProgramTreeImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.ProgramTreeImageList.TransparentColor = System.Drawing.Color.Transparent
		Me.ProgramTreeImageList.Images.SetKeyName(0, "DocScriptFile")
		Me.ProgramTreeImageList.Images.SetKeyName(1, "VariableDeclaration")
		Me.ProgramTreeImageList.Images.SetKeyName(2, "DSFunction")
		Me.ProgramTreeImageList.Images.SetKeyName(3, "VariableAssignment")
		Me.ProgramTreeImageList.Images.SetKeyName(4, "FunctionCall")
		Me.ProgramTreeImageList.Images.SetKeyName(5, "ReturnToCaller")
		Me.ProgramTreeImageList.Images.SetKeyName(6, "IfStatement")
		Me.ProgramTreeImageList.Images.SetKeyName(7, "WhileStatement")
		Me.ProgramTreeImageList.Images.SetKeyName(8, "LoopStatement")
		'
		'TheMenuStrip
		'
		Me.TheMenuStrip.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
		Me.TheMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ViewXMLToolStripMenuItem, Me.ReconstructToSourceToolStripMenuItem})
		Me.TheMenuStrip.Location = New System.Drawing.Point(0, 0)
		Me.TheMenuStrip.Name = "TheMenuStrip"
		Me.TheMenuStrip.Size = New System.Drawing.Size(584, 24)
		Me.TheMenuStrip.TabIndex = 1
		Me.TheMenuStrip.Text = "MenuStrip1"
		'
		'ViewXMLToolStripMenuItem
		'
		Me.ViewXMLToolStripMenuItem.Name = "ViewXMLToolStripMenuItem"
		Me.ViewXMLToolStripMenuItem.Size = New System.Drawing.Size(80, 20)
		Me.ViewXMLToolStripMenuItem.Text = "View &XML..."
		'
		'ReconstructToSourceToolStripMenuItem
		'
		Me.ReconstructToSourceToolStripMenuItem.Name = "ReconstructToSourceToolStripMenuItem"
		Me.ReconstructToSourceToolStripMenuItem.Size = New System.Drawing.Size(144, 20)
		Me.ReconstructToSourceToolStripMenuItem.Text = "Reconstruct to &Source..."
		'
		'ProgramTreeViewer
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(584, 362)
		Me.Controls.Add(Me.TheProgramTreeView)
		Me.Controls.Add(Me.TheMenuStrip)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MainMenuStrip = Me.TheMenuStrip
		Me.Name = "ProgramTreeViewer"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Program Tree"
		Me.TheMenuStrip.ResumeLayout(False)
		Me.TheMenuStrip.PerformLayout()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub

	Friend WithEvents TheProgramTreeView As System.Windows.Forms.TreeView
	Friend WithEvents ProgramTreeImageList As System.Windows.Forms.ImageList
	Friend WithEvents TheMenuStrip As System.Windows.Forms.MenuStrip
	Friend WithEvents ViewXMLToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
	Friend WithEvents ReconstructToSourceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class