<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InputExprForm
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(InputExprForm))
		Me.ResolveButton = New System.Windows.Forms.Button()
		Me.ExprInputTextBox = New System.Windows.Forms.TextBox()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.GenerateExprTreeButton = New System.Windows.Forms.Button()
		Me.ToolTipAllocator = New System.Windows.Forms.ToolTip(Me.components)
		Me.TheMenuStrip = New System.Windows.Forms.MenuStrip()
		Me.TheStatusStrip = New System.Windows.Forms.StatusStrip()
		Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
		Me.OptimiseLinkLabel = New System.Windows.Forms.LinkLabel()
		Me.TheStatusStrip.SuspendLayout()
		Me.SuspendLayout()
		'
		'ResolveButton
		'
		Me.ResolveButton.Location = New System.Drawing.Point(15, 12)
		Me.ResolveButton.Name = "ResolveButton"
		Me.ResolveButton.Size = New System.Drawing.Size(75, 23)
		Me.ResolveButton.TabIndex = 0
		Me.ResolveButton.Text = "&Resolve"
		Me.ResolveButton.UseVisualStyleBackColor = True
		'
		'ExprInputTextBox
		'
		Me.ExprInputTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
				  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.ExprInputTextBox.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.ExprInputTextBox.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.ExprInputTextBox.Location = New System.Drawing.Point(49, 44)
		Me.ExprInputTextBox.Name = "ExprInputTextBox"
		Me.ExprInputTextBox.Size = New System.Drawing.Size(373, 23)
		Me.ExprInputTextBox.TabIndex = 1
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Location = New System.Drawing.Point(12, 48)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(31, 13)
		Me.Label1.TabIndex = 2
		Me.Label1.Text = "Expr:"
		'
		'GenerateExprTreeButton
		'
		Me.GenerateExprTreeButton.Location = New System.Drawing.Point(96, 12)
		Me.GenerateExprTreeButton.Name = "GenerateExprTreeButton"
		Me.GenerateExprTreeButton.Size = New System.Drawing.Size(108, 23)
		Me.GenerateExprTreeButton.TabIndex = 3
		Me.GenerateExprTreeButton.Text = "Generate Expr&Tree"
		Me.GenerateExprTreeButton.UseVisualStyleBackColor = True
		'
		'ToolTipAllocator
		'
		Me.ToolTipAllocator.ShowAlways = True
		Me.ToolTipAllocator.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info
		Me.ToolTipAllocator.ToolTipTitle = "Info"
		'
		'TheMenuStrip
		'
		Me.TheMenuStrip.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
		Me.TheMenuStrip.Location = New System.Drawing.Point(0, 0)
		Me.TheMenuStrip.Name = "TheMenuStrip"
		Me.TheMenuStrip.Size = New System.Drawing.Size(434, 24)
		Me.TheMenuStrip.TabIndex = 4
		Me.TheMenuStrip.Text = "MenuStrip1"
		'
		'TheStatusStrip
		'
		Me.TheStatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1})
		Me.TheStatusStrip.Location = New System.Drawing.Point(0, 80)
		Me.TheStatusStrip.Name = "TheStatusStrip"
		Me.TheStatusStrip.Size = New System.Drawing.Size(434, 22)
		Me.TheStatusStrip.TabIndex = 5
		Me.TheStatusStrip.Text = "StatusStrip1"
		'
		'ToolStripStatusLabel1
		'
		Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
		Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(64, 17)
		Me.ToolStripStatusLabel1.Text = "Status: Idle"
		'
		'OptimiseLinkLabel
		'
		Me.OptimiseLinkLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.OptimiseLinkLabel.AutoSize = True
		Me.OptimiseLinkLabel.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
		Me.OptimiseLinkLabel.Location = New System.Drawing.Point(374, 4)
		Me.OptimiseLinkLabel.Name = "OptimiseLinkLabel"
		Me.OptimiseLinkLabel.Size = New System.Drawing.Size(56, 13)
		Me.OptimiseLinkLabel.TabIndex = 6
		Me.OptimiseLinkLabel.TabStop = True
		Me.OptimiseLinkLabel.Text = "&Optimise..."
		'
		'InputExprForm
		'
		Me.AcceptButton = Me.ResolveButton
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(434, 102)
		Me.Controls.Add(Me.OptimiseLinkLabel)
		Me.Controls.Add(Me.TheStatusStrip)
		Me.Controls.Add(Me.GenerateExprTreeButton)
		Me.Controls.Add(Me.Label1)
		Me.Controls.Add(Me.ExprInputTextBox)
		Me.Controls.Add(Me.ResolveButton)
		Me.Controls.Add(Me.TheMenuStrip)
		Me.Enabled = False
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MainMenuStrip = Me.TheMenuStrip
		Me.MaximizeBox = False
		Me.MaximumSize = New System.Drawing.Size(10000, 140)
		Me.MinimumSize = New System.Drawing.Size(240, 140)
		Me.Name = "InputExprForm"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "DocScript Standalone Expression Resolution Utility"
		Me.TheStatusStrip.ResumeLayout(False)
		Me.TheStatusStrip.PerformLayout()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents ResolveButton As System.Windows.Forms.Button
	Friend WithEvents ExprInputTextBox As System.Windows.Forms.TextBox
	Friend WithEvents Label1 As System.Windows.Forms.Label
	Friend WithEvents GenerateExprTreeButton As System.Windows.Forms.Button
	Friend WithEvents ToolTipAllocator As System.Windows.Forms.ToolTip
	Friend WithEvents TheMenuStrip As System.Windows.Forms.MenuStrip
	Friend WithEvents TheStatusStrip As System.Windows.Forms.StatusStrip
	Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
	Friend WithEvents OptimiseLinkLabel As System.Windows.Forms.LinkLabel
End Class
