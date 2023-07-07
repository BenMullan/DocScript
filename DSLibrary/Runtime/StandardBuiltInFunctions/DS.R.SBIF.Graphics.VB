Imports DocScript.Runtime.BuiltInFunction, DocScript.Language.Variables, DocScript.Language.Instructions, DocScript.Language.Instructions.Statements

Namespace Runtime

	Partial Public Class StandardBuiltInFunctions

		Private Const GraphicsBIFs_IdentifierStart_$ = "Graphics_"

#Region "Individual BIF Member Declarations"

		Private Const BIF_Graphics_TextWindow_Identifier_$ = GraphicsBIFs_IdentifierStart_ & "TextWindow"
		Private Shared ReadOnly BIF_Graphics_TextWindow_ As New BuiltInFunction(BIF_Graphics_TextWindow_Identifier_$, {New DSFunction.Parameter("_WindowTitle", GetType(DSString)), New DSFunction.Parameter("_Text", GetType(DSString))}, GetType(Void), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_Graphics_TextWindow_Identifier_$)
			 UsefulMethods.ShowTextInNewWindow(_Arguments.ElementAt(0).Coerce(Of DSString)().Value, _Arguments.ElementAt(1).Coerce(Of DSString)().Value)
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Shows _Text in a Text Window with Copy and Save-As Buttons."}

		Private Const BIF_Graphics_TextWindowFromFile_Identifier_$ = GraphicsBIFs_IdentifierStart_ & "TextWindowFromFile"
		Private Shared ReadOnly BIF_Graphics_TextWindowFromFile_ As New BuiltInFunction(BIF_Graphics_TextWindowFromFile_Identifier_$, {New DSFunction.Parameter("_WindowTitle", GetType(DSString)), New DSFunction.Parameter("_FilePath", GetType(DSString))}, GetType(Void), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_Graphics_TextWindowFromFile_Identifier_$)
			 UsefulMethods.ShowTextInNewWindow(_Arguments.ElementAt(0).Coerce(Of DSString)().Value, My.Computer.FileSystem.ReadAllText(_Arguments.ElementAt(1).Coerce(Of DSString)().Value))
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Shows the text from _FilePath in a Text Window with Copy and Save-As Buttons."}

#Region "Image Window"

		Private Shared CurrentImageWindow_ As Windows.Forms.Form = Nothing

		Private Const BIF_Graphics_ImageWindow_Identifier_$ = GraphicsBIFs_IdentifierStart_ & "ImageWindow"
		Private Shared ReadOnly BIF_Graphics_ImageWindow_ As New BuiltInFunction(BIF_Graphics_ImageWindow_Identifier_$, {New DSFunction.Parameter("_WindowTitle", GetType(DSString)), New DSFunction.Parameter("_ImageFilePath", GetType(DSString))}, GetType(Void), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_Graphics_ImageWindow_Identifier_$)

			 StandardBuiltInFunctions.CurrentImageWindow_ = _
			   UsefulMethods.ConstructImageWindow(_Arguments.ElementAt(0).Coerce(Of DSString)().Value, New System.Drawing.Bitmap(_Arguments.ElementAt(1).Coerce(Of DSString)().Value))

			 Dim _STAThread As New Threading.Thread(AddressOf StandardBuiltInFunctions.CurrentImageWindow_.ShowDialog)
			 _STAThread.SetApartmentState(Threading.ApartmentState.STA) : _STAThread.Start()

			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Shows the Image from _ImageFilePath in a Window with a Copy and Save-As Buttons."}

		Private Const BIF_Graphics_CenterScreenImageWindow_Identifier_$ = GraphicsBIFs_IdentifierStart_ & "CenterScreenImageWindow"
		Private Shared ReadOnly BIF_Graphics_CenterScreenImageWindow_ As New BuiltInFunction(BIF_Graphics_CenterScreenImageWindow_Identifier_$, {New DSFunction.Parameter("_WindowTitle", GetType(DSString)), New DSFunction.Parameter("_ImageFilePath", GetType(DSString))}, GetType(Void), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_Graphics_CenterScreenImageWindow_Identifier_$)

			 StandardBuiltInFunctions.CurrentImageWindow_ = _
			UsefulMethods.ConstructImageWindow(_Arguments.ElementAt(0).Coerce(Of DSString)().Value, New System.Drawing.Bitmap(_Arguments.ElementAt(1).Coerce(Of DSString)().Value))

			 StandardBuiltInFunctions.CurrentImageWindow_.StartPosition = Windows.Forms.FormStartPosition.CenterScreen

			 Dim _STAThread As New Threading.Thread(AddressOf StandardBuiltInFunctions.CurrentImageWindow_.ShowDialog)
			 _STAThread.SetApartmentState(Threading.ApartmentState.STA) : _STAThread.Start()

			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Shows the Image from _ImageFilePath in a Window with a Copy and Save-As Buttons, in the center of the logically-addressable desktop window area."}

		Private Const BIF_Graphics_ClosePreviousImageWindow_Identifier_$ = GraphicsBIFs_IdentifierStart_ & "ClosePreviousImageWindow"
		Private Shared ReadOnly BIF_Graphics_ClosePreviousImageWindow_ As New BuiltInFunction(BIF_Graphics_ClosePreviousImageWindow_Identifier_$, {}, GetType(Void), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_Graphics_ClosePreviousImageWindow_Identifier_$)

			 Try
				 If StandardBuiltInFunctions.CurrentImageWindow_ IsNot Nothing Then StandardBuiltInFunctions.CurrentImageWindow_.Invoke(Sub() StandardBuiltInFunctions.CurrentImageWindow_.Close())
			 Catch _Ex As Exception
				 LogSystemMessage("The ImageWindow could not be closed because: " & _Ex.Message)
			 End Try

			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "If a call to Graphics_ImageWindow() has previously opened an ImageWindow, then this window is closed."}

#End Region

		Private Const BIF_Graphics_URLWindow_Identifier_$ = GraphicsBIFs_IdentifierStart_ & "URLWindow"
		Private Shared ReadOnly BIF_Graphics_URLWindow_ As New BuiltInFunction(BIF_Graphics_URLWindow_Identifier_$, {New DSFunction.Parameter("_WindowTitle", GetType(DSString)), New DSFunction.Parameter("_URL", GetType(DSString))}, GetType(Void), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_Graphics_URLWindow_Identifier_$)
			 UsefulMethods.ShowURLContentsInNewWindow(_Arguments.ElementAt(0).Coerce(Of DSString)().Value, _Arguments.ElementAt(1).Coerce(Of DSString)().Value)
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "_URL can be a web address, local HTML File, or file directory."}

		Private Const BIF_Graphics_SaveScreenshot_Identifier_$ = GraphicsBIFs_IdentifierStart_ & "SaveScreenshot"
		Private Shared ReadOnly BIF_Graphics_SaveScreenshot_ As New BuiltInFunction(BIF_Graphics_SaveScreenshot_Identifier_$, {New DSFunction.Parameter("_PNGSaveAsPath", GetType(DSString))}, GetType(Void), New BuiltInFunctionDelegate(
		 Function(_SymTbls As SymbolTablesSnapshot, _Arguments As Language.Variables.IDataValue()) As ExecutionResult

			 Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("BIF\" & BIF_Graphics_SaveScreenshot_Identifier_$)
			 DocScript.Utilities.Screenshotting.TakeAndSaveScreenshotAsPNG(New IO.FileInfo(_Arguments.ElementAt(0).Coerce(Of DSString)().Value))
			 Return _ExeRes.StopExecutionTimer_AndFinaliseObject(_SymTbls)

		 End Function)
		) With {.[Description] = "Takes a screenshot (spanning all avaliable monitors), saving it as a PNG at the specified file path."}

#End Region

		''' <summary>DS-Graphics</summary>
		Public Shared ReadOnly Graphics As DocScript.Runtime.BuiltInFunction() = {
		 StandardBuiltInFunctions.BIF_Graphics_TextWindow_,
		 StandardBuiltInFunctions.BIF_Graphics_TextWindowFromFile_,
		 StandardBuiltInFunctions.BIF_Graphics_ImageWindow_,
		 StandardBuiltInFunctions.BIF_Graphics_CenterScreenImageWindow_,
		 StandardBuiltInFunctions.BIF_Graphics_ClosePreviousImageWindow_,
		 StandardBuiltInFunctions.BIF_Graphics_URLWindow_,
		 StandardBuiltInFunctions.BIF_Graphics_SaveScreenshot_
		}

	End Class

End Namespace