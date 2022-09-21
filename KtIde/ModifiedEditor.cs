using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

//
//	This class is created solely to override the ProcessCmdKey function
//	in order to trap the ENTER key event.
//

namespace KtIde
{
	public class ModifiedEditor //: ActiproSoftware.SyntaxEditor.SyntaxEditor
	{
		private System.ComponentModel.IContainer components = null;
		public event EventHandler EnterPressed;
		public event EventHandler DeletePressed;

        

		public ModifiedEditor()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
        //protected override void Dispose( bool disposing )
        //{
        //    if( disposing )
        //    {
        //        if (components != null) 
        //        {
        //            components.Dispose();
        //        }
        //    }
        //    base.Dispose( disposing );
        //}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// ModifiedEditor
			// 
            //this.Name = "ModifiedEditor";
            //this.Size = new System.Drawing.Size(672, 360);
            //this.BracketHighlightingVisible = true;
			//ToDo: FIXX this: this.TextAreaVisualStyle.BackgroundFill = new ActiproSoftware.Drawing.SolidColorBackgroundFill(System.Drawing.SystemColors.Control);

		}
		#endregion

		// Here's the magic override
        //protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        //{
        //    if (keyData == Keys.Enter)
        //    {
        //        this.EnterPressed(this, EventArgs.Empty);	// Raise event
        //    }
        //    else if (keyData == Keys.Delete)
        //    {
        //        this.DeletePressed(this, EventArgs.Empty);	// Raise event
        //    }
        //    return base.ProcessCmdKey (ref msg, keyData);
        //}

	}
}

