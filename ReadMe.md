# DocScript
...is a simple procedural programming language, with...
- Real-time multi-client distributed "execution sessions"
- Numeric literals in any base
- Built-in **remote code-execution** (DS-Remoting)
- **Compilation** to standalone .NET `exe`s (DS-Compilation)
- A capability to [pipe](https://github.com/BenMullan/DocScript?tab=readme-ov-file#ds-pipelining) seperate interpretation stages into one another.
<br/><br/>

> Watch the [**"DocScript in 3 Minutes" YouTube Video**](https://www.youtube.com/watch?v=ybl5pVSJOOk)!

<br/><br/>

### Source Example
An example DocScript Program, to solve [the Lightswitch Problem](https://www.youtube.com/watch?v=-UBDRX6bk-A)...
<br/><img src="https://github.com/BenMullan/DocScript/blob/master/_Resources/Documentation/TheLightswitchProblem_Sample.png?raw=true" width="100%" />
<br/><br/>

### There are 3 DocScript *Implementations*...
1. **A Graphical IDE**, `DSIDE.exe`:
	<br/><img src="https://github.com/BenMullan/DocScript/blob/master/_Resources/Documentation/DSIDE_Demo.png?raw=true" width="50%" /><br/>
2. **A Command-line Interpreter**, `DSCLI.exe`:
	<br/><img src="https://github.com/BenMullan/DocScript/blob/master/_Resources/Documentation/DSCLI_Demo.png?raw=true" width="50%" /><br/>
3. **A Web-based system** permitting distributed *multi-client Execution-Sessions*, DSInteractive:
	<br/><img src="https://github.com/BenMullan/DocScript/blob/master/_Resources/Documentation/DSInteractive_Demo.png?raw=true" width="50%" /><br/>

**Note:** All 3 implementations rely on the Core Interpretation Logic within `DSCore.dll` (which was previously called `DocScript.Library.dll`)
<br/><br/><br/><br/>


# ▶️ Get Started
**Start using DocScript in the next few seconds: open `cmd`, and run...**
```
curl https://raw.githubusercontent.com/BenMullan/DocScript/master/_Resources/DS-QuickSetup.cmd | cmd
```

*Alternatively...*
- Download `DSSetup.msi` (or just the binaries) from [§Releases](https://github.com/BenMullan/DocScript/releases)
- Launch DocScript IDE, and try a sample program from "_Insert Code Snippet..._". Use built-in Pictorial Help (Ctrl + Shift + H) to discover DocScript syntax.
    - ...Or, run `DSCLI.exe /?`
    - ...Or, read `\DSWebParts\(Source)\DatabaseResources\_CreateEntireDB.SQL` for DSInteractive Setup guidance
- **Take a look at the [§DocScript Sample Programs](https://github.com/BenMullan/DocScript/tree/master/_Resources/SamplePrograms/)** to see some of the cool [Multimedia](https://github.com/BenMullan/DocScript/blob/master/_Resources/SamplePrograms/PlayWav.DS), [Networking](https://github.com/BenMullan/DocScript/blob/master/_Resources/SamplePrograms/Curl.DS), and [Data-Processing](https://github.com/BenMullan/DocScript/blob/master/_Resources/SamplePrograms/LambdaWhere.DS) abilities of the language...

<br/>(Download the *very latest* (pre-release-candidate) version of DocScript [here](https://github.com/BenMullan/DocScript/raw/master/DSSetup/Release/DSSetup.msi))
<br/><br/><br/>


# DS-Pipelining
...allows each interpretation stage to be performed separately, and linked to other stages in a pipeline - like this:
```
GetText Hello.DS | DSParse | DSLex | DSExec
```
With 7 Pipelining modules (`GetText.exe`, `DSParse.exe`, `DSLex.exe`, `DSOptimise.exe`, `DSProgXMLToSource.exe`, `DSExec.exe`, and `DSCompile.exe`; compiled copies [here](https://github.com/BenMullan/DocScript/tree/master/_Resources/DSPipelining/Binaries)), there exist many possible permutations of interpretation-logic chain, with which to understand each stage of interpretation.

For instance, to view the Tokens produced from the source code in `Hello.DS`, you could use...<br/>
<br/><img src="https://github.com/BenMullan/DocScript/blob/master/_Resources/Documentation/DSPipelining_Tokens.png?raw=true" width="100%" /><br/>

To optimise [the `HighlyOptimisable.DS` sample program](https://github.com/BenMullan/DocScript/blob/master/_Resources/SamplePrograms/HighlyOptimisable.DS) and save it as `HighlyOptimised.DS`, you could use...
```
GetText HighlyOptimisable.DS | DSParse | DSLex | DSOptimise | DSProgXMLToSource > HighlyOptimised.DS
```

You could make an entertainingly long chain of interpretation/compilation operations, like...
```
GetText Hello.DS | DSParse | DSLex | DSOptimise | DSProgXMLToSource | DSParse | DSLex | DSCompile Hello && Hello
```

...or resolve a standalone DocScript expression, like...
```
echo [77+23]*9 | DSParse | DSLexExpr | DSResolveExpr
```

See [`\_Resources\DSPipelining\`](https://github.com/BenMullan/DocScript/tree/master/_Resources/DSPipelining)!

<br/><br/><br/>
<i>Perhaps you can be bothered to read [even more DocScript documentation...](https://github.com/BenMullan/DocScript/wiki)?</i>