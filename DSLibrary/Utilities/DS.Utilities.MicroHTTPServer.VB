Namespace Utilities

	''' <summary>
	''' A lightweight HTTP Server, which can serve either a constant String, single File, or a file from a specified Directory.
	''' 
	''' 	<example>
	''' 		Dim _MicroServer As New MicroHTTPServer(_Port:=9020, _ResponseContentString:="Same for all URLs!", _ResponseMIMEType:="text/plain")
	'''  	 	AddHandler _MicroServer.RequestHandledSuccessfully, Sub(_Request As System.Net.HttpListenerRequest) Console.Out.WriteLine(_Request.Url.ToString())
	'''		 	_MicroServer.Run()
	''' 	</example>
	''' 
	''' 	<example>
	''' 		Dim _MicroServer As New MicroHTTPServer(_Port:=9020, _DirectoryToServeFilesFrom:=New IO.DirectoryInfo("F:\"))
	'''  	 	AddHandler _MicroServer.RequestHandledSuccessfully, Sub(_Request As System.Net.HttpListenerRequest) Console.Out.WriteLine(_Request.Url.ToString())
	'''		 	_MicroServer.Run()
	''' 	</example>
	''' 
	''' </summary>
	Public Class MicroHTTPServer

		Public Enum MicroHTTPServerMode As [Byte]
			ServeString = 1 : ServeSingleFile = 2 : ServeFilesFromDirectory = 3
		End Enum

		Protected Shared FileEncodingDeterminer_ As Func(Of IO.FileInfo, System.Text.Encoding) = _
		 Function(_File As IO.FileInfo) As System.Text.Encoding
			 Dim _StreamReader As New System.IO.StreamReader(path:=_File.FullName, encoding:=System.Text.Encoding.ASCII, detectEncodingFromByteOrderMarks:=True)
			 _StreamReader.Peek() : Return _StreamReader.CurrentEncoding
		 End Function

		Protected MIMEType_FromFileExtension_Resolver_ As Func(Of [String], [String]) = _
		 Function(_FileExtension$) As [String]
			 _FileExtension = _FileExtension.ToUpper()
			 If Not Me.MIMETypesDictionary.ContainsKey(_FileExtension) Then Return "text/plain"
			 Return Me.MIMETypesDictionary.Item(_FileExtension)
		 End Function

		Protected HTTPListener_ As New System.Net.HttpListener()
		Protected Mode_ As MicroHTTPServerMode = Nothing
		Protected StringToServe_$ = Nothing, StringToServe_MIMEType_$ = Nothing, FileToServe_ As IO.FileInfo = Nothing, DirectoryToServeFilesFrom_ As IO.DirectoryInfo = Nothing

#Region "Public Propities & Events"

		''' <summary>The logical port on which the Web Server listens. E.g. "900" in the URL "http://localhost:900/Help/".</summary>
		Public Port As UInt16 = Nothing

		''' <summary>The URL sub-path on which the Web Server listens. E.g. "Help" in the URL "http://localhost:900/Help/".</summary>
		Public VirtualPath As [String] = Nothing

		''' <summary>The Number of requests responded-to by the Web Server instance so-far. This is NOT readonly, so that it can be externally reset.</summary>
		Public SuccessfullyHandledRequestsCount As UInt32 = 0

		''' <summary>Occurs directly after a HTTP Request is responded-to. The Argument is the Request, whose full URL is avaliable via the Property .Url.ToString(). Also note the Properties of .UserAgent, .UserHostName, and .HttpMethod.</summary>
		Public Event RequestHandledSuccessfully As Action(Of System.Net.HttpListenerRequest)

		''' <summary>A mapping of FILE-EXTENTIONS to MIME-TYPES. E.g. {"HTM", "text/html"}, {"BMP", "image/bmp"}. If a request's File Extension dosen't appear in the Dictionary's Keys, then the default of "text/plain" is used.</summary>
		Public MIMETypesDictionary As New Dictionary(Of [String], [String])() From {
		 {"HTM", "text/html"},
		 {"HTML", "text/html"},
		 {"TXT", "text/plain"},
		 {"AVI", "video/x-msvideo"},
		 {"BMP", "image/bmp"},
		 {"CSS", "text/css"},
		 {"CSV", "text/csv"},
		 {"ICO", "image/vnd.microsoft.icon"},
		 {"GIF", "image/gif"},
		 {"JFIF", "image/jpeg"},
		 {"JPEG", "image/jpeg"},
		 {"JPG", "image/jpeg"},
		 {"PNG", "image/png"},
		 {"JS", "text/javascript"},
		 {"MP3", "audio/mpeg"},
		 {"MP4", "video/mp4"},
		 {"PDF", "application/pdf"},
		 {"WAV", "audio/wav"},
		 {"XML", "application/xml"},
		 {"ZIP", "application/zip"},
		 {"7Z", "application/x-7z-compressed"}
		}

		''' <summary>E.g. Port=8090 and VirtualPath="Help" would make a URL of http://localhost:8090/Help/</summary>
		Public ReadOnly Property URL As [String]
			Get
				Return [String].Format("http://localhost:{0}/{1}", Me.Port.ToString(), If([String].IsNullOrEmpty(Me.VirtualPath), [String].Empty, Me.VirtualPath & "/"c))
			End Get
		End Property

#End Region

#Region "Constructors for each of the 3 Modes"

		''' <summary>Mode 1: Constructs the MicroHTTPServer to serve a STRING. Example: Port=8090 and VirtualPath="Help" would make a URL of http://localhost:8090/Help/</summary>
		Public Sub New(ByVal _Port As UInt16, ByVal _ResponseContentString$, ByVal _ResponseMIMEType$, Optional ByVal _VirtualPath$ = Nothing)
			Me.Mode_ = MicroHTTPServerMode.ServeString : Me.StringToServe_ = _ResponseContentString : Me.StringToServe_MIMEType_ = _ResponseMIMEType
			Me.Port = _Port : Me.VirtualPath = _VirtualPath
		End Sub

		''' <summary>Mode 2: Constructs the MicroHTTPServer to serve a SINGLE FILE. Uses the [MIMETypes] Dictionary to derive a MIME-Type from the _FileToServe's Extention. Example: Port=8090 and VirtualPath="Help" would make a URL of http://localhost:8090/Help/</summary>
		Public Sub New(ByVal _Port As UInt16, ByVal _FileToServe As IO.FileInfo, Optional ByVal _VirtualPath$ = Nothing)
			Me.Mode_ = MicroHTTPServerMode.ServeSingleFile : Me.FileToServe_ = _FileToServe
			Me.Port = _Port : Me.VirtualPath = _VirtualPath
		End Sub

		''' <summary>Mode 3: Constructs the MicroHTTPServer to serve FILES FROM A DIRECTORY. If the root URL / is requested, an Exception is Thrown. Uses the [MIMETypes] Dictionary to derive a MIME-Type from the File-to-serve's Extention. Example: Port=8090 and VirtualPath="Help" would make a URL of http://localhost:8090/Help/</summary>
		Public Sub New(ByVal _Port As UInt16, ByVal _DirectoryToServeFilesFrom As IO.DirectoryInfo, Optional ByVal _VirtualPath$ = Nothing)
			Me.Mode_ = MicroHTTPServerMode.ServeFilesFromDirectory : Me.DirectoryToServeFilesFrom_ = _DirectoryToServeFilesFrom
			Me.Port = _Port : Me.VirtualPath = _VirtualPath
		End Sub

#End Region

		Protected Sub HandleIncomingConnections_()
			While True

				REM Will wait here until get a Request
				Dim _HTTPListenerContext As System.Net.HttpListenerContext = Me.HTTPListener_.GetContext()

				REM We now have a Request; Extract the request and response objects
				Dim _Request As System.Net.HttpListenerRequest = _HTTPListenerContext.Request
				Dim _Response As System.Net.HttpListenerResponse = _HTTPListenerContext.Response
				Dim _ResponseContentBytes As [Byte]()

				REM Prepare the Response, depending on the Instance's current Mode
				Select Case Me.Mode_
					Case MicroHTTPServerMode.ServeString
						_ResponseContentBytes = System.Text.Encoding.UTF8.GetBytes(Me.StringToServe_)
						_Response.ContentType = Me.StringToServe_MIMEType_ : _Response.ContentEncoding = System.Text.Encoding.UTF8

					Case MicroHTTPServerMode.ServeSingleFile
						If Not Me.FileToServe_.Exists Then
							_ResponseContentBytes = System.Text.Encoding.UTF8.GetBytes("404" & vbCrLf & "The MicroHTTPServer cannot serve this file, because it does not exist en-disk: " & Me.FileToServe_.FullName)
							_Response.StatusCode = Net.HttpStatusCode.NotFound : _Response.ContentType = "text/plain" : _Response.ContentEncoding = System.Text.Encoding.UTF8
						Else
							_ResponseContentBytes = My.Computer.FileSystem.ReadAllBytes(Me.FileToServe_.FullName)
							_Response.ContentType = Me.MIMEType_FromFileExtension_Resolver_.Invoke(IO.Path.GetExtension(Me.FileToServe_.FullName))
							_Response.ContentEncoding = MicroHTTPServer.FileEncodingDeterminer_.Invoke(Me.FileToServe_)
						End If

					Case MicroHTTPServerMode.ServeFilesFromDirectory
						Dim _TheFileWithinTheDirectory_ToServe As New IO.FileInfo(IO.Path.Combine(Me.DirectoryToServeFilesFrom_.FullName, If([String].IsNullOrEmpty(Me.VirtualPath), _Request.Url.LocalPath.Skip(1).ToArray(), _Request.Url.LocalPath.Skip(1 + Me.VirtualPath.Length + 1).ToArray()))) 'The +1s are to skip over any leading forward slashes. Windows would treat /Dir as the root directory of a drive e.g. C:\Dir\
						If Not _TheFileWithinTheDirectory_ToServe.Exists Then
							_ResponseContentBytes = System.Text.Encoding.UTF8.GetBytes("404" & vbCrLf & "The MicroHTTPServer cannot serve this file, because it does not exist en-disk: " & _TheFileWithinTheDirectory_ToServe.FullName)
							_Response.StatusCode = Net.HttpStatusCode.NotFound : _Response.ContentType = "text/plain" : _Response.ContentEncoding = System.Text.Encoding.UTF8
						Else
							_ResponseContentBytes = My.Computer.FileSystem.ReadAllBytes(_TheFileWithinTheDirectory_ToServe.FullName)
							_Response.ContentType = Me.MIMEType_FromFileExtension_Resolver_.Invoke(IO.Path.GetExtension(_TheFileWithinTheDirectory_ToServe.FullName).Replace("."c, ""))
							_Response.ContentEncoding = MicroHTTPServer.FileEncodingDeterminer_.Invoke(_TheFileWithinTheDirectory_ToServe)
						End If

					Case Else : Throw New Exception("Unrecognised MicroHTTPServerMode: " & Me.Mode_.ToString("G"))
				End Select

				REM Send the Response off
				_Response.ContentLength64 = _ResponseContentBytes.LongLength
				_Response.OutputStream.Write(_ResponseContentBytes, 0, _ResponseContentBytes.Length)
				_Response.Close()

				REM Increment the Counter and Raise the [RequestHandledSuccessfully] Event
				Me.SuccessfullyHandledRequestsCount += 1US
				RaiseEvent RequestHandledSuccessfully(_Request)

			End While
		End Sub

		''' <summary>Initialises the HTTPListener and begins the request-handling loop. Each time a request is handled, the [RequestHandled] Event is raised.</summary>
		Public Sub Run()

			REM Create a Http server and start listening for incoming connections
			Me.HTTPListener_.Prefixes.Add(Me.URL)
			Me.HTTPListener_.Start()

			REM Handle requests (never returns)
			Me.HandleIncomingConnections_()

			REM Close the listener
			Me.HTTPListener_.Close()

		End Sub

	End Class

End Namespace