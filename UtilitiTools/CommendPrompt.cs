using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilitiTools;
internal class CommendPrompt
{
	internal static void ExecuteCommand(string[] args)
	{
		var processess = CreateCmdProcess();
		foreach (var arg in args)
		{
			processess.StandardInput.Write(arg);
		}
		processess.StandardInput.Close();
	}

	internal static async Task ExecuteCommandAsync(string[] args)
	{
		var processess = CreateCmdProcess();
		foreach (var arg in args)
			await processess.StandardInput.WriteAsync(arg);

		processess.StandardInput.Close();
	}

	static Process CreateCmdProcess()
	{
		var processStartInfo = new ProcessStartInfo("cmd.exe")
		{
			RedirectStandardInput = true,
			RedirectStandardOutput = true,
			//CreateNoWindow = true,
			//UseShellExecute = false
			CreateNoWindow = false,
		};
		return Process.Start(processStartInfo);
	}
}
