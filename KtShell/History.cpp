#include "StdAfx.h"
#include "History.h"

namespace KtShell
{

History::History(void)
{
    filename = "History.txt";
    //history = gcnew System::Collections::ArrayList(MAX_HISTORY_LOAD);
    _index = 0;
    searchStr = "";
}

}   // End of namespace KtShell
