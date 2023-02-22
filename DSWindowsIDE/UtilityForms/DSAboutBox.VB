Public NotInheritable Class DSAboutBox

	Private Sub DSAboutBox_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

		REM Title
		Dim ApplicationTitle As String
		If My.Application.Info.Title <> "" Then : ApplicationTitle = My.Application.Info.Title
		Else : ApplicationTitle = System.IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
		End If : Me.Text = String.Format("About {0}", ApplicationTitle)

		REM Other Information
		Me.LabelProductName.Text = My.Application.Info.ProductName
		Me.LabelVersion.Text = String.Format("Version {0}", My.Application.Info.Version.ToString)
		Me.LabelCopyright.Text = My.Application.Info.Copyright
		Me.LabelCompanyName.Text = My.Application.Info.CompanyName
		Me.TextBoxDescription.Text = My.Application.Info.Description

	End Sub

	Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKButton.Click
		Me.Close()
	End Sub

End Class