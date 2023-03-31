Namespace WebParts

	''' <summary>Respresents a DocScript API Response. Can be serialised to XML to be sent off to the Client.</summary>
	Public Class APIResponse

		Public Const XML_MIMEType$ = "application/xml"
		Protected Const Default_DARErrorMessage$ = "" 'Used to be "Not Applicable"

		REM Used by ToString() as a base template
		Protected Shared ReadOnly EmptyAPIResponseXML As XElement = _
		 <APIResponse>
			 <DataAboutResponse HTTPStatusCode="" ErrorMessage="" LongPollingTimedOut=""/>
			 <ResponseContent/>
		 </APIResponse>

		REM DataAboutResponse (DAR) Variables
		Public DAR_HTTPStatusCode As System.Net.HttpStatusCode = Net.HttpStatusCode.OK 'This is both Concatenated into the DAR XML, and applied as the actual HTTP Status Code on the HTTPResponse Object
		Public DAR_ErrorMessage As String = APIResponse.Default_DARErrorMessage	'E.g. explains why a Client's Request was invalid, or why an Internal Server Error occured.
		Public DAR_LongPollingTimedOut As Boolean = False 'Indicates weather or not the Response is being send because of a long-polling Timeout

		REM ResponseContent (RC) Variables and XML Children
		Public RC_KeyValuePairs As New Dictionary(Of String, String)()
		Public RC_ChildNodes As New List(Of XElement)()

		REM The class must be constructed using one of the Sideloading Constructors
		Protected Sub New()
		End Sub

#Region "Sideloading Constructors"

		''' <summary>
		''' Constructs an APIResponse Object to indicate that an unhandled Exception occured on the Server.
		''' HTTP Status Code: 500
		''' </summary>
		''' <param name="_ErrorMsg">The DAR Error Message explaining why the Internal Server Error may have occured</param>
		Public Shared Function FromInternalServerError(ByVal _ErrorMsg$) As APIResponse

			Dim _NewAPIResponse As New APIResponse() With {.DAR_HTTPStatusCode = Net.HttpStatusCode.InternalServerError, .DAR_ErrorMessage = _ErrorMsg}

			Return _NewAPIResponse

		End Function

		''' <summary>
		''' Constructs an APIResponse Object to indicate that the Client's Request was syntactically-invalid, or did not include all nessecary QueryStrings or PostData.
		''' HTTP Status Code: 400
		''' </summary>
		''' <param name="_ErrorMsg">The DAR Error Message explaining why the Client's Request is invalid</param>
		Public Shared Function FromInvalidRequest(ByVal _ErrorMsg$) As APIResponse

			Dim _NewAPIResponse As New APIResponse() With {.DAR_HTTPStatusCode = Net.HttpStatusCode.BadRequest, .DAR_ErrorMessage = _ErrorMsg}

			Return _NewAPIResponse

		End Function

		''' <summary>
		''' Constructs an APIResponse Object with no ResponseContent, indicating that the request was processed successfully.
		''' HTTP Status Code: 200
		''' </summary>
		Public Shared Function FromValidRequest() As APIResponse
			Try

				Return (New APIResponse() With {.DAR_HTTPStatusCode = Net.HttpStatusCode.OK, .DAR_ErrorMessage = APIResponse.Default_DARErrorMessage})

			Catch _Ex As Exception : Throw New DSException("@APIResponse\FromValidRequest(): " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>
		''' Constructs an APIResponse Object with the specified KeyValuePairs.
		''' HTTP Status Code: 200
		''' </summary>
		''' <param name="_ResponseContent_KVPs">The XML Attributes of the ResponseContent Node</param>
		Public Shared Function FromValidRequest(ByVal ParamArray _ResponseContent_KVPs As String()()) As APIResponse
			Try

				Dim _NewAPIResponse As New APIResponse() With {.DAR_HTTPStatusCode = Net.HttpStatusCode.OK, .DAR_ErrorMessage = APIResponse.Default_DARErrorMessage}

				REM _ResponseContentKVPs should look like:
				REM		{"Key0", "Value"}, {"Key1", "Value"}

				Dim _RCDictionary As New Dictionary(Of String, String)()

				For Each _RCKVP As String() In _ResponseContent_KVPs
					If Not _RCKVP.Length = 2 Then Throw New DSValidationException("The ResponseContent KeyValuePair did not have 2 values in its String Array", _RCKVP.GetStandardArraySerialisation())
					_RCDictionary.Add(_RCKVP(0), _RCKVP(1))
				Next

				_NewAPIResponse.RC_KeyValuePairs = _RCDictionary

				Return _NewAPIResponse

			Catch _Ex As Exception : Throw New DSException("@APIResponse\FromValidRequest(): " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>
		''' Constructs an APIResponse Object with the specified KeyValuePairs and XMLChildren.
		''' HTTP Status Code: 200
		''' </summary>
		''' <param name="_ResponseContent_KVPs">The XML Attributes of the ResponseContent Node</param>
		''' <param name="_ResponseContent_XMLChildren">The XML Children of the ResponseContent Node</param>
		Public Shared Function FromValidRequest(ByVal _ResponseContent_KVPs As String()(), ByVal _ResponseContent_XMLChildren As XElement()) As APIResponse
			Try

				Dim _NewAPIResponse As New APIResponse() With {.DAR_HTTPStatusCode = Net.HttpStatusCode.OK, .DAR_ErrorMessage = APIResponse.Default_DARErrorMessage}

				_NewAPIResponse.RC_ChildNodes = _ResponseContent_XMLChildren.ToList()

				REM _ResponseContentKVPs should look like:
				REM		{"Key0", "Value"}, {"Key1", "Value"}

				Dim _RCDictionary As New Dictionary(Of String, String)()

				For Each _RCKVP As String() In _ResponseContent_KVPs
					If Not _RCKVP.Length = 2 Then Throw New DSValidationException("The ResponseContent KeyValuePair did not have 2 values in its String Array", String.Join(", ", _RCKVP).InCurlies())
					_RCDictionary.Add(_RCKVP(0), _RCKVP(1))
				Next

				_NewAPIResponse.RC_KeyValuePairs = _RCDictionary

				Return _NewAPIResponse

			Catch _Ex As Exception : Throw New DSException("@APIResponse\FromValidRequest(): " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>
		''' Constructs an APIResponse Object to indicate that the response is only being sent because an AJAX-LongPolling Request Timed out.
		''' HTTP Status Code: 200
		''' </summary>
		Public Shared Function FromLongPollingTimeout() As APIResponse
			Try

				Return (New APIResponse() With {.DAR_HTTPStatusCode = Net.HttpStatusCode.OK, .DAR_ErrorMessage = APIResponse.Default_DARErrorMessage, .DAR_LongPollingTimedOut = True})

			Catch _Ex As Exception : Throw New DSException("@APIResponse\FromLongPollingTimeout(): " & _Ex.Message, _Ex) : End Try
		End Function

#End Region

		''' <summary>Clears the Response, then writes the APIResponse to the _HTTPResponseStream after setting the HTTP Status Code and MIME Type. The Response is then Ended.</summary>
		Public Sub Send(ByRef _HTTPResponseStream As System.Web.HttpResponse)
			Try

				REM Don't send any Response Bytes back until the entire response is ready
				_HTTPResponseStream.BufferOutput = True
				_HTTPResponseStream.Buffer = True

				REM Set the Response Status Code
				REM Set the Response MIME Type
				REM Set the Response Text

				_HTTPResponseStream.Clear()
				'_HTTPResponseStream.ClearHeaders()

				_HTTPResponseStream.ContentType = APIResponse.XML_MIMEType
				_HTTPResponseStream.StatusCode = Me.DAR_HTTPStatusCode

				_HTTPResponseStream.Write(Me.ToString())

				'_HTTPResponseStream.Flush()
				'_HTTPResponseStream.Close()
				_HTTPResponseStream.SafelyEnd()
				_HTTPResponseStream.End()

				'Return
				'_HTTPResponseStream.ClearAndWriteAndEnd(Me.ToString())

			Catch _Ex As System.Web.HttpException
				REM Ignore the following errors...
				If {
					"Der Server kann den Inhaltstyp nicht festlegen, nachdem HTTP-Header gesendet wurden",
					"Der Server kann den Status nicht festlegen, nachdem HTTP-Header gesendet wurden"
				}.Any(AddressOf _Ex.Message.StartsWith) Then : Debug.WriteLine("API Response HttpException: " & _Ex.Message)
				Else : Throw _Ex
				End If
			End Try
		End Sub

		''' <summary>
		''' Returns the XML for this APIResponse as a String, E.g. ready for it to be written to a HTTP Response Stream.
		''' Do not use this method to send the APIResponse; use .Send() instead!
		''' </summary>
		Public Overrides Function ToString() As String

			'If we just do...
			'Dim _APIResponseXML As XElement = EmptyAPIResponseXML
			'...Then _APIResponseXML points to the same instance as EmptyAPIResponseXML.
			'Therefore, we are modifying the ReadOnly Object.
			'
			'ASP .NET for some reason decides to keep this Object shared and alive for all requests,
			'Which causes the previous RC XML Children to be added too.

			Dim _APIResponseXML As New XElement(EmptyAPIResponseXML)

			_APIResponseXML.<DataAboutResponse>.@ErrorMessage = Me.DAR_ErrorMessage
			_APIResponseXML.<DataAboutResponse>.@HTTPStatusCode = (Convert.ToInt32(Me.DAR_HTTPStatusCode).ToString() & "/"c & Me.DAR_HTTPStatusCode.ToString("G")) 'E.g. 200/OK
			_APIResponseXML.<DataAboutResponse>.@LongPollingTimedOut = Me.DAR_LongPollingTimedOut.ToString()

			REM Add the KVPs as Attrs to the ResponseContent
			For Each _RCKVP As KeyValuePair(Of String, String) In Me.RC_KeyValuePairs
				_APIResponseXML.Element("ResponseContent").SetAttributeValue(_RCKVP.Key, _RCKVP.Value)
			Next

			REM Add any Children of the ResponseContent
			For Each _RCChild As XElement In Me.RC_ChildNodes
				_APIResponseXML.Element("ResponseContent").Add(_RCChild)
			Next

			Return _APIResponseXML.ToString()

		End Function

	End Class

End Namespace