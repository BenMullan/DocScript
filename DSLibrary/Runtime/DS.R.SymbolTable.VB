Namespace Runtime

	''' <summary>Holds a dictionary of Identifiers to [SymbolTableEntry]s. All Identifiers are ToUpper()-ed.</summary>
	Public Class SymbolTable

		''' <summary>Implemented by all derivatives of SymbolTableEntry(Of TValue), no matter what Type of Value they hold</summary>
		Public Interface ISymbolTableEntry
			Sub UpdateValue(ByVal _NewValue As ISymbolTableValue)
			Function Value_ToString() As [String]
		End Interface

		''' <summary>Implemented by: DSString, DSNumber, DSBoolean, DSArray(Of *), DSFunction, BuiltInFunction</summary>
		Public Interface ISymbolTableValue : End Interface

		''' <summary>Represents an Entry in a SymbolTable with a Value, FlagSet, and - implicitly - a Value Type.
		''' NOTE THAT THE IDENTIFIER FOR THE ENTRY IS STORED IN THE SYMBOLTABLE AS A DICTIONARY KEY.</summary>
		Public Class SymbolTableEntry(Of TValue As {ISymbolTableValue}) : Implements ISymbolTableEntry

			REM To be BitWise Or-ed together if needed
			Public Enum SymbolTableEntryFlags As Byte
				[None] = 0
				[DSFunctionParameter] = 1 'Nessecary because we'd have no way of knowing if the Value is for an Argument or a Local Variable if it's <Number> or <String> etc...
			End Enum

			Public ReadOnly Flags As SymbolTableEntryFlags = SymbolTableEntry(Of TValue).SymbolTableEntryFlags.None
			Public Value As TValue

			Public Sub New(ByVal _Value As TValue, Optional ByVal _Flags As SymbolTableEntryFlags = SymbolTableEntry(Of TValue).SymbolTableEntryFlags.None)
				Me.Flags = _Flags : Me.Value = _Value
			End Sub

			Public Sub UpdateValue(ByVal _NewValue As ISymbolTableValue) Implements ISymbolTableEntry.UpdateValue
				Try

					REM Ensure the _NewValue has the same ISymbolTable Type as the existing one
					REM (In other words, we mustn't be updating the value of an existing <Number>, to a DSFunction)
					If Not (Me.Value.GetType() = _NewValue.GetType()) Then Throw New DSValidationException("The _NewValue for the Entry is not of the same DataType as that of the existing value", "TypeOf _NewValue: " & _NewValue.GetType().Name.InSquares() & "; TypeOf Existing Value: " & Me.Value.GetType().Name.InSquares())

					Me.Value = CType(_NewValue, TValue)

				Catch _Ex As Exception : Throw New DSException("@SymbolTableEntry\UpdateValue: " & _Ex.Message, _Ex) : End Try
			End Sub

			''' <summary>Returns a KVP String to represent the Object</summary>
			Public Overrides Function ToString() As String
				Return DocScript.Utilities.KVPSerialisation.GetKVPString({"ValueType", Me.Value.GetType().Name}, {"Flags", Convert.ToString(CByte(Me.Flags), 2)})
			End Function

			''' <summary>Returns the ToString() of the Value. Implemented in the Interface, so that a preview of the Value can be retreived, even when the Type of the Value is unknown.</summary>
			Public Function Value_ToString() As String Implements ISymbolTableEntry.Value_ToString
				Return Me.Value.ToString()
			End Function

		End Class '</SymbolTableEntry>

		REM The Identifiers must be ToUpper()-ed
		Protected Entries_ As New Dictionary(Of String, ISymbolTableEntry)()
		Public ReadOnly Description$ 'Initialised via the Public-facing Constructor

		REM The fact that the [SymbolTableEntry]s are stored in a Dictionary, means that there cannot - by design - be two identical identifiers
		Public Sub New(ByVal _Description$)
			Me.Description = _Description
			LogExecutionMessage("Instanciated a new SymbolTable with Description " & Me.Description.InSquares())
		End Sub

		''' <summary>Used for external components who want to e.g. see if the SymTbl Contains() a specified Key, or see how many Entries there are</summary>
		Public ReadOnly Property Entries_ReadOnlyCopy As ObjectModel.ReadOnlyCollection(Of KeyValuePair(Of String, ISymbolTableEntry))
			Get
				Return (New ObjectModel.ReadOnlyCollection(Of KeyValuePair(Of String, ISymbolTableEntry))(Me.Entries_.ToList()))
			End Get
		End Property

		''' <summary>Generates a new SymTblsSnpSht containing just the current SymbolTable (as the Topmost SymTbl)</summary>
		Public ReadOnly Property SnapshotContainingJustThis As SymbolTablesSnapshot
			Get
				Return New SymbolTablesSnapshot(SymbolTablesSnapshot.Empty, Me)
			End Get
		End Property

		''' <summary>The _Identifier is ToUpper()-ed when added to the SymbolTable</summary>
		Public Sub AddEntry(ByVal _Identifier$, ByVal _Entry As ISymbolTableEntry)
			Try : LogExecutionMessage("Adding Entry with Identifier " & _Identifier.InSquares() & " and SymbolTableValue of " & _Entry.ToString() & " into SymbolTable " & Me.Description.InSquares())

				REM Ensure the SymbolTable dosen't already contain an Entry with the specified _Identifier
				If Me.Entries_.ContainsKey(_Identifier.ToUpper()) Then Throw New DSDuplicateSymbolException(_Identifier, "SymbolTable Description: " & Me.Description)

				REM Ensure that the _Entry IsNot Nothing
				_Entry.MustNotBeNothing(_MsgIfNull:="The _Entry cannot be Nothing")

				REM We now know that we're adding an Entry with the specified Identifier for the first time.
				Me.Entries_.Add(_Identifier.ToUpper(), _Entry)

			Catch _Ex As Exception : Throw New DSException("@SymbolTable\AddEntry: " & _Ex.Message, _Ex) : End Try
		End Sub

		Public Function GetEntry(ByVal _Identifier$) As ISymbolTableEntry
			Try : LogExecutionMessage("Getting SymbolTable Entry for " & _Identifier.InSquares() & " in SymbolTable " & Me.Description.InSquares())

				REM Ensure the SymbolTable contains an Entry with the specified _Identifier
				If Not Me.Entries_.ContainsKey(_Identifier.ToUpper()) Then Throw New [DSNonexistentSymbolException](_Identifier, "SymbolTable Description: " & Me.Description)

				Return Me.Entries_.Item(_Identifier.ToUpper())

			Catch _Ex As Exception : Throw New DSException("@SymbolTable\GetEntry: " & _Ex.Message, _Ex) : End Try
		End Function

		Public Sub UpdateEntry(ByVal _Identifier$, ByVal _NewValue As ISymbolTableValue)
			Try : LogExecutionMessage("Updating Value of " & _Identifier.InSquares() & " to new value of Type" & _NewValue.GetType().Name.InSquares() & " in SymbolTable " & Me.Description.InSquares())

				REM Ensure the SymbolTable contains an Entry with the specified _Identifier
				If Not Me.Entries_.ContainsKey(_Identifier.ToUpper()) Then Throw New [DSNonexistentSymbolException](_Identifier, "SymbolTable Description: " & Me.Description)

				REM Make the Entry equal to a New instance of itself, but with the _NewValue
				Me.Entries_.Item(_Identifier.ToUpper()).UpdateValue(_NewValue)

			Catch _Ex As Exception : Throw New DSException("@SymbolTable\UpdateEntry: " & _Ex.Message, _Ex) : End Try
		End Sub

		''' <summary>There must contain the same number of Entries, and must have the same Identifiers and Value Types; only the Values can be different</summary>
		Public ReadOnly Property AlignsWith(ByVal _OtherSymTbl As SymbolTable) As Boolean
			Get
				LogExecutionMessage("Checking individual SymbolTable Alignment")

				'There must contain the same number of Entries, and must have the same Identifiers and Value Types; only the Values can be different
				If Not (Me.Entries_.Count = _OtherSymTbl.Entries_.Count) Then Return False
				For _EntryIndex% = 0 To (_OtherSymTbl.Entries_.Count - 1) Step +1

					'	If the other SymTbl dosen't contain {current one of my Keys},
					'AND the Value's Type for this Key is the same as *My* Value's Type for this Key,
					'Then there is an Alignment Mismatch"
					If Not (_OtherSymTbl.Entries_.ContainsKey(Me.Entries_.ElementAt(_EntryIndex).Key) _
					 AndAlso (_OtherSymTbl.Entries_.Item(Me.Entries_.ElementAt(_EntryIndex).Key).GetType() = Me.Entries_.ElementAt(_EntryIndex).Value.GetType())) _
					 Then Return False

				Next

				Return True

			End Get
		End Property

		''' <summary>Produces a DS-KVP Human-readable String for the Object</summary>
		Public Overrides Function ToString() As String
			Return DocScript.Utilities.KVPSerialisation.GetKVPString({"Description", Me.Description}, {"EntriesCount", Me.Entries_.Count.ToString()})
		End Function

#Region "(Shared) Entry Type-Checking Functions"

		Public Shared ReadOnly Property IsFunctionEntry(ByVal _Entry As ISymbolTableEntry) As Boolean
			Get

				Return {
				   GetType(SymbolTableEntry(Of Language.Instructions.Statements.DSFunction)),
				   GetType(SymbolTableEntry(Of Runtime.BuiltInFunction))
				}.Contains(_Entry.GetType())

			End Get
		End Property

		Public Shared ReadOnly Property IsVariableEntry(ByVal _Entry As ISymbolTableEntry) As Boolean
			Get

				Return {
				 GetType(SymbolTableEntry(Of Language.Variables.DSString)),
				 GetType(SymbolTableEntry(Of Language.Variables.DSNumber)),
				 GetType(SymbolTableEntry(Of Language.Variables.DSBoolean)),
				 GetType(SymbolTableEntry(Of Language.Variables.DSArray(Of Language.Variables.DSString))),
				 GetType(SymbolTableEntry(Of Language.Variables.DSArray(Of Language.Variables.DSNumber))),
				 GetType(SymbolTableEntry(Of Language.Variables.DSArray(Of Language.Variables.DSBoolean)))
				}.Contains(_Entry.GetType())

			End Get
		End Property

#End Region

		''' <summary>Returns the IDataValue-based Variable Value from inside the SymbolTableEntry</summary>
		Public Shared Function ExtractVariableValue_FromSymbolTableEntry(ByVal _SymTblEntry As Runtime.SymbolTable.ISymbolTableEntry, Optional ByVal _EntryIdentifier$ = "(Unspecified)") As Language.Variables.IDataValue
			Try : LogExecutionMessage("Extracting Variable Value from SymbolTableEntry of Type " & _SymTblEntry.GetType().Name)

				If Not Runtime.SymbolTable.IsVariableEntry(_SymTblEntry) Then Throw New DSIncorrectSymbolTableEntryTypeException(_EntryIdentifier, "An IDataValue Variable Type".InSquares(), _SymTblEntry.GetType().FullName, "The Variable Value can therefore not be Extracted from the SymTblEntry")

				Select Case _SymTblEntry.GetType()
					Case GetType(Runtime.SymbolTable.SymbolTableEntry(Of Language.Variables.DSString)) : Return CType(_SymTblEntry, Runtime.SymbolTable.SymbolTableEntry(Of Language.Variables.DSString)).Value
					Case GetType(Runtime.SymbolTable.SymbolTableEntry(Of Language.Variables.DSNumber)) : Return CType(_SymTblEntry, Runtime.SymbolTable.SymbolTableEntry(Of Language.Variables.DSNumber)).Value
					Case GetType(Runtime.SymbolTable.SymbolTableEntry(Of Language.Variables.DSBoolean)) : Return CType(_SymTblEntry, Runtime.SymbolTable.SymbolTableEntry(Of Language.Variables.DSBoolean)).Value
					Case GetType(Runtime.SymbolTable.SymbolTableEntry(Of Language.Variables.DSArray(Of Language.Variables.DSString))) : Return CType(_SymTblEntry, Runtime.SymbolTable.SymbolTableEntry(Of Language.Variables.DSArray(Of Language.Variables.DSString))).Value
					Case GetType(Runtime.SymbolTable.SymbolTableEntry(Of Language.Variables.DSArray(Of Language.Variables.DSNumber))) : Return CType(_SymTblEntry, Runtime.SymbolTable.SymbolTableEntry(Of Language.Variables.DSArray(Of Language.Variables.DSNumber))).Value
					Case GetType(Runtime.SymbolTable.SymbolTableEntry(Of Language.Variables.DSArray(Of Language.Variables.DSBoolean))) : Return CType(_SymTblEntry, Runtime.SymbolTable.SymbolTableEntry(Of Language.Variables.DSArray(Of Language.Variables.DSBoolean))).Value
					Case Else : Throw New DSException("Despite passing IsVariableEntry(), the SymbolTable Entry was not of an accounted-for Type. It's Type was: " & _SymTblEntry.GetType().FullName)
				End Select

			Catch _Ex As Exception : Throw New DSException("@ExtractVariableValue_FromSymbolTableEntry: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>
		''' Generates a SymbolTableEntry(Of TheIDVsType) from the _InputIDV
		''' NOTE: If [_InputIDV Is Nothing], Then the relevant InstanceWithNullValue is used, depending on the GetType() of the _InputIDV
		''' </summary>
		''' <param name="_NullValueType_IfInputIDVIsNothing">
		''' If the _InputIDV Is Nothing, then we can't tell what type it is, so we wouldn't know which InstanceWithNullValue to use.
		''' Therefore, we'll have to take in a _NullValueType_IfInputIDVIsNothing to use instead
		''' This argument can be [Nothing], if you're audaciously sure that the _InputIDV isn't [Nothing]
		''' </param>
		Public Shared Function GetSymbolTableEntry_FromIDataValue(ByVal _InputIDV As Language.Variables.IDataValue, ByVal _NullValueType_IfInputIDVIsNothing As Type) As ISymbolTableEntry
			Try : LogExecutionMessage("Generating SymbolTable Entry for IDataValue of Type " & If(_InputIDV Is Nothing, "(_InputIDV Is Nothing; it dosen't have a Type)", _InputIDV.GetType().Name).InSquares() & "...")

				REM If the _InputIDV Is Nothing, then we can't tell what type it is, so we wouldn't know which InstanceWithNullValue to use.
				REM Therefore, we'll have to take in a _NullValueType_IfInputIDVIsNothing to use instead

				If (_InputIDV Is Nothing) Then Return SymbolTable.IDVNullValue_SymbolTableEntry_ForIDVType(_NullValueType_IfInputIDVIsNothing)

				REM If we're here, then the _InputIDV isn't nothing; Generate&&Return a SymbolTableEntry(Of TheIDVsType)

				Dim _ProcuredSymTblEntry As Runtime.SymbolTable.ISymbolTableEntry
				Select Case _InputIDV.GetType()
					Case GetType(Language.Variables.DSString) : _ProcuredSymTblEntry = New Runtime.SymbolTable.SymbolTableEntry(Of Language.Variables.DSString)(_Value:=_InputIDV.Coerce(Of Language.Variables.DSString)())
					Case GetType(Language.Variables.DSNumber) : _ProcuredSymTblEntry = New Runtime.SymbolTable.SymbolTableEntry(Of Language.Variables.DSNumber)(_Value:=_InputIDV.Coerce(Of Language.Variables.DSNumber)())
					Case GetType(Language.Variables.DSBoolean) : _ProcuredSymTblEntry = New Runtime.SymbolTable.SymbolTableEntry(Of Language.Variables.DSBoolean)(_Value:=_InputIDV.Coerce(Of Language.Variables.DSBoolean)())
					Case GetType(Language.Variables.DSArray(Of Language.Variables.DSString)) : _ProcuredSymTblEntry = New Runtime.SymbolTable.SymbolTableEntry(Of Language.Variables.DSArray(Of Language.Variables.DSString))(_Value:=_InputIDV.Coerce(Of Language.Variables.DSArray(Of Language.Variables.DSString))())
					Case GetType(Language.Variables.DSArray(Of Language.Variables.DSNumber)) : _ProcuredSymTblEntry = New Runtime.SymbolTable.SymbolTableEntry(Of Language.Variables.DSArray(Of Language.Variables.DSNumber))(_Value:=_InputIDV.Coerce(Of Language.Variables.DSArray(Of Language.Variables.DSNumber))())
					Case GetType(Language.Variables.DSArray(Of Language.Variables.DSBoolean)) : _ProcuredSymTblEntry = New Runtime.SymbolTable.SymbolTableEntry(Of Language.Variables.DSArray(Of Language.Variables.DSBoolean))(_Value:=_InputIDV.Coerce(Of Language.Variables.DSArray(Of Language.Variables.DSBoolean))())
					Case Else : Throw New DSValidationException("The _InputIDV's DataType was un-[accounted-for]", _InputIDV.GetType().FullName)
				End Select

				LogExecutionMessage("...Generated this SymbolTableEntry: " & _ProcuredSymTblEntry.ToString())
				Return _ProcuredSymTblEntry

			Catch _Ex As Exception : Throw New DSException("@GetSymTblEntry_FromIDataValue: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>
		''' Gets a SymbolTableEntry(Of _IDVType) with the InstanceWithNullValue for the correct IDV Type, via an exhaustive [Select Case]
		''' </summary>
		Public Shared ReadOnly Property IDVNullValue_SymbolTableEntry_ForIDVType(ByVal _IDVType As Type) As ISymbolTableEntry
			Get
				Try : LogExecutionMessage("Generating SymbolTable Entry with InstanceWithNullValue for IDataValue of Type " & _IDVType.Name.InSquares())

					Select Case _IDVType
						Case GetType(Language.Variables.DSString) : Return New Runtime.SymbolTable.SymbolTableEntry(Of Language.Variables.DSString)(Language.Variables.DSString.InstanceWithNullValue)
						Case GetType(Language.Variables.DSNumber) : Return New Runtime.SymbolTable.SymbolTableEntry(Of Language.Variables.DSNumber)(Language.Variables.DSNumber.InstanceWithNullValue)
						Case GetType(Language.Variables.DSBoolean) : Return New Runtime.SymbolTable.SymbolTableEntry(Of Language.Variables.DSBoolean)(Language.Variables.DSBoolean.InstanceWithNullValue)
						Case GetType(Language.Variables.DSArray(Of Language.Variables.DSString)) : Return New Runtime.SymbolTable.SymbolTableEntry(Of Language.Variables.DSArray(Of Language.Variables.DSString))(Language.Variables.DSArray(Of Language.Variables.DSString).InstanceWithNullValue)
						Case GetType(Language.Variables.DSArray(Of Language.Variables.DSNumber)) : Return New Runtime.SymbolTable.SymbolTableEntry(Of Language.Variables.DSArray(Of Language.Variables.DSNumber))(Language.Variables.DSArray(Of Language.Variables.DSNumber).InstanceWithNullValue)
						Case GetType(Language.Variables.DSArray(Of Language.Variables.DSBoolean)) : Return New Runtime.SymbolTable.SymbolTableEntry(Of Language.Variables.DSArray(Of Language.Variables.DSBoolean))(Language.Variables.DSArray(Of Language.Variables.DSBoolean).InstanceWithNullValue)
						Case Else : Throw New DSValidationException("The _IDVType was un-[accounted-for]", "_IDVType: " & _IDVType.FullName)
					End Select

				Catch _Ex As Exception : Throw New DSException("@IDVNullValue_SymbolTableEntry_ForIDVType: " & _Ex.Message, _Ex) : End Try
			End Get
		End Property

	End Class

End Namespace