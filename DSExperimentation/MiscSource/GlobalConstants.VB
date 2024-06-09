Module GlobalConstants

	REM DocScript.Runtime.Praser:
	Public Const CommentCharacter As Char = "#"c

	Public Const SLITPlaceholderPattern$ = "$Slit_{Number}$" '{Number} is later String.Replace()'d

	Public Const WordChar_RegExp$ = "^[0-9a-zA-Z_\$]$"

End Module