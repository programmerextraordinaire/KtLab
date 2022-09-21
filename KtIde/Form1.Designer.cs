namespace KtIde
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shellToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restartPythonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.scriptsTreeCtrl1 = new KtIde.ScriptsTreeCtrl();
            this.ktShellControl1 = new KtShell.KtShellControl();
            //this.ktShell = new ShellRedirect();
            this.menuStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.shellToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(937, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newEditorToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newEditorToolStripMenuItem
            // 
            this.newEditorToolStripMenuItem.Name = "newEditorToolStripMenuItem";
            this.newEditorToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newEditorToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.newEditorToolStripMenuItem.Text = "&New editor";
            this.newEditorToolStripMenuItem.ToolTipText = "Open a new editor window";
            this.newEditorToolStripMenuItem.Click += new System.EventHandler(this.newEditorToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // shellToolStripMenuItem
            // 
            this.shellToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearWindowToolStripMenuItem,
            this.restartPythonToolStripMenuItem});
            this.shellToolStripMenuItem.Name = "shellToolStripMenuItem";
            this.shellToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.shellToolStripMenuItem.Text = "Shell";
            // 
            // clearWindowToolStripMenuItem
            // 
            this.clearWindowToolStripMenuItem.Name = "clearWindowToolStripMenuItem";
            this.clearWindowToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.clearWindowToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.clearWindowToolStripMenuItem.Text = "Clear window";
            this.clearWindowToolStripMenuItem.Click += new System.EventHandler(this.clearWindowToolStripMenuItem_Click);
            // 
            // restartPythonToolStripMenuItem
            // 
            this.restartPythonToolStripMenuItem.Name = "restartPythonToolStripMenuItem";
            this.restartPythonToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.restartPythonToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.restartPythonToolStripMenuItem.Text = "Restart Python";
            this.restartPythonToolStripMenuItem.Click += new System.EventHandler(this.restartPythonToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.scriptsTreeCtrl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ktShellControl1);
            this.splitContainer1.Size = new System.Drawing.Size(937, 577);
            this.splitContainer1.SplitterDistance = 225;
            this.splitContainer1.TabIndex = 2;
            // 
            // scriptsTreeCtrl1
            // 
            this.scriptsTreeCtrl1.AllowDrop = true;
            this.scriptsTreeCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scriptsTreeCtrl1.Location = new System.Drawing.Point(0, 0);
            this.scriptsTreeCtrl1.Name = "scriptsTreeCtrl1";
            this.scriptsTreeCtrl1.Size = new System.Drawing.Size(225, 577);
            this.scriptsTreeCtrl1.TabIndex = 0;
            // 
            // ktShellControl1
            // 
            this.ktShellControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ktShellControl1.Location = new System.Drawing.Point(0, 0);
            this.ktShellControl1.Name = "ktShellControl1";
            this.ktShellControl1.Size = new System.Drawing.Size(708, 577);
            this.ktShellControl1.TabIndex = 1;
            //this.ktShell.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.ktShell.Location = new System.Drawing.Point(0, 0);
            //this.ktShell.Name = "ktShellControl1";
            //this.ktShell.Size = new System.Drawing.Size(708, 577);
            //this.ktShell.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(937, 601);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Kt IDE";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shellToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restartPythonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private KtIde.ScriptsTreeCtrl scriptsTreeCtrl1;
        private KtShell.KtShellControl ktShellControl1;
        private KtIde.ShellRedirect ktShell;
        private SynEditor syntaxEditor;
    }
}

