Namespace Runtime

	''' <summary>Represents a collection of Functions (imperatively including an EntryPoint Main()) and any Global Variable Declarations, from a source file</summary>
	Public Class Program

#Region "Shared Members"

		''' <summary>The plain EntryPoint with no Input or Output. Used by Program.IsEntryPointFunctionSignature()</summary>
		Public Shared ReadOnly ValidEntryPointFunction_Signature_Unadorned As New Language.Instructions.Statements.DSFunction.Signature(
			Runtime.Constants.EntryPointFunction_Identifier, {}, GetType(Language.Variables.Void)
		)

		''' <summary>The EntryPoint for OperatingSystem-Interoperability. Used by Program.IsEntryPointFunctionSignature()</summary>
		Public Shared ReadOnly ValidEntryPointFunction_Signature_OSInterop As New Language.Instructions.Statements.DSFunction.Signature(
			Runtime.Constants.EntryPointFunction_Identifier, {GetType(Language.Variables.DSArray(Of Language.Variables.DSString))}, GetType(Language.Variables.DSNumber)
		)

		''' <summary>Item1 = The DSFunctions; Item2 = The GlobalVarDecs;</summary>
		Protected Shared Function GetFunctionsAndGlobalVarDecsFromTokens_(ByRef _RawSourceTokens As Token()) As Tuple(Of Language.Instructions.Statements.DSFunction(), Language.Instructions.VariableDeclaration())
			Try

				If _RawSourceTokens.Length < 1 Then Throw New DSValidationException("The Tokens Array was Empty. A Program cannot be constructed without any Tokens", "[The Raw Source Tokens]")

				REM Example Source
				'
				'Function <Void> Main ()
				'	Output("Hello, World!")
				'	Return
				'EndFunction
				'
				'<Number> Age : Maths_GetPi()

				REM Only two forms of IInstruction can exist at the top statement level: [DSFunction]s and [VariableDeclaration]s
				REM Therefore, the first Token must be either [a Keyword with value "Function"] or [a GrammarChar with value "<"]

				Dim _Collected_GlobalVarDecs As New List(Of Language.Instructions.VariableDeclaration)()
				Dim _Collected_DSFunctions As New List(Of Language.Instructions.Statements.DSFunction)()

				Dim _RemainingTokens As Token() = _RawSourceTokens.ToArray()

				Do Until _RemainingTokens.Count = 0

					If BuiltInTPVs.Keyword_Function_TPV.IsSatisfiedBy({_RemainingTokens(0)}) Then

						LogLexingMessage("Identified a Function Declaration...")

						REM Expecting:
						'	Function <Void> SayHello ()
						'	Function <Number@> SayHello (<String> Name)

						REM We want to call [New DSFunction()] with the _RemainingTokens.Until(the next EndFunction)
						REM Then, set the _RemainingTokens to _RemainingTokens.Skip(_TokensJustUsed.Length)

						Dim _LastWantedToken_Detector As Func(Of Token, Boolean) = AddressOf BuiltInTPVs.StatementEnd_Function_TPV.IsSatisfiedBy

						REM Ensure that there is something to tell us where this IInstruction ends in the Source
						If Not _RemainingTokens.Any(_LastWantedToken_Detector) Then Throw New DSMissingTokenException("An " & Language.Constants.StatementEnd_Function & " was expected but not present for the Function Declaration begginning at Location " & _RemainingTokens(0).LocationInSource.ToString() & ".", New Token(Language.Constants.StatementEnd_Function, Token.TokenType.StatmentEnd, 0, 0))

						'For [the below assumtion that the Token following the next EndFunction is a {LineEnd}] to work, we should ensure that there is a {LineEnd} following the next EndFunction
						If Not Runtime.BuiltInTPVs.LineEnd_TPV.IsSatisfiedBy(_RemainingTokens.ElementAt(_RemainingTokens.IndexOfFirstWhere(_LastWantedToken_Detector) + 1)) Then Throw New DSUnexpectedTokenException("The Token following the next EndFunction was not a {LineEnd}. It's value was " & _RemainingTokens.ElementAt(_RemainingTokens.IndexOfFirstWhere(_LastWantedToken_Detector) + 1).Value.InSquares(), _RemainingTokens)

						'Up to the next [EndFunction {LineEnd}]
						Dim _TokensHerefor As Runtime.Token() = _RemainingTokens.UpToAndIncluding(_LastWantedToken_Detector).Concat({New Runtime.Token(vbCrLf, Token.TokenType.LineEnd, 0, 0)}).ToArray()
						_Collected_DSFunctions.Add(New Language.Instructions.Statements.DSFunction(_TokensHerefor))

						_RemainingTokens = _RemainingTokens.Skip(_TokensHerefor.Length).ToArray()
						LogLexingMessage("...Finished constructing a DSFunction for " & _Collected_DSFunctions.Last().Identifier.InSquares())

					ElseIf BuiltInTPVs.OpeningDataTypeBracket_TPV.IsSatisfiedBy({_RemainingTokens(0)}) Then

						LogLexingMessage("Identified a Global Variable Declaration...")

						REM Expecting:
						'	<String> Name
						'	<String> Name = "Ben"

						REM We want to call [New VariableDeclaration()] with the _RemainingTokens.Until(the next {LineEnd})
						REM Then, set the _RemainingTokens to _RemainingTokens.Skip(_TokensJustUsed.Length)

						Dim _LastWantedToken_Detector As Func(Of Token, Boolean) = AddressOf BuiltInTPVs.LineEnd_TPV.IsSatisfiedBy

						REM Ensure that there is something to tell us where this IInstruction ends in the Source
						If Not _RemainingTokens.Any(_LastWantedToken_Detector) Then Throw New DSMissingTokenException("A LineEnd was expected but not present for the Global Variable Declaration begginning at Location " & _RemainingTokens(0).LocationInSource.ToString() & ".", New Token(vbCrLf, Token.TokenType.LineEnd, 0, 0))
						Dim _TokensHerefor As Runtime.Token() = _RemainingTokens.UpToAndIncluding(_LastWantedToken_Detector) 'Up to the first {LineEnd}

						_Collected_GlobalVarDecs.Add(New Language.Instructions.VariableDeclaration(_TokensHerefor))

						_RemainingTokens = _RemainingTokens.Skip(_TokensHerefor.Length).ToArray()
						LogLexingMessage("...Finished constructing a Global Variable Declaration for " & _Collected_GlobalVarDecs.Last().Identifier.InSquares())

					Else : Throw New DSUnexpectedTokenException("Unexpected Token at Top Statement (Program) Level. Only Variable Declarations and Functions can exist here.", _RemainingTokens(0))
					End If

				Loop

				REM !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
				REM It is not the responsibility of *this* method to ensure that all the DSFunctions and GlobalVarDecs
				REM		have unique identifiers; that is done in New() and again in Run()
				REM !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

				Return New Tuple(Of Language.Instructions.Statements.DSFunction(), Language.Instructions.VariableDeclaration())(_Collected_DSFunctions.ToArray(), _Collected_GlobalVarDecs.ToArray())

			Catch _Ex As Exception : Throw New DSException("@Program\GetFunctionsAndGlobalVarDecsFromTokens_: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>Produces a New SymbolTable with the BuiltInFunctions from the ExecutionContext</summary>
		Public Shared Function GenerateGlobalSymbolTable(ByRef _ExeCxt As ExecutionContext) As SymbolTable
			Try : LogExecutionMessage("Generating a GlobalSymbolTable from ExecutionContext: " & _ExeCxt.ToString() & "...")

				Dim _SymTbl_ToReturn As New SymbolTable(Runtime.Constants.GlobalSymbolTable_Description)

				REM Add each BIF in the _ExeCxt
				For Each _BIF As Runtime.BuiltInFunction In _ExeCxt.BuiltInFunctions
					_SymTbl_ToReturn.AddEntry(_BIF.Identifier, New Runtime.SymbolTable.SymbolTableEntry(Of BuiltInFunction)(_BIF, SymbolTable.SymbolTableEntry(Of Global.DocScript.Runtime.BuiltInFunction).SymbolTableEntryFlags.None))
				Next

				LogExecutionMessage("...Produced this Global SymbolTable from the ExeCxt: " & _SymTbl_ToReturn.ToString())
				_SymTbl_ToReturn.Entries_ReadOnlyCopy.Select(Of String)(Function(_KVP As KeyValuePair(Of String, SymbolTable.ISymbolTableEntry)) _KVP.Key).ToList().ForEach(Sub(_EntryIdentifier$) LogDebugMessage("GlobalSymbolTable Entry: " & _EntryIdentifier.InSquares(), LogEvent.DSEventCatagory.Execution))

				Return _SymTbl_ToReturn

			Catch _Ex As Exception : Throw New DSException("@ProgramTreeXML: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>Determines weather or not the _FunctionSignature is that of a DocScript EntryPoint Function [Main]</summary>
		Public Shared ReadOnly Property IsEntryPointFunctionSignature(ByVal _FunctionSignature As Language.Instructions.Statements.DSFunction.Signature) As Boolean
			Get
				Return ({Program.ValidEntryPointFunction_Signature_Unadorned, Program.ValidEntryPointFunction_Signature_OSInterop} _
				 .Where(Function(_ValidSignature As Language.Instructions.Statements.DSFunction.Signature) _FunctionSignature = _ValidSignature) _
				 .Count() = 1)
			End Get
		End Property


#End Region

#Region "Instance Properties"

		''' <summary>The state of this Symbol Table persists after Program Execution, for analysis purposes</summary>
		Public Property GlobalSymbolTable As DocScript.Runtime.SymbolTable

		''' <summary>This *CAN BE NOTHING*, meaning that the Program should never be executed and is simply for e.g. producing an [XML Program Tree]</summary>
		Public Property ExecutionContext As Runtime.ExecutionContext

		''' <summary>The Code returned by the DocScript program (if there is one), otherwise [DocScript.Runtime.Constants.ProgramExitCode_Default]</summary>
		Public ReadOnly Property ExitCode As Int32
			Get
				Return Me.ExitCode_
			End Get
		End Property
		Protected ExitCode_ As Int32

		''' <summary>The DSFunctions from the DocScript Source</summary>
		Public ReadOnly Property Functions As ObjectModel.ReadOnlyCollection(Of Language.Instructions.Statements.DSFunction)
			Get
				Return New ObjectModel.ReadOnlyCollection(Of Language.Instructions.Statements.DSFunction)(Me.Functions_)
			End Get
		End Property
		Protected ReadOnly Functions_ As Language.Instructions.Statements.DSFunction()

		''' <summary>The Global VariableDeclarations from the DocScript Source</summary>
		Public ReadOnly Property GlobalVarDecs As ObjectModel.ReadOnlyCollection(Of Language.Instructions.VariableDeclaration)
			Get
				Return New ObjectModel.ReadOnlyCollection(Of Language.Instructions.VariableDeclaration)(Me.GlobalVarDecs_)
			End Get
		End Property
		Protected ReadOnly GlobalVarDecs_ As Language.Instructions.VariableDeclaration()

		''' <summary>Used when Compiling the DocScript Program to an EXE</summary>
		Public Name$, Description$, Version$, CopyrightMessage$, Author$

#End Region

		''' <summary>Instanciates a New DocScript Program from Tokens</summary>
		''' <param name="_ExecutionContext">
		''' Can be Nothing (null), if you never intend on running the Program. (I.e. just using it to Product a Program Tree etc...)
		''' The ExeCxt is passed in as a constructor argument, instead of an argument to Run(),
		''' because you shouldn't really need to specify it every time you want to run the Program,
		''' and things like the BuiltInFunctions are a parameter of the Program, not a particular Invoccation of it.
		''' It is, however, a Mutable member of the class, so you can alter these params between Run() calls...
		''' </param>
		Public Sub New(ByVal _Tokens As Token(), ByVal _ExecutionContext As Runtime.ExecutionContext)

			LogLexingMessage("Began Program Construction (Lexing) with " & _Tokens.Length.ToString() & " Tokens...", LogEvent.DSEventSeverity.Infomation)

			Me.ExecutionContext = _ExecutionContext REM COULD BE NOTHING
			'The ExeCxt is passed in as a constructor argument, instead of an argument to Run(),
			'because you shouldn't really need to specify it every time you want to run the Program,
			'and things like the BuiltInFunctions are a parameter of the Program, not a particular Invoccation of it.
			'It is, however, a Mutable member of the class, so you can alter these params between Run() calls...

			Dim _FunctionsAndGlobalVarDecs As Tuple(Of Language.Instructions.Statements.DSFunction(), Language.Instructions.VariableDeclaration()) =
			 Program.GetFunctionsAndGlobalVarDecsFromTokens_(_Tokens)
			Me.Functions_ = _FunctionsAndGlobalVarDecs.Item1 : Me.GlobalVarDecs_ = _FunctionsAndGlobalVarDecs.Item2

			REM !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
			REM It is the responsibility of *this* method to ensure that all the DSFunctions and GlobalVarDecs
			REM		have unique identifiers - this is done again in Run() before Execution
			REM !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

			'All GlobalVarDecs and DSFunctions *and BIFs* must have unique identifiers
			If Not _
			 Me.Functions.Select(Of String)(Function(_DSFunction As Language.Instructions.Statements.DSFunction) _DSFunction.Identifier.ToUpper()) _
			 .Concat(Me.GlobalVarDecs.Select(Of String)(Function(_VarDec As Language.Instructions.VariableDeclaration) _VarDec.Identifier.ToUpper())) _
			 .Concat(If(Me.ExecutionContext Is Nothing, {}, Me.ExecutionContext.BuiltInFunctions.Select(Of String)(Function(_BIF As BuiltInFunction) _BIF.Identifier.ToUpper()))) _
			 .ToArray().AllElementsAreUnique() _
			 Then Throw New DSValidationException("All BuiltInFunctions, DocScript-defined Functions, and Global Variables must have unique identifiers (and they weren't all unique)", "(The entry identifiers which would have been added to the Global SymbolTable)")

			'Ensure that there in exactly one Main() DSFunction
			If Not Me.Functions.Where(Function(_DSFunction As Language.Instructions.Statements.DSFunction) Program.IsEntryPointFunctionSignature(_DSFunction.EffectiveSignature)).Count() = 1 Then _
			 Throw New DSValidationException("The Program must contain exactly 1 EntryPoint Function Main. The valid signatures are: " & {Program.ValidEntryPointFunction_Signature_Unadorned, Program.ValidEntryPointFunction_Signature_OSInterop}.GetStandardArraySerialisation(), "(The absense of a valid Main Function) The Program's Function Signatures were: " & Me.Functions.Select(Of Language.Instructions.Statements.DSFunction.Signature)(Function(_DSFunction As Language.Instructions.Statements.DSFunction) _DSFunction.EffectiveSignature).ToArray().GetStandardArraySerialisation())
			LogLexingMessage("Found an EntryPoint Function in the Program")

			LogLexingMessage("Ensuring all Function and Global VariableDeclaration Identifiers are unique")
			If Not Me.Functions.Select(Function(_DSFunction As DocScript.Language.Instructions.Statements.DSFunction) _DSFunction.Identifier.ToUpper()).ToArray().AllElementsAreUnique() Then Throw New DSValidationException("All Functions within a DocScript Program must have unique Identifiers", String.Join(", ", Me.Functions.Select(Function(_DSFunction As DocScript.Language.Instructions.Statements.DSFunction) _DSFunction.Identifier.ToUpper()).ToArray()).InCurlies())
			If Not Me.GlobalVarDecs.Select(Function(_GlblVarDec As DocScript.Language.Instructions.VariableDeclaration) _GlblVarDec.Identifier.ToUpper()).ToArray().AllElementsAreUnique() Then Throw New DSValidationException("All Global Variables within a DocScript Program must have unique Identifiers", String.Join(", ", Me.GlobalVarDecs.Select(Function(_GlblVarDec As DocScript.Language.Instructions.VariableDeclaration) _GlblVarDec.Identifier.ToUpper()).ToArray()).InCurlies())

			LogLexingMessage("...Finished Program Construction (Lexing) with " & Me.Functions.Count.ToString() & " Function(s) and " & Me.GlobalVarDecs.Count.ToString() & " Global Variable Declaration(s).", LogEvent.DSEventSeverity.Infomation)

		End Sub

		''' <summary>SideLoading Constructor: Instanciates a New DocScript Program from DocScript Source</summary>
		Public Shared Function FromSource(ByVal _Source As [String], ByRef _ExeCxt As ExecutionContext) As Runtime.Program
			Return (New [Program]([Parser].[GetTokensFromSource]([_Source]), [_ExeCxt]))
		End Function

		''' <summary>
		''' Executes the DocScript EntryPoint Function Main(), passing in any CLAs, and returning the ExitCode if there is one.
		''' (Otherwise, DocScript.Runtime.Constants.ProgramExitCode_Default is returned as the ExitCode)
		''' This datum is wrapped up inside the ExecutionResult
		''' </summary>
		Public Function Run(ByVal _CommandLineArguments$()) As Language.Instructions.ExecutionResult

			LogExecutionMessage(String.Format("Began Program Execution with {0} Functions, {1} GlobalVarDecs, and an ExecutionContext of {2}...", Me.Functions.Count.ToString(), Me.GlobalVarDecs.Count.ToString(), Me.ExecutionContext.ToString()), LogEvent.DSEventSeverity.Infomation)

			REM ╔═════════════════════════════════════════════╗
			REM ║         DocScript Execution Process         ║
			REM ╚═════════════════════════════════════════════╝

			REM 1) Initialisation
			'		- If the ExeCxt is Nothing, then this Program instance shouldn't Run()
			'		- Generate the GlobalSymbolTable
			'		- Add Program Functions to the GlobalSymbolTable

			REM 2) Invocation
			'		- Execute() each GlobalVarDec
			'		- Run() DSFunction Main (located in the GlobalSymbolTable)
			'			- If it is the Signature with the CLAs, then:
			'				- Pass in _CommandLineArguments$()
			'				- Ensure that a ReturnValue is produced
			'		- Update the GlobalSymbolTable, to include any Modifications from during execution
			'		- Return this Program's ExitCode, wrapped in an ExeRes

			REM		[*] → [*]

			Try

				REM !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
				REM !!!!!!!!!!!!!! ↓ Initialisation ↓ !!!!!!!!!!!!!!!!
				REM !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

				If (Me.ExecutionContext Is Nothing) Then Throw New DSInvalidCallException("A [Program] cannot be Run() when its [ExecutionContext] is [Nothing]", "Program\Run()")

				'Initialise the ExecutionResult for the Program
				Dim _ExeRes As Language.Instructions.ExecutionResult = Language.Instructions.ExecutionResult.New_AndStartExecutionTimer("Program")
				Me.GlobalSymbolTable = Program.GenerateGlobalSymbolTable(Me.ExecutionContext) 'This call will add the ExecutionContext's BuiltInFunctions to the SymTbl
				Dim _Modified_SymTbls As Runtime.SymbolTablesSnapshot = Me.GlobalSymbolTable.SnapshotContainingJustThis.MustNotBeNothing("GlobalSymbolTable's [SnapshotContainingJustThis] was Nothing")

				'Add Program Functions to the GlobalSymbolTable
				For Each _DSFunction As Language.Instructions.Statements.DSFunction In Me.Functions
					LogExecutionMessage("Adding DSFunction " & _DSFunction.Identifier.InSquares() & " to Global SymbolTable")
					_Modified_SymTbls.AddEntryToToTopmost(_DSFunction.Identifier, New SymbolTable.SymbolTableEntry(Of Language.Instructions.Statements.DSFunction)(_DSFunction))
				Next

				REM !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
				REM !!!!!!!!!!!!!!! ↓  Invoccation  ↓ !!!!!!!!!!!!!!!!
				REM !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

				REM Should any [BIFs], [Global Variables], or [DSFunctions] have non-unique identifiers,
				REM	a DSDuplicateSymbolTableEntry will automatically be Thrown by the SymbolTable(s)

				'Execute() each GlobalVarDec
				For Each _GlobalVarDec As Language.Instructions.VariableDeclaration In Me.GlobalVarDecs

					'Log
					LogExecutionMessage("Executing Global VariableDeclaration for " & _GlobalVarDec.Identifier.InSquares() & " on Global SymbolTable")

					'Execute()
					Dim _GlobalVarDec_ExeRes As Language.Instructions.ExecutionResult = _GlobalVarDec.Execute(_Modified_SymTbls)

					'Add as an Upstairs ExeRes
					_ExeRes.UpstairsExecutionResults.Add(New Tuple(Of String, Language.Instructions.ExecutionResult)("Global VariableDeclaration " & _GlobalVarDec.Identifier.InBrackets(), _GlobalVarDec_ExeRes))

					'Update my SymbolTables
					_Modified_SymTbls = _GlobalVarDec_ExeRes.ResultantSymbolTablesState

				Next

				REM <Run() the EntryPoint Function>

				'Proove existance of
				If Not Me.Functions.Where(Function(_DSFunction As Language.Instructions.Statements.DSFunction) Program.IsEntryPointFunctionSignature(_DSFunction.EffectiveSignature)).Count() = 1 Then _
				Throw New DSValidationException("The Program must contain exactly 1 EntryPoint Function Main. The valid signatures are: " & {Program.ValidEntryPointFunction_Signature_Unadorned, Program.ValidEntryPointFunction_Signature_OSInterop}.GetStandardArraySerialisation(), "(The absense of a valid Main Function)")

				'Procure
				Dim _MainFunction As Language.Instructions.Statements.DSFunction =
				 _Modified_SymTbls.Bottommost _
				 .GetEntry(Runtime.Constants.EntryPointFunction_Identifier) _
				 .MustBe(Of SymbolTable.SymbolTableEntry(Of Language.Instructions.Statements.DSFunction))() _
				 .Value

				'Log
				LogExecutionMessage("Identified EntryPoint Function with signature: " & _MainFunction.EffectiveSignature.ToString(), LogEvent.DSEventSeverity.Infomation)

				'Run()
				Dim _MainFunction_ExeRes As Language.Instructions.ExecutionResult = Nothing
				If _MainFunction.EffectiveSignature = (Program.ValidEntryPointFunction_Signature_Unadorned) Then
					LogExecutionMessage("Executing Unadorned EntryPoint Function...")
					'No CLAs to pass in, and no ExitCode
					_MainFunction_ExeRes = _MainFunction.Run(_Modified_SymTbls, {})
				ElseIf _MainFunction.EffectiveSignature = (Program.ValidEntryPointFunction_Signature_OSInterop) Then
					LogExecutionMessage("Executing OSInterop EntryPoint Function with CLAs " & _CommandLineArguments.GetStandardArraySerialisation() & "...")
					'Pass in the Command-Line Arguments, and there should be an ExitCode ReturnValue (procured later on...)
					_MainFunction_ExeRes = _MainFunction.Run(_Modified_SymTbls, {New Language.Variables.DSArray(Of Language.Variables.DSString)((From _CLA As String In _CommandLineArguments Select New Language.Variables.DSString(_CLA)).ToArray())})
				Else : Throw New DSValidationException("Despite purportedly being an EntryPoint, the _MainFunction's Signature was not recognised", _MainFunction.EffectiveSignature.ToString())
				End If

				LogExecutionMessage("...Finished Executing EntryPoint Function")

				'Add as an Upstairs ExeRes
				_ExeRes.UpstairsExecutionResults.Add(New Tuple(Of String, Language.Instructions.ExecutionResult)("EntryPoint (Main) Function", _MainFunction_ExeRes))

				'Update my SymbolTables
				_Modified_SymTbls = _MainFunction_ExeRes.ResultantSymbolTablesState()

				'Set the Program_ExitCode
				If _MainFunction.EffectiveSignature = (Program.ValidEntryPointFunction_Signature_Unadorned) Then

					Me.ExitCode_ = Runtime.Constants.ProgramExitCode_Default

				ElseIf _MainFunction.EffectiveSignature = (Program.ValidEntryPointFunction_Signature_OSInterop) Then

					Try : If _MainFunction_ExeRes.ReturnStatus.CurrentDSFunction_ReturnValue Is Nothing Then Throw New DSValidationException("The OSInterop-style Main Function did not return a Value as was promised", "(The absence of the value)")
						Me.ExitCode_ = Convert.ToInt32(_MainFunction_ExeRes.ReturnStatus.CurrentDSFunction_ReturnValue.Coerce(Of Language.Variables.DSNumber)().Value)
					Catch _Ex As Exception : Throw New DSException("The EntryPoint's Return Value could not be converted to the Program's Exit code, because: " & _Ex.Message) : End Try

				Else : Throw New DSValidationException("Despite purportedly being an EntryPoint, the _MainFunction's Signature was not recognised", _MainFunction.EffectiveSignature.ToString())
				End If

				_ExeRes.ReturnStatus.Program_ExitCode = Me.ExitCode

				REM </Run() the EntryPoint Function>

				REM Re-apply the Modifications to the GlobalSymTbl
				Me.GlobalSymbolTable = _Modified_SymTbls.Topmost

				REM Archive a copy of the Global SymbolTable
				_ExeRes.Archived_SymbolTable = Me.GlobalSymbolTable

				_ExeRes.StopExecutionTimer_AndFinaliseObject(_Modified_SymTbls)
				LogExecutionMessage(String.Format("...Program Exited with Code {0}", Me.ExitCode.ToString()), LogEvent.DSEventSeverity.Infomation)
				Return _ExeRes

			Catch _Ex As Exception : Throw New DSException("@Program\Run: " & _Ex.Message, _Ex) : End Try
		End Function

#Region "DS Compilation"

		''' <summary>Accounts for differences in System Drive letters and WinDir locations, and then looks in %WINDIR%\Microsoft.NET\Framework64\v4.0.30319\vbc.exe</summary>
		Public Shared ReadOnly Property DefaultVbcExe_ForCurrentSystem As IO.FileInfo
			Get 'dir %WINDIR%\Microsoft.NET\Framework64\vbc.exe /s /b

				Dim _VBCFile As New IO.FileInfo(
					IO.Path.Combine(
						Environment.GetFolderPath(Environment.SpecialFolder.Windows),
						"Microsoft.NET\Framework\v4.0.30319",
						"vbc.exe"
					)
				)

				If Not _VBCFile.Exists Then Throw New IO.FileNotFoundException("The Default VBC.exe file did not exist", _VBCFile.FullName)

				Return _VBCFile

			End Get
		End Property

		''' <summary>Packages up the DocScript Program, and all required Runtime Resources, into a standalone .exe file, which needs only the .NET Framework present on the client.</summary>
		Public Sub CompileToEXE(
			ByVal _OutputDirectory As IO.DirectoryInfo,
			ByVal _FinalExeFile_NameOnly$,
			ByVal _VBCExe As IO.FileInfo,
			ByVal _ILMergeExe As IO.FileInfo,
			ByVal _DSCoreDll As IO.FileInfo,
			ByVal _IcoFile_ForExe As IO.FileInfo,
			ByVal _DeleteIntermediateFiles As Boolean
		)
			Try

				REM Generate DSPackage contents as a string
				REM Save [DSPackage.VB] in {_OutputDirectory}
				REM ShellExecute:	\vbc.exe DSPackage.VB /out:DSPackage.exe /target:exe /win32icon:DocScriptFile.ICO /reference:DSCore.dll
				REM ShellExecute:	\ILMerge.exe DSPackage.exe DSCore.dll /out:ILM_DSPackage.exe
				REM Delete [DSPackage.VB] and [DSCore.dll] from {_OutputDirectory}

				Const _DSPackageVB_FileName$ = "DSPackage.VB", _DSPackageExe_FileName$ = "DSPackage.exe" 'Created in the _OutputDirectory

				'DSCompilation requires either the GUIDefault or CLIDefault ExecutionContexts
				'The ExecutionContext in use also dictates the /target:* argument for vbc.exe
				If (
					(Me.ExecutionContext Is Nothing) OrElse
					Not {Runtime.ExecutionContext.GUIDefault, Runtime.ExecutionContext.CLIDefault}.Any(Function(_ExeCxt As Runtime.ExecutionContext) Me.ExecutionContext = _ExeCxt)
				) Then Throw New DSValidationException("DSCompilation requires that the Program use the GUIDefault or CLIDefault ExecutionContext", "ExecutionContext: " & Me.ExecutionContext.ToString())

				'VBEncoding Considerations:
				'	- Replace " with ""
				'	- Prepend each line with	"
				'	- Append each line with		" & vbcrlf & _
				Dim _VBEncoded_DSSource As String = String.Join("",
					Me.ReconstructedSource _
						.Replace(""""c, """""") _
						.Split({vbCrLf}, StringSplitOptions.RemoveEmptyEntries) _
						.Select(Of String)(Function(_DSLine$) """"c & _DSLine.Trim() & """ & vbcrlf & _" & vbCrLf)
				).ToCharArray().RemoveFromEnd(15) 'Remove the final " & vbcrlf & _

				Dim _DSPackage_SourceString$ =
				 "REM Automatically generated by DocScript Compilation @ " & DateTime.Now.ToString().Replace(vbCrLf, "{LineBreak}") & vbCrLf &
				 vbCrLf &
				 "<Assembly: System.Reflection.AssemblyTitle(""" & Me.Name & ": " & Me.Description & """)>" & vbCrLf &
				 "<Assembly: System.Reflection.AssemblyDescription(""" & Me.Description & """)>" & vbCrLf &
				 "<Assembly: System.Reflection.AssemblyProduct(""" & Me.Name & """)>" & vbCrLf &
				 "<Assembly: System.Reflection.AssemblyCopyright(""" & Me.CopyrightMessage & """)>" & vbCrLf &
				 "<Assembly: System.Reflection.AssemblyTrademark(""" & Me.Author & """)>" & vbCrLf &
				 "<Assembly: System.Reflection.AssemblyCompany(""" & Me.Author & """)>" & vbCrLf &
				 "<Assembly: System.Reflection.AssemblyFileVersion(""" & Me.Version.ToString() & """)>" & vbCrLf &
				 "<Assembly: System.Reflection.AssemblyVersion(""" & Me.Version.ToString() & """)>" & vbCrLf &
				 "<Assembly: System.Resources.NeutralResourcesLanguageAttribute(""en-GB"")>" & vbCrLf &
				 "<Assembly: System.Runtime.InteropServices.ComVisible(False)>" & vbCrLf &
				 vbCrLf &
				 "Public Module [DSPackage]" & vbCrLf &
				 vbCrLf &
				 vbTab & "Public Function [Main](ByVal [_CLAs] As [System].[String]()) As [System].[Int32]" & vbCrLf &
				 vbTab & vbTab & "Try" & vbCrLf &
				 vbCrLf &
				 vbTab & vbTab & vbTab & "Dim [_ExeCxtToUse] As [DocScript].[Runtime].[ExecutionContext] = _" & vbCrLf &
				 vbTab & vbTab & vbTab & vbTab & "[DocScript].[Runtime].[ExecutionContext].[" & If(Me.ExecutionContext = (Runtime.ExecutionContext.GUIDefault), "GUI", "CLI") & "Default]" & vbCrLf &
				 vbCrLf &
				 vbTab & vbTab & vbTab & "Dim [_Program] As [DocScript].[Runtime].[Program] = _" & vbCrLf &
				 vbTab & vbTab & vbTab & vbTab & "[DocScript].[Runtime].[Program].[FromSource]([_Source]:=[DSPackage].[DSSource], [_ExeCxt]:=[_ExeCxtToUse])" & vbCrLf &
				 vbCrLf &
				 vbTab & vbTab & vbTab & "Return [_Program].[Run]([_CLAs]).[ReturnStatus].[Program_ExitCode]" & vbCrLf &
				 vbCrLf &
				 vbTab & vbTab & "Catch [_DSRuntimeEx] As [System].[Exception]" & vbCrLf &
				 vbTab & vbTab & vbTab & "Dim [_ErrorMsg] As [System].[String] = ""A Runtime Error has occured in the compiled DocScript Program:"" & [vbCrLf] & [_DSRuntimeEx].Message" & vbCrLf &
				 vbTab & vbTab & vbTab & If(Me.ExecutionContext = (Runtime.ExecutionContext.GUIDefault), "[Microsoft].[VisualBasic].[Interaction].[MsgBox]([_ErrorMsg], [Microsoft].[VisualBasic].[MsgBoxStyle].[Critical])", "[DocScript].[CompilerExtentions].[UsefulMethods].[ConsoleWriteInColour]([_ErrorMsg], [ConsoleColor].[Red])") & vbCrLf &
				 vbTab & vbTab & vbTab & "Return -1" & vbCrLf &
				 vbTab & vbTab & "End Try" & vbCrLf &
				 vbTab & "End Function" & vbCrLf &
				 vbCrLf &
				 vbTab & "Public [DSSource] As [String] = _" & vbCrLf &
				 _VBEncoded_DSSource.TabbedInBy(2) & vbCrLf &
				 vbCrLf &
				 "End Module"

				'Save DSPackage.VB
				My.Computer.FileSystem.WriteAllText(IO.Path.Combine(_OutputDirectory.FullName, _DSPackageVB_FileName), _DSPackage_SourceString, append:=False)

				'Run vbc.exe
				If Not _VBCExe.Exists Then Throw New IO.FileNotFoundException("The copy of vbc.exe does not exist", _VBCExe.FullName)
				Dim _VBCProcess As New Process() : With _VBCProcess
					.StartInfo.CreateNoWindow = True : .StartInfo.WindowStyle = ProcessWindowStyle.Hidden
					.StartInfo.FileName = _VBCExe.FullName
					.StartInfo.Arguments = """"c & IO.Path.Combine(_OutputDirectory.FullName, _DSPackageVB_FileName) & """ /out:""" & IO.Path.Combine(_OutputDirectory.FullName, _DSPackageExe_FileName) & """ /target:" & If(Me.ExecutionContext = (Runtime.ExecutionContext.GUIDefault), "winexe", "exe") & " /win32icon:""" & _IcoFile_ForExe.FullName & """ /reference:""" & _DSCoreDll.FullName & """"c
				End With
				Dim _VBCOutput As String = UsefulMethods.RunProcess_AndReturnCommandLineOutput(_VBCProcess)
				If Not (_VBCProcess.ExitCode = 0) Then Throw New DSException("The VBC.exe Process returned a non-zero ExitCode of " & _VBCProcess.ExitCode & ". Its output was: " & vbCrLf & _VBCOutput)

				'Run ILMerge.exe
				If Not _ILMergeExe.Exists Then Throw New IO.FileNotFoundException("The copy of ILMerge.exe does not exist", _ILMergeExe.FullName)
				Dim _ILMergeProcess As New Process() : With _ILMergeProcess
					.StartInfo.CreateNoWindow = True : .StartInfo.WindowStyle = ProcessWindowStyle.Hidden
					.StartInfo.FileName = _ILMergeExe.FullName
					.StartInfo.Arguments = """"c & IO.Path.Combine(_OutputDirectory.FullName, _DSPackageExe_FileName) & """ """ & _DSCoreDll.FullName & """ /out:""" & IO.Path.Combine(_OutputDirectory.FullName, _FinalExeFile_NameOnly) & """"c
				End With
				Dim _ILMergeOutput As String = UsefulMethods.RunProcess_AndReturnCommandLineOutput(_ILMergeProcess)
				If Not (_ILMergeProcess.ExitCode = 0) Then Throw New DSException("The ILMerge.exe Process returned a non-zero ExitCode of " & _ILMergeProcess.ExitCode & ". Its output was: " & vbCrLf & _VBCOutput)

				'Delete DSPackage.VB and DSPackage.exe and {EXEName}.pdb
				If _DeleteIntermediateFiles Then Call {
					_DSPackageExe_FileName,
					_DSPackageVB_FileName,
					_FinalExeFile_NameOnly.Split("."c).ElementAt(0) & ".pdb"
				}.ToList().ForEach(Sub(_FileToDelete_PathRelativeToOutputDir$) IO.File.Delete(IO.Path.Combine(_OutputDirectory.FullName, _FileToDelete_PathRelativeToOutputDir)))

			Catch _Ex As Exception : Throw New DSException("@Program\CompileToEXE: " & _Ex.Message, _Ex) : End Try
		End Sub

#End Region

#Region "Serialisation Logic"

		''' <summary>Produces a DS-KVP Human-readable String for the Object</summary>
		Public Overrides Function ToString() As String
			Return DocScript.Utilities.KVPSerialisation.GetKVPString({"GlobalVarDecsCount", Me.GlobalVarDecs.Count.ToString()}, {"FunctionsCount", Me.Functions.Count.ToString()})
		End Function

		''' <summary>Gets the XML form of the DocScript Program, including all Global Variable Declarations, and Functions with their XML Contents.</summary>
		Public ReadOnly Property ProgramTreeXML As XElement
			Get
				Try

					Dim _XElementToReturn As XElement = <Program/>

					Dim _GlobalVarDecsXElement As XElement = <GlobalVarDecs/>
					For Each _GlobalVarDec As Language.Instructions.VariableDeclaration In Me.GlobalVarDecs
						_GlobalVarDecsXElement.Add(_GlobalVarDec.GetProgramTreeNodeXML())
					Next
					_XElementToReturn.Add(_GlobalVarDecsXElement)

					Dim _FunctionsXElement As XElement = <Functions/>
					For Each _DSFunction As Language.Instructions.Statements.DSFunction In Me.Functions
						_FunctionsXElement.Add(_DSFunction.GetProgramTreeNodeXML())
					Next
					_XElementToReturn.Add(_FunctionsXElement)

					Return _XElementToReturn

				Catch _Ex As Exception : Throw New DSException("@ProgramTreeXML: " & _Ex.Message, _Ex) : End Try
			End Get
		End Property

		''' <summary>Reconstructs a DocScript-Source version of the Program, by recursively serialising each Instruction</summary>
		Public ReadOnly Property ReconstructedSource As [String]
			Get
				Try

					Dim _Reconstruction$ = String.Empty

					For Each _GlobalVarDec As Language.Instructions.VariableDeclaration In Me.GlobalVarDecs : _Reconstruction &= (_GlobalVarDec.ToString() & Language.Constants.LineBreak) : Next
					For Each _DSFunction As Language.Instructions.Statements.DSFunction In Me.Functions : _Reconstruction &= (_DSFunction.ToString() & Language.Constants.LineBreak) : Next

					'We'd rather not have a superflous LineBreak on the end
					If _Reconstruction.EndsWith(DocScript.Language.Constants.LineBreak) Then _Reconstruction = _Reconstruction.WithLastCharacterRemoved().WithLastCharacterRemoved()

					Return _Reconstruction

				Catch _Ex As Exception : Throw New DSException("@ReconstructedSource: " & _Ex.Message, _Ex) : End Try
			End Get
		End Property

#End Region

	End Class

End Namespace