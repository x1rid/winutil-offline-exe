using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Principal;
using System.Windows.Forms;

// Offline WinUtil launcher: winutil.ps1 is embedded in this exe.
// Relaunches itself as admin, extracts the script to %TEMP%, and runs it.
static class Program
{
    [STAThread]
    static int Main()
    {
        bool isAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent())
            .IsInRole(WindowsBuiltInRole.Administrator);

        if (!isAdmin)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = Application.ExecutablePath,
                    UseShellExecute = true,
                    Verb = "runas"
                });
            }
            catch
            {
                MessageBox.Show("Administrator rights are required to run WinUtil.",
                    "WinUtil Launcher", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 1;
            }
            return 0;
        }

        string script = Path.Combine(Path.GetTempPath(), "winutil_" + Guid.NewGuid().ToString("N") + ".ps1");
        try
        {
            using (Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream("winutil.ps1"))
            using (FileStream f = File.Create(script))
            {
                s.CopyTo(f);
            }

            var p = Process.Start(new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = "-NoProfile -ExecutionPolicy Bypass -File \"" + script + "\"",
                UseShellExecute = true
            });
            p.WaitForExit();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Could not start WinUtil:\n" + ex.Message,
                "WinUtil Launcher", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return 1;
        }
        finally
        {
            try { File.Delete(script); } catch { }
        }
        return 0;
    }
}
