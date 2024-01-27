#File:		_CompileAll.PS1 - Compiles all of the DSPipeline Binaries, from their source DS*.VB Files
#Exec:		powershell -File _CompileAll.PS1
#Author:	Ben Mullan (c) 2024

#This script should automatically be run when the DSSetup Project is built from the Solution
#This script uses the Release\ version of DSCore.dll; if debugging in VS with Debug\, ensure you build to Release\ first.

If ( ! [System.IO.Directory]::Exists(".\Binaries\") ) { Write-Error "Please run this script in the directory of the DSPipelining Source Files (GetText.VB, DSParse.VB, etc.)."; Exit; }

$VBCPath = "C:\Windows\Microsoft.NET\Framework\v4.0.30319\vbc.exe";
Copy-Item "..\..\DSLibrary\bin\Release\DSCore.dll" -Destination ".\Binaries\"
$DSPipelineBinaries = @("GetText", "DSParse", "DSLex", "DSLexExpr", "DSOptimise", "DSProgXMLToSource", "DSExec", "DSCompile", "DSResolveExpr");

ForEach ($DSPipelineBinary In $DSPipelineBinaries) {
	Write-Host "-------------------- Compiling $($DSPipelineBinary) --------------------";
	& "$($VBCPath)" "$($DSPipelineBinary).VB" /out:".\Binaries\$($DSPipelineBinary).exe" /target:exe /reference:".\Binaries\DSCore.dll" /nologo
}

Write-Host "All Pipelining Modules compiled";
#Read-Host "Press Enter to continue...";