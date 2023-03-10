Public Module EntryPoint

	Public Enum WhenReadyAction As Byte
		Unresolved = 0
		Run = 1
		GetProgramTree = 2
		EnterDSLiveSession = 3
	End Enum

	Dim DSLive_StartupText$ = _
	  vbCrLf & _
	  "┌───────────────────────────────────────┐ " & vbCrLf & _
	  "│            ╔═══╗  DocScript           │▒" & vbCrLf & _
	  "│            ╚═╦═╝   /Live              │▒" & vbCrLf & _
	  "└───────────────────────────────────────┘▒" & vbCrLf & _
	  " ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒" & vbCrLf & _
	  vbCrLf & _
	  " Only use Statement-Contents Instructions (no Function declarations)" & vbCrLf & _
	  " Exit with !Exit (or Ctrl + C) - learn more with !Help" & vbCrLf & _
	  " Use ? to Resolve an Expression e.g. ?14 + 33" & vbCrLf & _
	  " Use ; for NewLine" & _
	  vbCrLf

	Public Const DSLive_HelpMetaCommandText$ = _
	 "Example: <String> Name : Input(""Enter your name: "")" & vbCrLf & _
	 "Example: ?14 + [90 ^ 3]" & vbCrLf & _
	  vbCrLf & _
	 "DSLive Meta-Commands:" & vbCrLf & _
	 "!Exit		Ends the DocScript Live Session" & vbCrLf & _
	 "!Clear		Equivalent of ""cls"" at a command prompt" & vbCrLf & _
	 "!ListBIFs	Lists avaliable Built-In Functions in the current Execution-Context" & vbCrLf & _
	 "!Help		Shows this help message" & vbCrLf

#Region "Global Instance Settigns"

	Public ExeCxt_ToUse As DocScript.Runtime.ExecutionContext 'Initialised to CLIDefault, once the CurrentLogEventHandler IsNot Nothing
	Public SourceToInterpret As String = Nothing
	Public DocScriptCLAs As String() = {}
	Public ActionWhenReady As WhenReadyAction = WhenReadyAction.Unresolved
	Public PromptBeforeExit As Boolean = False

#End Region

	Public Function Main(ByVal _CLAs As String()) As Int32
		Dim _ExitCode As Int32 = -1
		Try

			'Don't show and LogEvents unless /LogToConsole or /LogToFile is specified
			DocScript.Logging.LogUtilities.CurrentLogEventHandler = DocScript.Logging.LogUtilities.BuiltInLogEventHandlers.SilenceAll
			EntryPoint.ExeCxt_ToUse = DocScript.Runtime.ExecutionContext.CLIDefault

			REM Only one of [SourceFile] and [SourceString] should be specified
			REM Only one of [Run] and [GetProgramTree] should be specified

			Dim _CLAManager As New DocScript.Utilities.CLAManager(
			  _CLAs,
			  "DocScript Command-Line Interpreter. Interprets DocScript Source Files.",
			  {"DSCLI.EXE /Live",
			  "DSCLI.EXE /Live /LogToFile:""DSLive.DSLog"" /ProcessDebugEvents /GUI",
			  "DSCLI.EXE /Run /SourceString:""Function <Void> Main ();Output(`Hello, World!`);EndFunction""",
			  "DSCLI.EXE /Run /SourceFile:""X:\Programming\DocScript\HelloWorld.DS"" /LogToConsole",
			  "DSCLI.EXE /GetProgramTree /SourceString:""Function <Void> Main ();Output(`Hello, World!`);EndFunction""",
			  "DSCLI.EXE /Run /SourceString:""Function<Void>Main();System_Beep();EndFunction""",
			  "DSCLI.EXE /Run /SourceFile:""BIO2017.DS"" /DocScriptCLAs:""GRBBRB"" /LogToFile:BIO.DSLog"},
			  AddressOf Console.Write, True,
			  New Utilities.CLAManager.CLADatum("Live", "[Action] Enters a DocScript Live Session: a DS> prompt appears and accepts Statement-level Instructions", False, False, Sub(_Value$) ActionWhenReady = WhenReadyAction.EnterDSLiveSession),
			  New Utilities.CLAManager.CLADatum("Run", "[Action] Interprets the DocScript Source (specified by either /SourceFile or /SourceString). This process then returns the ExitCode of the DocScript Program.", False, False, Sub(_Value$) ActionWhenReady = WhenReadyAction.Run),
			  New Utilities.CLAManager.CLADatum("GetProgramTree", "[Action] Parses and Lexes the DocScript Source (specified by either /SourceFile or /SourceString), and writes the resultant XML Program tree to the Console Output Stream", False, False, Sub(_Value$) ActionWhenReady = WhenReadyAction.GetProgramTree),
			  New Utilities.CLAManager.CLADatum("SourceFile", "[Datum] Specifies the Source via a DocScript Source File", False, True, Sub(_Value$) SourceToInterpret = My.Computer.FileSystem.ReadAllText(_Value)),
			  New Utilities.CLAManager.CLADatum("SourceString", "[Datum] Specifies the Source via a DocScript Source String. Use ; for NewLine and ` for StringLiteralStartEndChar.", False, True, Sub(_Value$) SourceToInterpret = _Value.Replace(";", vbCrLf).Replace("`"c, """"c)),
			  New Utilities.CLAManager.CLADatum("DocScriptCLAs", "[Datum] Specifies Command-Line Arguments for the DocScript Program", False, True, Sub(_Value$) DocScriptCLAs = _Value.Split(" "c)),
			  New Utilities.CLAManager.CLADatum("LogToConsole", "[Flag] Writes Events from the DocScript Log to the Console Output Stream during Interpretation", False, False, Sub(_Value$) DocScript.Logging.LogUtilities.CurrentLogEventHandler = DocScript.Logging.LogUtilities.BuiltInLogEventHandlers.CLIDefault),
			  New Utilities.CLAManager.CLADatum("LogToFile", "[Flag+Datum] Writes Events from the DocScript Log to the specified Text File during Interpretation", False, True, Sub(_Value$) DocScript.Logging.LogUtilities.CurrentLogEventHandler = DocScript.Logging.LogUtilities.BuiltInLogEventHandlers.TextFile(New IO.FileInfo(_Value))),
			  New Utilities.CLAManager.CLADatum("ProcessDebugEvents", "[Flag] Processes and shows Debugging Messages in the Log (if the Log is shown)", False, False, Sub(_Value$) DocScript.Logging.LogUtilities.ProcessDebugEvents = True),
			  New Utilities.CLAManager.CLADatum("GUI", "[Flag] Indicates that the GUI ExecutionContext will be used instead of the CLI one", False, False, Sub(_Value$) EntryPoint.ExeCxt_ToUse = DocScript.Runtime.ExecutionContext.GUIDefault),
			  New Utilities.CLAManager.CLADatum("PromptBeforeExit", "[Flag] Shows ""Press [Enter] to continue..."" before exiting the DSCLI Process", False, False, Sub(_Value$) EntryPoint.PromptBeforeExit = True)
			)

			REM If the /? was specified and the HelpDictionary was displayed, then we needn't do anything else with this instance
			If _CLAManager.HelpDictionaryWasDisplayed Then End

			REM Ensure that we have an Action
			If EntryPoint.ActionWhenReady = WhenReadyAction.Unresolved Then Throw New DocScript.Exceptions.DSCLANotSpecifiedException("An Action must be specified; use /Live or /Run or /GetProgramTree. Use /? for Examples!")
			If EntryPoint.ActionWhenReady = WhenReadyAction.EnterDSLiveSession Then EntryPoint.EnterDSLiveSession() 'Never Returns...

			REM If we're here, then we didn't enter DSLive, so we need some Source!
			If SourceToInterpret Is Nothing Then Throw New DocScript.Exceptions.DSCLANotSpecifiedException("The Source To Interpret must be specified by means of either /SourceFile or /SourceString. Use /? for Examples!")

			Select Case ActionWhenReady
				Case WhenReadyAction.GetProgramTree

					UsefulMethods.ConsoleWriteInColour(
					   DocScript.Runtime.Program.FromSource(_Source:=EntryPoint.SourceToInterpret, _ExeCxt:=Nothing).ProgramTreeXML.ToString(),
					   _ForegroundColour:=ConsoleColor.Black, _BackgroundColour:=ConsoleColor.Gray
					)
					_ExitCode = 0

				Case WhenReadyAction.Run

					Dim _Program As New DocScript.Runtime.Program(
					  _Tokens:=DocScript.Runtime.Parser.GetTokensFromSource(_RawSource:=EntryPoint.SourceToInterpret),
					  _ExecutionContext:=EntryPoint.ExeCxt_ToUse
					)

					_ExitCode = _Program _
					 .Run(EntryPoint.DocScriptCLAs) _
					 .ReturnStatus.Program_ExitCode _
					 .GetValueOrDefault(defaultValue:=DocScript.Runtime.Constants.ProgramExitCode_Default)

				Case Else : Throw New DSValidationException("Un-accounted-for ActionWhenReady", "ActionWhenReady: " & ActionWhenReady.ToString("G"))
			End Select

		Catch _Ex As Exception When True
			[UsefulMethods].ConsoleWriteLineInColour("Exception: " & _Ex.Message, _ForegroundColour:=ConsoleColor.Red, _BackgroundColour:=ConsoleColor.Gray)
			_ExitCode = 1
		End Try

		REM PromptBeforeExit if needed
		If EntryPoint.PromptBeforeExit Then
			UsefulMethods.ConsoleWriteInColour("Press [Enter] to continue...", _ForegroundColour:=ConsoleColor.Magenta, _BackgroundColour:=ConsoleColor.Gray) : Console.ReadLine()
		End If

		REM Exit this DSCLI instance
		Return _ExitCode

	End Function

	Public Sub EnterDSLiveSession()

		UsefulMethods.ConsoleWriteLineInColour(EntryPoint.DSLive_StartupText, _ForegroundColour:=ConsoleColor.DarkBlue, _BackgroundColour:=ConsoleColor.Gray)
		Dim _Session_SymTbls As DocScript.Runtime.SymbolTablesSnapshot = Program.GenerateGlobalSymbolTable(EntryPoint.ExeCxt_ToUse).SnapshotContainingJustThis

		While True

			Try

				REM Request Console Input. This may be piped, in which case end the process if the line is null.
				UsefulMethods.ConsoleWriteInColour("DS>", _ForegroundColour:=ConsoleColor.Gray, _BackgroundColour:=ConsoleColor.DarkGray) : Console.Write(" "c)
				Dim _LineReadFromConsole As [String] = Console.ReadLine()
				If _LineReadFromConsole Is Nothing Then : UsefulMethods.ConsoleWriteLineInColour("Ending the DSCLI Process, because the Console Input was [Nothing]. This may be because the input to DSCLI was piped.", _ForegroundColour:=ConsoleColor.Red, _BackgroundColour:=ConsoleColor.Gray) : Environment.Exit(exitCode:=1) : End If
				Dim _DSStatementLine_Raw As [String] = Parser.UnflattenSource(_LineReadFromConsole)

				REM Now perform the correct action, depending on whether the Input Line is a Meta-Command, Expression Query, or Statement-Level Instruction
				If (_DSStatementLine_Raw.Length > 1) AndAlso (_DSStatementLine_Raw.First() = "!"c) Then	'Meta-Command

					Dim _RemainderOfLine$ = New String(_DSStatementLine_Raw.Skip(1).ToArray())
					Select Case _RemainderOfLine.ToUpper()
						Case "EXIT" : Environment.Exit(0)
						Case "CLEAR" : Console.Clear()
						Case "HELP" : UsefulMethods.ConsoleWriteLineInColour(EntryPoint.DSLive_HelpMetaCommandText, _ForegroundColour:=ConsoleColor.DarkGreen, _BackgroundColour:=ConsoleColor.Gray)
						Case "LISTBIFS" : UsefulMethods.ConsoleWriteLineInColour(String.Join(vbCrLf, EntryPoint.ExeCxt_ToUse.BuiltInFunctions.Select(Of [String])(Function(_BIF As BuiltInFunction) _BIF.TemplateCall)) & vbCrLf, _ForegroundColour:=ConsoleColor.DarkGreen, _BackgroundColour:=ConsoleColor.Gray)
						Case Else : Throw New Exception("Unrecognised DSLive Meta-command: " & _RemainderOfLine)
					End Select

				ElseIf (_DSStatementLine_Raw.Length > 1) AndAlso (_DSStatementLine_Raw.First() = "?"c) Then	'Expression

					Dim _RemainderOfLine$ = New String(_DSStatementLine_Raw.Skip(1).ToArray())
					Dim _ExpressionTokens As Token() = Parser.GetTokensFromSource(_RemainderOfLine)
					Dim _IExpression As Language.Expressions.IExpression = Language.Expressions.ConstructExpressionFromTokens(_ExpressionTokens)

					Dim _Expression_ExeRes As Instructions.ExecutionResult = _IExpression.Resolve(_Session_SymTbls)
					_Session_SymTbls = _Expression_ExeRes.ResultantSymbolTablesState
					[UsefulMethods].ConsoleWriteLineInColour(_Expression_ExeRes.ReturnStatus.IExpression_ResolutionResult.Coerce(Of Variables.DSString)().Value, _ForegroundColour:=ConsoleColor.Blue, _BackgroundColour:=ConsoleColor.Gray)

				Else 'Assume a DocScript inner-statement line

					Dim _DSStatementLine_Tokens As DocScript.Runtime.Token() = DocScript.Runtime.Parser.GetTokensFromSource(_DSStatementLine_Raw)
					Dim _DSStatementLine_IInstructions As Instructions.IInstruction() = Instructions.Statements.GetStatementContentsFromTokens(_DSStatementLine_Tokens)

					'Just in case multiple Instructions were specified...
					For Each _Instruction As Instructions.IInstruction In _DSStatementLine_IInstructions
						Dim _Instruction_ExeRes As Instructions.ExecutionResult = _Instruction.Execute(_Session_SymTbls)
						_Session_SymTbls = _Instruction_ExeRes.ResultantSymbolTablesState
					Next

				End If

			Catch _Ex As Exception When True
				[UsefulMethods].ConsoleWriteLineInColour("Exception: " & _Ex.Message, _ForegroundColour:=ConsoleColor.Red, _BackgroundColour:=ConsoleColor.Gray)
			End Try

		End While

	End Sub

End Module