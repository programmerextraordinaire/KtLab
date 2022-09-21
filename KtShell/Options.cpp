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
    return None\r\n\
def get_arg_text(ob):\r\n\
    import types \r\n\
    argText = '' \r\n\
    if ob is not None: \r\n\
        argOffset = 0 \r\n\
        if type(ob) in (types.ClassType, types.TypeType): \r\n\
            fob = _find_constructor(ob) \r\n\
            if fob is None: \r\n\
                fob = lambda: None \r\n\
            else: \r\n\
                argOffset = 1 \r\n\
        elif type(ob)==types.MethodType: \r\n\
            fob = ob.im_func \r\n\
            argOffset = 1 \r\n\
        else: \r\n\
            fob = ob \r\n\
        # Try and build one for Python defined functions \r\n\
        if type(fob) in [types.FunctionType, types.LambdaType]: \r\n\
            try: \r\n\
                realArgs = fob.func_code.co_varnames[argOffset:fob.func_code.co_argcount] \r\n\
                defaults = fob.func_defaults or [] \r\n\
                defaults = list(map(lambda name: '=%s' % repr(name), defaults)) \r\n\
                defaults = [''] * (len(realArgs)-len(defaults)) + defaults \r\n\
                items = map(lambda arg, dflt: arg+dflt, realArgs, defaults) \r\n\
                if fob.func_code.co_flags & 0x4: \r\n\
                    items.append('...') \r\n\
                if fob.func_code.co_flags & 0x8: \r\n\
                    items.append('***') \r\n\
                argText = string.join(items , ', ') \r\n\
                argText = '(%s)' % argText \r\n\
            except: \r\n\
                pass \r\n\
        # See if we can use the docstring \r\n\
        doc = getattr(ob, '__doc__', '') \r\n\
        if doc: \r\n\
            doc = doc.lstrip() \r\n\
            pos = doc.find('\\n') \r\n\
            if pos < 0 or pos > 70: \r\n\
                pos = 70 \r\n\
            if argText: \r\n\
                argText += '\\n' \r\n\
            argText += doc[:pos] \r\n\
    return argText\r\n";
    };

}
