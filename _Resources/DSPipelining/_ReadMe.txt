DocScript Pipelining
--------------------
	A set of modules allowing each interpretation stage to be performed separately, and linked to other stages in a pipeline.
	
	Example usage: GetText Hello.DS | DSParse | DSLex | DSExec
	Example usage: GetText Hello.DS | DSParse | DSLex | DSCompile Hello.exe
	Example usage: GetText ITakeCLAs.DS | DSParse | DSLex | DSExec CLA0 CLA1
	Example usage: GetText Unoptimised.DSProgramXML | DSOptimise | DSCompile Optimised.exe

╒═════════════════════════╤═══════════════════════════════════════════════════════════════╕ 
│ Module                  │ Description                                                   │▒
╞═════════════════════════╪═══════════════════════════════════════════════════════════════╡▒
│ GetText.exe             │ Outputs CLA[0] as Text; more reliable than the "TYPE" command │▒
├─────────────────────────┼───────────────────────────────────────────────────────────────┤▒
│ DSParse.exe             │ Parses the piped source, outputting XML Tokens                │▒
├─────────────────────────┼───────────────────────────────────────────────────────────────┤▒
│ DSLex.exe               │ Constructs a Program from piped Tokens, output in XML         │▒
├─────────────────────────┼───────────────────────────────────────────────────────────────┤▒
│ DSOptimise.exe          │ Performs DS Runtime Optimisation on the Program               │▒
├─────────────────────────┼───────────────────────────────────────────────────────────────┤▒
│ DSProgXMLToSource.exe   │ Reconstructs DocScript source from piped Program Tree XML     │▒
├─────────────────────────┼───────────────────────────────────────────────────────────────┤▒
│ DSExec.exe              │ Executes the Program from piped Program Tree XML              │▒
├─────────────────────────┼───────────────────────────────────────────────────────────────┤▒
│ DSCompile.exe           │ Compiles the Program from piped Program Tree XML, to an EXE   │▒
└─────────────────────────┴───────────────────────────────────────────────────────────────┘▒
 ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒

DS-Pipelining Notes:
	-
	- E.g. To see the Lexer Output, just run: GetText Hello.DS | DSParse | DSLex
	-
	- E.g. You could use "GetText Hello.DS | DSParse | DSLex > LexOut.XML"...
		...to save the Lexer Output to a File, change it, and then execute it using...
		..."DSExec < LexOut.XML"
	-
	- Press [Ctrl + Z], [Enter] on the Console, to indicate the end of StdIn
	-
	- DSExec's Command-line Arguments become the CLAs of the DocScript Program
	-
	- See the DS*.VB file for each Pipelining Module, for more module-specific details



Ben Mullan (c) 2023
https://github.com/BenMullan/DocScript/