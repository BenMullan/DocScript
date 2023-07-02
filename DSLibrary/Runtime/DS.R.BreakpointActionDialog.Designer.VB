Namespace Runtime

	<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
	Partial Public Class BreakpointActionDialog : Inherits System.Windows.Forms.Form

		'Form overrides dispose to clean up the component list.
		<System.Diagnostics.DebuggerNonUserCode()> _
		Protected Overrides Sub Dispose(ByVal disposing As [Boolean])
			Try : If disposing AndAlso components IsNot Nothing Then components.Dispose()
			Finally : MyBase.Dispose(disposing)
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
			Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BreakpointActionDialog))
			Me.DialogInstructionTextLabel = New System.Windows.Forms.Label()
			Me.BreakPointIconPictureBox = New System.Windows.Forms.PictureBox()
			Me.ContinueExecButton = New System.Windows.Forms.Button()
			Me.EditInDSLiveButton = New System.Windows.Forms.Button()
			Me.CopyToDSLiveButton = New System.Windows.Forms.Button()
			Me.ViewSymTblsButton = New System.Windows.Forms.Button()
			Me.TheToolTipProvider = New System.Windows.Forms.ToolTip(Me.components)
			Me.IgnoreAllFutureBreakPointsCheckBox = New System.Windows.Forms.CheckBox()
			CType(Me.BreakPointIconPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'DialogInstructionTextLabel
			'
			Me.DialogInstructionTextLabel.AutoSize = True
			Me.DialogInstructionTextLabel.Location = New System.Drawing.Point(95, 24)
			Me.DialogInstructionTextLabel.Name = "DialogInstructionTextLabel"
			Me.DialogInstructionTextLabel.Size = New System.Drawing.Size(281, 26)
			Me.DialogInstructionTextLabel.TabIndex = 2
			Me.DialogInstructionTextLabel.Text = "Execution of the DocScript Program has been suspended." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Choose one of the below a" & _
			  "ctions, or press [Esc]."
			Me.DialogInstructionTextLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter
			'
			'BreakPointIconPictureBox
			'
			Me.BreakPointIconPictureBox.Image = Global.DocScript.My.Resources.Resources.BreakPoint
			Me.BreakPointIconPictureBox.Location = New System.Drawing.Point(28, 1)
			Me.BreakPointIconPictureBox.Name = "BreakPointIconPictureBox"
			Me.BreakPointIconPictureBox.Size = New System.Drawing.Size(73, 71)
			Me.BreakPointIconPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
			Me.BreakPointIconPictureBox.TabIndex = 1
			Me.BreakPointIconPictureBox.TabStop = False
			Me.TheToolTipProvider.SetToolTip(Me.BreakPointIconPictureBox, "Dismiss this dialog to resume continous execution...")
			'
			'ContinueExecButton
			'
			Me.ContinueExecButton.Image = Global.DocScript.My.Resources.Resources.ContinueExec
			Me.ContinueExecButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
			Me.ContinueExecButton.Location = New System.Drawing.Point(42, 210)
			Me.ContinueExecButton.Name = "ContinueExecButton"
			Me.ContinueExecButton.Size = New System.Drawing.Size(330, 35)
			Me.ContinueExecButton.TabIndex = 0
			Me.ContinueExecButton.Text = "Continue E&xecution"
			Me.TheToolTipProvider.SetToolTip(Me.ContinueExecButton, "Dismisses this Dialog and resumes execution of the DocScript Program." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Same as pr" & _
				  "essing [Esc].")
			Me.ContinueExecButton.UseVisualStyleBackColor = True
			'
			'EditInDSLiveButton
			'
			Me.EditInDSLiveButton.Enabled = False
			Me.EditInDSLiveButton.Image = CType(resources.GetObject("EditInDSLiveButton.Image"), System.Drawing.Image)
			Me.EditInDSLiveButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
			Me.EditInDSLiveButton.Location = New System.Drawing.Point(42, 169)
			Me.EditInDSLiveButton.Name = "EditInDSLiveButton"
			Me.EditInDSLiveButton.Size = New System.Drawing.Size(330, 35)
			Me.EditInDSLiveButton.TabIndex = 0
			Me.EditInDSLiveButton.Text = "&Edit the Execution-State in a DSLive Session..."
			Me.TheToolTipProvider.SetToolTip(Me.EditInDSLiveButton, "Changes made in the DSLive session ARE re-applied to the running Program.")
			Me.EditInDSLiveButton.UseVisualStyleBackColor = True
			'
			'CopyToDSLiveButton
			'
			Me.CopyToDSLiveButton.Image = CType(resources.GetObject("CopyToDSLiveButton.Image"), System.Drawing.Image)
			Me.CopyToDSLiveButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
			Me.CopyToDSLiveButton.Location = New System.Drawing.Point(42, 128)
			Me.CopyToDSLiveButton.Name = "CopyToDSLiveButton"
			Me.CopyToDSLiveButton.Size = New System.Drawing.Size(330, 35)
			Me.CopyToDSLiveButton.TabIndex = 0
			Me.CopyToDSLiveButton.Text = "&Copy the Execution-State to a DSLive Session..."
			Me.TheToolTipProvider.SetToolTip(Me.CopyToDSLiveButton, resources.GetString("CopyToDSLiveButton.ToolTip"))
			Me.CopyToDSLiveButton.UseVisualStyleBackColor = True
			'
			'ViewSymTblsButton
			'
			Me.ViewSymTblsButton.Image = Global.DocScript.My.Resources.Resources.SymbolTables
			Me.ViewSymTblsButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
			Me.ViewSymTblsButton.Location = New System.Drawing.Point(42, 87)
			Me.ViewSymTblsButton.Name = "ViewSymTblsButton"
			Me.ViewSymTblsButton.Size = New System.Drawing.Size(330, 35)
			Me.ViewSymTblsButton.TabIndex = 0
			Me.ViewSymTblsButton.Text = "View the &Symbol Tables..."
			Me.TheToolTipProvider.SetToolTip(Me.ViewSymTblsButton, "Shows the current values for all visible Variables, Parameters, and Functions." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "J" & _
				  "ust like calling ""Debug_ShowSymbolTables()"".")
			Me.ViewSymTblsButton.UseVisualStyleBackColor = True
			'
			'TheToolTipProvider
			'
			Me.TheToolTipProvider.AutoPopDelay = 500000
			Me.TheToolTipProvider.InitialDelay = 50
			Me.TheToolTipProvider.ReshowDelay = 1
			Me.TheToolTipProvider.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info
			Me.TheToolTipProvider.ToolTipTitle = "Info"
			'
			'IgnoreAllFutureBreakPointsCheckBox
			'
			Me.IgnoreAllFutureBreakPointsCheckBox.AutoSize = True
			Me.IgnoreAllFutureBreakPointsCheckBox.ForeColor = System.Drawing.Color.DimGray
			Me.IgnoreAllFutureBreakPointsCheckBox.Location = New System.Drawing.Point(117, 268)
			Me.IgnoreAllFutureBreakPointsCheckBox.Name = "IgnoreAllFutureBreakPointsCheckBox"
			Me.IgnoreAllFutureBreakPointsCheckBox.Size = New System.Drawing.Size(181, 17)
			Me.IgnoreAllFutureBreakPointsCheckBox.TabIndex = 3
			Me.IgnoreAllFutureBreakPointsCheckBox.Text = "Ignore all breakpoints henceforth"
			Me.TheToolTipProvider.SetToolTip(Me.IgnoreAllFutureBreakPointsCheckBox, "Indicates that calls to Break() should be skipped-over." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "This setting persists on" & _
				  "ly for the current execution.")
			Me.IgnoreAllFutureBreakPointsCheckBox.UseVisualStyleBackColor = True
			'
			'BreakpointActionDialog
			'
			Me.AcceptButton = Me.ContinueExecButton
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(414, 292)
			Me.Controls.Add(Me.IgnoreAllFutureBreakPointsCheckBox)
			Me.Controls.Add(Me.DialogInstructionTextLabel)
			Me.Controls.Add(Me.BreakPointIconPictureBox)
			Me.Controls.Add(Me.ContinueExecButton)
			Me.Controls.Add(Me.EditInDSLiveButton)
			Me.Controls.Add(Me.CopyToDSLiveButton)
			Me.Controls.Add(Me.ViewSymTblsButton)
			Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
			Me.MaximizeBox = False
			Me.MaximumSize = New System.Drawing.Size(430, 330)
			Me.MinimizeBox = False
			Me.MinimumSize = New System.Drawing.Size(430, 330)
			Me.Name = "BreakpointActionDialog"
			Me.ShowIcon = False
			Me.ShowInTaskbar = False
			Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
			Me.Text = "Breakpoint Hit"
			CType(Me.BreakPointIconPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)
			Me.PerformLayout()

		End Sub

		Friend WithEvents ViewSymTblsButton As System.Windows.Forms.Button
		Friend WithEvents CopyToDSLiveButton As System.Windows.Forms.Button
		Friend WithEvents EditInDSLiveButton As System.Windows.Forms.Button
		Friend WithEvents ContinueExecButton As System.Windows.Forms.Button
		Friend WithEvents BreakPointIconPictureBox As System.Windows.Forms.PictureBox
		Friend WithEvents DialogInstructionTextLabel As System.Windows.Forms.Label
		Friend WithEvents TheToolTipProvider As System.Windows.Forms.ToolTip
		Friend WithEvents IgnoreAllFutureBreakPointsCheckBox As System.Windows.Forms.CheckBox

	End Class

End Namespace