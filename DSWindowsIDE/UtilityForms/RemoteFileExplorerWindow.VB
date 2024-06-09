Public Class RemoteFileExplorerWindow

	Public TheActionInProgressDialog As ActionInProgressDialog

	Public Sub New(Optional ByVal _UNCPath$ = "", Optional ByVal _Username$ = "", Optional ByVal _Password$ = "")
		Me.InitializeComponent()
		Me.RemoteUNCPath_TextBox.Text = _UNCPath : Me.Username_TextBox.Text = _Username : Me.Password_TextBox.Text = _Password
	End Sub

	Public Sub UpdatePathTextbox() Handles RemoteFilesWebBrowser.Navigated
		Try
			Me.RemoteUNCPath_TextBox.Text = Me.RemoteFilesWebBrowser.Url.LocalPath
		Catch _Ex As Exception : MsgBox("The Path could not be shown: " & _Ex.Message, MsgBoxStyle.Critical) : End Try
	End Sub

	Public Sub OpenRemoteDirectory() Handles ConnectButton.Click
		Try

			Me.Enabled = False : Me.Cursor = Forms.Cursors.WaitCursor
			Me.TheActionInProgressDialog = New ActionInProgressDialog("Initialising...")
			Me.TheActionInProgressDialog.ShowInNewThread()

			If {Me.RemoteUNCPath_TextBox.Text, Me.Username_TextBox.Text, Me.Password_TextBox.Text}.Any(AddressOf [String].IsNullOrEmpty) Then Throw New Exception("Specify a UNC Path, Username, and Password. For example ""\\Hostname\DriveLetter$\Directory\"".")
			Dim _UNCDirectoryOnRemotePC As New IO.DirectoryInfo(Me.RemoteUNCPath_TextBox.Text)

			'Disconnect in case we are currently connected with other credentials
			Me.TheActionInProgressDialog.UpdateText("Removing old Credentials...")
			DocScript.Runtime.Remoting.RemoveCredentialsAssociatedWithNetworkFolder(_UNCDirectoryOnRemotePC.FullName.WithoutBackslashOnEnd(), _ForceDisconnection:=True)

			'Connect \w new credentials
			Me.TheActionInProgressDialog.UpdateText("Registering new Credentials...")
			DocScript.Runtime.Remoting.AuthenticateWithNetworkFolder(
			 _UNCPath:=_UNCDirectoryOnRemotePC.FullName.WithoutBackslashOnEnd(),
			 _Username:=Me.Username_TextBox.Text,
			 _Password:=Me.Password_TextBox.Text
			)

			'Use the open authenticated UNC Connection
			Me.TheActionInProgressDialog.UpdateText("Opening remote directory...")
			Me.RemoteFilesWebBrowser.Url = (New System.Uri(_UNCDirectoryOnRemotePC.FullName))
			Me.UpADir_LinkLabel.Visible = True : Me.RemoteFilesGroupBox.Enabled = True

		Catch _Ex As Exception
			MsgBox("A connection to the Remote Host could not be established:" & vbCrLf & vbCrLf & _Ex.Message, MsgBoxStyle.Critical)
		Finally

			REM Dismiss the ActionInProgressDialog
			Try : Me.TheActionInProgressDialog.Dismiss() : Catch : End Try
			Me.Cursor = Forms.Cursors.Default : Me.Enabled = True : Me.BringToFront() : Me.Activate()

		End Try
	End Sub

	Public Sub NavigateUpADir() Handles UpADir_LinkLabel.LinkClicked
		Try
			If (New IO.DirectoryInfo(Me.RemoteUNCPath_TextBox.Text)).Parent Is Nothing Then Throw New Exception("There is no parent directory.")
			Me.RemoteUNCPath_TextBox.Text = (New IO.DirectoryInfo(Me.RemoteUNCPath_TextBox.Text)).Parent.FullName
			Me.OpenRemoteDirectory()
		Catch _Ex As Exception : MsgBox(_Ex.Message, MsgBoxStyle.Critical, "Could not resolve Parent Directory") : End Try
	End Sub

#Region "Close the form on pressing [Esc]"

	Protected Sub Register_CloseOnEscKey_Handler() Handles Me.Load
		Me.KeyPreview = True
		AddHandler Me.KeyDown, Sub(_Sender As Object, _KeyEventArgs As Forms.KeyEventArgs) If _KeyEventArgs.KeyCode = Forms.Keys.Escape Then Me.Close()
	End Sub

	Protected Overrides Function ProcessDialogKey(ByVal keyData As System.Windows.Forms.Keys) As Boolean
		If (Windows.Forms.Form.ModifierKeys = Forms.Keys.None) AndAlso (keyData = Forms.Keys.Escape) Then
			Me.Close() : Return True
		End If
		Return MyBase.ProcessDialogKey(keyData)
	End Function

#End Region

End Class