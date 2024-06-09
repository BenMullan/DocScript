<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BIFExplorerWindow
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BIFExplorerWindow))
		Me.BIFsListView = New System.Windows.Forms.ListView()
		Me.IdentifierColumn = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me.DescriptionColumn = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me.ReturnTypeColumn = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me.ParametersColumn = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me.BIFExplorerImageList = New System.Windows.Forms.ImageList(Me.components)
		Me.FilterTextBox = New System.Windows.Forms.TextBox()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.TheToolTipProvider = New System.Windows.Forms.ToolTip(Me.components)
		Me.SuspendLayout()
		'
		'BIFsListView
		'
		Me.BIFsListView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
				  Or System.Windows.Forms.AnchorStyles.Left) _
				  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.BIFsListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.IdentifierColumn, Me.DescriptionColumn, Me.ReturnTypeColumn, Me.ParametersColumn})
		Me.BIFsListView.FullRowSelect = True
		Me.BIFsListView.LargeImageList = Me.BIFExplorerImageList
		Me.BIFsListView.Location = New System.Drawing.Point(0, 38)
		Me.BIFsListView.Name = "BIFsListView"
		Me.BIFsListView.ShowItemToolTips = True
		Me.BIFsListView.Size = New System.Drawing.Size(784, 324)
		Me.BIFsListView.SmallImageList = Me.BIFExplorerImageList
		Me.BIFsListView.TabIndex = 0
		Me.BIFsListView.UseCompatibleStateImageBehavior = False
		Me.BIFsListView.View = System.Windows.Forms.View.Details
		'
		'IdentifierColumn
		'
		Me.IdentifierColumn.Text = "Identifier"
		Me.IdentifierColumn.Width = 200
		'
		'DescriptionColumn
		'
		Me.DescriptionColumn.Text = "Description"
		Me.DescriptionColumn.Width = 350
		'
		'ReturnTypeColumn
		'
		Me.ReturnTypeColumn.Text = "ReturnType"
		Me.ReturnTypeColumn.Width = 100
		'
		'ParametersColumn
		'
		Me.ParametersColumn.Text = "Parameters"
		Me.ParametersColumn.Width = 250
		'
		'BIFExplorerImageList
		'
		Me.BIFExplorerImageList.ImageStream = CType(resources.GetObject("BIFExplorerImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.BIFExplorerImageList.TransparentColor = System.Drawing.Color.Transparent
		Me.BIFExplorerImageList.Images.SetKeyName(0, "BuiltInFunction")
		'
		'FilterTextBox
		'
		Me.FilterTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
				  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.FilterTextBox.Location = New System.Drawing.Point(62, 10)
		Me.FilterTextBox.Name = "FilterTextBox"
		Me.FilterTextBox.Size = New System.Drawing.Size(710, 20)
		Me.FilterTextBox.TabIndex = 1
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Location = New System.Drawing.Point(12, 13)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(44, 13)
		Me.Label1.TabIndex = 2
		Me.Label1.Text = "Search:"
		'
		'TheToolTipProvider
		'
		Me.TheToolTipProvider.AutoPopDelay = 500000
		Me.TheToolTipProvider.InitialDelay = 0
		Me.TheToolTipProvider.ReshowDelay = 1
		Me.TheToolTipProvider.ShowAlways = True
		Me.TheToolTipProvider.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info
		Me.TheToolTipProvider.ToolTipTitle = "Template Call"
		'
		'BIFExplorerWindow
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(784, 362)
		Me.Controls.Add(Me.Label1)
		Me.Controls.Add(Me.FilterTextBox)
		Me.Controls.Add(Me.BIFsListView)
		Me.HelpButton = True
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "BIFExplorerWindow"
		Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
		Me.Text = "BuiltInFunction Explorer"
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents BIFsListView As System.Windows.Forms.ListView
	Friend WithEvents IdentifierColumn As System.Windows.Forms.ColumnHeader
	Friend WithEvents DescriptionColumn As System.Windows.Forms.ColumnHeader
	Friend WithEvents ParametersColumn As System.Windows.Forms.ColumnHeader
	Friend WithEvents BIFExplorerImageList As System.Windows.Forms.ImageList
	Friend WithEvents ReturnTypeColumn As System.Windows.Forms.ColumnHeader
	Friend WithEvents FilterTextBox As System.Windows.Forms.TextBox
	Friend WithEvents Label1 As System.Windows.Forms.Label
	Friend WithEvents TheToolTipProvider As System.Windows.Forms.ToolTip
End Class
