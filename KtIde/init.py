def execfile(filepath, globals=None, locals=None):
    if globals is None:
        globals = {}
    globals.update({"__file__": filepath, "__name__": "__main__",})
    with open(filepath, 'rb') as file:
        exec(compile(file.read(), filepath, 'exec'), globals, locals)

def _find_constructor(class_ob):
    # Given a class object, return a function object used for the
    # constructor (ie, __init__() ) or None if we can't find one.
    try:
        return class_ob.__init__.im_func
    except AttributeError:
        for base in class_ob.__bases__:
            rc = _find_constructor(base)
            if rc is not None: 
                return rc
    return None

def get_arg_text(ob):
    import types
    import inspect
    _MAX_COLS = 85
    _MAX_LINES = 5
    _INDENT = ' ' * 4
    _default_callable_argspec = 'See source or doc'
    _invalid_method = 'invalid method signature'
    argspec = ''
    try:
        ob_call = ob.__call__
    except BaseException:
        return ''  
    fob = ob_call if isinstance(ob_call, types.MethodType) else ob
    try:
        argspec = str(inspect.signature(fob))
    except Exception as err:
        msg = str(err)
        if msg.startswith(_invalid_method):
            return _invalid_method
        else:
            argspec = ''
    if isinstance(fob, type) and argspec == '()':
        argspec = _default_callable_argspec
    lines = (textwrap.wrap(argspec, _MAX_COLS, subsequent_indent = _INDENT) if len(argspec) > _MAX_COLS else[argspec] if argspec else[])
    doc = inspect.getdoc(ob)
    if doc:
        for line in doc.split('\n', _MAX_LINES)[:_MAX_LINES] :
            line = line.strip()
            if not line:
                break
            if len(line) > _MAX_COLS:
                line = line[: _MAX_COLS - 3] + '...'
            lines.append(line)
    argspec = '\n'.join(lines)
    return argspec or _default_callable_argspec

