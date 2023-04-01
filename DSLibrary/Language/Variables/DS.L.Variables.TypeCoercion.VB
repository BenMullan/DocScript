Namespace Language.Variables

	''' <summary>DS-TC</summary>
	Public Module TypeCoercion

		Public Function CoerceIDV_IntoTargetIDVType_NoGenericModifier(ByVal _InputIDV As IDataValue, ByVal _TargetOutputType As Type) As IDataValue
			Try

				Dim _CoercedIDV As Object = _
				GetType(TypeCoercion) _
				.GetMethod("CoerceIDV_IntoTargetIDVType") _
				.MakeGenericMethod(_TargetOutputType) _
				.Invoke(Nothing, New Object() {_InputIDV})

				LogLexingMessage("Finished executing a DS-TC Call using .NET Reflection: Attempted to derive a " & _TargetOutputType.Name & " from an Input " & _InputIDV.GetType().Name & ", and what's actually being returned is a " & _CoercedIDV.GetType().Name)

				Return (CType(_CoercedIDV, [IDataValue]))

			Catch _TargetInnvoccationException As System.Reflection.TargetInvocationException : If _TargetInnvoccationException.InnerException IsNot Nothing Then Throw New DSException("(From .NET Reflection calling of CoerceIDV_IntoTargetIDVType) " & _TargetInnvoccationException.InnerException.Message, _TargetInnvoccationException.InnerException) Else Throw _TargetInnvoccationException
			Catch _Ex As Exception When True : Throw New DSException("@CoerceIDV_IntoTargetIDVType_NoGenericModifier", _Ex) : End Try
		End Function

		''' <summary>Use via the Coerce() Compiler Extention.</summary>
		''' <typeparam name="_TSoughtIDV">The DataType that you want to coerce the _InputIDV into</typeparam>
		''' <remarks>DO NOT RENAME THIS FUNCTION; ITS NAME IS HARD_CODED AS A STRING LITERAL IN THE REFLECTION CALL ABOVE</remarks>
		Public Function CoerceIDV_IntoTargetIDVType(Of _TSoughtIDV As {IDataValue})(ByVal _InputIDV As IDataValue) As _TSoughtIDV
			Try

				LogExecutionMessage("Coercing IDataValue of type " & VariableUtilities.GetDataTypeString_FromDSVariableType(_InputIDV.GetType()) & " to type " & VariableUtilities.GetDataTypeString_FromDSVariableType(GetType(_TSoughtIDV)), LogEvent.DSEventSeverity.Verbose)
				If Not (_InputIDV.GetType() = GetType(_TSoughtIDV)) Then LogExecutionMessage("Attempting implicit coercion for value currently of type " & VariableUtilities.GetDataTypeString_FromDSVariableType(_InputIDV.GetType()) & ", into target type " & VariableUtilities.GetDataTypeString_FromDSVariableType(GetType(_TSoughtIDV)) & "."c, LogEvent.DSEventSeverity.Warning)

				REM	"What is the desired Type for the Output IDV?"
				Select Case GetType(_TSoughtIDV)

					Case GetType(DSString) 'ToString() Conversion...

						REM	"What is the current Type of the Input IDV"
						Select Case _InputIDV.GetType()
							Case GetType(DSString) : Return CType(_InputIDV, _TSoughtIDV) 'No Change
							Case GetType(DSNumber) : Return CType(CObj(New DSString(_InputIDV.ToString())), _TSoughtIDV) '"10.2468"
							Case GetType(DSBoolean) : Return CType(CObj(New DSString(_InputIDV.ToString())), _TSoughtIDV) '"True"
							Case GetType(DSArray(Of DSString)) : Return CType(CObj(New DSString(_InputIDV.ToString())), _TSoughtIDV) '"{"Name1", "Name2", "Name3"}"
							Case GetType(DSArray(Of DSNumber)) : Return CType(CObj(New DSString(_InputIDV.ToString())), _TSoughtIDV) '"{1, 2, 3}"
							Case GetType(DSArray(Of DSBoolean)) : Return CType(CObj(New DSString(_InputIDV.ToString())), _TSoughtIDV) '"{True, True, False}"
							Case Else : Throw New DSValidationException("The Type for the Input IDV is not valid.", "_InputIDV's Type: " & _InputIDV.GetType().FullName)
						End Select

					Case GetType(DSNumber)

						REM	"What is the current Type of the Input IDV"
						Select Case _InputIDV.GetType()
							Case GetType(DSString)

								REM String-to-Number Parsing
								If DSNumber.IsValidNumericLiteralString(_InputIDV.ToString()) Then
									LogDebugMessage("The Input IDV is a valid Numeric Literal String: " & _InputIDV.ToString(), LogEvent.DSEventCatagory.Execution)
									Return CType(CObj(DSNumber.FromNumericLiteralString(_InputIDV.ToString())), _TSoughtIDV)
								Else
									'Try/Parse the String into a Number
									LogDebugMessage("The Input IDV is NOT a valid Numeric Literal String: " & _InputIDV.ToString(), LogEvent.DSEventCatagory.Execution)
									Throw New DSRefusedToCoerceToTypeException(_InputIDV, "DSNumber", "The String was not a valid DocScript Numeric Literal.")
								End If

							Case GetType(DSNumber) : Return CType(_InputIDV, _TSoughtIDV) 'No Change
							Case GetType(DSBoolean) : Throw New DSRefusedToCoerceToTypeException(_InputIDV, "DSNumber")
							Case GetType(DSArray(Of DSString)) : Throw New DSRefusedToCoerceToTypeException(_InputIDV, "DSNumber")
							Case GetType(DSArray(Of DSNumber)) : Throw New DSRefusedToCoerceToTypeException(_InputIDV, "DSNumber")
							Case GetType(DSArray(Of DSBoolean)) : Throw New DSRefusedToCoerceToTypeException(_InputIDV, "DSNumber")
							Case Else : Throw New DSValidationException("The Type for the Input IDV is not valid.", "_InputIDV's Type: " & _InputIDV.GetType().FullName)
						End Select

					Case GetType(DSBoolean)

						REM	"What is the current Type of the Input IDV"
						Select Case _InputIDV.GetType()
							Case GetType(DSString) : Throw New DSRefusedToCoerceToTypeException(_InputIDV, "DSBoolean")
							Case GetType(DSNumber) : Throw New DSRefusedToCoerceToTypeException(_InputIDV, "DSBoolean")
							Case GetType(DSBoolean) : Return CType(_InputIDV, _TSoughtIDV) 'No Change
							Case GetType(DSArray(Of DSString)) : Throw New DSRefusedToCoerceToTypeException(_InputIDV, "DSBoolean")
							Case GetType(DSArray(Of DSNumber)) : Throw New DSRefusedToCoerceToTypeException(_InputIDV, "DSBoolean")
							Case GetType(DSArray(Of DSBoolean)) : Throw New DSRefusedToCoerceToTypeException(_InputIDV, "DSBoolean")
							Case Else : Throw New DSValidationException("The Type for the Input IDV is not valid.", "_InputIDV's Type: " & _InputIDV.GetType().FullName)
						End Select

					Case GetType(DSArray(Of DSString))

						REM	"What is the current Type of the Input IDV"
						Select Case _InputIDV.GetType()
							Case GetType(DSString) : Throw New DSRefusedToCoerceToTypeException(_InputIDV, "DSArray(Of DSString)")
							Case GetType(DSNumber) : Throw New DSRefusedToCoerceToTypeException(_InputIDV, "DSArray(Of DSString)")
							Case GetType(DSBoolean) : Throw New DSRefusedToCoerceToTypeException(_InputIDV, "DSArray(Of DSString)")
							Case GetType(DSArray(Of DSString)) : Return CType(_InputIDV, _TSoughtIDV) 'No Change
							Case GetType(DSArray(Of DSNumber)) : Throw New DSRefusedToCoerceToTypeException(_InputIDV, "DSArray(Of DSString)")
							Case GetType(DSArray(Of DSBoolean)) : Throw New DSRefusedToCoerceToTypeException(_InputIDV, "DSArray(Of DSString)")
							Case Else : Throw New DSValidationException("The Type for the Input IDV is not valid.", "_InputIDV's Type: " & _InputIDV.GetType().FullName)
						End Select

					Case GetType(DSArray(Of DSNumber))

						REM	"What is the current Type of the Input IDV"
						Select Case _InputIDV.GetType()
							Case GetType(DSString) : Throw New DSRefusedToCoerceToTypeException(_InputIDV, "DSArray(Of DSNumber)")
							Case GetType(DSNumber) : Throw New DSRefusedToCoerceToTypeException(_InputIDV, "DSArray(Of DSNumber)")
							Case GetType(DSBoolean) : Throw New DSRefusedToCoerceToTypeException(_InputIDV, "DSArray(Of DSNumber)")
							Case GetType(DSArray(Of DSString)) : Throw New DSRefusedToCoerceToTypeException(_InputIDV, "DSArray(Of DSNumber)")
							Case GetType(DSArray(Of DSNumber)) : Return CType(_InputIDV, _TSoughtIDV) 'No Change
							Case GetType(DSArray(Of DSBoolean)) : Throw New DSRefusedToCoerceToTypeException(_InputIDV, "DSArray(Of DSNumber)")
							Case Else : Throw New DSValidationException("The Type for the Input IDV is not valid.", "_InputIDV's Type: " & _InputIDV.GetType().FullName)
						End Select

					Case GetType(DSArray(Of DSBoolean))

						REM	"What is the current Type of the Input IDV"
						Select Case _InputIDV.GetType()
							Case GetType(DSString) : Throw New DSRefusedToCoerceToTypeException(_InputIDV, "DSArray(Of DSBoolean)")
							Case GetType(DSNumber) : Throw New DSRefusedToCoerceToTypeException(_InputIDV, "DSArray(Of DSBoolean)")
							Case GetType(DSBoolean) : Throw New DSRefusedToCoerceToTypeException(_InputIDV, "DSArray(Of DSBoolean)")
							Case GetType(DSArray(Of DSString)) : Throw New DSRefusedToCoerceToTypeException(_InputIDV, "DSArray(Of DSBoolean)")
							Case GetType(DSArray(Of DSNumber)) : Throw New DSRefusedToCoerceToTypeException(_InputIDV, "DSArray(Of DSBoolean)")
							Case GetType(DSArray(Of DSBoolean)) : Return CType(_InputIDV, _TSoughtIDV) 'No Change
							Case Else : Throw New DSValidationException("The Type for the Input IDV is not valid.", "_InputIDV's Type: " & _InputIDV.GetType().FullName)
						End Select

					Case Else : Throw New DSValidationException("The specified output Type for the IDV Coercion is not valid.", "_TSoughtIDV: " & GetType(_TSoughtIDV).FullName)
				End Select

			Catch _DSRefusedToCoerceToTypeException As DSRefusedToCoerceToTypeException : Throw _DSRefusedToCoerceToTypeException
			Catch _Ex As Exception : Throw New DSException("@CoerceIDV_IntoTargetIDVType: " & _Ex.Message, _Ex) : End Try
		End Function

	End Module

End Namespace