using SocketFileTransfer.Attributes;
using System;
using System.IO;
using System.Linq;

namespace SocketFileTransfer.Handler;
internal sealed class ConfigurationSetting
{
	private const string _settingFile = "C:\\Users\\Ayan\\Desktop\\setting.txt";
	public static bool IsInitialized => File.Exists(_settingFile);

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
		using var stream = File.Open(_settingFile, FileMode.Create, FileAccess.Write, FileShare.None);
		using var sr = new StreamWriter(stream);
		foreach (var property in properties)
		{
			var key = property.Property.Name;
			var value = property.Property.GetValue(null);
			var type = property.Type;
			sr.WriteLine(key + ":" + Convert.ChangeType(value, type));
		}
	}

	private static DirectoryInfo _savePath;

	[SettingOption(typeof(string))]
	public static string SavePath
	{
		get => _savePath == null
			? Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
			: _savePath.ToString();
		set => _savePath = new(value);
	}

	private static int? _applicationRequiredPort;

	[SettingOption(typeof(int))]
	public static int ApplicationRequiredPort
	{
		get => _applicationRequiredPort.HasValue
			? _applicationRequiredPort.Value
			: 1400;
		set => _applicationRequiredPort = value;
	}

}
