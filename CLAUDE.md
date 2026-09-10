# KtLab / KtIde

Small, fast Windows Python IDE. WinForms front end driving an external `python.exe -i` over redirected pipes. Keep it simple — no new dependencies, no frameworks. Ponytail is enabled for this repo (`.claude/settings.json`).

## Layout (only these three are in `KtLab.sln`)
- `KtIde/` — C# WinForms app (.NET Framework 4.8, x86). `Form1` hosts the shell, script tree, and `SyntaxEditor`.
- `KtShell/` — C++/CLI `KtShellControl` (header-heavy: logic lives in `KtShellControl.h`). Spawns Python via `CreateProcessA` + pipes, reads on a background thread. Options in `Options.cpp/.h` (serialized to `KtLabOptions.xml`).
- `ScintillaNET/` — vendored editor control + native `SciLexer.dll` (32-bit, which is why everything targets x86). Treat as third-party; don't refactor.

Not in the solution (legacy, don't touch unless asked): `KtLab/` (old C++ app), `KtIDEmin/`, `KtIdeSetup/`, `*.old` files. `KtIde/ShellRedirect.cs` is an unfinished C# replacement for KtShell.

## Build
```
& "C:\Program Files\Microsoft Visual Studio\18\Professional\MSBuild\Current\Bin\MSBuild.exe" KtLab.sln /p:Configuration=Debug "/p:Platform=Mixed Platforms" /m /v:m
```
- Use `Mixed Platforms` — it's the only solution platform that builds all three projects.
- KtShell needs the VS "Desktop development with C++" workload (toolset v145). Release config uses `/clr:pure` (deprecated).
- No tests. Verify by building and running `KtIde\bin\Debug\KtIde.exe`.
- Public repo (MIT): keep personal paths and signing keys (`*.snk`, gitignored) out of it.

## Gotchas
- Python is configured in `KtIde/KtLabOptions.xml` (`python.exe -i` from PATH, scripts in `C:\Python\Scripts`). It's read from the working directory at startup, so run `KtIde.exe` from its own folder.
- Stale hardcoded path: `C:\Python\Python3` in unused `ShellRedirect.cs`.
- `KtIde/init.py` is duplicated inside `<InitScript>` in `KtIde/KtLabOptions.xml` — change both together. `RunFile` calls its `_ktrun`, so F5 breaks if init.py doesn't load.
- Per-script env switching (`SwitchPythonFor`/`ScriptPython` in `KtShellControl.h`) keeps the active interpreter in `pythonOverride`, never in `options` — options are re-read on restart and saved on exit.
