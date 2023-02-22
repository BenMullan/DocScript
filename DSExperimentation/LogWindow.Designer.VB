<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LogWindow
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
		Me.LogEntriesListBox = New System.Windows.Forms.ListBox()
		Me.SuspendLayout()
		'
		'LogEntriesListBox
		'
		Me.LogEntriesListBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
					Or System.Windows.Forms.AnchorStyles.Left) _
					Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.LogEntriesListBox.FormattingEnabled = True
		Me.LogEntriesListBox.Location = New System.Drawing.Point(12, 12)
		Me.LogEntriesListBox.Name = "LogEntriesListBox"
		Me.LogEntriesListBox.ScrollAlwaysVisible = True
		Me.LogEntriesListBox.Size = New System.Drawing.Size(577, 329)
		Me.LogEntriesListBox.TabIndex = 0
		'
		'LogWindow
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(601, 350)
		Me.Controls.Add(Me.LogEntriesListBox)
		Me.Name = "LogWindow"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "LogWindow"
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents LogEntriesListBox As System.Windows.Forms.ListBox
End Class
