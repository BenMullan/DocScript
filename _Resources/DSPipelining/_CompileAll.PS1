#Compiles all of the DSPipeline Binaries
#This script should automatically be run when the DSSetup Project is built from the Solution

#This script uses the Release\ version of DSCore.dll; if debugging in VS with Debug\, ensure you build to Release\ first.

$VBCPath = "C:\Windows\Microsoft.NET\Framework\v4.0.30319\vbc.exe";
Copy-Item "..\..\DSLibrary\bin\Release\DSCore.dll" -Destination ".\Binaries\"
$DSPipelineBinaries = @("GetText", "DSParse", "DSLex", "DSOptimise", "DSProgXMLToSource", "DSExec", "DSCompile");

ForEach ($DSPipelineBinary In $DSPipelineBinaries) {
	Write-Host "-------------------- Compiling $($DSPipelineBinary) --------------------";
	& "$($VBCPath)" "$($DSPipelineBinary).VB" /out:".\Binaries\$($DSPipelineBinary).exe" /target:exe /reference:".\Binaries\DSCore.dll" /nologo
}

#Read-Host "Press Enter to continue...";