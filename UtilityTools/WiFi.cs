// Ignore Spelling: Wifi

using ManagedNativeWifi;
using System.Text;
using UtilityTools.Helpers;

namespace UtilityTools;
public sealed class Wifi
{
    private static Wifi? _instance;
    private readonly InterfaceInfo? _interface;

    private const string _hotspotTemplate = "Models/HotspotConfiguration.xml";

    private static readonly object _lockObj = new object();
    public static Wifi Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lockObj)
                {
                    if (_instance == null)
                    {
                        _instance = new Wifi();
                    }
                }
            }
            return _instance;
        }
    }

    Wifi()
    {
        _interface = NativeWifi.EnumerateInterfaces()
            .First(x =>
            {
                var radioSet = NativeWifi.GetInterfaceRadio(x.Id)?.RadioSets.FirstOrDefault();
                if (radioSet is null)
                    return false;

                if (!radioSet.HardwareOn.GetValueOrDefault()) // Hardware radio state is off.
                    return false;

                return radioSet.SoftwareOn ?? false; // Software radio state is off.
            });

    }

    public void Start()
    {
        NativeWifi.TurnOnInterfaceRadio(_interface.Id);
    }


    public void Stop()
    {
        NativeWifi.TurnOffInterfaceRadio(_interface.Id);
    }

    public List<AvailableNetworkPack> Scan() =>
        NativeWifi.EnumerateAvailableNetworks()
            .Where(a => a.Interface.Id == _interface.Id)
            .GroupBy(a => new { a = a.Ssid.ToString(), b = a.Interface.Id })
            .Select(a => a.First())
            .ToList();

    public async Task<bool> Connect(AvailableNetworkPack networkPack, string? password = null)
    {
        NativeWifiCustom.SetProfile(_interface.Id,
            ProfileType.AllUser,
            CreateProfile(networkPack.Ssid.ToString(), password),
            "WPA2PSK AES",
            true);

        return await NativeWifi.ConnectNetworkAsync(
            _interface.Id,
            networkPack.Ssid.ToString(),
            BssType.None,
            TimeSpan.FromSeconds(10));
    }

    static string CreateProfile(string profileName, string password)
    {
        var template = File.ReadAllText(_hotspotTemplate);
        var profileNameInByte = Encoding.Default.GetBytes(profileName);
        var profileNameInHEX = BitConverter.ToString(profileNameInByte).Replace("-", "");

        password ??= GeneratePassword();

        return string.Format(template, profileName, profileNameInHEX, password);
    }

    static string GeneratePassword()
    {
        var random = new Random(DateTime.UtcNow.Day);
        var password = random.Next(00000000, 99999999);
        return password.ToString();
    }
}
