# DocScript!
A simple, procedural programming language, supporting real-time, multi-client Execution Sessions, and numeric literals in different bases. (BM A-Level CS Coursework 2023)  
Watch the ["DocScript in 3 Minutes" Video](https://www.youtube.com/watch?v=ybl5pVSJOOk).


### There are four DocScript "Implementations":
1. **A Command-line Interpreter**, `DSCLI.exe`
	![DocScript Windows IDE Demonstration](https://github.com/BenMullan/DocScript/blob/master/_Resources/Documentation/DSIDE_Demo.png?raw=true)
2. **A Graphical IDE**, `DSIDE.exe`
	![DocScript Command-line Demonstration](https://github.com/BenMullan/DocScript/blob/master/_Resources/Documentation/DSCLI_Demo.png?raw=true)
3. **A Web-based system** permitting distributed *multi-client Execution-Sessions*, DSInteractive
	![DocScript Interactive Demonstration](https://github.com/BenMullan/DocScript/blob/master/_Resources/Documentation/DSInteractive_Demo.png?raw=true)
**Note:** All three implementations rely on the Core Interpretation Logic contained in `DocScript.Library.dll`
- DocScript.Library.dll		(Core Interpretation Logic)
- DSCLI.EXE					(DS Command-Line Interpreter. Use the /? argument.)
- DSIDE.EXE					(DS Windows GUI IDE)
- _CreateEntireDB.SQL		(Read the comments herein for DS-Interactive setup guidance)

(Use Everything-Search to find said files in the solution)

# Solution Conventions
--------------------------------------------------

|Notation:		|Meaning:															|
|---------------|-------------------------------------------------------------------|
|No.			|Number Of															|
|UpToInc *		|Up To and Including *												|
|UpToExc *		|Up To but Excluding *												|
|<*>			|Of the DataType * (DocScript Source)								|
|_*				|* is an Identifier for a Local Item								|
|*_				|* is an Identifier for a Private or Protected Item					|
|*__			|* is an identifier for a Friend Item								|
|_*_			|* is an Identifier for a Static (!= Shared) Item					|
|T*				|* is a Generic Type Specifier										|
|I*				|* is an Interface													|
|Ensures *		|Throws an Exception if * is not the case							|
|DS*Exception	|* inherits from System.Exception									|
|[*]			|WebParts API: * will be returned, as an XML Attr.					|
|[...*]			|WebParts API: * will be returned, when Long-Polling ends			|
|[<*>]			|WebParts API: * will be returned, within &lt;ResponseContent&gt;	|


General:
--------
- CompilerExpentions should generally not modify the input parameter; they should only return the output. The Method names should reflect this. E.g., "WithLastCharacterRemoved()" instead of "RemoveLastCharacter()"
- Solution Files are named according to whatever the most narrowed-down logical container is (Namespace, Class, or Module etc...)
- Remember to "ALTER DATABASE [DocScript] SET ENABLE_BROKER;" in order for [SqlDependancy]s to work. As of 01-01-2023, DSInteractive no longer uses a QueryWatch anyway, so this isn't needed.
- The ToString() of an Instruction or Expression, returns what the it would have looked like in the Source


Volcabulary:
------------
- Upstairs/Downstairs		A new recursivly-created stack frame, logically above/below the current one on the Stack
- Upstream/Downstream		A component which exists forwards/backwards of the current one, along a linear structure


[Ctrl + F] Global Search Codes:
-------------------------------
- **NI				Not Implemented
- **ToDo			Minor improvement to be made at a later date
- **ChangeLog		A component of an algorithm has been updated on the specified date


Abbreviations:
--------------
- LBL		Linear Bracketed Level (ExprTree Construction)
- IOT		Intermediate Operator Tree (ExprTree Construction)
- SCI		Scanned Component Indicator (ExprTree Construction)
- ESID		ExecutionSession Identifier (DocScript Interactive)
- IEID		Input Event Identifier (DocScript Interactive)
- CEP		Client Execution Package (DocScript Interactive)
- TPV		Tokens to (Token)Patterns Validator (Lexing)
- IR		Intermediate Representation (Lexing; the Instruction Tree)

Did you really just make it all the way to the end of this README? Well done!