Namespace Utilities

	''' <summary>
	''' Manages a set of CLAData to act on any specified Command-Line Arguments, and to display a CLA Help Dictionary if the argument /? is specified.
	''' Runs the relevant CLADatums' Actions-If-Specified, as soon as the CLAManager is constructed.
	''' </summary>
	Partial Public Class CLAManager

		''' <summary>
		''' Represents a configuration for a single CLA key, and what to do if it's specified.
		''' (Specified in the Syntax /Key or /Key:Value or /Key:"Value")
		''' NOTE: CLA Keys are not case-sensitive; /Name is the same as /NAmE
		''' </summary>
		''' <remarks></remarks>
		Public Class CLADatum

			Public ReadOnly Key As [String]
			Public ReadOnly Description As [String]
			Public ReadOnly MustBeSpecified As [Boolean]
			Public ReadOnly MustHaveValue As [Boolean]
			Public ReadOnly ActionIfSpecified As Action(Of String)

			''' <summary>
			''' Constructs a new Object to represent the desired configuration for a single CLA Key (Specified in the Syntax /Key or /Key:Value or /Key:"Value")
			''' </summary>
			''' <param name="_Key">The CLA Key E.g. "ShowLog". The forward slash / is not to be included in this string. NOTE: CLA Keys are not case-sensitive; /Name is the same as /NAmE</param>
			''' <param name="_Description">The Description used in the CLA Help Dictionary ( /? )</param>
			''' <param name="_MustBeSpecified">Indicates that the Key must be specified in the Command-Line Arguments provided to the CLAManager, otherwise an Exception is Thrown.</param>
			''' <param name="_MustHaveValue">Indicates that if the Key is specified, then a corrosponding Value must also be specified, otherwise an Exception is Thrown.</param>
			''' <param name="_ActionIfSpecified">The Action to run if the Key is specified. The String Argument is the Value corrosponding to the Key, which could be [Nothing] (null) {nullptr} &lt;undefined&gt; if there is no Value corrosponding to the Key</param>
			''' <remarks></remarks>
			Public Sub New( _
			 ByVal _Key$,
			 ByVal _Description$,
			 ByVal _MustBeSpecified As Boolean,
			 ByVal _MustHaveValue As Boolean,
			 ByVal _ActionIfSpecified As Action(Of String) _
			)

				If _Key.Contains(CLAManager.CLAKeyStartChar) OrElse _Key.Contains(CLAManager.CLAValueStartChar) Then Throw New Exception("A Command-Line Argument Key in a CLADatum cannot contain the CLAKeyStartChar " & CLAManager.CLAKeyStartChar & " or the CLAValueStartChar " & CLAManager.CLAValueStartChar)
				If _Key.Contains(" "c) Then Throw New Exception("A Command-Line Argument Key in a CLADatum cannot contain a space [ ], because this character is used by the Operating System to seperate Command-Line arguments, and therfore using it would lead to ambigous Command-Line syntacies")

				Me.Key = _Key
				Me.Description = _Description
				Me.MustBeSpecified = _MustBeSpecified
				Me.MustHaveValue = _MustHaveValue
				Me.ActionIfSpecified = _ActionIfSpecified

			End Sub

		End Class

		Public Const CLAKeyStartChar As Char = "/"c	'E.g.	/ShowLog
		Public Const CLAValueStartChar As Char = ":"c
		Public Const HelpCLA$ = CLAKeyStartChar & "?"c

		Public ReadOnly ProvidedCLAs As ObjectModel.ReadOnlyCollection(Of [String])
		Public ReadOnly OutputDelegate As Action(Of [String])
		Public ReadOnly CLAData As ObjectModel.ReadOnlyCollection(Of CLADatum)
		Public ReadOnly CLAHelpDictionary_Description As [String]
		Public ReadOnly CLAHelpDictionary_Examples As [String]()

		''' <summary>
		''' Indicates that the Help Dictionary will have the Linebreaks and padding-whitespace automatically inserted.
		''' Use True, if this is a Console Application;
		''' If MsgBox()-ing out the /? Dictionary, then use False.
		''' </summary>
		Public UseCommandLineFormatting_ForHelpDictionary As Boolean = True

		''' <summary>Indicates weather /? was handled instead of processing CLAData. If the Help Dictionary is displayed, then no other CLA Actions will be dealt with or run, and HelpDictionaryWasDisplayed will be set to True.</summary>
		Public ReadOnly HelpDictionaryWasDisplayed As Boolean = False

		''' <summary>
		''' <para>Constructs a Command-Line Argument Manager. All managment of the CLAs occours as soon as the object is constructed. No additional call is nessecary.</para>
		''' <para>
		''' Exmaple Call:
		'''
		''' Dim _CLAManager As New DocScript.Utilities.CLAManager(
		'''     _CLAs,
		'''     "DocScript Command-Line Interpreter. Interprets DocScript Source Files.",
		'''     {"DSCLI.EXE /Live",
		'''     "DSCLI.EXE /Live /LogToFile:""DSLive.DSLog"" /ProcessDebugEvents /GUI",
		'''     "DSCLI.EXE /Run /SourceString:""Function &lt;Void&gt; Main ();Output(`Hello, World!`);EndFunction""",
		'''     "DSCLI.EXE /Run /SourceFile:""X:\Programming\DocScript\HelloWorld.DS"" /LogToConsole",
		'''     "DSCLI.EXE /GetProgramTree /SourceString:""Function &lt;Void&gt; Main ();Output(`Hello, World!`);EndFunction""",
		'''     "DSCLI.EXE /Run /SourceString:""Function&lt;Void&gt;Main();System_Beep();EndFunction""",
		'''     "DSCLI.EXE /Run /SourceFile:""BIO2017.DS"" /DocScriptCLAs:""GRBBRB"" /LogToFile:BIO.DSLog"},
		'''     AddressOf Console.WriteLine, True,
		'''     New Utilities.CLAManager.CLADatum("Live", "[Action] Enters a DocScript Live Session: a DS> prompt appears and accepts Statement-level Instructions", False, False, Sub(_Value$) ActionWhenReady = WhenReadyAction.EnterDSLiveSession),
		'''     New Utilities.CLAManager.CLADatum("Run", "[Action] Interprets the DocScript Source (specified by either /SourceFile or /SourceString). This process then returns the ExitCode of the DocScript Program.", False, False, Sub(_Value$) ActionWhenReady = WhenReadyAction.Run),
		'''     New Utilities.CLAManager.CLADatum("GetProgramTree", "[Action] Parses and Lexes the DocScript Source (specified by either /SourceFile or /SourceString), and writes the resultant XML Program tree to the Console Output Stream", False, False, Sub(_Value$) ActionWhenReady = WhenReadyAction.GetProgramTree),
		'''     New Utilities.CLAManager.CLADatum("SourceFile", "[Datum] Specifies the Source via a DocScript Source File", False, True, Sub(_Value$) SourceToInterpret = My.Computer.FileSystem.ReadAllText(_Value)),
		'''     New Utilities.CLAManager.CLADatum("SourceString", "[Datum] Specifies the Source via a DocScript Source String. Use ; for NewLine and ` for StringLiteralStartEndChar.", False, True, Sub(_Value$) SourceToInterpret = _Value.Replace(";", vbCrLf).Replace("`"c, """"c)),
		'''     New Utilities.CLAManager.CLADatum("DocScriptCLAs", "[Datum] Specifies Command-Line Arguments for the DocScript Program", False, True, Sub(_Value$) DocScriptCLAs = _Value.Split(" "c))
		''' )
		''' 
		''' </para>
		''' <para>If the Help Dictionary is displayed, then no other CLA Actions will be dealt with or run, and HelpDictionaryWasDisplayed will be set to True.</para>
		''' <para>NOTE: CLA Keys are not case-sensitive; /Name is the same as /NAmE</para>
		''' </summary>
		''' <param name="_CLAs">The Command-Line Arguments to deal with E.g. My.Application.CommandLineArgs.ToArray() or an "args" Parameter</param>
		''' <param name="_CLAData">A collection of CLADatum Instances (#KnowYourLatin)</param>
		''' <param name="_CLAHelpDictionary_Description">The Text displayed at the start of the CLA Help Dictionary, when it is displayed via the presence of the /? CLA</param>
		''' <param name="_CLAHelpDictionary_Examples">Example Calls to the Binary executable with CLAs. E.g. DSCLI.EXE /RunSourceFile:"X:\Programming\DocScript\HelloWorld.DS"</param>
		''' <param name="_OutputDelegate">Used for E.g. outputting the CLA Help Dictionary if the /? CLA is specified</param>
		''' <remarks></remarks>
		Public Sub New(ByVal _CLAs As String(), ByVal _CLAHelpDictionary_Description$, ByVal _CLAHelpDictionary_Examples$(), ByVal _OutputDelegate As Action(Of String), ByVal _UseCommandLineFormatting_ForHelpDictionary As Boolean, ByVal ParamArray _CLAData As CLADatum())
			Try

				REM Ensure all Keys are Unique
				If Not _CLAData.Select(Of String)(Function(_CLADatum As CLADatum) _CLADatum.Key.ToUpper()).ToArray().AllElementsAreUnique() Then Throw New Exception("The CLAData Keys were not all Unique.")

				Me.OutputDelegate = _OutputDelegate
				Me.ProvidedCLAs = New ObjectModel.ReadOnlyCollection(Of String)(_CLAs.ToList())
				Me.CLAData = New ObjectModel.ReadOnlyCollection(Of CLADatum)(_CLAData.ToList())
				Me.CLAHelpDictionary_Description = _CLAHelpDictionary_Description : Me.CLAHelpDictionary_Examples = _CLAHelpDictionary_Examples
				Me.UseCommandLineFormatting_ForHelpDictionary = _UseCommandLineFormatting_ForHelpDictionary

				If Me.ProvidedCLAs.Contains(HelpCLA) Then
					Me.DisplayHelpDictionary() : Me.HelpDictionaryWasDisplayed = True
				Else
					Me.ExecuteActionsForSpecifiedCLAs()	'Ensures mandatory keys are specified, and executes the relevant [ActionIfSpecified]s
				End If

			Catch _Ex As Exception When True : Throw New DSException("Command-Line Argument Manager: " & _Ex.Message, _Ex) : End Try
		End Sub

		Public Shared Function CLAsContainsKey(ByVal _CLAs$(), ByVal _Key$) As Boolean

			'													/Key																	/Key: {...}
			Return _CLAs.Any(Function(_CLA$) (_CLA.ToUpper() = (CLAKeyStartChar & _Key).ToUpper()) OrElse _CLA.ToUpper().StartsWith((CLAKeyStartChar & _Key & CLAValueStartChar).ToUpper()))

			REM Parenthetically: We couldn't have just checked whether the _CLA StartsWith() [/Key], because you could have another Key like /KeyTwo

		End Function

		Protected Sub ExecuteActionsForSpecifiedCLAs()

			REM Ensure the Mandatory Keys have been specified, and execute the Actions on derivable <Value>s
			For Each _CLADatum As CLADatum In Me.CLAData

				If _CLADatum.MustBeSpecified Then _
				 If Not CLAsContainsKey(Me.ProvidedCLAs.ToArray(), _CLADatum.Key) _
				  Then Throw New DSCLANotSpecifiedException("The mandatory Command-Line Argument with Key """ & _CLADatum.Key & """ was not specified. Command-Line Arguments must be provided in the syntax  /Key  or  /Key:Value  or  /Key:""Value"" . NOTE: CLA Keys are not case-sensitive;  /Name  is the same as  /NAmE .")

				'Here, we know that *if* the _CLADatum was mandatory, then it has been specified
				'If it hasn't been specified, then we don't need to do anything about it
				If Not CLAsContainsKey(Me.ProvidedCLAs.ToArray(), _CLADatum.Key) Then Continue For

				'If we're here, then the CLA Has been specified, so ensure it's value is present if it needs one
				Dim _CorrospondingValue$ = Nothing

				If _CLADatum.MustHaveValue Then

					Dim _LambdaCopyOf_CLADatum As CLADatum = _CLADatum

					Dim _CLAWithMatchingKey_Resolver As Func(Of String, Boolean) = _
					   Function(_CLA$) _CLA.ToUpper().StartsWith((CLAKeyStartChar & _LambdaCopyOf_CLADatum.Key & CLAValueStartChar).ToUpper()) AndAlso (_CLA.Length > (CLAKeyStartChar & _LambdaCopyOf_CLADatum.Key & CLAValueStartChar).Length)

					'Ensure that there is a Value
					If Not (Me.ProvidedCLAs.Where(_CLAWithMatchingKey_Resolver).Count() = 1) Then _
					 Throw New DSCLAValueNotSpecifiedException("The Command-Line Argument with Key """ & _CLADatum.Key & """ declared that a corrosponding Value was mandatory, but no recognisable value was provided in the Syntax /Key:Value or /Key:""Value"".")

					'Extract the :Value or :"Value"
					_CorrospondingValue = New String(Me.ProvidedCLAs.First(_CLAWithMatchingKey_Resolver).ToCharArray().Skip(Me.ProvidedCLAs.First(_CLAWithMatchingKey_Resolver).ToCharArray().ToList().IndexOf(CLAValueStartChar) + 1).ToArray())

				End If

				REM Now run the Action, using the derived <Value>
				_CLADatum.ActionIfSpecified.Invoke(_CorrospondingValue)

			Next

		End Sub

		REM Writes the CLA Help Dictionary to the OutputDelegate if the /? CLA is specified
		Protected Sub DisplayHelpDictionary()

			Dim _CLAHelpDictionaryText$

			If Me.UseCommandLineFormatting_ForHelpDictionary Then

				REM CLA Help Dictionary Format: CommandLineFormatting=True

				'Description:
				'----------------------------------------
				'{Description}
				'
				'Examples:
				'----------------------------------------
				'{Examples}
				'
				'Argument Usage:
				'----------------------------------------
				'/RunFile:<Value>		The File to Execute (*.DS)
				'/Debug					(Optional) Shows Debugging Logging Messages. More text can go here, bu
				'						t it wraps around after 80 columns (in total)
				'/ShowLog				(Optional) Shows the Log during Interpretation

				_CLAHelpDictionaryText$ = _
				 "Description:" & vbCrLf & _
				 "-"c.Repeat(80) & vbCrLf & _
				 Me.CLAHelpDictionary_Description & vbCrLf & vbCrLf & _
				 "Examples:" & vbCrLf & _
				 "-"c.Repeat(80) & vbCrLf

				For Each _Example$ In Me.CLAHelpDictionary_Examples
					_CLAHelpDictionaryText &= _Example & vbCrLf
				Next

				_CLAHelpDictionaryText &= vbCrLf & _
				 "Argument Usage: (Keys are case-insensitive)" & vbCrLf & _
				 "-"c.Repeat(80) & vbCrLf

				For Each _CLADatum As CLADatum In Me.CLAData

					Const _KeyAndValuePart_TargetLength% = 30
					Const _DescriptionPart_MaxColumnWidth% = (80 - _KeyAndValuePart_TargetLength)

					'E.g. /RunFile:<Value>
					Dim _KeyAndValuePart$ = (CLAKeyStartChar & _CLADatum.Key) & If(_CLADatum.MustHaveValue, CLAValueStartChar & "<Value>", String.Empty)

					'E.g. (Optional) Shows the Log during Interpretation
					Dim _DescriptionPart$ = If(Not _CLADatum.MustBeSpecified, "(Optional) ", String.Empty) & _CLADatum.Description

					REM Start the Descriptions 30 Chars in, and put a {LineBreak} and {30 Spaces} every 50 chars.
					_CLAHelpDictionaryText &= _
					 _KeyAndValuePart.PadRight(_KeyAndValuePart_TargetLength, " "c) & _
					 _DescriptionPart.ToCharArray().Intersperse((vbCrLf & " "c.Repeat(_KeyAndValuePart_TargetLength)).ToCharArray(), _DescriptionPart_MaxColumnWidth) & _
					 vbCrLf

				Next

			Else

				REM CLA Help Dictionary Format: CommandLineFormatting=False

				'Description: {Description}
				'
				'Examples:
				'{Examples...}
				'
				'Argument Usage:
				'/RunFile:<Value> → The File to Execute (*.DS)
				'/Debug → (Optional) Shows Debugging Logging Messages. More text can go here, but it wraps around after 80 columns (in total)
				'/ShowLog → (Optional) Shows the Log during Interpretation

				_CLAHelpDictionaryText$ = _
				 "Description:" & vbCrLf & Me.CLAHelpDictionary_Description & vbCrLf & vbCrLf & _
				 "Examples:" & vbCrLf

				For Each _Example$ In Me.CLAHelpDictionary_Examples
					_CLAHelpDictionaryText &= _Example & vbCrLf
				Next

				_CLAHelpDictionaryText &= vbCrLf & _
				 "Argument Usage: (Keys are case-insensitive)" & vbCrLf

				For Each _CLADatum As CLADatum In Me.CLAData

					'E.g. /RunFile:<Value>
					Dim _KeyAndValuePart$ = (CLAKeyStartChar & _CLADatum.Key) & If(_CLADatum.MustHaveValue, CLAValueStartChar & "<Value>", String.Empty)

					'E.g. (Optional) Shows the Log during Interpretation
					Dim _DescriptionPart$ = If(Not _CLADatum.MustBeSpecified, "(Optional) ", String.Empty) & _CLADatum.Description

					_CLAHelpDictionaryText &= _KeyAndValuePart & " → " & _DescriptionPart & vbCrLf

				Next

			End If

			REM The OutputDelegate will usually be Console.WriteLine, to write the CLAHD to the std out stream.
			REM Sometimes though, e.g. in the case of a Windows Application, a MessageBox may be shown instead.
			Me.OutputDelegate.Invoke(_CLAHelpDictionaryText)

			Return

		End Sub

	End Class

End Namespace