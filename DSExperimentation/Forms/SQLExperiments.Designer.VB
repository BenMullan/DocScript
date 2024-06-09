<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SQLExperiments
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
		Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
		Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
		Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SQLExperiments))
		Me.ExecQueryButton = New System.Windows.Forms.Button()
		Me.SQLQueryTextBox = New System.Windows.Forms.TextBox()
		Me.SQLOutput_DataGridView = New System.Windows.Forms.DataGridView()
		Me.QueryWatchButton = New System.Windows.Forms.Button()
		Me.ExecTenButton = New System.Windows.Forms.Button()
		Me.DseFormChooser1 = New DSExperimentation.DSEFormChooser()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.QueryWatchDBTextBox = New System.Windows.Forms.TextBox()
		Me.Label3 = New System.Windows.Forms.Label()
		Me.SQLServerTextBox = New System.Windows.Forms.TextBox()
		Me.TBFEButton = New System.Windows.Forms.Button()
		CType(Me.SQLOutput_DataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'ExecQueryButton
		'
		Me.ExecQueryButton.Location = New System.Drawing.Point(588, 12)
		Me.ExecQueryButton.Name = "ExecQueryButton"
		Me.ExecQueryButton.Size = New System.Drawing.Size(55, 23)
		Me.ExecQueryButton.TabIndex = 0
		Me.ExecQueryButton.Text = "Execute"
		Me.ExecQueryButton.UseVisualStyleBackColor = True
		'
		'SQLQueryTextBox
		'
		Me.SQLQueryTextBox.Location = New System.Drawing.Point(212, 14)
		Me.SQLQueryTextBox.Name = "SQLQueryTextBox"
		Me.SQLQueryTextBox.Size = New System.Drawing.Size(370, 20)
		Me.SQLQueryTextBox.TabIndex = 2
		Me.SQLQueryTextBox.Text = "SELECT ""Name"" FROM [dbo].[NewPeople];"
		'
		'SQLOutput_DataGridView
		'
		Me.SQLOutput_DataGridView.AllowUserToAddRows = False
		Me.SQLOutput_DataGridView.AllowUserToDeleteRows = False
		Me.SQLOutput_DataGridView.BackgroundColor = System.Drawing.SystemColors.GradientInactiveCaption
		DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
		DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
		DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
		DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
		DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
		DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
		Me.SQLOutput_DataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
		Me.SQLOutput_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
		DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
		DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
		DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
		DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
		DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
		Me.SQLOutput_DataGridView.DefaultCellStyle = DataGridViewCellStyle2
		Me.SQLOutput_DataGridView.Location = New System.Drawing.Point(168, 99)
		Me.SQLOutput_DataGridView.Name = "SQLOutput_DataGridView"
		Me.SQLOutput_DataGridView.ReadOnly = True
		DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
		DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
		DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
		DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
		DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
		DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
		Me.SQLOutput_DataGridView.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
		Me.SQLOutput_DataGridView.Size = New System.Drawing.Size(475, 162)
		Me.SQLOutput_DataGridView.StandardTab = True
		Me.SQLOutput_DataGridView.TabIndex = 3
		'
		'QueryWatchButton
		'
		Me.QueryWatchButton.Location = New System.Drawing.Point(445, 40)
		Me.QueryWatchButton.Name = "QueryWatchButton"
		Me.QueryWatchButton.Size = New System.Drawing.Size(85, 23)
		Me.QueryWatchButton.TabIndex = 4
		Me.QueryWatchButton.Text = "QueryWatch"
		Me.QueryWatchButton.UseVisualStyleBackColor = True
		'
		'ExecTenButton
		'
		Me.ExecTenButton.Location = New System.Drawing.Point(536, 40)
		Me.ExecTenButton.Name = "ExecTenButton"
		Me.ExecTenButton.Size = New System.Drawing.Size(107, 23)
		Me.ExecTenButton.TabIndex = 5
		Me.ExecTenButton.Text = "Execute 10 Times"
		Me.ExecTenButton.UseVisualStyleBackColor = True
		'
		'DseFormChooser1
		'
		Me.DseFormChooser1.AutoScroll = True
		Me.DseFormChooser1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
		Me.DseFormChooser1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.DseFormChooser1.Location = New System.Drawing.Point(12, 12)
		Me.DseFormChooser1.Name = "DseFormChooser1"
		Me.DseFormChooser1.Size = New System.Drawing.Size(150, 249)
		Me.DseFormChooser1.TabIndex = 1
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Location = New System.Drawing.Point(168, 45)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(88, 13)
		Me.Label1.TabIndex = 6
		Me.Label1.Text = "QueryWatch DB:"
		'
		'Label2
		'
		Me.Label2.AutoSize = True
		Me.Label2.Location = New System.Drawing.Point(168, 17)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(38, 13)
		Me.Label2.TabIndex = 6
		Me.Label2.Text = "Query:"
		'
		'QueryWatchDBTextBox
		'
		Me.QueryWatchDBTextBox.Location = New System.Drawing.Point(262, 42)
		Me.QueryWatchDBTextBox.Name = "QueryWatchDBTextBox"
		Me.QueryWatchDBTextBox.Size = New System.Drawing.Size(143, 20)
		Me.QueryWatchDBTextBox.TabIndex = 7
		Me.QueryWatchDBTextBox.Text = "QueryWatchTest"
		'
		'Label3
		'
		Me.Label3.AutoSize = True
		Me.Label3.Location = New System.Drawing.Point(168, 76)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(65, 13)
		Me.Label3.TabIndex = 6
		Me.Label3.Text = "SQL Server:"
		'
		'SQLServerTextBox
		'
		Me.SQLServerTextBox.Location = New System.Drawing.Point(239, 73)
		Me.SQLServerTextBox.Name = "SQLServerTextBox"
		Me.SQLServerTextBox.Size = New System.Drawing.Size(166, 20)
		Me.SQLServerTextBox.TabIndex = 8
		Me.SQLServerTextBox.Text = "MNLT01\SQLEXPRESS"
		'
		'TBFEButton
		'
		Me.TBFEButton.Location = New System.Drawing.Point(445, 69)
		Me.TBFEButton.Name = "TBFEButton"
		Me.TBFEButton.Size = New System.Drawing.Size(121, 23)
		Me.TBFEButton.TabIndex = 9
		Me.TBFEButton.Text = "TakeBlockFromEnd()"
		Me.TBFEButton.UseVisualStyleBackColor = True
		'
		'SQLExperiments
		'
		Me.AcceptButton = Me.ExecQueryButton
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(655, 273)
		Me.Controls.Add(Me.TBFEButton)
		Me.Controls.Add(Me.SQLServerTextBox)
		Me.Controls.Add(Me.QueryWatchDBTextBox)
		Me.Controls.Add(Me.Label2)
		Me.Controls.Add(Me.Label3)
		Me.Controls.Add(Me.Label1)
		Me.Controls.Add(Me.ExecTenButton)
		Me.Controls.Add(Me.QueryWatchButton)
		Me.Controls.Add(Me.SQLOutput_DataGridView)
		Me.Controls.Add(Me.SQLQueryTextBox)
		Me.Controls.Add(Me.DseFormChooser1)
		Me.Controls.Add(Me.ExecQueryButton)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.Name = "SQLExperiments"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "SQLExperiments"
		Me.TransparencyKey = System.Drawing.SystemColors.AppWorkspace
		CType(Me.SQLOutput_DataGridView, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub

	Friend WithEvents ExecQueryButton As System.Windows.Forms.Button
	Friend WithEvents DseFormChooser1 As DSExperimentation.DSEFormChooser
	Friend WithEvents SQLQueryTextBox As System.Windows.Forms.TextBox
	Friend WithEvents SQLOutput_DataGridView As System.Windows.Forms.DataGridView
	Friend WithEvents QueryWatchButton As System.Windows.Forms.Button
	Friend WithEvents ExecTenButton As System.Windows.Forms.Button
	Friend WithEvents Label1 As System.Windows.Forms.Label
	Friend WithEvents Label2 As System.Windows.Forms.Label
	Friend WithEvents QueryWatchDBTextBox As System.Windows.Forms.TextBox
	Friend WithEvents Label3 As System.Windows.Forms.Label
	Friend WithEvents SQLServerTextBox As System.Windows.Forms.TextBox
	Friend WithEvents TBFEButton As System.Windows.Forms.Button

End Class