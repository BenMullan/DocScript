''' <summary>The default host Window for the IDEWindowContent Control</summary>
Public Class MainWindow

	''' <summary>Initialises the Window, making the hosted IDE-Content-Pane point back to this MainWindow</summary>
	Public Sub New()
		Me.InitializeComponent()
		Me.HostedIDEWindowContent.ParentWPFWindow = Me
	End Sub

	''' <summary>Passthrough for Alt + F4 or the [x] button</summary>
	Protected Overrides Sub OnClosing(ByVal _CancelEventArgs As System.ComponentModel.CancelEventArgs)
		Me.HostedIDEWindowContent.OnClosingAction_Passthrough.Invoke(_CancelEventArgs)
	End Sub

End Class