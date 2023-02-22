Namespace Runtime

	''' <summary>Represents a Segmented String with a TokenType, from the Source of a DocScript Program</summary>
	Public Class Token

		''' <summary>Represents the Catagorical Vareity of a Token</summary>
		Public Enum TokenType As UInt16
			Unresolved = 0
			StringLiteral = 1
			NumericLiteral = 2
			BooleanLiteral = 3
			Keyword = 4
			DataType = 5
			Identifier = 6
			DSOperator = 7
			GrammarChar = 8
			LineEnd = 9
			StatmentEnd = 10
		End Enum

		''' <summary>Represents the original location of a Token in the Source</summary>
		Public Structure TokenLocation

			Public ReadOnly Line As UInt16, Column As UInt16

			Public Sub New(ByVal _Line As UInt16, ByVal _Column As UInt16)
				Me.Line = _Line : Me.Column = _Column
			End Sub

			''' <summary>Returns a KVPSerialised String to represent the Data of the current Object</summary>
			Public Overrides Function ToString() As String
				Return Utilities.KVPSerialisation.GetKVPString({"Line", Me.Line.ToString()}, {"Column", Me.Column.ToString()})
			End Function

			''' <summary>Returns a Shortened String, in the Form (E.g.) [14,7]</summary>
			Public Function ToShortString() As String
				Return String.Format("[{0},{1}]", Me.Line.ToString(), Me.Column.ToString())
			End Function

		End Structure

		Public ReadOnly Value As String
		Public ReadOnly Type As TokenType
		Public ReadOnly LocationInSource As TokenLocation

		Public Sub New(ByVal _Value$, ByVal _Type As TokenType, ByVal _LocationLine As UInt16, ByVal _LocationColumn As UInt16)
			Me.Value = _Value
			Me.Type = _Type
			Me.LocationInSource = (New Token.TokenLocation(_LocationLine, _LocationColumn))
			'LogDebugMessage("Instanciated a New Token with Value [" & Me.Value & "] and Location [" & Me.LocationInSource.ToShortString() & "]", LogEvent.DSEventCatagory.Parsing)
		End Sub

		''' <summary>Returns a KVPSerialised String to represent the Data of the current Object</summary>
		Public Overrides Function ToString() As String
			Return Utilities.KVPSerialisation.GetKVPString({"Value", Me.Value}, {"Type", Me.Type.ToString()}, {"LocationInSource", Me.LocationInSource.ToString()})
		End Function

	End Class

End Namespace