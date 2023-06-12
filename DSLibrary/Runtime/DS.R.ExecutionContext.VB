Namespace Runtime

	''' <summary>Contains contextual parameters for a DocScript Program</summary>
	Public Class ExecutionContext

#Region "ExeCxt \ Propities"

		'''' <summary>Should be unique for each type of ExecutionContext (e.g. a GUI one vs a CLI one). Used when comparing several delegates.</summary>
		Public ReadOnly ID As String

		REM Can be set automatically via System.IO.Path.GetDirectoryName(Application.ExecutablePath)
		Public ReadOnly RootFolder As IO.DirectoryInfo

		''' <summary>
		''' This is a conveinient layer of abstraction for the creater of the ExecutionContext, who'll want to deal in Strings.
		''' The InputDelegate will be wrapped-up into the Input() Function which actually appears on the SymTbl.
		''' This is to say: InputDelegate IS DIFFERENT FROM the Input() BuiltInFunction
		''' </summary>
		Public ReadOnly InputDelegate As Func(Of [String], [String])

		REM Similarly, OutputDelegate is an abstraction of Output()
		Public ReadOnly OutputDelegate As Action(Of [String])

		''' <summary>
		''' By default, this is set to Nothing, and the ExeCxt will use the OutputDelegate as the OutputErrorDelegate.
		''' For the CLIDefault, this writes to STDERR in Red.
		''' For the GUIDefault, this shows a MsgBox with the Error Icon.
		''' </summary>
		Public OutputErrorDelegate As Action(Of [String]) = Nothing

		REM These will be injected into the Global SymbolTable at Runtime
		Protected ReadOnly BuiltInFunctions_WithoutIOBIFs_ As BuiltInFunction()

		''' <summary>When retreived, this Property Concatenates the IO BIFs in.</summary>
		Public ReadOnly Property BuiltInFunctions As ObjectModel.ReadOnlyCollection(Of DocScript.Runtime.BuiltInFunction)
			Get

				REM We need to return the provided BIFs, along with Input() and Output() too.
				Return New ObjectModel.ReadOnlyCollection(Of DocScript.Runtime.BuiltInFunction)(
				  Me.BuiltInFunctions_WithoutIOBIFs_.Concat({
				  Me.InputBIF_(Me.InputDelegate),
				  Me.OutputBIF_(Me.OutputDelegate),
				  Me.OutputErrorBIF_
				 }).ToList()
				)

			End Get
		End Property

#End Region

		''' <summary>Note that the OutputError() Delegate should be set with an inline With Statement, as it is not taken in as a Constructor Argument</summary>
		Public Sub New(ByVal _ID$, ByVal _RootFolder As IO.DirectoryInfo, ByVal _InputDelegate As Func(Of [String], [String]), ByVal _OutputDelegate As Action(Of [String]), ByVal _BuiltInFunctions As BuiltInFunction())

			LogExecutionMessage(String.Format("Constructing an ExecutionContext with {0} BuitInFunctions and a RootFoler of {1}", _BuiltInFunctions.Length.ToString(), _RootFolder.FullName.InSquares()))

			REM Ensure that all Identifiers for the BuiltInFunctions are unique (accounting for the fact that the I/O Delegates aren't included in _BuiltInFunctions...)
			If Not _BuiltInFunctions.Select(Of [String])(Function(_BIF As Runtime.BuiltInFunction) _BIF.Identifier.ToUpper()) _
			 .Concat({Runtime.Constants.InputFunction_Identifier.ToUpper(), Runtime.Constants.OutputFunction_Identifier.ToUpper(), Runtime.Constants.OutputErrorFunction_Identifier.ToUpper()}) _
			 .ToArray().AllElementsAreUnique() Then Throw New DSValidationException("ExeCxt: Each BuiltInFunction must have a unique Identifier", "The BIF Identifiers were: " & _BuiltInFunctions.Select(Of [String])(Function(_BIF As Runtime.BuiltInFunction) _BIF.Identifier.ToUpper()).Concat({Runtime.Constants.InputFunction_Identifier.ToUpper(), Runtime.Constants.OutputFunction_Identifier.ToUpper(), Runtime.Constants.OutputErrorFunction_Identifier.ToUpper()}).ToArray().GetStandardArraySerialisation())

			Me.ID = _ID
			Me.RootFolder = _RootFolder
			Me.InputDelegate = _InputDelegate : Me.OutputDelegate = _OutputDelegate
			Me.BuiltInFunctions_WithoutIOBIFs_ = _BuiltInFunctions

		End Sub

		''' <summary>Wraps the InputDelegate in a DocScript BIF</summary>
		Protected ReadOnly Property InputBIF_(ByVal _InputDelegate As Func(Of String, String)) As BuiltInFunction
			Get
				Try

					Dim _BIFAction As BuiltInFunction.BuiltInFunctionDelegate = _
					 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As DocScript.Language.Variables.IDataValue()) As Language.Instructions.ExecutionResult

						 Dim _ExeRes As Language.Instructions.ExecutionResult = Language.Instructions.ExecutionResult.New_AndStartExecutionTimer(_ExecuteeDescription:="BIF\" & Runtime.Constants.InputFunction_Identifier)

						 REM Now we actually Invoke() the InputDelegate and store the User's Response
						 Dim _UserResponse$ = _InputDelegate.Invoke(_Arguments.First().Coerce(Of Language.Variables.DSString)().Value)
						 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = (New Language.Variables.DSString(_UserResponse))

						 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

					 End Function

					Return New BuiltInFunction(
					 _Identifier:=Runtime.Constants.InputFunction_Identifier,
					 _ExpectedParameters:={New Language.Instructions.Statements.DSFunction.Parameter("_Prompt", GetType(Language.Variables.DSString))},
					 _ReturnType:=GetType(Language.Variables.DSString),
					 _Action:=_BIFAction
					) With {
					 .Description = "Requests Input from the User, showing the _Prompt, and returning the resultant Response."
					}

				Catch _Ex As Exception : Throw New DSException("@ExecutionContext\GetInputBIF_: " & _Ex.Message, _Ex) : End Try
			End Get
		End Property

		''' <summary>Wraps the OutputDelegate in a DocScript BIF</summary>
		Protected ReadOnly Property OutputBIF_(ByVal _OutputDelegate As Action(Of [String])) As BuiltInFunction
			Get
				Try

					Dim _BIFAction As BuiltInFunction.BuiltInFunctionDelegate = _
					 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As DocScript.Language.Variables.IDataValue()) As Language.Instructions.ExecutionResult

						 Dim _ExeRes As Language.Instructions.ExecutionResult = Language.Instructions.ExecutionResult.New_AndStartExecutionTimer(_ExecuteeDescription:=Runtime.Constants.OutputFunction_Identifier)

						 REM Now we actually Invoke() the OutputDelegate (which displays the Message to the User)
						 _OutputDelegate.Invoke(_Arguments.First().Coerce(Of Language.Variables.DSString)().Value)

						 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

					 End Function

					Return New BuiltInFunction(
					 _Identifier:=Runtime.Constants.OutputFunction_Identifier,
					 _ExpectedParameters:={New Language.Instructions.Statements.DSFunction.Parameter("_Message", GetType(Language.Variables.DSString))},
					 _ReturnType:=GetType(Language.Variables.Void),
					 _Action:=_BIFAction
					) With {
					 .Description = "Displays the _Message to the user."
					}

				Catch _Ex As Exception : Throw New DSException("@ExecutionContext\GetOutputBIF_: " & _Ex.Message, _Ex) : End Try
			End Get
		End Property

		''' <summary>Wraps the OutputErrorDelegate in a DocScript BIF if is ISNOT NOTHING. If it IS NOTHING, then the OutputDelegate is used.</summary>
		Protected ReadOnly Property OutputErrorBIF_ As BuiltInFunction
			Get
				Try

					If Me.OutputErrorDelegate Is Nothing Then : Return Me.OutputBIF_(Me.OutputDelegate)
					Else

						Dim _BIFAction As BuiltInFunction.BuiltInFunctionDelegate = _
						 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As DocScript.Language.Variables.IDataValue()) As Language.Instructions.ExecutionResult

							 Dim _ExeRes As Language.Instructions.ExecutionResult = Language.Instructions.ExecutionResult.New_AndStartExecutionTimer(_ExecuteeDescription:=Runtime.Constants.OutputErrorFunction_Identifier)

							 REM Now we actually Invoke() the OutputDelegate (which displays the Message to the User)
							 Me.OutputErrorDelegate.Invoke(_Arguments.First().Coerce(Of Language.Variables.DSString)().Value)

							 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

						 End Function

						Return New BuiltInFunction(
						 _Identifier:=Runtime.Constants.OutputErrorFunction_Identifier,
						 _ExpectedParameters:={New Language.Instructions.Statements.DSFunction.Parameter("_Message", GetType(Language.Variables.DSString))},
						 _ReturnType:=GetType(Language.Variables.Void),
						 _Action:=_BIFAction
						) With {
						 .Description = "For the CLI, writes the _Message to the Standard Error Output (stderr), and for GUI, shows a MsgBox with the Error Icon." & vbCrLf & "If the ExecutionContext.OutputErrorDelegate Is Nothing, then this simply piggy-backs off of the Output() function."
						}

					End If

				Catch _Ex As Exception : Throw New DSException("@ExecutionContext\OutputErrorBIF_: " & _Ex.Message, _Ex) : End Try
			End Get
		End Property

		''' <summary>Uses Console.Write()&amp;Console.ReadLine() and Console.WriteLine()</summary>
		Public Shared ReadOnly Property CLIDefault As ExecutionContext
			Get

				Static _CLIDefault_InputDelegate_ As Func(Of String, String) = _
				 Function(_Prompt$) As String
					 Console.Write(_Prompt) : Return Console.ReadLine()
				 End Function

				Static _CLIDefault_OutputDelegate_ As Action(Of String) = _
				 AddressOf Console.WriteLine

				Return New ExecutionContext(
				 _ID:="DS_BuiltInExeCxt_CLIDefault",
				 _RootFolder:=New IO.DirectoryInfo(IO.Directory.GetCurrentDirectory()),
				 _InputDelegate:=_CLIDefault_InputDelegate_,
				 _OutputDelegate:=_CLIDefault_OutputDelegate_,
				 _BuiltInFunctions:=ExecutionContext.AllStandardBuiltInFunctions
				) With {
				 .OutputErrorDelegate = Sub(_Message$) UsefulMethods.ConsoleErrorWriteLineInColour(_Message, ConsoleColor.Red)
				}

			End Get
		End Property

		''' <summary>Uses InputBox(), and MessageBox.Show() with the ServiceNotification flag. This means that even if the DocScript Interpreter process is running on a different session to that of the GUI Console session, a MessageBox will still be shown on the console.</summary>
		Public Shared ReadOnly Property GUIDefault As ExecutionContext
			Get

				Static _GUIDefault_InputDelegate_ As Func(Of [String], [String]) = _
				 Function(_Prompt$) Global.Microsoft.VisualBasic.Interaction.InputBox(_Prompt, "DocScript Input")

				Static _GUIDefault_OutputDelegate_ As Action(Of [String]) = _
				 Sub(_Message$) Global.System.Windows.Forms.MessageBox.Show(_Message, "DocScript Output", Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Information, Windows.Forms.MessageBoxDefaultButton.Button1, Windows.Forms.MessageBoxOptions.ServiceNotification, displayHelpButton:=False)
				'Sub(_Message$) Global.Microsoft.VisualBasic.MsgBox(_Message, MsgBoxStyle.Information, "DocScript: Output")

				Return New ExecutionContext(
				 _ID:="DS_BuiltInExeCxt_GUIDefault",
				 _RootFolder:=New IO.DirectoryInfo(IO.Directory.GetCurrentDirectory()),
				 _InputDelegate:=_GUIDefault_InputDelegate_,
				 _OutputDelegate:=_GUIDefault_OutputDelegate_,
				 _BuiltInFunctions:=ExecutionContext.AllStandardBuiltInFunctions
				) With {
				 .OutputErrorDelegate = Sub(_Message$) Global.System.Windows.Forms.MessageBox.Show(_Message, "DocScript Error Output", Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error, Windows.Forms.MessageBoxDefaultButton.Button1, Windows.Forms.MessageBoxOptions.ServiceNotification, displayHelpButton:=False)
				}

			End Get
		End Property

		''' <summary>Concatenates together all the Standard BuiltInFunctions into a single Array</summary>
		Public Shared ReadOnly Property AllStandardBuiltInFunctions As BuiltInFunction()
			Get

				Return DocScript.Runtime.StandardBuiltInFunctions.[Maths] _
				  .Concat(DocScript.Runtime.StandardBuiltInFunctions.[System]) _
				  .Concat(DocScript.Runtime.StandardBuiltInFunctions.[File]) _
				  .Concat(DocScript.Runtime.StandardBuiltInFunctions.[DS]) _
				  .Concat(DocScript.Runtime.StandardBuiltInFunctions.[Const]) _
				  .Concat(DocScript.Runtime.StandardBuiltInFunctions.[Debug]) _
				  .ToArray()

			End Get
		End Property

		Public Shared Operator <>(ByVal _A As ExecutionContext, ByVal _B As ExecutionContext) As Boolean
			Return Not (_A = _B)
		End Operator

		Public Shared Operator =(ByVal _A As ExecutionContext, ByVal _B As ExecutionContext) As Boolean

			Return (_A.ID = _B.ID)

			'Return (
			'	(_A.RootFolder.FullName = _B.RootFolder.FullName) AndAlso
			'	(_A.InputDelegate = _B.InputDelegate) AndAlso
			'	(_A.OutputDelegate = _B.OutputDelegate) AndAlso
			'	(_A.BuiltInFunctions_WithoutIOBIFs_.Length = _B.BuiltInFunctions_WithoutIOBIFs_.Length)
			')

		End Operator

		''' <summary>Produces a DS-KVP Human-readable String for the Object</summary>
		Public Overrides Function ToString() As String
			Return DocScript.Utilities.KVPSerialisation.GetKVPString({"BIFCount", Me.BuiltInFunctions.Count.ToString()}, {"RootFolder", Me.RootFolder.FullName})
		End Function

	End Class

End Namespace