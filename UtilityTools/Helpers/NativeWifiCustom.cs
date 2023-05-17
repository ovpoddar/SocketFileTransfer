using ManagedNativeWifi;
using Microsoft.Win32;
using System.Xml;

namespace UtilityTools.Helpers;
internal class NativeWifiCustom
{
	const string _interfaceGuid = "{4d36e972-e325-11ce-bfc1-08002be10318}";
	const string _interfaceNamePath = @"SYSTEM\CurrentControlSet\Control\Network\{0}\{1}\Connection";

	public static bool SetProfile(Guid interfaceId, ProfileType profileType, string profileXml, string profileSecurity, bool overwrite)
	{
		if (interfaceId == Guid.Empty)
			throw new ArgumentException("The specified interface ID is invalid.", "interfaceId");

		if (string.IsNullOrWhiteSpace(profileXml))
			throw new ArgumentNullException("profileXml");

		var profileName = GetProfileName(profileXml);
		var checkIfExist = NativeWifi.EnumerateProfiles()
			.Any(a => a.Name == profileName);

		if (string.IsNullOrWhiteSpace(profileName))
			throw new ArgumentNullException("profileXml");

		if (!overwrite && checkIfExist)
			return false;

		if (checkIfExist)
			NativeWifi.DeleteProfile(interfaceId, profileName);

		var profilePath = GetProfile(profileXml);
		SetProfile(GetInterfaceName(interfaceId), ConvertToUser(profileType), profilePath);

		if (File.Exists(profilePath))
			File.Delete(profilePath);

		return true;
	}

	private static string ConvertToUser(ProfileType profileType) =>
		profileType switch
		{
			ProfileType.AllUser => "all",
			ProfileType.GroupPolicy => "all",
			ProfileType.PerUser => "current",
			_ => "all",
		};

	private static string GetProfile(string profileXml)
	{
		var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("B") + ".xml");
		File.WriteAllText(path, profileXml);
		return path;
	}

	private static string GetInterfaceName(Guid interfaceId)
	{
		var interfaceFolder = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default)
			.OpenSubKey(string.Format(_interfaceNamePath, _interfaceGuid, interfaceId.ToString("B")));
		return interfaceFolder!.GetValue("Name")!.ToString();
	}

	static string GetProfileName(string profileXml)
	{
		XmlDocument xDoc = new XmlDocument();
		xDoc.LoadXml(profileXml);
		return xDoc.GetElementsByTagName("name")[0]!.InnerText;
	}

	static void SetProfile(string interfaceName, string profileType, string profileXmlPath)
	{
		CommendPrompt.ExecuteCommand(new string[]
		{
			$"netsh wlan add profile filename=\"{profileXmlPath}\" interface=\"{interfaceName}\" user=\"{profileType}\""
		});
	}
}
