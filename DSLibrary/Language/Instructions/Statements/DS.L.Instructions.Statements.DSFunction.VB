Namespace Language.Instructions.Statements

	''' <summary>Represents E.g. [Function &lt;DataType&gt; Identifier (Arguments...)]</summary>
	Public Class DSFunction : Implements Language.Instructions.Statements.IStatement, Runtime.SymbolTable.ISymbolTableValue

#Region "Declarations specific to this IInstruction"

		''' <summary>Represents a Placeholder for an Argument, used as a Local Variable within a DSFunction. (Parameter == Placeholder; Argument == A Datum applied to a Parameter at Runtime)</summary>
		Public Structure Parameter

			'<Boolean>, <Number>, <String>, <Boolean@>, <Number@>, <String@>
			Public Shared ReadOnly PermittedParameterDataTypes As System.Type() = {GetType(Variables.DSBoolean), GetType(Variables.DSNumber), GetType(Variables.DSString), GetType(Variables.DSArray(Of Variables.DSBoolean)), GetType(Variables.DSArray(Of Variables.DSNumber)), GetType(Variables.DSArray(Of Variables.DSString))}

			''' <summary>The Unique Identifier for the Parameter</summary>
			Public ReadOnly Identifier As Global.System.String

			''' <summary>The DataType must be an IDataValue-based DocScript DataType e.g. DSBoolean</summary>
			Public ReadOnly DataType As Global.System.Type 'A DocScript DataType e.g. <Number> or <String@>

			''' <summary>The DataType must be a DocScript DataType e.g. &lt;Number&gt;. Use the IDataValue Types herefor.</summary>
			Public Sub New(ByVal _Identifier$, ByVal _DataType As Type)
				If Not Parameter.PermittedParameterDataTypes.Contains(_DataType) Then Throw New DSValidationException("A DSFunction's Parameter must be one of the permitted Types herefor: " & PermittedParameterDataTypes.GetStandardArraySerialisation(), "Specified Type: " & _DataType.Name.ToString())
				Me.Identifier = _Identifier : Me.DataType = _DataType
			End Sub

			''' <summary>Returns an XML Representation of the Parameter, with the Identifier and DataType</summary>
			Public Function GetProgramTreeNodeXML() As XElement
				Return <Parameter Identifier=<%= Me.Identifier %> DataType=<%= Variables.VariableUtilities.GetDataTypeString_FromDSVariableType(Me.DataType) %>/>
			End Function

			''' <summary>Constructs an instance of the class from its XML serialasation</summary>
			Public Shared Function FromXML(ByVal _XElement As XElement) As Parameter
				Try

					Return New Parameter(
					 _Identifier:=_XElement.@Identifier,
					 _DataType:=Variables.VariableUtilities.GetDSVariableType_FromDataTypeString(_XElement.@DataType)
					)

				Catch _Ex As Exception : Throw New DSException("@Parameter\FromXML: " & _Ex.Message, _Ex) : End Try
			End Function

			''' <summary>Returns what the Argument would have looked like in the Source, without an extra LineBreak on the End</summary>
			Public Overrides Function ToString() As String

				REM E.g.
				'	<STRING@> NAMES

				Return (Constants.OpeningDataTypeBracket & Variables.VariableUtilities.GetDataTypeString_FromDSVariableType(Me.DataType) & Constants.ClosingDataTypeBracket & Constants.Space & Me.Identifier)

			End Function

		End Structure

		''' <summary>Contains an UPPERCASE Identifier, ParameterTypes Array, and ReturnType Property</summary>
		Public Structure Signature

			'Signature:
			'	Identifier
			'	ParameterTypes
			'	ReturnType

			Public ReadOnly Identifier As String
			Public ReadOnly ParameterTypes As Type()
			Public ReadOnly ReturnType As Type

			''' <summary>The Identifier will be ToUpper()-ed</summary>
			Public Sub New(ByVal _Identifier$, ByVal _ParameterTypes As Type(), ByVal _ReturnType As Type)
				Me.Identifier = _Identifier.ToUpper()
				Me.ParameterTypes = _ParameterTypes
				Me.ReturnType = _ReturnType
			End Sub

			Public Shared Operator =(ByVal _SignatureA As Signature, ByVal _SignatureB As Signature) As Boolean
				Try : LogLexingMessage(String.Format("Comparing Function Signature {0} with Signature {1} for Operator=()", _SignatureA.ToString(), _SignatureB.ToString()))

					If _SignatureA.Identifier <> _SignatureB.Identifier Then Return False
					If _SignatureA.ReturnType <> _SignatureB.ReturnType Then Return False

					If _SignatureA.ParameterTypes.Length <> _SignatureB.ParameterTypes.Length Then Return False
					For _ParameterType_Index% = 0 To (_SignatureA.ParameterTypes.Length - 1) Step +1
						If Not _SignatureA.ParameterTypes.ElementAt(_ParameterType_Index) = _SignatureB.ParameterTypes.ElementAt(_ParameterType_Index) Then Return False
					Next

					LogLexingMessage("(...The Function Signatures were equal)")
					Return True

				Catch _Ex As Exception : Throw New DSException("@DSFunction\ProcessTokensToInitialiseFields: " & _Ex.Message, _Ex) : End Try
			End Operator

			Public Shared Operator <>(ByVal _SignatureA As Signature, ByVal _SignatureB As Signature) As Boolean
				Return Not (_SignatureA = _SignatureB)
			End Operator

			''' <summary>Returns a KVP-Serialised String to represent the current Object</summary>
			Public Overrides Function ToString() As String
				Return DocScript.Utilities.KVPSerialisation.GetKVPString({"Identifier", Me.Identifier}, {"ParameterTypes", Me.ParameterTypes.GetStandardArraySerialisation()}, {"ReturnType", Me.ReturnType.Name})
			End Function

		End Structure

		Protected Const InstructionName_$ = "[DSFunction]"

		Public Identifier As String
		Public Parameters As ObjectModel.ReadOnlyCollection(Of Parameter) 'If there are no Parameters, this is an Empty Collection (not Nothing)
		Public ReturnType As Type

#End Region

#Region "Declarations declared by all IStatements"

		Protected ReadOnly Tokens_ As Runtime.Token()
		Public Property ScopedVariables As Runtime.SymbolTable Implements IStatement.ScopedVariables

		Protected Contents_ As IInstruction()
		Public ReadOnly Property Contents As System.Collections.ObjectModel.ReadOnlyCollection(Of IInstruction) Implements IStatement.Contents
			Get
				Return New ObjectModel.ReadOnlyCollection(Of IInstruction)(Me.Contents_)
			End Get
		End Property

#End Region

		''' <summary>Constructs the IInstruction from an existing instance</summary>
		Public Sub New(ByVal _Identifier$, ByVal _ReturnType As Type, ByVal _Parameters As Parameter(), ByVal _Contents As List(Of IInstruction))
			Me.Identifier = _Identifier : Me.ReturnType = _ReturnType : Me.Parameters = (New ObjectModel.ReadOnlyCollection(Of Parameter)(_Parameters.ToList()))
			Me.Contents_ = _Contents.ToArray()
		End Sub

		''' <summary>Constructs the IInstruction from its Tokens</summary>
		Sub New(ByRef _Tokens As Runtime.Token())
			Me.Tokens_ = _Tokens : Me.ProcessTokensToInitialiseFields()
		End Sub

		Public Sub ProcessTokensToInitialiseFields()
			Try : LogLexingMessage("Began constructing a DSFunction...")

				REM Source should look like:
				REM		Function <Void> Main () {LineEnd} ... EndFunction {LineEnd}
				REM		Function <Number> Main (<String@> _CLAs, <Number> _Age) {LineEnd} ... EndFunction {LineEnd}

				REM Tokens should look like:
				REM		[Keyword], [GrammarChar], [DataType], [GrammarChar], [Identifier], [GrammarChar], [GrammarChar], [LineEnd]
				REM		[Keyword], [GrammarChar], [DataType], [GrammarChar], [Identifier], [GrammarChar], ([GrammarChar], [DataType], [GrammarChar], [Identifier])... [LineEnd]

				REM Fields to Initialise:
				REM		Identifier
				REM		ReturnType
				REM		Parameters
				REM		Contents_

				REM Ensure that there are enough tokens to construct the IInstruction
				If Me.Tokens_.Count < Runtime.MinimumRequiredTokens(Me.GetType()) Then Throw New DSValidationException(String.Format("The No. Tokens ({0}) was less than the minimum No. Tokens required to construct the Instruction ({1}).", Me.Tokens_.Count.ToString(), Runtime.MinimumRequiredTokens(Me.GetType()).ToString()), "Tokens: " & Me.Tokens_.GetValuesAndLocationsString())

				REM Ensure that the last Token is a {LineEnd}
				If Not Runtime.BuiltInTPVs.LineEnd_TPV.IsSatisfiedBy(Me.Tokens_.Last()) Then Throw New DSUnexpectedTokenException("The Last Token was not a {LineEnd}", Me.Tokens_)

				REM Ensure that the penultimate Token is the StatementEnd
				Runtime.BuiltInTPVs.StatementEnd_Function_TPV.EnsureIsSatisfiedBy({Me.Tokens_(Me.Tokens_.Length - 2)})

				REM Ensure that the main TPV herefor is satisfied
				Runtime.BuiltInTPVs.DSFunction_UpToIncIdentifier_TPV.EnsureIsSatisfiedBy(Me.Tokens_)

				REM We now know that we have [Function <String> GetName]
				REM So set these Fields...

				REM On the first line, there should be at least 8 Tokens
				'Function < Number > Main ( ) {LineEnd}
				Dim _Statement_FirstLine As Runtime.Token() = Me.Tokens_.UpToAndIncluding(AddressOf Runtime.BuiltInTPVs.LineEnd_TPV.IsSatisfiedBy) 'UpTo the first {LineEnd}
				If Not (_Statement_FirstLine.Length >= 8) Then Throw New DSValidationException("The first line of the Statement was not syntactically-valid, because it didn't contain enough Tokens", Me.Tokens_.GetValuesAndLocationsString())

				REM Ensure that the First Line ends as we expect it to
				If Not _Statement_FirstLine.TakeBlockFromEnd(_Count:=2).SatisfiesTPV(Runtime.BuiltInTPVs.Statement_FirstLineEnding_TPV) Then Throw New DSUnexpectedTokenException("The Statement's First Line should have ended with [)] and [{LineEnd}], but did not.", _Statement_FirstLine)

				REM The DataType should be Token(2)
				Me.ReturnType = Variables.GetDSVariableType_FromDataTypeString(Me.Tokens_(2).Value)

				REM The Identifier should be Token(4)
				Me.Identifier = Me.Tokens_(4).Value

				'Find out about the Parameters next...

				REM Ensure we have an Opening Function Bracket ( in the expected Position
				If Not Runtime.BuiltInTPVs.OpeningFunctionBracket_TPV.IsSatisfiedBy(_Statement_FirstLine(5)) Then Throw New DSUnexpectedTokenException("An Opening Function Bracket " & Constants.OpeningFunctionBracket.ToString().InBrackets() & " was expected but not found", Me.Tokens_.Take(6).ToArray())

				REM Ensure that there is a ) directly before the next {LineEnd}
				If Not Runtime.BuiltInTPVs.ClosingFunctionBracket_TPV.IsSatisfiedBy(_Statement_FirstLine.ElementAt(_Statement_FirstLine.IndexOfFirstWhere(AddressOf Runtime.BuiltInTPVs.LineEnd_TPV.IsSatisfiedBy) - 1)) Then Throw New DSUnexpectedTokenException("A Function Closing Bracket " & Constants.ClosingFunctionBracket & " was expected (but not found to be) directly before the first {LineEnd}", Me.Tokens_.Take(10).ToArray())

				REM If there are no Parameters in this Declaration...
				If Runtime.BuiltInTPVs.Parameterless_DSFunction_TPV.IsSatisfiedBy(_Statement_FirstLine) Then
					Me.Parameters = New ObjectModel.ReadOnlyCollection(Of Parameter)(New List(Of Parameter)())
				Else 'There ARE some Parameters; extract them...

					Dim _DerivedParameters As New List(Of Parameter)()

					'Input could look like:
					'Function < Number > TheMain ( < String@ > _CLAs , < Number > _LetterCount ) {LineEnd}

					'The Parameter Tokens should start at FirstLine(6) and end at just before the First ClosingFunctionBracket )
					'We need to Split by Comma , to get the Groups of Parameter Tokens
					Dim _GroupsOf_ParametersTokens As Runtime.Token()() = _
					 _Statement_FirstLine _
					.Skip(6) _
					.ToArray() _
					.UpToButExcluding(AddressOf Runtime.BuiltInTPVs.ClosingFunctionBracket_TPV.IsSatisfiedBy) _
					.SplitWhere(AddressOf Runtime.BuiltInTPVs.FunctionArgumentSeperator_Comma_TPV.IsSatisfiedBy)

					'_GroupsOf_ParametersTokens looks like:
					'{ < String > Name }, { < Number > Age }, { < String@ > Address }

					'This is the Parameter Derivation Loop
					For Each _ParameterTokens As Runtime.Token() In _GroupsOf_ParametersTokens

						REM There must be 4 Tokens to construct a Parameter, satisfying the Parameter_TPV
						If Not Runtime.BuiltInTPVs.Parameter_TPV.IsSatisfiedBy(_ParameterTokens, _MustBeAsManyTokensAsPatterns:=True) Then Throw New DSUnexpectedTokenException("A Function Parameter was malformed. The ParameterTokens Group was: " & _ParameterTokens.GetValuesAndLocationsString() & ". There may have been a leading or trailing comma, or one comma followed by another.", _Statement_FirstLine)

						Dim _ParameterToAdd_Identifier As [String], _ParameterToAdd_Type As [Type]

						REM Token(1) should be the DataType
						_ParameterToAdd_Type = Variables.GetDSVariableType_FromDataTypeString(_ParameterTokens(1).Value)

						REM Token(3) should be the Identifier
						_ParameterToAdd_Identifier = _ParameterTokens(3).Value

						REM Ensure there is not already a Parameter with this Name
						If _DerivedParameters.Where(Function(_Parameter As Parameter) _Parameter.Identifier = _ParameterToAdd_Identifier).Count > 0 Then Throw New DSValidationException("A Parameter already exists for the current Function with the same Identifier", "Parameter Identifier: " & _ParameterToAdd_Identifier)

						REM Add the Parameter
						_DerivedParameters.Add(New Parameter(_ParameterToAdd_Identifier, _ParameterToAdd_Type))

					Next

					Me.Parameters = (New ObjectModel.ReadOnlyCollection(Of Parameter)(_DerivedParameters))

				End If

				REM Derive this Function's Contents (From [After 1st {LineEnd}] to [1st EndFunction])
				Me.Contents_ = DocScript.Language.Instructions.Statements.StatementUtilities.GetStatementContentsFromTokens(
				 Me.Tokens_ _
				 .Skip(_Statement_FirstLine.Length) _
				 .ToArray() _
				 .UpToButExcluding(AddressOf Runtime.BuiltInTPVs.StatementEnd_Function_TPV.IsSatisfiedBy)
				)

				LogLexingMessage("...Finished constructing a DSFunction Object for " & Language.Constants.OpeningDataTypeBracket & Variables.VariableUtilities.GetDataTypeString_FromDSVariableType(Me.ReturnType) & Language.Constants.ClosingDataTypeBracket & " "c & Me.Identifier.InSquares())
			Catch _Ex As Exception : Throw New DSException("@DSFunction\ProcessTokensToInitialiseFields: " & _Ex.Message, _Ex) : End Try
		End Sub

		''' <summary>DO NOT CALL; Use Run().</summary>
		Public Function Execute(ByVal _SymbolTables As Runtime.SymbolTablesSnapshot) As ExecutionResult Implements IInstruction.Execute
			Throw New DSInvalidCallException("DSFunction.Execute() was called, however this is not the method used to run a DSFunction. Use DSFunction.RunFunction(Arguments...) instead.", "DSFunction\Execute()")
		End Function

		''' <summary>
		''' The Arguments are IDataValues and not IExpressions, because if the Exprs referanced a variable Local to ANOTHER DSFUNCTION, then it wouldn't be accessible here!
		''' DSFunction Symbol Table Rules:
		'''     - A DSFunction may take in several SymbolTables, but only pays attention to the Globals on the bottom
		'''     - A DSFunction only returns the Global Symbol Table
		''' </summary>
		Public Function Run(ByVal _InputSymbolTables As Runtime.SymbolTablesSnapshot, ByVal _Arguments As Variables.IDataValue()) As DocScript.Language.Instructions.ExecutionResult

			LogExecutionMessage(String.Format("Began DSFunction Execution for ({0}) with {1} Argument(s), {2} Child Instruction(s), and {3} Input Symbol Tables...", Me.Identifier, _Arguments.Length.ToString(), Me.Contents.Count.ToString(), _InputSymbolTables.InnerSymbolTables_Count.ToString()))

			REM DSFunction Execution Process
			'	- Ensure that we have the correct number of _Arguments
			'	- Create the SymbolTable for this IStatement → Me.ScopedVariables
			'	- Create a New SymTblsSnapshot from the _InputSymbolTables, and Me.ScopedVars
			'
			'	- Add each (resolved) _Argument as an entry to the Topmost SymTbl, Coerce()-ing each to the Type of the Expected Parameter therefor
			'
			'	- For Each Child IInstruction:
			'		- Pass in _ModifiedSymTbls to Execute()
			'		- Add ExeRes thereof to UpstairsExecutionResults
			'		- Apply the returned SymTbls to _ModifiedSymTbls
			'		- Check if the _ExeRes's ReturnStatus's CurrentDSFunction_ReturnHasOccured is [True]
			'	- Re-apply the Topmost SymTbl's state to Me.ScopedVars
			'	- Add Me.ScopedVars as an Archived UpstairsSymTbl to the _ExeRes

			Try : Dim _ExeRes As ExecutionResult = ExecutionResult.New_AndStartExecutionTimer("DSFunction " & Me.Identifier.InBrackets())

				If Not _Arguments.Length = Me.Parameters.Count Then Throw New DSException(String.Format("The number of Arguments provided to the DSFunction ({0}) was not equal to the number of expected Parameters ({1})", _Arguments.Length.ToString(), Me.Parameters.Count.ToString()))

				REM Each DSFunction is an isloated execution environment; although more than one SymbolTable may have been passed
				REM		to this call (e.g. because the DSFunction is being invoked as part of a FunctionCallExpr or FunctionCall IInstruction)
				REM		, we must only use the GlobalSymbolTable for the BottomStack of the _Modified_SymTbls.
				REM		This is because we would otherwise be herein able to access Local Variables of another DSFunction

				If (_InputSymbolTables.InnerSymbolTables_Count < 1) OrElse (_InputSymbolTables.Bottommost.Description <> Runtime.Constants.GlobalSymbolTable_Description) Then Throw New DSValidationException("The Bottommost SymbolTable passed to a DSFunction Run() must be the Global Symbol Table.", "(The absence of a recognised Global SymTbl. The recognition is done based on the Description of the SymTbl being equal to Runtime.Constants.GlobalSymbolTable_Description$)")

				Me.ScopedVariables = New Runtime.SymbolTable("DSFunction_" & Me.Identifier)
				LogExecutionMessage("(Generated Local SymbolTable for DSFunction " & Me.Identifier.InSquares() & ")")
				Dim _Modified_SymTbls As Runtime.SymbolTablesSnapshot = _InputSymbolTables.Bottommost.[SnapshotContainingJustThis].SnapshotFrom_ThisAndTopmost(_NewTopmost:=Me.ScopedVariables)

				REM Add each (ALREADY-RESOLVED) Argument (applied to this DSFunction at call-time) to the Topmost SymbolTable
				Try
					For _ArgumentIndex% = 0 To (_Arguments.Length - 1) Step +1

						'Coerce
						Dim _Argument_CoercedToParameterType As Variables.IDataValue = _
						   Variables.TypeCoercion.CoerceIDV_IntoTargetIDVType_NoGenericModifier(_InputIDV:=_Arguments.ElementAt(_ArgumentIndex), _TargetOutputType:=Me.Parameters.ElementAt(_ArgumentIndex).DataType)

						'Generate SymTblEntry with Argument Flag
						Dim _Argument_SymTblEntry As Runtime.SymbolTable.ISymbolTableEntry = _
						   Runtime.SymbolTable.GetSymbolTableEntry_FromIDataValue(_InputIDV:=_Argument_CoercedToParameterType.MustNotBeNothing("The Argument was [Nothing]"), _NullValueType_IfInputIDVIsNothing:=Nothing)
						_Argument_SymTblEntry.SetFlags(Runtime.SymbolTable.SymbolTableEntryFlags.DSFunctionParameter)

						'Add to SymbolTable
						_Modified_SymTbls.AddEntryToToTopmost(
						 _Identifier:=Me.Parameters.ElementAt(_ArgumentIndex).Identifier,
						 _Entry:=_Argument_SymTblEntry
						)

					Next
				Catch _Ex As Exception : Throw New DSException("Whilst adding the Arguments to the Local SymbolTable: " & _Ex.Message, _Ex) : End Try

				REM !!!!!!!!!!!!!!!!!!!!!!!!!!!!
				REM	    <Contents Execution>
				REM !!!!!!!!!!!!!!!!!!!!!!!!!!!!

				'No Return has occured yet
				_ExeRes.[ReturnStatus].CurrentDSFunction_ReturnHasOccurred = False

				For Each _ChildInstruction As IInstruction In Me.Contents

					'Log
					LogExecutionMessage("Executing DSFunction " & Me.Identifier.InBrackets() & " Child Instruction of Type " & _ChildInstruction.GetType().Name.InSquares())

					'Execute()
					Dim _ChildInstruction_ExeRes As ExecutionResult = _ChildInstruction.Execute(_Modified_SymTbls) 'Pass in the Globals and Locals

					'Add as an Upstairs ExeRes
					_ExeRes.UpstairsExecutionResults.Add(New Tuple(Of String, ExecutionResult)("ChildInsruction " & _ChildInstruction.GetType().Name.InBrackets(), _ChildInstruction_ExeRes))

					'Update my SymbolTables; 
					_Modified_SymTbls = _ChildInstruction_ExeRes.ResultantSymbolTablesState

					'Work out weather or not to Return from the DSFunction
					If (_ChildInstruction_ExeRes.ReturnStatus.CurrentDSFunction_ReturnHasOccurred <> Nothing) AndAlso (_ChildInstruction_ExeRes.ReturnStatus.CurrentDSFunction_ReturnHasOccurred) Then

						'A Return HAS just occured for the current DSFunction
						_ExeRes.ReturnStatus.CurrentDSFunction_ReturnHasOccurred = True

						'Log
						LogExecutionMessage("A Child Instruction within DSFunction " & Me.Identifier.InBrackets() & " of Type " & _ChildInstruction.GetType().Name.InSquares() & " raised the Return Flag; no further Child Instructions will be executed")

						'Coerce() the ReturnValue into this DSFunction's ReturnType (It's much easier to do this here, as opposed to in ReturnToCaller.Execute(), because that method can't get hold of this DSFunction's Return Type)
						If (_ChildInstruction_ExeRes.ReturnStatus.CurrentDSFunction_ReturnValue IsNot Nothing) Then _ExeRes.ReturnStatus.CurrentDSFunction_ReturnValue = Variables.CoerceIDV_IntoTargetIDVType_NoGenericModifier(_ChildInstruction_ExeRes.ReturnStatus.CurrentDSFunction_ReturnValue, Me.ReturnType)

						'Don't execute any further Contents IInstructions
						Exit For

					End If

				Next

				REM Having exited that Contents-Execution For loop, it's either because:
				REM		- One of the Child IInstructions caused [Exit For] because CurrentDSFunction_ReutrnHasOccured was [True], or...
				REM		- We executed all Child IInstructions without one of them causing a Reutrn (we'll need to use an implicit Return)
				REM We could check which of these scenarios has befallen using the Boolean Expr [_ExeRes.ReturnStatus.CurrentDSFunction_ReturnHasOccurred IsNot Nothing, AndAlso it's value is True]

				'At this point:
				'	- If the DSFunction's Return Type =  Void, then [CurrentDSFunction_ReturnValue] should be [Nothing]
				'	- If the DSFunction's Return Type <> Void, then [CurrentDSFunction_ReturnValue] should be [Something]

				If Not Variables.IsNonVoidFunctionReturnType(Me.ReturnType) Then If (_ExeRes.ReturnStatus.CurrentDSFunction_ReturnValue IsNot Nothing) Then Throw New DSValidationException("The DSFunction declared itself to be <Void>, but produced a Return Value", "Return Value: " & _ExeRes.ReturnStatus.CurrentDSFunction_ReturnValue.ToString())
				If Variables.IsNonVoidFunctionReturnType(Me.ReturnType) Then If (_ExeRes.ReturnStatus.CurrentDSFunction_ReturnValue Is Nothing) Then Throw New DSValidationException("The DSFunction declared itself to be non-Void, but produced no Return Value", "(The absence of a Return Value)")

				REM !!!!!!!!!!!!!!!!!!!!!!!!!!!!
				REM	    </Contents Execution>
				REM !!!!!!!!!!!!!!!!!!!!!!!!!!!!

				REM Re-apply the State to Me.ScopedVars
				If (_Modified_SymTbls.InnerSymbolTables_Count <> 2) Then Throw New DSValidationException("Before re-applying the ScopedVariables for the DSFunction, it was discovered that the _ModifiedSymbolTables contained fewer than 2 Tables (The Globals and the Locals)", "(The incorrect SymTbls)")
				Me.ScopedVariables = _Modified_SymTbls.Topmost : _ExeRes.Archived_SymbolTable = Me.ScopedVariables

				_ExeRes.StopExecutionTimer_AndFinaliseObject(_Modified_SymTbls.Bottommost.SnapshotContainingJustThis) 'Return just the Globals
				LogExecutionMessage(String.Format("...DSFunction [{0}] produced Return Value [{1}].", Me.Identifier, If(_ExeRes.ReturnStatus.CurrentDSFunction_ReturnValue, New Variables.DSString("(No Return Value)")).ToString()), LogEvent.DSEventSeverity.Infomation)
				Return _ExeRes

			Catch _Ex As Exception : Throw New DSException("@DSFunction\Run " & Me.Identifier.InBrackets() & ": " & _Ex.Message, _Ex) : End Try

		End Function

		''' <summary>Contains an UPPERCASE Identifier, ParameterTypes Array, and ReturnType Property</summary>
		Public ReadOnly Property EffectiveSignature As DSFunction.Signature
			Get
				Return New Signature(
				 _Identifier:=Me.Identifier,
				 _ParameterTypes:=Me.Parameters.Select(Of Type)(Function(_Param As Parameter) _Param.DataType).ToArray(),
				 _ReturnType:=Me.ReturnType
				 )
			End Get
		End Property

		''' <summary>Returns an XML Representation of the Instruction, with all Propities and Child Structres included</summary>
		Public Function GetProgramTreeNodeXML() As XElement Implements IStatement.GetProgramTreeNodeXML

			Dim _XElementToReturn As XElement = <DSFunction Identifier=<%= Me.Identifier %> ReturnType=<%= Variables.GetDataTypeString_FromDSVariableType(Me.ReturnType) %>/>

			Dim _ParametersXElement As XElement = <Parameters/>
			For Each _Parameter As Parameter In Me.Parameters
				_ParametersXElement.Add(_Parameter.GetProgramTreeNodeXML())
			Next
			_XElementToReturn.Add(_ParametersXElement)

			Dim _ContentsXElement As XElement = <Contents/>
			For Each _IInstruction As IInstruction In Me.Contents
				_ContentsXElement.Add(_IInstruction.GetProgramTreeNodeXML())
			Next
			_XElementToReturn.Add(_ContentsXElement)

			Return _XElementToReturn

		End Function

		''' <summary>Constructs an instance of the IInstruction class from its XML serialasation</summary>
		Public Shared Function FromXML(ByVal _IInstructionXElement As XElement) As DSFunction
			Try

				Return New DSFunction(
				 _Identifier:=_IInstructionXElement.@Identifier,
				 _ReturnType:=Variables.VariableUtilities.GetDSVariableType_FromDataTypeString(_IInstructionXElement.@ReturnType),
				 _Parameters:=_IInstructionXElement.<Parameters>.First().<Parameter>.Select(Of DSFunction.Parameter)(AddressOf Parameter.FromXML).ToArray(),
				 _Contents:=StatementUtilities.GetStatementContentsFromXML(_IInstructionXElement.<Contents>.First().Elements().ToArray()).ToList()
				)

			Catch _Ex As Exception : Throw New DSException("@DSFunction\FromXML: " & _Ex.Message, _Ex) : End Try
		End Function

		''' <summary>Returns what the Instruction would have looked like in the Source, without an extra LineBreak on the End.</summary>
		Public Overrides Function ToString() As String

			REM E.g.
			'	FUNCTION <NUMBER> MAIN (<STRING@> _CLAS)
			'		
			'		OUTPUT("Hello, World!")
			'		RETURN 0
			'		
			'	ENDFUNCTION

			Return _
			 Constants.Keyword_Function & Constants.Space & Constants.OpeningDataTypeBracket & Variables.VariableUtilities.GetDataTypeString_FromDSVariableType(Me.ReturnType) & Constants.ClosingDataTypeBracket & Constants.Space & Me.Identifier & Constants.Space & Constants.OpeningFunctionBracket & _
			 If(Me.Parameters.Count = 0, "", [String].Join(Constants.FunctionArgumentSeperator & Constants.Space, Me.Parameters.Select(Function(_Parameter As Parameter) _Parameter.ToString()))) & Constants.ClosingFunctionBracket & Constants.LineBreak & _
			 Constants.LineBreak & _
			 [StatementUtilities].ReconstructStatementContentsAsSource(Me.Contents.ToArray()).TabbedInBy(1) & Constants.LineBreak & _
			 Constants.LineBreak & _
			 Constants.StatementEnd_Function

		End Function

		''' <summary>Returns the Value of the current ISymbolTableValue inside a TagName of the Type</summary>
		Public Function ToXML() As System.Xml.Linq.XElement Implements Runtime.SymbolTable.ISymbolTableValue.ToXML
			Return Me.GetProgramTreeNodeXML()
		End Function

	End Class

End Namespace