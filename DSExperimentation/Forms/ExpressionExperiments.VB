Public Class ExpressionExperiments

	Public Sub ResolveInputExpr() Handles ResolveButton.Click
		Try

			Console.WriteLine("2ExprExperiments: The Console is Working if you can read this!") : Console.WriteLine()

			DocScript.Logging.ProcessDebugEvents = True
			DocScript.Logging.LogUtilities.CurrentLogEventHandler = DocScript.Logging.BuiltInLogEventHandlers.CLIDefault

			Me.OutputTextBox.Text = DocScript.Language.Expressions.ExprUtilities.ConstructExpressionFromTokens(DocScript.Runtime.Parser.GetTokensFromSource(Me.InputExprTextBox.Text)).GetExprTreeNodeXML().ToString(Xml.Linq.SaveOptions.None)

		Catch _Ex As Exception When True
			MsgBox("Exception:" & vbCrLf & vbCrLf & _Ex.Message, MsgBoxStyle.Critical, _Ex.GetType().Name)
		End Try
	End Sub

	Private Sub StringToNumberCoercionButton_Click() Handles StringToNumberCoercionButton.Click
		Try

			DocScript.Logging.ProcessDebugEvents = True
			DocScript.Logging.LogUtilities.CurrentLogEventHandler = DocScript.Logging.BuiltInLogEventHandlers.CLIDefault

			Dim _Number_AsString As New DocScript.Language.Variables.DSString(InputBox("String to make into a DSNumber"))
			_Number_AsString.Coerce(Of DocScript.Language.Variables.DSNumber)().Value.Output(AddressOf MsgBox)

		Catch _Ex As Exception When True
			MsgBox("Exception:" & vbCrLf & vbCrLf & _Ex.Message, MsgBoxStyle.Critical, _Ex.GetType().Name)
		End Try
	End Sub

End Class

Namespace DSEExpressions

	Public Module ExprUtilities

		Public Function ConstructTreeFromExpr(ByVal _InputExpr$) As Node

			REM Start with an Empty Node
			Dim _Tree As New Node()

			Dim _LastTouchedNodePath As New List(Of Boolean)() 'True = Left; False = Right

			REM Get the Tokens of the _InputExpr
			Dim _InputExprTokens As DocScript.Runtime.Token() = DocScript.Runtime.Parser.GetTokensFromSource(_InputExpr)

			For Each _Token As DocScript.Runtime.Token In _InputExprTokens
				Select Case _Token.Type
					Case DocScript.Runtime.Token.TokenType.NumericLiteral

						REM Add the Number as the Left or Right Child of the current Node (whichever is empty)

						Dim _LastTouchedNode As Node = _Tree.GetNode(_LastTouchedNodePath.ToArray())

						If _LastTouchedNode.LeftChild Is Nothing Then
							_LastTouchedNode.LeftChild = New Node() With {.Value = _Token.Value}
							_LastTouchedNodePath.Add(True)
						ElseIf _LastTouchedNode.RightChild Is Nothing Then
							_LastTouchedNode.RightChild = New Node() With {.Value = _Token.Value}
							_LastTouchedNodePath.Add(False)
						Else
							Throw New DocScript.Exceptions.DSException("Both the Right and Left Children were populated for Node: " & _LastTouchedNode.ToString())
						End If

					Case DocScript.Runtime.Token.TokenType.DSOperator

						REM Climb to the first Empty Node
						Dim _CurrentPath As List(Of Boolean) = _LastTouchedNodePath
						Dim _FirstEmptyNode As Node = _Tree.GetNode(_CurrentPath.ToArray())

						Do Until (_FirstEmptyNode.Value = String.Empty) OrElse (_CurrentPath.Count = 0)
							_CurrentPath = _CurrentPath.Take(_CurrentPath.Count - 1).ToList()
							_FirstEmptyNode = _Tree.GetNode(_CurrentPath.ToArray())
						Loop

						REM Add a new Top-Level Node if [Not (_FirstEmptyNode.Value = String.Empty)]
						If Not (_FirstEmptyNode.Value = String.Empty) Then
							_Tree = New Node() With {.Value = _Token.Value, .LeftChild = _Tree}
						Else
							_FirstEmptyNode.Value = _Token.Value
						End If

					Case DocScript.Runtime.Token.TokenType.GrammarChar
						If Not {"[", "]"}.Contains(_Token.Value) Then Throw New DocScript.Exceptions.DSException("Invalid GrammarChar: " & _Token.ToString())
					Case Else
						Throw New DocScript.Exceptions.DSException("Invalid Token (Type): " & _Token.ToString())
				End Select
			Next

			Return _Tree

		End Function

	End Module

	Public Class Node

		Public Value As String = String.Empty
		Public RightChild As Node = Nothing
		Public LeftChild As Node = Nothing

		Public Function GetNode(ByVal _Path As Boolean()) As Node

			REM [True = Left]; [False = Right]

			Dim _CurrentNode As Node = Me

			For Each _Direction As Boolean In _Path
				If _Direction Then _CurrentNode = _CurrentNode.LeftChild Else _CurrentNode = _CurrentNode.RightChild
			Next

			Return _CurrentNode

		End Function

		''' <summary>Returns a KVPSerialised String to represent the Data of the current Object</summary>
		Public Overrides Function ToString() As String
			Return DocScript.Utilities.KVPSerialisation.GetKVPString({"Value", Me.Value}, {"LeftChild", If(Me.LeftChild Is Nothing, "(NOTHING)", Me.LeftChild.ToString())}, {"RightChild", If(Me.RightChild Is Nothing, "(NOTHING)", Me.RightChild.ToString())})
		End Function

	End Class

End Namespace