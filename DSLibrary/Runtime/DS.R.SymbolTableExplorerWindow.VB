Namespace Runtime

	Partial Public Class SymbolTableExplorerWindow

		Public ReadOnly SymbolTables As SymbolTablesSnapshot
		Protected ReadOnly SymbolTablesCameFrom_XMLFile As IO.FileInfo = Nothing

		Public Sub New(ByRef _SymbolTables As DocScript.Runtime.SymbolTablesSnapshot)

			Me.InitializeComponent()
			Me.SymbolTables = _SymbolTables

			AddHandler Me.ContinueButton.Click, Sub() Me.Close()

		End Sub

		Public Sub New(ByVal _SymTblsXMLFile As IO.FileInfo)
			Me.New(_SymbolTables:=SymbolTablesSnapshot.FromXML(XElement.Load(_SymTblsXMLFile.FullName)))
			Me.SymbolTablesCameFrom_XMLFile = _SymTblsXMLFile
		End Sub

		Public Sub CreateControls_ForCurrentSymbolTables() Handles Me.Shown

			REM Put the Window into a loading state, then generate the TabPages, then re-enable the Window
			Me.Enabled = False
			Me.Text = "Loading SymbolTables..."
			Me.Cursor = Windows.Forms.Cursors.WaitCursor
			Me.Refresh()

			Me.CreateTabPages_FromSymbolTables_()
			Me.Text = "SymbolTable Explorer (" & Me.SymbolTables_TabControl.TabPages.Count.ToString() & " SymbolTables)"
			If Me.SymbolTablesCameFrom_XMLFile IsNot Nothing Then Me.Text &= " From " & Me.SymbolTablesCameFrom_XMLFile.FullName

			Me.Enabled = True
			Me.Cursor = Windows.Forms.Cursors.Default
			Me.Refresh()

		End Sub

		Public Sub LoadExistingSymbolTablesFromXML() Handles OpenMenuItem.Click
			Try : Me.Enabled = False : Me.Cursor = Windows.Forms.Cursors.WaitCursor

				Dim _ChosenFile As IO.FileInfo = UsefulMethods.RequestFilePath_WithOpenFileDialog("Open Saved SymbolTables' States", {New Tuple(Of [String], [String])("DocScript SymbolTables' States XML", "XML")})
				If _ChosenFile IsNot Nothing Then Call New Threading.Thread(AddressOf (New SymbolTableExplorerWindow(_SymTblsXMLFile:=_ChosenFile)).ShowDialog).Start()
				Me.Activate() : Me.BringToFront() : Me.Focus()

			Catch _Ex As Exception : MsgBox("The SymbolTables could not be opened." & vbCrLf & _Ex.Message, MsgBoxStyle.Critical, "DocScript Runtime")
			Finally : Me.Enabled = True : Me.Cursor = Windows.Forms.Cursors.Default : End Try
		End Sub

#Region "TabPage and ListView Generation Logic"

		Protected Sub CreateTabPages_FromSymbolTables_()

			'Just in case...
			Me.SymbolTables_TabControl.TabPages.Clear()

			REM Each SymbolTable has its own Tab
			For Each _SymTbl As SymbolTable In Me.SymbolTables.SymbolTables_ReadOnlyCopy

				Dim _TabKey$ = "SymbolTableTab_" & Me.SymbolTables_TabControl.TabPages.Count.ToString()

				Me.SymbolTables_TabControl.TabPages.Add(
				 key:=_TabKey,
				 text:=_SymTbl.Entries_ReadOnlyCopy.Count.ToString().InSquares() & " "c & _SymTbl.Description,
				 imageIndex:=0
				)

				Me.SymbolTables_TabControl.TabPages.Item(_TabKey).Controls.Add(GenerateListView_ForSymbolTable_(_SymTbl))

			Next

			'Select the deepest-down (last) Tab, which contains at least one SymTbl-Entry
			Dim _FirstTabPageWithAtLeastOneSymTblEntry_Resolver As Func(Of Windows.Forms.TabPage, [Boolean]) = Function(_TabPage As Windows.Forms.TabPage) (_TabPage.Controls.OfType(Of Windows.Forms.ListView)().Count() = 1) AndAlso (_TabPage.Controls.OfType(Of Windows.Forms.ListView)().First().Items.Count > 0)
			If Not Me.SymbolTables_TabControl.TabPages.OfType(Of Windows.Forms.TabPage)().Any(_FirstTabPageWithAtLeastOneSymTblEntry_Resolver) Then Return
			Me.SymbolTables_TabControl.SelectedIndex = Me.SymbolTables_TabControl.TabPages.OfType(Of Windows.Forms.TabPage)().ToArray().IndexOfLastWhere(_FirstTabPageWithAtLeastOneSymTblEntry_Resolver)

		End Sub

		Protected Function GenerateListView_ForSymbolTable_(ByRef _SymTbl As SymbolTable) As Windows.Forms.ListView

			Dim _Symbols_ListView As New Windows.Forms.ListView() With {
			 .Dock = Windows.Forms.DockStyle.Fill,
			 .FullRowSelect = True,
			 .LargeImageList = Me.TheImageList, .SmallImageList = Me.TheImageList,
			 .GridLines = True,
			 .View = Windows.Forms.View.Details
			 }

			_Symbols_ListView.Columns.AddRange({
			 New Windows.Forms.ColumnHeader(imageIndex:=1) With {.Text = "Identifier", .Width = 200},
			 New Windows.Forms.ColumnHeader(imageIndex:=3) With {.Text = "Type", .Width = 150},
			 New Windows.Forms.ColumnHeader(imageIndex:=2) With {.Text = "Value", .Width = 400}
			})

			'For Each _SymTblEntry As KeyValuePair(Of String, SymbolTable.ISymbolTableEntry) In _SymTbl.Entries_ReadOnlyCopy
			REM Because we want the most-recently-added Symbols to be at the top, we'll use a reverse ForLoop
			For _SymTblEntry_Index% = (_SymTbl.Entries_ReadOnlyCopy.Count - 1) To 0 Step -1 : Dim _SymTblEntry As KeyValuePair(Of String, SymbolTable.ISymbolTableEntry) = _SymTbl.Entries_ReadOnlyCopy.ElementAt(_SymTblEntry_Index)

				REM Determine the Entry's Icon
				Dim _EntryIcon_ImageKey$
				If Runtime.SymbolTable.IsVariableEntry(_SymTblEntry.Value) Then : _EntryIcon_ImageKey = "IDataValue"
				ElseIf Runtime.SymbolTable.IsFunctionEntry(_SymTblEntry.Value) Then : _EntryIcon_ImageKey = "Function"
				Else : Throw New DSValidationException("The SymbolTableEntry's Payload type was unaccounted-for", "The SymbolTableEntry was of type: " & _SymTblEntry.Value.GetType().FullName)
				End If

				REM Attempt to generate a Type-Description-String for the Entry
				Dim _EntryValue_TypeDescriptionString$
				Try
					'Could be like:		ISymbolTableEntry(Of DSArray(Of DSString))
					Dim _FirstGeneric As Type = _SymTblEntry.Value.GetType().GetGenericArguments().First()
					_EntryValue_TypeDescriptionString$ = _FirstGeneric.Name.Split("`"c).First() & If(_FirstGeneric.GetGenericArguments().Length > 0, " (Of " & _FirstGeneric.GetGenericArguments().First().Name & ")", String.Empty)
				Catch _Ex As Exception : _EntryValue_TypeDescriptionString = "(Could not determine Entry's Payload type) Entry Type: " & _SymTblEntry.Value.GetType().FullName & ". Exception: " & _Ex.Message : End Try

				_Symbols_ListView.Items.Add(New Windows.Forms.ListViewItem(items:={_SymTblEntry.Key, _EntryValue_TypeDescriptionString, _SymTblEntry.Value.Value_ToString()}, imagekey:=_EntryIcon_ImageKey))

			Next

			Return _Symbols_ListView

		End Function

#End Region

#Region "HTML, CSV, and XML Serialisation"

		Public Sub SaveSymbolTablesAs() Handles SaveAsMenuItem.Click
			Try : Me.Enabled = False : Me.Cursor = Windows.Forms.Cursors.WaitCursor

				REM Always ask for a new Path to save the source as
				Dim _SaveAsFile As IO.FileInfo =
				 UsefulMethods.RequestFilePath_WithSaveFileDialog(
				   "Save the DS-SymbolTables as a File", {
				  New Tuple(Of [String], [String])("Hypertext Markup", "HTML"),
				  New Tuple(Of [String], [String])("Comma-Seperated Values", "CSV"),
				  New Tuple(Of [String], [String])("DocScript SymbolTables' States XML", "XML")
				 },
				   Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
				 )

				If _SaveAsFile IsNot Nothing Then

					REM Resolve the Serialisation Text
					Dim _FileContent$ = String.Empty
					Select Case _SaveAsFile.Extension.ToUpper()
						Case ".HTML" : _FileContent = Me.MySymbolTables_AsHTML.ToTabFormattedString()
						Case ".CSV" : _FileContent = Me.MySymbolTables_AsCSV
						Case ".XML" : _FileContent = Me.SymbolTables.ToXML().ToTabFormattedString()
						Case Else : Throw New DSValidationException("The SaveAs File-Extention was unaccounted-for", "Extention: " & _SaveAsFile.Extension.ToUpper())
					End Select

					REM Save and Open the File
					Global.DocScript.My.Computer.FileSystem.WriteAllText(
					 file:=_SaveAsFile.FullName,
					 text:=_FileContent,
					 append:=False
					)
					Process.Start(_SaveAsFile.FullName)

				End If

			Catch _Ex As Exception : MsgBox("The SymbolTables could not be saved." & vbCrLf & _Ex.Message, MsgBoxStyle.Critical, "DocScript Runtime Serialisation")
			Finally : Me.Enabled = True : Me.Cursor = Windows.Forms.Cursors.Default : End Try
		End Sub

		Public ReadOnly Property MySymbolTables_AsHTML As XElement
			Get

				REM ↓ The call to Reverse() is so that the most recent (upstairs) SymbolTables are at the top

				Return _
				 <html>
					 <!-- Generated by the DocScript SymbolTable-Explorer Window -->
					 <head>
						 <title>DocScript SymbolTables Export (<%= DateTime.Now.ToString() %>)</title>
						 <style>
								table, th, td { border:1px solid black; }
								body, html { font-family: Inconsolata Variable Weight,Consolas,Courier New; }
								th, td { padding-left: 5px; padding-right: 5px; }
						 </style>
					 </head>
					 <body>
						 <center>
							 <h1>DocScript SymbolTables Export [<%= Me.SymbolTables.SymbolTables_ReadOnlyCopy.Count.ToString() %> SymbolTable(s)] (<%= DateTime.Now.ToString() %>)</h1>
							 <%=
								 (From _SymTbl As SymbolTable In Me.SymbolTables.SymbolTables_ReadOnlyCopy Select
								 <div style="padding: 2%;">
									 <h2><%= _SymTbl.Description & " "c & _SymTbl.Entries_ReadOnlyCopy.Count.ToString().InSquares() %></h2>
									 <table style="width: 80%;">
										 <tr>
											 <th>Identifier</th>
											 <th>Datatype</th>
											 <th>Value</th>
										 </tr>
										 <%=
											 From _SymTblEntry As KeyValuePair(Of String, SymbolTable.ISymbolTableEntry) In _SymTbl.Entries_ReadOnlyCopy Select
											 <tr>
												 <td><%= _SymTblEntry.Key %></td>
												 <td><%=
														 (Function() As String

															  REM Attempt to generate a Type-Description-String for the Entry
															  Dim _EntryValue_TypeDescriptionString$
															  Try
																  'Could be like:		ISymbolTableEntry(Of DSArray(Of DSString))
																  Dim _FirstGeneric As Type = _SymTblEntry.Value.GetType().GetGenericArguments().First()
																  _EntryValue_TypeDescriptionString$ = _FirstGeneric.Name.Split("`"c).First() & If(_FirstGeneric.GetGenericArguments().Length > 0, " (Of " & _FirstGeneric.GetGenericArguments().First().Name & ")", String.Empty)
															  Catch _Ex As Exception : _EntryValue_TypeDescriptionString = "(Could not determine Entry's Payload type) Entry Type: " & _SymTblEntry.Value.GetType().FullName & ". Exception: " & _Ex.Message : End Try
															  Return _EntryValue_TypeDescriptionString

														  End Function).Invoke()
													 %></td>
												 <td><%= _SymTblEntry.Value.Value_ToString().Replace(vbCr, "{CR}").Replace(vbLf, "{LF}").Replace(vbTab, "{Tab}") %></td>
											 </tr>
										 %>
									 </table>
								 </div>).Reverse()
							 %>
						 </center>
					 </body>
				 </html>

			End Get
		End Property

		Public ReadOnly Property MySymbolTables_AsCSV As [String]
			Get

				REM E.g....
				'SymbolTable,Identifier,Datatype,Value
				'

				REM ↓ The call to Reverse() is so that the most recent (upstairs) SymbolTables are at the top

				Return "SymbolTable Description,Identifier,DataType,Value" & vbCrLf & _
				 [String].Join([vbCrLf] & [vbCrLf],
				  (From _SymTbl As SymbolTable In Me.SymbolTables.SymbolTables_ReadOnlyCopy Select
				   [String].Join([vbCrLf],
				 (From _SymTblEntry As KeyValuePair(Of [String], SymbolTable.ISymbolTableEntry) In _SymTbl.Entries_ReadOnlyCopy Select
				  [String].Join(","c,
				   _SymTbl.Description,
				   _SymTblEntry.Key,
				   (Function() As String

						REM Attempt to generate a Type-Description-String for the Entry
						Dim _EntryValue_TypeDescriptionString$
						Try
							'Could be like:		ISymbolTableEntry(Of DSArray(Of DSString))
							Dim _FirstGeneric As Type = _SymTblEntry.Value.GetType().GetGenericArguments().First()
							_EntryValue_TypeDescriptionString$ = _FirstGeneric.Name.Split("`"c).First() & If(_FirstGeneric.GetGenericArguments().Length > 0, " (Of " & _FirstGeneric.GetGenericArguments().First().Name & ")", [String].Empty)
						Catch _Ex As Exception : _EntryValue_TypeDescriptionString = "(Could not determine Entry's Payload type) Entry Type: " & _SymTblEntry.Value.GetType().FullName & ". Exception: " & _Ex.Message : End Try
						Return _EntryValue_TypeDescriptionString

					End Function).Invoke(),
				   _SymTblEntry.Value.Value_ToString().Replace(vbCr, "{CR}").Replace(vbLf, "{LF}").Replace(vbTab, "{Tab}").Replace(","c, "{Comma}")
				  )
				 ).Reverse()
				   )
				  ).Reverse()
				 )

			End Get
		End Property

#End Region

#Region "Close the form on pressing [Esc]"

		Protected Sub Register_CloseOnEscKey_Handler() Handles Me.Load
			Me.KeyPreview = True
			AddHandler Me.KeyDown, Sub(_Sender As [Object], _KeyEventArgs As Windows.Forms.KeyEventArgs) If _KeyEventArgs.KeyCode = Windows.Forms.Keys.Escape Then Me.Close()
		End Sub

		Protected Overrides Function ProcessDialogKey(keyData As System.Windows.Forms.Keys) As [Boolean]
			If (Windows.Forms.Form.ModifierKeys = Windows.Forms.Keys.None) AndAlso (keyData = Windows.Forms.Keys.Escape) Then
				Me.Close() : Return True
			End If
			Return MyBase.ProcessDialogKey(keyData)
		End Function

#End Region

	End Class

End Namespace