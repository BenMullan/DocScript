Namespace WebParts

	Partial Public NotInheritable Class API

		Public Shared Sub GetASPX(ByRef _Request As HttpRequest, ByRef _Response As HttpResponse)

			DocScript.WebParts.InitialiseWebExecutionEnvironment(_IncommingURL:=_Request.Url.AbsoluteUri)

			Try : _Request.EnsureTheseQueryStringsAreSpecified(_Response, "Item")

				Select Case _Request.QueryString("Item").ToUpper()

					Case "Debug".ToUpper() 'http://localhost:400/API/Get.ASPX?Item=Debug

						'DatabaseInteraction.AddUploadedProgram(InputBox("ProgramName"), "This is the Program Source")
						'APIResponse.FromValidRequest().Send(_Response)
						APIResponse.FromValidRequest({}, DatabaseInteraction.GetExecutionSesionInputEvents("2SAYHE_KJU")).Send(_Response)

						'Dim _DocScriptDatabaseName$ = System.Configuration.ConfigurationManager.AppSettings.Item("DocScriptDatabaseName")
						'APIResponse.FromValidRequest({"Config-DocScriptDatabaseName", _DocScriptDatabaseName}).Send(_Response)

						'Dim _Configuration As System.Configuration.Configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~")
						'_Configuration.AppSettings.Settings.Add("DocScriptDatabaseName", "TheValue")
						'_Configuration.AppSettings.Settings!DocScriptDatabaseName.Value = "DSInteractive0"
						'APIResponse.FromValidRequest().Send(_Response)

						'APIResponse.FromValidRequest(Get2DArray({"Tes1Key0", "0TestValue"}), {<TestChild UserDomainName=<%= Environment.UserDomainName %>>Who Am I?</TestChild>}).Send(_Response)
						'MsgBox("After the Response is alledgedly sent!")

						'Dim HoldData As String = "DefaultHoldData"

						'If _Request.QueryStringIsSpecifiedAndHasValue("HoldData") Then
						'	'Set
						'	HoldData = _Request.QueryString("HoldData")
						'Else
						'	'Get
						'End If

					Case "UploadedProgramsWithoutSource".ToUpper()	'http://localhost:400/API/Get.ASPX?Item=UploadedProgramsWithoutSource

						REM QueryStrings:	[]
						REM Returns:		[<UploadedPrograms>]

						Try

							'Each <UploadedProgram> has @ProgramName and @TimeUploaded
							APIResponse.FromValidRequest({}, {DatabaseInteraction.GetUploadedProgramsWithoutSource().WrapIn("UploadedPrograms")}).Send(_Response)

						Catch _Ex As Exception : Throw New DSWebException("@Item=UploadedProgramsWithoutSource", _Ex) : End Try

					Case "ProgramSource".ToUpper() 'http://localhost:400/API/Get.ASPX?Item=ProgramSource&ProgramName=HelloWorld.DS

						REM QueryStrings:	[ProgramName]
						REM Returns:		(RAW: The Program's Source)

						Try : _Request.EnsureTheseQueryStringsAreSpecified(_Response, "ProgramName")

							_Response.ContentType = "text/plain"
							_Response.ClearAndWriteAndEnd(DatabaseInteraction.GetUploadedProgramSource(_Request.QueryString("ProgramName")))

						Catch _Ex As Exception : Throw New DSWebException("@Item=ProgramSource", _Ex) : End Try

					Case "DeleteProgram".ToUpper() 'http://localhost:400/API/Get.ASPX?Item=DeleteProgram&ProgramName=HelloWorld.DS

						REM QueryStrings:	[ProgramName]
						REM Returns:		

						Try : _Request.EnsureTheseQueryStringsAreSpecified(_Response, "ProgramName")

							DatabaseInteraction.DeleteUploadedProgram(_Request.QueryString("ProgramName"))
							APIResponse.FromValidRequest().Send(_Response)

						Catch _Ex As Exception : Throw New DSWebException("@Item=DeleteProgram", _Ex) : End Try

					Case "ExprTreeFromSourceExpr".ToUpper()	'http://localhost:400/API/Get.ASPX?Item=ExprTreeFromSourceExpr&SourceExpr=4^5-F

						REM QueryStrings:	[SourceExpr]
						REM Returns:		[ExprTree]

						Try : _Request.EnsureTheseQueryStringsAreSpecified(_Response, "SourceExpr")

							Dim _ExprTree As XElement = DocScript.Language.Expressions.ExprUtilities.ConstructExpressionFromTokens( _
							 DocScript.Runtime.Parser.GetTokensFromSource(_Request.QueryString("SourceExpr"))
							).GetExprTreeNodeXML()

							APIResponse.FromValidRequest({}, {_ExprTree.WrapIn("ExprTreeRoot")}).Send(_Response)

						Catch _Ex As Exception : Throw New DSWebException("@Item=ExprTreeFromSourceExpr", _Ex) : End Try

					Case "ProgramTreeFromSource".ToUpper()	'http://localhost:400/API/Get.ASPX?Item=ProgramTreeFromSource

						REM QueryStrings:	
						REM PostData:		[Source]
						REM Returns:		[ProgramTree]

						Try : _Request.EnsureThesePostDataKeysAreSpecified(_Response, "Source")

							Dim _ProgramTree As XElement = Global.DocScript.Runtime.Program.FromSource(_Request.Form("Source"), Nothing).ProgramTreeXML
							APIResponse.FromValidRequest({}, {_ProgramTree}).Send(_Response)

						Catch _Ex As Exception : Throw New DSWebException("@Item=ProgramTreeFromSource", _Ex) : End Try

					Case Else
						REM An unmatched ?Item was specified...
						APIResponse.FromInvalidRequest("An unrecognised ?Item was specified.").Send(_Response)
				End Select

				'If no response has been sent, then the request hasn't been dealt with properly (e.g. a case didn't APIResponse...Send() anything)
				Throw New DSException("The API-EndPoint did not respond with any data")

			Catch _Ex As Exception
				APIResponse.FromInternalServerError("@GetASPX: " & _Ex.GetType().Name.InSquares() & " "c & _Ex.Message).Send(_Response)
			End Try

		End Sub

	End Class

End Namespace