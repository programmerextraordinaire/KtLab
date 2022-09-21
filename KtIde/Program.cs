using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KtIde
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var ide = new Form1();
            //ide.InitializeIde();
            Application.Run(ide);
        }
    }
}