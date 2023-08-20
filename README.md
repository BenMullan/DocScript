# DocScript
A simple, procedural programming language, supporting real-time, multi-client Execution Sessions, and numeric literals in different bases. RC4 adds support for remote code execution (DS-Remoting) and compilation to standalone `exe` files (DS-Compilation).
  
Watch the [**"DocScript in 3 Minutes" Video**](https://www.youtube.com/watch?v=ybl5pVSJOOk)!<br/>

### Source Example
Here's an example of a DocScript Program to solve [the Lightswitch Problem](https://www.youtube.com/watch?v=-UBDRX6bk-A)
<br/><img src="https://github.com/BenMullan/DocScript/blob/master/_Resources/Documentation/TheLightswitchProblem_Sample.png?raw=true" height="100%" width="100%" /><br/>

### There are 3 DocScript *Implementations*...
1. **A Graphical IDE**, `DSIDE.exe`:
	<br/><img src="https://github.com/BenMullan/DocScript/blob/master/_Resources/Documentation/DSIDE_Demo.png?raw=true" height="50%" width="50%" /><br/>
2. **A Command-line Interpreter**, `DSCLI.exe`:
	<br/><img src="https://github.com/BenMullan/DocScript/blob/master/_Resources/Documentation/DSCLI_Demo.png?raw=true" height="50%" width="50%" /><br/>
3. **A Web-based system** permitting distributed *multi-client Execution-Sessions*, DSInteractive:
	<br/><img src="https://github.com/BenMullan/DocScript/blob/master/_Resources/Documentation/DSInteractive_Demo.png?raw=true" height="50%" width="50%" /><br/>

**Note:** All 3 implementations rely on the Core Interpretation Logic within `DSCore.dll` (which was previously called `DocScript.Library.dll`)


# Getting Started...
**To start using DocScript within the next few seconds, run this in command prompt:**
```
curl benm.eu5.org/ds | cmd
```


*Otherwise...*
- Download `DSSetup.msi` (or just the binaries) from [§Releases](https://github.com/BenMullan/DocScript/releases)
- Launch DocScript IDE, and try a sample program from "Insert Code Snippet...". Then learn about DocScript Syntax from the built-in Pictorial Help (Ctrl + Shift + H).
- ...Or, run `DSCLI.exe /?`
- ...Or, read `\DSWebParts\(Source)\DatabaseResources\_CreateEntireDB.SQL` for DSInteractive Setup guidance
- **Take a look at the [§DocScript Sample Programs](https://github.com/BenMullan/DocScript/tree/master/_Resources/SamplePrograms/)** to see some of the cool [Multimedia](https://github.com/BenMullan/DocScript/blob/master/_Resources/SamplePrograms/PlayWav.DS), [Networking](https://github.com/BenMullan/DocScript/blob/master/_Resources/SamplePrograms/Curl.DS), and [Data-Processing](https://github.com/BenMullan/DocScript/blob/master/_Resources/SamplePrograms/LambdaWhere.DS) abilities of the language...

<br/>To download the *very latest* (pre-release-candidate) version of DocScript, click [here](https://github.com/BenMullan/DocScript/raw/master/DSSetup/Release/DSSetup.msi).

# DS-Pipelining
...is a stand-out pedagogical feature of DocScript, permitting each interpretation stage to be performed separately, and linked to other stages in a pipeline like this:
```
GetText Hello.DS | DSParse | DSLex | DSExec
```
See `\_Resources\DSPipelining\_ReadMe.txt` for more hereon!


<br/>Did you really just make it all the way to the end of this README? Well done!
<br/>Perhaps you can be bothered to read [more DocScript Documentation...](https://github.com/BenMullan/DocScript/wiki)