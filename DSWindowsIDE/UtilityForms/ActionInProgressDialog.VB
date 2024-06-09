Public Class ActionInProgressDialog

	'Protected IsCurrentlyShownAsDialog As [Boolean] = False

	Public Sub New(ByVal _ActionDescription$)
		Me.InitializeComponent()
		Me.CurrentActionDescription_Label.Text = _ActionDescription
	End Sub

	Public Sub ShowInNewThread()
		System.Threading.Tasks.Task.Factory.StartNew(
		 Sub()
			 Try
				 Me.ShowDialog()
				 Me.Activate() : Me.InvokeIfRequired(AddressOf Me.BringToFront) : Me.Focus()
			 Catch : End Try
		 End Sub
		 )
	End Sub

	Public Sub Dismiss()
		Try : Me.Invoke(CType(
		  Sub()
			  Me.Close() ': Me.IsCurrentlyShownAsDialog = False
		  End Sub, Action)) : Catch : End Try
	End Sub

	Public Sub UpdateText(ByVal _NewText$)

		If Not Me.Visible Then Me.ShowInNewThread()

		While Not Me.IsHandleCreated
			System.Threading.Thread.Sleep(50)
			Try

				Me.Invoke(CType(
				  Sub()
					  Me.TopMost = True : Me.Show() : Me.Activate() : Me.BringToFront() : Me.Focus()
				  End Sub, Action))

			Catch _Ex As Exception : Debug.WriteLine("Could not bring ActionInProgressDialog to front") : End Try
		End While

		Me.Invoke(CType(
		 Sub()
			 Me.CurrentActionDescription_Label.Text = _NewText
			 Me.TopMost = True : Me.Show() : Me.Activate() : Me.BringToFront() : Me.Focus()
		 End Sub, Action))

	End Sub

End Class