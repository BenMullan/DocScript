Namespace Runtime

	<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
	Partial Public Class SymbolTableExplorerWindow
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
			Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SymbolTableExplorerWindow))
			Me.ContinueButton = New System.Windows.Forms.Button()
			Me.SymbolTables_TabControl = New System.Windows.Forms.TabControl()
			Me.TheImageList = New System.Windows.Forms.ImageList(Me.components)
			Me.TheMainMenu = New System.Windows.Forms.MainMenu(Me.components)
			Me.SaveAsMenuItem = New System.Windows.Forms.MenuItem()
			Me.OpenMenuItem = New System.Windows.Forms.MenuItem()
			Me.SuspendLayout()
			'
			'ContinueButton
			'
			Me.ContinueButton.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
					  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.ContinueButton.Location = New System.Drawing.Point(230, 327)
			Me.ContinueButton.Name = "ContinueButton"
			Me.ContinueButton.Size = New System.Drawing.Size(275, 23)
			Me.ContinueButton.TabIndex = 0
			Me.ContinueButton.Text = "&Continue"
			Me.ContinueButton.UseVisualStyleBackColor = True
			'
			'SymbolTables_TabControl
			'
			Me.SymbolTables_TabControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
					  Or System.Windows.Forms.AnchorStyles.Left) _
					  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.SymbolTables_TabControl.ImageList = Me.TheImageList
			Me.SymbolTables_TabControl.Location = New System.Drawing.Point(12, 12)
			Me.SymbolTables_TabControl.Name = "SymbolTables_TabControl"
			Me.SymbolTables_TabControl.SelectedIndex = 0
			Me.SymbolTables_TabControl.Size = New System.Drawing.Size(710, 309)
			Me.SymbolTables_TabControl.TabIndex = 1
			'
			'TheImageList
			'
			Me.TheImageList.ImageStream = CType(resources.GetObject("TheImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
			Me.TheImageList.TransparentColor = System.Drawing.Color.Transparent
			Me.TheImageList.Images.SetKeyName(0, "SymbolTable")
			Me.TheImageList.Images.SetKeyName(1, "Identifier")
			Me.TheImageList.Images.SetKeyName(2, "Value")
			Me.TheImageList.Images.SetKeyName(3, "Type")
			Me.TheImageList.Images.SetKeyName(4, "IDataValue")
			Me.TheImageList.Images.SetKeyName(5, "Function")
			'
			'TheMainMenu
			'
			Me.TheMainMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.SaveAsMenuItem, Me.OpenMenuItem})
			'
			'SaveAsMenuItem
			'
			Me.SaveAsMenuItem.Index = 0
			Me.SaveAsMenuItem.Text = "&Save As..."
			'
			'OpenMenuItem
			'
			Me.OpenMenuItem.Index = 1
			Me.OpenMenuItem.Text = "&Open..."
			'
			'SymbolTableExplorerWindow
			'
			Me.AcceptButton = Me.ContinueButton
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(734, 362)
			Me.Controls.Add(Me.SymbolTables_TabControl)
			Me.Controls.Add(Me.ContinueButton)
			Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
			Me.Menu = Me.TheMainMenu
			Me.MinimumSize = New System.Drawing.Size(550, 150)
			Me.Name = "SymbolTableExplorerWindow"
			Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
			Me.Text = "SymbolTable Explorer"
			Me.ResumeLayout(False)

		End Sub

		Friend WithEvents ContinueButton As System.Windows.Forms.Button
		Friend WithEvents SymbolTables_TabControl As System.Windows.Forms.TabControl
		Friend WithEvents TheImageList As System.Windows.Forms.ImageList
		Friend WithEvents TheMainMenu As System.Windows.Forms.MainMenu
		Friend WithEvents SaveAsMenuItem As System.Windows.Forms.MenuItem
		Friend WithEvents OpenMenuItem As System.Windows.Forms.MenuItem

	End Class

End Namespace