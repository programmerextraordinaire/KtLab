using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
//using ActiproSoftware.SyntaxEditor;


namespace KtIde
{


	/// <summary>
	/// Summary description for SyntaxEditor.
	/// </summary>
	public class SynEditor : System.Windows.Forms.Form 
	{
		/// <summary>
		/// Indicates a type of application action.
		/// </summary>
		/// <remarks>
		/// This is simply used to have all menu items and toolbars call the same centralized procedure.
		/// </remarks>
		private enum AppAction 
		{
			FileNew, 
			FileOpen,
			FileSave,
			FilePrint,
			FilePrintPreview,
			EditCut,
			EditCopy,
			EditPaste,
			EditDelete,
			EditUndo,
			EditRedo,
			EditFind,
			EditReplace,
			EditGotoLine,
			ToolsIndent,
			ToolsOutdent,
			ToolsCommentSelection,
			ToolsUncommentSelection,
			ToolsToggleBookmark,
			ToolsNextBookmark,
			ToolsPreviousBookmark,
			ToolsClearBookmarks,
			Run,
		}

		/// <summary>
		/// Find/replace options.
		/// </summary>
		//private FindReplaceOptions findReplaceOptions = new FindReplaceOptions();
		private bool isModified;

		private System.Windows.Forms.MainMenu mainMenu1;
		private KtShell.KtShellControl pythhonShell;
		private System.Windows.Forms.MenuItem menuRun;
		private System.Windows.Forms.ToolBar toolBar;
		private System.Windows.Forms.ToolBarButton fileNewButton;
		private System.Windows.Forms.ToolBarButton fileOpenButton;
		private System.Windows.Forms.ToolBarButton fileSaveButton;
		private System.Windows.Forms.ToolBarButton separator1;
		private System.Windows.Forms.ToolBarButton filePrintButton;
		private System.Windows.Forms.ToolBarButton filePrintPreviewButton;
		private System.Windows.Forms.ToolBarButton separator2;
		private System.Windows.Forms.ToolBarButton editCutButton;
		private System.Windows.Forms.ToolBarButton editCopyButton;
		private System.Windows.Forms.ToolBarButton editPasteButton;
		private System.Windows.Forms.ToolBarButton editDeleteButton;
		private System.Windows.Forms.ToolBarButton separator3;
		private System.Windows.Forms.ToolBarButton separator4;
		private System.Windows.Forms.ToolBarButton separator5;
		private System.Windows.Forms.ToolBarButton separator6;
		private System.Windows.Forms.ToolBarButton separator7;
		private System.Windows.Forms.ToolBarButton runButton;
		private KtIde.ModifiedEditor editor;
		private System.Windows.Forms.MenuItem menuItem11;
		private System.Windows.Forms.MenuItem menuItem13;
		private System.Windows.Forms.MenuItem menuItem16;
		private System.Windows.Forms.MenuItem menuItem22;
		private System.Windows.Forms.MenuItem menuItem33;
		private System.Windows.Forms.MenuItem menuFile;
		private System.Windows.Forms.MenuItem menuFileExit;
		private System.Windows.Forms.MenuItem menuEdit;
		private System.Windows.Forms.MenuItem menuHelp;
		private System.Windows.Forms.MenuItem menuFileNew;
		private System.Windows.Forms.MenuItem menuFileOpen;
		private System.Windows.Forms.MenuItem menuFileSave;
		private System.Windows.Forms.MenuItem menuFilePrint;
		private System.Windows.Forms.MenuItem menuFilePrintPreview;
		private System.Windows.Forms.MenuItem menuEditUndo;
		private System.Windows.Forms.MenuItem menuEditRedo;
		private System.Windows.Forms.MenuItem menuEditCut;
		private System.Windows.Forms.MenuItem menuEditCopy;
		private System.Windows.Forms.MenuItem menuEditPaste;
		private System.Windows.Forms.MenuItem menuEditDelete;
		private System.Windows.Forms.MenuItem menuEditFind;
		private System.Windows.Forms.MenuItem menuEditReplace;
		private System.Windows.Forms.MenuItem menuTools;
		private System.Windows.Forms.MenuItem menuToolsComment;
		private System.Windows.Forms.MenuItem menuToolsUncomment;
		private System.Windows.Forms.MenuItem menuToolsNextBookmark;
		private System.Windows.Forms.MenuItem menuToolsPrevBookmark;
		private System.Windows.Forms.MenuItem menuToolsClearBookmarks;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.ContextMenu contextMenu;
		private System.Windows.Forms.MenuItem contextUndoMenuItem;
		private System.Windows.Forms.MenuItem contextRedoMenuItem;
		private System.Windows.Forms.MenuItem contextBarMenuItem;
		private System.Windows.Forms.MenuItem contextCutMenuItem;
		private System.Windows.Forms.MenuItem contextCopyMenuItem;
		private System.Windows.Forms.MenuItem contextPasteMenuItem;
		private System.Windows.Forms.MenuItem menuToolsToggleBookmark;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuEditGotoLine;
		private System.Windows.Forms.ToolBarButton toolsOutdentButton;
		private System.Windows.Forms.ToolBarButton toolsIndentButton;
		private System.Windows.Forms.ToolBarButton toolsCommentSelectionButton;
		private System.Windows.Forms.ToolBarButton toolsUncommentSelectionButton;
		private System.Windows.Forms.ToolBarButton toolsToggleBookmarkButton;
		private System.Windows.Forms.ToolBarButton toolsNextBookmarkButton;
		private System.Windows.Forms.ToolBarButton toolsPreviousBookmarkButton;
		private System.Windows.Forms.ToolBarButton toolsClearBookmarksButton;
		private System.Windows.Forms.ToolBarButton separator8;
		private System.Windows.Forms.ToolBarButton editUndoButton;
		private System.Windows.Forms.ToolBarButton editRedoButton;
		private System.Windows.Forms.ContextMenu redoNames;
		private System.Windows.Forms.MenuItem redoName1MenuItem;
		private System.Windows.Forms.MenuItem redoName2MenuItem;
		private System.Windows.Forms.MenuItem redoName3MenuItem;
		private System.Windows.Forms.MenuItem redoName4MenuItem;
		private System.Windows.Forms.MenuItem redoName5MenuItem;
		private System.Windows.Forms.ContextMenu undoNames;
		private System.Windows.Forms.MenuItem undoName1MenuItem;
		private System.Windows.Forms.MenuItem undoName2MenuItem;
		private System.Windows.Forms.MenuItem undoName3MenuItem;
		private System.Windows.Forms.MenuItem undoName4MenuItem;
		private System.Windows.Forms.MenuItem undoName5MenuItem;
		private System.Windows.Forms.MenuItem menuEditFindNext;
		private System.Windows.Forms.ImageList imageList;
        //private ScintillaNET.Scintilla scintilla1;
        private ScintillaNet.Scintilla scintilla1;
		//private ActiproSoftware.SyntaxEditor.SyntaxEditor editor;
		private System.ComponentModel.IContainer components;

		public SynEditor() //PythonShell pythhonshell)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            //this.pythhonShell = pythhonshell;
            this.scintilla1.ConfigurationManager.Language = "Python";
            this.isModified = scintilla1.Modified;		// used to determine whether or not the text has changed.
            //scintilla1.IndentationGuides = ScintillaNET.IndentView.LookBoth;
            //scintilla1.Margins.Left = 40;

            scintilla1.Indentation.SmartIndentType = ScintillaNet.SmartIndent.Simple;
            scintilla1.Styles.LineNumber.IsVisible = true;
            scintilla1.Margins.Margin0.Width = 40;
            scintilla1.Margins.Margin0.Type = ScintillaNet.MarginType.Number;
            scintilla1.Margins.Margin1.Width = 10;
            scintilla1.Margins.Margin1.Type = ScintillaNet.MarginType.Symbol;
            scintilla1.Margins.Margin1.IsMarkerMargin = true;
            scintilla1.Margins.Margin1.IsClickable = true;

            //scintilla1.Indentation.ShowGuides = true; // Looks TERRIBLE!
            //scintilla1.Folding.IsEnabled = true;
            //scintilla1.Folding.MarkerScheme = ScintillaNet.FoldMarkerScheme.BoxPlusMinus;
            // IMPORTANT!!
            // The caller who instantiates this object must set the KtShellControl
            // by calling SetKtShell(KtShell.KtShellControl ktshell)
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SynEditor));
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuFile = new System.Windows.Forms.MenuItem();
            this.menuFileNew = new System.Windows.Forms.MenuItem();
            this.menuFileOpen = new System.Windows.Forms.MenuItem();
            this.menuFileSave = new System.Windows.Forms.MenuItem();
            this.menuItem13 = new System.Windows.Forms.MenuItem();
            this.menuFilePrint = new System.Windows.Forms.MenuItem();
            this.menuFilePrintPreview = new System.Windows.Forms.MenuItem();
            this.menuItem11 = new System.Windows.Forms.MenuItem();
            this.menuFileExit = new System.Windows.Forms.MenuItem();
            this.menuEdit = new System.Windows.Forms.MenuItem();
            this.menuEditUndo = new System.Windows.Forms.MenuItem();
            this.menuEditRedo = new System.Windows.Forms.MenuItem();
            this.menuItem16 = new System.Windows.Forms.MenuItem();
            this.menuEditCut = new System.Windows.Forms.MenuItem();
            this.menuEditCopy = new System.Windows.Forms.MenuItem();
            this.menuEditPaste = new System.Windows.Forms.MenuItem();
            this.menuEditDelete = new System.Windows.Forms.MenuItem();
            this.menuItem22 = new System.Windows.Forms.MenuItem();
            this.menuEditFind = new System.Windows.Forms.MenuItem();
            this.menuEditReplace = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuEditGotoLine = new System.Windows.Forms.MenuItem();
            this.menuEditFindNext = new System.Windows.Forms.MenuItem();
            this.menuTools = new System.Windows.Forms.MenuItem();
            this.menuToolsComment = new System.Windows.Forms.MenuItem();
            this.menuToolsUncomment = new System.Windows.Forms.MenuItem();
            this.menuItem33 = new System.Windows.Forms.MenuItem();
            this.menuToolsToggleBookmark = new System.Windows.Forms.MenuItem();
            this.menuToolsNextBookmark = new System.Windows.Forms.MenuItem();
            this.menuToolsPrevBookmark = new System.Windows.Forms.MenuItem();
            this.menuToolsClearBookmarks = new System.Windows.Forms.MenuItem();
            this.menuRun = new System.Windows.Forms.MenuItem();
            this.menuHelp = new System.Windows.Forms.MenuItem();
            this.toolBar = new System.Windows.Forms.ToolBar();
            this.fileNewButton = new System.Windows.Forms.ToolBarButton();
            this.fileOpenButton = new System.Windows.Forms.ToolBarButton();
            this.fileSaveButton = new System.Windows.Forms.ToolBarButton();
            this.separator1 = new System.Windows.Forms.ToolBarButton();
            this.filePrintButton = new System.Windows.Forms.ToolBarButton();
            this.filePrintPreviewButton = new System.Windows.Forms.ToolBarButton();
            this.separator2 = new System.Windows.Forms.ToolBarButton();
            this.editCutButton = new System.Windows.Forms.ToolBarButton();
            this.editCopyButton = new System.Windows.Forms.ToolBarButton();
            this.editPasteButton = new System.Windows.Forms.ToolBarButton();
            this.editDeleteButton = new System.Windows.Forms.ToolBarButton();
            this.separator3 = new System.Windows.Forms.ToolBarButton();
            this.editUndoButton = new System.Windows.Forms.ToolBarButton();
            this.undoNames = new System.Windows.Forms.ContextMenu();
            this.undoName1MenuItem = new System.Windows.Forms.MenuItem();
            this.undoName2MenuItem = new System.Windows.Forms.MenuItem();
            this.undoName3MenuItem = new System.Windows.Forms.MenuItem();
            this.undoName4MenuItem = new System.Windows.Forms.MenuItem();
            this.undoName5MenuItem = new System.Windows.Forms.MenuItem();
            this.editRedoButton = new System.Windows.Forms.ToolBarButton();
            this.redoNames = new System.Windows.Forms.ContextMenu();
            this.redoName1MenuItem = new System.Windows.Forms.MenuItem();
            this.redoName2MenuItem = new System.Windows.Forms.MenuItem();
            this.redoName3MenuItem = new System.Windows.Forms.MenuItem();
            this.redoName4MenuItem = new System.Windows.Forms.MenuItem();
            this.redoName5MenuItem = new System.Windows.Forms.MenuItem();
            this.separator4 = new System.Windows.Forms.ToolBarButton();
            this.toolsOutdentButton = new System.Windows.Forms.ToolBarButton();
            this.toolsIndentButton = new System.Windows.Forms.ToolBarButton();
            this.separator5 = new System.Windows.Forms.ToolBarButton();
            this.toolsCommentSelectionButton = new System.Windows.Forms.ToolBarButton();
            this.toolsUncommentSelectionButton = new System.Windows.Forms.ToolBarButton();
            this.separator6 = new System.Windows.Forms.ToolBarButton();
            this.toolsToggleBookmarkButton = new System.Windows.Forms.ToolBarButton();
            this.toolsNextBookmarkButton = new System.Windows.Forms.ToolBarButton();
            this.toolsPreviousBookmarkButton = new System.Windows.Forms.ToolBarButton();
            this.toolsClearBookmarksButton = new System.Windows.Forms.ToolBarButton();
            this.separator7 = new System.Windows.Forms.ToolBarButton();
            this.runButton = new System.Windows.Forms.ToolBarButton();
            this.separator8 = new System.Windows.Forms.ToolBarButton();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.contextMenu = new System.Windows.Forms.ContextMenu();
            this.contextUndoMenuItem = new System.Windows.Forms.MenuItem();
            this.contextRedoMenuItem = new System.Windows.Forms.MenuItem();
            this.contextBarMenuItem = new System.Windows.Forms.MenuItem();
            this.contextCutMenuItem = new System.Windows.Forms.MenuItem();
            this.contextCopyMenuItem = new System.Windows.Forms.MenuItem();
            this.contextPasteMenuItem = new System.Windows.Forms.MenuItem();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            //this.scintilla1 = new ScintillaNET.Scintilla();
            this.scintilla1 = new ScintillaNet.Scintilla();
            ((System.ComponentModel.ISupportInitialize)(this.scintilla1)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuFile,
            this.menuEdit,
            this.menuTools,
            this.menuRun,
            this.menuHelp});
            // 
            // menuFile
            // 
            this.menuFile.Index = 0;
            this.menuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuFileNew,
            this.menuFileOpen,
            this.menuFileSave,
            this.menuItem13,
            this.menuFilePrint,
            this.menuFilePrintPreview,
            this.menuItem11,
            this.menuFileExit});
            this.menuFile.Text = "&File";
            // 
            // menuFileNew
            // 
            this.menuFileNew.Index = 0;
            this.menuFileNew.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
            this.menuFileNew.Text = "New";
            this.menuFileNew.Click += new System.EventHandler(this.menuFileNew_Click);
            // 
            // menuFileOpen
            // 
            this.menuFileOpen.Index = 1;
            this.menuFileOpen.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.menuFileOpen.Text = "Open";
            this.menuFileOpen.Click += new System.EventHandler(this.menuFileOpen_Click);
            // 
            // menuFileSave
            // 
            this.menuFileSave.Index = 2;
            this.menuFileSave.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.menuFileSave.Text = "Save";
            this.menuFileSave.Click += new System.EventHandler(this.menuFileSave_Click);
            // 
            // menuItem13
            // 
            this.menuItem13.Index = 3;
            this.menuItem13.Text = "-";
            // 
            // menuFilePrint
            // 
            this.menuFilePrint.Index = 4;
            this.menuFilePrint.Shortcut = System.Windows.Forms.Shortcut.CtrlP;
            this.menuFilePrint.Text = "Print";
            this.menuFilePrint.Click += new System.EventHandler(this.menuFilePrint_Click);
            // 
            // menuFilePrintPreview
            // 
            this.menuFilePrintPreview.Index = 5;
            this.menuFilePrintPreview.Text = "Print preview";
            this.menuFilePrintPreview.Click += new System.EventHandler(this.menuFilePrintPreview_Click);
            // 
            // menuItem11
            // 
            this.menuItem11.Index = 6;
            this.menuItem11.Text = "-";
            // 
            // menuFileExit
            // 
            this.menuFileExit.Index = 7;
            this.menuFileExit.Shortcut = System.Windows.Forms.Shortcut.AltF4;
            this.menuFileExit.Text = "E&xit";
            this.menuFileExit.Click += new System.EventHandler(this.menuFileExit_Click);
            // 
            // menuEdit
            // 
            this.menuEdit.Index = 1;
            this.menuEdit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuEditUndo,
            this.menuEditRedo,
            this.menuItem16,
            this.menuEditCut,
            this.menuEditCopy,
            this.menuEditPaste,
            this.menuEditDelete,
            this.menuItem22,
            this.menuEditFind,
            this.menuEditReplace,
            this.menuItem1,
            this.menuEditGotoLine,
            this.menuEditFindNext});
            this.menuEdit.Text = "Edit";
            // 
            // menuEditUndo
            // 
            this.menuEditUndo.Index = 0;
            this.menuEditUndo.Shortcut = System.Windows.Forms.Shortcut.CtrlZ;
            this.menuEditUndo.Text = "Undo";
            this.menuEditUndo.Click += new System.EventHandler(this.menuEditUndo_Click);
            // 
            // menuEditRedo
            // 
            this.menuEditRedo.Index = 1;
            this.menuEditRedo.Shortcut = System.Windows.Forms.Shortcut.CtrlY;
            this.menuEditRedo.Text = "Redo";
            this.menuEditRedo.Click += new System.EventHandler(this.menuEditRedo_Click);
            // 
            // menuItem16
            // 
            this.menuItem16.Index = 2;
            this.menuItem16.Text = "-";
            // 
            // menuEditCut
            // 
            this.menuEditCut.Index = 3;
            this.menuEditCut.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
            this.menuEditCut.Text = "Cut";
            this.menuEditCut.Click += new System.EventHandler(this.menuEditCut_Click);
            // 
            // menuEditCopy
            // 
            this.menuEditCopy.Index = 4;
            this.menuEditCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
            this.menuEditCopy.Text = "Copy";
            this.menuEditCopy.Click += new System.EventHandler(this.menuEditCopy_Click);
            // 
            // menuEditPaste
            // 
            this.menuEditPaste.Index = 5;
            this.menuEditPaste.Shortcut = System.Windows.Forms.Shortcut.CtrlV;
            this.menuEditPaste.Text = "Paste";
            this.menuEditPaste.Click += new System.EventHandler(this.menuEditPaste_Click);
            // 
            // menuEditDelete
            // 
            this.menuEditDelete.Index = 6;
            this.menuEditDelete.Shortcut = System.Windows.Forms.Shortcut.Del;
            this.menuEditDelete.Text = "Delete";
            this.menuEditDelete.Click += new System.EventHandler(this.menuEditDelete_Click);
            // 
            // menuItem22
            // 
            this.menuItem22.Index = 7;
            this.menuItem22.Text = "-";
            // 
            // menuEditFind
            // 
            this.menuEditFind.Index = 8;
            this.menuEditFind.Shortcut = System.Windows.Forms.Shortcut.CtrlF;
            this.menuEditFind.Text = "Find";
            this.menuEditFind.Click += new System.EventHandler(this.menuEditFind_Click);
            // 
            // menuEditReplace
            // 
            this.menuEditReplace.Index = 9;
            this.menuEditReplace.Shortcut = System.Windows.Forms.Shortcut.CtrlH;
            this.menuEditReplace.Text = "Replace";
            this.menuEditReplace.Click += new System.EventHandler(this.menuEditReplace_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 10;
            this.menuItem1.Text = "-";
            // 
            // menuEditGotoLine
            // 
            this.menuEditGotoLine.Index = 11;
            this.menuEditGotoLine.Shortcut = System.Windows.Forms.Shortcut.CtrlG;
            this.menuEditGotoLine.Text = "Go to line";
            this.menuEditGotoLine.Click += new System.EventHandler(this.menuEditGotoLine_Click);
            // 
            // menuEditFindNext
            // 
            this.menuEditFindNext.Index = 12;
            this.menuEditFindNext.Shortcut = System.Windows.Forms.Shortcut.F3;
            this.menuEditFindNext.Text = "Find Next";
            this.menuEditFindNext.Click += new System.EventHandler(this.menuEditFindNext_Click);
            // 
            // menuTools
            // 
            this.menuTools.Index = 2;
            this.menuTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuToolsComment,
            this.menuToolsUncomment,
            this.menuItem33,
            this.menuToolsToggleBookmark,
            this.menuToolsNextBookmark,
            this.menuToolsPrevBookmark,
            this.menuToolsClearBookmarks});
            this.menuTools.Text = "Tools";
            // 
            // menuToolsComment
            // 
            this.menuToolsComment.Index = 0;
            this.menuToolsComment.Text = "Comment";
            this.menuToolsComment.Click += new System.EventHandler(this.menuToolsComment_Click);
            // 
            // menuToolsUncomment
            // 
            this.menuToolsUncomment.Index = 1;
            this.menuToolsUncomment.Text = "Toggle Comment";
            this.menuToolsUncomment.Click += new System.EventHandler(this.menuToolsUncomment_Click);
            // 
            // menuItem33
            // 
            this.menuItem33.Index = 2;
            this.menuItem33.Text = "-";
            // 
            // menuToolsToggleBookmark
            // 
            this.menuToolsToggleBookmark.Index = 3;
            this.menuToolsToggleBookmark.Shortcut = System.Windows.Forms.Shortcut.CtrlF2;
            this.menuToolsToggleBookmark.Text = "Toggle Bookmark";
            this.menuToolsToggleBookmark.Click += new System.EventHandler(this.menuToolsToggleBookmark_Click);
            // 
            // menuToolsNextBookmark
            // 
            this.menuToolsNextBookmark.Index = 4;
            this.menuToolsNextBookmark.Shortcut = System.Windows.Forms.Shortcut.F2;
            this.menuToolsNextBookmark.Text = "Next bookmark";
            this.menuToolsNextBookmark.Click += new System.EventHandler(this.menuToolsNextBookmark_Click);
            // 
            // menuToolsPrevBookmark
            // 
            this.menuToolsPrevBookmark.Index = 5;
            this.menuToolsPrevBookmark.Text = "Prev bookmark";
            this.menuToolsPrevBookmark.Click += new System.EventHandler(this.menuToolsPrevBookmark_Click);
            // 
            // menuToolsClearBookmarks
            // 
            this.menuToolsClearBookmarks.Index = 6;
            this.menuToolsClearBookmarks.Text = "Clear all bookmarks";
            this.menuToolsClearBookmarks.Click += new System.EventHandler(this.menuToolsClearBookmarks_Click);
            // 
            // menuRun
            // 
            this.menuRun.Index = 3;
            this.menuRun.Shortcut = System.Windows.Forms.Shortcut.F5;
            this.menuRun.Text = "&Run";
            this.menuRun.Click += new System.EventHandler(this.menuRun_Click);
            // 
            // menuHelp
            // 
            this.menuHelp.Index = 4;
            this.menuHelp.Text = "Help";
            this.menuHelp.Click += new System.EventHandler(this.menuHelp_Click);
            // 
            // toolBar
            // 
            this.toolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.toolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.fileNewButton,
            this.fileOpenButton,
            this.fileSaveButton,
            this.separator1,
            this.filePrintButton,
            this.filePrintPreviewButton,
            this.separator2,
            this.editCutButton,
            this.editCopyButton,
            this.editPasteButton,
            this.editDeleteButton,
            this.separator3,
            this.editUndoButton,
            this.editRedoButton,
            this.separator4,
            this.toolsOutdentButton,
            this.toolsIndentButton,
            this.separator5,
            this.toolsCommentSelectionButton,
            this.toolsUncommentSelectionButton,
            this.separator6,
            this.toolsToggleBookmarkButton,
            this.toolsNextBookmarkButton,
            this.toolsPreviousBookmarkButton,
            this.toolsClearBookmarksButton,
            this.separator7,
            this.runButton,
            this.separator8});
            this.toolBar.DropDownArrows = true;
            this.toolBar.ImageList = this.imageList;
            this.toolBar.Location = new System.Drawing.Point(0, 0);
            this.toolBar.Name = "toolBar";
            this.toolBar.ShowToolTips = true;
            this.toolBar.Size = new System.Drawing.Size(720, 28);
            this.toolBar.TabIndex = 13;
            this.toolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar_ButtonClick);
            // 
            // fileNewButton
            // 
            this.fileNewButton.ImageIndex = 0;
            this.fileNewButton.Name = "fileNewButton";
            this.fileNewButton.Tag = "FileNew";
            this.fileNewButton.ToolTipText = "New Document";
            // 
            // fileOpenButton
            // 
            this.fileOpenButton.ImageIndex = 1;
            this.fileOpenButton.Name = "fileOpenButton";
            this.fileOpenButton.Tag = "FileOpen";
            this.fileOpenButton.ToolTipText = "Open Document";
            // 
            // fileSaveButton
            // 
            this.fileSaveButton.ImageIndex = 2;
            this.fileSaveButton.Name = "fileSaveButton";
            this.fileSaveButton.Tag = "FileSave";
            this.fileSaveButton.ToolTipText = "Save Document";
            // 
            // separator1
            // 
            this.separator1.Name = "separator1";
            this.separator1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // filePrintButton
            // 
            this.filePrintButton.ImageIndex = 3;
            this.filePrintButton.Name = "filePrintButton";
            this.filePrintButton.Tag = "FilePrint";
            this.filePrintButton.ToolTipText = "Print";
            // 
            // filePrintPreviewButton
            // 
            this.filePrintPreviewButton.ImageIndex = 4;
            this.filePrintPreviewButton.Name = "filePrintPreviewButton";
            this.filePrintPreviewButton.Tag = "FilePrintPreview";
            this.filePrintPreviewButton.ToolTipText = "Print Preview";
            // 
            // separator2
            // 
            this.separator2.Name = "separator2";
            this.separator2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // editCutButton
            // 
            this.editCutButton.ImageIndex = 5;
            this.editCutButton.Name = "editCutButton";
            this.editCutButton.Tag = "EditCut";
            this.editCutButton.ToolTipText = "Cut";
            // 
            // editCopyButton
            // 
            this.editCopyButton.ImageIndex = 6;
            this.editCopyButton.Name = "editCopyButton";
            this.editCopyButton.Tag = "EditCopy";
            this.editCopyButton.ToolTipText = "Copy";
            // 
            // editPasteButton
            // 
            this.editPasteButton.ImageIndex = 7;
            this.editPasteButton.Name = "editPasteButton";
            this.editPasteButton.Tag = "EditPaste";
            this.editPasteButton.ToolTipText = "Paste";
            // 
            // editDeleteButton
            // 
            this.editDeleteButton.ImageIndex = 8;
            this.editDeleteButton.Name = "editDeleteButton";
            this.editDeleteButton.Tag = "EditDelete";
            this.editDeleteButton.ToolTipText = "Delete";
            // 
            // separator3
            // 
            this.separator3.Name = "separator3";
            this.separator3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // editUndoButton
            // 
            this.editUndoButton.DropDownMenu = this.undoNames;
            this.editUndoButton.ImageIndex = 9;
            this.editUndoButton.Name = "editUndoButton";
            this.editUndoButton.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
            this.editUndoButton.Tag = "EditUndo";
            this.editUndoButton.ToolTipText = "Undo";
            // 
            // undoNames
            // 
            this.undoNames.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.undoName1MenuItem,
            this.undoName2MenuItem,
            this.undoName3MenuItem,
            this.undoName4MenuItem,
            this.undoName5MenuItem});
            this.undoNames.Popup += new System.EventHandler(this.undoNames_Popup);
            // 
            // undoName1MenuItem
            // 
            this.undoName1MenuItem.Index = 0;
            this.undoName1MenuItem.Text = "Undo1";
            this.undoName1MenuItem.Click += new System.EventHandler(this.undoName1MenuItem_Click);
            // 
            // undoName2MenuItem
            // 
            this.undoName2MenuItem.Index = 1;
            this.undoName2MenuItem.Text = "Undo2";
            this.undoName2MenuItem.Click += new System.EventHandler(this.undoName2MenuItem_Click);
            // 
            // undoName3MenuItem
            // 
            this.undoName3MenuItem.Index = 2;
            this.undoName3MenuItem.Text = "Undo3";
            this.undoName3MenuItem.Click += new System.EventHandler(this.undoName3MenuItem_Click);
            // 
            // undoName4MenuItem
            // 
            this.undoName4MenuItem.Index = 3;
            this.undoName4MenuItem.Text = "Undo4";
            this.undoName4MenuItem.Click += new System.EventHandler(this.undoName4MenuItem_Click);
            // 
            // undoName5MenuItem
            // 
            this.undoName5MenuItem.Index = 4;
            this.undoName5MenuItem.Text = "Undo5";
            this.undoName5MenuItem.Click += new System.EventHandler(this.undoName5MenuItem_Click);
            // 
            // editRedoButton
            // 
            this.editRedoButton.DropDownMenu = this.redoNames;
            this.editRedoButton.ImageIndex = 10;
            this.editRedoButton.Name = "editRedoButton";
            this.editRedoButton.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
            this.editRedoButton.Tag = "EditRedo";
            this.editRedoButton.ToolTipText = "Redo";
            // 
            // redoNames
            // 
            this.redoNames.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.redoName1MenuItem,
            this.redoName2MenuItem,
            this.redoName3MenuItem,
            this.redoName4MenuItem,
            this.redoName5MenuItem});
            this.redoNames.Popup += new System.EventHandler(this.redoNames_Popup);
            // 
            // redoName1MenuItem
            // 
            this.redoName1MenuItem.Index = 0;
            this.redoName1MenuItem.Text = "Redo1";
            this.redoName1MenuItem.Click += new System.EventHandler(this.redoName1MenuItem_Click);
            // 
            // redoName2MenuItem
            // 
            this.redoName2MenuItem.Index = 1;
            this.redoName2MenuItem.Text = "Redo2";
            this.redoName2MenuItem.Click += new System.EventHandler(this.redoName2MenuItem_Click);
            // 
            // redoName3MenuItem
            // 
            this.redoName3MenuItem.Index = 2;
            this.redoName3MenuItem.Text = "Redo3";
            this.redoName3MenuItem.Click += new System.EventHandler(this.redoName3MenuItem_Click);
            // 
            // redoName4MenuItem
            // 
            this.redoName4MenuItem.Index = 3;
            this.redoName4MenuItem.Text = "Redo4";
            this.redoName4MenuItem.Click += new System.EventHandler(this.redoName4MenuItem_Click);
            // 
            // redoName5MenuItem
            // 
            this.redoName5MenuItem.Index = 4;
            this.redoName5MenuItem.Text = "Redo5";
            this.redoName5MenuItem.Click += new System.EventHandler(this.redoName5MenuItem_Click);
            // 
            // separator4
            // 
            this.separator4.Name = "separator4";
            this.separator4.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolsOutdentButton
            // 
            this.toolsOutdentButton.ImageIndex = 11;
            this.toolsOutdentButton.Name = "toolsOutdentButton";
            this.toolsOutdentButton.Tag = "ToolsOutdent";
            this.toolsOutdentButton.ToolTipText = "Outdent";
            // 
            // toolsIndentButton
            // 
            this.toolsIndentButton.ImageIndex = 12;
            this.toolsIndentButton.Name = "toolsIndentButton";
            this.toolsIndentButton.Tag = "ToolsIndent";
            this.toolsIndentButton.ToolTipText = "Indent";
            // 
            // separator5
            // 
            this.separator5.Name = "separator5";
            this.separator5.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolsCommentSelectionButton
            // 
            this.toolsCommentSelectionButton.ImageIndex = 13;
            this.toolsCommentSelectionButton.Name = "toolsCommentSelectionButton";
            this.toolsCommentSelectionButton.Tag = "ToolsCommentSelection";
            this.toolsCommentSelectionButton.ToolTipText = "Comment Selection";
            // 
            // toolsUncommentSelectionButton
            // 
            this.toolsUncommentSelectionButton.ImageIndex = 14;
            this.toolsUncommentSelectionButton.Name = "toolsUncommentSelectionButton";
            this.toolsUncommentSelectionButton.Tag = "ToolsUncommentSelection";
            this.toolsUncommentSelectionButton.ToolTipText = "Uncomment Selection";
            // 
            // separator6
            // 
            this.separator6.Name = "separator6";
            this.separator6.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolsToggleBookmarkButton
            // 
            this.toolsToggleBookmarkButton.ImageIndex = 15;
            this.toolsToggleBookmarkButton.Name = "toolsToggleBookmarkButton";
            this.toolsToggleBookmarkButton.Tag = "ToolsToggleBookmark";
            this.toolsToggleBookmarkButton.ToolTipText = "Toggle Bookmark";
            // 
            // toolsNextBookmarkButton
            // 
            this.toolsNextBookmarkButton.ImageIndex = 16;
            this.toolsNextBookmarkButton.Name = "toolsNextBookmarkButton";
            this.toolsNextBookmarkButton.Tag = "ToolsNextBookmark";
            this.toolsNextBookmarkButton.ToolTipText = "Next Bookmark";
            // 
            // toolsPreviousBookmarkButton
            // 
            this.toolsPreviousBookmarkButton.ImageIndex = 17;
            this.toolsPreviousBookmarkButton.Name = "toolsPreviousBookmarkButton";
            this.toolsPreviousBookmarkButton.Tag = "ToolsPreviousBookmark";
            this.toolsPreviousBookmarkButton.ToolTipText = "Previous Bookmark";
            // 
            // toolsClearBookmarksButton
            // 
            this.toolsClearBookmarksButton.ImageIndex = 18;
            this.toolsClearBookmarksButton.Name = "toolsClearBookmarksButton";
            this.toolsClearBookmarksButton.Tag = "ToolsClearBookmarks";
            this.toolsClearBookmarksButton.ToolTipText = "Clear Bookmarks";
            // 
            // separator7
            // 
            this.separator7.Name = "separator7";
            this.separator7.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // runButton
            // 
            this.runButton.ImageIndex = 19;
            this.runButton.Name = "runButton";
            this.runButton.Tag = "Run";
            this.runButton.ToolTipText = "Run Script (F5)";
            // 
            // separator8
            // 
            this.separator8.Name = "separator8";
            this.separator8.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "");
            this.imageList.Images.SetKeyName(1, "");
            this.imageList.Images.SetKeyName(2, "");
            this.imageList.Images.SetKeyName(3, "");
            this.imageList.Images.SetKeyName(4, "");
            this.imageList.Images.SetKeyName(5, "");
            this.imageList.Images.SetKeyName(6, "");
            this.imageList.Images.SetKeyName(7, "");
            this.imageList.Images.SetKeyName(8, "");
            this.imageList.Images.SetKeyName(9, "");
            this.imageList.Images.SetKeyName(10, "");
            this.imageList.Images.SetKeyName(11, "");
            this.imageList.Images.SetKeyName(12, "");
            this.imageList.Images.SetKeyName(13, "");
            this.imageList.Images.SetKeyName(14, "");
            this.imageList.Images.SetKeyName(15, "");
            this.imageList.Images.SetKeyName(16, "");
            this.imageList.Images.SetKeyName(17, "");
            this.imageList.Images.SetKeyName(18, "");
            this.imageList.Images.SetKeyName(19, "");
            // 
            // contextMenu
            // 
            this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.contextUndoMenuItem,
            this.contextRedoMenuItem,
            this.contextBarMenuItem,
            this.contextCutMenuItem,
            this.contextCopyMenuItem,
            this.contextPasteMenuItem});
            // 
            // contextUndoMenuItem
            // 
            this.contextUndoMenuItem.Index = 0;
            this.contextUndoMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlZ;
            this.contextUndoMenuItem.Text = "Undo";
            this.contextUndoMenuItem.Click += new System.EventHandler(this.contextUndoMenuItem_Click);
            // 
            // contextRedoMenuItem
            // 
            this.contextRedoMenuItem.Index = 1;
            this.contextRedoMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlY;
            this.contextRedoMenuItem.Text = "Redo";
            this.contextRedoMenuItem.Click += new System.EventHandler(this.contextRedoMenuItem_Click);
            // 
            // contextBarMenuItem
            // 
            this.contextBarMenuItem.Index = 2;
            this.contextBarMenuItem.Text = "-";
            // 
            // contextCutMenuItem
            // 
            this.contextCutMenuItem.Index = 3;
            this.contextCutMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
            this.contextCutMenuItem.Text = "Cut";
            this.contextCutMenuItem.Click += new System.EventHandler(this.contextCutMenuItem_Click);
            // 
            // contextCopyMenuItem
            // 
            this.contextCopyMenuItem.Index = 4;
            this.contextCopyMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
            this.contextCopyMenuItem.Text = "Copy";
            this.contextCopyMenuItem.Click += new System.EventHandler(this.contextCopyMenuItem_Click);
            // 
            // contextPasteMenuItem
            // 
            this.contextPasteMenuItem.Index = 5;
            this.contextPasteMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlV;
            this.contextPasteMenuItem.Text = "Paste";
            this.contextPasteMenuItem.Click += new System.EventHandler(this.contextPasteMenuItem_Click);
            // 
            // scintilla1
            // 
            this.scintilla1.ConfigurationManager.CustomLocation = ".";
            this.scintilla1.ConfigurationManager.Language = "Python";
            //this.scintilla1.LexerLanguage = "Python";
            this.scintilla1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintilla1.Indentation.IndentWidth = 4;
            this.scintilla1.Indentation.SmartIndentType = ScintillaNet.SmartIndent.Simple;
            this.scintilla1.Indentation.TabIndents = false;
            this.scintilla1.Indentation.TabWidth = 4;
            this.scintilla1.Indentation.UseTabs = false;
            this.scintilla1.Location = new System.Drawing.Point(0, 28);
            this.scintilla1.Name = "scintilla1";
            this.scintilla1.Size = new System.Drawing.Size(720, 448);
            this.scintilla1.Styles.BraceBad.FontName = "Verdana";
            this.scintilla1.Styles.BraceLight.FontName = "Verdana";
            this.scintilla1.Styles.ControlChar.FontName = "Verdana";
            this.scintilla1.Styles.Default.FontName = "Verdana";
            this.scintilla1.Styles.IndentGuide.FontName = "Verdana";
            this.scintilla1.Styles.LastPredefined.FontName = "Verdana";
            this.scintilla1.Styles.LineNumber.FontName = "Verdana";
            this.scintilla1.Styles.Max.FontName = "Verdana";
            this.scintilla1.TabIndex = 14;
            this.scintilla1.MouseHover += new System.EventHandler(this.OnMouseHover);
            this.scintilla1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.scintilla1_KeyDown);
            // 
            // SynEditor
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(720, 476);
            this.Controls.Add(this.scintilla1);
            this.Controls.Add(this.toolBar);
            this.Menu = this.mainMenu1;
            this.Name = "SynEditor";
            this.Text = "KtEdit - (untitled)";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.OnClosing);
            ((System.ComponentModel.ISupportInitialize)(this.scintilla1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        void scintilla1_KeyDown(object sender, KeyEventArgs e)
        {
            SetDirtyFlag();
        }
		#endregion

		/// <summary>
		/// Set the KtShell object.
		/// </summary>
		public void SetKtShell(KtShell.KtShellControl ktshell)
		{
			this.pythhonShell = ktshell;
		}

		public void LoadFile(string filename)
		{
			// ToDo: Make sure the file exists...
			if (File.Exists(filename))
			{
				try
				{
					this.saveFileDialog.FileName = filename;
					this.Text = "KtEdit - " + filename;
                    using (System.IO.StreamReader sr = new StreamReader(filename))
                    {
                        scintilla1.Text = sr.ReadToEnd();
                    }
				}
				catch (UnauthorizedAccessException)
				{
					MessageBox.Show("Error opening file " + filename);
				}
			}
		}


		private void OnTrigger(object sender, EventArgs e) // ActiproSoftware.SyntaxEditor.TriggerEventArgs e)
		{
            //if (editor.Document.Language.Key == "Python") 
            //{
            //    switch (e.Trigger.Key) 
            //    {
            //        case "MemberListTrigger": 
            //        {
            //            // Construct full name of item to see if reflection can be used... iterate backwards through the token stream
            //            TokenStream stream = editor.Document.GetTokenStream(editor.Document.Tokens.IndexOf(
            //                editor.SelectedView.Selection.EndOffset - 1));
            //            string fullName = String.Empty;
            //            int periods = 0;
            //            while (stream.Position > 0) 
            //            {
            //                Token token = stream.ReadReverse();
            //                switch (token.Key) 
            //                {
            //                    case "IdentifierToken":
            //                    case "NativeTypeToken":
            //                        fullName = editor.Document.GetTokenText(token) + fullName;
            //                        break;
            //                    case "PunctuationToken":
            //                        if ((token.Length == 1) && (editor.Document.GetTokenText(token) == ".")) 
            //                        {
            //                            fullName = editor.Document.GetTokenText(token) + fullName;
            //                            periods++;
            //                        }
            //                        else
            //                            stream.Position = 0;
            //                        break;
            //                    default:
            //                        stream.Position = 0;
            //                        break;
            //                }
            //            }

            //            // If a full name is found...
            //            if (fullName.Length > 0) 
            //            {
            //                // Get the member list
            //                IntelliPromptMemberList memberList = editor.IntelliPrompt.MemberList;

            //                // Set IntelliPrompt ImageList
            //                memberList.ImageList = SyntaxEditor.ReflectionImageList;

            //                // Add items to the list
            //                memberList.Clear();

            //                // Show the list
            //                if (0 == memberList.Count)
            //                {
            //                    // Here's the Python interface
            //                    string members = "";//this.pythhonShell.GetTokenMemberList(fullName);

            //                    // Now process what Python sent
            //                    if (null != members)
            //                    {
            //                        string delimStr = ",";
            //                        char [] delimiter = delimStr.ToCharArray();
            //                        string trimStr = "[] \t\r\n";
            //                        char [] trimchars = trimStr.ToCharArray();
            //                        string [] split = null;
            //                        string str;

            //                        split = members.Split(delimiter, 256);
            //                        foreach (string s in split) 
            //                        {
            //                            str = s.Trim(trimchars);
            //                            if (str.Length > 1 && str[0] != '_' && str.Length < 24)
            //                                memberList.Add(new IntelliPromptMemberListItem(str, 0));
            //                        }
            //                    }
            //                }
            //                if (memberList.Count > 0)
            //                    memberList.Show();
            //            }
            //        }	// end of case  MemberListTrigger
            //            break;
            //        default:
            //            break;
            //    }  // end of switch
            //}	// end of  if-Python 
		
		}

		private void OnEnterPressed(object sender, System.EventArgs e)
		{
			SetDirtyFlag();
			// Hide the tooltip if it's showing
            //if (editor.IntelliPrompt.InfoTip.Visible)
            //    editor.IntelliPrompt.InfoTip.Hide();
            //// Can we send the line secretly to Python?
            //int offset = editor.Caret.Offset;
            //int lineIndex = editor.Document.Lines.IndexOf(offset);
            //string lineText = editor.Document.Lines[lineIndex].Text;
			//MessageBox.Show(lineText);
		}

		private void OnKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			SetDirtyFlag();

            switch (e.KeyChar)
            {
                //case '(':
                //    {
                //        // Get the offset
                //        int offset = editor.SelectedView.Selection.EndOffset;

                //        // Get the text stream
                //        TextStream stream = editor.Document.GetTextStream(offset);

                //        // Get the language
                //        SyntaxLanguage language = stream.CurrentToken.Language;

                //        // If in C#...
                //        if (language.Key == "Python")
                //        {
                //            // Get information about the current token
                //            Token token = editor.Document.Tokens.GetTokenAtOffset(editor.Caret.Offset);
                //            string tooltip = "";//this.pythhonShell.GetTooltip(token.ToString());
                //            //String * GetTooltip(String * token)
                //            if (null != tooltip)
                //            {
                //                // Show an info tip
                //                editor.IntelliPrompt.InfoTip.Info.Clear();
                //                editor.IntelliPrompt.InfoTip.Info.Add(tooltip);
                //                //editor.IntelliPrompt.InfoTip.Info.Add(@"void <b>Control.Invalidate</b>()<br/>" +
                //                //	@"Invalidates a specific region of the control and causes a paint message to be sent to the control.");
                //                editor.IntelliPrompt.InfoTip.Show(offset);
                //            }
                //        }
                //    }
                //    break;
                //case ')':
                //    editor.IntelliPrompt.InfoTip.Hide();
                //    break;
                default:
                    break;
            }
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Execute an application action.
		/// </summary>
		/// <param name="action">
		/// An <c>AppAction</c> specifying the type of action to take.
		/// </param>
		/// <remarks>
		/// This procedure is a centralized place for handling menu and toolbar commands.
		/// </remarks>
		private void ExecuteAppAction(AppAction action) 
		{
            string filename = saveFileDialog.FileName;
            // Based on the action passed, do something...
            switch (action) 
            {
                case AppAction.ToolsClearBookmarks:
                    // Clear all bookmarks
                    scintilla1.Commands.Execute(ScintillaNet.BindableCommand.ToggleCaretSticky);
                    //editor.Document.Indicators.Clear(BookmarkIndicator.DefaultName);
                    break;
                case AppAction.ToolsCommentSelection:
                    // Comment the currently selected text
                    scintilla1.Commands.Execute(ScintillaNet.BindableCommand.LineComment);
                    SetDirtyFlag();
                    break;
                case AppAction.EditCopy:
                    // Copy the currently selected text
                    //scintilla1.ExecuteCmd(ScintillaNET.Command.SelectionDuplicate);
                    scintilla1.Commands.Execute(ScintillaNet.BindableCommand.Copy);
                    SetDirtyFlag();
                    break;
                case AppAction.EditCut:
                    // Cut the currently selected text
                    scintilla1.Commands.Execute(ScintillaNet.BindableCommand.Cut);
                    //editor.SelectedView.CutToClipboard();
                    SetDirtyFlag();
                    break;
                case AppAction.EditDelete:
                    // Delete the currently selected text
                    scintilla1.Commands.Execute(ScintillaNet.BindableCommand.Cut);
                    //editor.SelectedView.Delete();
                    SetDirtyFlag();
                    break;
                //				case AppAction.EditGotoLine:
                //					break;
                case AppAction.ToolsIndent:
                    // Indent the currently selected text
                    scintilla1.Commands.Execute(ScintillaNet.BindableCommand.Tab);
                    SetDirtyFlag();
                    break;
                case AppAction.ToolsNextBookmark:
                    // Move to the next bookmark
                    scintilla1.Commands.Execute(ScintillaNet.BindableCommand.NextSnippetRange);
                    //editor.SelectedView.GotoNextLineIndicator(BookmarkIndicator.DefaultName);
                    break;
            //    case AppAction.ToolsOutdent:
            //        // Outdent the currently selected text
            //        editor.SelectedView.Outdent();
            //        SetDirtyFlag();
            //        break;
                case AppAction.EditPaste:
                    // Paste text from the clipboard
                    scintilla1.Commands.Execute(ScintillaNet.BindableCommand.Paste);
                    SetDirtyFlag();
                    break;
            //    case AppAction.ToolsPreviousBookmark:
            //        // Move to the previous bookmark
            //        editor.SelectedView.GotoPreviousLineIndicator(BookmarkIndicator.DefaultName);
            //        break;
                case AppAction.EditRedo:
                    // Perform a redo action
                    //scintilla1.Redo();
                    scintilla1.UndoRedo.Redo();
                    SetDirtyFlag();
                    break;
                case AppAction.ToolsToggleBookmark:
                    // Toggle the bookmark at the current line
                    scintilla1.Commands.Execute(ScintillaNet.BindableCommand.ToggleCaretSticky);
                    break;
                case AppAction.ToolsUncommentSelection:
                    // Uncomment the currently selected text
                    scintilla1.Commands.Execute(ScintillaNet.BindableCommand.ToggleLineComment);
                    SetDirtyFlag();
                    break;
                case AppAction.EditUndo:
                    // Perform an undo action
                    //scintilla1.Undo();
                    scintilla1.UndoRedo.Undo();
                    SetDirtyFlag();
                    break;
                case AppAction.EditFind:
                    // Show the find/replace form
                    // If a word is selected, use that as the search string
                    string findstr = scintilla1.FindReplace.LastFindString;
                    if (scintilla1.Selection.Text.Length > 1)
                    {
                        findstr = scintilla1.Selection.Text;
                    }
                    scintilla1.FindReplace.Window.MruFind.Add(findstr);
                    scintilla1.FindReplace.Window.ShowDialog();
                    break;
                case AppAction.FileNew:
                    // Simulate creating a new file by clearing the text in the current document
                    if (scintilla1.Modified)
                    {
                        MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                        DialogResult result;
                        result = MessageBox.Show(this, "Save file?", "File modified", buttons,
                            MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                        if (result == DialogResult.Yes)
                        {
                            ExecuteAppAction(AppAction.FileSave);
                        }
                    }
                    scintilla1.Text = "";
                    this.isModified = false;
                    this.saveFileDialog.FileName = "";
                    this.Text = "KtEdit - (untitled)";

                    // Turn off auto-loading language example text
                    //fileAutoLoadLanguageExampleTextMenuItem.Checked = false;
                    break;
                case AppAction.FileOpen:
                    // Open a document
                    openFileDialog.Filter = "Python Documents (*.py)|*.py|Text Documents (*.txt)|*.txt|All Documents (*.*)|*.*";
                    if (openFileDialog.ShowDialog() == DialogResult.Cancel) break;
                    saveFileDialog.FileName = openFileDialog.FileName;
                    LoadFile(saveFileDialog.FileName);
                    this.Text = "KtEdit - " + openFileDialog.FileName;

                    // Turn off auto-loading language example text
                    //fileAutoLoadLanguageExampleTextMenuItem.Checked = false;
                    break;
                //				case AppAction.FilePageSetup: 
                //				{
                //					// Show the page setup
                //					PageSetupDialog dialog = new PageSetupDialog();
                //					dialog.Document = editor.PrintSettings.PrintDocument;
                //					dialog.ShowDialog(this);
                //					break;
                //				}
                case AppAction.FilePrint:
                    // Implement to use the DocumentTitle property to make the current filename appear at the top of each page
                    scintilla1.Printing.PrintDocument.DocumentName = saveFileDialog.FileName;

                    // Show the print dialog and print
                    scintilla1.Printing.Print();
                    break;
                case AppAction.FilePrintPreview:
                    // Implement to use the DocumentTitle property to make the current filename appear at the top of each page
                    scintilla1.Printing.PrintDocument.DocumentName = saveFileDialog.FileName;

                    // Show the print preview dialog
                    scintilla1.Printing.PrintPreview();
                    break;
                case AppAction.FileSave:
                    // Save a document
                    // First check to see if we have a filename; if so, just save without dialog
                    if (String.Equals(filename, ""))
                    {
                        filename = "(untitled)";
                        saveFileDialog.Filter = "Python Documents (*.py)|*.py|Text Documents (*.txt)|*.txt|All Documents (*.*)|*.*";
                        if (saveFileDialog.ShowDialog() == DialogResult.Cancel) 
                            break;
                        filename = saveFileDialog.FileName;
                        SaveFile(filename);
                        // ToDo: Fix this editor.Document.ChangeLineModificationMarkColor(editor.LineModificationMarkingColor, Color.Lime);
                    }
                    else
                    {
                        SaveFile(filename);
                    }
                    this.isModified = false;
                    this.Text = "KtEdit - " + filename;	// Set the window title
                    break;
                case AppAction.Run:
                    filename = saveFileDialog.FileName;
                    if (String.Equals(filename, ""))
                        filename = "_tmpfile.py";
                    SaveFile(filename);
                    this.pythhonShell.RunFile(filename);
                    break;
            }
		}


        private void SaveFile(string filename)
        {
            using (System.IO.StreamWriter sw = new StreamWriter(filename))
            {
                sw.Write(this.scintilla1.Text);
            }
        }

		/////////////////////////////////////////////////////////////////////////////////////////////////////


   

		/////////////////////////////////////////////////////////////////////////////////////////////////////
		// MENU/TOOLBAR EVENT HANDLERS
		/////////////////////////////////////////////////////////////////////////////////////////////////////

		private void toolBar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			AppAction action = (AppAction)Enum.Parse(typeof(AppAction), e.Button.Tag.ToString());
			this.ExecuteAppAction(action);
		}

		private void menuFileNew_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.FileNew);
		}

		private void menuFileOpen_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.FileOpen);
		}

		private void menuFileSave_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.FileSave);
		}

		private void menuFilePrint_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.FilePrint);
		}

		private void menuFilePrintPreview_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.FilePrintPreview);
		}

		private void menuFileExit_Click(object sender, System.EventArgs e)
		{
			// Close the form
			this.Close();
		}

		private void menuEditUndo_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.EditUndo);
		}

		private void menuEditRedo_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.EditRedo);
		}

		private void menuEditCut_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.EditCut);
		}

		private void menuEditCopy_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.EditCopy);
		}

		private void menuEditPaste_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.EditPaste);
		}

		private void menuEditDelete_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.EditDelete);
		}

		private void menuEditFind_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.EditFind);
		}

		private void menuEditReplace_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.EditReplace);
		}

		private void menuEditGotoLine_Click(object sender, System.EventArgs e)
		{
			// Show the goto line form
            //scintilla1.GotoPosition()
            scintilla1.GoTo.ShowGoToDialog();
		}

		private void menuToolsComment_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.ToolsCommentSelection);
		}

		private void menuToolsUncomment_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.ToolsUncommentSelection);
		}

		private void menuToolsToggleBookmark_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.ToolsToggleBookmark);
		}

		private void menuToolsNextBookmark_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.ToolsNextBookmark);
		}

		private void menuToolsPrevBookmark_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.ToolsPreviousBookmark);
		}

		private void menuToolsClearBookmarks_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.ToolsClearBookmarks);
		}

		private void menuRun_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.Run);
		}

		private void menuHelp_Click(object sender, System.EventArgs e)
		{
			// ToDo - Add help
		}

		private void contextUndoMenuItem_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.EditUndo);
		}

		private void contextRedoMenuItem_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.EditRedo);
		}

		private void contextCutMenuItem_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.EditCut);
		}

		private void contextCopyMenuItem_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.EditCopy);
		}

		private void contextPasteMenuItem_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.EditPaste);
		}

		private void undoNames_Popup(object sender, System.EventArgs e)
		{
			// Get the number of items on the undo stack
            int undoCount = 0;// editor.Document.UndoRedo.UndoStack.Count;

			if (undoCount == 0) 
			{
				// There are no items on the undo stack
				undoName1MenuItem.Visible = true;
				undoName1MenuItem.Enabled = false;
				undoName1MenuItem.Text = "(No Actions)";
				undoName2MenuItem.Visible = undoName3MenuItem.Visible = undoName4MenuItem.Visible = undoName5MenuItem.Visible = false;
			}
			else 
			{
				// Make the menu items visible if there are enough items on the stack
				undoName1MenuItem.Visible = (undoCount >= 1);
				undoName1MenuItem.Enabled = true;
				undoName2MenuItem.Visible = (undoCount >= 2);
				undoName3MenuItem.Visible = (undoCount >= 3);
				undoName4MenuItem.Visible = (undoCount >= 4);
				undoName5MenuItem.Visible = (undoCount >= 5);

				// Update the menu item text
            //    if (undoCount >= 1) undoName1MenuItem.Text = editor.Document.UndoRedo.UndoStack.GetName(undoCount - 1);
            //    if (undoCount >= 2) undoName2MenuItem.Text = editor.Document.UndoRedo.UndoStack.GetName(undoCount - 2);
            //    if (undoCount >= 3) undoName3MenuItem.Text = editor.Document.UndoRedo.UndoStack.GetName(undoCount - 3);
            //    if (undoCount >= 4) undoName4MenuItem.Text = editor.Document.UndoRedo.UndoStack.GetName(undoCount - 4);
            //    if (undoCount >= 5) undoName5MenuItem.Text = editor.Document.UndoRedo.UndoStack.GetName(undoCount - 5);
            }		
		}

		private void undoName1MenuItem_Click(object sender, System.EventArgs e)
		{
			// Undo 1 level off the undo stack
            //for (int i = 0; i < 1; i++)
            //    editor.Document.UndoRedo.Undo();			
		}

		private void undoName2MenuItem_Click(object sender, System.EventArgs e)
		{
			// Undo 2 levels off the undo stack
            //for (int i = 0; i < 2; i++)
            //    editor.Document.UndoRedo.Undo();			
		}

		private void undoName3MenuItem_Click(object sender, System.EventArgs e)
		{
			// Undo 3 levels off the undo stack
            //for (int i = 0; i < 3; i++)
            //    editor.Document.UndoRedo.Undo();			
		}

		private void undoName4MenuItem_Click(object sender, System.EventArgs e)
		{
			// Undo 4 levels off the undo stack
            //for (int i = 0; i < 4; i++)
            //    editor.Document.UndoRedo.Undo();			
		}

		private void undoName5MenuItem_Click(object sender, System.EventArgs e)
		{
			// Undo 5 levels off the undo stack
            //for (int i = 0; i < 5; i++)
            //    editor.Document.UndoRedo.Undo();		
		}

		private void redoNames_Popup(object sender, System.EventArgs e)
		{
			// Get the number of items on the redo stack
            int redoCount = 0;// editor.Document.UndoRedo.RedoStack.Count;

			if (redoCount == 0) 
			{
				// There are no items on the redo stack
				redoName1MenuItem.Visible = true;
				redoName1MenuItem.Enabled = false;
				redoName1MenuItem.Text = "(No Actions)";
				redoName2MenuItem.Visible = redoName3MenuItem.Visible = redoName4MenuItem.Visible = redoName5MenuItem.Visible = false;
			}
			else 
			{
				// Make the menu items visible if there are enough items on the stack
				redoName1MenuItem.Visible = (redoCount >= 1);
				redoName1MenuItem.Enabled = true;
				redoName2MenuItem.Visible = (redoCount >= 2);
				redoName3MenuItem.Visible = (redoCount >= 3);
				redoName4MenuItem.Visible = (redoCount >= 4);
				redoName5MenuItem.Visible = (redoCount >= 5);

				// Update the menu item text
                //if (redoCount >= 1) redoName1MenuItem.Text = editor.Document.UndoRedo.RedoStack.GetName(redoCount - 1);
                //if (redoCount >= 2) redoName2MenuItem.Text = editor.Document.UndoRedo.RedoStack.GetName(redoCount - 2);
                //if (redoCount >= 3) redoName3MenuItem.Text = editor.Document.UndoRedo.RedoStack.GetName(redoCount - 3);
                //if (redoCount >= 4) redoName4MenuItem.Text = editor.Document.UndoRedo.RedoStack.GetName(redoCount - 4);
                //if (redoCount >= 5) redoName5MenuItem.Text = editor.Document.UndoRedo.RedoStack.GetName(redoCount - 5);
			
            }		
		}

		private void redoName1MenuItem_Click(object sender, System.EventArgs e)
		{
			// Undo 1 level off the undo stack
            for (int i = 0; i < 1; i++)
                ;
				//editor.Document.UndoRedo.Redo();		
		}

		private void redoName2MenuItem_Click(object sender, System.EventArgs e)
		{
			// Undo 1 level off the undo stack
            for (int i = 0; i < 2; i++)
                ;
				//editor.Document.UndoRedo.Redo();		
		}

		private void redoName3MenuItem_Click(object sender, System.EventArgs e)
		{
			// Undo 1 level off the undo stack
            for (int i = 0; i < 3; i++)
                ;
				//editor.Document.UndoRedo.Redo();		
		}

		private void redoName4MenuItem_Click(object sender, System.EventArgs e)
		{
			// Undo 1 level off the undo stack
            for (int i = 0; i < 4; i++)
                ;
				//editor.Document.UndoRedo.Redo();		
		}

		private void redoName5MenuItem_Click(object sender, System.EventArgs e)
		{
			// Undo 1 level off the undo stack
            for (int i = 0; i < 5; i++)
                ;
				//editor.Document.UndoRedo.Redo();		
		}

		private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
            if (scintilla1.Modified)
			{
				MessageBoxButtons buttons = MessageBoxButtons.YesNo;
				DialogResult result;
				result = MessageBox.Show(this, "Save file?", "File modified", buttons,
					MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
				if(result == DialogResult.Yes)
				{
					ExecuteAppAction(AppAction.FileSave);
				}
			}
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////
		// DIRTYFLAG CODE
		/////////////////////////////////////////////////////////////////////////////////////////////////////

		private void SetDirtyFlag()
		{
			this.isModified = true;
			if (!this.Text.EndsWith("*") )
			{
				this.Text += "*";
			}
		}

		private void OnDocumentModified(object sender, System.EventArgs e)
		{
			SetDirtyFlag();
		}

		private void OnKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Back)
				SetDirtyFlag();
		}

		private void OnDeletePressed(object sender, System.EventArgs e)
		{
			SetDirtyFlag();
		}

		private void menuEditFindNext_Click(object sender, System.EventArgs e)
		{
            //scintilla1.Commands.Execute(ScintillaNet.BindableCommand.FindNext);
		}

        private void OnMouseHover(object sender, EventArgs e)
        {
            //int position = this.scintilla1.PositionFromPoint(MousePosition.X, MousePosition.Y);
            //char c = this.scintilla1.CharAt(position);
            //string msg = "Hey there!";
            //this.scintilla1.CallTip.Show(msg, position);
        }

		/////////////////////////////////////////////////////////////////////////////////////////////////////


	}
}
