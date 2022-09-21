#pragma once

namespace KtShell {

    using namespace System;
    using namespace System::IO;
    using namespace System::Xml;
    using namespace System::Xml::Serialization;


public ref class Options
{
public:
	Options(void);
	// TODO: add an option to load last session

	// public variables for XML serialization
public:
    String ^ OptionsFileName;
    String ^ StartupDirectory;
    String ^ ScriptDirectory;
    String ^ PythonExecuteable;
    String ^ PythonErrorMessage;
    String ^ FontName;
    float FontSize;
	String ^ GetMethods;
	String ^ GetMethodArgs;
    String ^ InitScript;

public:
    void DeserializeXML(void)
    {
         // Create an instance of the XmlSerializer class of type Form1       
        XmlSerializer ^ x = gcnew XmlSerializer( this->GetType() );

        // Read the XML file; create it if it doesn't exist ---
		FileStream ^ fs = gcnew FileStream( OptionsFileName, FileMode::OpenOrCreate );
        try
        {
			// Deserialize the content of the XML file to a Contact array 
			// utilizing XMLReader.
			XmlReader ^ reader = gcnew XmlTextReader(fs);         
			Options ^ tmpOptions =  safe_cast <Options^> (x->Deserialize( reader ) );

			// Now set our variables
			this->FontName = tmpOptions->FontName;
			this->FontSize = tmpOptions->FontSize;
			this->OptionsFileName = tmpOptions->OptionsFileName;
			this->StartupDirectory = tmpOptions->StartupDirectory;
			this->PythonExecuteable = tmpOptions->PythonExecuteable;
			this->PythonErrorMessage = tmpOptions->PythonErrorMessage;
			this->InitScript = tmpOptions->InitScript;
			this->ScriptDirectory = tmpOptions->ScriptDirectory;
		}
        catch( System::Exception ^ e )
        {
            // Set defaults if the file does not exist or there was an error.
            this->FontName ="Courier New";
            this->FontSize = 10.0;
            this->OptionsFileName = "KtLabOptions.xml";  // File name for Serialization / Deserialization
            this->StartupDirectory = ".";                // Default to current dir
            this->PythonExecuteable = "python.exe -i";   // CPython interactive mode
            this->PythonErrorMessage = "Traceback (most recent call last):"; // CPython default message
            this->ScriptDirectory = "..";                      // Goofy, but not a bad default
			this->GetMethods = "dir(_object_)";
        }
		finally
		{
			if ( fs )
				delete (IDisposable^)fs;
		}
    }

public:
    void SerializeXML(void)
    {
        // Serialize the content of the String into the xml file
		Options ^ tmpOptions = this;
        XmlSerializer ^ x = gcnew XmlSerializer( this->GetType() );
		TextWriter ^ writer = gcnew StreamWriter( OptionsFileName );
        x->Serialize( writer, tmpOptions );
    }
};

}