Class MainWindow

	Public Property Cached_Tokens As DocScript.Runtime.Token() = Nothing 'Parsing Output
	Public Property Cached_Program As DocScript.Runtime.Program = Nothing 'Lexing Output
	Public Property Cached_ProgramExeRes As DocScript.Language.Instructions.ExecutionResult = Nothing 'Execution Output

	Public Property CurrentExecutionContext As DocScript.Runtime.ExecutionContext 'Initialised in the Constructor, because with an inline initialisation, ExeCxt.GUIDefault attempt to Log a message, and can't (because the CurrentLogEventHandler isn't yet initialised)

	Protected Property AvalonEdit_SearchPanel_ As New ICSharpCode.AvalonEdit.Search.SearchPanel()

	Public Sub New()

		' This call is required by the designer.
		Me.InitializeComponent()
		System.Windows.Forms.Application.EnableVisualStyles()

		Me.Title = "DocScript IDE (" & Environment.UserName & " on \\" & My.Computer.Name & ")"c
		DocScript.Logging.CurrentLogEventHandler = DocScript.Logging.BuiltInLogEventHandlers.SilenceAll	'TextFile(New IO.FileInfo("D:\Benedict\Documents\SchoolWork\Projects\DocScript\Resources\DocScript.Log"))
		Me.CurrentExecutionContext = DocScript.Runtime.ExecutionContext.GUIDefault

		RegisterCodeSnippetInsertion_EventHandlers_()
		Me.InitialiseTextEditorControl_()
		Me.InitializeComponent()

		CLAManagment.ExamineCLAs_And_SetPassthroughVariables() 'Variables including CLAManagment.CLAPassthrough_CLAHelpDictionaryText are then later picked-up on.

	End Sub

	'WPF substitute for a Me.Shown Handler...
	Public Sub ConfigureInstance_FromCLAs() Handles Me.ContentRendered
		Try

			REM Pick-up on and Data passed through from the CLA-Manager
			If CLAManagment.CLAPassthrough_CLAHelpDictionaryText <> Nothing Then Me.SourceTextEditor.Text = CLAManagment.CLAPassthrough_CLAHelpDictionaryText
			If CLAManagment.CLAPassthrough_RunWhenReady AndAlso (CLAManagment.CLAPassthrough_SourceFileToOpen Is Nothing) Then Throw New DSException("The /Run Argument is invalid, because no DocScript Program was specified the /OpenSourceFile Argument")
			If CLAManagment.CLAPassthrough_SourceFileToOpen IsNot Nothing Then Me.OpenFile_FromCLAs(CLAManagment.CLAPassthrough_SourceFileToOpen, Sub() If CLAManagment.CLAPassthrough_RunWhenReady Then Me.RunCurrentSource())

		Catch _Ex As Exception
			[Interaction].MsgBox("On Initialising the UI Components:" & vbCrLf & vbCrLf & _Ex.Message, MsgBoxStyle.Exclamation, _Ex.GetType().Name)
		End Try
	End Sub

	Public Sub HandleShortcutKey(ByVal _Sender As Object, ByVal _KeyEventArgs As KeyEventArgs) Handles Me.KeyDown

		If (Keyboard.Modifiers = ModifierKeys.Control) AndAlso (_KeyEventArgs.Key = Key.F5) Then : MsgDebug("Ctrl+F5")		'Ctrl + F5
		ElseIf (Keyboard.Modifiers = ModifierKeys.Control) AndAlso (_KeyEventArgs.Key = Key.N) Then : Me.StartNewFile()		'Ctrl + N
		ElseIf (Keyboard.Modifiers = ModifierKeys.Control) AndAlso (_KeyEventArgs.Key = Key.O) Then : Me.OpenFile()			'Ctrl + O
		ElseIf (Keyboard.Modifiers = ModifierKeys.Control) AndAlso (_KeyEventArgs.Key = Key.S) Then : Me.SaveFile()			'Ctrl + S
		ElseIf (Keyboard.Modifiers = ModifierKeys.Control) AndAlso (_KeyEventArgs.Key = Key.H) Then : Me.ShowHelpWindow()	'Ctrl + H
		ElseIf (Keyboard.Modifiers = (ModifierKeys.Control Or ModifierKeys.Shift)) AndAlso (_KeyEventArgs.Key = Key.N) Then : Me.StartNewDSIDEInstance()		'Ctrl + Shift + N
		ElseIf (Keyboard.Modifiers = (ModifierKeys.Control Or ModifierKeys.Shift)) AndAlso (_KeyEventArgs.Key = Key.O) Then : Me.OpenContainingFolder()			'Ctrl + Shift + O
		ElseIf (Keyboard.Modifiers = (ModifierKeys.Control Or ModifierKeys.Shift)) AndAlso (_KeyEventArgs.Key = Key.S) Then : Me.SaveFileAs()					'Ctrl + Shift + S
		ElseIf (Keyboard.Modifiers = (ModifierKeys.Control Or ModifierKeys.Shift)) AndAlso (_KeyEventArgs.Key = Key.T) Then : Me.ShowProgramTree_InNewWindow()	'Ctrl + Shift + T
		ElseIf (Keyboard.Modifiers = (ModifierKeys.Control Or ModifierKeys.Shift)) AndAlso (_KeyEventArgs.Key = Key.R) Then : Me.ShowExeResTree_InNewWindow()	'Ctrl + Shift + R
		ElseIf (Keyboard.Modifiers = (ModifierKeys.Control Or ModifierKeys.Shift)) AndAlso (_KeyEventArgs.Key = Key.B) Then : Me.ShowNewBIFExplorerWindow()		'Ctrl + Shift + B
		ElseIf (Keyboard.Modifiers = (ModifierKeys.Control Or ModifierKeys.Shift)) AndAlso (_KeyEventArgs.Key = Key.H) Then : Call (New PictorialHelpWindow()).Show() 'Ctrl + Shift + H
		ElseIf _KeyEventArgs.Key = Key.F1 Then : Me.ParseCurrentSource()	'F1
		ElseIf _KeyEventArgs.Key = Key.F2 Then : Me.LexCachedTokens()		'F2
		ElseIf _KeyEventArgs.Key = Key.F3 Then : Me.ExecuteCachedProgram()	'F3
		ElseIf _KeyEventArgs.Key = Key.F5 Then : Me.RunCurrentSource()		'F5
		Else : Return 'Don't set the Handled as below...
		End If

		_KeyEventArgs.Handled = True 'Don't type the Keys into Me.SourceTextEditor

	End Sub

	Protected Sub RegisterCodeSnippetInsertion_EventHandlers_()

		AddHandler Insert_SampleProgram_HelloWorld.Click, Sub() Me.InsertTextAtCursor(PredefinedSnippets.SampleProgram_HelloWorld)
		AddHandler Insert_SampleProgram_BeepSleepRepeat.Click, Sub() Me.InsertTextAtCursor(PredefinedSnippets.SampleProgram_BeepSleepRepeat)
		AddHandler Insert_SampleProgram_DebuggingExample.Click, Sub() Me.InsertTextAtCursor(PredefinedSnippets.SampleProgram_DebuggingExample)

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

		Dim _BackgroundWorker As New System.ComponentModel.BackgroundWorker()

		_BackgroundWorker.WorkerReportsProgress = False : _BackgroundWorker.WorkerSupportsCancellation = True

		REM If the Abort link is pressed, cancel the task
		Dim _CancelBackgroundWorker_Action As New Windows.Input.MouseButtonEventHandler(
		 Sub()
			 _BackgroundWorker.CancelAsync() 'Sets [CancellationPending] to True
			 RemoveHandler Me.AbortBackgroundWorkerLink.PreviewMouseDown, _CancelBackgroundWorker_Action
			 Me.InvokeIfRequired(AddressOf Me.LoadingUIComponents_Reset)
		 End Sub)
		AddHandler Me.AbortBackgroundWorkerLink.PreviewMouseDown, _CancelBackgroundWorker_Action

		AddHandler _BackgroundWorker.DoWork, _
		 Sub(_Sender As Object, _DoWorkEA As ComponentModel.DoWorkEventArgs)

			 Dim _CurrentBackgroundWorker As ComponentModel.BackgroundWorker = CType(_Sender, ComponentModel.BackgroundWorker)
			 Dim _InnerWorkerThread As New System.Threading.Thread(
			   Sub()
				   Try
					   _Action.Invoke()
				   Catch _Exception As Exception When True
					   REM Don't show the MsgBox if the user has clicked "Abort"...
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

		AddHandler _BackgroundWorker.RunWorkerCompleted, _
		 Sub(_Sender As Object, _CompletedEA As ComponentModel.RunWorkerCompletedEventArgs)
			 If _CompletedEA.Error IsNot Nothing Then
				 Dim _Exception As System.Exception = CType(_CompletedEA.Error, Exception)
				 MsgBox("A Fatal Exception was Thrown whilst """ & _TaskStatusDescription & """:" & vbCrLf & vbCrLf & _Exception.Message, MsgBoxStyle.Critical, _Exception.GetType().FullName)
			 End If
			 RemoveHandler Me.AbortBackgroundWorkerLink.PreviewMouseDown, _CancelBackgroundWorker_Action
			 Me.InvokeIfRequired(AddressOf Me.LoadingUIComponents_Reset)
			 Me.InvokeIfRequired(AddressOf Me.Activate) 'Make the MainWindow focoused again
		 End Sub

		_BackgroundWorker.RunWorkerAsync()

	End Sub

#Region "SourceTextEditor-related"

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

		Dim _XSHD_ByteArray As Byte() = My.Resources.DocScript_SyntaxHighlighting
		Dim _XSHD_XMLString As String = System.Text.Encoding.ASCII.GetString(_XSHD_ByteArray)

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
		If Not (_TextCompositionEA.Text.First() = "_"c) Then Return

		Me.IntelliSenseWindow_ = New Global.ICSharpCode.AvalonEdit.CodeCompletion.CompletionWindow(Me.SourceTextEditor.TextArea) With {.Width = 250}

		Dim _DSIntellisenseEntries As IList(Of Global.ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData) = Me.IntelliSenseWindow_.CompletionList.CompletionData

		REM Add all non-array DataTypes
		For Each _DataType$ In Global.DocScript.Language.Constants.AllNonArrayDataTypes
			_DSIntellisenseEntries.Add(New DSIntelliSenseEntry(_Text:=_DataType, _Description:="[DocScript DataType]", _Image:=DSIntelliSenseEntry.DSIntellisenseEntryImage.DataType))
		Next

		REM Add all Keywords
		For Each _Keyword$ In Global.DocScript.Language.Constants.AllKeywords
			_DSIntellisenseEntries.Add(New DSIntelliSenseEntry(_Text:=_Keyword, _Description:="[DocScript Keyword]", _Image:=DSIntelliSenseEntry.DSIntellisenseEntryImage.Keyword))
		Next

		REM Add all BIFs
		For Each _BIF As DocScript.Runtime.BuiltInFunction In Me.CurrentExecutionContext.BuiltInFunctions
			_DSIntellisenseEntries.Add(New DSIntelliSenseEntry(_Text:=_BIF.Identifier, _Description:="[BuiltInFunction] " & _BIF.Description, _Image:=DSIntelliSenseEntry.DSIntellisenseEntryImage.BuiltInFunction))
		Next

		Me.IntelliSenseWindow_.Show()

		AddHandler Me.IntelliSenseWindow_.Closed, Sub() Me.IntelliSenseWindow_ = Nothing

	End Sub

	Protected Sub TextArea_TextEntering_(ByVal _Sender As Object, ByVal _TextCompositionEA As TextCompositionEventArgs)

		If (_TextCompositionEA.Text.Length > 0) AndAlso (Me.IntelliSenseWindow_ IsNot Nothing) Then
			'Whenever a non-letter is typed while the completion window is open,
			'insert the currently selected element.
			If Not Char.IsLetterOrDigit(_TextCompositionEA.Text.First()) Then Me.IntelliSenseWindow_.CompletionList.RequestInsertion(_TextCompositionEA)
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
			If Me.CurrentlyOpenFile Is Nothing Then	'Don't change anything...
			ElseIf _SourceIsSaved Then : Me.Title = Me.CurrentlyOpenFile.Name
			Else : Me.Title = "(Unsaved) " & Me.CurrentlyOpenFile.Name
			End If
		End Set
	End Property
	Protected CurrentSource_IsSaved_ As Boolean = False

	REM Drag-and-Drop Open
	Private Sub LayoutRoot_Drop(ByVal _Sender As Object, ByVal _DragEventArgs As System.Windows.DragEventArgs) Handles LayoutRoot.Drop

		If ((Me.SourceTextEditor.Text.Length > 0) AndAlso (Not Me.CurrentSource_IsSaved)) AndAlso (MsgBox("The Source-text in the Editor may be unsaved; it will be lost if a new file is opened." & vbCrLf & vbCrLf & "Do you still want to Open the new DocScript File?", MsgBoxStyle.YesNo Or MsgBoxStyle.Question, "Source may be deleted") = MsgBoxResult.No) Then Return

		Me.StartBackgroundWorker("Opening Dropped File...",
		 Sub()

			 If _DragEventArgs.Data.GetDataPresent(DataFormats.FileDrop) Then

				 Dim _File As New IO.FileInfo(CType(_DragEventArgs.Data.GetData(DataFormats.FileDrop), String()).First())
				 If Not _File.Exists Then Throw New DSValidationException("The file specified via the Drop-Event does not exist.", _File.FullName)

				 Me.CurrentlyOpenFile = _File
				 Dim _SourceText_FromFile$ = My.Computer.FileSystem.ReadAllText(Me.CurrentlyOpenFile.FullName)
				 Me.InvokeIfRequired(Sub()
										 Me.SourceTextEditor.Text = _SourceText_FromFile
										 Me.CurrentSource_IsSaved = True 'Sets the Window Title accordingly
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
					   If If(.ShowDialog(), False) Then
						   Me.CurrentlyOpenFile = New IO.FileInfo(.FileName)
					   Else : Return 'If cancelled, don't save the File
					   End If
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
				   If If(.ShowDialog(), False) Then : Me.CurrentlyOpenFile = New IO.FileInfo(.FileName)
				   Else : Return 'If cancelled, don't save the File
				   End If
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
		Me.CurrentSource_IsSaved = False : Me.Title = "DocScript IDE (" & Environment.UserName & " on \\" & My.Computer.Name & ")"c
		Me.LastPerformedAction_InfoText.Text = "Started New (unsaved) File"

	End Sub

#End Region

#Region "Misc. Control Event Handlers"

	REM Used in RegisterCodeSnippetInsertion_EventHandlers_()
	Public Sub InsertTextAtCursor(ByVal _Text$)
		Me.SourceTextEditor.Document.Insert(Me.SourceTextEditor.CaretOffset, _Text)
	End Sub

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

	Protected Sub HandleCtrlScroll_(ByVal _Sender As Object, ByVal _MouseWheelEventArgs As MouseWheelEventArgs) Handles Me.PreviewMouseWheel
		If Keyboard.Modifiers = ModifierKeys.Control Then
			If _MouseWheelEventArgs.Delta > 0 Then Me.ZoomValueSlider.Value += 0.2 'MsgBox("ZoomIn()")
			If _MouseWheelEventArgs.Delta < 0 Then Me.ZoomValueSlider.Value -= 0.2 'MsgBox("ZoomOut()")
		End If
	End Sub

	Public Sub UpdateLineAndColLabel(ByVal _Sender As Object, ByVal _EventArgs As EventArgs) 'Handler set in InitialiseTextEditorControl_()
		Dim _CaretLocation As Global.ICSharpCode.AvalonEdit.Document.TextLocation = Me.SourceTextEditor.Document.GetLocation(Me.SourceTextEditor.CaretOffset)
		Me.CaretLineColumnLabel.Text = String.Format("Line: {0}, Col: {1}", _CaretLocation.Line.ToString(), _CaretLocation.Column.ToString())
	End Sub

	Public Sub UpdateLineCountLabel_And_AlterIsSavedState() Handles SourceTextEditor.TextChanged

		Me.LineCountLabel.Text = (Me.SourceTextEditor.Text.ToCharArray().Where(Function(_Char As Char) _Char = vbLf.First()).Count() + 1).ToString() & " Line(s)"
		Me.CurrentSource_IsSaved = False 'Sets the Window Title accordingly. If there's no open file, then the Title dosen't change.

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

	Public Sub ZoomIn() Handles ZoomInButton.MouseUp
		Me.ZoomValueSlider.Value = 2
	End Sub

	Public Sub ZoomOut() Handles ZoomInButton.MouseUp
		Me.ZoomValueSlider.Value = 0.5
	End Sub

	Public Sub ShowNewBIFExplorerWindow() Handles ViewBIFsButton.Click
		Dim _NewBIFExplorerWindow As New BIFExplorerWindow(Me) : _NewBIFExplorerWindow.Show()
	End Sub

	Public Sub LaunchDSExpr() Handles OpenDSExprButton.Click
		Dim _DSExpr_ExeFile As New IO.FileInfo("DSExpr.exe")
		Try
			Process.Start(_DSExpr_ExeFile.FullName)
		Catch _Ex As Exception
			MsgBox("On attempting to launch " & _DSExpr_ExeFile.FullName.InSquares() & ":" & vbCrLf & vbCrLf & _Ex.Message, MsgBoxStyle.Critical, _Ex.GetType().Name)
		End Try
	End Sub

	Public Sub RunInDSCLI() Handles RunInDSCLIButton.Click
		Dim _DSCLI_ExeFile As New IO.FileInfo("DSCLI.exe")
		Try

			REM Ensure that we have a CurrentlyOpenFile
			If Me.CurrentlyOpenFile Is Nothing Then Throw New NullReferenceException("There is no currently-open File, whose path can be passed to DSCLI." & vbCrLf & vbCrLf & "Open a File first with Ctrl + O...")

			REM Run DSCLI.EXE
			Process.Start(
			 _DSCLI_ExeFile.FullName,
			 String.Format("/PromptBeforeExit /Run /SourceFile:""{0}"" {1}", Me.CurrentlyOpenFile.FullName, If(String.IsNullOrEmpty(Me.ProgramCLAsTextBox.Text), String.Empty, "/DocScriptCLAs:""" & Me.ProgramCLAsTextBox.Text & """"))
			)

		Catch _Ex As Exception
			MsgBox("On attempting to launch " & _DSCLI_ExeFile.FullName.InSquares() & ":" & vbCrLf & vbCrLf & _Ex.Message, MsgBoxStyle.Critical, _Ex.GetType().Name)
		End Try
	End Sub

	Public Sub ShowAboutBox() Handles AboutMenuItem.Click
		Call (New DSAboutBox()).ShowDialog()
	End Sub

	Public Sub ShowHelpWindow() Handles HelpMenuItem.Click, PaneHelpButton.Click
		Call (New HelpWindow()).Show()
	End Sub

	Public Sub ShowProgramTree_InNewWindow() Handles GenerateProgTreeButton.Click
		Try
			Dim _NewProgramTreeWindow As New ProgramTreeViewer(Me.Cached_Program.MustNotBeNothing("A Program must first be created by Parsing and Lexing"))
			_NewProgramTreeWindow.Show()
		Catch _Ex As Exception
			MsgBox("On Generating the Program Tree:" & vbCrLf & vbCrLf & _Ex.Message, MsgBoxStyle.Critical, _Ex.GetType().Name)
		End Try
	End Sub

	Public Sub ShowExeResTree_InNewWindow() Handles GenerateExeResTreeButton.Click
		Try
			Dim _NewExeResTreeWindow As New ExeResExplorerWindow(Me.Cached_ProgramExeRes.MustNotBeNothing("A Program must first be Executed, to create the cached ExecutionResult"))
			_NewExeResTreeWindow.Show()
		Catch _Ex As Exception
			MsgBox("On Generating the Execution-Result Tree:" & vbCrLf & vbCrLf & _Ex.Message, MsgBoxStyle.Critical, _Ex.GetType().Name)
		End Try
	End Sub

	Public Sub ShowSymbolTableViewingMsgBox() Handles ViewSymTblButton.Click
		MsgBox("This feature has been moved." & vbCrLf & vbCrLf & "Call ""Debug_ShowSymbolTables()"" from the DocScript Program, where you wish to inspect the Symbol-Tables' current states", MsgBoxStyle.Information, "DS-IDE")
	End Sub

#Region "Window Closure Handling"

	Public Sub AskToSaveUnsavedSource_ThenCloseProgram() Handles ExitMenuItem.Click
		If ((Me.SourceTextEditor.Text.Length > 0) AndAlso (Not Me.CurrentSource_IsSaved)) _
		AndAlso (MsgBox("The Source-text in the Editor may be unsaved; it will be lost if DSIDE is closed." & vbCrLf & vbCrLf & "Do you still want to exit?", MsgBoxStyle.YesNo Or MsgBoxStyle.Question, "Source may be deleted") = MsgBoxResult.No) _
		Then
			'Stay open
			Return
		Else
			Me.ExitIDE()
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
			Me.ExitIDE()
		End If
	End Sub

	Public Sub HandleEscKeyPress(ByVal _Sender As Object, ByVal _KeyEventArgs As KeyEventArgs) Handles Me.PreviewKeyDown
		If _KeyEventArgs.Key = Key.Escape Then Me.AskToSaveUnsavedSource_ThenCloseProgram()
	End Sub

	Public Sub ExitIDE()
		'End this Process, including the background thread
		Global.System.Windows.Application.Current.Shutdown()
		System.Diagnostics.Process.GetCurrentProcess().Kill()
	End Sub

#End Region

#Region "Logging-related"

	Public Sub UpdateWeatherOrNotToUseMsgBoxLogging() Handles UseMsgBoxLoggingCheckBox.Checked

		If UseMsgBoxLoggingCheckBox.IsChecked Then
			DocScript.Logging.CurrentLogEventHandler = DocScript.Logging.BuiltInLogEventHandlers.MsgBoxPerEvent
			Me.ShowDSLogCheckBox.IsChecked = True
		Else 'It's not Checked; don't use MsgBox Logging
			DocScript.Logging.CurrentLogEventHandler = DocScript.Logging.BuiltInLogEventHandlers.SilenceAll
			Me.ShowDSLogCheckBox.IsChecked = False
		End If

	End Sub

	Public Sub UpdateWeatherOrNotToShowDSLog() Handles ShowDSLogCheckBox.Checked

		If ShowDSLogCheckBox.IsChecked Then
			DocScript.Logging.CurrentLogEventHandler = DocScript.Logging.BuiltInLogEventHandlers.GUIDefault
			Me.UseMsgBoxLoggingCheckBox.IsChecked = False
		Else 'Don't show the Log
			DocScript.Logging.CurrentLogEventHandler = DocScript.Logging.BuiltInLogEventHandlers.SilenceAll
			Me.UseMsgBoxLoggingCheckBox.IsChecked = False
		End If

	End Sub

	Public Sub UpdateWeatherOrNotToProcessDebugEvents() Handles ProcessDebugEventsCheckBox.Checked
		DocScript.Logging.LogUtilities.ProcessDebugEvents = (Me.ProcessDebugEventsCheckBox.IsChecked.HasValue AndAlso Me.ProcessDebugEventsCheckBox.IsChecked.Value)
	End Sub

#End Region

#End Region

#Region "Interpretation Actions"

	''' <summary>Calls the ParseCurrentSource(), LexCachedTokens(), and ExecuteCachedProgram() Methods</summary>
	Public Sub RunCurrentSource() Handles RunButton.Click

		'Show a *kind* message if there isn't any source
		If System.String.IsNullOrEmpty(Me.SourceTextEditor.Text) Then : MsgBox("There is no Source to interpret in the Text-Editor.", MsgBoxStyle.Information, "DSIDE") : Return : End If

		Dim _RawSourceText$ = Me.SourceTextEditor.Text
		Dim _CLAs$() = Me.ProgramCLAsTextBox.Text.Split(" "c)

		'Exceptions hence ↓ are MsgBoxed out
		Me.StartBackgroundWorker("Interpreting...",
		 Sub()

			 : Me.InvokeIfRequired(Sub() Me.StatusLabel.Text = "Status: Parsing...")
			 Me.Cached_Tokens = DocScript.Runtime.Parser.GetTokensFromSource(_RawSourceText$)

			 : Me.InvokeIfRequired(Sub() Me.StatusLabel.Text = "Status: Lexing...")
			 Me.Cached_Program = New DocScript.Runtime.Program(Me.Cached_Tokens, Me.CurrentExecutionContext)
			 : Me.InvokeIfRequired(Sub() Me.GenerateProgTreeButton.IsEnabled = True)

			 : Me.InvokeIfRequired(Sub() Me.StatusLabel.Text = "Status: Executing...")
			 Me.Cached_ProgramExeRes = Me.Cached_Program.Run(_CLAs)
			 : Me.InvokeIfRequired(Sub() Me.GenerateExeResTreeButton.IsEnabled = True)
			 : Me.InvokeIfRequired(Sub() Me.LastPerformedAction_InfoText.Text = "Program finished in " & Me.Cached_ProgramExeRes.ExecutionTimeMS.ToString() & "ms with ExitCode " & Me.Cached_ProgramExeRes.ReturnStatus.Program_ExitCode.ToString())

		 End Sub
		)

	End Sub

	''' <summary>Sets the Me.Cached_Tokens Member, and Applies SyntaxHighlighting</summary>
	Public Sub ParseCurrentSource() Handles ParseButton.Click
		Try

			'Show a *kind* message if there isn't any source
			If System.String.IsNullOrEmpty(Me.SourceTextEditor.Text) Then : MsgBox("There is no Source to interpret in the Text-Editor.", MsgBoxStyle.Information, "DSIDE") : Return : End If

			Dim _RawSourceText$ = Me.SourceTextEditor.Text
			Me.StartBackgroundWorker(
			 "Parsing...",
			 Sub()
				 Me.Cached_Tokens = DocScript.Runtime.Parser.GetTokensFromSource(_RawSourceText$)
				 : Me.InvokeIfRequired(Sub() Me.LexButton.IsEnabled = True)	'Lexing can occur, now that we have the Cached_Tokens
				 : Me.InvokeIfRequired(Sub() Me.LastPerformedAction_InfoText.Text = "Parsed Source into " & Me.Cached_Tokens.Length.ToString() & " Token(s)")
			 End Sub
			)

		Catch _Ex As Exception
			MsgBox("On parsing the DocScript Source:" & vbCrLf & vbCrLf & _Ex.Message, MsgBoxStyle.Critical, _Ex.GetType().Name) : Me.LoadingUIComponents_Reset()
		End Try
	End Sub

	''' <summary>Sets the Me.Cached_Program Member</summary>
	Public Sub LexCachedTokens() Handles LexButton.Click
		Try
			Me.Cached_Tokens.MustNotBeNothing("There were no Cached Tokens. Parsing must occur first")
			Me.StartBackgroundWorker(
			 "Lexing...",
			 Sub()
				 Me.Cached_Program = New DocScript.Runtime.Program(Me.Cached_Tokens, Me.CurrentExecutionContext)
				 : Me.InvokeIfRequired(Sub() Me.GenerateProgTreeButton.IsEnabled = True)
				 : Me.InvokeIfRequired(Sub() Me.ExecuteButton.IsEnabled = True)	'Execution can occur, now that we have the Cached_Program
				 : Me.InvokeIfRequired(Sub() Me.LastPerformedAction_InfoText.Text = "Constructed Program with " & Me.Cached_Program.Functions.Count.ToString() & " Function(s) and " & Me.Cached_Program.GlobalVarDecs.Count.ToString() & "Global VarDec(s)")
			 End Sub
			)
		Catch _Ex As Exception
			MsgBox("On lexing the DocScript Source:" & vbCrLf & vbCrLf & _Ex.Message, MsgBoxStyle.Critical, _Ex.GetType().Name) : Me.LoadingUIComponents_Reset()
		End Try
	End Sub

	''' <summary>Sets the Me.Cached_ProgramExeRes Member</summary>
	Public Sub ExecuteCachedProgram() Handles ExecuteButton.Click
		Try
			Me.Cached_Program.MustNotBeNothing("There was no Cached Program. Parsing and Lexing must occur first")
			Dim _CLAs$() = Me.ProgramCLAsTextBox.Text.Split(" "c)
			Me.StartBackgroundWorker("Executing...",
			Sub()
				Me.Cached_ProgramExeRes = Me.Cached_Program.Run(_CLAs)
				: Me.InvokeIfRequired(Sub() Me.GenerateExeResTreeButton.IsEnabled = True)
				: Me.InvokeIfRequired(Sub() Me.LastPerformedAction_InfoText.Text = "Program finished in " & Me.Cached_ProgramExeRes.ExecutionTimeMS.ToString() & "ms with ExitCode " & Me.Cached_ProgramExeRes.ReturnStatus.Program_ExitCode.ToString())
			End Sub
			)
		Catch _Ex As Exception
			MsgBox("On executing the DocScript Source:" & vbCrLf & vbCrLf & _Ex.Message, MsgBoxStyle.Critical, _Ex.GetType().Name) : Me.LoadingUIComponents_Reset()
		End Try
	End Sub

#End Region

#Region "Window LoadingMode Setters"

	Public Sub LoadingUIComponents_SetForTaskStart(ByVal _TaskStatusDescription$)
		Me.InvokeIfRequired(
		Sub()
			Me.StatusLabel.Text = "Status: " & _TaskStatusDescription
			Me.StatusLabel.Foreground = System.Windows.Media.Brushes.DarkBlue
			Me.InterpretationProgressBar.IsIndeterminate = True
			Me.Cursor = Cursors.Wait
			Me.TheRibbon.IsEnabled = False : Me.SourceTextEditor.IsEnabled = False 'Prevent any other buttons etc from being pressed
			Me.AbortBackgroundWorkerLink.Text = " Abort " : Me.AbortBackgroundWorkerLink.IsEnabled = True

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

End Class