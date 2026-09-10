# KtIde

A small, fast Python IDE for Windows.

KtIde is an interactive Python shell with a code editor and a file tree next to it. It runs your own installed `python.exe -i` in the background and talks to it over redirected stdin/stdout, so any Python works: a system install, a venv, or a conda env. There's no embedded interpreter and nothing to install into Python.

## Features

- **Interactive shell**: a real `python -i` session with syntax coloring, multi-line block entry, and error highlighting.
- **Autocomplete and call tips**: type `.` after an object to get a member list (from `dir()`), or `(` to see its signature.
- **Smart history**: Up and Down scroll through past commands. If you've typed part of a line, only commands that start with it are shown. History is saved to `History.txt` between sessions.
- **Script editor**: a Scintilla-based editor with Python syntax highlighting. **F5** runs the current file in the shell.
- **Script tree**: browse your scripts folder. Double-click a file to run it in the shell.
- **Shell shortcuts**: **Ctrl+L** clears the window, **Ctrl+R** restarts Python, and **Ctrl+N** opens a new editor.

## Requirements

- Windows
- .NET Framework 4.8
- Python 3 installed anywhere on the machine
- To build: Visual Studio 2026 (v18) with the **.NET desktop development** and **Desktop development with C++** workloads (C++/CLI, toolset v145)

## Building

Open `KtLab.sln` in Visual Studio and build with the **Mixed Platforms** solution platform. It's the only platform that builds all three projects. Or build from the command line:

```powershell
& "C:\Program Files\Microsoft Visual Studio\18\Professional\MSBuild\Current\Bin\MSBuild.exe" KtLab.sln `
    /p:Configuration=Debug "/p:Platform=Mixed Platforms" /m /v:m
```

Everything targets **x86**, because the bundled `SciLexer.dll` is 32-bit.

The output is `KtIde\bin\Debug\KtIde.exe`.

## Configuration

At startup, KtIde reads `KtLabOptions.xml` from its **working directory**, so launch `KtIde.exe` from its own folder. Edit these settings for your machine before the first run:

| Element | What it does |
|---|---|
| `PythonExecuteable` | The command that starts Python. Keep the `-i` flag. Example: `C:\Python312\python.exe -i` |
| `ScriptDirectory` | The root folder shown in the script tree |
| `FontName`, `FontSize` | The shell font |
| `InitScript` | Python code that runs at startup. It defines the helpers used for autocomplete and call tips. |
| `GetMethods`, `GetMethodArgs` | The expressions used for autocomplete and call tips. `_object_` is replaced with the name being completed. |

`KtIde/init.py` holds the same code as `<InitScript>`. If you change one, change the other to match.

## Project layout

| Folder | What's in it |
|---|---|
| `KtIde/` | The C# WinForms app: the main form, the script tree, and the `SyntaxEditor` |
| `KtShell/` | The C++/CLI `KtShellControl`. It starts Python with `CreateProcess` and pipes, reads output on a background thread, and handles history and autocomplete. Most of the logic is in `KtShellControl.h`. |
| `ScintillaNET/` | A vendored copy of ScintillaNET 2.2 and the native `SciLexer.dll` |

Some folders aren't part of `KtLab.sln`: `KtLab/` (the original C++ app), `KtIDEmin/`, `KtIdeSetup/`, and the `*.old` files. They're kept for history. `KtIde/ShellRedirect.cs` is an unfinished attempt to replace KtShell with pure C#.

## Known rough edges

- The KtShell Release configuration uses `/clr:pure`, which is deprecated.
- The project has no automated tests.

## License

KtIde is released under the [MIT License](LICENSE).

ScintillaNET and Scintilla are third-party code, covered by their own licenses. Their copyrights belong to the ScintillaNET team and to Neil Hodgson.
