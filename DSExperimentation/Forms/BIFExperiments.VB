Imports DocScript.Runtime.BuiltInFunction
#Const PassExceptionsOn = False

Public Class BIFExperiments

	Private Sub InputTestButton_Click() Handles InputTestButton.Click
#If PassExceptionsOn Then
		Try
#End If
			DocScript.Logging.ProcessDebugEvents = False
			DocScript.Logging.LogUtilities.CurrentLogEventHandler = DocScript.Logging.BuiltInLogEventHandlers.CLIDefault

		DocScript.Runtime.ExecutionContext.GUIDefault.BuiltInFunctions _
		 .First(Function(_BIF As DocScript.Runtime.BuiltInFunction) _BIF.Identifier.ToUpper() = DocScript.Runtime.Constants.InputFunction_Identifier.ToUpper()) _
		 .Run(
		   _InputSymbolTablesState:=Nothing,
		   _Arguments:={New DocScript.Language.Variables.DSString("Enter your Name:")}
		  ).ReturnStatus.BuiltInFunction_ReturnValue.MustBe(Of DSString).Value _
		 .Output(AddressOf MsgBox)


		'(DocScript.Runtime.SymbolTablesSnapshot.Empty)

#If PassExceptionsOn Then
		Catch _Ex As Exception When True
			MsgBox("Exception:" & vbCrLf & vbCrLf & _Ex.Message, MsgBoxStyle.Critical, _Ex.GetType().Name)
		End Try
#End If
	End Sub

	Private Sub Button1_Click() Handles Button1.Click

		DocScript.Logging.ProcessDebugEvents = True
		DocScript.Logging.LogUtilities.CurrentLogEventHandler = DocScript.Logging.BuiltInLogEventHandlers.CLIDefault

		Dim _LambdaBIF As BuiltInFunction = (
		 Function(_Identifier$) As BuiltInFunction
			 Return New BuiltInFunction(
			_Identifier, {New DSFunction.Parameter("L_Input", GetType(DSNumber))}, GetType(DSNumber), New BuiltInFunctionDelegate(
			 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue())

				 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer(_ExecuteeDescription:="BIF\" & _Identifier)
				 _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSNumber(Math.Round(_Arguments.First().MustBe(Of DSNumber)().Value))
				 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

			 End Function
			 )
			 ) With {.[Description] = "LRounds the _Input to the nearest Integeral value"}
		 End Function
		).Invoke("LMaths_Round")




		REM ↓ Without Lambda-Expr Initialisation



		MsgBox("_LambdaBIF.Identifier Is Nothing=" & (_LambdaBIF.Identifier Is Nothing).ToString())
		MsgBox("_LambdaBIF.Identifier = Nothing=" & (_LambdaBIF.Identifier = Nothing).ToString())
		MsgBox("_LambdaBIF.Identifier=" & _LambdaBIF.Identifier)




		Dim _LambdaBIF2 As New BuiltInFunction(
		 _Identifier:="TMaths_Round",
		 _ExpectedParameters:={New DSFunction.Parameter("T_Input", GetType(DSNumber))},
		 _ReturnType:=GetType(DSNumber),
		 _Action:=New BuiltInFunctionDelegate(
		  Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue())

			  Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer(_ExecuteeDescription:="BIF\" & "TMaths_Round")
			  _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = New DSNumber(Math.Round(_Arguments.First().MustBe(Of DSNumber)().Value))
			  Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		  End Function
		  )
		) With {.[Description] = "TRounds the _Input to the nearest Integer"}


		MsgBox("_LambdaBIF2.Identifier Is Nothing=" & (_LambdaBIF2.Identifier Is Nothing).ToString())
		MsgBox("_LambdaBIF2.Identifier = Nothing=" & (_LambdaBIF2.Identifier = Nothing).ToString())
		MsgBox("_LambdaBIF2.Identifier=" & _LambdaBIF2.Identifier)


	End Sub

	Private Sub Button2_Click() Handles Button2.Click
		MsgBox("New_AndStartExecutionTimer() Returns Nothing: " & (Language.Instructions.ExecutionResult.New_AndStartExecutionTimer(_ExecuteeDescription:="BIF\" & DocScript.Runtime.Constants.InputFunction_Identifier) Is Nothing).ToString())
	End Sub

	Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
		DocScript.Logging.LogUtilities.CurrentLogEventHandler = DocScript.Logging.BuiltInLogEventHandlers.WindowsEventLog("Experimentation")
		MsgDebug("DocScript.Logging.LogUtilities.CurrentLogEventHandler = NOTHING: " & (DocScript.Logging.LogUtilities.CurrentLogEventHandler = Nothing).ToString())
		LogSystemMessage(InputBox("(Warning) Message"), LogEvent.DSEventSeverity.Warning)
	End Sub

End Class