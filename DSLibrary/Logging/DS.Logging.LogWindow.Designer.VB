Namespace Logging

	<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
	Partial Class LogWindow : Inherits System.Windows.Forms.Form

		'Form overrides dispose to clean up the component list.
		<System.Diagnostics.DebuggerNonUserCode()> _
		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			Try
				If disposing AndAlso components IsNot Nothing Then components.Dispose()
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
			Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(LogWindow))
			Me.LogWindowMenuStrip = New System.Windows.Forms.MenuStrip()
			Me.FileToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
			Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.toolStripSeparator = New System.Windows.Forms.ToolStripSeparator()
			Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.SaveAsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.toolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
			Me.PrintToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.PrintPreviewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.toolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
			Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.UndoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.RedoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.toolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
			Me.CutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.PasteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.toolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
			Me.SelectAllToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.DeleteAllToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.ToolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.CustomizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.OptionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
			Me.GenerateExampleLogEventsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.LogWindowStatusStrip = New System.Windows.Forms.StatusStrip()
			Me.EventCountLabel = New System.Windows.Forms.ToolStripStatusLabel()
			Me.LogEventsDataGridView = New System.Windows.Forms.DataGridView()
			Me.EventIcon = New System.Windows.Forms.DataGridViewImageColumn()
			Me.EventTime = New System.Windows.Forms.DataGridViewTextBoxColumn()
			Me.EventSeverity = New System.Windows.Forms.DataGridViewTextBoxColumn()
			Me.EventCatagory = New System.Windows.Forms.DataGridViewTextBoxColumn()
			Me.EventMessage = New System.Windows.Forms.DataGridViewTextBoxColumn()
			Me.ShowErrorSeverityCheckBox = New System.Windows.Forms.CheckBox()
			Me.ShowWarningSeverityCheckBox = New System.Windows.Forms.CheckBox()
			Me.ShowInfomationSeverityCheckBox = New System.Windows.Forms.CheckBox()
			Me.ShowVerboseSeverityCheckBox = New System.Windows.Forms.CheckBox()
			Me.ShowDebugSeverityCheckBox = New System.Windows.Forms.CheckBox()
			Me.PlaySoundCheckBox = New System.Windows.Forms.CheckBox()
			Me.LogWindowMenuStrip.SuspendLayout()
			Me.LogWindowStatusStrip.SuspendLayout()
			CType(Me.LogEventsDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'LogWindowMenuStrip
			'
			Me.LogWindowMenuStrip.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
			Me.LogWindowMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem1, Me.EditToolStripMenuItem, Me.ToolsToolStripMenuItem})
			Me.LogWindowMenuStrip.Location = New System.Drawing.Point(0, 0)
			Me.LogWindowMenuStrip.Name = "LogWindowMenuStrip"
			Me.LogWindowMenuStrip.Size = New System.Drawing.Size(784, 24)
			Me.LogWindowMenuStrip.TabIndex = 0
			Me.LogWindowMenuStrip.Text = "Facilitates performing tasks with the Log Data"
			'
			'FileToolStripMenuItem1
			'
			Me.FileToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.OpenToolStripMenuItem, Me.toolStripSeparator, Me.SaveToolStripMenuItem, Me.SaveAsToolStripMenuItem, Me.toolStripSeparator1, Me.PrintToolStripMenuItem, Me.PrintPreviewToolStripMenuItem, Me.toolStripSeparator2, Me.ExitToolStripMenuItem})
			Me.FileToolStripMenuItem1.Name = "FileToolStripMenuItem1"
			Me.FileToolStripMenuItem1.Size = New System.Drawing.Size(37, 20)
			Me.FileToolStripMenuItem1.Text = "&File"
			'
			'NewToolStripMenuItem
			'
			Me.NewToolStripMenuItem.Image = CType(resources.GetObject("NewToolStripMenuItem.Image"), System.Drawing.Image)
			Me.NewToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
			Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
			Me.NewToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
			Me.NewToolStripMenuItem.Size = New System.Drawing.Size(146, 22)
			Me.NewToolStripMenuItem.Text = "&New"
			'
			'OpenToolStripMenuItem
			'
			Me.OpenToolStripMenuItem.Image = CType(resources.GetObject("OpenToolStripMenuItem.Image"), System.Drawing.Image)
			Me.OpenToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
			Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
			Me.OpenToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
			Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(146, 22)
			Me.OpenToolStripMenuItem.Text = "&Open"
			'
			'toolStripSeparator
			'
			Me.toolStripSeparator.Name = "toolStripSeparator"
			Me.toolStripSeparator.Size = New System.Drawing.Size(143, 6)
			'
			'SaveToolStripMenuItem
			'
			Me.SaveToolStripMenuItem.Image = CType(resources.GetObject("SaveToolStripMenuItem.Image"), System.Drawing.Image)
			Me.SaveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
			Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
			Me.SaveToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
			Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(146, 22)
			Me.SaveToolStripMenuItem.Text = "&Save"
			'
			'SaveAsToolStripMenuItem
			'
			Me.SaveAsToolStripMenuItem.Name = "SaveAsToolStripMenuItem"
			Me.SaveAsToolStripMenuItem.Size = New System.Drawing.Size(146, 22)
			Me.SaveAsToolStripMenuItem.Text = "Save &As"
			'
			'toolStripSeparator1
			'
			Me.toolStripSeparator1.Name = "toolStripSeparator1"
			Me.toolStripSeparator1.Size = New System.Drawing.Size(143, 6)
			'
			'PrintToolStripMenuItem
			'
			Me.PrintToolStripMenuItem.Image = CType(resources.GetObject("PrintToolStripMenuItem.Image"), System.Drawing.Image)
			Me.PrintToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
			Me.PrintToolStripMenuItem.Name = "PrintToolStripMenuItem"
			Me.PrintToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
			Me.PrintToolStripMenuItem.Size = New System.Drawing.Size(146, 22)
			Me.PrintToolStripMenuItem.Text = "&Print"
			'
			'PrintPreviewToolStripMenuItem
			'
			Me.PrintPreviewToolStripMenuItem.Image = CType(resources.GetObject("PrintPreviewToolStripMenuItem.Image"), System.Drawing.Image)
			Me.PrintPreviewToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
			Me.PrintPreviewToolStripMenuItem.Name = "PrintPreviewToolStripMenuItem"
			Me.PrintPreviewToolStripMenuItem.Size = New System.Drawing.Size(146, 22)
			Me.PrintPreviewToolStripMenuItem.Text = "Print Pre&view"
			'
			'toolStripSeparator2
			'
			Me.toolStripSeparator2.Name = "toolStripSeparator2"
			Me.toolStripSeparator2.Size = New System.Drawing.Size(143, 6)
			'
			'ExitToolStripMenuItem
			'
			Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
			Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(146, 22)
			Me.ExitToolStripMenuItem.Text = "E&xit"
			'
			'EditToolStripMenuItem
			'
			Me.EditToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.UndoToolStripMenuItem, Me.RedoToolStripMenuItem, Me.toolStripSeparator3, Me.CutToolStripMenuItem, Me.CopyToolStripMenuItem, Me.PasteToolStripMenuItem, Me.toolStripSeparator4, Me.SelectAllToolStripMenuItem, Me.DeleteAllToolStripMenuItem})
			Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
			Me.EditToolStripMenuItem.Size = New System.Drawing.Size(39, 20)
			Me.EditToolStripMenuItem.Text = "&Edit"
			'
			'UndoToolStripMenuItem
			'
			Me.UndoToolStripMenuItem.Name = "UndoToolStripMenuItem"
			Me.UndoToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Z), System.Windows.Forms.Keys)
			Me.UndoToolStripMenuItem.Size = New System.Drawing.Size(144, 22)
			Me.UndoToolStripMenuItem.Text = "&Undo"
			'
			'RedoToolStripMenuItem
			'
			Me.RedoToolStripMenuItem.Name = "RedoToolStripMenuItem"
			Me.RedoToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Y), System.Windows.Forms.Keys)
			Me.RedoToolStripMenuItem.Size = New System.Drawing.Size(144, 22)
			Me.RedoToolStripMenuItem.Text = "&Redo"
			'
			'toolStripSeparator3
			'
			Me.toolStripSeparator3.Name = "toolStripSeparator3"
			Me.toolStripSeparator3.Size = New System.Drawing.Size(141, 6)
			'
			'CutToolStripMenuItem
			'
			Me.CutToolStripMenuItem.Image = CType(resources.GetObject("CutToolStripMenuItem.Image"), System.Drawing.Image)
			Me.CutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
			Me.CutToolStripMenuItem.Name = "CutToolStripMenuItem"
			Me.CutToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
			Me.CutToolStripMenuItem.Size = New System.Drawing.Size(144, 22)
			Me.CutToolStripMenuItem.Text = "Cu&t"
			'
			'CopyToolStripMenuItem
			'
			Me.CopyToolStripMenuItem.Image = CType(resources.GetObject("CopyToolStripMenuItem.Image"), System.Drawing.Image)
			Me.CopyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
			Me.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
			Me.CopyToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
			Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(144, 22)
			Me.CopyToolStripMenuItem.Text = "&Copy"
			'
			'PasteToolStripMenuItem
			'
			Me.PasteToolStripMenuItem.Image = CType(resources.GetObject("PasteToolStripMenuItem.Image"), System.Drawing.Image)
			Me.PasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
			Me.PasteToolStripMenuItem.Name = "PasteToolStripMenuItem"
			Me.PasteToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
			Me.PasteToolStripMenuItem.Size = New System.Drawing.Size(144, 22)
			Me.PasteToolStripMenuItem.Text = "&Paste"
			'
			'toolStripSeparator4
			'
			Me.toolStripSeparator4.Name = "toolStripSeparator4"
			Me.toolStripSeparator4.Size = New System.Drawing.Size(141, 6)
			'
			'SelectAllToolStripMenuItem
			'
			Me.SelectAllToolStripMenuItem.Name = "SelectAllToolStripMenuItem"
			Me.SelectAllToolStripMenuItem.Size = New System.Drawing.Size(144, 22)
			Me.SelectAllToolStripMenuItem.Text = "Select &All"
			'
			'DeleteAllToolStripMenuItem
			'
			Me.DeleteAllToolStripMenuItem.Image = Global.DocScript.My.Resources.Resources.Delete
			Me.DeleteAllToolStripMenuItem.Name = "DeleteAllToolStripMenuItem"
			Me.DeleteAllToolStripMenuItem.Size = New System.Drawing.Size(144, 22)
			Me.DeleteAllToolStripMenuItem.Text = "&Delete All"
			'
			'ToolsToolStripMenuItem
			'
			Me.ToolsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CustomizeToolStripMenuItem, Me.OptionsToolStripMenuItem, Me.ToolStripSeparator5, Me.GenerateExampleLogEventsToolStripMenuItem})
			Me.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem"
			Me.ToolsToolStripMenuItem.Size = New System.Drawing.Size(48, 20)
			Me.ToolsToolStripMenuItem.Text = "&Tools"
			'
			'CustomizeToolStripMenuItem
			'
			Me.CustomizeToolStripMenuItem.Name = "CustomizeToolStripMenuItem"
			Me.CustomizeToolStripMenuItem.Size = New System.Drawing.Size(225, 22)
			Me.CustomizeToolStripMenuItem.Text = "&Customize"
			'
			'OptionsToolStripMenuItem
			'
			Me.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem"
			Me.OptionsToolStripMenuItem.Size = New System.Drawing.Size(225, 22)
			Me.OptionsToolStripMenuItem.Text = "&Options"
			'
			'ToolStripSeparator5
			'
			Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
			Me.ToolStripSeparator5.Size = New System.Drawing.Size(222, 6)
			'
			'GenerateExampleLogEventsToolStripMenuItem
			'
			Me.GenerateExampleLogEventsToolStripMenuItem.Name = "GenerateExampleLogEventsToolStripMenuItem"
			Me.GenerateExampleLogEventsToolStripMenuItem.Size = New System.Drawing.Size(225, 22)
			Me.GenerateExampleLogEventsToolStripMenuItem.Text = "Generate Example LogEvents"
			'
			'LogWindowStatusStrip
			'
			Me.LogWindowStatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EventCountLabel})
			Me.LogWindowStatusStrip.Location = New System.Drawing.Point(0, 440)
			Me.LogWindowStatusStrip.Name = "LogWindowStatusStrip"
			Me.LogWindowStatusStrip.Size = New System.Drawing.Size(784, 22)
			Me.LogWindowStatusStrip.TabIndex = 7
			Me.LogWindowStatusStrip.Text = "StatusStrip1"
			'
			'EventCountLabel
			'
			Me.EventCountLabel.Name = "EventCountLabel"
			Me.EventCountLabel.Size = New System.Drawing.Size(52, 17)
			Me.EventCountLabel.Text = "Count: 0"
			'
			'LogEventsDataGridView
			'
			Me.LogEventsDataGridView.AllowUserToAddRows = False
			Me.LogEventsDataGridView.AllowUserToDeleteRows = False
			Me.LogEventsDataGridView.AllowUserToOrderColumns = True
			Me.LogEventsDataGridView.BackgroundColor = System.Drawing.Color.LightGray
			Me.LogEventsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
			Me.LogEventsDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.EventIcon, Me.EventTime, Me.EventSeverity, Me.EventCatagory, Me.EventMessage})
			Me.LogEventsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill
			Me.LogEventsDataGridView.Location = New System.Drawing.Point(0, 24)
			Me.LogEventsDataGridView.Name = "LogEventsDataGridView"
			Me.LogEventsDataGridView.ReadOnly = True
			Me.LogEventsDataGridView.RowHeadersWidth = 10
			Me.LogEventsDataGridView.Size = New System.Drawing.Size(784, 416)
			Me.LogEventsDataGridView.TabIndex = 6
			'
			'EventIcon
			'
			Me.EventIcon.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
			Me.EventIcon.FillWeight = 3.0!
			Me.EventIcon.HeaderText = "Icon"
			Me.EventIcon.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
			Me.EventIcon.Name = "EventIcon"
			Me.EventIcon.ReadOnly = True
			Me.EventIcon.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
			Me.EventIcon.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
			'
			'EventTime
			'
			Me.EventTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
			Me.EventTime.FillWeight = 10.0!
			Me.EventTime.HeaderText = "Time"
			Me.EventTime.Name = "EventTime"
			Me.EventTime.ReadOnly = True
			'
			'EventSeverity
			'
			Me.EventSeverity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
			Me.EventSeverity.FillWeight = 8.0!
			Me.EventSeverity.HeaderText = "Severity"
			Me.EventSeverity.Name = "EventSeverity"
			Me.EventSeverity.ReadOnly = True
			'
			'EventCatagory
			'
			Me.EventCatagory.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
			Me.EventCatagory.FillWeight = 6.0!
			Me.EventCatagory.HeaderText = "Catagory"
			Me.EventCatagory.Name = "EventCatagory"
			Me.EventCatagory.ReadOnly = True
			'
			'EventMessage
			'
			Me.EventMessage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
			Me.EventMessage.FillWeight = 73.0!
			Me.EventMessage.HeaderText = "Message"
			Me.EventMessage.Name = "EventMessage"
			Me.EventMessage.ReadOnly = True
			'
			'ShowErrorSeverityCheckBox
			'
			Me.ShowErrorSeverityCheckBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.ShowErrorSeverityCheckBox.AutoSize = True
			Me.ShowErrorSeverityCheckBox.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
			Me.ShowErrorSeverityCheckBox.Checked = True
			Me.ShowErrorSeverityCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
			Me.ShowErrorSeverityCheckBox.Location = New System.Drawing.Point(445, 5)
			Me.ShowErrorSeverityCheckBox.Name = "ShowErrorSeverityCheckBox"
			Me.ShowErrorSeverityCheckBox.Size = New System.Drawing.Size(48, 17)
			Me.ShowErrorSeverityCheckBox.TabIndex = 1
			Me.ShowErrorSeverityCheckBox.Tag = ""
			Me.ShowErrorSeverityCheckBox.Text = "Error"
			Me.ShowErrorSeverityCheckBox.UseVisualStyleBackColor = False
			'
			'ShowWarningSeverityCheckBox
			'
			Me.ShowWarningSeverityCheckBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.ShowWarningSeverityCheckBox.AutoSize = True
			Me.ShowWarningSeverityCheckBox.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
			Me.ShowWarningSeverityCheckBox.Checked = True
			Me.ShowWarningSeverityCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
			Me.ShowWarningSeverityCheckBox.Location = New System.Drawing.Point(499, 5)
			Me.ShowWarningSeverityCheckBox.Name = "ShowWarningSeverityCheckBox"
			Me.ShowWarningSeverityCheckBox.Size = New System.Drawing.Size(66, 17)
			Me.ShowWarningSeverityCheckBox.TabIndex = 2
			Me.ShowWarningSeverityCheckBox.Text = "Warning"
			Me.ShowWarningSeverityCheckBox.UseVisualStyleBackColor = False
			'
			'ShowInfomationSeverityCheckBox
			'
			Me.ShowInfomationSeverityCheckBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.ShowInfomationSeverityCheckBox.AutoSize = True
			Me.ShowInfomationSeverityCheckBox.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
			Me.ShowInfomationSeverityCheckBox.Checked = True
			Me.ShowInfomationSeverityCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
			Me.ShowInfomationSeverityCheckBox.Location = New System.Drawing.Point(571, 5)
			Me.ShowInfomationSeverityCheckBox.Name = "ShowInfomationSeverityCheckBox"
			Me.ShowInfomationSeverityCheckBox.Size = New System.Drawing.Size(75, 17)
			Me.ShowInfomationSeverityCheckBox.TabIndex = 3
			Me.ShowInfomationSeverityCheckBox.Text = "Infomation"
			Me.ShowInfomationSeverityCheckBox.UseVisualStyleBackColor = False
			'
			'ShowVerboseSeverityCheckBox
			'
			Me.ShowVerboseSeverityCheckBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.ShowVerboseSeverityCheckBox.AutoSize = True
			Me.ShowVerboseSeverityCheckBox.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
			Me.ShowVerboseSeverityCheckBox.Checked = True
			Me.ShowVerboseSeverityCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
			Me.ShowVerboseSeverityCheckBox.Location = New System.Drawing.Point(652, 5)
			Me.ShowVerboseSeverityCheckBox.Name = "ShowVerboseSeverityCheckBox"
			Me.ShowVerboseSeverityCheckBox.Size = New System.Drawing.Size(65, 17)
			Me.ShowVerboseSeverityCheckBox.TabIndex = 4
			Me.ShowVerboseSeverityCheckBox.Text = "Verbose"
			Me.ShowVerboseSeverityCheckBox.UseVisualStyleBackColor = False
			'
			'ShowDebugSeverityCheckBox
			'
			Me.ShowDebugSeverityCheckBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.ShowDebugSeverityCheckBox.AutoSize = True
			Me.ShowDebugSeverityCheckBox.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
			Me.ShowDebugSeverityCheckBox.Location = New System.Drawing.Point(723, 5)
			Me.ShowDebugSeverityCheckBox.Name = "ShowDebugSeverityCheckBox"
			Me.ShowDebugSeverityCheckBox.Size = New System.Drawing.Size(58, 17)
			Me.ShowDebugSeverityCheckBox.TabIndex = 5
			Me.ShowDebugSeverityCheckBox.Text = "Debug"
			Me.ShowDebugSeverityCheckBox.UseVisualStyleBackColor = False
			'
			'PlaySoundCheckBox
			'
			Me.PlaySoundCheckBox.AutoSize = True
			Me.PlaySoundCheckBox.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
			Me.PlaySoundCheckBox.Location = New System.Drawing.Point(133, 5)
			Me.PlaySoundCheckBox.Name = "PlaySoundCheckBox"
			Me.PlaySoundCheckBox.Size = New System.Drawing.Size(57, 17)
			Me.PlaySoundCheckBox.TabIndex = 8
			Me.PlaySoundCheckBox.Text = "Sound"
			Me.PlaySoundCheckBox.UseVisualStyleBackColor = False
			'
			'LogWindow
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(784, 462)
			Me.Controls.Add(Me.PlaySoundCheckBox)
			Me.Controls.Add(Me.ShowDebugSeverityCheckBox)
			Me.Controls.Add(Me.ShowVerboseSeverityCheckBox)
			Me.Controls.Add(Me.ShowInfomationSeverityCheckBox)
			Me.Controls.Add(Me.ShowWarningSeverityCheckBox)
			Me.Controls.Add(Me.ShowErrorSeverityCheckBox)
			Me.Controls.Add(Me.LogEventsDataGridView)
			Me.Controls.Add(Me.LogWindowStatusStrip)
			Me.Controls.Add(Me.LogWindowMenuStrip)
			Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
			Me.MainMenuStrip = Me.LogWindowMenuStrip
			Me.MinimumSize = New System.Drawing.Size(600, 200)
			Me.Name = "LogWindow"
			Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
			Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds
			Me.Text = "DocScript Log Window"
			Me.LogWindowMenuStrip.ResumeLayout(False)
			Me.LogWindowMenuStrip.PerformLayout()
			Me.LogWindowStatusStrip.ResumeLayout(False)
			Me.LogWindowStatusStrip.PerformLayout()
			CType(Me.LogEventsDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)
			Me.PerformLayout()

		End Sub

		Friend WithEvents LogWindowMenuStrip As System.Windows.Forms.MenuStrip
		Friend WithEvents LogWindowStatusStrip As System.Windows.Forms.StatusStrip
		Friend WithEvents LogEventsDataGridView As System.Windows.Forms.DataGridView
		Friend WithEvents FileToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
		Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Friend WithEvents toolStripSeparator As System.Windows.Forms.ToolStripSeparator
		Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Friend WithEvents SaveAsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Friend WithEvents toolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
		Friend WithEvents PrintToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Friend WithEvents PrintPreviewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Friend WithEvents toolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
		Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Friend WithEvents EditToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Friend WithEvents UndoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Friend WithEvents RedoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Friend WithEvents toolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
		Friend WithEvents CutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Friend WithEvents CopyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Friend WithEvents PasteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Friend WithEvents toolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
		Friend WithEvents SelectAllToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Friend WithEvents ToolsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Friend WithEvents CustomizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Friend WithEvents OptionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Friend WithEvents EventCountLabel As System.Windows.Forms.ToolStripStatusLabel
		Friend WithEvents ShowErrorSeverityCheckBox As System.Windows.Forms.CheckBox
		Friend WithEvents ShowWarningSeverityCheckBox As System.Windows.Forms.CheckBox
		Friend WithEvents ShowInfomationSeverityCheckBox As System.Windows.Forms.CheckBox
		Friend WithEvents ShowVerboseSeverityCheckBox As System.Windows.Forms.CheckBox
		Friend WithEvents ShowDebugSeverityCheckBox As System.Windows.Forms.CheckBox
		Friend WithEvents EventIcon As System.Windows.Forms.DataGridViewImageColumn
		Friend WithEvents EventTime As System.Windows.Forms.DataGridViewTextBoxColumn
		Friend WithEvents EventSeverity As System.Windows.Forms.DataGridViewTextBoxColumn
		Friend WithEvents EventCatagory As System.Windows.Forms.DataGridViewTextBoxColumn
		Friend WithEvents EventMessage As System.Windows.Forms.DataGridViewTextBoxColumn
		Friend WithEvents DeleteAllToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Friend WithEvents PlaySoundCheckBox As System.Windows.Forms.CheckBox
		Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
		Friend WithEvents GenerateExampleLogEventsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

	End Class

End Namespace