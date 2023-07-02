Public NotInheritable Class DSAboutBox

	Private Sub DSAboutBox_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

		REM Title
		Dim _ApplicationTitle$ = If(
			Not [String].IsNullOrEmpty(My.Application.Info.Title),
			My.Application.Info.Title,
			System.IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
		)
		Me.Text = [String].Format("About {0}", _ApplicationTitle)

		REM Other Information
		Me.LabelProductName.Text = My.Application.Info.ProductName
		Me.LabelVersion.Text = String.Format("Version {0}", My.Application.Info.Version.ToString)
		Me.LabelCopyright.Text = My.Application.Info.Copyright
		Me.LabelCompanyName.Text = My.Application.Info.CompanyName
		Me.TextBoxDescription.Text = My.Application.Info.Description

		REM Close on [Ok]
		AddHandler Me.OKButton.Click, Sub() Me.Close()

	End Sub

End Class