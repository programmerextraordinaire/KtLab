#include "StdAfx.h"
#include "Options.h"

namespace KtShell {

    Options::Options() 
    {
        // Set default values
        OptionsFileName = "KtLabOptions.xml";       // File name for Serialization / Deserialization
        StartupDirectory = ".\\";                   // Use the current directory
        ScriptDirectory = "..";						// Goofy, but not a bad default
        PythonExecuteable = "C:\\Python\\Python3\\python.exe -i";       // Run Python (CPython)
        PythonErrorMessage = "Traceback (most recent call last):"; // Default error message
        FontName = "Courier New";
        FontSize = 10;
		// TODO: put good defaults in here.  Rename them to propertiesAndMethods and tooltips
		GetMethods = "dir(_object_)";
		GetMethodArgs = "get_arg_text(_object_)";

        // See: https://stackoverflow.com/questions/436198/what-is-an-alternative-to-execfile-in-python-3
        // and https://github.com/python/cpython/blob/3.10/Lib/inspect.py
        // asdf Lifted from Idle source code - calltip.py
		InitScript = "# If Python 3, no execfile\r\n\
def execfile(filepath, globals = None, locals = None):\r\n\
    if globals is None :\r\n\
        globals = {}\r\n\
    globals.update({ '__file__': filepath, '__name__' : '__main__', })\r\n\
    with open(filepath, 'rb') as file:\r\n\
        exec(compile(file.read(), filepath, 'exec'), globals, locals)\r\n\r\n\
def _find_constructor(class_ob):\r\n\
    # Given a class object, return a function object used for the\r\n\
    # constructor (ie, __init__() ) or None if we can't find one.\r\n\
    try:\r\n\
        return class_ob.__init__.im_func\r\n\
    except AttributeError:\r\n\
        for base in class_ob.__bases__:\r\n\
            rc = _find_constructor(base)\r\n\
            if rc is not None: \r\n\
                return rc\r\n\
    return None\r\n\\r\n\
def get_arg_text(ob):\r\n\
    import types \r\n\
    import inspect \r\n\
    _MAX_COLS = 85 \r\n\
    _MAX_LINES = 5  \r\n\
    _INDENT = ' ' * 4  \r\n\
    _default_callable_argspec = 'See source or doc' \r\n\
    _invalid_method = 'invalid method signature' \r\n\
    argspec = '' \r\n\
    try : \r\n\
        ob_call = ob.__call__ \r\n\
    except BaseException :  \r\n\
        return ''   \r\n\
    fob = ob_call if isinstance(ob_call, types.MethodType) else ob \r\n\
    try : \r\n\
        argspec = str(inspect.signature(fob)) \r\n\
    except Exception as err : \r\n\
        msg = str(err) \r\n\
        if msg.startswith(_invalid_method) : \r\n\
            return _invalid_method \r\n\
        else : \r\n\
            argspec = '' \r\n\
    if isinstance(fob, type) and argspec == '()' : \r\n\
        argspec = _default_callable_argspec \r\n\
    lines = (textwrap.wrap(argspec, _MAX_COLS, subsequent_indent = _INDENT) if len(argspec) > _MAX_COLS else[argspec] if argspec else[]) \r\n\
    doc = inspect.getdoc(ob) \r\n\
    if doc: \r\n\
        for line in doc.split('\n', _MAX_LINES)[:_MAX_LINES] : \r\n\
            line = line.strip() \r\n\
            if not line : \r\n\
                break \r\n\
            if len(line) > _MAX_COLS: \r\n\
                line = line[: _MAX_COLS - 3] + '...' \r\n\
            lines.append(line) \r\n\
    argspec = '\n'.join(lines) \r\n\
    return argspec or _default_callable_argspec \r\n";
    };

}
