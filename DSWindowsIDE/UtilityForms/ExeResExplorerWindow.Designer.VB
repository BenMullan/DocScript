<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ExeResExplorerWindow
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ExeResExplorerWindow))
		Me.TheRootExeResTreeView = New System.Windows.Forms.TreeView()
		Me.TheExeResTreeImageList = New System.Windows.Forms.ImageList(Me.components)
		Me.SuspendLayout()
		'
		'TheRootExeResTreeView
		'
		Me.TheRootExeResTreeView.Dock = System.Windows.Forms.DockStyle.Fill
		Me.TheRootExeResTreeView.ImageIndex = 0
		Me.TheRootExeResTreeView.ImageList = Me.TheExeResTreeImageList
		Me.TheRootExeResTreeView.Location = New System.Drawing.Point(0, 0)
		Me.TheRootExeResTreeView.Name = "TheRootExeResTreeView"
		Me.TheRootExeResTreeView.SelectedImageIndex = 0
		Me.TheRootExeResTreeView.Size = New System.Drawing.Size(684, 262)
		Me.TheRootExeResTreeView.TabIndex = 0
		'
		'TheExeResTreeImageList
		'
		Me.TheExeResTreeImageList.ImageStream = CType(resources.GetObject("TheExeResTreeImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.TheExeResTreeImageList.TransparentColor = System.Drawing.Color.Transparent
		Me.TheExeResTreeImageList.Images.SetKeyName(0, "ExeRes")
		Me.TheExeResTreeImageList.Images.SetKeyName(1, "TimeMS")
		Me.TheExeResTreeImageList.Images.SetKeyName(2, "SymTbl")
		Me.TheExeResTreeImageList.Images.SetKeyName(3, "ReturnStatus")
		'
		'ExeResExplorerWindow
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(684, 262)
		Me.Controls.Add(Me.TheRootExeResTreeView)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.Name = "ExeResExplorerWindow"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "ExecutionResult Explorer"
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents TheRootExeResTreeView As System.Windows.Forms.TreeView
	Friend WithEvents TheExeResTreeImageList As System.Windows.Forms.ImageList
End Class
