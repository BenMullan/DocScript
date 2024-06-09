<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PictorialHelpWindow
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PictorialHelpWindow))
		Me.PicturesTabControl = New System.Windows.Forms.TabControl()
		Me.SuspendLayout()
		'
		'PicturesTabControl
		'
		Me.PicturesTabControl.Dock = System.Windows.Forms.DockStyle.Fill
		Me.PicturesTabControl.Location = New System.Drawing.Point(0, 0)
		Me.PicturesTabControl.Name = "PicturesTabControl"
		Me.PicturesTabControl.SelectedIndex = 0
		Me.PicturesTabControl.Size = New System.Drawing.Size(784, 562)
		Me.PicturesTabControl.TabIndex = 0
		'
		'PictorialHelpWindow
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(784, 562)
		Me.Controls.Add(Me.PicturesTabControl)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MinimumSize = New System.Drawing.Size(200, 200)
		Me.Name = "PictorialHelpWindow"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "DSIDE Pictorial Help                                                       (Use [" & _
		  "Ctrl] + [Tab] to cycle through...)"
		Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents PicturesTabControl As System.Windows.Forms.TabControl
End Class
