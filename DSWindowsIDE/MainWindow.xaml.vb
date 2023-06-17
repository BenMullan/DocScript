'The Properties and Feilds are not declared at the top of this Class File. Rather, they are close to where they are used, inside [#Region]s.

Class MainWindow

	'Initialised in the Constructor, because with an inline initialisation, ExeCxt.GUIDefault attempt to Log a message, and can't (because the CurrentLogEventHandler isn't yet initialised)
	Public Property CurrentExecutionContext As DocScript.Runtime.ExecutionContext

	Public Sub New()

		Try

			Me.InitializeComponent() : System.Windows.Forms.Application.EnableVisualStyles()

			Me.Title = "DocScript IDE (" & Environment.UserName & " on \\" & My.Computer.Name & ")"c
			Me.CurrentExecutionContext = DocScript.Runtime.ExecutionContext.GUIDefault

			RegisterCodeSnippetInsertion_EventHandlers_()
			Me.InitialiseTextEditorControl_()
			Me.PopulateLogEventHandlersComboBox()

			CLAManagment.ExamineCLAs_And_SetPassthroughVariables() 'Variables including CLAManagment.CLAPassthrough_CLAHelpDictionaryText are then later picked-up on.

		Catch _Ex As Exception
			Dim _NewMsgBoxThread As New System.Threading.Thread(Sub() MsgBox("Exception on DSIDE Main-Window Construction:" & vbCrLf & vbCrLf & _Ex.Message & If(_Ex.InnerException IsNot Nothing, vbCrLf & _Ex.InnerException.Message, ""), MsgBoxStyle.Critical, _Ex.GetType().FullName))
			_NewMsgBoxThread.Start() : _NewMsgBoxThread.Join()
			Environment.Exit(exitCode:=2)
		End Try

	End Sub

	'WPF substitute for a "Me.Shown" Handler...
	Public Sub ConfigureInstance_FromCLAs() Handles Me.ContentRendered
		Try

			REM Pick-up on and Data passed through from the CLA-Manager
			If CLAManagment.CLAPassthrough_TextToInsertIntoTextEditor IsNot Nothing Then Me.SourceTextEditor.Text = CLAManagment.CLAPassthrough_TextToInsertIntoTextEditor
			If CLAManagment.CLAPassthrough_RunWhenReady AndAlso (CLAManagment.CLAPassthrough_SourceFileToOpen Is Nothing) Then Throw New DSException("The /Run Argument is invalid, because no DocScript Program was specified the /OpenSourceFile Argument")
			If CLAManagment.CLAPassthrough_SourceFileToOpen IsNot Nothing Then Me.OpenFile_FromCLAs(CLAManagment.CLAPassthrough_SourceFileToOpen, Sub() If CLAManagment.CLAPassthrough_RunWhenReady Then Me.RunCurrentSource())

			REM Ensure the correct Buttons' State is applied
			Me.UpdateInterpretationButtonsState_(InterpretationStage_.NoInterpretationPerformedYet)

		Catch _Ex As Exception
			[Interaction].MsgBox("On Initialising the UI Components:" & vbCrLf & vbCrLf & _Ex.Message, MsgBoxStyle.Exclamation, _Ex.GetType().Name)
		End Try
	End Sub

	''' <summary>The centralised mapping of Shortcut-Keys to Handlers-thereof</summary>
	Public Sub HandleShortcutKey(ByVal _Sender As Object, ByVal _KeyEventArgs As KeyEventArgs) Handles Me.KeyDown

		REM Do not process shortcut keys whilst a BackgroundThread is in progress
		If Me.AbortBackgroundWorkerLink.Text = MainWindow.AbortButton_TextWhenEnabled Then Return

		If (Keyboard.Modifiers = ModifierKeys.Control) AndAlso (_KeyEventArgs.Key = Key.N) Then : Me.StartNewFile()									'Ctrl + N
		ElseIf (Keyboard.Modifiers = ModifierKeys.Control) AndAlso (_KeyEventArgs.Key = Key.O) Then : Me.OpenFile()									'Ctrl + O
		ElseIf (Keyboard.Modifiers = ModifierKeys.Control) AndAlso (_KeyEventArgs.Key = Key.S) Then : Me.SaveFile()									'Ctrl + S
		ElseIf (Keyboard.Modifiers = ModifierKeys.Control) AndAlso (_KeyEventArgs.Key = Key.H) Then : Me.ShowHelpWindow()							'Ctrl + H
		ElseIf (Keyboard.Modifiers = ModifierKeys.Control) AndAlso (_KeyEventArgs.Key = Key.F4) Then : Me.AskToSaveUnsavedSource_ThenExitDSIDE()	'Ctrl + F4
		ElseIf (Keyboard.Modifiers = ModifierKeys.Control) AndAlso (_KeyEventArgs.Key = Key.F5) Then : Me.LaunchDSCompilationWindow()				'Ctrl + F5
		ElseIf (Keyboard.Modifiers = ModifierKeys.Shift) AndAlso (_KeyEventArgs.Key = Key.F5) Then : Me.LaunchDSRemotingWindow()					'Shift + F5
		ElseIf (Keyboard.Modifiers = (ModifierKeys.Control Or ModifierKeys.Shift)) AndAlso (_KeyEventArgs.Key = Key.N) Then : Me.StartNewDSIDEInstance()				'Ctrl + Shift + N
		ElseIf (Keyboard.Modifiers = (ModifierKeys.Control Or ModifierKeys.Shift)) AndAlso (_KeyEventArgs.Key = Key.O) Then : Me.OpenContainingFolder()					'Ctrl + Shift + O
		ElseIf (Keyboard.Modifiers = (ModifierKeys.Control Or ModifierKeys.Shift)) AndAlso (_KeyEventArgs.Key = Key.S) Then : Me.SaveFileAs()							'Ctrl + Shift + S
		ElseIf (Keyboard.Modifiers = (ModifierKeys.Control Or ModifierKeys.Shift)) AndAlso (_KeyEventArgs.Key = Key.K) Then : Me.ShowTokensTable_InNewWindow()			'Ctrl + Shift + K
		ElseIf (Keyboard.Modifiers = (ModifierKeys.Control Or ModifierKeys.Shift)) AndAlso (_KeyEventArgs.Key = Key.T) Then : Me.ShowProgramTree_InNewWindow()			'Ctrl + Shift + T
		ElseIf (Keyboard.Modifiers = (ModifierKeys.Control Or ModifierKeys.Shift)) AndAlso (_KeyEventArgs.Key = Key.R) Then : Me.ShowExeResTree_InNewWindow()			'Ctrl + Shift + R
		ElseIf (Keyboard.Modifiers = (ModifierKeys.Control Or ModifierKeys.Shift)) AndAlso (_KeyEventArgs.Key = Key.B) Then : Me.ShowNewBIFExplorerWindow()				'Ctrl + Shift + B
		ElseIf (Keyboard.Modifiers = (ModifierKeys.Control Or ModifierKeys.Shift)) AndAlso (_KeyEventArgs.Key = Key.H) Then : Call (New PictorialHelpWindow()).Show()	'Ctrl + Shift + H
		ElseIf (Keyboard.Modifiers = (ModifierKeys.Control Or ModifierKeys.Shift)) AndAlso (_KeyEventArgs.Key = Key.F) Then : Me.ShowNewRemoteFileExplorerWindow()		'Ctrl + Shift + F
		ElseIf (Keyboard.Modifiers = (ModifierKeys.Control Or ModifierKeys.Shift)) AndAlso (_KeyEventArgs.Key = Key.E) Then : Me.LaunchDSExpr()							'Ctrl + Shift + E
		ElseIf _KeyEventArgs.Key = Key.F1 Then : Me.ParseCurrentSource()	'F1
		ElseIf _KeyEventArgs.Key = Key.F2 Then : Me.LexCachedTokens()		'F2
		ElseIf _KeyEventArgs.Key = Key.F3 Then : Me.ExecuteCachedProgram()	'F3
		ElseIf _KeyEventArgs.Key = Key.F5 Then : Me.RunCurrentSource()		'F5
		Else : Return 'Don't set the Handled as below...
		End If

		'Don't type the Keys into Me.SourceTextEditor
		_KeyEventArgs.Handled = True

	End Sub

	Protected Sub RegisterCodeSnippetInsertion_EventHandlers_()

		AddHandler Insert_SampleProgram_HelloWorld.Click, Sub() Me.InsertTextAtCursor(PredefinedSnippets.SampleProgram_HelloWorld, _SetSourceSavedFlag_IfThisIsTheFirstTextPutIntoTheEditor:=True)
		AddHandler Insert_SampleProgram_BeepSleepRepeat.Click, Sub() Me.InsertTextAtCursor(PredefinedSnippets.SampleProgram_BeepSleepRepeat, _SetSourceSavedFlag_IfThisIsTheFirstTextPutIntoTheEditor:=True)
		AddHandler Insert_SampleProgram_DebuggingExample.Click, Sub() Me.InsertTextAtCursor(PredefinedSnippets.SampleProgram_DebuggingExample, _SetSourceSavedFlag_IfThisIsTheFirstTextPutIntoTheEditor:=True)

		AddHandler Insert_EntryPoint_Unadorned.Click, Sub() Me.InsertTextAtCursor(PredefinedSnippets.EntryPoint_Unadorned)
		AddHandler Insert_EntryPoint_OSInterop.Click, Sub() Me.InsertTextAtCursor(PredefinedSnippets.EntryPoint_OSInterop)

		AddHandler Insert_Loop_ConditionControlled.Click, Sub() Me.InsertTextAtCursor(PredefinedSnippets.Loop_ConditionControlled)
		AddHandler Insert_Loop_CountControlled_While.Click, Sub() Me.InsertTextAtCursor(PredefinedSnippets.Loop_CountControlled_While)
		AddHandler Insert_Loop_CountControlled_Loop.Click, Sub() Me.InsertTextAtCursor(PredefinedSnippets.Loop_CountControlled_Loop)
		AddHandler Insert_Loop_ArrayIteration.Click, Sub() Me.InsertTextAtCursor(PredefinedSnippets.Loop_ArrayIteration)
		AddHandler Insert_IfElseStatement.Click, Sub() Me.InsertTextAtCursor(PredefinedSnippets.IfElseStatement)

	End Sub

	''' <summary>
	''' Starts the Task, then Returns. When finished, the UI is reset to the Idle state.
	''' Exceptions hence ↓ are MsgBoxed out
	''' </summary>
	''' <param name="_TaskStatusDescription"></param>
	''' <param name="_Action">This MUST NOT Access UI Components without Me.InvokeIfRequired()</param>
	''' <remarks></remarks>
	Public Sub StartBackgroundWorker(ByVal _TaskStatusDescription$, ByVal _Action As System.Action)

		Me.LoadingUIComponents_SetForTaskStart(_TaskStatusDescription)

		Dim _BackgroundWorker As New System.ComponentModel.BackgroundWorker() With {
		 .WorkerReportsProgress = False,
		 .WorkerSupportsCancellation = True
		}

		REM If the Abort link is pressed, cancel the task
		Dim _CancelBackgroundWorker_Action As New Windows.Input.MouseButtonEventHandler(
		 Sub()
			 _BackgroundWorker.CancelAsync() 'Sets [CancellationPending] to True
			 RemoveHandler Me.AbortBackgroundWorkerLink.PreviewMouseDown, _CancelBackgroundWorker_Action
			 Me.InvokeIfRequired(AddressOf Me.LoadingUIComponents_Reset)
		 End Sub)
		AddHandler Me.AbortBackgroundWorkerLink.PreviewMouseDown, _CancelBackgroundWorker_Action

		REM The main action of the Background Thread
		AddHandler _BackgroundWorker.DoWork, _
		 Sub(_Sender As Object, _DoWorkEA As ComponentModel.DoWorkEventArgs)

			 Dim _CurrentBackgroundWorker As ComponentModel.BackgroundWorker = CType(_Sender, ComponentModel.BackgroundWorker)
			 Dim _InnerWorkerThread As New System.Threading.Thread(
			   Sub()
				   Try
					   _Action.Invoke()
				   Catch _Exception As Exception When True
					   REM Don't show the MsgBox if the user has clicked [Abort]...
					   If Not ({"Thread was being aborted.", "Der Thread wurde abgebrochen."}.Any(AddressOf _Exception.Message.EndsWith)) Then MsgBox("A Fatal Exception was Thrown whilst """ & _TaskStatusDescription & """:" & vbCrLf & vbCrLf & _Exception.Message, MsgBoxStyle.Critical, _Exception.GetType().FullName)
					   RemoveHandler Me.AbortBackgroundWorkerLink.PreviewMouseDown, _CancelBackgroundWorker_Action
					   Me.InvokeIfRequired(AddressOf Me.LoadingUIComponents_Reset)
				   End Try
			   End Sub)

			 _InnerWorkerThread.Start()

			 While _InnerWorkerThread.IsAlive

				 'Check if the _InnerWorkerThread should be Abort()-ed
				 If _CurrentBackgroundWorker.CancellationPending Then
					 _InnerWorkerThread.Abort()
					 _DoWorkEA.Cancel = True
				 End If

				 System.Threading.Thread.SpinWait(1)

			 End While

		 End Sub

		REM The action to run when the Background Thread's main task is complete
		AddHandler _BackgroundWorker.RunWorkerCompleted, _
		 Sub(_Sender As Object, _CompletedEA As ComponentModel.RunWorkerCompletedEventArgs)
			 If _CompletedEA.Error IsNot Nothing Then
				 Dim _Exception As System.Exception = CType(_CompletedEA.Error, Exception)
				 MsgBox("A Fatal Exception was Thrown whilst """ & _TaskStatusDescription & """:" & vbCrLf & vbCrLf & _Exception.Message, MsgBoxStyle.Critical, _Exception.GetType().FullName)
			 End If
			 RemoveHandler Me.AbortBackgroundWorkerLink.PreviewMouseDown, _CancelBackgroundWorker_Action
			 Me.InvokeIfRequired(AddressOf Me.LoadingUIComponents_Reset)
			 Me.InvokeIfRequired(AddressOf Me.Activate)					'Make the MainWindow focoused again...
			 Me.InvokeIfRequired(AddressOf Me.SourceTextEditor.Focus)	'...Specifically, make the SourceTextEditor Focoused
		 End Sub

		REM Now actually start the Background Thread
		_BackgroundWorker.RunWorkerAsync()

	End Sub

#Region "SourceTextEditor-related"

	Protected Property AvalonEdit_SearchPanel_ As New ICSharpCode.AvalonEdit.Search.SearchPanel()

	Protected Sub InitialiseTextEditorControl_()

		AddHandler Me.SourceTextEditor.TextArea.Caret.PositionChanged, AddressOf Me.UpdateLineAndColLabel
		AddHandler Me.SourceTextEditor.TextArea.TextEntered, AddressOf Me.TextArea_TextEntered_
		AddHandler Me.SourceTextEditor.TextArea.TextEntering, AddressOf Me.TextArea_TextEntering_

		Me.LoadSyntaxHighlighting_()

		With Me.SourceTextEditor.Options
			.AllowScrollBelowDocument = True
			.ShowBoxForControlCharacters = True
			.ConvertTabsToSpaces = False
			.CutCopyWholeLine = True
			.EnableEmailHyperlinks = True
			.EnableHyperlinks = True
			.EnableImeSupport = False
			.EnableRectangularSelection = True
			.EnableTextDragDrop = True
		End With

		REM Register the Ctrl + F Find Dialog
		Me.AvalonEdit_SearchPanel_.Attach(Me.SourceTextEditor.TextArea)
		Me.SourceTextEditor.TextArea.DefaultInputHandler.NestedInputHandlers.Add(New ICSharpCode.AvalonEdit.Search.SearchInputHandler(Me.SourceTextEditor.TextArea))
		Me.SourceTextEditor.Focus()

	End Sub

	Protected Sub LoadSyntaxHighlighting_()

		Dim _XSHD_ByteArray As [Byte]() = Global.DocScript.[My].Resources.DocScript_SyntaxHighlighting
		Dim _XSHD_XMLString As [String] = System.Text.Encoding.ASCII.GetString(_XSHD_ByteArray)

		Dim _XMLReader As New Xml.XmlTextReader(New System.IO.StringReader(_XSHD_XMLString))

		Dim _DocScriptSyntaxHighlighting As Global.ICSharpCode.AvalonEdit.Highlighting.IHighlightingDefinition = _
		   Global.ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(_XMLReader, Global.ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance)

		Global.ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.RegisterHighlighting(
		 name:="DocScriptSyntaxHighlighting",
		 extensions:={".DS"},
		 highlighting:=_DocScriptSyntaxHighlighting
		)

		Me.SourceTextEditor.SyntaxHighlighting = _DocScriptSyntaxHighlighting

	End Sub

	Protected IntelliSenseWindow_ As Global.ICSharpCode.AvalonEdit.CodeCompletion.CompletionWindow = Nothing
	Protected Sub TextArea_TextEntered_(ByVal _Sender As Object, ByVal _TextCompositionEA As TextCompositionEventArgs)

		REM Only open the IntelliSense Window after a _ has been typed
		If Not ((_TextCompositionEA.Text.First() = "_"c) OrElse [Char].IsUpper(_TextCompositionEA.Text.First())) Then Return

		Me.IntelliSenseWindow_ = New Global.ICSharpCode.AvalonEdit.CodeCompletion.CompletionWindow(Me.SourceTextEditor.TextArea) With {.Width = 250, .CloseWhenCaretAtBeginning = True, .ExpectInsertionBeforeStart = False}
		Dim _DSIntellisenseEntries As IList(Of Global.ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData) = Me.IntelliSenseWindow_.CompletionList.CompletionData

		REM Add a note about Parsing to get Identifiers added
		_DSIntellisenseEntries.Add(New DSIntelliSenseEntry(_Text:="(Parse to load Identifiers: F1)", _Description:="", _Image:=DSIntelliSenseEntry.DSIntellisenseEntryImage.Identifier))

		REM Identifiers in currently-cached Tokens
		If Me.Cached_Tokens IsNot Nothing Then
			For Each _IdentifierToken As Runtime.Token In Me.Cached_Tokens.Where(Function(_Token As Runtime.Token) _Token.Type = Runtime.Token.TokenType.Identifier)
				_DSIntellisenseEntries.Add(New DSIntelliSenseEntry(_Text:=_IdentifierToken.Value, _Description:="[Identifier]", _Image:=DSIntelliSenseEntry.DSIntellisenseEntryImage.Identifier))
			Next
		End If

		REM Non-array DataTypes
		For Each _DataType$ In Global.DocScript.Language.Constants.AllNonArrayDataTypes
			_DSIntellisenseEntries.Add(New DSIntelliSenseEntry(_Text:=_DataType.InTitleCase(), _Description:="[Data Type]", _Image:=DSIntelliSenseEntry.DSIntellisenseEntryImage.DataType))
		Next

		REM Keywords
		For Each _Keyword$ In Global.DocScript.Language.Constants.AllKeywords
			_DSIntellisenseEntries.Add(New DSIntelliSenseEntry(_Text:=_Keyword.InTitleCase(), _Description:="[Keyword]", _Image:=DSIntelliSenseEntry.DSIntellisenseEntryImage.Keyword))
		Next

		REM StatementEnds
		For Each _StatementEnd$ In Global.DocScript.Language.Constants.AllStatementEnds
			_DSIntellisenseEntries.Add(New DSIntelliSenseEntry(_Text:=_StatementEnd.InTitleCase(), _Description:="[Statement End]", _Image:=DSIntelliSenseEntry.DSIntellisenseEntryImage.Keyword))
		Next

		REM BIFs
		For Each _BIF As DocScript.Runtime.BuiltInFunction In Me.CurrentExecutionContext.BuiltInFunctions
			_DSIntellisenseEntries.Add(New DSIntelliSenseEntry(_Text:=_BIF.Identifier, _Description:="[Built-in Function] " & _BIF.TemplateCall & vbCrLf & _BIF.Description, _Image:=DSIntelliSenseEntry.DSIntellisenseEntryImage.BuiltInFunction))
		Next

		Me.IntelliSenseWindow_.Show()
		AddHandler Me.IntelliSenseWindow_.Closed, Sub() Me.IntelliSenseWindow_ = Nothing

	End Sub

	Protected Sub TextArea_TextEntering_(ByVal _Sender As Object, ByVal _TextCompositionEA As TextCompositionEventArgs)

		If (_TextCompositionEA.Text.Length > 0) AndAlso (Me.IntelliSenseWindow_ IsNot Nothing) Then

			REM OLD
			'Whenever a non-letter is typed while the completion window is open, insert the currently selected element.
			'If Not [Char].IsLetterOrDigit(_TextCompositionEA.Text.First()) Then Me.IntelliSenseWindow_.CompletionList.RequestInsertion(_TextCompositionEA)

			REM NEW
			'If a non-letter-or-number is pressed (e.g. [SPACE]), then Hide the IntelliSenseWindow_
			If Not [Char].IsLetterOrDigit(_TextCompositionEA.Text.First()) Then Me.IntelliSenseWindow_.Close()

		End If

	End Sub

	REM Used in RegisterCodeSnippetInsertion_EventHandlers_()
	Public Sub InsertTextAtCursor(ByVal _Text$, Optional ByVal _SetSourceSavedFlag_IfThisIsTheFirstTextPutIntoTheEditor As Boolean = False)

		REM Capture the boolean emptiness datum for use lates herein
		Dim _TextEditorWasEmpty_BeforeTextWasInserted As Boolean = String.IsNullOrEmpty(Me.SourceTextEditor.Text)

		REM Inject the string
		Me.SourceTextEditor.Document.Insert(Me.SourceTextEditor.CaretOffset, _Text)

		REM Raise the Source-Is-Saved flag, if (1) We've been told to, and (2) There wasn't any text in the SourceTextEditor beforehand
		If _SetSourceSavedFlag_IfThisIsTheFirstTextPutIntoTheEditor AndAlso _TextEditorWasEmpty_BeforeTextWasInserted Then Me.CurrentSource_IsSaved = True

	End Sub

	Public Sub UpdateLineAndColLabel(ByVal _Sender As Object, ByVal _EventArgs As EventArgs) 'Handler set in InitialiseTextEditorControl_()
		Dim _CaretLocation As Global.ICSharpCode.AvalonEdit.Document.TextLocation = Me.SourceTextEditor.Document.GetLocation(Me.SourceTextEditor.CaretOffset)
		Me.CaretLineColumnLabel.Text = String.Format("Line: {0}, Col: {1}", _CaretLocation.Line.ToString(), _CaretLocation.Column.ToString())
	End Sub

	Public Sub UpdateLineCountLabel_And_AlterIsSavedState() Handles SourceTextEditor.TextChanged

		Me.LineCountLabel.Text = (Me.SourceTextEditor.Text.ToCharArray().Where(Function(_Char As Char) _Char = vbLf.First()).Count() + 1).ToString() & " Line(s)"
		Me.CurrentSource_IsSaved = False 'Sets the Window Title accordingly. If there's no open file, then the Title dosen't change.

	End Sub

	REM Shift + Scroll
	Protected Sub HandleHorozontalScrollingEvent_(ByVal _Sender As [Object], ByVal _MouseWheelEventArgs As [MouseWheelEventArgs]) Handles SourceTextEditor.PreviewMouseWheel
		If Keyboard.Modifiers = ModifierKeys.Shift Then
			If (_MouseWheelEventArgs.Delta < 0) Then
				Me.SourceTextEditor.LineRight() : Me.SourceTextEditor.LineRight()
			Else
				Me.SourceTextEditor.LineLeft() : Me.SourceTextEditor.LineLeft()
			End If
			_MouseWheelEventArgs.Handled = True
		End If
	End Sub

	REM Ctrl + Scroll
	Protected Sub HandleCtrlScroll_(ByVal _Sender As Object, ByVal _MouseWheelEventArgs As MouseWheelEventArgs) Handles Me.PreviewMouseWheel
		If Keyboard.Modifiers = ModifierKeys.Control Then
			'Dim _FirstVisibleLine_Number% = Me.SourceTextEditor.TextArea.TextView.GetDocumentLineByVisualTop(Me.SourceTextEditor.TextArea.TextView.ScrollOffset.Y).LineNumber
			If _MouseWheelEventArgs.Delta > 0 Then Me.ZoomValueSlider.Value += 0.2
			If _MouseWheelEventArgs.Delta < 0 Then Me.ZoomValueSlider.Value -= 0.2
			'Me.SourceTextEditor.ScrollToLine(_FirstVisibleLine_Number)
			_MouseWheelEventArgs.Handled = True
		End If
	End Sub

#End Region

#Region "File Open and Save Button Event Handlers"

	Public CurrentlyOpenFile As IO.FileInfo = Nothing

	''' <summary>Automatically adjusts the Window Title</summary>
	Public Property CurrentSource_IsSaved As Boolean
		Get
			Return Me.CurrentSource_IsSaved_
		End Get
		Set(_SourceIsSaved As Boolean)

			Me.CurrentSource_IsSaved_ = _SourceIsSaved

			If Me.CurrentlyOpenFile Is Nothing Then : Me.Title = "(Unsaved) DocScript IDE (" & Environment.UserName & " on \\" & My.Computer.Name & ")"c
			ElseIf Me.CurrentSource_IsSaved_ Then : Me.Title = Me.CurrentlyOpenFile.Name
			Else : Me.Title = "(Unsaved) " & Me.CurrentlyOpenFile.Name
			End If

			If Me.CurrentSource_IsSaved_ Then Me.LastPerformedAction_InfoText.Text = "Saved Source"

		End Set
	End Property
	Protected CurrentSource_IsSaved_ As Boolean = False

	REM Drag-and-Drop Open
	Private Sub LayoutRoot_Drop_(ByVal _Sender As Object, ByVal _DragEventArgs As System.Windows.DragEventArgs) Handles LayoutRoot.Drop

		If ((Me.SourceTextEditor.Text.Length > 0) AndAlso (Not Me.CurrentSource_IsSaved)) AndAlso (MsgBox("The Source-text in the Editor may be unsaved; it will be lost if a new file is opened." & vbCrLf & vbCrLf & "Do you still want to Open the new DocScript File?", MsgBoxStyle.YesNo Or MsgBoxStyle.Question, "Source may be deleted") = MsgBoxResult.No) Then Return

		Me.StartBackgroundWorker("Opening Dropped File...",
		 Sub()

			 If _DragEventArgs.Data.GetDataPresent(DataFormats.FileDrop) Then

				 Dim _File As New IO.FileInfo(CType(_DragEventArgs.Data.GetData(DataFormats.FileDrop), [String]()).First())
				 If Not _File.Exists Then Throw New DSValidationException("The file specified via the Drop-Event does not exist.", _File.FullName)

				 Me.CurrentlyOpenFile = _File
				 Dim _SourceText_FromFile$ = My.Computer.FileSystem.ReadAllText(Me.CurrentlyOpenFile.FullName)
				 Me.InvokeIfRequired(Sub()
										 Me.SourceTextEditor.Text = _SourceText_FromFile
										 Me.CurrentSource_IsSaved = True 'Sets the Window Title accordingly
										 Me.UpdateInterpretationButtonsState_(InterpretationStage_.NoInterpretationPerformedYet)
										 Me.LastPerformedAction_InfoText.Text = "Opened File [Drop] " & Me.CurrentlyOpenFile.Name.InBrackets()
									 End Sub)

			 End If

		 End Sub)

	End Sub

	REM Command-Line Argument Open
	Public Sub OpenFile_FromCLAs(ByVal _File As IO.FileInfo, ByVal _OnceFileLoaded_Callback As Action)

		Me.StartBackgroundWorker("Opening File from CLAs...",
		 Sub()

			 If Not _File.Exists Then Throw New DSValidationException("The file specified via the Command-Line Arguments does not exist.", _File.FullName)
			 Me.CurrentlyOpenFile = _File
			 Dim _SourceText_FromFile$ = My.Computer.FileSystem.ReadAllText(Me.CurrentlyOpenFile.FullName)

			 Me.InvokeIfRequired(Sub()
									 Me.SourceTextEditor.Text = _SourceText_FromFile
									 Me.CurrentSource_IsSaved = True 'Sets the Window Title accordingly
									 Me.UpdateInterpretationButtonsState_(InterpretationStage_.NoInterpretationPerformedYet)
									 Me.LastPerformedAction_InfoText.Text = "Opened File [CLA] " & Me.CurrentlyOpenFile.Name.InBrackets()
									 _OnceFileLoaded_Callback.Invoke()
								 End Sub)

		 End Sub)

	End Sub

	REM Open
	Public Sub OpenFile() Handles OpenButton.Click

		If ((Me.SourceTextEditor.Text.Length > 0) AndAlso (Not Me.CurrentSource_IsSaved)) AndAlso (MsgBox("The Source-text in the Editor may be unsaved; it will be lost if a new file is opened." & vbCrLf & vbCrLf & "Do you still want to Open the new DocScript File?", MsgBoxStyle.YesNo Or MsgBoxStyle.Question, "Source may be deleted") = MsgBoxResult.No) Then Return

		Me.StartBackgroundWorker("Opening File...",
		 Sub()

			 With New Global.Microsoft.Win32.OpenFileDialog()
				 .CheckFileExists = True
				 .Title = "DocScript: Open Source File..."
				 .DefaultExt = ".DS"
				 .Filter = "DocScript Source (*.DS)|*.DS|All files (*.*)|*.*"
				 If If(.ShowDialog(), False) Then
					 Me.CurrentlyOpenFile = New IO.FileInfo(.FileName)
					 Dim _SourceText_FromFile$ = My.Computer.FileSystem.ReadAllText(Me.CurrentlyOpenFile.FullName)
					 Me.InvokeIfRequired(Sub()
											 Me.SourceTextEditor.Text = _SourceText_FromFile
											 Me.CurrentSource_IsSaved = True 'Sets the Window Title accordingly
											 Me.UpdateInterpretationButtonsState_(InterpretationStage_.NoInterpretationPerformedYet)
											 Me.LastPerformedAction_InfoText.Text = "Opened File " & Me.CurrentlyOpenFile.Name.InBrackets()
										 End Sub)
				 End If
			 End With

		 End Sub)

	End Sub

	REM Save
	Public Sub SaveFile() Handles SaveButton.Click

		Me.StartBackgroundWorker("Saving File...",
		   Sub()

			   REM If there isn't currently a CurrentlyOpenFile, then request the path for this
			   If Me.CurrentlyOpenFile Is Nothing Then
				   With New Global.Microsoft.Win32.SaveFileDialog()
					   .CheckFileExists = False
					   .Title = "DocScript: Save Source File..."
					   .DefaultExt = ".DS"
					   .Filter = "DocScript Source (*.DS)|*.DS|All files (*.*)|*.*"
					   If If(.ShowDialog(), False) Then Me.CurrentlyOpenFile = New IO.FileInfo(.FileName) Else Return 'If cancelled, don't save the File
				   End With
			   End If

			   REM At this point, we should have a CurrentlyOpenFile; Save it...
			   Me.InvokeIfRequired(Sub()
									   Me.SourceTextEditor.Save(Me.CurrentlyOpenFile.FullName)
									   Me.CurrentSource_IsSaved = True 'Sets the Window Title accordingly
									   Me.LastPerformedAction_InfoText.Text = "Saved File " & Me.CurrentlyOpenFile.Name.InBrackets()
								   End Sub)

		   End Sub)

	End Sub

	REM SaveAs
	Public Sub SaveFileAs() Handles SaveAsButton.Click

		Me.StartBackgroundWorker("Saving File As...",
		   Sub()

			   REM Always ask for a new Path to save the source as
			   With New Global.Microsoft.Win32.SaveFileDialog()
				   .CheckFileExists = False
				   .Title = "DocScript: Save Source File As..."
				   .DefaultExt = ".DS"
				   .Filter = "DocScript Source (*.DS)|*.DS|All files (*.*)|*.*"
				   If If(.ShowDialog(), False) Then Me.CurrentlyOpenFile = New IO.FileInfo(.FileName) Else Return 'If cancelled, don't save the File
			   End With

			   'At this point, we should have a CurrentlyOpenFile; Save it...
			   Me.InvokeIfRequired(Sub()
									   Me.SourceTextEditor.Save(Me.CurrentlyOpenFile.FullName)
									   Me.CurrentSource_IsSaved = True 'Sets the Window Title accordingly
									   Me.LastPerformedAction_InfoText.Text = "Saved File as " & Me.CurrentlyOpenFile.Name.InBrackets()
								   End Sub)

		   End Sub)

	End Sub

	REM New
	Public Sub StartNewFile() Handles NewButton.Click

		If ((Me.SourceTextEditor.Text.Length > 0) AndAlso (Not Me.CurrentSource_IsSaved)) AndAlso (MsgBox("The Source-text in the Editor may be unsaved; it will be lost if a new document is started." & vbCrLf & vbCrLf & "Do you still want to start a new DocScript File?", MsgBoxStyle.YesNo Or MsgBoxStyle.Question, "Source may be deleted") = MsgBoxResult.No) Then Return

		Me.SourceTextEditor.Text = String.Empty
		Me.CurrentlyOpenFile = Nothing

		Me.UpdateInterpretationButtonsState_(InterpretationStage_.NoInterpretationPerformedYet)

		Me.CurrentSource_IsSaved = False : Me.Title = "DocScript IDE (" & Environment.UserName & " on \\" & My.Computer.Name & ")"c
		Me.LastPerformedAction_InfoText.Text = "Started New (unsaved) File"

	End Sub

#End Region

#Region "Misc. Control Event Handlers"

	Public ReadOnly Property DocScriptCLAs_FromTextbox As [String]()
		Get

			REM If the TextBox is empty, then return an Empty Array
			If [String].IsNullOrEmpty(Me.ProgramCLAsTextBox.Text) Then Return (New [String](-1) {}) '(New List(Of [String])()).ToArray()

			REM Otherwise, return the .Text, Split() by " "c
			Return Me.ProgramCLAsTextBox.Text.Split(" "c)

		End Get
	End Property

	REM Mapped to a shortcut key
	Public Sub OpenContainingFolder()
		Try
			If Me.CurrentlyOpenFile Is Nothing Then Throw New NullReferenceException("There is no Currently-Open File. Use Ctrl + O, or command-line arguments, to open one.")
			Process.Start("explorer.exe", "/select,""" & Me.CurrentlyOpenFile.FullName & """")
		Catch _Ex As Exception
			MsgBox("On attempting to open the containing folder in Explorer:" & vbCrLf & vbCrLf & _Ex.Message, MsgBoxStyle.Critical, _Ex.GetType().Name)
		End Try
	End Sub

	REM Mapped to a shortcut key
	Public Sub StartNewDSIDEInstance()
		Dim _DSIDE_Path$ = System.Reflection.Assembly.GetEntryAssembly().Location
		Try
			Process.Start(_DSIDE_Path)
		Catch _Ex As Exception
			MsgBox("On attempting to launch " & _DSIDE_Path.InSquares() & ":" & vbCrLf & vbCrLf & _Ex.Message, MsgBoxStyle.Critical, _Ex.GetType().Name)
		End Try
	End Sub

	Public Sub UpdateZoomValueText() Handles ZoomValueSlider.ValueChanged
		Me.ZoomValueText.Text = "Zoom: " & Math.Round(Me.ZoomValueSlider.Value, 2).ToString().InBrackets()
		'Because the Me.ZoomValueStatusBarText appears after the ZoomValueSlider in the XAML, it's not Initialised (New()) when this Sub is run during the initialisation of ZoomValueSlider when it's Value is set
		If Me.ZoomValueStatusBarText IsNot Nothing Then Me.ZoomValueStatusBarText.Text = "Zoom: " & Math.Round(Me.ZoomValueSlider.Value * 100).ToString() & "%"c
	End Sub

	Public Sub UpdateSkewValueText() Handles SkewValueSlider.ValueChanged
		Me.SkewValueText.Text = "Skew: " & Math.Round(Me.SkewValueSlider.Value, 2).ToString().InBrackets()
	End Sub

	Public Sub UpdateRotateValueText() Handles RotateValueSlider.ValueChanged
		Me.RotateValueText.Text = "Rotate: " & Math.Round(Me.RotateValueSlider.Value, 2).ToString().InBrackets()
	End Sub

	Public Sub ResetAllTextTransformations() Handles ResetAllTextTransformationsButton.Click
		Me.ZoomValueSlider.Value = 1
		Me.SkewValueSlider.Value = 0
		Me.RotateValueSlider.Value = 0
	End Sub

	Public Sub AnimateTextRotation() Handles AnimateTextRotationButton.Click
		Dim _Duration As New Duration(New TimeSpan(days:=0, hours:=0, minutes:=0, seconds:=7, milliseconds:=0))
		Dim _RotationAnimation As New Windows.Media.Animation.DoubleAnimation(360, _Duration)
		If (Keyboard.IsKeyDown(Key.LeftCtrl) OrElse Keyboard.IsKeyDown(Key.RightCtrl)) Then _RotationAnimation.RepeatBehavior = System.Windows.Media.Animation.RepeatBehavior.Forever
		Me.SourceTextRotateTransform.BeginAnimation(RotateTransform.AngleProperty, _RotationAnimation)
		Me.AnimateTextRotationButton.IsEnabled = False : Me.RotateValueSlider.IsEnabled = False
	End Sub

	Public Sub ToggleFullScreen() Handles FullScreenButton.Click

		REM If we're in FullScreen...
		If Me.WindowState = Windows.WindowState.Maximized Then

			REM ...Then Exit FullScreen
			Me.WindowState = WindowState.Normal
			Me.FullScreenButton.Label = "FullScreen"
			Me.FullScreenButton.SmallImageSource = New BitmapImage(New Uri("pack://application:,,,/DSIDE;component/Images/FullScreen.PNG"))

		Else

			REM Enter FullScreen
			Me.WindowState = WindowState.Maximized
			Me.FullScreenButton.Label = "Exit FullScreen"
			Me.FullScreenButton.SmallImageSource = New BitmapImage(New Uri("pack://application:,,,/DSIDE;component/Images/UndoFullScreen.PNG"))

		End If

	End Sub

	Protected Const ZoomInOut_Increment_# = 0.5

	Public Sub ZoomIn() Handles ZoomInButton.Click
		Me.ZoomValueSlider.Value += ZoomInOut_Increment_#
	End Sub

	Public Sub ZoomOut() Handles ZoomOutButton.Click
		Me.ZoomValueSlider.Value -= ZoomInOut_Increment_#
	End Sub

	Public Sub ShowNewBIFExplorerWindow() Handles ViewBIFsButton.Click
		Call (New BIFExplorerWindow(Me)).Show()
	End Sub

	Public Sub LaunchDSExpr() Handles OpenDSExprButton.Click
		Dim _DSExpr_ExeFile As New IO.FileInfo((New System.IO.FileInfo(System.Environment.GetCommandLineArgs().ElementAt(0))).DirectoryName & "\DSExpr.exe") 'CLA[0] is the Full Path to the current Process's Binary Image.
		Try
			Process.Start(_DSExpr_ExeFile.FullName)
		Catch _Ex As Exception
			MsgBox("On attempting to launch " & _DSExpr_ExeFile.FullName.InSquares() & ":" & vbCrLf & vbCrLf & _Ex.Message & vbCrLf & vbCrLf & "Note that the [Launch DSExpr] feature is designed only to work when the DocScript binaries exist within the same directory, such as when DocScript has been installed via the .msi setup program.", MsgBoxStyle.Critical, _Ex.GetType().Name)
		End Try
	End Sub

	Public Sub ShowNewRemoteFileExplorerWindow() Handles LaunchNewRemoteFileExplorerButton.Click
		Dim _NewRFEWindowThread As New System.Threading.Thread(Sub() Call (New RemoteFileExplorerWindow()).ShowDialog())
		_NewRFEWindowThread.SetApartmentState(System.Threading.ApartmentState.STA)
		_NewRFEWindowThread.Start()
	End Sub

	Public Sub RunInDSCLI() Handles RunInDSCLIButton.Click
		Dim _DSCLI_ExeFile As New IO.FileInfo((New System.IO.FileInfo(System.Environment.GetCommandLineArgs().ElementAt(0))).DirectoryName & "\DSCLI.exe") 'CLA[0] is the Full Path to the current Process's Binary Image.
		Try

			REM Ensure that we have a CurrentlyOpenFile
			If Me.CurrentlyOpenFile Is Nothing Then Throw New NullReferenceException("There is no currently-open File, whose path can be passed to DSCLI." & vbCrLf & vbCrLf & "Open a File first with Ctrl + O...")

			REM Run DSCLI.EXE
			Process.Start(
			 _DSCLI_ExeFile.FullName,
			 String.Format("/PromptBeforeExit /Run /SourceFile:""{0}"" {1}", Me.CurrentlyOpenFile.FullName, If(String.IsNullOrEmpty(Me.ProgramCLAsTextBox.Text), String.Empty, "/DocScriptCLAs:""" & Me.ProgramCLAsTextBox.Text & """"))
			)

		Catch _Ex As Exception
			MsgBox("On attempting to launch " & _DSCLI_ExeFile.FullName.InSquares() & ":" & vbCrLf & vbCrLf & _Ex.Message & vbCrLf & vbCrLf & "Note that the [Run in DSCLI] feature is designed only to work when the DocScript binaries exist within the same directory, such as when DocScript has been installed via the .msi setup program.", MsgBoxStyle.Critical, _Ex.GetType().Name)
		End Try
	End Sub

	Public Sub ShowAboutBox() Handles AboutMenuItem.Click
		Call (New DSAboutBox()).ShowDialog()
	End Sub

	Public Sub LaunchDSOnGitHub() Handles DSGitHubMenuItem.Click
		Process.Start("https://github.com/BenMullan/DocScript/")
	End Sub

	Public Sub ShowHelpWindow() Handles HelpMenuItem.Click, PaneHelpButton.Click
		Call (New HelpWindow()).Show()
	End Sub

	Public Sub ShowTokensTable_InNewWindow() Handles GenerateTokensTableButton.Click
		Try
			Call (New TokenTableViewerWindow(Me.Cached_Tokens.MustNotBeNothing("Tokens must first be created by Parsing"))).Show()
		Catch _Ex As Exception
			MsgBox("On Generating the Tokens' Table:" & vbCrLf & vbCrLf & _Ex.Message, MsgBoxStyle.Critical, _Ex.GetType().Name)
		End Try
	End Sub

	Public Sub ShowProgramTree_InNewWindow() Handles GenerateProgTreeButton.Click
		Try
			Call (New ProgramTreeViewer(Me.Cached_Program.MustNotBeNothing("A Program must first be created by Parsing and Lexing"))).Show()
		Catch _Ex As Exception
			MsgBox("On Generating the Program Tree:" & vbCrLf & vbCrLf & _Ex.Message, MsgBoxStyle.Critical, _Ex.GetType().Name)
		End Try
	End Sub

	Public Sub ShowExeResTree_InNewWindow() Handles GenerateExeResTreeButton.Click
		Try
			Call (New ExeResExplorerWindow(Me.Cached_ProgramExeRes.MustNotBeNothing("A Program must first be Executed, to create the cached ExecutionResult"))).Show()
		Catch _Ex As Exception
			MsgBox("On Generating the Execution-Result Tree:" & vbCrLf & vbCrLf & _Ex.Message, MsgBoxStyle.Critical, _Ex.GetType().Name)
		End Try
	End Sub

	Public Sub ShowSymbolTableViewingMsgBox() Handles ViewSymTblButton.Click
		MsgBox("This feature has been moved." & vbCrLf & vbCrLf & "Call ""Debug_ShowSymbolTables()"" from the DocScript Program, where you wish to inspect the Symbol-Tables' current states.", MsgBoxStyle.Information, "DS-IDE")
	End Sub

#Region "Window Closure Handling"

	Public Sub AskToSaveUnsavedSource_ThenExitDSIDE() Handles ExitMenuItem.Click
		If ((Me.SourceTextEditor.Text.Length > 0) AndAlso (Not Me.CurrentSource_IsSaved)) _
		AndAlso (MsgBox("The Source-text in the Editor may be unsaved; it will be lost if DSIDE is closed." & vbCrLf & vbCrLf & "Do you still want to exit?", MsgBoxStyle.YesNo Or MsgBoxStyle.Question, "Source may be deleted") = MsgBoxResult.No) _
		Then
			'Stay open
			Return
		Else
			Me.KillCurrentDSIDEProcess()
		End If
	End Sub

	Protected Overrides Sub OnClosing(ByVal _CancelEventArgs As System.ComponentModel.CancelEventArgs)
		'MyBase.OnClosing(e)
		If ((Me.SourceTextEditor.Text.Length > 0) AndAlso (Not Me.CurrentSource_IsSaved)) _
		AndAlso (MsgBox("The Source-text in the Editor may be unsaved; it will be lost if DSIDE is closed." & vbCrLf & vbCrLf & "Do you still want to exit?", MsgBoxStyle.YesNo Or MsgBoxStyle.Question, "Source may be deleted") = MsgBoxResult.No) _
		Then
			'Stay open
			_CancelEventArgs.Cancel = True
			Return
		Else
			Me.KillCurrentDSIDEProcess()
		End If
	End Sub

#Const MainWindow_IsClosedByEscKeyPress = False
#If MainWindow_IsClosedByEscKeyPress Then
	Public Sub HandleEscKeyPress(ByVal _Sender As [Object], ByVal _KeyEventArgs As [KeyEventArgs]) Handles Me.PreviewKeyDown
		If _KeyEventArgs.Key = Key.Escape Then Me.AskToSaveUnsavedSource_ThenCloseProgram()
	End Sub
#End If

	Public Sub KillCurrentDSIDEProcess()
		'End this Process, including the background thread
		Global.System.Windows.Application.Current.Shutdown()
		System.Diagnostics.Process.GetCurrentProcess().Kill()
	End Sub

#End Region

#Region "Window LoadingMode Setters"

	Public Const AbortButton_TextWhenEnabled$ = " Abort "

	Public Sub LoadingUIComponents_SetForTaskStart(ByVal _TaskStatusDescription$)
		Me.InvokeIfRequired(
		  Sub()
			  Me.StatusLabel.Text = "Status: " & _TaskStatusDescription
			  Me.StatusLabel.Foreground = System.Windows.Media.Brushes.DarkBlue
			  Me.InterpretationProgressBar.IsIndeterminate = True
			  Me.Cursor = Cursors.Wait
			  Me.TheRibbon.IsEnabled = False : Me.SourceTextEditor.IsEnabled = False 'Prevent any other buttons etc from being pressed
			  Me.AbortBackgroundWorkerLink.Text = MainWindow.AbortButton_TextWhenEnabled : Me.AbortBackgroundWorkerLink.IsEnabled = True
		  End Sub
		)
	End Sub

	Public Sub LoadingUIComponents_Reset()
		Me.InvokeIfRequired(
		  Sub()
			  Me.StatusLabel.Text = "Status: Idle"
			  Me.StatusLabel.Foreground = System.Windows.Media.Brushes.Black
			  Me.InterpretationProgressBar.IsIndeterminate = False
			  Me.Cursor = Cursors.Arrow
			  Me.TheRibbon.IsEnabled = True : Me.SourceTextEditor.IsEnabled = True
			  Me.AbortBackgroundWorkerLink.Text = "" : Me.AbortBackgroundWorkerLink.IsEnabled = False
		  End Sub
		)
	End Sub

#End Region

#End Region

#Region "Logging-related"

	Protected Shared ReadOnly LogEventHandlers_Names_ToHandlers_Dictionary_ As New Dictionary(Of [String], DocScript.Logging.LogEventHandler)() From {
	 {"Silence All", Logging.BuiltInLogEventHandlers.SilenceAll},
	 {"Log Window", Logging.BuiltInLogEventHandlers.GUIDefault},
	 {"Command-line", Logging.BuiltInLogEventHandlers.CLIDefault},
	 {"MsgBox-per-Event", Logging.BuiltInLogEventHandlers.MsgBoxPerEvent},
	 {"Write to "".\DSInterpretation.Log""", Logging.BuiltInLogEventHandlers.TextFile(UsefulMethods.GetExecutingAssemblyDirectory().FileHereIn("DSInterpretation.Log"))},
	 {"Write to Windows Event Log", Logging.BuiltInLogEventHandlers.WindowsEventLog("IDE")}
	}

	Protected LogWindow_ForGUIDefault_ As DocScript.Logging.LogWindow = Nothing

	Public Sub PopulateLogEventHandlersComboBox() 'Called by the Constructor
		Me.LogEventHandlers_ComboBox.ItemsSource = MainWindow.LogEventHandlers_Names_ToHandlers_Dictionary_.Keys.ToArray()
		Me.LogEventHandlers_ComboBox.SelectedIndex = 0
	End Sub

	Protected Sub SetCurrentLogEventHandler_() Handles LogEventHandlers_ComboBox.SelectionChanged

		REM Tell DS-Logging what the new LogEventHandler should be
		DocScript.Logging.CurrentLogEventHandler = MainWindow.LogEventHandlers_Names_ToHandlers_Dictionary_.Item(key:=Me.LogEventHandlers_ComboBox.SelectedItem.ToString())

	End Sub

	Public Sub UpdateWeatherOrNotToProcessDebugEvents() Handles ProcessDebugEventsCheckBox.Checked, ProcessDebugEventsCheckBox.Unchecked
		DocScript.Logging.LogUtilities.ProcessDebugEvents = (Me.ProcessDebugEventsCheckBox.IsChecked.HasValue AndAlso Me.ProcessDebugEventsCheckBox.IsChecked.Value)
	End Sub

	Public Sub PerformAnyRequiredLoggingInitialisation_PreInterpretation()

		REM Perform Handler-specific Initialisation
		Select Case DocScript.Logging.CurrentLogEventHandler
			Case Logging.BuiltInLogEventHandlers.GUIDefault : Me.LogWindow_ForGUIDefault_ = New Logging.LogWindow(_SessionName:=Me.SourceTextEditor.Text.Replace(vbCrLf, "  ")) : Me.LogWindow_ForGUIDefault_.Show() : DocScript.Logging.BuiltInLogEventHandlers.CurrentGUIDefaultLogWindow = Me.LogWindow_ForGUIDefault_
			Case Logging.BuiltInLogEventHandlers.CLIDefault : UsefulMethods.AllocConsole()
		End Select

	End Sub

#End Region

#Region "Interpretation Actions"

	Public Property Cached_Tokens As DocScript.Runtime.Token() = Nothing								'Parsing Output		Used in [View Cached Tokens...]
	Public Property Cached_Program As DocScript.Runtime.Program = Nothing								'Lexing Output		Used in [Generate Program Tree...]
	Public Property Cached_ProgramExeRes As DocScript.Language.Instructions.ExecutionResult = Nothing	'Execution Output	Used in [Explore Execution Result...]

#Region "Interpretation Action Payloads"

	Public ReadOnly InterpretationPayload_Parse As Action(Of [String]) = _
	  Sub(_RawSourceText$)
		  : Me.InvokeIfRequired(Sub() Me.StatusLabel.Text = "Status: Parsing...")
		  Me.Cached_Tokens = DocScript.Runtime.Parser.GetTokensFromSource(_RawSourceText$)
		  : Me.InvokeIfRequired(Sub() Me.UpdateInterpretationButtonsState_(InterpretationStage_.Parsing))
		  : Me.InvokeIfRequired(Sub() Me.LastPerformedAction_InfoText.Text = "Parsed Source into " & Me.Cached_Tokens.Length.ToString() & " Token(s)")
	  End Sub

	Public ReadOnly InterpretationPayload_Lex As Action = _
	 Sub()
		 : Me.InvokeIfRequired(Sub() Me.StatusLabel.Text = "Status: Lexing...")
		 Me.Cached_Program = New DocScript.Runtime.Program(Me.Cached_Tokens, Me.CurrentExecutionContext)
		 : Me.InvokeIfRequired(Sub() Me.UpdateInterpretationButtonsState_(InterpretationStage_.Lexing))
		 : Me.InvokeIfRequired(Sub() Me.LastPerformedAction_InfoText.Text = "Constructed Program with " & Me.Cached_Program.Functions.Count.ToString() & " Function(s) and " & Me.Cached_Program.GlobalVarDecs.Count.ToString() & " Global VarDec(s)")
	 End Sub

	Public ReadOnly InterpretationPayload_Execute As Action(Of [String]()) = _
	  Sub(_CLAs$())
		  : Me.InvokeIfRequired(Sub() Me.StatusLabel.Text = "Status: Executing...")
		  Me.Cached_ProgramExeRes = Me.Cached_Program.Run(_CLAs)
		  : Me.InvokeIfRequired(Sub() Me.UpdateInterpretationButtonsState_(InterpretationStage_.Execution))
		  : Me.InvokeIfRequired(Sub() Me.LastPerformedAction_InfoText.Text = "Execution finished after " & Me.Cached_ProgramExeRes.ExecutionTimeMS.ToString() & "ms with ExitCode " & Me.Cached_ProgramExeRes.ReturnStatus.Program_ExitCode.ToString())
	  End Sub

#End Region

	''' <summary>Calls the ParseCurrentSource(), LexCachedTokens(), and ExecuteCachedProgram() Methods</summary>
	Public Sub RunCurrentSource() Handles RunButton.Click

		'Show a *kind* message if there isn't any source
		If System.String.IsNullOrEmpty(Me.SourceTextEditor.Text) Then : MsgBox("There is no Source to interpret in the Text-Editor.", MsgBoxStyle.Information, "DSIDE") : Return : End If
		Dim _RawSourceText$ = Me.SourceTextEditor.Text
		Dim _CLAs$() = Me.DocScriptCLAs_FromTextbox	'Must be procured here, because the BackgroundWorker Thread cannot access UI Controls

		Me.PerformAnyRequiredLoggingInitialisation_PreInterpretation()

		'Exceptions hence ↓ are MsgBoxed out
		Me.StartBackgroundWorker("Interpreting...",
		 Sub()

			 Me.InterpretationPayload_Parse.Invoke(_RawSourceText)
			 Me.InterpretationPayload_Lex.Invoke()
			 Me.InterpretationPayload_Execute.Invoke(_CLAs)

		 End Sub
		)

	End Sub

	''' <summary>Sets the Me.Cached_Tokens Member, and Applies SyntaxHighlighting</summary>
	Public Sub ParseCurrentSource() Handles ParseButton.Click
		Try

			'Show a *kind* message if there isn't any source
			If System.String.IsNullOrEmpty(Me.SourceTextEditor.Text) Then : MsgBox("There is no Source to interpret in the Text-Editor.", MsgBoxStyle.Information, "DSIDE") : Return : End If
			Dim _RawSourceText$ = Me.SourceTextEditor.Text

			Me.PerformAnyRequiredLoggingInitialisation_PreInterpretation()
			Me.StartBackgroundWorker("Parsing...", Sub() Me.InterpretationPayload_Parse.Invoke(_RawSourceText))

		Catch _Ex As Exception
			REM This {Catch} is only hit, by an Exception from outside the BackgroundWorker Sub
			MsgBox("On parsing the DocScript Source:" & vbCrLf & vbCrLf & _Ex.Message, MsgBoxStyle.Critical, _Ex.GetType().Name)
		End Try
	End Sub

	''' <summary>Sets the Me.Cached_Program Member</summary>
	Public Sub LexCachedTokens() Handles LexButton.Click
		Try

			Me.Cached_Tokens.MustNotBeNothing("There were no Cached Tokens; Parsing must occur first.")

			Me.PerformAnyRequiredLoggingInitialisation_PreInterpretation()
			Me.StartBackgroundWorker("Lexing...", Me.InterpretationPayload_Lex)

		Catch _Ex As Exception
			REM This {Catch} is only hit, by an Exception from outside the BackgroundWorker Sub
			MsgBox("On lexing the DocScript Tokens:" & vbCrLf & vbCrLf & _Ex.Message, MsgBoxStyle.Critical, _Ex.GetType().Name)
		End Try
	End Sub

	''' <summary>Sets the Me.Cached_ProgramExeRes Member</summary>
	Public Sub ExecuteCachedProgram() Handles ExecuteButton.Click
		Try

			Me.Cached_Program.MustNotBeNothing("There was no Cached Program. Parsing and Lexing must occur first")
			Dim _CLAs$() = Me.DocScriptCLAs_FromTextbox	'Must be procured here, because the BackgroundWorker Thread cannot access UI Controls

			Me.PerformAnyRequiredLoggingInitialisation_PreInterpretation()
			Me.StartBackgroundWorker("Executing...", Sub() Me.InterpretationPayload_Execute.Invoke(_CLAs))

		Catch _Ex As Exception
			REM This {Catch} is only hit, by an Exception from outside the BackgroundWorker Sub
			MsgBox("On executing the DocScript Program:" & vbCrLf & vbCrLf & _Ex.Message, MsgBoxStyle.Critical, _Ex.GetType().Name)
		End Try
	End Sub

	REM Used by UpdateInterpretationButtonsState_()
	Protected Enum InterpretationStage_ As [Byte]
		'These Integers are used as Index-Keys for the [_InterpretationButtons_] Array
		NoInterpretationPerformedYet = 0 : Parsing = 1 : Lexing = 2 : Execution = 3
	End Enum

	''' <summary>
	''' Enables the buttons up-to-and-including _FurthestDownInterpretationButtonToBeEnabled, and sets all buttons BEFORE _FurthestDownInterpretationButtonToBeEnabled to the GREEN Gradient Background Colour.
	''' Handles the Program Analysis Buttons corrosponding too.
	''' DOES NOT Manage which Interpretation-Cache-Objects should be set to [Nothing]. (This functionality was removed as it was causing major bugs.)
	''' </summary>
	Protected Sub UpdateInterpretationButtonsState_(ByVal _FurthestInterpretationStageCompleted As InterpretationStage_)

		Static _InterpretationButtons_ As Ribbon.RibbonButton() = {Me.ParseButton, Me.LexButton, Me.ExecuteButton}
		Static _ProgramAnalysisButtons_ As Ribbon.RibbonButton() = {Me.GenerateTokensTableButton, Me.GenerateProgTreeButton, Me.GenerateExeResTreeButton}

		Static _GreenBackgroundBrush_ As New LinearGradientBrush(Colors.PaleGreen, Colors.Aquamarine, New Point(0.1, 0), New Point(0.9, 1))

		REM State-Map for [_FurthestInterpretationStageCompleted]:
		'
		'	[0] NoInterpretationPerformedYet:
		'		Interpret Enabled:	ParseButton
		'		Interpret Green:	/
		'		Analysis Enabled:	/
		'		Analysis Green:		/
		'		Interpret Cache:	/
		'
		'	[1] Parsing:
		'		Interpret Enabled:	ParseButton, LexButton
		'		Interpret Green:	ParseButton
		'		Analysis Enabled:	GenerateTokensTableButton
		'		Analysis Green:		GenerateTokensTableButton
		'		Interpret Cache:	Cached_Tokens
		'
		'	[2] Lexing:
		'		Interpret Enabled:	ParseButton, LexButton, ExecuteButton
		'		Interpret Green:	ParseButton, LexButton
		'		Analysis Enabled:	GenerateTokensTableButton, GenerateProgTreeButton
		'		Analysis Green:		GenerateTokensTableButton, GenerateProgTreeButton
		'		Interpret Cache:	Cached_Tokens, Cached_Program
		'
		'	[3] Execution:
		'		Interpret Enabled:	ParseButton, LexButton, ExecuteButton
		'		Interpret Green:	ParseButton, LexButton, ExecuteButton
		'		Analysis Enabled:	GenerateTokensTableButton, GenerateProgTreeButton, GenerateExeResTreeButton
		'		Analysis Green:		GenerateTokensTableButton, GenerateProgTreeButton, GenerateExeResTreeButton
		'		Interpret Cache:	Cached_Tokens, Cached_Program, Cached_ProgramExeRes


		REM ∴ Equations derived from this:
		'	Highest Interpret Enabled	= _InterpretationButtons_		At [_FurthestInterpretationStageCompleted]
		'	Highest Interpret Green		= _InterpretationButtons_		At [_FurthestInterpretationStageCompleted - 1]
		'	Highest Analysis Enabled	= _ProgramAnalysisButtons_		At [_FurthestInterpretationStageCompleted - 1]
		'	Highest Analysis Green		= _ProgramAnalysisButtons_		At [_FurthestInterpretationStageCompleted - 1]
		'	Highest Interpret Cache		= _InterpretationCacheObjects_	At [_FurthestInterpretationStageCompleted - 1]



		REM Reset all Buttons to disabled, with the normal Background
		For Each _InterpretationButton As Ribbon.RibbonButton In _InterpretationButtons_ : _InterpretationButton.IsEnabled = False : _InterpretationButton.Background = Windows.Media.Brushes.Transparent : Next
		For Each _ProgAnalysisButton As Ribbon.RibbonButton In _ProgramAnalysisButtons_ : _ProgAnalysisButton.IsEnabled = False : _ProgAnalysisButton.Background = Windows.Media.Brushes.Transparent : Next

		'Note: In Visual B.A.S.I.C. .NET, a For-Loop iterating from 0 To 0 WILL ITERATE ONCE

		REM Enable Interpretation Buttons up to [_FurthestInterpretationStageCompleted]
		For _InterpretationButtonToEnable_Index% = 0 To Math.Min(val1:=CInt(_FurthestInterpretationStageCompleted), val2:=_InterpretationButtons_.Length - 1) Step +1
			'The Math.Min() is there above, because if _FurthestInterpretationStageCompleted is [3]Execution, then this For-Loop would attempt to set a non-existant 4th InterpretationButton ([3]) to enabled.
			'This doesn't need to occur, since the final InterpretationStage ([3]) is an edge-case whereby there is NO NEW InterpretationButton TO ENABLE
			_InterpretationButtons_(_InterpretationButtonToEnable_Index%).IsEnabled = True
		Next

		REM Make Interpretation Buttons Green up to [_FurthestInterpretationStageCompleted - 1]
		REM Enable Analysis Buttons up to [_FurthestInterpretationStageCompleted - 1]
		REM Make Analysis Buttons Green up to [_FurthestInterpretationStageCompleted - 1]
		For _Index% = 0 To CInt(_FurthestInterpretationStageCompleted) - 1 Step +1
			_InterpretationButtons_(_Index%).Background = _GreenBackgroundBrush_
			_ProgramAnalysisButtons_(_Index%).IsEnabled = True
			_ProgramAnalysisButtons_(_Index%).Background = _GreenBackgroundBrush_
		Next

	End Sub

	Public Sub LaunchDSCompilationWindow() Handles DSCompilationButton.Click
		Try

			'Show a *kind* message if there isn't any source
			If System.String.IsNullOrEmpty(Me.SourceTextEditor.Text) Then : MsgBox("There is no Source for the DS-Compilation Program in the Text-Editor.", MsgBoxStyle.Information, "DSIDE") : Return : End If
			Dim _RawSourceText$ = Me.SourceTextEditor.Text

			Me.PerformAnyRequiredLoggingInitialisation_PreInterpretation()

			Me.StartBackgroundWorker(
			 "Using DS-Compilation...",
			 Sub()

				 Me.InterpretationPayload_Parse.Invoke(_RawSourceText)
				 Me.InterpretationPayload_Lex.Invoke()

				 'The Program name is used for auto-filling the Default Output-path for the EXE
				 If Me.CurrentlyOpenFile IsNot Nothing Then Me.Cached_Program.Name = Me.CurrentlyOpenFile.FullName

				 : Me.InvokeIfRequired(Sub() Me.StatusLabel.Text = "Status: Using DS-Compilation...")
				 Call (New DSCompilationWindow(Me.Cached_Program)).ShowDialog()
				 : Me.InvokeIfRequired(Sub() Me.LastPerformedAction_InfoText.Text = "Finished DS-Compilation session")

			 End Sub
			)

		Catch _Ex As Exception
			REM This {Catch} is only hit, by an Exception from outside the BackgroundWorker Sub
			MsgBox("The Text-Editor's source could not be constructed into a Program for DS-Compilation" & vbCrLf & vbCrLf & _Ex.Message, MsgBoxStyle.Critical)
		End Try
	End Sub

	Public Sub LaunchDSRemotingWindow() Handles DSRemotingButton.Click
		Try

			'Show a *kind* message if there isn't any source
			If System.String.IsNullOrEmpty(Me.SourceTextEditor.Text) Then : MsgBox("There is no Source for the DS-Remoting Program in the Text-Editor.", MsgBoxStyle.Information, "DSIDE") : Return : End If
			Dim _RawSourceText$ = Me.SourceTextEditor.Text

			Me.PerformAnyRequiredLoggingInitialisation_PreInterpretation()

			Me.StartBackgroundWorker(
			 "Using DS-Remoting...",
			 Sub()

				 Me.InterpretationPayload_Parse.Invoke(_RawSourceText)
				 Me.InterpretationPayload_Lex.Invoke()

				 'Not used by DS-Remoting, but there's no harm in setting it
				 If Me.CurrentlyOpenFile IsNot Nothing Then Me.Cached_Program.Name = Me.CurrentlyOpenFile.FullName

				 : Me.InvokeIfRequired(Sub() Me.StatusLabel.Text = "Status: Using DS-Remoting...")
				 Call (New DSRemotingWindow(Me.Cached_Program)).ShowDialog()
				 : Me.InvokeIfRequired(Sub() Me.LastPerformedAction_InfoText.Text = "Finished DS-Remoting session")

			 End Sub
			)

		Catch _Ex As Exception
			REM This {Catch} is only hit, by an Exception from outside the BackgroundWorker Sub
			MsgBox("The Text-Editor's source could not be constructed into a Program for DS-Remoting" & vbCrLf & vbCrLf & _Ex.Message, MsgBoxStyle.Critical)
		End Try
	End Sub

#End Region

End Class