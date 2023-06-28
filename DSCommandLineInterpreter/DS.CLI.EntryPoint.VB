Public Module EntryPoint

	Public Enum DSCLIActionToPerform As Byte
		Unresolved = 0
		Run = 1
		GetProgramTree = 2
		EnterDSLiveSession = 3
	End Enum

	Public Const DSLive_StartupText$ =
	  vbCrLf &
	  "┌───────────────────────────────────────┐ " & vbCrLf &
	  "│            ╔═══╗  DocScript           │▒" & vbCrLf &
	  "│            ╚═╦═╝   /Live              │▒" & vbCrLf &
	  "└───────────────────────────────────────┘▒" & vbCrLf &
	  " ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒" & vbCrLf &
	  vbCrLf &
	  " Only use Inner-Statement Instructions (no Function declarations)" & vbCrLf &
	  " Exit with !Exit (or Ctrl + C) - learn more with !Help" & vbCrLf &
	  " Use ? to Resolve an Expression e.g. ?14 + 33" & vbCrLf &
	  " Use ; for NewLine" &
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

#Region "Global Instance-specific Propities"

	Public ExeCxt_ToUse As DocScript.Runtime.ExecutionContext 'Initialised to CLIDefault, as soon as (CurrentLogEventHandler IsNot Nothing)
	Public SourceToInterpret As [String] = Nothing
	Public DocScriptCLAs As [String]() = {}
	Public ActionWhenReady As DSCLIActionToPerform = DSCLIActionToPerform.Unresolved
	Public HideConsoleBeforeExecution As [Boolean] = False
	Public PromptBeforeExit As [Boolean] = False
	Public SymbolTablesXMLFile_ForDSLiveSession As IO.FileInfo = Nothing

#End Region

	Public Function Main(ByVal _CLAs As String()) As Int32

		REM [-1] means [indeterminate]. The _ExitCode will be set depending on the ActionWhenReady...
		Dim _ExitCode As Int32 = -1

		Try

			'Don't show and LogEvents unless /LogToConsole or /LogToFile is specified
			DocScript.Logging.LogUtilities.CurrentLogEventHandler = DocScript.Logging.LogUtilities.BuiltInLogEventHandlers.SilenceAll
			EntryPoint.ExeCxt_ToUse = DocScript.Runtime.ExecutionContext.CLIDefault

			REM ↓ Scenario-1: We have just been passed a Source-File-Path (with 0 or more DS-CLAs) in the CLAs to DSCLI.exe; just run the DS Program, instead of bothering with the CLAManager...
			If (_CLAs.Length >= 1) AndAlso _CLAs.ElementAt(0).IsValidFilePath_AndThatFileExists() Then

				EntryPoint.ActionWhenReady = EntryPoint.DSCLIActionToPerform.Run
				EntryPoint.SourceToInterpret = My.Computer.FileSystem.ReadAllText(_CLAs.ElementAt(0))
				EntryPoint.DocScriptCLAs = _CLAs.Skip(1).ToArray()

			Else REM ↓ Scenario-2: We have not been passed a DocScript File in _CLAs[0]; use a CLAManager to process the action for this DSCLI instance...

				REM Only one of [SourceFile] and [SourceString] should be specified
				REM Only one of [Run] and [GetProgramTree] should be specified
				Dim _CLAManager As New DocScript.Utilities.CLAManager(
				 _CLAs,
				 "DocScript Command-Line Interpreter v" & Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() & ". Interprets DocScript Source Files." & vbCrLf & "Note that for compatibility reasons, source files can be passed to DSCLI in" & vbCrLf & "the form DSCLI.exe {PathToSourceFile} {ArgumentsForTargetScript...}." & vbCrLf & "However, the /Key:Value CLAs provide more options.",
				 {"DSCLI.EXE /Live",
				 "DSCLI.EXE /Run /SourceString:""Function <Void> Main ();Output(`Hello, World!`);EndFunction""",
				 "DSCLI.EXE /Run /SourceFile:""X:\Programming\DocScript\HelloWorld.DS"" /LogToConsole",
				 "DSCLI.EXE /GetProgramTree /SourceString:""Function <Void> Main ();Output(`Hello, World!`);EndFunction""",
				 "DSCLI.EXE /Run /SourceString:""Function<Void>Main();System_Beep();EndFunction""",
				 "DSCLI.EXE /Live /LogToFile:""DSLive.DSLog"" /ProcessDebugEvents /GUI",
				 "DSCLI.EXE /Run /SourceFile:""BIO2017.DS"" /DocScriptCLAs:""GRBBRB"" /LogToFile:BIO.DSLog"},
				 AddressOf Console.Write, True,
				 New Utilities.CLAManager.CLADatum("Live", "[Action] Enters a DocScript Live Session: a DS> prompt appears and accepts Statement-level Instructions", False, False, Sub(_Value$) EntryPoint.ActionWhenReady = DSCLIActionToPerform.EnterDSLiveSession),
				 New Utilities.CLAManager.CLADatum("Run", "[Action] Interprets the DocScript Source (specified by either /SourceFile or /SourceString). This process then returns the ExitCode of the DocScript Program.", False, False, Sub(_Value$) EntryPoint.ActionWhenReady = DSCLIActionToPerform.Run),
				 New Utilities.CLAManager.CLADatum("GetProgramTree", "[Action] Parses and Lexes the DocScript Source (specified by either /SourceFile or /SourceString), and writes the resultant XML Program tree to the Console Output Stream", False, False, Sub(_Value$) EntryPoint.ActionWhenReady = DSCLIActionToPerform.GetProgramTree),
				 New Utilities.CLAManager.CLADatum("SourceFile", "[Datum] Specifies the Source via a DocScript Source File", False, True, Sub(_Value$) EntryPoint.SourceToInterpret = My.Computer.FileSystem.ReadAllText(_Value)),
				 New Utilities.CLAManager.CLADatum("SourceString", "[Datum] Specifies the Source via a DocScript Source String. Use ; for NewLine and ` for StringLiteralStartEndChar.", False, True, Sub(_Value$) EntryPoint.SourceToInterpret = _Value.Replace(";", vbCrLf).Replace("`"c, """"c)),
				 New Utilities.CLAManager.CLADatum("DocScriptCLAs", "[Datum] Specifies Command-Line Arguments for the DocScript Program", False, True, Sub(_Value$) EntryPoint.DocScriptCLAs = _Value.Split(" "c)),
				 New Utilities.CLAManager.CLADatum("LogToConsole", "[Flag] Writes Events from the DocScript Log to the Console Output Stream during Interpretation", False, False, Sub(_Value$) DocScript.Logging.LogUtilities.CurrentLogEventHandler = DocScript.Logging.LogUtilities.BuiltInLogEventHandlers.CLIDefault),
				 New Utilities.CLAManager.CLADatum("LogToFile", "[Flag+Datum] Writes Events from the DocScript Log to the specified Text File during Interpretation", False, True, Sub(_Value$) DocScript.Logging.LogUtilities.CurrentLogEventHandler = DocScript.Logging.LogUtilities.BuiltInLogEventHandlers.TextFile(New IO.FileInfo(_Value))),
				 New Utilities.CLAManager.CLADatum("ProcessDebugEvents", "[Flag] Processes and shows Debugging Messages in the Log (if the Log is shown)", False, False, Sub(_Value$) DocScript.Logging.LogUtilities.ProcessDebugEvents = True),
				 New Utilities.CLAManager.CLADatum("GUI", "[Flag] Indicates that the GUI ExecutionContext will be used instead of the CLI one", False, False, Sub(_Value$) EntryPoint.ExeCxt_ToUse = DocScript.Runtime.ExecutionContext.GUIDefault),
				 New Utilities.CLAManager.CLADatum("DSLiveSymbolTablesFile", "[Flag+Datum] A path to a DocScript SymbolTables' States XML File, such as can be produced by the SymbolTableExplorer Window in DSISE, or via Debug_StartDSLiveSessionFromCurrentExecutionState(). The SymbolTables' Entries are loaded into the DSLive Session.", False, True, Sub(_Value$) EntryPoint.SymbolTablesXMLFile_ForDSLiveSession = New IO.FileInfo(_Value)),
				 New Utilities.CLAManager.CLADatum("HideConsoleBeforeExecution", "[Flag] Indicates that DSCLI should relinquish its Win32 Console, when Execution of the DocScript Program begins. (Only valid for the /Run action, and if the DSCLI process isn't launched by another Console process)", False, False, Sub(_Value$) EntryPoint.HideConsoleBeforeExecution = True),
				 New Utilities.CLAManager.CLADatum("PromptBeforeExit", "[Flag] Shows ""Press [Enter] to continue..."" before exiting the DSCLI Process", False, False, Sub(_Value$) EntryPoint.PromptBeforeExit = True)
				)

				REM If the /? was specified and the HelpDictionary was displayed, then we needn't do anything else with this instance
				If _CLAManager.HelpDictionaryWasDisplayed Then End

			End If

			REM Ensure that we have an Action to perform
			If EntryPoint.ActionWhenReady = DSCLIActionToPerform.Unresolved Then Throw New DocScript.Exceptions.DSCLANotSpecifiedException("An Action must be specified (e.g. /Live or /Run or /GetProgramTree). Use /? for Examples!")

			REM If the specified [ActionWhenReady] requires source, then ensure we've been given some source to interpret
			If {DSCLIActionToPerform.Run, DSCLIActionToPerform.GetProgramTree}.Contains(EntryPoint.ActionWhenReady) _
			 Then If SourceToInterpret Is Nothing _
			  Then Throw New DocScript.Exceptions.DSCLANotSpecifiedException("The specified Action (" & EntryPoint.ActionWhenReady.ToString("G") & ") requires some Source to interpret, which must be specified by either /SourceFile or /SourceString. Use /? for Examples!")

			REM At this points, we've understood WHAT to do, from the CLAs.
			REM We've also validated 
			REM Now, perform the actual task for this DSCLI.exe process

			Select Case EntryPoint.ActionWhenReady
				Case DSCLIActionToPerform.GetProgramTree 'Construct source into a New Program

					DocScript.Runtime.Program.FromSource(_Source:=EntryPoint.SourceToInterpret, _ExeCxt:=Nothing) _
					 .ProgramTreeXML.WriteToConsoleWithXMLColourFormatting(_TabChar:="    ")

					_ExitCode = 0

				Case DSCLIActionToPerform.Run 'Construct and Execute a New Program

					Dim _Program As New DocScript.Runtime.Program(
					  _Tokens:=DocScript.Runtime.Parser.GetTokensFromSource(_RawSource:=EntryPoint.SourceToInterpret),
					  _ExecutionContext:=EntryPoint.ExeCxt_ToUse
					)

					If EntryPoint.HideConsoleBeforeExecution Then UsefulMethods.FreeConsole()

					_ExitCode = _Program _
					 .Run(EntryPoint.DocScriptCLAs) _
					 .ReturnStatus.Program_ExitCode _
					 .GetValueOrDefault(defaultValue:=DocScript.Runtime.Constants.ProgramExitCode_Default)

				Case DSCLIActionToPerform.EnterDSLiveSession 'Start a DS-Live session. This call never returns.

					EntryPoint.EnterDSLiveSession()

				Case Else : Throw New DSValidationException("Un-accounted-for Action", "ActionWhenReady: " & ActionWhenReady.ToString("G"))
			End Select

		Catch _Ex As Exception When True
			[UsefulMethods].ConsoleWriteLineInColour("Exception: " & _Ex.Message, _ForegroundColour:=ConsoleColor.Red, _BackgroundColour:=ConsoleColor.Gray)
			_ExitCode = 1
		End Try

		REM PromptBeforeExit if needed
		If EntryPoint.PromptBeforeExit Then : UsefulMethods.ConsoleWriteInColour(vbCrLf & "DSCLI: Press [Enter] to exit...", _ForegroundColour:=ConsoleColor.Magenta, _BackgroundColour:=ConsoleColor.Gray) : Console.ReadLine() : End If

		REM Exit this DSCLI instance
		Return [_ExitCode]

	End Function

	Public Sub EnterDSLiveSession()

		REM Print the Start-up Banner
		UsefulMethods.ConsoleWriteLineInColour(EntryPoint.DSLive_StartupText, _ForegroundColour:=ConsoleColor.DarkBlue, _BackgroundColour:=ConsoleColor.Gray)

		REM Determine what the contents of the SymbolTables should be
		Dim _Session_SymTbls As DocScript.Runtime.SymbolTablesSnapshot
		If EntryPoint.SymbolTablesXMLFile_ForDSLiveSession Is Nothing Then
			_Session_SymTbls = DocScript.Runtime.Program.GenerateGlobalSymbolTable(EntryPoint.ExeCxt_ToUse).SnapshotContainingJustThis
		Else

			Console.Out.WriteLine("Loading Symbol Tables from file: " & EntryPoint.SymbolTablesXMLFile_ForDSLiveSession.FullName)
			_Session_SymTbls = Runtime.SymbolTablesSnapshot.FromXML(XElement.Load(EntryPoint.SymbolTablesXMLFile_ForDSLiveSession.FullName))

			Dim _NonBIFEntries_DatatypesAndIdentifiersStrings$() = _Session_SymTbls.AllEntriesInAllSymbolTables_ReadOnlyCopy _
			   .Where(Function(_Entry As KeyValuePair(Of [String], SymbolTable.ISymbolTableEntry)) _Entry.Value.GetType() <> GetType(SymbolTable.SymbolTableEntry(Of BuiltInFunction))) _
			   .Select(Function(_Entry As KeyValuePair(Of [String], SymbolTable.ISymbolTableEntry)) (_Entry.Value.GetType().GetGenericArguments().First().Name.Replace("`1", "") & If(_Entry.Value.GetType().GetGenericArguments().First().IsGenericType, "→" & _Entry.Value.GetType().GetGenericArguments().First().GetGenericArguments().First().Name, "")).PadRight(18) & _Entry.Key) _
			   .ToArray()

			Console.Out.WriteLine("Loaded " & _NonBIFEntries_DatatypesAndIdentifiersStrings.Count().ToString() & " entries (excluding BIFs) from SymbolTables XML:")
			_NonBIFEntries_DatatypesAndIdentifiersStrings.ToList().ForEach(AddressOf Console.Out.WriteLine)
			Console.Out.WriteLine()

		End If

		REM Enter the (infinite) prompt-response loop
		While True

			Try

				REM Request Console Input. This may be piped, in which case end the process if the line is null.
				UsefulMethods.ConsoleWriteInColour("DS>", _ForegroundColour:=ConsoleColor.Gray, _BackgroundColour:=ConsoleColor.DarkGray) : Console.Out.Write(" "c)
				Dim _LineReadFromConsole As [String] = Console.ReadLine()
				If _LineReadFromConsole Is Nothing Then : UsefulMethods.ConsoleWriteLineInColour("Ending the DSCLI Process, because the Console Input was [Nothing]. This may be because the input to DSCLI was piped.", _ForegroundColour:=ConsoleColor.Red, _BackgroundColour:=ConsoleColor.Gray) : Environment.Exit(exitCode:=-2) : End If
				Dim _DSLiveInputCommand_UnflattenedAndTrimmed As [String] = Parser.UnflattenSource(_LineReadFromConsole.WithLeadingWhiteSpaceRemoved().WithTrailingWhiteSpaceRemoved())

				REM Now perform the correct action, depending on whether the Input Line is a Meta-Command, Expression-Query, or Inner-Statement Instruction
				If (_DSLiveInputCommand_UnflattenedAndTrimmed.Length > 1) AndAlso (_DSLiveInputCommand_UnflattenedAndTrimmed.First() = "!"c) Then	'Meta-Command

					Dim _RemainderOfLine$ = New String(_DSLiveInputCommand_UnflattenedAndTrimmed.Skip(1).ToArray())
					Select Case _RemainderOfLine.ToUpper()
						Case "EXIT" : Environment.Exit(0)
						Case "CLEAR" : Console.Clear()
						Case "HELP" : UsefulMethods.ConsoleWriteLineInColour(EntryPoint.DSLive_HelpMetaCommandText, _ForegroundColour:=ConsoleColor.DarkGreen, _BackgroundColour:=ConsoleColor.Gray)
						Case "LISTBIFS" : UsefulMethods.ConsoleWriteLineInColour(String.Join(vbCrLf, EntryPoint.ExeCxt_ToUse.BuiltInFunctions.Select(Of [String])(Function(_BIF As BuiltInFunction) _BIF.TemplateCall)) & vbCrLf, _ForegroundColour:=ConsoleColor.DarkGreen, _BackgroundColour:=ConsoleColor.Gray)
						Case Else : Throw New Exception("Unrecognised DSLive Meta-command: " & _RemainderOfLine)
					End Select

				ElseIf (_DSLiveInputCommand_UnflattenedAndTrimmed.Length > 1) AndAlso (_DSLiveInputCommand_UnflattenedAndTrimmed.First() = "?"c) Then	'Expression-Query

					Dim _RemainderOfLine$ = New String(_DSLiveInputCommand_UnflattenedAndTrimmed.Skip(1).ToArray())
					Dim _ExpressionTokens As Token() = Parser.GetTokensFromSource(_RemainderOfLine)
					Dim _IExpression As Language.Expressions.IExpression = Language.Expressions.ConstructExpressionFromTokens(_ExpressionTokens)

					Dim _Expression_ExeRes As Instructions.ExecutionResult = _IExpression.Resolve(_Session_SymTbls)
					_Session_SymTbls = _Expression_ExeRes.ResultantSymbolTablesState
					[UsefulMethods].ConsoleWriteLineInColour(_Expression_ExeRes.ReturnStatus.IExpression_ResolutionResult.Coerce(Of Variables.DSString)().Value, _ForegroundColour:=ConsoleColor.Blue, _BackgroundColour:=ConsoleColor.Gray)

				Else 'Assume the input to be 1+ DocScript Inner-Statement Instruction Line(s)

					Dim _DSStatementLine_Tokens As DocScript.Runtime.Token() = DocScript.Runtime.Parser.GetTokensFromSource(_DSLiveInputCommand_UnflattenedAndTrimmed)
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