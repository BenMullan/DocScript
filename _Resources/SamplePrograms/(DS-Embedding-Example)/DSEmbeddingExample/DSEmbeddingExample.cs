using System;
using System.Windows.Forms;

namespace DSEmbeddingExample {
    internal static class DSEmbeddingExample {
        
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }

    }
}