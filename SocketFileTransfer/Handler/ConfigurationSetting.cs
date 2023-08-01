using Microsoft.VisualBasic;
using SocketFileTransfer.Attributes;
using SocketFileTransfer.Canvas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SocketFileTransfer.Handler;
internal sealed class ConfigurationSetting
{
	private static string _settingFile;
	public static bool IsInitialized => File.Exists(_settingFile);

    static ConfigurationSetting()
    {
		GetDownloadFolder(new Guid("374DE290-123F-4565-9164-39C4925E467B"), 0, IntPtr.Zero, out var path);
		_settingFile = Path.Combine(path, "Setting.txt");
	}

    public static void Load()
	{
		using var sr = File.OpenText(_settingFile);
		var config = sr.ReadLine();
		var properties = typeof(ConfigurationSetting)
				.GetProperties()
				.Where(a => a.CustomAttributes.Any(a => a.AttributeType == typeof(SettingOptionAttribute)))
				.Select(a => new
				{
					Property = a,
					(a.GetCustomAttributes(true).OfType<SettingOptionAttribute>().FirstOrDefault()).Type
				})
				.ToList();
		while (!string.IsNullOrWhiteSpace(config))
		{
			var spliterIndex = config.IndexOf(':');
			
			var key = config[..spliterIndex];
			var value = config[(spliterIndex + 1)..];

			var proprity = properties.FirstOrDefault(a => a.Property.Name == key);
			if (proprity == null)
			{
				config = sr.ReadLine();
				continue;
			}
			proprity.Property.SetValue(null, Convert.ChangeType(value, proprity.Type));
			config = sr.ReadLine();
		}
	}

	public static void Initialized()
	{
		var properties = typeof(ConfigurationSetting)
				.GetProperties()
				.Where(a => a.CustomAttributes.Any(a => a.AttributeType == typeof(SettingOptionAttribute)))
				.Select(a => new
				{
					Property = a,
					(a.GetCustomAttributes(true).OfType<SettingOptionAttribute>().FirstOrDefault()).Type
				})
				.ToList();
		using var stream = File.Open(_settingFile, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
		using var sr = new StreamWriter(stream);
		foreach (var property in properties)
		{
			var key = property.Property.Name;
			var value = property.Property.GetValue(null);
			var type = property.Type;
			sr.WriteLine(key + ":" + Convert.ChangeType(value, type));
		}
	}

	private static void UpdateSetting(string proprityName, object value)
	{
		var tempFileName = Path.GetTempFileName();
		try
		{
			using (var streamReader = new StreamReader(_settingFile))
			using (var streamWriter = new StreamWriter(tempFileName))
			{
				string line;
				while ((line = streamReader.ReadLine()) != null)
				{
					if (!string.IsNullOrWhiteSpace(line))
					{
						var spliterIndex = line.IndexOf(':');
						var key = line[..spliterIndex];
						if(key != proprityName)
							streamWriter.WriteLine(line);
						else
							streamWriter.WriteLine(proprityName+':'+ value.ToString());
					}
				}
			}
			File.Copy(tempFileName, _settingFile, true);
		}
		finally
		{
			File.Delete(tempFileName);
		}
	}

	private static string _savePath;

	[SettingOption(typeof(string), "Received File Location")]
	public static string SavePath
	{
		get => _savePath == null
			? Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
			: _savePath;
		set
		{
			if (_savePath != null)
				UpdateSetting(nameof(SavePath), value);
			_savePath = value;
		}
	}

	private static int? _applicationRequiredPort;

	[SettingOption(typeof(int), "Port application is using to connect. i.e. make sure the device you are trying to connect has same value.")]
	public static int ApplicationRequiredPort
	{
		get => _applicationRequiredPort.HasValue
			? _applicationRequiredPort.Value
			: 1400;
		set
		{
			if (_applicationRequiredPort != null)
				UpdateSetting(nameof(ApplicationRequiredPort), value);
			_applicationRequiredPort = value;
		}
	}

	public static void Reset()
	{
		File.Delete(_settingFile);
	}

	[DllImport("shell32.dll", EntryPoint ="SHGetKnownFolderPath",CharSet = CharSet.Unicode)]
	static extern void GetDownloadFolder([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken, out string ppszPath);
}
