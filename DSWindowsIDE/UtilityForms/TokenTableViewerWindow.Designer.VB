<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TokenTableViewerWindow
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TokenTableViewerWindow))
		Me.TokensTable_DataGridView = New System.Windows.Forms.DataGridView()
		Me.IndexCol = New System.Windows.Forms.DataGridViewTextBoxColumn()
		Me.TokenTypeCol = New System.Windows.Forms.DataGridViewTextBoxColumn()
		Me.TokenValueCol = New System.Windows.Forms.DataGridViewTextBoxColumn()
		Me.TokenLISCol = New System.Windows.Forms.DataGridViewTextBoxColumn()
		CType(Me.TokensTable_DataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'TokensTable_DataGridView
		'
		Me.TokensTable_DataGridView.AllowUserToOrderColumns = True
		Me.TokensTable_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		Me.TokensTable_DataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.IndexCol, Me.TokenTypeCol, Me.TokenValueCol, Me.TokenLISCol})
		Me.TokensTable_DataGridView.Dock = System.Windows.Forms.DockStyle.Fill
		Me.TokensTable_DataGridView.Location = New System.Drawing.Point(0, 0)
		Me.TokensTable_DataGridView.Name = "TokensTable_DataGridView"
		Me.TokensTable_DataGridView.Size = New System.Drawing.Size(735, 299)
		Me.TokensTable_DataGridView.TabIndex = 0
		'
		'IndexCol
		'
		Me.IndexCol.HeaderText = "#"
		Me.IndexCol.Name = "IndexCol"
		Me.IndexCol.ReadOnly = True
		Me.IndexCol.Width = 30
		'
		'TokenTypeCol
		'
		Me.TokenTypeCol.HeaderText = "Type"
		Me.TokenTypeCol.Name = "TokenTypeCol"
		Me.TokenTypeCol.ReadOnly = True
		'
		'TokenValueCol
		'
		Me.TokenValueCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
		Me.TokenValueCol.HeaderText = "Value"
		Me.TokenValueCol.Name = "TokenValueCol"
		Me.TokenValueCol.ReadOnly = True
		'
		'TokenLISCol
		'
		Me.TokenLISCol.HeaderText = "Location In Source"
		Me.TokenLISCol.Name = "TokenLISCol"
		Me.TokenLISCol.ReadOnly = True
		'
		'TokenTableViewerWindow
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(735, 299)
		Me.Controls.Add(Me.TokensTable_DataGridView)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.Name = "TokenTableViewerWindow"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Tokens' Viewer"
		CType(Me.TokensTable_DataGridView, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents TokensTable_DataGridView As System.Windows.Forms.DataGridView
	Friend WithEvents IndexCol As System.Windows.Forms.DataGridViewTextBoxColumn
	Friend WithEvents TokenTypeCol As System.Windows.Forms.DataGridViewTextBoxColumn
	Friend WithEvents TokenValueCol As System.Windows.Forms.DataGridViewTextBoxColumn
	Friend WithEvents TokenLISCol As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
