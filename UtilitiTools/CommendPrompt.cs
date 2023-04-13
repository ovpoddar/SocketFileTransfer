using System.Diagnostics;

namespace UtilitiTools;
internal class CommendPrompt
{
	internal static void ExecuteCommand(string[] args)
	{
		var processess = CreateCmdProcess();
		foreach (var arg in args)
		{
			processess.StandardInput.WriteLine(arg);
			processess.StandardInput.Flush();
		}
		processess.StandardInput.Close();
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
