using System.Linq; using DocScript.CompilerExtentions; using DocScript.Language.Instructions;
using DocScript.Language.Instructions.Statements; using DocScript.Language.Variables; using DocScript.Runtime;

namespace DSEmbeddingExample {

    public partial class MainWindow : System.Windows.Forms.Form {
        
        public MainWindow() { this.InitializeComponent(); }

        public void ExecuteSource() {

            try {

                global::DocScript.Runtime.Program _Program = Program.FromSource(
                    _Source: this.SourceTextBox.Text,
                    _ExeCxt: ref MyExecutionContext.TheExeCxt
                );

                global::DocScript.Language.Instructions.ExecutionResult _ExeRes =
                    _Program.Run(new System.String[] {})
                ;

                this.ExecutionResult_StatusLabel.Text =
                    $"Ran in {_ExeRes.ExecutionTimeMS}ms; exit-code {_ExeRes.ReturnStatus.Program_ExitCode}"
                ;

            } catch (System.Exception _Ex) {
                System.Windows.Forms.MessageBox.Show(_Ex.Message, "Execution Error");
            }

        }

    }

    /// <summary>For injecting the custom functions into the DocScript runtime...</summary>
    public static class MyExecutionContext {

        public static ExecutionContext TheExeCxt = new ExecutionContext(
            _ID:                 "Example_GUI_ExeCxt",
            _RootFolder:         new System.IO.DirectoryInfo(System.IO.Directory.GetCurrentDirectory()),
            _InputDelegate:      ExecutionContext.GUIDefault.InputDelegate,
            _OutputDelegate:     ExecutionContext.GUIDefault.OutputDelegate,
            _BuiltInFunctions:   ExecutionContext.AllStandardBuiltInFunctions.Concat(
                new BuiltInFunction[] { MyExecutionContext.AddNumbers_ }
            ).ToArray()
        );

        private static BuiltInFunction AddNumbers_ {
            get {

                System.String _BifName = "AddNumbers";

                return new BuiltInFunction(
                    _Identifier: _BifName,
                    _ReturnType: typeof(DSNumber),
                    _ExpectedParameters: new DSFunction.Parameter[] {
                        new DSFunction.Parameter("_numOne", typeof(DSNumber)),
                        new DSFunction.Parameter("_numTwo", typeof(DSNumber))
                    },
                    _Action: new BuiltInFunction.BuiltInFunctionDelegate(
                        (SymbolTablesSnapshot _SymTbls, IDataValue[] _Arguments) => {

                            ExecutionResult _ExeRes = ExecutionResult.New_AndStartExecutionTimer(@"Example-BIF\" + _BifName);

                            System.Double _numOne = _Arguments[0].Coerce<DSNumber>().Value;
                            System.Double _numTwo = _Arguments[1].Coerce<DSNumber>().Value;

                            _ExeRes.ReturnStatus.BuiltInFunction_ReturnValue = new DSNumber(
                                _numOne + _numTwo
                            );

                            return _ExeRes.StopExecutionTimer_AndFinaliseObject(ref _SymTbls);

                        }
                    )
                ) {
                    Description = "Adds _numOne to _numTwo, returning the result."
                };

            }
        }

    }

}