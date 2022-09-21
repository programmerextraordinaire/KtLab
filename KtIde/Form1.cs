using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KtIde
{
    public partial class Form1 : Form
    {
        

        public Form1()
        {
            InitializeComponent();
            //
            // Additional constructor code after InitializeComponent call
            //
            this.scriptsTreeCtrl1.TabStop = false;
            try
            {
                Object thisLock = new Object();

                //this.ktShell.Initialize();

                this.ktShellControl1.LoadOptions();
                string rootdir = this.ktShellControl1.options.ScriptDirectory;
                if (this.scriptsTreeCtrl1.Initialize(rootdir))
                {
                    this.scriptsTreeCtrl1.SetKtShell(this.ktShellControl1);
                    this.Show();
                    //this.ktShellCtrl1.TextFocus();
                    lock (thisLock)
                    {
                        this.ktShellControl1.Initialize();
                    }
                    //this.ktShellControl1.RunPythonInitScripts();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                //throw;
            }
        }

        public void InitializeIde()
        {
            this.ktShellControl1.RunPythonInitScripts();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("KtLab v0.3.0 beta\nby Thane Plummer \n\n© Copyright Kokopelli Technology and TKP Corp.\n");
        }

        private void clearWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ktShellControl1.Clear();
        }

        private void restartPythonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ktShellControl1.RestartProcess();
        }

        private void newEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.syntaxEditor = new SynEditor();
            this.syntaxEditor.SetKtShell(this.ktShellControl1);
            syntaxEditor.Show();
        }
    }
}