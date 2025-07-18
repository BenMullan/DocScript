﻿Imports DocScript.Runtime.BuiltInFunction, DocScript.Language.Variables, DocScript.Language.Instructions, DocScript.Language.Instructions.Statements

Namespace Runtime

	Partial Public Class StandardBuiltInFunctions

		Private Const SystemBIFs_IdentifierStart_$ = "System_"

#Region "Individual BIF Member Declarations"

#Region "Data-retrival: System_Get* BIFs"

		Private Const BIF_System_GetTime_Identifier_$ = SystemBIFs_IdentifierStart_ & "GetTime"
		Private Shared ReadOnly BIF_System_GetTime_ As New BuiltInFunction(BIF_System_GetTime_Identifier_$, {}, GetType(DSString), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_System_GetTime_Identifier_$)
			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSString(Global.System.DateTime.Now.ToString("HH:mm:ss"))
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Returns the current Time on the System in the format HH:mm:ss"}

		Private Const BIF_System_GetHostname_Identifier_$ = SystemBIFs_IdentifierStart_ & "GetHostname"
		Private Shared ReadOnly BIF_System_GetHostname_ As New BuiltInFunction(BIF_System_GetHostname_Identifier_$, {}, GetType(DSString), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_System_GetHostname_Identifier_$)
			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSString(My.Computer.Name)
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Returns the Hostname of the current Computer"}

		Private Const BIF_System_GetUsername_Identifier_$ = SystemBIFs_IdentifierStart_ & "GetUsername"
		Private Shared ReadOnly BIF_System_GetUsername_ As New BuiltInFunction(BIF_System_GetUsername_Identifier_$, {}, GetType(DSString), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_System_GetUsername_Identifier_$)
			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSString(Environment.UserName)
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Returns the Username of the current Computer"}

		Private Const BIF_System_GetCurrentInterpreterDirectory_Identifier_$ = SystemBIFs_IdentifierStart_ & "GetCurrentInterpreterDirectory"
		Private Shared ReadOnly BIF_System_GetCurrentInterpreterDirectory_ As New BuiltInFunction(BIF_System_GetCurrentInterpreterDirectory_Identifier_$, {}, GetType(DSString), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_System_GetCurrentInterpreterDirectory_Identifier_$)
			 '_ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSString(UsefulMethods.GetExecutingAssemblyDirectory().FullName.WithBackslashOnEnd())
			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSString(UsefulMethods.GetExecutingAssemblyFilePath().Directory.FullName.WithBackslashOnEnd())
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Returns the directory in which the binary of the current Interpreter exists, with a Backslash on the end. Functionality altered 07072023."}

#End Region

		Private Const BIF_System_Beep_Identifier_$ = SystemBIFs_IdentifierStart_ & "Beep"
        Private Shared ReadOnly BIF_System_Beep_ As New BuiltInFunction(BIF_System_Beep_Identifier_$, {}, GetType(Void), New BuiltInFunctionDelegate(
         Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

             Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_System_Beep_Identifier_$)

             'Call (New System.Speech.Synthesis.SpeechSynthesizer()).Speak("beep")
             : Call (New System.Speech.Synthesis.SpeechSynthesizer()).Speak(New System.Speech.Synthesis.Prompt("<speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='en-US'><prosody pitch='high' rate='slow' volume='loud'>beep</prosody></speak>", Speech.Synthesis.SynthesisTextFormat.Ssml))
             'Global.System.Console.Beep(frequency:=440, duration:=600)
             'Global.Microsoft.VisualBasic.Interaction.Beep()

             Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

         End Function)
        ) With {.[Description] = "Sounds a tone through the computer's speaker"}

		Private Const BIF_System_PlayWav_Identifier_$ = SystemBIFs_IdentifierStart_ & "PlayWav"
        Private Shared ReadOnly BIF_System_PlayWav_ As New BuiltInFunction(BIF_System_PlayWav_Identifier_$, {New DSFunction.Parameter("_WavFilePath", GetType(DSString))}, GetType(Void), New BuiltInFunctionDelegate(
         Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

             Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_System_PlayWav_Identifier_$)
             My.Computer.Audio.Play(_Arguments.First().Coerce(Of DSString)().Value, AudioPlayMode.WaitToComplete)
             Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

         End Function)
        ) With {.[Description] = "Plays the _WavFilePath as a Wav File"}

        Private Const BIF_System_SayString_Identifier_$ = SystemBIFs_IdentifierStart_ & "SayString"
        Private Shared ReadOnly BIF_System_SayString_ As New BuiltInFunction(BIF_System_SayString_Identifier_$, {New DSFunction.Parameter("_StringToSpeak", GetType(DSString))}, GetType(Void), New BuiltInFunctionDelegate(
         Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

             Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_System_SayString_Identifier_$)
             Call (New Global.System.Speech.Synthesis.SpeechSynthesizer()).Speak(textToSpeak:=_Arguments.First().Coerce(Of DSString)().Value)
             Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

         End Function)
        ) With {.[Description] = "Speaks the _StringToSpeak using Windows (R) text-to-speech APIs"}

#Region "ShellExecute() and CreateProcess() BIFs"

		Private Const BIF_System_Run_Identifier_$ = SystemBIFs_IdentifierStart_ & "Run"
		Private Shared ReadOnly BIF_System_Run_ As New BuiltInFunction(BIF_System_Run_Identifier_$, {New DSFunction.Parameter("_Command", GetType(DSString))}, GetType(Void), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_System_Run_Identifier_$)
			 Global.Microsoft.VisualBasic.Interaction.Shell(_Arguments.First().Coerce(Of DSString)().Value, AppWinStyle.NormalFocus, False, -1)
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Executes the _Command in the Shell"}

		Private Const BIF_System_RunAndGetOutput_Identifier_$ = SystemBIFs_IdentifierStart_ & "RunAndGetOutput"
		Private Shared ReadOnly BIF_System_RunAndGetOutput_ As New BuiltInFunction(BIF_System_RunAndGetOutput_Identifier_$, {New DSFunction.Parameter("_ExecutablePath", GetType(DSString)), New DSFunction.Parameter("_CLArguments", GetType(DSString))}, GetType(DSString), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_System_RunAndGetOutput_Identifier_$)

			 Dim _ProcessOutput$ = UsefulMethods.RunProcess_AndReturnCommandLineOutput(
			   New IO.FileInfo(_Arguments.ElementAt(0).Coerce(Of DSString)().Value),
			   _Process_CLAs:=_Arguments.ElementAt(1).Coerce(Of DSString)().Value
			 )

			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSString(_ProcessOutput)

			 Return (_ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls))

		 End Function)
		) With {.[Description] = "Runs the program at _ExecutablePath with the _CLArguments, waits for it to finish, and then returns the StdOut and StdErr text."}

		Private Const BIF_System_RunInline_Identifier_$ = SystemBIFs_IdentifierStart_ & "RunInline"
		Private Shared ReadOnly BIF_System_RunInline_ As New BuiltInFunction(BIF_System_RunInline_Identifier_$, {New DSFunction.Parameter("_ExecutablePath", GetType(DSString))}, GetType(Void), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_System_RunInline_Identifier_$)

			 Dim _NewInlineProcess As New Process() With {.StartInfo = (New ProcessStartInfo(_Arguments.First().Coerce(Of DSString)().Value) With {.UseShellExecute = False})}
			 _NewInlineProcess.Start() : _NewInlineProcess.WaitForExit()

			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Executes the _ExecutablePath in the Shell, WITHOUT ShellExecute(). Arguments for the executable are NOT permitted here."}

		Private Const BIF_System_RunInlineWithArguments_Identifier_$ = SystemBIFs_IdentifierStart_ & "RunInlineWithArguments"
		Private Shared ReadOnly BIF_System_RunInlineWithArguments_ As New BuiltInFunction(BIF_System_RunInlineWithArguments_Identifier_$, {New DSFunction.Parameter("_ExecutablePath", GetType(DSString)), New DSFunction.Parameter("_CLArguments", GetType(DSString))}, GetType(Void), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_System_RunInlineWithArguments_Identifier_$)

			 Dim _NewInlineProcess As New Process() With {
			   .StartInfo = (New ProcessStartInfo(fileName:=_Arguments.ElementAt(0).Coerce(Of DSString)().Value, arguments:=_Arguments.ElementAt(1).Coerce(Of DSString)().Value) With {.UseShellExecute = False})
			 }
			 _NewInlineProcess.Start() : _NewInlineProcess.WaitForExit()

			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Executes the _ExecutablePath in the Shell, WITHOUT ShellExecute(). Arguments for the executable are specified in _CLArguments."}

		Private Const BIF_System_OpenFile_Identifier_$ = SystemBIFs_IdentifierStart_ & "OpenFile"
		Private Shared ReadOnly BIF_System_OpenFile_ As New BuiltInFunction(BIF_System_OpenFile_Identifier_$, {New DSFunction.Parameter("_FilePath", GetType(DSString))}, GetType(Void), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_System_OpenFile_Identifier_$)
			 Global.System.Diagnostics.Process.Start(_Arguments.First().Coerce(Of DSString)().Value)
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Launches the _FilePath via the Shell"}

#End Region

#Region "Network-related"

		Private Const BIF_System_Curl_Identifier_$ = SystemBIFs_IdentifierStart_ & "Curl"
		Private Shared ReadOnly BIF_System_Curl_ As New BuiltInFunction(BIF_System_Curl_Identifier_$, {New DSFunction.Parameter("_URL", GetType(DSString))}, GetType(DSString), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_System_Curl_Identifier_$)
			 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSString(
			 (New Net.WebClient()).DownloadString(_Arguments.First().Coerce(Of DSString)().Value)
			 )
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Downloads and returns the plain-text content at _URL"}

		Private Const BIF_System_Ping_Identifier_$ = SystemBIFs_IdentifierStart_ & "Ping"
        Private Shared ReadOnly BIF_System_Ping_ As New BuiltInFunction(BIF_System_Ping_Identifier_$, {New DSFunction.Parameter("_HostnameOrIP", GetType(DSString))}, GetType(DSBoolean), New BuiltInFunctionDelegate(
         Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

             Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_System_Ping_Identifier_$)
             _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSBoolean(
                My.Computer.Network.Ping(_Arguments.First().Coerce(Of DSString)().Value)
             )
             Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

         End Function)
        ) With {.[Description] = "Attempts to ping the _HostnameOrIP. Returns True if this was successful, and otherwise, False."}

		Private Const BIF_System_RegisterUNCCredentials_Identifier_$ = SystemBIFs_IdentifierStart_ & "RegisterUNCCredentials"
		Private Shared ReadOnly BIF_System_RegisterUNCCredentials_ As New BuiltInFunction(BIF_System_RegisterUNCCredentials_Identifier_$, {New DSFunction.Parameter("_UNCPath", GetType(DSString)), New DSFunction.Parameter("_Username", GetType(DSString)), New DSFunction.Parameter("_Password", GetType(DSString))}, GetType(Void), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_System_RegisterUNCCredentials_Identifier_$)

			 DocScript.Runtime.Remoting.AuthenticateWithNetworkFolder(
			   _UNCPath:=_Arguments.ElementAt(0).Coerce(Of DSString)().Value,
			   _Username:=_Arguments.ElementAt(1).Coerce(Of DSString)().Value,
			   _Password:=_Arguments.ElementAt(2).Coerce(Of DSString)().Value
			)

			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Associates the _Username and _Password as credentials for the _UNCPath, such as ""\\DC1\NetLogon\""."}

#Region "Web Server Methods"

		Private Const BIF_System_HostWebServerForString_Identifier_$ = SystemBIFs_IdentifierStart_ & "HostWebServerForString"
		Private Shared ReadOnly BIF_System_HostWebServerForString_ As New BuiltInFunction(BIF_System_HostWebServerForString_Identifier_$, {New DSFunction.Parameter("_Port", GetType(DSNumber)), New DSFunction.Parameter("_MIMEType", GetType(DSString)), New DSFunction.Parameter("_ResponseString", GetType(DSString))}, GetType(Void), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_System_HostWebServerForString_Identifier_$)

			 Dim _MicroServer As New DocScript.Utilities.MicroHTTPServer(
			   _Port:=Convert.ToUInt16(_Arguments.ElementAt(0).Coerce(Of DSNumber)().Value),
			   _ResponseContentString:=_Arguments.ElementAt(2).Coerce(Of DSString)().Value,
			   _ResponseMIMEType:=_Arguments.ElementAt(1).Coerce(Of DSString)().Value
			 )
			 AddHandler _MicroServer.RequestHandledSuccessfully, Sub(_Request As System.Net.HttpListenerRequest) LogSystemMessage("Handled a DS-WebServer Request for URL: " & _Request.Url.ToString(), LogEvent.DSEventSeverity.Infomation)
			 _MicroServer.Run()

			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Runs a lightweight Web Server, serving the given _ResponseString for all requests, listening on the URL http://localhost:{_Port}/"}

		Private Const BIF_System_HostWebServerForDirectory_Identifier_$ = SystemBIFs_IdentifierStart_ & "HostWebServerForDirectory"
        Private Shared ReadOnly BIF_System_HostWebServerForDirectory_ As New BuiltInFunction(BIF_System_HostWebServerForDirectory_Identifier_$, {New DSFunction.Parameter("_Port", GetType(DSNumber)), New DSFunction.Parameter("_DirectoryPath", GetType(DSString))}, GetType(Void), New BuiltInFunctionDelegate(
         Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

             Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_System_HostWebServerForDirectory_Identifier_$)

             Dim _MicroServer As New DocScript.Utilities.MicroHTTPServer(
                _Port:=Convert.ToUInt16(_Arguments.ElementAt(0).Coerce(Of DSNumber)().Value),
                _DirectoryToServeFilesFrom:=New IO.DirectoryInfo(_Arguments.ElementAt(1).Coerce(Of DSString)().Value)
             )
             AddHandler _MicroServer.RequestHandledSuccessfully, Sub(_Request As System.Net.HttpListenerRequest) LogSystemMessage("Handled a DS-WebServer Request for URL: " & _Request.Url.ToString(), LogEvent.DSEventSeverity.Infomation)
             _MicroServer.Run()

             Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

         End Function)
        ) With {.[Description] = "Runs a lightweight Web Server, serving files in the _DirectoryPath, depending on the URL, listening on the URL http://localhost:{_Port}/"}

#End Region

#End Region

#End Region

		''' <summary>System-related Functions such as getting the current Time or executing a Shell Command</summary>
        Public Shared ReadOnly System As DocScript.Runtime.BuiltInFunction() = {
         StandardBuiltInFunctions.BIF_System_GetTime_,
         StandardBuiltInFunctions.BIF_System_GetHostname_,
         StandardBuiltInFunctions.BIF_System_GetUsername_,
         StandardBuiltInFunctions.BIF_System_GetCurrentInterpreterDirectory_,
         StandardBuiltInFunctions.BIF_System_Beep_,
         StandardBuiltInFunctions.BIF_System_PlayWav_,
         StandardBuiltInFunctions.BIF_System_SayString_,
         StandardBuiltInFunctions.BIF_System_Run_,
         StandardBuiltInFunctions.BIF_System_RunInline_,
         StandardBuiltInFunctions.BIF_System_RunInlineWithArguments_,
         StandardBuiltInFunctions.BIF_System_RunAndGetOutput_,
         StandardBuiltInFunctions.BIF_System_OpenFile_,
         StandardBuiltInFunctions.BIF_System_Curl_,
         StandardBuiltInFunctions.BIF_System_Ping_,
         StandardBuiltInFunctions.BIF_System_HostWebServerForString_,
         StandardBuiltInFunctions.BIF_System_HostWebServerForDirectory_,
         StandardBuiltInFunctions.BIF_System_RegisterUNCCredentials_
        }

	End Class

End Namespace