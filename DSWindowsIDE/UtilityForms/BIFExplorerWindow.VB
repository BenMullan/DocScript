Public Class BIFExplorerWindow

	Protected NumberOfBIFs_InsertedOntoIDEWindow As UInt16 = 0
	Public ReadOnly Target_IDEWindow As Global.DocScript.MainWindow
	Public ReadOnly Property BuiltInFunctions As DocScript.Runtime.BuiltInFunction()
		Get
			Return Me.Target_IDEWindow.CurrentExecutionContext.BuiltInFunctions.ToArray()
		End Get
	End Property

	Public Sub New(ByRef _Target_IDEWindow As MainWindow)

		' This call is required by the designer.
		InitializeComponent()

		' Add any initialization after the InitializeComponent() call.
		Me.Target_IDEWindow = _Target_IDEWindow
		Me.PopulateListView_FromBIFs_()
		Me.Text = "BuiltInFunction Explorer (" & Me.BuiltInFunctions.Length.ToString() & " BIFs)"

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

	Protected Sub PopulateListView_FromBIFs_()
		Try

			REM The "items" String[] is for each column...
			For Each _BIF As Runtime.BuiltInFunction In Me.BuiltInFunctions
				Me.BIFsListView.Items.Add(New Forms.ListViewItem(items:={
				 _BIF.Identifier,
				 _BIF.Description,
				 Language.Constants.OpeningDataTypeBracket & DocScript.Language.Variables.VariableUtilities.GetDataTypeString_FromDSVariableType(_BIF.ReturnType) & Language.Constants.ClosingDataTypeBracket,
				 String.Join(", ", _BIF.ExpectedParameters.Select(Of String)(Function(_Parameter As Language.Instructions.Statements.DSFunction.Parameter) Language.Constants.OpeningDataTypeBracket & DocScript.Language.Variables.VariableUtilities.GetDataTypeString_FromDSVariableType(_Parameter.DataType) & Language.Constants.ClosingDataTypeBracket & Language.Constants.Space & _Parameter.Identifier))
				 }, imageKey:="BuiltInFunction"))
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
			Me.Target_IDEWindow.InsertTextAtCursor([String].Join(vbCrLf, Me.BIFsListView.SelectedIndices.Cast(Of Int32)().Select(Of String)(Function(_SelectedBIF_Index As Int32) Me.BuiltInFunctions.ElementAt(_SelectedBIF_Index).TemplateCall)) & [vbCrLf])
			Me.NumberOfBIFs_InsertedOntoIDEWindow += Convert.ToUInt16(Me.BIFsListView.SelectedItems.Count)
			Me.Text = "BuiltInFunction Explorer (" & Me.NumberOfBIFs_InsertedOntoIDEWindow.ToString() & " BIF(s) Inserted)"

		Catch _Ex As Exception : MsgBox("The selected BIFs could not be inserted into the SourceText Editor." & vbCrLf & _Ex.Message) : End Try
	End Sub

End Class