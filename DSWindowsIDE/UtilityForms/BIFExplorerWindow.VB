Public Class BIFExplorerWindow

	Protected NumberOfBIFs_InsertedOntoIDEWindow As UInt16 = 0
	Public ReadOnly Target_IDEWindowContent As Global.DocScript.WindowsIDE.IDEWindowContent
	Public ReadOnly Property AllBuiltInFunctions_FromMainWindow As DocScript.Runtime.BuiltInFunction()
		Get
			Return Me.Target_IDEWindowContent.CurrentExecutionContext.BuiltInFunctions.ToArray()
		End Get
	End Property

	Public Sub New(ByRef _Target_IDEWindowContent As IDEWindowContent)

		' This call is required by the designer.
		InitializeComponent()

		' Add any initialization after the InitializeComponent() call.
		Me.Target_IDEWindowContent = _Target_IDEWindowContent
		Me.PopulateListView_ForTheseBIFs_(Me.AllBuiltInFunctions_FromMainWindow)
		Me.Text = "BuiltInFunction Explorer (" & Me.AllBuiltInFunctions_FromMainWindow.Length.ToString() & " BIFs)"

	End Sub

	Public Sub InitialiseWindow() Handles Me.Shown
		Me.FilterTextBox.Focus()
	End Sub

	Public Sub FilterBIFsBySearchText() Handles FilterTextBox.TextChanged
		Me.BIFsListView.Items.Clear()
		Me.PopulateListView_ForTheseBIFs_(
		 Me.AllBuiltInFunctions_FromMainWindow.Where(Function(_BIF As Runtime.BuiltInFunction) _BIF.Identifier.ToUpper().Contains(Me.FilterTextBox.Text.ToUpper())).ToArray()
		)
	End Sub

#Region "Close the form on pressing [Esc]"

	Protected Sub Register_CloseOnEscKey_Handler() Handles Me.Load
		Me.KeyPreview = True
		AddHandler Me.KeyDown, Sub(_Sender As Object, _KeyEventArgs As Forms.KeyEventArgs) If _KeyEventArgs.KeyCode = Forms.Keys.Escape Then Me.Close()
	End Sub

	Protected Overrides Function ProcessDialogKey(keyData As System.Windows.Forms.Keys) As Boolean
		If (Windows.Forms.Form.ModifierKeys = Forms.Keys.None) AndAlso (keyData = Forms.Keys.Escape) Then
			Me.Close() : Return True
		End If
		Return MyBase.ProcessDialogKey(keyData)
	End Function

#End Region

	Protected Sub PopulateListView_ForTheseBIFs_(ByRef _BIFs As Runtime.BuiltInFunction())
		Try

			REM The "items" String[] is for each column...
			For Each _BIF As Runtime.BuiltInFunction In _BIFs
				Me.BIFsListView.Items.Add(
				 New Forms.ListViewItem(
				 items:={
				   _BIF.Identifier,
				   _BIF.Description,
				   Language.Constants.OpeningDataTypeBracket & DocScript.Language.Variables.VariableUtilities.GetDataTypeString_FromDSVariableType(_BIF.ReturnType) & Language.Constants.ClosingDataTypeBracket,
				   [String].Join(", ", _BIF.ExpectedParameters.Select(Of [String])(Function(_Parameter As Language.Instructions.Statements.DSFunction.Parameter) Language.Constants.OpeningDataTypeBracket & DocScript.Language.Variables.VariableUtilities.GetDataTypeString_FromDSVariableType(_Parameter.DataType) & Language.Constants.ClosingDataTypeBracket & Language.Constants.Space & _Parameter.Identifier))
				   },
				   imageKey:="BuiltInFunction"
				   ) With {.ToolTipText = _BIF.TemplateCall}
				  )
			Next

		Catch _Ex As Exception
			MsgBox("On Adding the BIFs to the ListView for a [BIF Explorer]:" & vbCrLf & vbCrLf & _Ex.Message, MsgBoxStyle.Critical, _Ex.GetType().Name)
		End Try
	End Sub

	Private Sub BIFsListView_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles BIFsListView.KeyDown
		If e.KeyCode = Forms.Keys.Enter Then Me.InsertSelectedBIF_IntoSourceTextEditor()
	End Sub

	Public Sub InsertSelectedBIF_IntoSourceTextEditor() Handles BIFsListView.MouseDoubleClick
		Try

			REM Join()-ing by CrLf puts each Call on a new Line
			Me.Target_IDEWindowContent.InsertTextAtCursor([String].Join(vbCrLf, Me.BIFsListView.SelectedItems.Cast(Of Forms.ListViewItem)().Select(Of [String])(Function(_SelectedBIFListViewRow As Forms.ListViewItem) Me.AllBuiltInFunctions_FromMainWindow.First(Function(_BIF As Runtime.BuiltInFunction) _BIF.Identifier.ToUpper() = _SelectedBIFListViewRow.SubItems.Item(index:=0).Text.ToUpper()).TemplateCall)) & [vbCrLf])
			Me.NumberOfBIFs_InsertedOntoIDEWindow += Convert.ToUInt16(Me.BIFsListView.SelectedItems.Count)
			Me.Text = "BuiltInFunction Explorer (" & Me.NumberOfBIFs_InsertedOntoIDEWindow.ToString() & " BIF(s) Inserted)"

		Catch _Ex As Exception : MsgBox("The selected BIFs could not be inserted into the SourceText Editor." & vbCrLf & _Ex.Message) : End Try
	End Sub

End Class