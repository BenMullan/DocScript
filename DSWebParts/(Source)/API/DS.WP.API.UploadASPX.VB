Namespace WebParts

	Partial Public NotInheritable Class API

		Public Shared Sub UploadASPX(ByRef _Request As HttpRequest, ByRef _Response As HttpResponse)

			DocScript.WebParts.InitialiseWebExecutionEnvironment(_IncommingURL:=_Request.Url.AbsoluteUri)

			Try : _Request.EnsureTheseQueryStringsAreSpecified(_Response, "Item")

				Select Case _Request.QueryString("Item").ToUpper()

					Case "Program".ToUpper()	'http://localhost:400/API/Upload.ASPX?Item=Program&ProgramName=HelloWorld.DS

						REM QueryStrings:	[ProgramName]
						REM PostKeys:		[SourceText]
						REM Returns:		[ProgramSavedAsName]

						Try : _Request.EnsureTheseQueryStringsAreSpecified(_Response, "ProgramName") : _Request.EnsureThesePostDataKeysAreSpecified(_Response, "SourceText")

							Dim _ProgramSavedAsName$ = DocScript.WebParts.DatabaseInteraction.AddUploadedProgram(_Request.QueryString("ProgramName"), _Request.Form("SourceText"))
							APIResponse.FromValidRequest({"ProgramSavedAsName", _ProgramSavedAsName}).Send(_Response)

						Catch _Ex As Exception : Throw New DSWebException("@Item=Program", _Ex) : End Try

					Case Else
						REM An unmatched ?Item was specified...
						APIResponse.FromInvalidRequest("An unrecognised ?Item was specified.").Send(_Response)
				End Select

				'If no response has been sent, then the request hasn't been dealt with properly (e.g. a case didn't APIResponse...Send() anything)
				Throw New DSException("The API-EndPoint did not respond with any data")

			Catch _Ex As Exception
				APIResponse.FromInternalServerError("@UploadASPX: " & _Ex.GetType().Name.InSquares() & " "c & _Ex.Message).Send(_Response)
			End Try

		End Sub

	End Class

End Namespace