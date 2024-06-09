Namespace Utilities

	''' <summary>
	''' Provides methods for Key-Value Pair Serialisation
	''' </summary>
	Public Module KVPSerialisation

		''' <summary>Returns a Serialised String form of the Object's Key-Value Pairs specified</summary>
		Public Function GetKVPString(ByVal ParamArray _KeyValuePairs As String()()) As String

			REM Input:	{{"Line", "18"}, {"Column", "9"}}
			REM Output:	[ Line="18", Column="9"]

			If _KeyValuePairs.Length < 1 Then Throw New DSValidationException("The _KeyValuePairs 2-D Array contained fewer than one item(s). It must contain at least one array.", "_KeyValuePairs")
			If _KeyValuePairs.Any(Function(_KVP As [String]()) Not (_KVP.Length = 2)) Then Throw New DSValidationException("At least one of the Arrays in the _KeyValuePairs Array did not have two items in it. The Array should be in the form {{""Line"", ""18""}, {""Column"", ""9""}}""", "_KeyValuePairs")

			Return "[ " & String.Join(", ", (From _KVP As String() In _KeyValuePairs Select _KVP.ElementAt(0) & "=""" & If(_KVP(1) IsNot Nothing, _KVP.ElementAt(1).Replace(""""c, "{Quote}"), "(Nothing)") & """")) & " ]"

		End Function

		''' <summary>Returns a Serialised XML form of the Object's Key-Value Pairs specified</summary>
		Public Function GetKVPXML(ByVal _TagName$, ByVal ParamArray _KeyValuePairs As [String]()()) As XElement
			Try

				REM Input:	{{"Line", "18"}, {"Column", "9"}}
				REM Output:	<TokenLocation Line="18" Column="9"/>

				If _KeyValuePairs.Length < 1 Then Throw New DSValidationException("The _KeyValuePairs 2-D Array contained fewer than one item(s). It must contain at least one array.", "_KeyValuePairs")
				If _KeyValuePairs.Any(Function(_KVP As [String]()) Not (_KVP.Length = 2)) Then Throw New DSValidationException("At least one of the Arrays in the _KeyValuePairs Array did not have two items in it. The Array should be in the form {{""Line"", ""18""}, {""Column"", ""9""}}""", "_KeyValuePairs")

				Dim _XElementToReturn As New XElement(_TagName$)

				For Each _KVP As String() In _KeyValuePairs
					_XElementToReturn.SetAttributeValue(_KVP.ElementAt(0), _KVP.ElementAt(1))
				Next

				Return _XElementToReturn

			Catch _Ex As Exception : Throw New DSException("@GetKVPXML: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>Returns a Serialised XML form of the Object's Key-Value Pairs specified</summary>
		Public Function GetKVPXML(Of _TObject)(ByVal ParamArray _KeyValuePairs As String()()) As XElement
			Return GetKVPXML(GetType(_TObject).Name, _KeyValuePairs)
		End Function

	End Module

End Namespace