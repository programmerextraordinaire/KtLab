#pragma once

using namespace System;
using namespace System::Collections;
using namespace System::Data;
using namespace System::IO;
using namespace System::Media;


const int MAX_HISTORY_LOAD = 4096;


namespace KtShell
{
	// Class to notify interested modules of history being added
	public ref class HistoryAddedEventArgs : public EventArgs
	{
	private: 
		String ^ _historyline;

	public: 
		// ctor
		HistoryAddedEventArgs(String ^ line)
		{
			this->_historyline = line;
		}

	public: property String ^ HistoryLine
			{ 
				String ^ get() { return _historyline; }
			}
	};


	public delegate void HistoryAddedEventHandler(System::Object ^  sender, HistoryAddedEventArgs ^ e);


	public ref class History : public ArrayList
	{
	public:
		History(void);

		// Event delegate and handler
		// Event handler delegate

	public: event HistoryAddedEventHandler ^ HistoryAdded;
	protected:
		virtual void OnHistoryAdded(HistoryAddedEventArgs ^ e)
		{
			//if (HistoryAdded)
			HistoryAdded(this, e);
		}

		// member vars
	private:
		String ^ filename;
		int _index;
	public:
		String ^ searchStr;
		String ^ GetStringAtIndex(int idx) { return this[idx]->ToString(); }
	public: property int Index
			{
				int get() { return _index; }
				void set( int val) { _index = val; }
			}

	public: property String ^ Next
			{
				String ^ get()
				{
					if (this->Count > 0)
					{
						if (_index < (this->Count - 1))       // Account for zero-offset
							_index++;
						// Beep if the user hits the last element
						if (_index == (this->Count - 1))
							SystemSounds::Beep->Play();
						return this[_index]->ToString();
					}
					return "";
				}
			}

	public: property String ^ Previous
			{
				String ^ get()
				{
					if (this->Count > 0)
					{
						if (_index > 0)       // Account for zero-offset
							_index--;
						// Beep if the user hits the first element
						if (0 == _index )
							SystemSounds::Beep->Play();
						return this[_index]->ToString();
					}
					return "";
				}
			}

	public:
		String ^ FindNext(String ^ searchStr)
		{
			if (this->Count > 0)
			{
				while (_index < (this->Count - 1))       // Account for zero-offset
				{
					_index++;
					if (this[_index]->ToString()->StartsWith(searchStr))
						return this[_index]->ToString();
				}
			}
			// Beep if the user hits the last element without finding a match
			SystemSounds::Beep->Play();
			return "";
		}



	public:
		String ^ FindPrevious(String ^ searchStr)
		{
			if (this->Count > 0)
			{
				while (_index > 0)
				{
					_index--;
					if (this[_index]->ToString()->StartsWith(searchStr))
						return this[_index]->ToString();
				}
			}
			// Beep if the user hits the last element without finding a match
			SystemSounds::Beep->Play();
			return "";
		}


	public: virtual void Clear() override
			{
				this->Clear();
				_index = this->Count;
			}

			// TODO: add Session string format to Options
			// This accomodates other comment delimiters (Ruby?)
	public:
		/// <summary> 
		/// Adds a date-time stamp to the current history
		/// </summary> 
		void AddTimestamp(void)
		{
			String ^ timestamp = "#------------    {0}    ------------#";
			timestamp = String::Format(timestamp, System::DateTime::Now.ToString("F"));    // "g" for short date/time
			SaveHistoryLine(timestamp);
		}


	public:
		///  <summary> 
		/// Save the given line to the history file.
		///  </summary> 
		bool SaveHistoryLine(String ^ line)
		{
			bool retval = false;

			// Only process lines with data of at least 1 char
			if (line->Length > 0)
			{
				AddLine(line);
				StreamWriter ^ sw = File::AppendText(filename);
				try
				{
					sw->WriteLine(line);
					retval = true;
				}
				catch( System::Exception ^ e )
				{
					throw e;
				}
				finally
				{
					if (sw)
						sw->Close();
				}
			}

			// Done
			this->_index = this->Count;
			return retval;
		}


	private:
		/// <summary> 
		/// Add a line to the history array and raise the Added event
		/// </summary> 
		void AddLine(String ^ line)
		{
			_index = this->Add(line);
			// Create the event to notify interested parties...
			HistoryAddedEventArgs ^ e = gcnew HistoryAddedEventArgs(line);
			OnHistoryAdded(e);
		}


	public:
		/// <summary> 
		/// Load the history file into a list, one line per list element.
		/// </summary> 
		bool LoadHistory(void)
		{
			// Read the history file
			bool retval = false;
			if (!File::Exists(filename))
			{
				FileStream ^ fs = File::Create(filename);
				fs->Close();
			}
			StreamReader ^ sr = gcnew StreamReader( File::OpenRead(filename));

			try
			{  
				String ^ line = String::Empty;
				// Read and display lines from the file until the end of 
				// the file is reached.
				while (!sr->EndOfStream) 
				{
					line = sr->ReadLine();
					this->Add(line);
				}
				// Now delete all earliest entries so the list is 
				// no bigger than MAX_HISTORY_LOAD.
				if (this->Count > MAX_HISTORY_LOAD)
				{
					int num = this->Count - MAX_HISTORY_LOAD;
					this->RemoveRange(0, num);
				}
				_index = this->Count;
				retval = true;
			}
			catch( System::Exception ^ e )
			{
				throw e;
			}
			finally
			{
				if (sr)
					sr->Close();		// Close the file
			}

			return retval;
		}

		// TODO: Idea... load a session from the history file
		// pass in a number or a date, and search for the corresponding session
	};

}