# WinUtil Offline

Unofficial offline `.exe` launcher for [WinUtil](https://github.com/ChrisTitusTech/winutil) by Chris Titus Tech.

It bundles the latest stable `winutil.ps1` inside a single executable. Double-click, accept the admin prompt, and WinUtil opens, with no download needed at launch.

> This project is not affiliated with or endorsed by Chris Titus Tech. All credit for WinUtil goes to its author and contributors.

## Download

Get `WinUtil-Offline.exe` from the [Releases](../../releases/latest) page.

A new release is built automatically (checked daily) whenever upstream publishes a new stable WinUtil version.

## How it works

- The launcher asks for administrator rights (UAC).
- It extracts the embedded `winutil.ps1` to a temporary file and runs it with `powershell.exe -ExecutionPolicy Bypass` (this applies to that process only).
- The temporary file is deleted when WinUtil closes.
- GitHub Actions downloads the official release script from the WinUtil releases page and compiles it into the exe, so the build is visible and verifiable in the Actions logs.

## Limitations

- **Offline for launching only.** Tweaks and settings work offline. Anything that installs apps (winget, Chocolatey) or runs Windows Update still needs internet.
- **Unsigned exe.** Windows SmartScreen or antivirus may warn about it. You can review the source (`WinUtil.cs`) and the build workflow before running.
- **Requires PowerShell**, which is built into Windows 10 and 11.
- Group Policy, AppLocker or WDAC rules can still block the script on managed PCs.

## Build it yourself

On any Windows PC, download the latest `winutil.ps1` from the [WinUtil releases](https://github.com/ChrisTitusTech/winutil/releases/latest), put it next to `WinUtil.cs`, and run:

```
%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\csc.exe /target:winexe /out:WinUtil-Offline.exe /reference:System.Windows.Forms.dll /resource:winutil.ps1,winutil.ps1 WinUtil.cs
```

## License and credits

WinUtil is created by Chris Titus Tech and distributed under the MIT license. Each release includes the upstream license as `LICENSE-WinUtil.txt`. This repository only contains the launcher wrapper and build workflow.
