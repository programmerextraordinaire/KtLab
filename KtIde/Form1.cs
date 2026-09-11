using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace KtIde
{
    public partial class Form1 : Form
    {
        private const int MaxRecentScriptFolders = 8;
        private ToolStripMenuItem scriptFolderMenu;

        public Form1()
        {
            InitializeComponent();
            //
            // Additional constructor code after InitializeComponent call
            //
            this.scriptsTreeCtrl1.TabStop = false;

            // File > Script folder: choose a folder, or pick a recent one (rebuilt each time it opens)
            this.scriptFolderMenu = new ToolStripMenuItem("&Script folder");
            this.scriptFolderMenu.DropDownItems.Add("");   // Placeholder so the submenu arrow shows
            this.scriptFolderMenu.DropDownOpening += (s, e) => BuildScriptFolderMenu();
            this.fileToolStripMenuItem.DropDownItems.Insert(1, this.scriptFolderMenu);

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
            MessageBox.Show("KtLab v0.3.0 beta\nby Thane Plummer \n\n� Copyright Kokopelli Technology and TKP Corp.\n");
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

        private string OptionsFile
        {
            get { return this.ktShellControl1.options.OptionsFileName; }
        }

        private string CurrentScriptFolder()
        {
            string dir = this.ktShellControl1.options.ScriptDirectory;
            try { return string.IsNullOrEmpty(dir) ? Directory.GetCurrentDirectory() : Path.GetFullPath(dir); }
            catch (Exception) { return dir; }   // Bad path typed into the XML: show it as-is
        }

        private void BuildScriptFolderMenu()
        {
            string current = CurrentScriptFolder();
            ToolStripItemCollection items = this.scriptFolderMenu.DropDownItems;
            items.Clear();
            items.Add("&Choose folder...", null, (s, e) => ChooseScriptFolder());
            items.Add(new ToolStripSeparator());
            foreach (string dir in RecentScriptFolders())
            {
                // Clicking the checked (current) folder just refreshes the tree
                ToolStripMenuItem item = new ToolStripMenuItem(dir.Replace("&", "&&"), null, (s, e) => SetScriptFolder(dir));
                item.Checked = string.Equals(dir, current, StringComparison.OrdinalIgnoreCase);
                item.Enabled = Directory.Exists(dir);   // e.g. an unplugged drive: greyed out, but kept
                items.Add(item);
            }
        }

        private void ChooseScriptFolder()
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Choose the folder of Python scripts to show in the tree";
                dlg.SelectedPath = CurrentScriptFolder();
                if (dlg.ShowDialog(this) == DialogResult.OK)
                    SetScriptFolder(dlg.SelectedPath);
            }
        }

        private void SetScriptFolder(string dir)
        {
            dir = Path.GetFullPath(dir);
            this.ktShellControl1.options.ScriptDirectory = dir;
            this.scriptsTreeCtrl1.Initialize(dir);
            try
            {
                SaveScriptFolder(dir, RecentScriptFolders());
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not save the script folder to " + OptionsFile + ":\n" + e.Message);
            }
        }

        /// <summary>
        /// Current script folder first, then the saved recent ones, without duplicates.
        /// </summary>
        private List<string> RecentScriptFolders()
        {
            List<string> dirs = new List<string>();
            AddRecent(dirs, CurrentScriptFolder());
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(OptionsFile);
                foreach (XmlNode node in doc.SelectNodes("/Options/RecentScriptFolders/string"))
                    AddRecent(dirs, node.InnerText);
            }
            catch (Exception) { }   // No options file or bad XML: just the current folder
            return dirs;
        }

        private static void AddRecent(List<string> dirs, string dir)
        {
            if (!string.IsNullOrEmpty(dir) && dirs.Count < MaxRecentScriptFolders
                && !dirs.Exists(d => string.Equals(d, dir, StringComparison.OrdinalIgnoreCase)))
                dirs.Add(dir);
        }

        /// <summary>
        /// Writes ScriptDirectory and RecentScriptFolders into the options file, leaving every
        /// other setting (and the file's hand-edited layout) exactly as the user wrote it.
        /// </summary>
        private void SaveScriptFolder(string dir, List<string> recent)
        {
            string file = OptionsFile;
            if (!File.Exists(file))     // Only case where the whole Options object is written
                using (StreamWriter w = new StreamWriter(file))
                    new XmlSerializer(typeof(KtShell.Options)).Serialize(w, this.ktShellControl1.options);

            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.Load(file);
            XmlElement root = doc.DocumentElement;
            XmlElement scriptDir = root["ScriptDirectory"] ?? (XmlElement)root.AppendChild(doc.CreateElement("ScriptDirectory"));
            scriptDir.InnerText = dir;

            XmlElement list = root["RecentScriptFolders"];
            if (list == null)
            {
                list = (XmlElement)root.InsertAfter(doc.CreateElement("RecentScriptFolders"), scriptDir);
                root.InsertBefore(doc.CreateWhitespace("\r\n  "), list);
            }
            list.RemoveAll();
            foreach (string d in recent)
            {
                list.AppendChild(doc.CreateWhitespace("\r\n    "));
                list.AppendChild(doc.CreateElement("string")).InnerText = d;
            }
            list.AppendChild(doc.CreateWhitespace("\r\n  "));
            doc.Save(file);
        }
    }
}