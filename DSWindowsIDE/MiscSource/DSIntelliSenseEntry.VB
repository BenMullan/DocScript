Public Class DSIntelliSenseEntry : Implements Global.ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData

	Public Enum DSIntellisenseEntryImage As [Byte]
		BuiltInFunction = 0
		Keyword = 1
		DataType = 2
		Identifier = 3
	End Enum

	Protected Property Text_$
	Protected ReadOnly Description_$
	Protected ReadOnly Image_ As DSIntellisenseEntryImage

	Public Sub New(ByVal _Text$, ByVal _Description$, ByVal _Image As DSIntellisenseEntryImage)
		Me.Text_ = _Text : Me.Description_ = _Description : Me.Image_ = _Image
	End Sub

	Public ReadOnly Property Image As System.Windows.Media.ImageSource Implements ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData.Image
		Get
			Select Case Me.Image_
				Case DSIntellisenseEntryImage.BuiltInFunction : Return New BitmapImage(New Uri("pack://application:,,,/DSIDE;component/Images/BuiltInFunction.PNG"))
				Case DSIntellisenseEntryImage.Keyword : Return New BitmapImage(New Uri("pack://application:,,,/DSIDE;component/Images/Keyword.PNG"))
				Case DSIntellisenseEntryImage.DataType : Return New BitmapImage(New Uri("pack://application:,,,/DSIDE;component/Images/DataType.PNG"))
				Case DSIntellisenseEntryImage.Identifier : Return New BitmapImage(New Uri("pack://application:,,,/DSIDE;component/Images/About.PNG"))
				Case Else : Throw New DSValidationException("The DSIntellisenseEntryImage was unaccounted-for", Me.Image_.ToString())
			End Select
		End Get
	End Property

#Region "Rather Useless Passthrough Propities"

	Public ReadOnly Property Text As String Implements ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData.Text
		Get
			Return Me.Text_
		End Get
	End Property

	Public ReadOnly Property Content As Object Implements ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData.Content
		Get
			Return Me.Text
		End Get
	End Property

	Public ReadOnly Property Description As Object Implements ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData.Description
		Get
			Return Me.Description_
		End Get
	End Property

	Public ReadOnly Property Priority As Double Implements ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData.Priority
		Get
			Return 0
		End Get
	End Property

	''' <summary>Inserts the AutoComplete text into the Source-Code-Editor</summary>
	Public Sub Complete(
	 ByVal _TextArea As ICSharpCode.AvalonEdit.Editing.TextArea,
	 ByVal _CompletionSegment As ICSharpCode.AvalonEdit.Document.ISegment,
	 ByVal _InsertionRequestEventArgs As System.EventArgs
	) Implements ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData.Complete
		Try

			REM Patch 09062023
			'Problem: DSIntelliSense is inserting characters that have already been typed
			'E.g. "Fun" → "FFunction"
			'E.g. "Stringarray" → "SDS_StringArray_EachToNumber"
			'
			'Solution: Overwrite the 1st character; Make the 

			_TextArea.Document.Replace(
			 offset:=_CompletionSegment.Offset - 1,
			 length:=_CompletionSegment.Length + 1,
			 text:=Me.Text
			)

		Catch _Ex As Exception : MsgBox("DSIDE could not insert the Auto-Completion String: " & _Ex.Message, MsgBoxStyle.Critical) : End Try
	End Sub

#End Region

End Class