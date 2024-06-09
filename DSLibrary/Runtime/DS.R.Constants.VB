Namespace Runtime

	''' <summary>An Uninheritable, Uninstanciatable Class holding Runtime (Execution) Constants for DocScript</summary>
	Public NotInheritable Class Constants

		'Make this Class uninstanciatable from outside
		Private Sub New()
		End Sub

		REM Programs
		Public Const ProgramExitCode_Default As Int32 = 101	'Used for the Function<Void>Main() EntryPoint

		REM Identifiers
		Public Const EntryPointFunction_Identifier$ = "Main"
		Public Const InputFunction_Identifier$ = "Input"
		Public Const OutputFunction_Identifier$ = "Output"
		Public Const OutputErrorFunction_Identifier$ = "OutputError"

		REM Misc.
		Public Const GlobalSymbolTable_Description$ = "Global Symbol Table"

	End Class

End Namespace