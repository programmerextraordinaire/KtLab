#pragma once

#include <windows.h>    // for HANDLE and the obvious Windows stuff
#include "History.h"
#include "Options.h"

using namespace System;
using namespace System::ComponentModel;
using namespace System::Collections;
using namespace System::Windows::Forms;
using namespace System::Data;
using namespace System::Drawing;
using namespace System::Runtime::InteropServices;
using namespace System::Threading;


#define BUFSIZE 16384	//4096*4

HANDLE hChildStdinRead, hChildStdinWriteDup, hChildStdoutWrite, hChildStdoutReadDup; 

namespace KtShell {

	/// <summary>
	/// Summary for KtShellControl
	/// </summary>
	///
	/// WARNING: If you change the name of this class, you will need to change the
	///          'Resource File Name' property for the managed resource compiler tool
	///          associated with all .resx files this class depends on.  Otherwise,
	///          the designers will not be able to interact properly with localized
	///          resources associated with this form.
	public ref class KtShellControl : public System::Windows::Forms::UserControl
	{
#pragma region ctor / dtor
	public:
		KtShellControl(void)
		{
			InitializeComponent();
			options = gcnew KtShell::Options();
			history = gcnew KtShell::History();

		}

	protected:
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		~KtShellControl()
		{
			Shutdown();
			if (components)
			{
				delete components;
			}
		}
#pragma endregion

#pragma region Member variables
	private: String ^ delimStr = "\n";		// Use when communicating with Python.
	private: String^ newline = System::Environment::NewLine;	// Use when working with strings not directly going to Python, e.g. Split.
	private: String^ newlines = "\r\n\r\n";	// Use when working with strings not directly going to Python, e.g. Split.
	private: delegate void SetRichEditTextCallback(String ^ text);
	private: delegate void SafeFocusCallback();
	private: delegate void SafeSetFontCallback(String ^ fontName, float fontSize);

	public: KtShell::Options ^ options;  // User specified options for the program
	public: KtShell::History ^ history;

	private: System::Windows::Forms::RichTextBox^  richTextBox1;
	private: String ^ secretStr;
	private: String ^ prompt;
	private: int lastKeyValue;
	private: bool indentedBlock;
	private: int indentBlockStart;
	private: int indentBlockEnd;
	private: bool inTripleQuote;
			 // Handles
	private: HANDLE hStdout;
			 DWORD PythonProcessId;
			 // Threads
	private: Thread ^ readThread;
			 // Events and control variables
			 static ManualResetEvent ^ eventSecretStringReady = gcnew ManualResetEvent(false);
			 static int secretPython = 0;  // 0 = False; 1 = True
			 // Autocomplete list box items
			 int listBoxSelectionStart;
			 String ^ listBoxSearchStr;
			 // Control objects
	private: System::Windows::Forms::ListBox^  listBox1;
	private: System::Windows::Forms::ToolTip^  toolTip1;
	private: System::ComponentModel::IContainer^  components;

	protected: 

	private:
		/// <summary>
		/// Required designer variable.
		/// </summary>
#pragma endregion

#pragma region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		void InitializeComponent(void)
		{
			this->components = (gcnew System::ComponentModel::Container());
			this->richTextBox1 = (gcnew System::Windows::Forms::RichTextBox());
			this->listBox1 = (gcnew System::Windows::Forms::ListBox());
			this->toolTip1 = (gcnew System::Windows::Forms::ToolTip(this->components));
			this->SuspendLayout();
			// 
			// richTextBox1
			// 
			this->richTextBox1->AcceptsTab = true;
			this->richTextBox1->Dock = System::Windows::Forms::DockStyle::Fill;
			this->richTextBox1->Location = System::Drawing::Point(0, 0);
			this->richTextBox1->Name = L"richTextBox1";
			this->richTextBox1->Size = System::Drawing::Size(508, 286);
			this->richTextBox1->TabIndex = 0;
			this->richTextBox1->Text = L"";
			this->richTextBox1->WordWrap = false;
			this->richTextBox1->KeyDown += gcnew System::Windows::Forms::KeyEventHandler(this, &KtShellControl::OnKeyDown);
			this->richTextBox1->KeyPress += gcnew System::Windows::Forms::KeyPressEventHandler(this, &KtShellControl::OnKeyPress);
			this->richTextBox1->MouseHover += gcnew System::EventHandler(this, &KtShellControl::OnMouseHover);
			// 
			// listBox1
			// 
			this->listBox1->FormattingEnabled = true;
			this->listBox1->Location = System::Drawing::Point(25, 13);
			this->listBox1->Name = L"listBox1";
			this->listBox1->Size = System::Drawing::Size(226, 121);
			this->listBox1->TabIndex = 1;
			this->listBox1->Visible = false;
			this->listBox1->DoubleClick += gcnew System::EventHandler(this, &KtShellControl::OnListBoxDoubleClick);
			this->listBox1->KeyPress += gcnew System::Windows::Forms::KeyPressEventHandler(this, &KtShellControl::OnListBoxKeyPress);
			// 
			// toolTip1
			// 
			this->toolTip1->AutomaticDelay = 0;
			this->toolTip1->UseAnimation = false;
			this->toolTip1->UseFading = false;
			// 
			// KtShellControl
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->Controls->Add(this->listBox1);
			this->Controls->Add(this->richTextBox1);
			this->Name = L"KtShellControl";
			this->Size = System::Drawing::Size(508, 286);
			this->ResumeLayout(false);

		}
#pragma endregion

#pragma region Initial Bootstrap
		//--------------------------------------------------------------------------------
		//--------------------------  I N I T I A L   B O O T S T R A P   ----------------
		//--------------------------------------------------------------------------------

		/// <summary>
		/// Initialize the KtShell Control;
		/// load the options and create the redirected shell.
		/// </summary>
	public: bool Initialize()
			{
				bool retval = false;
				try
				{
					LoadOptions();
					retval = CreateShellRedirect();
					this->history->LoadHistory();
					this->history->AddTimestamp();
					GetPrompts();

					// Set member vars
					this->indentedBlock = false;
					this->inTripleQuote = false;

					// Run initialization scripts
					//RunPythonInitScripts();
				}
				catch (System::Exception ^ e)
				{
					throw e;
				}

				return retval;
			}


			/// <summary>
			/// Creates the object to hold the programs options,
			/// then it Deserializes the XML option file.
			/// </summary>
	public: void LoadOptions()
			{
				if (!options)
					options = gcnew KtShell::Options();
				options->DeserializeXML();
				// Set the Font
				SetFont(options->FontName, options->FontSize);
			}

	public: void SetFont(String ^ fontName, float fontSize)
			{
				// set the Font from calling thread
				SafeSetFont(fontName, fontSize);
			}

	private: void GetPrompts()
			 {
				 // TODO: Ask Python what sys.ps1 and sys.ps2 are ... -- problems
				 //String * cmd = S"import sys;sys.ps1\r\n";
				 //bool readOK = SecretPythonRead(cmd, 777);
				 //readOK = SecretPythonRead(cmd, 777);
				 //if (readOK && secretStr->Length > 1)
				 //{
				 //    prompt = secretStr->Replace("'","");
				 //}
				 // Fake it for now
				 prompt = ">>> ";
			 }
			 //-------------------------------------------------------------------------------------
			 //--------------------------  E N D   O F   I N I T I A L   B O O T S T R A P   -------
			 //-------------------------------------------------------------------------------------
#pragma endregion

#pragma region Child Process Functions
			 //-------------------------------------------------------------------------------------
			 //--------------------------  C H I L D   P R O C E S S   F U N C T I O N S   ---------
			 //-------------------------------------------------------------------------------------
			 /// <summary>
			 /// Create the new read/write pipes then create
			 /// the child process and attach the pipes the this process. 
			 /// Start the read thread to read StdIn.
			 /// </summary>
	public: bool CreateShellRedirect(void) 
			{ 
				SECURITY_ATTRIBUTES saAttr; 
				BOOL fSuccess; 
				HANDLE hChildStdoutRd, hChildStdinWr; // temp handles

				// Set the bInheritHandle flag so pipe handles are inherited. 
				saAttr.nLength = sizeof(SECURITY_ATTRIBUTES); 
				saAttr.bInheritHandle = TRUE; 
				saAttr.lpSecurityDescriptor = NULL; 

				// Get the handle to the current STDOUT. 
				hStdout = GetStdHandle(STD_OUTPUT_HANDLE); 

				// Create a pipe for the child process's STDOUT. 
				if (! CreatePipe(&hChildStdoutRd, &hChildStdoutWrite, &saAttr, 0)) 
					throw gcnew System::Exception("KtShell::CreateShellRedirect - Stdout pipe creation failed.");
				//return false;	//ErrorExit("Stdout pipe creation failed\n"); 

				// Create noninheritable read handle and close the inheritable read handle. 
				fSuccess = DuplicateHandle(GetCurrentProcess(), hChildStdoutRd, GetCurrentProcess(), &hChildStdoutReadDup , 0,
					FALSE,                  // not inherited 
					DUPLICATE_SAME_ACCESS);
				if( !fSuccess )
					throw gcnew System::Exception("KtShell::CreateShellRedirect - DuplicateHandle 1 failed.");
				//return false;	//ErrorExit("DuplicateHandle failed");
				CloseHandle(hChildStdoutRd);

				// Create a pipe for the child process's STDIN. 
				if (! CreatePipe(&hChildStdinRead, &hChildStdinWr, &saAttr, 0)) 
					throw gcnew System::Exception("KtShell::CreateShellRedirect - Stdin pipe creation failed.");
				//return false;	//ErrorExit("Stdin pipe creation failed\n"); 

				// Duplicate the write handle to the pipe so it is not inherited. 
				fSuccess = DuplicateHandle(GetCurrentProcess(), hChildStdinWr, GetCurrentProcess(), &hChildStdinWriteDup, 0, 
					FALSE,                  // not inherited 
					DUPLICATE_SAME_ACCESS); 
				if (! fSuccess) 
					throw gcnew System::Exception("KtShell::CreateShellRedirect - DuplicateHandle 2 failed.");
				//return false;	//ErrorExit("DuplicateHandle failed"); 
				CloseHandle(hChildStdinWr); 

				// Now create the child process. 
				fSuccess = CreateChildProcess();
				if (! fSuccess) 
					throw gcnew System::Exception("KtShell::CreateShellRedirect - Create process failed.");
				//return false;	//ErrorExit("Create process failed"); 

				RunPythonInitScripts();

				// Kick off reader thread
				//KillReadThread();
				readThread = gcnew Thread(gcnew ThreadStart(this, &KtShell::KtShellControl::ReadPipeThreadProc));
				readThread->Priority = ThreadPriority::AboveNormal;
				readThread->IsBackground = true;        // Allow the app to close w/o hanging on thread.
				readThread->Start();

				//RunPythonInitScripts();

				return true; 
			} 

			/// <summary>
			/// Create the child process. 
			/// </summary>
	private: bool CreateChildProcess() 
			 { 
				 PROCESS_INFORMATION piProcInfo; 
				 STARTUPINFOA siStartInfo;
				 SECURITY_ATTRIBUTES saAttr;
				 bool bRetval = false; 

				 // security stuff
				 saAttr.nLength = sizeof(SECURITY_ATTRIBUTES);
				 saAttr.lpSecurityDescriptor = NULL;

				 // Set up members of the PROCESS_INFORMATION structure. 
				 ::ZeroMemory( &piProcInfo, sizeof(PROCESS_INFORMATION) );
				 //System::Buffer::SetByte(&piProcInfo, 0, sizeof(PROCESS_INFORMATION));
				 DWORD dwzero = 0;
				 piProcInfo.dwProcessId = dwzero;
				 piProcInfo.dwThreadId = dwzero;
				 piProcInfo.hProcess = 0x0;
				 piProcInfo.hThread = 0x0;

				 // Set up members of the STARTUPINFO structure. 
				 ::ZeroMemory( &siStartInfo, sizeof(STARTUPINFO) );
				 siStartInfo.dwFillAttribute = dwzero;
				 siStartInfo.dwX = dwzero;
				 siStartInfo.dwXCountChars = dwzero;
				 siStartInfo.dwXSize = dwzero;
				 siStartInfo.dwY = dwzero;
				 siStartInfo.dwYCountChars = dwzero;
				 siStartInfo.dwYSize = dwzero;
				 siStartInfo.lpDesktop = 0x0;
				 siStartInfo.wShowWindow = 0;
				 siStartInfo.cb = sizeof(STARTUPINFO);
				 siStartInfo.dwFlags |= STARTF_USESTDHANDLES;
				 //siStartInfo.dwFlags |= STARTF_USESHOWWINDOW;
				 //siStartInfo.wShowWindow = SW_HIDE;
				 siStartInfo.hStdError = hChildStdoutWrite;
				 siStartInfo.hStdOutput = hChildStdoutWrite;
				 siStartInfo.hStdInput = hChildStdinRead;
				 //siStartInfo.lpTitle = (LPWSTR)"42 in the universe";

				 // Convert System::String ^ to C style string pointers
				 char * lpCurrentDir = (char*) Marshal::StringToHGlobalAnsi(options->StartupDirectory).ToPointer();
				 char * lpCommandLine = (char*) Marshal::StringToHGlobalAnsi(options->PythonExecuteable).ToPointer();

				 // Create the child process. 
				 BOOL bFuncRetn = CreateProcessA(NULL, 
					 lpCommandLine,       // command line "C:\\Python\\Python24\\python.exe -i"
					 &saAttr, 	    // process security attributes 
					 NULL,          // primary thread security attributes 
					 TRUE,          // handles are inherited 
					 CREATE_NO_WINDOW, //CREATE_NEW_CONSOLE,             // creation flags 
					 NULL,          // use parent's environment 
					 lpCurrentDir,          // NULL = use parent's current directory ".\\  "
					 &siStartInfo,  // STARTUPINFO pointer 
					 &piProcInfo);  // receives PROCESS_INFORMATION 

				 if (FALSE == bFuncRetn) 
				 {
					 String ^ errstr = "KtShell::CreateProcess failed. Process: [{0}]  Directory: [{1}]";
					 errstr = String::Format(errstr, options->PythonExecuteable, options->StartupDirectory);
					 throw gcnew System::Exception(errstr);
				 }
				 // Clean up
				 Marshal::FreeHGlobal(IntPtr(lpCurrentDir));
				 Marshal::FreeHGlobal(IntPtr(lpCommandLine));
				 CloseHandle(piProcInfo.hProcess);
				 CloseHandle(piProcInfo.hThread);
				 PythonProcessId = piProcInfo.dwProcessId;
				 bRetval = true;
				 return bRetval;
			 }

			 /// <summary>
			 /// Read the StdIn pipe and send it to the RichTextBox. 
			 ///
			 /// Note use of "Secret" event flag that lets KtIDE communicate
			 /// silently with Python.  Basically goes like this:
			 ///	1. Set the secret flag after the ">>> " prompt or "... " prompt seen
			 ///	2. Write to pipe (but not to history)
			 ///	3. Read from pipe and/or Wait for response -- put output in secretStr
			 ///	4. Unset the secret flag after the return key is pressed.
			 ///
			 /// Note: String::Concat is used instead of StringBuilder->Append
			 /// Since there are usually few concats, this is probably fastest.  See
			 /// http://www.heikniemi.net/hc/archives/000124.html 
			 /// </summary>
	private: void ReadPipeThreadProc(void)
				 //public: void ReadPipeThreadProc(
				 //	System::Object ^ param, 
				 //	System::ComponentModel::RunWorkerCompletedEventArgs ^ args)
			 {
				 DWORD dwRead;
				 char chBuf[BUFSIZE+1];		// Allow for null terminator.
				 int end = 0;    // Buffer counter for end of data read.
				 secretStr = gcnew String("");

				 for (;;)
				 {
					 //if( !ReadConsoleW(hChildStdoutReadDup, chBuf, BUFSIZE, &dwRead, NULL) || 0 == dwRead) 
					 if( !ReadFile(hChildStdoutReadDup, chBuf, BUFSIZE, &dwRead, NULL) || 0 == dwRead) 
					 {
						 if (ERROR_BROKEN_PIPE == ::GetLastError() )
						 {
							 break;      //MessageBox.Show("Broken pipe!");
						 }
					 }
					 if (dwRead > 0) 
					 {
						 chBuf[(dwRead/sizeof(char))] = '\0';	// null terminate the output
						 String ^ bufstr = gcnew String(chBuf);

						 if (0 == secretPython)
						 {
							 // Append the text and scroll
							 SafeAppendText(bufstr);

							 // look for end of output (prompt) 
							 if (bufstr->EndsWith(">>> "))
							 {
								 // end of public mode
								 Threading::Interlocked::Exchange(secretPython, 1); // Secret mode (secretPython=1)
								 SafeFocus();		                              // Force a scroll by getting the focus
							 }
						 }
						 else
						 {
							 // *****   S E C R E T   M O D E   *****
							 // look for ps1 prompt to notify objects waiting for the buffer
							 if (bufstr->EndsWith(">>> "))
							 {
								 // signal that Python has returned
								 if (bufstr->EndsWith("\r\n>>> "))
								 {
									 end = bufstr->LastIndexOf("\r\n>>> ");
									 secretStr = String::Concat(secretStr, bufstr->Substring(0, end));
								 }
								 eventSecretStringReady->Set();      // Secret Python string is ready
							 }
							 // look for ps2 prompt to notify objects waiting for the buffer
							 else if (bufstr->EndsWith("... "))
							 {
								 secretStr = String::Empty;

							     //// signal that Python has returned
							     //if (bufstr->EndsWith("\r\n... "))
							     //{
							     //    end = bufstr->LastIndexOf("\r\n... ");
							     //    secretStr = String::Concat(secretStr, bufstr->Substring(0, end));
							     //}
							     //eventSecretStringReady->Set();      // Secret Python string is ready
							 }
							 else
							 {
								 // Write the output to the secret Python string.
								 if (secretStr)
								 {
									 secretStr = String::Concat(secretStr, bufstr);
								 }
								 else
									 secretStr = String::Empty;
							 }
						 }
					 }
					 else
					 {
						 Thread::Sleep(0);   // Allow other waiting threads to execute.
					 }
				 }

				 // close thread i/o handles
				 CloseHandle(hChildStdinRead);
				 CloseHandle(hChildStdoutWrite);
				 readThread = nullptr;
				 PythonProcessId = DWORD(-1);
			 }


	private: bool SecretPythonRead(String ^ cmd) //, const int waitms=10)
			 {
				int waitms = 50;
				 // Append crlf to command if needed
				if (!cmd->EndsWith(delimStr))
					cmd = String::Concat(cmd, delimStr);
					 //cmd = String::Concat(cmd, "\r\n");

				 // clear the secretString
				 secretStr = "";
				 Threading::Interlocked::Exchange(secretPython, 1);    // Enter Secret mode (secretPython=1)
				 eventSecretStringReady->Reset();
				 WriteToProcess(cmd);
				 // wait for Python to return or waitms milliseconds (1000=default)
				 bool OK = eventSecretStringReady->WaitOne(waitms, true);
				 if (!OK)
				 {
					 int foo = 42;
					 // ToDo: Maybe popup dialog box here... give option to cancel or try again (Python is busy??)
					 eventSecretStringReady->Reset();
					 OK = eventSecretStringReady->WaitOne(waitms*2, true);
					 if (!OK)
					 {
						 eventSecretStringReady->Set();
						 Threading::Interlocked::Exchange(secretPython, 0);    // Public mode (secretPython=0)
					 }
				 }
				 // now make sure an error wasn't returned
				 if (secretStr->StartsWith(options->PythonErrorMessage))
					 OK = false;
				 return OK;
			 }


			 /// <summary>
			 /// Exit from unrecoverable error.
			 /// </summary>
	private: void ErrorExit (String ^ errorMessage) 
			 { 
				 //Windows::Forms::MessageBox::Show(errorMessage);
				 ExitProcess(0); 
			 }

	private: void SafeSetFont(String ^ fontName, float fontSize)
			 {
				 if (this->richTextBox1->InvokeRequired)
				 {
					 SafeSetFontCallback ^ d = gcnew SafeSetFontCallback(this, &KtShellControl::SafeSetFont);
					 this->Invoke(d, fontName, fontSize);
				 }
				 else
				 {
					 // First try to create the Font, and set it on success;
					 System::Drawing::Font ^ newFont;
					 try
					 {
						 newFont = gcnew System::Drawing::Font(fontName, fontSize, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)0);
					 }
					 catch (System::Exception ^ e )
					 {
						 // pass - put up a message box?
						 throw e;
					 }
					 this->richTextBox1->Font = newFont;
				 }
			 }


	private: void SafeFocus()
			 {
				 // InvokeRequired required compares the thread ID of the
				 // calling thread to the thread ID of the creating thread.
				 // If these threads are different, it returns true.

				 if (this->richTextBox1->InvokeRequired)
				 {
					 SafeFocusCallback ^ d = gcnew SafeFocusCallback(this, &KtShellControl::SafeFocus);
					 this->Invoke(d);
				 }
				 else
				 {
					 this->richTextBox1->Focus();
					 this->richTextBox1->ScrollToCaret();
				 }
			 }

	private: void SafeAppendText(String ^ text)
			 {
				 // InvokeRequired required compares the thread ID of the
				 // calling thread to the thread ID of the creating thread.
				 // If these threads are different, it returns true.

				 if (this->richTextBox1->InvokeRequired)
				 {
					 SetRichEditTextCallback ^ d = gcnew SetRichEditTextCallback(this, &KtShellControl::SafeAppendText);
					 try
					 {
						 this->Invoke(d, text );
					 }
					 catch (System::Exception ^ e)
					 {
						 System::Windows::Forms::MessageBox::Show(e->Message);
					 }
				 }
				 else
				 {
					 text = text->Replace("\r\r","\r");		// IPY BUG FIX!!
					 this->richTextBox1->AppendText(text);
					 this->richTextBox1->ScrollToCaret();
				 }
			 }


			 /// <summary>
			 /// Kill the thread that reads StdIn from the child process.
			 /// </summary>
	private: void KillReadThread(void)
			 {
				 // Kill stdout read thread for spawned Python
				 if (readThread)
				 {
					 readThread->Interrupt();
					 readThread = nullptr;
				 }
			 }

			 /// <summary>
			 /// Kill the Child process (Python shell).
			 /// </summary>
	private: void KillChildProcess()
			 {
				 if (PythonProcessId != DWORD(-1))
				 {
					 HANDLE hProcess = OpenProcess(PROCESS_ALL_ACCESS, FALSE, PythonProcessId);
					 if (hProcess)
					 {
						 TerminateProcess(hProcess, 0);
						 WaitForSingleObject(hProcess, INFINITE);
						 CloseHandle(hProcess);
					 }
				 }
			 }

			 /// <summary>
			 /// Stops and then restarts the child process.
			 /// </summary>
	public:
		bool RestartProcess(void)
		{
			// Kill Python child process and Stdin read thread
			//richTextBox1->Clear();
			richTextBox1->AppendText(prompt);
			richTextBox1->AppendText("============================= RESTART =============================\n");
			KillChildProcess();
			KillReadThread();
			Thread::Sleep(200);
			bool retval = CreateChildProcess();
			Thread::Sleep(200);
			richTextBox1->AppendText(prompt);
			LoadOptions();

			return retval;
		}

	public: void Shutdown(void)
			{
				// Kill Python child process and Stdin read thread
				KillChildProcess();
				KillReadThread();
				if (options)
					options->SerializeXML();    // Save our variables
			}
			//-------------------------------------------------------------------------------------
			//-----------  E N D   O F   C H I L D   P R O C E S S   F U N C T I O N S   ----------
			//-------------------------------------------------------------------------------------
#pragma endregion

#pragma region Python String Handlers
			//---------------------------------------------------------------------------------------
			//--------------------------  P Y T H O N   S T R I N G   H A N D L E R S   -------------
			//---------------------------------------------------------------------------------------
			/// <summary>
			/// Removes the Python prompt from the input string.
			/// First looks for prompts: ">>> ", "... ", "help> ", and "(Pdb) "
			/// and if not found, it returns everything after the first space character.
			/// Returns the entire line if none of the above prompts are found.
			/// </summary>
			String ^ RemovePrompt(String ^line)
			{
				int promptlen = 0;

				// Strip Python prompt
				if (line->StartsWith(">>> ") || line->StartsWith("... "))
					promptlen = 4;
				else if (line->StartsWith("help> ") || line->StartsWith("(Pdb) "))
					promptlen = 6;
				else
					promptlen = line->IndexOf(' ') + 1;
				return line->Substring(promptlen);
			}

			void WriteToProcess(String ^ line) 
			{ 
				DWORD dwWritten;

				// Marshal the managed string to unmanaged memory.
				char* stringPointer = (char*) Marshal::StringToHGlobalAnsi(line).ToPointer();
				WriteFile(hChildStdinWriteDup, stringPointer, line->Length, &dwWritten, NULL);
				// Free the unmanaged string.
				Marshal::FreeHGlobal(IntPtr(stringPointer));
			}


			// http://www.thescripts.com/forum/thread611956.html
			// http://support.microsoft.com/kb/311259
			/// <summary>
			/// Convert a System::String to (char *)
			/// </summary>
	private: void StringToChar(String ^ str, char * charStr )
			 {
				 // Marshal the managed string to unmanaged memory.
				 charStr = (char*) Marshal::StringToHGlobalAnsi(str).ToPointer();

				 // IMPORTANT!!  Free the memory after using the unmanaged string
				 // Put the line below in the calling code after use.
				 //Marshal::FreeHGlobal(IntPtr(charStr));
			 }

			 //-----------------------------------------------------------------------------------------------
			 //--------------------  E N D   O F   P Y T H O N   S T R I N G   H A N D L E R S   -------------
			 //-----------------------------------------------------------------------------------------------
#pragma endregion

#pragma region Python Interface
			 //--------------------------------------------------------------------------------
			 //--------------------------  P Y T H O N   I N T E R F A C E  -------------------
			 //--------------------------------------------------------------------------------
	public:
		System::Void RunPythonInitScripts()
		{
			// Run initialization code for Python
			// Wait until process loads.
			System::Threading::Thread::Sleep(1000);
			ProcessLinesFromStrings("print('Running initialization scripts.')");
			//ProcessLinesFromStrings("def execfile(filename):    exec(open(filename).read())");
			ProcessLinesFromStrings("exec(open('init.py', 'rb').read())");
			ProcessLinesFromStrings("print('Finished initialization scripts.')");

		/*	array<String^>^ parts = options->InitScript->Split(newlines->ToCharArray());
			for each (String ^ script in parts)
			{
				ProcessLinesFromStrings(script);
			}*/
			//ProcessLinesFromStrings(options->InitScript);
		}

	private:
		void ProcessLinesFromStrings(String ^ script)
		{
			// Go invisible.
			Threading::Interlocked::Exchange(secretPython, 1);    // Enter Secret mode (secretPython=1)
			eventSecretStringReady->Reset();

			// Loop through the strings breaking them into lines
			array<String ^> ^ lines;
			//String^ delimStr = "\r\n";

			lines = script->Split(newline->ToCharArray(), System::StringSplitOptions::RemoveEmptyEntries);
			for each(String ^ line in lines)
			{
				WriteToProcess(String::Concat(line, delimStr));
			}
			// For multiple lines we have to end the ellipsis with a double NewLine.
			if (lines->Length > 1)
				WriteToProcess(String::Concat(delimStr, delimStr));

			// Go visible again.
			eventSecretStringReady->Set();
			Threading::Interlocked::Exchange(secretPython, 0);    // Public mode (secretPython=0)
		}

		/// <summary>
		/// Given a filename, run the script
		/// </summary>
	public:
		void RunFile(String ^ filename)
		{
			if (filename)
			{
				bool python2 = false;
				Threading::Interlocked::Exchange(secretPython, 0);    // Public mode (secretPython=0)
				// Python3 now default
				String ^ cmd = String::Format("exec(open(r'{0}').read())", filename);
				if (python2)
				{
					cmd = String::Format("execfile(r'{0}')", filename);
				}

				// Send to history, add line feed, and send to Python
				history->SaveHistoryLine(cmd);
				cmd = String::Concat(cmd, delimStr);
				WriteToProcess(cmd);
				this->SafeAppendText(cmd);
			}
		}

		/// <summary>
		/// Run the command string
		/// </summary>
	public:
		void RunString(String ^ cmd)
		{
			if (!String::Equals(cmd, ""))
			{
				Threading::Interlocked::Exchange(secretPython, 0);    // Public mode (secretPython=0)
				// Send to history, add line feed, and send to Python
				history->SaveHistoryLine(cmd);
				cmd = String::Concat(cmd, delimStr);
				WriteToProcess(cmd);
				this->SafeAppendText(cmd);
			}
		}
#pragma endregion

#pragma region RichTextBox Helper Functions
		//---------------------------------------------------------------------------------------
		//--------------   R I C H T E X T B O X   H E L P E R   F U N C T I O N S   ------------
		//---------------------------------------------------------------------------------------

	public: void Clear()
			{
				this->richTextBox1->Clear();
				richTextBox1->AppendText(prompt);
			}

	private: String ^ StripPromptFromLine(String ^ line)
			 {
				 if (line->StartsWith(this->prompt))
				 {
					 return line->Substring(this->prompt->Length);
				 }
				 return line;
			 }

			 /// <summary>
			 /// Get the last object from the current line.
			 /// TODO: FIXX!  Use regex to get this?
			 /// </summary>
	private: String ^ GetLastObject(String ^ regularExpression)
			 {
				 String ^ object = "";
				 String ^ line = GetLastLine(-1);
				 String ^ whitespace = " ,\t";
				 line = StripPromptFromLine(line)->TrimEnd(whitespace->ToCharArray());
				 if (line->Length > 0)
				 {
					 // First, count the quotes to see if this is the middle of a string
					 int quotes = CountStringChars(line, '"') + CountStringChars(line, '\'');
					 if (quotes % 2)
						 return "";

					 Char lastChar = line[line->Length - 1];
					 if (Char::IsLetterOrDigit(lastChar))
						 lastChar = 'z';
					 int idx = 0;
					 switch (lastChar)
					 {
					 case ']':
						 //idx = line->LastIndexOf('[');
						 object = "[ ]";					// Temp hack for Python - object = list
						 break;
					 case '}':
						 object = "{ }";					// Temp hack for Python - object = dictionary
						 break;
					 case '"':
						 object = "' '";					// Temp hack for Python - object = string
						 break;
					 case '\'':
						 object = "' '";					// Temp hack for Python - object = string
						 break;
					 default:
						 // Get to the beginning of the previous alphanumeric?
						 break;
					 }

					 // No standard objects found - use the last string
					 if (object->Length == 0)
					 {
						 // Split the line into tokens and get the last one
						 array<String ^> ^ split;
						 split = line->Split(regularExpression->ToCharArray(), System::StringSplitOptions::RemoveEmptyEntries);
						 if (split->Length > 0)
						 {
							 object = split[split->Length - 1];
							 if (object->Equals(")") && split->Length > 1)
							 {
								 object = split[split->Length - 2];
							 }
							 /*String ^ dot = ".";
							 if (object->Contains(dot))
							 {
								 split = line->Split(dot->ToCharArray());
								 object = split[0];
							 }*/
						 }
					 }
				 }
				 return object;
			 }


			 /// <summary>
			 /// Get the last word from the current line.
			 /// </summary>
	private: String ^ GetLastWord(String ^ delimiter)
			 {
				 String ^ line = GetLastLine(-1);
				 line = StripPromptFromLine(line);
				 // First, count the quotes to see if this is a string
				 // Hmmm... TODO: Think about this logic; would we ever want to ignore this?
				 int quotes = CountStringChars(line, '"') + CountStringChars(line, '\'');
				 if (quotes % 2)
					 return "";

				 // Split the line into tokens and get the last one
				 array<String ^> ^ split;
				 split = line->Split(delimiter->ToCharArray(), System::StringSplitOptions::RemoveEmptyEntries);
				 if (split->Length > 0)
					 return split[split->Length - 1];
				 return "";
			 }


			 /// <summary>
			 /// Get the last line (set offset = -2 to account for CR just pressed).
			 /// To remove the prompt, call as:
			 ///     myString = RemovePrompt(GetLastLine(-1));
			 /// </summary>
	private: String ^ GetLastLine(int offset)
			 {
				 String ^ line = "";
				 // Get linenum and error check
				 if (richTextBox1)
				 {
					 int linenum = richTextBox1->Lines->Length + offset;
					 if (linenum < 0)
						 linenum = 0;
					 line = richTextBox1->Lines[linenum];
				 }

				 return line;
			 }

			 /// <summary>
			 /// Count the occurances of the input character in a given string.
			 /// </summary>
	private: int CountStringChars(String ^ str, Char c)
			 {
				 int retval = 0;
				 for each ( Char ch in str)
				 {
					 if (ch == c)
						 retval++;
				 }
				 //CharEnumerator ^ myEnum = str->GetEnumerator();
				 //while (myEnum->MoveNext()) 
				 //{
				 //    if (myEnum->Current == c)
				 //        retval++;
				 //}
				 return retval;
			 }

			 /// <summary>
			 /// Count the occurances of the input character in a given string.
			 /// </summary>
	private: int CountStringString(String ^ source, String ^ search)
			 {
				 int retval = 0;
				 int strlen = search->Length;
				 int idx = source->IndexOf(search);
				 while (idx != -1)
				 {
					 retval++;
					 idx += strlen;				// bump index so we don't keep counting the same match
					 if (idx >= source->Length)
						 break;
					 idx = source->IndexOf(search, idx);
				 }
				 return retval;
			 }


			 /// <summary>
			 /// Gets the character index of the beginning of the line.
			 /// </summary>
	private: int GetIndexBOL(void)
			 {
				 int linenumber = richTextBox1->GetLineFromCharIndex(richTextBox1->SelectionStart);
				 int charPos = richTextBox1->GetFirstCharIndexFromLine(linenumber);
				 return charPos;
			 }

			 /// <summary>
			 /// Gets the character index of the end of the line.
			 /// </summary>
	private: int GetIndexEOL(void)
			 {
				 int linenumber = richTextBox1->GetLineFromCharIndex(richTextBox1->SelectionStart);
				 int charPos = richTextBox1->GetFirstCharIndexFromLine(linenumber);
				 int numLines = richTextBox1->Lines->Length;
				 if (numLines <= linenumber)
				 {
					 linenumber = numLines < 1 ? 0 : numLines - 1;
				 }
				 charPos += richTextBox1->Lines[linenumber]->Length;
				 return charPos;
			 }


			 /// <summary>
			 /// Replace the last line with newStr.
			 /// </summary>
	private: void ReplaceLastLine(String ^ newStr)
			 {
				 //  Delete the current text and append the new text
				 int selEnd = GetIndexEOL();      // Current cursor to EOL
				 int selStart = GetIndexBOL();
				 selStart += prompt->Length;  // ToDo: Fix prompt grabber
				 richTextBox1->Select(selStart, selEnd);
				 richTextBox1->Cut();
				 richTextBox1->AppendText(newStr);
				 //FIXX! use invoke
			 }

			 /// <summary>
			 /// Look for unmatched triple-quotes on the line.
			 /// </summary>
	private: bool TripleQuoteFound(String ^ line)
			 {
				 int num = 0;
				 if (line->Contains("'''"))
				 {
					 num = CountStringString(line, "'''");
				 }
				 else if (line->Contains("\"\"\""))
				 {
					 num = CountStringString(line, "\"\"\"");
				 }
				 return num % 2;
			 }

			 //---------------------------------------------------------------------------------------
			 //----------------   E N D   O F   R I C H T E X T B O X   H E L P E R S   --------------
			 //---------------------------------------------------------------------------------------
#pragma endregion

#pragma region ListBox Helper Functions			 
			 //---------------------------------------------------------------------------------------
			 //-----------------   L I S T B O X   H E L P E R   F U N C T I O N S   -----------------
			 //----------------------------------------------------------------------------------------
			 /// <summary>
			 /// Populates the list box with the values in the 
			 /// comma separated string.
			 /// </summary>
			 void PopulateListBox(String ^ cmdlist)
			 {
				 try
				 {
					 array<String ^> ^ split;
					 String ^ comma = ",";
					 String ^ trimStr = "[] \t\r\n";

					 listBox1->BeginUpdate();        // stop drawing
					 listBox1->Items->Clear();       // clear the list
					 split = cmdlist->Split(comma->ToCharArray(), System::StringSplitOptions::RemoveEmptyEntries);
					 IEnumerator ^ myEnum = split->GetEnumerator();
					 while (myEnum->MoveNext()) 
					 {
						 String ^ s = (String ^)(myEnum->Current);
						 s = s->Trim(trimStr->ToCharArray());
						 // TODO:! put this code in Python! We don't strip out these unless requested.
						 // [m for m in dir(foo) if m[0] != '_'] or dir(_object_)
						 if ((!s->StartsWith("_")) && (s->Length > 1))
							 listBox1->Items->Add(s);
						 // ToDo: Add images to the list
					 }
					 listBox1->EndUpdate();          // restart the drawing
				 }
				 catch( System::Exception ^ e )
				 {
					 // catch error here
					 throw e;
				 }
			 }


			 /// <summary>
			 /// Gets the selected item in the member listbox, and autofills the
			 /// richtextbox by taking everything before and after the "." in 
			 /// the richtextbox, and appending the listbox word to the text.
			 /// </summary>
			 void ListBoxSelectItem()
			 {
				 String ^ listboxText;
				 String ^ strTyped = listBoxSearchStr;   // name change for clarity only

				 int len = 0;
				 if (strTyped)
					 len = strTyped->Length;
				 richTextBox1->Select(listBoxSelectionStart, len);

				 try 
				 {
					 listboxText = listBox1->SelectedItem->ToString();
				 }
				 catch( System::Exception ^ e )
				 {
					 listboxText = strTyped;
				 }

				 // Now replace the text from the selection
				 // FIXX! use invoke
				 richTextBox1->Focus();
				 richTextBox1->Cut();
				 if (listboxText)
					 richTextBox1->AppendText(listboxText);
				 listBoxSearchStr = "";
			 }


			 void OnListBoxKeyPress(System::Object ^  sender, System::Windows::Forms::KeyPressEventArgs ^  e)
			 {
				 //  Process keys for selection from the list box.
				 if (listBox1->Visible)
				 {
					 // ENTER, SPACE, and TAB select item
					 if ((e->KeyChar == (Char)Keys::Enter) || 
						 (e->KeyChar == (Char)Keys::Space) || 
						 (e->KeyChar == (Char)Keys::Tab))
					 {
						 ListBoxSelectItem();
						 listBox1->Hide();
					 }
					 // OpenBracket key selects item and Hides box
					 else if (('(' == e->KeyChar) || 
						 (e->KeyChar == (Char)Keys::OemPeriod))
					 {
						 ListBoxSelectItem();
						 listBox1->Hide();
						 richTextBox1->AppendText(Char::ToString(e->KeyChar));	// FIXX Invoke
					 }
					 // ESC key Hides box without selecting Item
					 else if ((Char)Keys::Escape == e->KeyChar)
					 {
						 listBox1->Hide();
					 }
				 }
			 }


			 void OnListBoxDoubleClick(System::Object ^  sender, System::EventArgs ^  e)
			 {
				 //  Put the selection in the text box from the list box.
				 if (listBox1->Visible)
				 {
					 ListBoxSelectItem();
					 listBox1->Hide();
				 }
			 }
			 //---------------------------------------------------------------------------------------
			 //--------------------   E N D   O F   L I S T B O X   H E L P E R S   ------------------
			 //---------------------------------------------------------------------------------------
#pragma endregion


#pragma region Intellisense and Tooltips
			 //--------------------------------------------------------------------------------------
			 //-----------------  I N T E L L I S E N S E   A N D   T O O L T I P S   ---------------
			 //--------------------------------------------------------------------------------------
			 /// <summary>
			 /// Return an autocompletion comma separated string that displays functions
			 ///    and member variables for the given object, i.e. "Intellisense".
			 /// </summary>
	public:
		String ^ GetMembers(String ^ token)
		{
			//
			String ^ cmd = this->options->GetMethods->Replace("_object_", "{0}"); // "dir({0})";
			cmd = String::Format(cmd, token);
			String ^ memberlist;
			bool readOK = SecretPythonRead(cmd);
			if (readOK && secretStr->Length > 1)
			{
				memberlist = secretStr->Replace("'","");
			}
			return memberlist;
		}


		/// <summary>
		/// Return a tooltip string that displays syntax
		///    for the given object, i.e. "Intellisense Tooltip".
		/// </summary>
	public:
		String ^ GetTooltip(String ^ token)
		{
			// Call Python secretly...
			String ^ tooltip;
			String ^ cmd = this->options->GetMethodArgs->Replace("_object_", "{0}"); // "get_arg_text({0})";
			cmd = String::Format(cmd, token);
			bool readOK = SecretPythonRead(cmd);
			if (readOK && secretStr->Length > 1)
			{
				// First set the text
				tooltip = secretStr->Replace("'","");   // Get rid of the quotes
				int maxtextlen = 77;            // keep the text length reasonable
				if (tooltip->Length > maxtextlen) 
				{
					tooltip = tooltip->Substring(0, maxtextlen); 
					tooltip = String::Concat(tooltip, "...");
				}
			}
			return tooltip;
		}


		/// <summary>
		/// Popup an autocompletion listbox that displays functions
		///    and member variables for the given object, i.e. "Intellisense".
		/// </summary>
	private:
		void ShowAutocompleteBox(void)
		{
			String ^ lastword;
			if (!listBox1->Visible)
			{
				listBoxSelectionStart = this->richTextBox1->SelectionStart + 1;
				lastword = GetLastObject(" (+-*/=");     // Get the object preceding the cursor
				//lastword = GetLastWord("[], ()=");     // Get string preceding the cursor
				if (!lastword->Equals(""))
				{
					String ^ cmdlist = GetMembers(lastword);

					if (cmdlist)
					{
						PopulateListBox(cmdlist);
						if (listBox1->Items->Count > 0)
						{
							Drawing::Point point = richTextBox1->GetPositionFromCharIndex(richTextBox1->SelectionStart);
							int fontheight = richTextBox1->Font->Height;
							point.Y += fontheight + 2;
							point.X -= 10;
							// If we're near the bottom of the screen, push the box up
							// ToDo: check for window size and shrink box if needed
							if ((point.Y + listBox1->Height + fontheight*2) > richTextBox1->ClientSize.Height)
								point.Y -= (listBox1->Height + fontheight + 2);
							listBox1->Location = point;
							listBox1->BringToFront();
							listBox1->Visible = true;
							listBox1->Show();
							this->richTextBox1->Focus();    // return focus to the editor  FIXX! Invoke
						}
					}
				}
			}
		}


		/// <summary>
		// Popup a tool tip window that shows function arguments (tool tip)
		/// </summary>
	private:
		void ShowTooltipHelp(void)
		{
			// Try and get a tooltip
			//String ^ lastword = GetLastWord(" =");
			String ^ lastword = GetLastObject(" ,=");
			if (!lastword->Equals(""))
			{
				String ^ tooltiptext = GetTooltip(lastword);
				//String ^ tooltiptext = String::Format("Help on {0}", lastword);
				if (tooltiptext)
				{
					if (tooltiptext->Length > 0)
					{
						toolTip1->SetToolTip(this, tooltiptext);

						// Now set the position
						Point point = richTextBox1->GetPositionFromCharIndex(richTextBox1->SelectionStart);
						point.X -= 10;
						int fontheight = richTextBox1->Font->Height + 2;
						point.Y += fontheight;
						// If we're near the bottom of the screen, push the box up
						if ((point.Y + fontheight*2) > richTextBox1->ClientSize.Height)
							point.Y -= fontheight*2;
						toolTip1->Show(tooltiptext, this, point);
					}
				}
			}
		}
		//--------------------------------------------------------------------------------------
		//----------  E N D   O F   I N T E L L I S E N S E   A N D   T O O L T I P S   --------
		//--------------------------------------------------------------------------------------  
#pragma endregion


#pragma region Event Processing
		//-------------------------------------------------------------------------------------
		//------------------------  E V E N T   P R O C E S S I N G   -------------------------
		//-------------------------------------------------------------------------------------
	private: 
		void OnKeyDown(System::Object ^  sender, System::Windows::Forms::KeyEventArgs ^  e)
		{
			// Create/Set vars
			String ^ historyString = "";

			// process key down here...
			switch (e->KeyValue)
			{
			case (int)Keys::Enter:   // ENTER
				if (listBox1->Visible)
				{
					ListBoxSelectItem();   // Get item but DO NOT HIDE (do it in OnKeyPress)
					e->Handled = true;
				}
				else
				{
					// Go to the end of the last line
					richTextBox1->SelectionStart = richTextBox1->TextLength;
				}
				break;
			case (int)Keys::Back:     // BACKSPACE
			case (int)Keys::Left:    // LEFT ARROW
				// Don't allow BS before the ps1 prompt
				if (!listBox1->Visible)
				{
					// Move the cursor to just past the ps1 prompt.
					if (richTextBox1->SelectionStart == (GetIndexBOL() + prompt->Length))
					{
						//::MessageBeep(-1);
						System::Media::SystemSounds::Exclamation->Play();

						System::Media::SystemSounds::Beep->Play();	// TODO: Why doesn't this work?
						e->Handled = true;
					}
				}
				break;
			case (int)Keys::Tab:    // TAB
			case (int)Keys::Space:   // SPACE
				if (listBox1->Visible)
				{
					ListBoxSelectItem();   // Get item but DO NOT HIDE (do it in OnKeyPress)
					e->Handled = true;
				}
				break;
			case (int)Keys::Escape:    // ESC
				if (listBox1->Visible)
				{
					listBox1->Hide();       // Hide the list box
					e->Handled = true;
				}
				else
				{
					if (toolTip1->Active)
					{
						toolTip1->Hide(this); // Hide the tooltip
					}
					ReplaceLastLine("");   // Clear the line
				}
				break;
			case (int)Keys::Home:    // HOME
				if (!listBox1->Visible)
				{
					// Move the cursor to just past the ps1 prompt.
					richTextBox1->SelectionStart = GetIndexBOL() + prompt->Length;
					e->Handled = true;
				}
				break;
			case (int)Keys::Up:   // UP
				if (listBox1->Visible)
				{
					// Pass up/down arrow on to the list box if it's visible.
					listBox1->Focus();
				}
				else    // Otherwise process the history
				{
					if (lastKeyValue != (int)Keys::Up && lastKeyValue != (int)Keys::Down)     // If not history scrolling
						history->searchStr = RemovePrompt(GetLastLine(-1));         // Look for a string 
					if (!history->searchStr->Equals( ""))
						historyString = history->FindPrevious(history->searchStr);  // Try a smart search
					else
						historyString = history->Previous;							// Default to previous item
					if (!historyString->Equals( ""))
						ReplaceLastLine(historyString);
					lastKeyValue = e->KeyValue;
					e->Handled = true;
				}
				break;
			case (int)Keys::Down:   // DOWN 
				if (listBox1->Visible)
				{
					// Pass up/down arrow on to the list box if it's visible.
					listBox1->Focus();
				}
				else    // Otherwise process the history
				{
					if (lastKeyValue != (int)Keys::Up && lastKeyValue != (int)Keys::Down)     // If not history scrolling
						history->searchStr = RemovePrompt(GetLastLine(-1));         // Look for a string 
					if (!history->searchStr->Equals( ""))
						historyString = history->FindNext(history->searchStr);      // Try a smart search
					else
						historyString = history->Next;								// Default to next item
					if (!historyString->Equals( ""))
						ReplaceLastLine(historyString);
					lastKeyValue = e->KeyValue;
					e->Handled = true;
				}
				break;
			default:
				break;
			}
			lastKeyValue = e->KeyValue;
		}

	private: 
		void OnKeyPress(System::Object ^  sender, System::Windows::Forms::KeyPressEventArgs ^  e)
		{
			switch (e->KeyChar)
			{
			case (Char)Keys::Enter:    // ENTER KEY
				if (listBox1->Visible)
				{
					listBox1->Hide();
					e->Handled = true;
				}
				else
				{
					if (toolTip1->Active)
					{
						toolTip1->Hide(this);
					}
					// ToDo: Put into ProcessLine() function
					Threading::Interlocked::Exchange(secretPython, 0);    // Public mode (secretPython=0)
					String ^ line = RemovePrompt(GetLastLine(-2));
					line = line->TrimEnd(0);                        // remove trailing white space

					// See if we have started a triple quote
					if (TripleQuoteFound(line))
					{
						if (this->inTripleQuote)
							this->inTripleQuote = false;
						else
							this->inTripleQuote = true;
					}

					// Send to history, add line feed, and send to Python
					history->SaveHistoryLine(line);
					if (line->EndsWith(":") && !indentedBlock)
					{
						// This is the start of our indent history
						indentBlockStart = history->Count - 1;
						indentedBlock = true;
					}
					if (indentedBlock)
					{
						// Are we done with the indent?
						if (line->Trim(0)->Equals(""))
						{
							// We leave the block and process all of the indented lines
							indentedBlock = false;
							Threading::Interlocked::Exchange(secretPython, 1);    // Secret mode (secretPython=1)
							eventSecretStringReady->Reset();
							indentBlockEnd = history->Count;
							for (int i=indentBlockStart; i < indentBlockEnd; ++i)
							{
								// send it to Python
								line = String::Concat((String ^)history[i], "\r\n");
								WriteToProcess(line);
							}
							// Enter public mode
							bool OK = eventSecretStringReady->WaitOne(20, true);
							Threading::Interlocked::Exchange(secretPython, 0);    // Public mode (secretPython=0)
							WriteToProcess("\r\n");        // Done
						}
						else
						{
							// Fake the ps2 prompt
							if (richTextBox1)
							{
								// Append the text and scroll
								this->SafeAppendText("... ");       //  sys.ps2
							}
						}
					}
					else    // default - process one line
					{
						line = String::Concat(line, "\r\n");
						WriteToProcess(line);
					}
				}
				break;
			case '.':
				if (!indentedBlock && !inTripleQuote)
				{
					if (listBox1->Visible)
					{
						// Get current selection
						ListBoxSelectItem();
						listBox1->Hide();
					}
					ShowAutocompleteBox();
				}
				break;
			case (Char)Keys::Tab:     // TAB
			case (Char)Keys::Space:   // SPACE
				if (listBox1->Visible)
				{
					listBox1->Hide();
					e->Handled = true;
				}
				break;
			case '(':
				if (listBox1->Visible)
				{
					ListBoxSelectItem();   // Get item   
					listBox1->Hide();
				}
				ShowTooltipHelp();          // Now put up a tooltip if possible
				break;
			case ')':
				if (toolTip1->Active)
				{
					toolTip1->Hide(this);
				}
				break;
			case '[':
				if (listBox1->Visible)
				{
					ListBoxSelectItem();   // Get item   
					listBox1->Hide();
				}
				break;
			default:
				// see if we can modify the autocomplete list box
				if (listBox1->Visible && e->KeyChar != 46 && !indentedBlock)  // 46 = '.'
				{
					listBox1->Focus();
					String ^ tmpstr = this->GetLastWord(".");
					if (System::Char::IsLetter(e->KeyChar))
						tmpstr = String::Concat(tmpstr, System::Char::ToString(e->KeyChar));
					listBoxSearchStr = tmpstr;
					int idx;
					try
					{
						idx = listBox1->FindString(tmpstr);
					}
					catch( System::Exception ^ e )
					{
						idx = 0;
						// TODO: Log error;
					}
					listBox1->SelectedIndex = idx;
					this->SafeFocus();
				}
				break;
			}
		}

		//--------------------------------------------------------------------------------------
		//-------------------  E N D   O F   E V E N T   P R O C E S S I N G   -----------------
		//--------------------------------------------------------------------------------------
#pragma endregion

	private: System::Void OnMouseHover(System::Object^  sender, System::EventArgs^  e) 
			 {
				 // Get word and pop up value for debugging
				 RichTextBox ^ rtb = (RichTextBox ^)sender;
				 // TODO: Get word/object under cursor and get the VALUE - not the tool tip
				 // http://social.msdn.microsoft.com/Forums/en/vbgeneral/thread/26f94342-7ac0-4c69-9c86-cd65d382f10d
				 String ^ lastword = ""; //GetLastObject(" ,=");	
				 if (!lastword->Equals(""))
				 {
					 String ^ tooltiptext = GetTooltip(lastword);
					 //String ^ tooltiptext = String::Format("Help on {0}", lastword);
					 if (tooltiptext)
					 {
						 if (tooltiptext->Length > 0)
						 {
							 toolTip1->SetToolTip(this, tooltiptext);

							 // Now set the position
							 Point point = richTextBox1->GetPositionFromCharIndex(richTextBox1->SelectionStart);
							 point.X -= 10;
							 int fontheight = richTextBox1->Font->Height + 2;
							 point.Y += fontheight;
							 // If we're near the bottom of the screen, push the box up
							 if ((point.Y + fontheight*2) > richTextBox1->ClientSize.Height)
								 point.Y -= fontheight*2;
							 toolTip1->Show(tooltiptext, this, point);
						 }
					 }
				 }
			 }
	};
}
