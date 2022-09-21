using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace KtIde
{

    // See: https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.processstartinfo.redirectstandardoutput?view=netframework-4.8
    public class ShellRedirect : System.Windows.Forms.UserControl
    {
        public string Executable { get; set; }
        private Process ShellProcess;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.ComponentModel.IContainer  components;



        // Default ctor
        public ShellRedirect()
        {
            Executable = @"C:\Python\Python3\python.exe -i";
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            //this.listBox1 = (gcnew System::Windows::Forms::ListBox());
            //this.toolTip1 = (gcnew System::Windows::Forms::ToolTip(this.components));
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.AcceptsTab = true;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(508, 286);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            this.richTextBox1.WordWrap = false;
            //this.richTextBox1.KeyDown += gcnew System::Windows::Forms::KeyEventHandler(this, &KtShellControl::OnKeyDown);
            //this.richTextBox1.KeyPress += gcnew System::Windows::Forms::KeyPressEventHandler(this, &KtShellControl::OnKeyPress);
            //this.richTextBox1.MouseHover += gcnew System::EventHandler(this, &KtShellControl::OnMouseHover);

            // 
            // KtShellControl
            // 
            //this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
            //this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
            //this->Controls->Add(this->listBox1);
            this.Controls.Add(this.richTextBox1);
            this.Name = "KtShellControl";
            this.Size = new System.Drawing.Size(508, 286);
            this.ResumeLayout(false);
        }

        public bool Initialize()
        {
            ShellProcess = new Process();
            ShellProcess.StartInfo.UseShellExecute = false;
            ShellProcess.StartInfo.RedirectStandardOutput = true;
            string eOut = null;
            ShellProcess.StartInfo.RedirectStandardError = true;
            ShellProcess.ErrorDataReceived += new DataReceivedEventHandler((sender, e) =>
            { eOut += e.Data; });
            ShellProcess.StartInfo.FileName = Executable;
            ShellProcess.Start();

            return true;
        }
    }
}
