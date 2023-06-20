#If False Then

#Region "DSWindowsIDE, and - at this point in time - rather a large amount more of random left-overs"




DocScript.CompilerExtentions.UsefulMethods.ConsoleErrorWriteLineInColour("DSParse: Provide StdIn via piping, or by typing here and pressing [Ctrl + Z], [Enter] to submit...", ConsoleColor.Blue)















	Protected SourceTextEditor_HorozontalOffset, SourceTextEditor_VerticalOffset As [Double]

	Protected Sub SourceTextEditor_PreviewMouseWheel(ByVal _Sender As [Object], ByVal _MouseWheelEventArgs As MouseWheelEventArgs) Handles SourceTextEditor.PreviewMouseWheel
		If Keyboard.Modifiers = ModifierKeys.Shift Then
			Dim _ZoomFactor# = 100 / 100D
			Me.SourceTextEditor.ScrollTo(CInt((Me.SourceTextEditor_HorozontalOffset + -_MouseWheelEventArgs.Delta) / _ZoomFactor), CInt(Me.SourceTextEditor_VerticalOffset / _ZoomFactor))
			_MouseWheelEventArgs.Handled = True
		End If
	End Sub

	Protected Sub SourceTextEditor_ScrollChanged(ByVal _Sender As [object], ByVal _ScrollChangedEventArgs As ScrollChangedEventArgs) Handles SourceTextEditor.



















		REM Unfortunately, an array like {Me.Cached_Tokens, Me.Cached_Program, Me.Cached_ProgramExeRes} wouldn't referance the origional objects inside it for some reason (even though it does work with the Buttons' arrays above).
		REM Therefore, we'll have to manually account for each of the 4 eventualities...
		'Select Case _FurthestInterpretationStageCompleted
		'	Case InterpretationStage_.NoInterpretationPerformedYet : Me.Cached_Tokens = Nothing : Me.Cached_Program = Nothing : Me.Cached_ProgramExeRes = Nothing
		'	Case InterpretationStage_.Parsing : Me.Cached_Program = Nothing : Me.Cached_ProgramExeRes = Nothing
		'	Case InterpretationStage_.Lexing : Me.Cached_ProgramExeRes = Nothing
		'	Case InterpretationStage_.Execution
		'	Case Else : Throw New Exception("The Interpretation-Cache-Objects cannot be annulled, because the specified interpretation stage is unaccounted-for: " & _FurthestInterpretationStageCompleted.ToString())
		'End Select






		REM Annul the Interpretation-Cache-Objects which shouldn't exist for the current _FurthestInterpretationStageCompleted
		REM Walk down from the highest Interpretation-Cache-Object (ExeRes), To _FurthestInterpretationStageCompleted
		For _InterpretationCacheObjectToAnnul_Index% = (_InterpretationCacheObjects_.Length - 1) To _FurthestInterpretationStageCompleted Step -1
			_InterpretationCacheObjects_(_InterpretationCacheObjectToAnnul_Index) = Nothing
		Next













Public Sub HandleShiftScholling(ByVal _Sender As [Object], ByVal _EventArgs As System.Windows.Input.MouseWheelEventArgs) Handles SourceTextEditor.PreviewMouseWheel
		Debug.WriteLine("Scroll")
		If Keyboard.IsKeyDown(Key.LeftShift) OrElse Keyboard.IsKeyDown(Key.RightShift) Then
			Debug.WriteLine("Horozontal Scroll")
		End If
End Sub












Public Sub ApplySyntaxHighlighting()

		Me.Cached_Tokens.MustNotBeNothing("There were no Cached Tokens. Parsing must occur first")
		Me.InvokeIfRequired(AddressOf Me.SourceRichTextBox.Focus)

		For Each _Token As DocScript.Runtime.Token In Me.Cached_Tokens

			'Don't worry about LineEnds
			If _Token.Type = Runtime.Token.TokenType.LineEnd Then Continue For

			Dim _LambdaCopyOf_Token As Runtime.Token = _Token

			'MsgDebug("Applying SyntaxHighlighting to Token:" & vbCrLf & _LambdaCopyOf_Token.Value)
			Me.InvokeIfRequired(
			  Sub()
				  Me.SourceRichTextBox.ApplyColourToSelection(
				  _SelectionStart:=New Tuple(Of UInt32, UInt32)(_LambdaCopyOf_Token.LocationInSource.Line, _LambdaCopyOf_Token.LocationInSource.Column),
				  _SelectionLength:=CUInt(_LambdaCopyOf_Token.Value.Length),
				  _NewColour_Brush:=Global.DocScript.Resources.TokenTypeToBrush_Dictionary.Item(_LambdaCopyOf_Token.Type),
				  _TEMP_REMOVE_THIS_PARAM_TokenValue:=_LambdaCopyOf_Token.Value
				  )
			  End Sub
			)

		Next

	End Sub




''' <summary>(MullNet CompilerExtention) Makes the text at the specified Selection into the _NewColour. Item1=Line; Item2=Column.</summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Sub ApplyColourToSelection(ByRef _RichTextBox As Windows.Controls.RichTextBox, ByVal _SelectionStart As Tuple(Of UInt32, UInt32), ByVal _SelectionLength As UInt32, ByVal _NewColour_Brush As System.Windows.Media.Brush, ByVal _TEMP_REMOVE_THIS_PARAM_TokenValue$)
			Try

				Colorize(_RichTextBox, 45, 11, Windows.Media.Brushes.Green) : Return

				'Takes in the LineNumber, Returns the No. Chars on that Line (including the trailing LineBreak)
				Dim _GetCharCount_OnLine As Func(Of String, UInt16, UInt32) = _
				 Function(_AllText$, _LineNumber As UInt16) As UInt32

					 Dim _Lines$() = _AllText.Split({vbCrLf}, StringSplitOptions.None)
					 Dim _TargetLine$ = _Lines.ElementAt(_LineNumber)
					 'MsgDebug("Line " & _LineNumber & vbCrLf & _TargetLine & vbCrLf & CUInt(_TargetLine.Length + 2) & "Char(s)")
					 Return CUInt(_TargetLine.Length + 2) 'Add 2 for the [Cr and Lf] we Split() by

				 End Function

				'Returns the Index of the Token's First Char
				Dim _GetTokenStart_CharIndex As Func(Of String, UInt32) = _
				 Function(_AllText$) As UInt32

					 Dim _TokenStart_CharIndex As UInt32 = 0
					 For _PreTokenLine_Index As UInt32 = 0 To CUInt(_SelectionStart.Item1 - 2) Step +1
						 _TokenStart_CharIndex += _GetCharCount_OnLine.Invoke(_AllText, CUShort(_PreTokenLine_Index))
					 Next
					 'MsgDebug("Token " & _TEMP_REMOVE_THIS_PARAM_TokenValue & " starts at:" & vbCrLf & (_TokenStart_CharIndex + CUInt(_SelectionStart.Item2)))
					 Return _TokenStart_CharIndex + CUInt(_SelectionStart.Item2) 'Item2 = ColumnNumber

				 End Function




				REM Calculate the Offset in Chars, at which the current Token Begins...
				Dim _TokenText_IndexOfFirstChar As UInt32 = _GetTokenStart_CharIndex.Invoke(_RichTextBox.GetText())
				'Dim _WholeDocument As New Windows.Documents.TextRange(_RichTextBox.Document.ContentStart, _RichTextBox.Document.ContentEnd)

				'Dim _DEBUG_AllChars As Char() = _RichTextBox.GetText().ToCharArray()
				'MsgDebug("The Token Begins at:" & vbCrLf & _TokenText_BeginsAt & vbCrLf & vbCrLf & "The Char at that Index is:" & vbCrLf & _RichTextBox.GetText().ToCharArray().ElementAt(CInt(_TokenText_BeginsAt)))

				'Dim _TokenText_Start As Windows.Documents.TextPointer = _WholeDocument.Start.GetPositionAtOffset(CInt(_TokenText_IndexOfFirstChar), Windows.Documents.LogicalDirection.Forward)
				'Dim _TokenText_End As Windows.Documents.TextPointer = _WholeDocument.Start.GetPositionAtOffset(CInt(_TokenText_IndexOfFirstChar + _SelectionLength), Windows.Documents.LogicalDirection.Forward)

				'_RichTextBox.Selection.Select(_TokenText_Start, _TokenText_End)
				'MsgDebug("Selected Token with value" & vbCrLf & _TEMP_REMOVE_THIS_PARAM_TokenValue)
				'_RichTextBox.Selection.ApplyPropertyValue(System.Windows.Documents.TextElement.ForegroundProperty, _NewColour_Brush)



				Colorize(_RichTextBox, CInt(_TokenText_IndexOfFirstChar - 1), CInt(_SelectionLength), _NewColour_Brush) : Return



				'Dim _Selection As Windows.Documents.TextRange = _RichTextBox.Selection
				'Dim _Start As Windows.Documents.TextPointer = _RichTextBox.Document.ContentStart

				'Dim _TokenText_Start As Windows.Documents.TextPointer = GetTextPointer_FromCharOffset(_Start, CInt(_TokenText_IndexOfFirstChar))
				'Dim _TokenText_End As Windows.Documents.TextPointer = GetTextPointer_FromCharOffset(_Start, CInt(_TokenText_IndexOfFirstChar + _SelectionLength))

				'_Selection.Select(_TokenText_Start, _TokenText_End)
				'MsgDebug("For Token:" & _TEMP_REMOVE_THIS_PARAM_TokenValue & vbCrLf & "Selected:" & vbCrLf & _Selection.Text)
				'_Selection.ApplyPropertyValue(System.Windows.Documents.TextElement.ForegroundProperty, _NewColour_Brush)

				'Return





				'MsgDebug("Entered ApplyColourToSelection() for line " & (_SelectionStart.Item1 - 1).ToString())

				'Dim _TargetRange As New System.Windows.Documents.TextRange(
				' position1:=New Windows.Documents.TextPointer(),
				' position2:=Nothing
				' )

				'Dim _NewColour_Brush As New System.Windows.Media.SolidColorBrush(Windows.Media.Color.FromArgb(_NewColour.A, _NewColour.R, _NewColour.G, _NewColour.B))
				'_TargetRange.ApplyPropertyValue(Windows.Documents.TextElement.ForegroundProperty, _NewColour_Brush)





				'Dim line As Int32 = Convert.ToInt32(_SelectionStart.Item1 - 1)
				'Dim _TextRange As New System.Windows.Documents.TextRange(_RichTextBox.Document.Blocks.ElementAt(line).ContentStart, _RichTextBox.Document.Blocks.ElementAt(line).ContentEnd)
				'_TextRange.ApplyPropertyValue(System.Windows.Documents.TextElement.ForegroundProperty, _NewColour_Brush)
				'Return

				'Dim _TextRange As System.Windows.Documents.TextRange

				'For Each Line in the RichTextBox:
				'For _BlockIndex% = 0 To (_RichTextBox.Document.Blocks.Count - 1) Step +1

				'If we're on the sought Line:
				'If _BlockIndex = line Then

				'Get the current Line
				'_TextRange = New System.Windows.Documents.TextRange(_RichTextBox.Document.Blocks.ElementAt(_BlockIndex).ContentStart, _RichTextBox.Document.Blocks.ElementAt(_BlockIndex).ContentEnd)


				'If it's empty, skip it
				'If _TextRange.Text.Trim().Equals("") Then Continue For

				'Apply the new Formatting
				'_TextRange.ApplyPropertyValue(System.Windows.Documents.TextElement.BackgroundProperty, System.Windows.Media.Brushes.Red)
				'MsgDebug("Applying " & _NewColour_Brush.Color.ToString() & " to line " & line)

				'Don;t look at any further lines
				'Return

				'End If

				'Next
				'RichTextBox.Selection.Select(Nothing, Nothing)
				'_RichTextBox.Selection.ApplyPropertyValue(Nothing, Nothing)


			Catch _Ex As Exception : Throw New DSException("@ApplyColourToSelection (CompilerExtention): " & _Ex.Message, _Ex) : End Try
		End Sub

		Private Sub Colorize(ByRef MyRichTextBox As Windows.Controls.RichTextBox, ByVal offset As Integer, ByVal length As Integer, ByVal brush As Windows.Media.Brush) ' color As Windows.Media.Color)
			Dim textRange = MyRichTextBox.Selection
			Dim start = MyRichTextBox.Document.ContentStart
			Dim startPos = GetTextPointer_FromCharOffset(start, offset)
			Dim endPos = GetTextPointer_FromCharOffset(start, offset + length)
			textRange.[Select](startPos, endPos)
			textRange.ApplyPropertyValue(Windows.Documents.TextElement.ForegroundProperty, brush)
			textRange.ApplyPropertyValue(Windows.Documents.TextElement.FontWeightProperty, Windows.FontWeights.Bold)
		End Sub

		'https://stackoverflow.com/questions/1454440/select-range-of-text-in-wpf-richtextbox-flowdocument-programmatically
		Private Function GetTextPointer_FromCharOffset(ByVal start As Windows.Documents.TextPointer, ByVal x As Integer) As Windows.Documents.TextPointer
			Dim ret = start
			Dim i = 0

			While i < x AndAlso ret IsNot Nothing
				If ret.GetPointerContext(Windows.Documents.LogicalDirection.Backward) = Windows.Documents.TextPointerContext.Text OrElse ret.GetPointerContext(Windows.Documents.LogicalDirection.Backward) = Windows.Documents.TextPointerContext.None Then i += 1
				If ret.GetPositionAtOffset(1, Windows.Documents.LogicalDirection.Forward) Is Nothing Then Return ret
				ret = ret.GetPositionAtOffset(1, Windows.Documents.LogicalDirection.Forward)
			End While

			Return ret
		End Function

		REM ↓ UNUSED ↓
		Private ReadOnly CharOffestProperty As System.Reflection.PropertyInfo = GetType(System.Windows.Documents.TextPointer).GetProperty("CharOffset", System.Reflection.BindingFlags.NonPublic Or System.Reflection.BindingFlags.Instance)
		<System.Runtime.CompilerServices.Extension()>
		Private Function GetCharOffsetUsingReflection(ByVal textPointer As System.Windows.Documents.TextPointer) As Integer
			Return CInt(CharOffestProperty.GetValue(textPointer, Nothing))
		End Function






'<!-- Left-aligned -->
'<!--<StatusBarItem>
'	<StackPanel Orientation="Horizontal">
'		<TextBlock Name="StatusLabel" Text="Status: Idle" Margin="5,0,5,0"/>
'		<ProgressBar Height="12" Name="InterpretationProgressBar" IsIndeterminate="True" Width="100" />
'	</StackPanel>
'</StatusBarItem>-->

'<!-- Right-aligned -->
'<!--<StatusBarItem HorizontalAlignment="Right" Grid.Column="0">
'	<StackPanel Orientation="Horizontal">

'		<Separator Width="1" Grid.ColumnSpan="2" Grid.Column="0" Background="#FF969696" />
'		<TextBlock Name="LineCountLabel" Text="0 Line(s)" />

'		<Separator Width="1" Background="#FF969696" />
'		<TextBlock Name="CaretLineColumnLabel" Text="Line: 1, Col: 1" />

'		<Separator Width="1" Background="#FF969696" />
'		<TextBlock x:Name="ZoomValueStatusBarText" Text="Zoom: 100%"/>

'	</StackPanel>
'</StatusBarItem>-->


'Public ReadOnly TokenTypeToColour_Dictionary As New Dictionary(Of DocScript.Runtime.Token.TokenType, System.Drawing.Color)() From {
' {Runtime.Token.TokenType.Unresolved, System.Drawing.Color.FromArgb(red:=0, blue:=0, green:=0)},
' {Runtime.Token.TokenType.StringLiteral, System.Drawing.Color.FromArgb(red:=160, blue:=23, green:=20)},
' {Runtime.Token.TokenType.NumericLiteral, System.Drawing.Color.FromArgb(red:=171, blue:=13, green:=144)},
' {Runtime.Token.TokenType.BooleanLiteral, System.Drawing.Color.FromArgb(red:=0, blue:=1, green:=253)},
' {Runtime.Token.TokenType.Keyword, System.Drawing.Color.FromArgb(red:=0, blue:=1, green:=253)},
' {Runtime.Token.TokenType.DataType, System.Drawing.Color.FromArgb(red:=45, blue:=144, green:=178)},
' {Runtime.Token.TokenType.Identifier, System.Drawing.Color.FromArgb(red:=0, blue:=0, green:=0)},
' {Runtime.Token.TokenType.DSOperator, System.Drawing.Color.FromArgb(red:=0, blue:=0, green:=0)},
' {Runtime.Token.TokenType.GrammarChar, System.Drawing.Color.FromArgb(red:=118, blue:=113, green:=113)},
' {Runtime.Token.TokenType.LineEnd, System.Drawing.Color.FromArgb(red:=0, blue:=0, green:=0)},
' {Runtime.Token.TokenType.StatmentEnd, System.Drawing.Color.FromArgb(red:=0, blue:=1, green:=253)}
'}

#End Region


#Region "DSLibrary"

#Region "GUI LogEvent Handler"

Private Shared LogWindow_ As Logging.LogWindow = Nothing
Private Shared LogWindowThread_ As Threading.Thread

Private Shared LogWindow_HasBeenInstanciated As Boolean = False

''' <summary>Outputs the LogEvents to a Built-in LogWindow, which is run in a seperate Thread</summary>
Public Shared ReadOnly GUIDefault As New LogEventHandler(
  Sub(_Event As LogEvent)

	  REM If this is the first time a Log call is being made, Initialise the LogWindow in a new Thread:
	  If Not LogWindow_HasBeenInstanciated Then
		  LogWindowThread_ = New Threading.Thread(New Threading.ThreadStart(AddressOf LogWindowThreadStart))
		  LogWindowThread_.Start()
		  LogWindow_HasBeenInstanciated = True
		  'Threading.Thread.Sleep(200)
	  End If

	  LogWindow_.InvokeIfRequired(Sub() LogWindow_.AddLogEvent(_event))

  End Sub
)

REM This is ONLY run once
Private Shared Sub LogWindowThreadStart()
	LogWindow_ = New LogWindow()
	System.Windows.Forms.Application.Run(LogWindow_)
	'LogWindow_.Show()
End Sub

#End Region






'MsgDebug("_Index Argument: " & _Arguments.ElementAt(1).ToString())
'MsgBox("_Index As Int32: " & Convert.ToInt32(_Arguments.ElementAt(1).Coerce(Of DSNumber)().Value))




'**NI

'If Me.ReturnValueExpr IsNot Nothing Then
'Me.ReturnValueExpr.Resolve(Of {My Parent DSFunction's RETURN TYPE})()
'Dim _SymTbls_ToReturn As Runtime.SymbolTablesSnapshot
'End If

'Dim _ParentFunctionReturnType As Type '= SymTbl(0).Item(SymTbl(1).LocalFunctionName).ReturnType

'Return New ExecutionResult(_SymbolTables, True, If(Me.ReturnType = ReturnInstructionType.ReturnWithValue, ctype(ReturnValueExpr.GetType().GetMethod("Resolve").MakeGenericMethod(_ParentFunctionReturnType).Invoke(Me, Nothing),_ParentFunctionReturnType.gettype), Nothing)
'Return New ExecutionResult(_SymbolTables, True, If(Me.ReturnType = ReturnInstructionType.ReturnWithValue, ctypedynamic(ReturnValueExpr.GetType().GetMethod("Resolve").MakeGenericMethod(_ParentFunctionReturnType).Invoke(Me, Nothing),_ParentFunctionReturnType), Nothing)
'Return New ExecutionResult(_SymbolTables, True, CTypeDynamic(ReturnValueExpr.GetType().GetMethod("Resolve").MakeGenericMethod(_ParentFunctionReturnType).Invoke(Me, Nothing), _ParentFunctionReturnType))










'Throw New DSValidationException("A DSString could not be constructed because the specified _Value contained the StringLiteralStartEndChar. This char is not allowed to appear in the VALUE of the string itself. The Sideloading constructor FromToken() is able to remove these quotes from the token.", _Value)

'For Debugging, list Each Entry in Each SymbolTable in the Stack
'For Each _SymTbl As SymbolTable In Me.SymbolTables_
'	For Each _Entry As KeyValuePair(Of String, SymbolTable.ISymbolTableEntry) In _SymTbl.Entries_ReadOnlyCopy
'		LogDebugMessage("SymTbl " & _SymTbl.Description.InBrackets() & " contains: " & _Entry.Key, LogEvent.DSEventCatagory.Execution)
'	Next
'Next
'For Each _SymTbl As SymbolTable In Me.SymbolTables_
'	If _SymTbl.Entries_ReadOnlyCopy.GetAllKeys().Contains(_Identifier.ToUpper()) Then MsgDebug(_SymTbl.Description & " contains " & _Identifier)
'	If _SymTblContainingReleventEntry_Resolver.Invoke(_SymTbl) Then MsgDebug(_SymTbl.Description & " contains (Lambda) " & _Identifier)
'Next


'If this current BIF has been declared to return a value, then Ensure that it does:
'If (Me.ReturnType <> GetType(Language.Variables.Void)) AndAlso (_ExeRes.ReturnStatus.CurrentDSFunction_ReturnValue Is Nothing) Then Throw New DSNoReturnValueException(Me.Identifier, "No Child Instruction within the DSFunction raised a [Return]")

REM Procure a SymbolTableEntry(Of TheCorrectType)
'Dim _ProcuredSymTblEntry As Runtime.SymbolTable.ISymbolTableEntry
'				Select Case Me.DataType
'					Case GetType(Variables.DSString) : _ProcuredSymTblEntry = If(_Resolved_AssignmentExpr Is Nothing, New Runtime.SymbolTable.SymbolTableEntry(Of Variables.DSString)(Variables.DSString.InstanceWithNullValue), New Runtime.SymbolTable.SymbolTableEntry(Of Variables.DSString)(_Value:=_Resolved_AssignmentExpr.Coerce(Of Variables.DSString)()))
'					Case GetType(Variables.DSNumber) : _ProcuredSymTblEntry = If(_Resolved_AssignmentExpr Is Nothing, New Runtime.SymbolTable.SymbolTableEntry(Of Variables.DSNumber)(Variables.DSNumber.InstanceWithNullValue), New Runtime.SymbolTable.SymbolTableEntry(Of Variables.DSNumber)(_Value:=_Resolved_AssignmentExpr.Coerce(Of Variables.DSNumber)()))
'					Case GetType(Variables.DSBoolean) : _ProcuredSymTblEntry = If(_Resolved_AssignmentExpr Is Nothing, New Runtime.SymbolTable.SymbolTableEntry(Of Variables.DSBoolean)(Variables.DSBoolean.InstanceWithNullValue), New Runtime.SymbolTable.SymbolTableEntry(Of Variables.DSBoolean)(_Value:=_Resolved_AssignmentExpr.Coerce(Of Variables.DSBoolean)()))
'					Case GetType(Variables.DSArray(Of Variables.DSString)) : _ProcuredSymTblEntry = If(_Resolved_AssignmentExpr Is Nothing, New Runtime.SymbolTable.SymbolTableEntry(Of Variables.DSArray(Of Variables.DSString))(Variables.DSArray(Of Variables.DSString).InstanceWithNullValue), New Runtime.SymbolTable.SymbolTableEntry(Of Variables.DSArray(Of Variables.DSString))(_Value:=_Resolved_AssignmentExpr.Coerce(Of Variables.DSArray(Of Variables.DSString))()))
'					Case GetType(Variables.DSArray(Of Variables.DSNumber)) : _ProcuredSymTblEntry = If(_Resolved_AssignmentExpr Is Nothing, New Runtime.SymbolTable.SymbolTableEntry(Of Variables.DSArray(Of Variables.DSNumber))(Variables.DSArray(Of Variables.DSNumber).InstanceWithNullValue), New Runtime.SymbolTable.SymbolTableEntry(Of Variables.DSArray(Of Variables.DSNumber))(_Value:=_Resolved_AssignmentExpr.Coerce(Of Variables.DSArray(Of Variables.DSNumber))()))
'					Case GetType(Variables.DSArray(Of Variables.DSBoolean)) : _ProcuredSymTblEntry = If(_Resolved_AssignmentExpr Is Nothing, New Runtime.SymbolTable.SymbolTableEntry(Of Variables.DSArray(Of Variables.DSBoolean))(Variables.DSArray(Of Variables.DSBoolean).InstanceWithNullValue), New Runtime.SymbolTable.SymbolTableEntry(Of Variables.DSArray(Of Variables.DSBoolean))(_Value:=_Resolved_AssignmentExpr.Coerce(Of Variables.DSArray(Of Variables.DSBoolean))()))
'					Case Else : Throw New DSValidationException("The VariableDeclaration's DataType was un-[accounted-for]", Me.DataType.FullName)
'				End Select

REM Validate the Token Values against their Types:
'Parser.ValidateTokensValuesAgainstTheirTypes(_ClassifiedTokens)

'My.Computer.FileSystem.WriteAllText(DebugTxtPath, "All _ClassifiedTokens:" & vbCrLf & vbCrLf & String.Join(vbCrLf, From _Token As Token In _ClassifiedTokens Select _Token.Value & _Token.Type.ToString().InSquares()), True)
'MsgDebug("These are the Statement Openings and Closings:" & vbCrLf & vbCrLf & String.Join(vbCrLf, (From _Token As Runtime.Token In _ClassifiedTokens Where (BuiltInTPVs.StatementOpening_TPV.IsSatisfiedBy(_Token) OrElse BuiltInTPVs.StatementClosing_TPV.IsSatisfiedBy(_Token)) Select _Token.Value.ToUpper()).ToArray()))
'If _RawTokens.Any(Function(_Token As Runtime.Token) ((_Token.Type = Runtime.Token.TokenType.DSOperator) AndAlso (_Token.Value = Constants.AssignmentOperator.ToString()))) Then Throw New DSValidationException("An Assignment Operator was found in the Expression.", _RawTokens.First(Function(_Token As Runtime.Token) ((_Token.Type = Runtime.Token.TokenType.DSOperator) AndAlso (_Token.Value = Constants.AssignmentOperator.ToString()))).ToString())
'
'If Not SubStatementOpening_TPV.IsSatisfiedBy(_CurrentLine) Then MsgDebug("About to Increment _TokenIndex from " & _TokenIndex.ToString() & ", by " & _CurrentLine.Length)
'MsgDebug("_TokenIndex is now " & _TokenIndex.ToString())
'MsgDebug("_TokenIndex is " & _TokenIndex.ToString() & vbCrLf & "Processing Line: " & vbCrLf & _CurrentLine.ReconstructIntoSource(" ").InSquares())
'MsgDebug("About to evaluate _CurrentLine. Remaining Tokens: " & vbCrLf & _StatementContentsTokens.Skip(_TokenIndex).ToArray().ReconstructIntoSource())
'MsgDebug("Got Contents and ElseContents for an IfStatement." & vbCrLf & vbCrLf & "Contents:" & vbCrLf & _Contents_And_ElseContents.Item1.ReconstructIntoSource().InSquares() & vbCrLf & vbCrLf & "ElseContents:" & vbCrLf & If(_Contents_And_ElseContents.Item2 Is Nothing, "[NOTHING]", _Contents_And_ElseContents.Item2.ReconstructIntoSource()).InSquares())
'MsgDebug("Checking LBL Component: " & _LBLComp.ToString().InSquares() & " of Type " & _LBLComp.GetType().Name & vbCrLf & _LBLComp.GetType().FullName)
'MsgDebug("ParameterDerivationLoop ParameterGroup: " & _ParameterTokens.GetValuesAndLocationsString())


'MsgBox("_OperatorChar = Nothing: " & (_OperatorChar = Nothing).ToString()) : MsgBox("DSOperators Is Nothing: " & (DSOperators Is Nothing).ToString()) : MsgBox("DSOperators.Item(_OperatorChar) Is Nothing: " & (DSOperators.Item(_OperatorChar) Is Nothing).ToString())

'For Each _Pattern$ In _ValidNumericLiteralString_Patterns
'	MsgDebug("NumericLiteralRegExp: " & _Pattern)
'Next

REM From the BIF\Run() Method
'Try	REM Coerce() each Argument into the Type of the Parameter
'	Dim _CorrectlyTyped_Arguments As New List(Of Language.Variables.IDataValue)(capacity:=_Arguments.Length)
'	For _ArgumentIndex% = 0 To (_Arguments.Length - 1) Step +1

'		LogExecutionMessage(String.Format("Coercing BIF Argument {0} with Identifier {1} of Type {2} to required Parameter type {3}", _ArgumentIndex.ToString(), Me.ExpectedParameters.ElementAt(_ArgumentIndex%).Identifier, _Arguments.ElementAt(_ArgumentIndex).GetType().Name, Me.ExpectedParameters.ElementAt(_ArgumentIndex).DataType.Name))

'		_CorrectlyTyped_Arguments.Add(
'		 Global.DocScript.Language.Variables.CoerceIDV_IntoTargetIDVType_NoGenericModifier(_Arguments.ElementAt(_ArgumentIndex), Me.ExpectedParameters.ElementAt(_ArgumentIndex).DataType)
'		)

'	Next
'Catch _Ex As Exception : Throw New DSException("Whilst Coerce()-ing each Argument to the expected Parameter's Type: " & _Ex.Message, _Ex) : End Try

Public Shared ReadOnly Property Empty As SymbolTablesSnapshot
	Get
		Return (New SymbolTablesSnapshot({}))
	End Get
End Property

#End Region

#Region "DSExperimentation"

#Region "Old Line-Filtering Algorythm"
Private Function FilterOutInsignificantPartsFromRawSource(ByRef _RawSourceLines$()) As List(Of String)

	Dim _ProcessedSourceLines As New List(Of String)()

	Dim _LinesWhichHadLeadingWhitespace As UInt16 = 0
	Dim _LinesWhichHadTrailingWhitespace As UInt16 = 0
	Dim _LinesWhichWereEmptyOrJustWhitespace As UInt16 = 0
	Dim _LinesWhichWereComments As UInt16 = 0

	For Each _RawLine$ In _RawSourceLines

		Dim _ProcessedLine$ = _RawLine

		REM Check for Leading Whitespace
		'The If() is needed in case the _RawLine is empty already
		If DSECompilerExtentions.WhitespaceChars.Contains(If(_RawLine.Length > 0, _RawLine.ToCharArray().First(), "*"c)) Then _LinesWhichHadLeadingWhitespace += 1US
		_ProcessedLine = _ProcessedLine.WithLeadingWhiteSpaceRemoved()

		REM Check for Trailing Whitespace
		'The If() is needed in case the _RawLine is empty already
		If DSECompilerExtentions.WhitespaceChars.Contains(If(_RawLine.Length > 0, _RawLine.ToCharArray().Last(), "*"c)) Then _LinesWhichHadTrailingWhitespace += 1US
		_ProcessedLine = _ProcessedLine.WithTrailingWhiteSpaceRemoved()

		REM Check if the Line is now empty
		'This is either because it had always been empty, or because it was only whitespace
		If _ProcessedLine.IsEmpty() Then
			_LinesWhichWereEmptyOrJustWhitespace += 1US : Continue For
		End If

		REM Check if the Line is a #Comment
		If _ProcessedLine.StartsWith(GlobalConstants.CommentCharacter.ToString()) Then
			_LinesWhichWereComments += 1US : Continue For
		End If

		'If we're here, then the Line wasn't empty, and it wasn't a comment
		_ProcessedSourceLines.Add(_ProcessedLine)

	Next

	LogMsg("Removed Leading Whitespace form " & _LinesWhichHadLeadingWhitespace.ToString() & " Line(s)")
	LogMsg("Removed Trailing Whitespace form " & _LinesWhichHadTrailingWhitespace.ToString() & " Line(s)")
	LogMsg("Ignored " & _LinesWhichWereComments.ToString() & " Comment Line(s)")
	LogMsg("Ignored " & _LinesWhichWereEmptyOrJustWhitespace.ToString() & " Empty or Whitespace-only Line(s)")

	Return _ProcessedSourceLines

End Function
Private Function InsertPlaceholdersForInsignificantLinesOfSource(ByRef _RawSourceLines$()) As List(Of String)

	Dim _ProcessedSourceLines As New List(Of String)()

	Dim _LinesWhichHadLeadingWhitespace As UInt16 = 0
	Dim _LinesWhichHadTrailingWhitespace As UInt16 = 0
	Dim _LinesWhichWereEmptyOrJustWhitespace As UInt16 = 0
	Dim _LinesWhichWereComments As UInt16 = 0

	For Each _RawLine$ In _RawSourceLines

		Dim _ProcessedLine$ = _RawLine

		REM Check for Leading Whitespace
		'The If() is needed in case the _RawLine is empty already
		If DSECompilerExtentions.WhitespaceChars.Contains(If(_RawLine.Length > 0, _RawLine.ToCharArray().First(), "*"c)) Then _LinesWhichHadLeadingWhitespace += 1US
		_ProcessedLine = _ProcessedLine.WithLeadingWhiteSpaceRemoved()

		REM Check for Trailing Whitespace
		'The If() is needed in case the _RawLine is empty already
		If DSECompilerExtentions.WhitespaceChars.Contains(If(_RawLine.Length > 0, _RawLine.ToCharArray().Last(), "*"c)) Then _LinesWhichHadTrailingWhitespace += 1US
		_ProcessedLine = _ProcessedLine.WithTrailingWhiteSpaceRemoved()

		REM Check if the Line is now empty
		'This is either because it had always been empty, or because it was only whitespace
		If _ProcessedLine.IsEmpty() Then
			_LinesWhichWereEmptyOrJustWhitespace += 1US
			'Instead of the Whitespace, insert a PlaceHolder
			_ProcessedSourceLines.Add(GlobalConstants.PlaceholderFor_EmptyOrWhitespaceLine)
			Continue For
		End If

		REM Check if the Line is a #Comment
		If _ProcessedLine.StartsWith(GlobalConstants.CommentCharacter.ToString()) Then
			_LinesWhichWereComments += 1US
			'Instead of the Whitespace, insert a PlaceHolder
			_ProcessedSourceLines.Add(GlobalConstants.PlaceholderFor_CommentLine)
			Continue For
		End If

		'If we're here, then the Line wasn't empty, and it wasn't a comment
		_ProcessedSourceLines.Add(_ProcessedLine)

	Next

	LogMsg("Ignored " & _LinesWhichWereComments.ToString() & " Comment Line(s)")
	LogMsg("Ignored " & _LinesWhichWereEmptyOrJustWhitespace.ToString() & " Empty or Whitespace-only Line(s)")

	Return _ProcessedSourceLines

End Function
#End Region

#Region "Old Exclusively Line-By-Line Parse() Algorythm"
Public Function LineByLine_Parse(ByVal _RawSourceCode$) As Token()

	Dim _DerivedTokens As List(Of Token) = _
	 (From _SplitBit As String In _RawSourceCode.SplitAtWithoutRemoving(Parser.SplitAtChars) Select (New Token(_SplitBit, Token.TokenType.BooleanLiteral, 0, 0))).ToList()

	'The unprocessed lines from the Source String
	Dim _RawSourceLines As String() = _RawSourceCode.Split({vbCrLf}, StringSplitOptions.None).ToArray()
	LogMsg("Loaded in " & _RawSourceLines.Length.ToString() & " Line(s) from Source")

	REM Filter out Leading & Trailing Whitespace, empty Lines, and Comments:
	Dim _FilteredSourceLines As List(Of String) = Parser.FilterOutInsignificantPartsFromRawSource(_RawSourceLines)
	_FilteredSourceLines.ForEach(Sub(_Line$) ParserExperiments.TokensTextBox.Text &= ("Filtered Source Line: |" & _Line & "|"c & vbCrLf))

	REM TOKEN LOCATIONS!!!!!

	LogMsg("Finished Token Generation; Returning " & _DerivedTokens.Count.ToString() & " Token(s)")
	Return _DerivedTokens.ToArray()

End Function
#End Region

#If False Then
LogMsg("Removed Leading Whitespace form " & _LinesWhichHadLeadingWhitespace.ToString() & " Line(s)")
LogMsg("Removed Trailing Whitespace form " & _LinesWhichHadTrailingWhitespace.ToString() & " Line(s)")

SubmitLogEvent("Test", LogEvent.DSEventSeverity.Error, LogEvent.DSEventCatagory.Execution)
			SubmitLogEvent("Test", LogEvent.DSEventSeverity.Warning, LogEvent.DSEventCatagory.Lexing)
			SubmitLogEvent("Test", LogEvent.DSEventSeverity.Infomation, LogEvent.DSEventCatagory.Parsing)
			SubmitLogEvent("Test", LogEvent.DSEventSeverity.Verbose, LogEvent.DSEventCatagory.System)
			SubmitLogEvent("Test", LogEvent.DSEventSeverity.Debug, LogEvent.DSEventCatagory.Unspecified)

#End If

Dim _DerivedTokens As New List(Of Token)() ' = (From _SplitBit As String In _Source.SplitAtWithoutRemoving(Parser.SplitAtChars) Select (New Token() With {.Value = _SplitBit, .Type = Token.TokenType.BooleanLiteral})).ToList()

Dim _DerivedTokens As List(Of Token) = _
   (From _SplitBit As String In _RawSourceCode.SplitAtWithoutRemoving(Parser.SplitAtChars) Select (New Token(_SplitBit, Token.TokenType.BooleanLiteral, 0, 0))).ToList()

'From each whitespace-trimmed line, select the ones which don't start with "#" (comment)
Dim _SourceLines As New List(Of String)( _
  From _WhitespaceTrimmedLine As String In ( _
  From _RawLine As String In _RawSourceCode.Split({vbCrLf}, StringSplitOptions.RemoveEmptyEntries) Select _RawLine.WithLeadingWhiteSpaceRemoved().WithTrailingWhiteSpaceRemoved() _
  ) Where Not _WhitespaceTrimmedLine.StartsWith("#") _
 )

#End Region

#Region "DSWebParts"

''' <summary>
''' INVALID REQUEST Constructor
''' Constructs the API Response to indicate that the Request was NOT valid, specifying the _ErrorMessage to send back to the client.
''' This is NOT to indicate that there was an Internal Server Error, merely that the Client failed to form a request with the correct syntax and nessecary QueryStrings and PostData.
''' </summary>
''' <param name="_ErrorMessage">The _ErrorMessage to send back to the client</param>
''' <remarks></remarks>
Public Sub New(ByVal _ErrorMessage$)
	Me.DAR_HTTPStatusCode = Net.HttpStatusCode.BadRequest
	Me.DAR_ErrorMessage = _ErrorMessage
End Sub

''' <summary>
''' VALID REQUEST with KVPs Constructor
''' Constructs the API Response to indicate that the Request WAS valid, specifying the ResponseContent KeyValuePairs to send back to the client
''' </summary>
''' <param name="_ResponseContentKVPs">The ResponseContent KeyValuePairs to send back to the client</param>
''' <remarks></remarks>
Public Sub New(ByVal ParamArray _ResponseContentKVPs As String()())
	Try

		Me.DAR_HTTPStatusCode = Net.HttpStatusCode.OK
		Me.DAR_ErrorMessage = APIResponse.Default_DARErrorMessage 'NA

		REM _ResponseContentKVPs should look like:
		REM		{"Key0", "Value"}, {"Key1", "Value"}

		Dim _RCDictionary As New Dictionary(Of String, String)()

		For Each _RCKVP As String() In _ResponseContentKVPs
			If Not _RCKVP.Length = 2 Then Throw New DSValidationException("The ResponseContent KeyValuePair did not have 2 values in its String Array", _RCKVP.GetStandardArraySerialisation())
			_RCDictionary.Add(_RCKVP(0), _RCKVP(1))
		Next

		Me.RC_KeyValuePairs = _RCDictionary

	Catch _Ex As Exception : Throw New DSException("@APIResponse\New(): " & _Ex.Message, _Ex) : End Try
End Sub

''' <summary>
''' VALID REQUEST with KVPs and XMLChildren Constructor
''' Constructs the API Response to indicate that the Request WAS valid, specifying the ResponseContent KeyValuePairs, and ResponseContent XML Children to send back to the client
''' </summary>
''' <param name="_ResponseContent_KVPs">The XML Attributes of the ResponseContent Node</param>
''' <param name="_ResponseContent_XMLChildren">The XML Children of the ResponseContent Node</param>
''' <remarks></remarks>
Public Sub New(ByVal _ResponseContent_KVPs As String()(), ByVal _ResponseContent_XMLChildren As XElement())
	Try

		Me.DAR_HTTPStatusCode = Net.HttpStatusCode.OK
		Me.DAR_ErrorMessage = APIResponse.Default_DARErrorMessage 'NA

		Me.RC_ChildNodes = _ResponseContent_XMLChildren.ToList()

		REM _ResponseContentKVPs should look like:
		REM		{"Key0", "Value"}, {"Key1", "Value"}

		Dim _RCDictionary As New Dictionary(Of String, String)()

		For Each _RCKVP As String() In _ResponseContent_KVPs
			If Not _RCKVP.Length = 2 Then Throw New DSValidationException("The ResponseContent KeyValuePair did not have 2 values in its String Array", String.Join(", ", _RCKVP).InCurlies())
			_RCDictionary.Add(_RCKVP(0), _RCKVP(1))
		Next

		Me.RC_KeyValuePairs = _RCDictionary

	Catch _Ex As Exception : Throw New DSException("@APIResponse\New(): " & _Ex.Message, _Ex) : End Try
End Sub

#End Region




Console.WriteLine(WebParts.DatabaseInteraction.GetUploadedProgramFromESID(EntryPoint.TargetExeSes_ESID).ToString())

/* Ensure that the [Send...] Button is only triggered once */
			if (window.SendInputResponse_HasBeenCalled) { console.warn("SendInputResponse() had already been called. Not executing it again."); return; }
			window.SendInputResponse_HasBeenCalled = true;
















''' <summary>(MullNet CompilerExtention) Attempts return the first Parameter whose Name is equal to _ParameterName$</summary>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function ByName(ByVal _Arguments As Language.Variables.IDataValue(), ByVal _ParameterName$, ByRef _BIF As Runtime.BuiltInFunction) As Language.Instructions.Statements.DSFunction.Parameter

			REM Determine which 

			Return DocScript.Language.Variables.TypeCoercion.CoerceIDV_IntoTargetIDVType(Of _TSoughtIDV)(_InputIDV)
		End Function














		
				'Copy 
				If Not _DSCoreDll.Name = _DSCoreDll_FileName Then Throw New DSValidationException("The pointed-to instance of DSCore.dll must be called " & _DSCoreDll_FileName & ", because this name is releid upon in subsequent stages of DSCompilation", _DSCoreDll.Name)
				If Not _DSCoreDll.Exists Then Throw New IO.FileNotFoundException("The pointed-to instance of DSCore.dll was not found", _DSCoreDll.FullName)


#End If