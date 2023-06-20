''' <summary>Used for internal debugging purposes. No calls to members hereof should exist within ReleaseCandidate* builds.</summary>
Public Module _DebugResources

	Public DebugTxtPath$ = "D:\Benedict\Documents\SchoolWork\Projects\DocScript\Resources\Debug.txt"
	Public MsgDebug As Action(Of [Object]) = Sub(_Object As [Object]) MsgBox("DEBUG:" & vbCrLf & vbCrLf & If(_Object, "{MsgDebug: The _Object was NOTHING}").ToString(), MsgBoxStyle.Exclamation, "DocScript Debug Function")

End Module