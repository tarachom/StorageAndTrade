using System;
using System.Windows.Forms;

namespace StorageAndTrade
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Gecko.Xpcom.Initialize("Firefox");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ConfigurationSelectionForm());
        }
    }
}
