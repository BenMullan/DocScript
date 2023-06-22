#Compiles all of the DSPipeline Binaries

$VBCPath = "C:\Windows\Microsoft.NET\Framework\v4.0.30319\vbc.exe";
Copy-Item "..\..\DSLibrary\bin\Release\DSCore.dll" -Destination ".\Binaries\"
$DSPipelineBinaries = @("GetText", "DSParse", "DSLex", "DSExec", "DSCompile");

ForEach ($DSPipelineBinary In $DSPipelineBinaries) {
	Write-Host "-------------------- Compiling $($DSPipelineBinary) --------------------";
	& "$($VBCPath)" "$($DSPipelineBinary).VB" /out:".\Binaries\$($DSPipelineBinary).exe" /target:exe /reference:".\Binaries\DSCore.dll" /nologo
}

Read-Host "Press Enter to continue...";