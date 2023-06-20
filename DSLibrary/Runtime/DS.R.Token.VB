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

		''' <summary>Constructs a new Token from a Value string, TokenType, and Location-in-source</summary>
		Public Sub New(ByVal _Value$, ByVal _Type As TokenType, ByVal _LocationLine As UInt16, ByVal _LocationColumn As UInt16)
			Me.Value = _Value : Me.Type = _Type
			Me.LocationInSource = (New Token.TokenLocation(_LocationLine, _LocationColumn))
		End Sub

		''' <summary>SideLoading Constructor: </summary>
		Public Shared Function FromXML(ByVal _TokenXML As XElement) As Token
			Try
				Return New Token(
				 _Value:=_TokenXML.@Value,
				 _Type:=CType([Enum].Parse(GetType(Token.TokenType), _TokenXML.@Type), Token.TokenType),
				 _LocationLine:=Convert.ToUInt16(_TokenXML.@LocationInSource.Split(","c).ElementAt(0)),
				 _LocationColumn:=Convert.ToUInt16(_TokenXML.@LocationInSource.Split(","c).ElementAt(1))
				)
			Catch _Ex As Exception : Throw New DSException("@Token\FromXML: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>Returns a KVPSerialised String to represent the Data of the current Object</summary>
		Public Overrides Function ToString() As String
			Return Utilities.KVPSerialisation.GetKVPString({"Value", Me.Value}, {"Type", Me.Type.ToString()}, {"LocationInSource", Me.LocationInSource.ToString()})
		End Function

		''' <summary>Returns a KVPSerialised XML Object with the members of the current object</summary>
		Public Function ToXML() As XElement
			Return Utilities.KVPSerialisation.GetKVPXML(
			 GetType(Token).Name,
			 {"Value", Me.Value},
			 {"Type", Me.Type.ToString()},
			 {"LocationInSource", [String].Format("{0},{1}", Me.LocationInSource.Line.ToString(), Me.LocationInSource.Column.ToString())}
			)
		End Function

	End Class

End Namespace