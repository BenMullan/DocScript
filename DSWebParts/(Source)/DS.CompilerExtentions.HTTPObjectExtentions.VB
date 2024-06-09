Namespace CompilerExtentions

	REM There exists also the WebObjectExtentions Module in the Library
	Public Module HTTPObjectExtentions

		''' <summary>
		''' (MullNet CompilerExtention) Writes the opening html, head (etc...) elements, importing the required scripts too.
		''' </summary>
		''' <remarks></remarks>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Function GetScriptAndCSSImports(ByRef _ResponseObject As HttpResponse) As String

			Return (
			 <div>

				 <!-- Bootstrap -->
				 <link rel="Stylesheet" href="/ClientPages/Resources/Stylesheets/bootstrap.min.css"/>
				 <script defer="true" type="text/javascript" src="/ClientPages/Resources/Stylesheets/bootstrap.bundle.min.js"></script>
				 <link rel="Stylesheet" href="/ClientPages/Resources/Stylesheets/bootstrap-icons.css"/><!-- Needs IIS woff MIME-Type -->

				 <!-- CSS -->
				 <link rel="stylesheet" href="/ClientPages/Resources/Stylesheets/DocScript.CSS"/>
				 <link rel="stylesheet" href="/ClientPages/Resources/Stylesheets/Utilities.CSS"/>

				 <!-- JavaScript -->
				 <script type="text/javascript" src="/ClientPages/Resources/Scripts/JQuery.Min.JS"></script>
				 <script type="text/javascript" src="/ClientPages/Resources/Scripts/JQuery.UI.Min.JS"></script>
				 <script type="text/javascript" src="/ClientPages/Resources/Scripts/SweetAlert.JS"></script>
				 <script type="text/javascript" src="/ClientPages/Resources/Scripts/General.JS"></script>
				 <script type="text/javascript" src="/ClientPages/Resources/Scripts/Utilities.JS"></script>
				 <script type="text/javascript" src="/ClientPages/Resources/Scripts/EntryPoints.JS"></script>
				 <script type="text/javascript" src="/ClientPages/Resources/Scripts/AJAX.JS"></script>
				 <script type="text/javascript" src="/ClientPages/Resources/Scripts/UserInterface.JS"></script>
				 <script type="text/javascript" src="/ClientPages/Resources/Scripts/DSI.JS"></script>

			 </div>
			).ToString(System.Xml.Linq.SaveOptions.None) '"Indent" whilst formatting

		End Function

		''' <summary>
		''' (MullNet CompilerExtention) Checks that the Specified Required QueryString Keys are contained in the current Request Object, AND that they each HAVE VALUES.
		''' </summary>
		''' <param name="_RequestObject">(Filled in by the CompilerExtention)</param>
		''' <param name="_ResponseObject">This is needed in case a QueryString is missing, in which case an APIResponse is sent back, informing the user of the missing Parameter</param>
		''' <remarks></remarks>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Sub EnsureTheseQueryStringsAreSpecified(ByRef _RequestObject As HttpRequest, ByRef _ResponseObject As HttpResponse, ByVal ParamArray _RequiredQueryStringKeys$())

			For Each _RequiredQueryStringKey$ In _RequiredQueryStringKeys

				If Not _RequestObject.QueryStringIsSpecifiedAndHasValue(_RequiredQueryStringKey) Then _
				 [WebParts].APIResponse.FromInvalidRequest("No ?" & _RequiredQueryStringKey$ & " was specified with a Value.").Send(_ResponseObject)

			Next

			Return

		End Sub

		''' <summary>
		''' (MullNet CompilerExtention) Checks that the Specified Required Post Data Keys are contained in the current Request Object, and that they HAVE VALUES.
		''' </summary>
		''' <param name="_RequestObject"></param>
		''' <param name="_ResponseObject">This is needed in case a PostData Key is missing, in which case an APIResponse is sent back, informing the user of the missing Parameter</param>
		''' <remarks></remarks>
		<Global.System.Runtime.CompilerServices.Extension()>
		Public Sub EnsureThesePostDataKeysAreSpecified(ByRef _RequestObject As HttpRequest, ByRef _ResponseObject As HttpResponse, ByVal ParamArray _RequiredPostDataKeys$())

			For Each _RequiredPostDataKey$ In _RequiredPostDataKeys

				If Not _RequestObject.PostDataItemIsSpecifiedAndHasValue(_RequiredPostDataKey) Then _
				 [WebParts].APIResponse.FromInvalidRequest("The Post Data Key " & _RequiredPostDataKey$.InSquares() & " was not specified with a Value.").Send(_ResponseObject)

			Next

			Return

		End Sub

	End Module

End Namespace