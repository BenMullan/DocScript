Public Module PredefinedSnippets

	Public ReadOnly SampleProgram_HelloWorld$ = _
	  "#HelloWorld.DS" & vbCrLf & _
	  "Function <Void> Main ()" & vbCrLf & _
	  vbTab & "Output(""Hello, World!"")" & vbCrLf & _
	  "EndFunction"

	Public ReadOnly SampleProgram_BeepSleepRepeat$ = _
	 "#BeepSleepRepeat.DS" & vbCrLf & _
	 "Function <Void> Main ()" & vbCrLf & _
	 vbTab & "System_Beep()" & vbCrLf & _
	 vbTab & "DS_Sleep(1000)" & vbCrLf & _
	 vbTab & "Main()" & vbCrLf & _
	 "EndFunction"

	Public ReadOnly SampleProgram_DebuggingExample$ = _
	  <S>#DebuggingExample.DS

#This ↓ Entrypoint Function ↓ takes in command-line arguments,
#	and returns an ExitCode to the operating system.

Function &lt;Number&gt; Main (&lt;String@&gt; _CLAs)
	
	Loop (3)
		&lt;String&gt; _Random : DS_String_GetRandom(5)
		Debug_ShowSymbolTables()
	EndLoop

	Break()

	Return 0
EndFunction</S>.Value

	Public ReadOnly EntryPoint_Unadorned$ = _
	 "Function <Void> Main ()" & vbCrLf & _
	 vbTab & vbCrLf & _
	 "EndFunction"

	Public ReadOnly EntryPoint_OSInterop$ = _
	 "Function <Number> Main (<String@> _CLAs)" & vbCrLf & _
	 vbTab & vbCrLf & _
	 vbTab & "Return 0" & vbCrLf & _
	 "EndFunction"

	Public ReadOnly Loop_ConditionControlled$ = _
	 "While (Input(""GO?"") = ""GO"")" & vbCrLf & _
	 vbTab & "Output(""Going!"")" & vbCrLf & _
	 "EndWhile"

	Public ReadOnly Loop_CountControlled_While$ = _
	 "<Number> _IterationsToPerform : 4" & vbCrLf & _
	 "<Number> _CurrentIteration : 1" & vbCrLf & _
	  vbCrLf & _
	 "While ( Maths_LessThan(_CurrentIteration, _IterationsToPerform + 1) )" & vbCrLf & _
	  vbTab & "Output(""This is Iteration "" & _CurrentIteration & "" of "" & _IterationsToPerform)" & vbCrLf & _
	  vbTab & "_CurrentIteration : _CurrentIteration + 1" & vbCrLf & _
	 "EndWhile"

	Public ReadOnly Loop_CountControlled_Loop$ = _
	 "Loop (4)" & vbCrLf & _
	 vbTab & "Output(""Hello"")" & vbCrLf & _
	 "EndLoop"

	Public ReadOnly Loop_ArrayIteration$ = _
	 "<String@> _Array" & vbCrLf & _
	"_Array : DS_StringArray_Append(_Array, ""Andrew"")" & vbCrLf & _
	"_Array : DS_StringArray_Append(_Array, ""Ryan"")" & vbCrLf & _
	"_Array : DS_StringArray_Append(_Array, ""Ollie"")" & vbCrLf & _
	vbCrLf & _
	"<Number> _CurrentIndex : 0" & vbCrLf & _
	"While ( Maths_LessThan(_CurrentIndex, DS_StringArray_Length(_Array)) )" & vbCrLf & _
	vbTab & "Output(""The current Element is: "" & DS_StringArray_At(_Array, _CurrentIndex))" & vbCrLf & _
	vbTab & "_CurrentIndex : _CurrentIndex + 1" & vbCrLf & _
	"EndWhile"

	Public ReadOnly IfElseStatement$ = _
	  "If (True)" & vbCrLf & _
	  vbTab & "Output(""Was True!"")" & vbCrLf & _
	  "Else" & vbCrLf & _
	  vbTab & "Output(""Was False!"")" & vbCrLf & _
	  "EndIf"

End Module