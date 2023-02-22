Namespace Language.Variables

	Public Module VariableUtilities

		''' <summary>E.g. Takes in "STRING@" and Returns [DSArray(Of DSString)]. The Case of the Input does not matter; it is ToUpper()'d anyway.</summary>
		Public Function GetDSVariableType_FromDataTypeString(ByVal _DataTypeString$) As Type
			Try

				Dim _ResolvedTypeToReturn As Type

				Select Case _DataTypeString.ToUpper()
					Case Constants.DataType_Void : _ResolvedTypeToReturn = GetType(Void)
					Case Constants.DataType_String : _ResolvedTypeToReturn = GetType(DSString)
					Case Constants.DataType_Number : _ResolvedTypeToReturn = GetType(DSNumber)
					Case Constants.DataType_Boolean : _ResolvedTypeToReturn = GetType(DSBoolean)
					Case Constants.DataType_StringArray : _ResolvedTypeToReturn = GetType(DSArray(Of DSString))
					Case Constants.DataType_NumberArray : _ResolvedTypeToReturn = GetType(DSArray(Of DSNumber))
					Case Constants.DataType_BooleanArray : _ResolvedTypeToReturn = GetType(DSArray(Of DSBoolean))
					Case Else : Throw New DSValidationException("The DataType String was not recognised", _DataTypeString)
				End Select

				LogLexingMessage("Resolved DataType String " & _DataTypeString.InSquares() & " to Type " & _ResolvedTypeToReturn.Name.InSquares(), LogEvent.DSEventSeverity.Debug)
				Return _ResolvedTypeToReturn

			Catch _Ex As Exception : Throw New DSException("@GetDSVariableTypeFromDataTypeString: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>E.g. Takes in [DSArray(Of DSString)] and Returns "STRING@". The Output comes from the Language-level Constants, so it UPPERCASE.</summary>
		Public Function GetDataTypeString_FromDSVariableType(ByVal _DSVariableType As Type) As [String]
			Try

				Dim _ResolvedDTStringToReturn$

				Select Case _DSVariableType
					Case GetType(Void) : _ResolvedDTStringToReturn = Constants.DataType_Void
					Case GetType(DSString) : _ResolvedDTStringToReturn = Constants.DataType_String
					Case GetType(DSNumber) : _ResolvedDTStringToReturn = Constants.DataType_Number
					Case GetType(DSBoolean) : _ResolvedDTStringToReturn = Constants.DataType_Boolean
					Case GetType(DSArray(Of DSString)) : _ResolvedDTStringToReturn = Constants.DataType_StringArray
					Case GetType(DSArray(Of DSNumber)) : _ResolvedDTStringToReturn = Constants.DataType_NumberArray
					Case GetType(DSArray(Of DSBoolean)) : _ResolvedDTStringToReturn = Constants.DataType_BooleanArray
					Case Else : Throw New DSValidationException("The Type was not recognised as a DocScript Variable DataType. The Type must be derived from IDataValue", "FullName of unrecognised Type: " & _DSVariableType.FullName)
				End Select

				LogLexingMessage("Resolved DS Variable DataType " & _DSVariableType.Name.InSquares & " to String " & _ResolvedDTStringToReturn.InSquares(), LogEvent.DSEventSeverity.Debug)
				Return _ResolvedDTStringToReturn

			Catch _Ex As Exception : Throw New DSException("@GetDataTypeStringFromDSVariableType: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>Determines if _FunctionReturnType is for a valid IDV, which isn't Void etc. (It must be a String, Number, Boolean, or Array(Of *←)). This is done by an assume-false method.</summary>
		Public ReadOnly Property IsNonVoidFunctionReturnType(ByVal _FunctionReturnType As Type) As Boolean
			Get

				REM Incidentially, these are the only valid Type Modifier Arguments for the LiteralExpr Class. It therefore uses this method.

				Return {
				   GetType(Language.Variables.DSString),
				   GetType(Language.Variables.DSNumber),
				   GetType(Language.Variables.DSBoolean),
				   GetType(Language.Variables.DSArray(Of Language.Variables.DSString)),
				   GetType(Language.Variables.DSArray(Of Language.Variables.DSNumber)),
				   GetType(Language.Variables.DSArray(Of Language.Variables.DSBoolean))
				}.Contains(_FunctionReturnType)

			End Get
		End Property

	End Module

End Namespace