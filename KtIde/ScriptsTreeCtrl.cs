using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace KtIde
{
	/// <summary>
	/// Summary description for ScriptsTreeCtrl.
	/// </summary>
	public class ScriptsTreeCtrl : System.Windows.Forms.UserControl
	{
		private string scriptPath;
		private string selectedFile;
		public string parentDirectory;
		private KtShell.KtShellControl pythhonShell;
		private SynEditor syntaxEditor;
		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem menuPopupEdit;
		private System.Windows.Forms.MenuItem menuPopupRun;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuPopupChangeDir;
        private MenuItem menuItemDebug;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ScriptsTreeCtrl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
			this.treeView1.TabStop = false;
			
			// IMPORTANT!!
			// The caller who instantiates this object must set the KtShellControl
			// by calling SetKtShell(KtShell.KtShellControl ktshell)

		}

		public bool Initialize(string rootdir)
		{
			bool retval = true;

			// Make sure the string exists
			if (String.Equals(rootdir, null) || rootdir.Length == 0)
				rootdir = Directory.GetCurrentDirectory();

			// See if directory exists, and create it if not
			try
			{
				if (!Directory.Exists(rootdir))
				{
					Directory.CreateDirectory(rootdir);
				}
				this.parentDirectory = rootdir;
				InitializeScriptTree(this.parentDirectory );
				this.treeView1.ContextMenu = this.contextMenu1;
			}
			catch (Exception e)
			{
				string msg = e.Message + "  Error with ScriptDirectory -- see KtLabOptions.xml";
				MessageBox.Show(msg);
				retval = true;
			}
			return retval;
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuPopupEdit = new System.Windows.Forms.MenuItem();
            this.menuPopupRun = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuPopupChangeDir = new System.Windows.Forms.MenuItem();
            this.menuItemDebug = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(216, 384);
            this.treeView1.TabIndex = 0;
            this.treeView1.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.OnBeforeExpand);
            this.treeView1.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnAfterSelect);
            this.treeView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            this.treeView1.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.OnBeforeSelect);
            // 
            // contextMenu1
            // 
            this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuPopupEdit,
            this.menuPopupRun,
            this.menuItemDebug,
            this.menuItem1,
            this.menuPopupChangeDir});
            // 
            // menuPopupEdit
            // 
            this.menuPopupEdit.Index = 0;
            this.menuPopupEdit.Text = "Edit";
            this.menuPopupEdit.Click += new System.EventHandler(this.menuPopupEdit_Click);
            // 
            // menuPopupRun
            // 
            this.menuPopupRun.Index = 1;
            this.menuPopupRun.Text = "Run";
            this.menuPopupRun.Click += new System.EventHandler(this.menuPopupRun_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 3;
            this.menuItem1.Text = "-";
            // 
            // menuPopupChangeDir
            // 
            this.menuPopupChangeDir.Index = 4;
            this.menuPopupChangeDir.Text = "Change dir";
            this.menuPopupChangeDir.Click += new System.EventHandler(this.menuPopupChangeDir_Click);
            // 
            // menuItemDebug
            // 
            this.menuItemDebug.Index = 2;
            this.menuItemDebug.Text = "Debug";
            this.menuItemDebug.Click += new System.EventHandler(this.menuItemDebug_Click);
            // 
            // ScriptsTreeCtrl
            // 
            this.AllowDrop = true;
            this.Controls.Add(this.treeView1);
            this.Name = "ScriptsTreeCtrl";
            this.Size = new System.Drawing.Size(216, 384);
            this.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnDragEnter);
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Set the KtShell object.
		/// </summary>
		public void SetKtShell(KtShell.KtShellControl ktshell)
		{
			this.pythhonShell = ktshell;
		}

		/// <summary>
		/// Set the tree root, populate the tree, and expand the first node.
		/// </summary>
		public void InitializeScriptTree(string parentdir)
		{
			this.scriptPath = parentdir;
			this.treeView1.Nodes.Add(scriptPath);
			this.PopulateTreeView(scriptPath, treeView1.Nodes[0]);
			this.treeView1.Nodes[0].Expand();
		}

		/// <summary>
		/// Populate the tree by recursively walking down the file path.
		/// </summary>
		public void PopulateTreeView(string parentdir, TreeNode parentNode)
		{
			// Get subdirs
			try
			{
				string [] directoryArray = Directory.GetDirectories( parentdir );

				if (directoryArray.Length != 0)
				{
					foreach (string directory in directoryArray)
					{
						string shortdir = directory.Substring(directory.LastIndexOf("\\")+1);
						TreeNode newNode = new TreeNode( shortdir);
						parentNode.Nodes.Add( newNode );

						PopulateTreeView( directory, newNode );
					}
				}
				string [] files = Directory.GetFiles( parentdir, "*.py" );
				foreach (string file in files)
				{
					string shortfile = file.Substring(file.LastIndexOf("\\")+1);
					parentNode.Nodes.Add(shortfile);
				}

			}

				// Catch exceptions
			catch ( UnauthorizedAccessException )
			{
				parentNode.Nodes.Add( "Access denied" );
			}
			catch ( DirectoryNotFoundException )
			{
				string msg = String.Format("Cannot find scripts directory [{0}].", parentdir);
				MessageBox.Show(msg);
			}
		}

		private void OnDoubleClick(object sender, System.EventArgs e)
		{
			// Run the file in the Python Shell (but not directories)
			if (File.Exists(this.selectedFile))
			{
				this.pythhonShell.RunFile(this.selectedFile);
			}
		}

		private void OnAfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			this.selectedFile = e.Node.FullPath;
		}

		private void OnBeforeSelect(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			this.selectedFile = e.Node.FullPath;
		}

		private void OnMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			TreeNode tn = this.treeView1.GetNodeAt(e.X, e.Y);
			if (tn != null)
			{
				this.treeView1.SelectedNode = this.treeView1.GetNodeAt(e.X, e.Y);
				this.selectedFile = this.treeView1.SelectedNode.FullPath;		
			}
		}

		private void OnBeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			// Get a list of the current files and directories here
			string [] directoryArray = Directory.GetDirectories( e.Node.FullPath );
			string [] files = Directory.GetFiles( e.Node.FullPath, "*.py" );
			int nodesNeeded = directoryArray.Length + files.Length;
			
			// Here we repopulate the tree if needed
			if (e.Node.GetNodeCount(false) != (nodesNeeded))
			{
				e.Node.Nodes.Clear();
				foreach (string directory in directoryArray)
				{
					string shortdir = directory.Substring(directory.LastIndexOf("\\")+1);
					e.Node.Nodes.Add(shortdir);
				}
				foreach (string file in files)
				{
					string shortfile = file.Substring(file.LastIndexOf("\\")+1);
					e.Node.Nodes.Add(shortfile);
				}
			}

		}

		private void OnDragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			try
			{
				Array a = (Array)e.Data.GetData(DataFormats.FileDrop);

				if ( a != null )
				{
					// Call owner's OpenFiles asynchronously.
					if (System.IO.Directory.Exists(a.GetValue(0).ToString()) )
					{
						string dir = a.GetValue(0).ToString();
						this.treeView1.Nodes.Add(dir);
						int lastnode = this.treeView1.Nodes.Count - 1;	// Adjust for 0-offset
						this.PopulateTreeView(dir, treeView1.Nodes[lastnode]);
					}

					this.Parent.FindForm().Activate();        // in the case Explorer overlaps owner form
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error in OnDragDrop function: " + ex.Message);
			}
		}

		private void OnDragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop)) 
				e.Effect = DragDropEffects.Copy;
			else
				e.Effect = DragDropEffects.None;
		}

		private void menuPopupEdit_Click(object sender, System.EventArgs e)
		{
			// Edit here
			this.syntaxEditor = new SynEditor(); //this.pythhonShell);
			this.syntaxEditor.SetKtShell(this.pythhonShell);
			// ToDo: error checking for file existing (how do I get the selected file on r-click?)
			if (File.Exists(this.selectedFile))
			{
				this.syntaxEditor.LoadFile(this.selectedFile);
			}
			else
			{
				MessageBox.Show("Unable to load file: " + this.selectedFile);
			}
			syntaxEditor.Show();
		}

		private void menuPopupRun_Click(object sender, System.EventArgs e)
		{
			// Run the file in the Python Shell (but not directories)
			if (File.Exists(this.selectedFile))
			{
				this.pythhonShell.RunFile(this.selectedFile);
			}
		
		}

		private void menuPopupChangeDir_Click(object sender, System.EventArgs e)
		{
			string dir = this.selectedFile;

			// If a file is selected, get the directory
			if (System.IO.File.Exists(dir))
			{
				dir = dir.Substring(0, dir.LastIndexOf("\\"));
			}

			if (System.IO.Directory.Exists(dir))
			{
				string command = "import os; os.chdir(r'{0}')";
				command = string.Format(command, dir);
				this.pythhonShell.RunString(command);
			}
		}

        private void menuItemDebug_Click(object sender, EventArgs e)
        {
            // TODO: Get these arguments from the Options file (this.pythhonShell.options)
            string args = string.Format("--trace -- \"{0}\" ", this.selectedFile);
            // Launch the debugger for this file
            if (File.Exists(this.selectedFile))
            {
                System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo("pydbgr.exe");
                info.Arguments = args;
                info.CreateNoWindow = false;
                //info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Minimized;
                //this.pythhonShell.options.GetMethods
                System.Diagnostics.Process.Start(info);
            }
        }
	}
}
