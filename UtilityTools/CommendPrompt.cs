using System.Diagnostics;

namespace UtilityTools;
internal class CommendPrompt
{
    internal static void ExecuteCommand(string[] args)
    {
        var processes = CreateCmdProcess();
        foreach (var arg in args)
        {
            processes.StandardInput.WriteLine(arg);
            processes.StandardInput.Flush();
        }
        processes.StandardInput.Close();
    }

    internal static async Task ExecuteCommandAsync(string[] args)
    {
        var processess = CreateCmdProcess();
        foreach (var arg in args)
        {
            await processess.StandardInput.WriteLineAsync(arg);
            await processess.StandardInput.FlushAsync();
        }
        processess.StandardInput.Close();
    }

    static Process CreateCmdProcess()
    {
        var processStartInfo = new ProcessStartInfo("cmd.exe")
        {
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            CreateNoWindow = true,
            UseShellExecute = false
        };
        return Process.Start(processStartInfo);
    }
}
