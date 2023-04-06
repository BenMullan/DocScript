Public Class ActionInProgressDialog

#Region "Original, dysfunctional implementation"

	'Protected Shared WindowThread_ As System.Threading.Thread = Nothing
	'Protected Shared GlobalInstance_ As ActionInProgressDialog = Nothing

	'Public Shared Sub ShowOrUpdate(ByVal _ActionDescription$)

	'	ActionInProgressDialog.Dismiss()

	'	ActionInProgressDialog.WindowThread_ = New System.Threading.Thread(
	'	 Sub()
	'		 ActionInProgressDialog.GlobalInstance_ = New ActionInProgressDialog()
	'		 ActionInProgressDialog.GlobalInstance_.CurrentActionDescription_Label.Text = _ActionDescription
	'		 ActionInProgressDialog.GlobalInstance_.ShowDialog()
	'	 End Sub
	'	 )

	'	ActionInProgressDialog.WindowThread_.Start()

	'End Sub

	'Public Shared Sub Dismiss()

	'	REM Hide() the Window, and Abort() the Thread
	'	If ActionInProgressDialog.GlobalInstance_ IsNot Nothing Then ActionInProgressDialog.GlobalInstance_.InvokeIfRequired(
	'	 Sub()
	'		 ActionInProgressDialog.GlobalInstance_.Hide() : ActionInProgressDialog.GlobalInstance_ = Nothing
	'		 If ActionInProgressDialog.WindowThread_ IsNot Nothing Then ActionInProgressDialog.WindowThread_.Abort()
	'	 End Sub
	'  )

	'End Sub

#End Region

	Public Sub New(ByVal _ActionDescription$)
		Me.InitializeComponent()
		Me.CurrentActionDescription_Label.Text = _ActionDescription
	End Sub

	Public Sub ShowInNewThread()
		System.Threading.Tasks.Task.Factory.StartNew(
		 Sub()
			 Me.ShowDialog()
			 Me.Activate() : Me.BringToFront() : Me.Focus()
		 End Sub)
	End Sub

	Public Sub Dismiss()
		Me.Invoke(CType(Sub() Me.Close(), Action))
	End Sub

	Public Sub UpdateText(ByVal _NewText$)
		While Not Me.IsHandleCreated : System.Threading.Thread.Sleep(50) : End While
		Me.Invoke(CType(
		 Sub()
			 Me.CurrentActionDescription_Label.Text = _NewText
			 Me.Activate() : Me.BringToFront() : Me.Focus()
		 End Sub, Action))
	End Sub

End Class