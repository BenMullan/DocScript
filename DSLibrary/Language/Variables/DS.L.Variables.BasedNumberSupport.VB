Namespace Language.Variables

	''' <summary>Implements support for Based Numbers in DocScript</summary>
	Public Module BasedNumberSupport

		''' <summary>Represents a numerical magnitude with a Base and a Value, providing easy support for converting between bases (in the PlaceValue (Positional-Notation) System)</summary>
		Public Structure BasedNumber

			''' <summary>Used when there is no Grapheme avaliable for a specified GraphemeIndex</summary>
			Public Const NullGrapheme As Char = "#"c

			Public ReadOnly Value As [String]
			Public ReadOnly Base As [Byte] '0 to 255

			''' <summary>The characters used to represent the Base Number. Starting 0123456789 and then ABCDEF...</summary>
			Public Shared ReadOnly Property GraphemeSet As Char()
				Get
					Return Global.DocScript.Language.Constants.BasedNumericLiteralGraphemes
				End Get
			End Property

			''' <summary>The _Value must use Graphemes from the BasedNumber.Graphemes Property</summary>
			Public Sub New(ByVal _Value As [String], ByVal _Base As [Byte])

				REM Ensure the _Value only contains Chars from our GraphemeSet
				If Not _Value.ToCharArray().All(Function(_Char As Char) BasedNumber.GraphemeSet.Contains(_Char)) Then Throw New DSValidationException("At least one char in the _Value does not appear in the BasedNumber's GraphemeSet", _Value)

				REM Check that the _Value is valid for the _Base
				If (_Value.ToCharArray().Any(Function(_Char As Char) Array.IndexOf(BasedNumber.GraphemeSet, _Char) > (_Base - 1))) Then Throw New DSValidationException("An attempt was made to Instanciate a BasedNumber, however the Specified _Value was not valid for the specified _Base", "The Value was: " & _Value & ", and the Base was: " & _Base.ToString())

				REM Check that we have enough Graphemes for the specified base
				If (BasedNumber.GraphemeSet.Length < _Base) Then Throw New DSValidationException("An attempt was made to instanciate a BasedNumber, however there are not enough tokens to evaluate all possible values in the Specified _Base", "The Base was: " & _Base.ToString())

				Me.Value = _Value
				Me.Base = _Base

			End Sub

			''' <summary>Returns the current BasedNumber as a New BasedNumber, converting it to the _TargetBase</summary>
			''' <param name="_TargetBase">The Base for the New BasedNumber</param>
			Public Function ToBase(ByVal _TargetBase As [Byte]) As BasedNumber

				If (Not _TargetBase >= 2) Then Throw New DSValidationException("An attempt was made to convert a BasedNumber into another Based Number, with a Base less than 2. This cannot be done using the PlaceValue (Positional-Notation) System.", _TargetBase.ToString())

				Dim _Base10NumberToFindInNewBase& = Me.AsBase10

				Dim _DepletableNumberToFind@ = _Base10NumberToFindInNewBase
				Dim _NumberOfDigits%

				While (_DepletableNumberToFind > 0)

					_NumberOfDigits += 1

					_DepletableNumberToFind -= CDec(If(
					  Not ((_DepletableNumberToFind / (_TargetBase ^ (_NumberOfDigits - 1))) = 0),
					  ((_TargetBase ^ (_NumberOfDigits - 1)) * (_TargetBase - 1)),
					  (_DepletableNumberToFind / (_TargetBase ^ (_NumberOfDigits - 1)))))

				End While

				_DepletableNumberToFind = _Base10NumberToFindInNewBase

				Dim _GraphemeCodes As New List(Of Integer)()

				For _CurrentItiteration% = (_NumberOfDigits - 1) To 0 Step -1

					_GraphemeCodes.Add(CInt(If(
					 Math.Floor(_DepletableNumberToFind / (_TargetBase ^ _CurrentItiteration)) > (_TargetBase - 1),
					 _TargetBase - 1,
					 Math.Floor(_DepletableNumberToFind / (_TargetBase ^ _CurrentItiteration)))))

					_DepletableNumberToFind -= CDec((If(
					 Math.Floor(_DepletableNumberToFind / (_TargetBase ^ _CurrentItiteration)) > (_TargetBase - 1),
					 _TargetBase ^ _CurrentItiteration,
					 (Math.Floor(_DepletableNumberToFind / (_TargetBase ^ (_CurrentItiteration))) * (_TargetBase ^ (_CurrentItiteration))))))

				Next

				Return New BasedNumber(BasedNumber.ProduceBasedNumberRepresentation_FromGraphemeCodes_$(_GraphemeCodes.ToArray()), _TargetBase)

			End Function

			''' <summary>E.g. takes in {0, 10, 8, 1666} and returns "A80#"</summary>
			Private Shared Function ProduceBasedNumberRepresentation_FromGraphemeCodes_$(ByVal _GraphemeCodes%())

				Dim _BasedNumberRepresentation$ = ""

				For Each _GraphemeCode% In _GraphemeCodes
					_BasedNumberRepresentation &= If((_GraphemeCode% <= (BasedNumber.GraphemeSet.Length - 1)), BasedNumber.GraphemeSet.ElementAt(_GraphemeCode%).ToString(), "#")
				Next

				Return _BasedNumberRepresentation

			End Function

			''' <summary>Computes the Base-10 Representation of the Number, then Parses this as an Int64, and Reutrns it.</summary>
			Public ReadOnly Property AsBase10 As Int64
				Get
					REM Value looks like:	"A2"
					REM Base Looks Like:	12

					Dim _TotalValue As Int64

					Dim _CurrentCharIndex As Int32 = (Me.Value.Length - 1)

					REM Go through the Value String, adding to the Total the PlaceValue of the current Token
					For _PlaceValueExponent As Int32 = 0 To (Me.Value.Length - 1) Step +1

						_TotalValue += CLng((Me.Base ^ _PlaceValueExponent) * Array.IndexOf(BasedNumber.GraphemeSet, Me.Value.ToCharArray()(_CurrentCharIndex)))

						'Move onto the Next Token, going <- Direction...
						_CurrentCharIndex -= 1

					Next

					Return _TotalValue

				End Get
			End Property

			''' <summary>Returns a KVP-Serialised representation of the BasedNumber</summary>
			Public Overrides Function ToString() As String
				Return DocScript.Utilities.KVPSerialisation.GetKVPString({"Value", Me.Value}, {"Base", Me.Base.ToString()})
			End Function

		End Structure

	End Module

End Namespace