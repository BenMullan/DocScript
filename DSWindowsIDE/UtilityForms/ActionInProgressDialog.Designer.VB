<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ActionInProgressDialog
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ActionInProgressDialog))
		Me.TheProgressBar = New System.Windows.Forms.ProgressBar()
		Me.CurrentActionDescription_Label = New System.Windows.Forms.Label()
		Me.SuspendLayout()
		'
		'TheProgressBar
		'
		Me.TheProgressBar.Location = New System.Drawing.Point(12, 12)
		Me.TheProgressBar.MarqueeAnimationSpeed = 8
		Me.TheProgressBar.Name = "TheProgressBar"
		Me.TheProgressBar.Size = New System.Drawing.Size(210, 23)
		Me.TheProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee
		Me.TheProgressBar.TabIndex = 0
		'
		'CurrentActionDescription_Label
		'
		Me.CurrentActionDescription_Label.AutoSize = True
		Me.CurrentActionDescription_Label.Location = New System.Drawing.Point(12, 46)
		Me.CurrentActionDescription_Label.Name = "CurrentActionDescription_Label"
		Me.CurrentActionDescription_Label.Size = New System.Drawing.Size(16, 13)
		Me.CurrentActionDescription_Label.TabIndex = 1
		Me.CurrentActionDescription_Label.Text = "..."
		'
		'ActionInProgressDialog
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(234, 72)
		Me.ControlBox = False
		Me.Controls.Add(Me.CurrentActionDescription_Label)
		Me.Controls.Add(Me.TheProgressBar)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.MaximumSize = New System.Drawing.Size(250, 110)
		Me.MinimizeBox = False
		Me.MinimumSize = New System.Drawing.Size(250, 110)
		Me.Name = "ActionInProgressDialog"
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Please Wait..."
		Me.TopMost = True
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents TheProgressBar As System.Windows.Forms.ProgressBar
	Friend WithEvents CurrentActionDescription_Label As System.Windows.Forms.Label
End Class
