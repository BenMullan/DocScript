Namespace Runtime

	''' <summary>Represents a Function accessible from, but not defined within, a DocScript Program</summary>
	Public Class BuiltInFunction : Implements DocScript.Runtime.SymbolTable.ISymbolTableValue

		Public Delegate Function BuiltInFunctionDelegate(ByVal _SymTbls As SymbolTablesSnapshot, ByVal _Arguments As DocScript.Language.Variables.IDataValue()) As Language.Instructions.ExecutionResult

		Public ReadOnly [Identifier] As String
		Public ReadOnly [ExpectedParameters] As ObjectModel.ReadOnlyCollection(Of DocScript.Language.Instructions.Statements.DSFunction.Parameter) 'Struct \w an Ident. and a Type
		Public ReadOnly [ReturnType] As Type
		Public ReadOnly [Action] As BuiltInFunctionDelegate

		'To be set from within an Inline With Statement following a Construction Expression
		Public [Description] As String = "(No Description Specified)"

		''' <summary>Remember to Initilise the Descrtiption by means of an Inline With Statement {.Description = "..."}</summary>
		Public Sub New(ByVal _Identifier As String, ByVal _ExpectedParameters As Language.Instructions.Statements.DSFunction.Parameter(), ByVal _ReturnType As Type, ByVal _Action As BuiltInFunctionDelegate)

			REM All Parameter Identifiers should be unique. This check is really just to ensure that the BIF has been correctly structured in the Initialiser Values passed to the ctor
			If Not _ExpectedParameters.Select(Of String)(Function(_Param As Language.Instructions.Statements.DSFunction.Parameter) _Param.Identifier.ToUpper()).ToArray().AllElementsAreUnique() Then Throw New DSValidationException("Each Parameter of the BuiltInFunction should really have a unique identifier, but this was not the case.", "The Parameter Identifiers were: " & _ExpectedParameters.Select(Of String)(Function(_Param As Language.Instructions.Statements.DSFunction.Parameter) _Param.Identifier.ToUpper()).ToArray().GetStandardArraySerialisation())

			Me.Identifier = _Identifier : Me.Action = _Action : Me.ReturnType = _ReturnType : Me.ExpectedParameters = New ObjectModel.ReadOnlyCollection(Of Language.Instructions.Statements.DSFunction.Parameter)(_ExpectedParameters)
			LogExecutionMessage(String.Format("Constructed a New BuiltInFunction with Identifier {0} and {1} ExpectedParameter(s)", Me.Identifier, Me.ExpectedParameters.Count.ToString()), LogEvent.DSEventSeverity.Debug)

		End Sub

		''' <summary>Automatically ensures that the correct number of _Arguments have been provided, in accordance with the .ExpectedParameters Property.</summary>
		Public Function Run(ByVal _InputSymbolTablesState As Runtime.SymbolTablesSnapshot, ByVal _Arguments As Language.Variables.IDataValue()) As Language.Instructions.ExecutionResult
			Try

				REM Ensure the number of Arguments specified are correct for the BuiltInFunction in question
				If Not _Arguments.Length = Me.ExpectedParameters.Count Then Throw New DSException(String.Format("The number of Arguments provided to the BuiltInFunction ({0}) was not equal to the number of expected Parameters ({1})", _Arguments.Length.ToString(), Me.ExpectedParameters.Count.ToString()))

				'There must be the same number of Arguments as Parameters,
				'	BUT: Their Types don't have to be the same; we'll Coerce them...

				REM !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
				REM It is the duty of the BIF's .NET Delegate to Coerce() the IDV into the correct type
				REM !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

				Dim _BIFAction_ExeRes As Language.Instructions.ExecutionResult = Me.Action.Invoke(_InputSymbolTablesState, _Arguments)

				'If this current BIF has been declared to return a value, then Ensure that it does:
				If (Me.ReturnType <> GetType(Language.Variables.Void)) AndAlso (_BIFAction_ExeRes.ReturnStatus.BuiltInFunction_ReturnValue Is Nothing) Then Throw New DSNoReturnValueException(Me.Identifier, "The BIF's .NET Action Delegate did not return a BIF ReturnValue in its ExeRes")

				Return _BIFAction_ExeRes

			Catch _Ex As Exception : Throw New DSBuiltInFunctionException(Me.Identifier, _Ex) : End Try
		End Function

		''' <summary>Contains an UPPERCASE Identifier, ParameterTypes Array, and ReturnType Property</summary>
		Public ReadOnly Property EffectiveSignature As Language.Instructions.Statements.DSFunction.Signature
			Get
				Return New Language.Instructions.Statements.DSFunction.Signature(
				 _Identifier:=Me.Identifier,
				 _ParameterTypes:=Me.ExpectedParameters.Select(Of Type)(Function(_Param As Language.Instructions.Statements.DSFunction.Parameter) _Param.DataType).ToArray(),
				 _ReturnType:=Me.ReturnType
				 )
			End Get
		End Property

		''' <summary>Returns E.g. Output(_Text)</summary>
		Public ReadOnly Property TemplateCall As [String]
			Get

				Return _
				 Me.Identifier &
				 Language.Constants.OpeningFunctionBracket &
				 String.Join(
				  separator:=Language.Constants.FunctionArgumentSeperator & Language.Constants.Space,
				  values:=Me.ExpectedParameters.Select(Of String)(Function(_BIFParameter As Language.Instructions.Statements.DSFunction.Parameter) _BIFParameter.Identifier)
				  ) &
				 Language.Constants.ClosingFunctionBracket

			End Get
		End Property

		''' <summary>Produces a DS-KVP Human-readable String for the Object</summary>
		Public Overrides Function ToString() As String
			Return DocScript.Utilities.KVPSerialisation.GetKVPString({"Identifier", Me.Identifier}, {"Description", Me.Description}, {"ExpectedParameters.Count", Me.ExpectedParameters.Count.ToString()}, {"ReturnType", Me.ReturnType.Name}, {"TemplateCall", Me.TemplateCall}, {"Action", Me.Action.ToString()}, {"Action.Method.Name", Me.Action.Method.Name})
		End Function

	End Class

End Namespace