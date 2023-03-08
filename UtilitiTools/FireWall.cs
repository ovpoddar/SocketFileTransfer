using UtilitiTools.Models;
using WindowsFirewallHelper;

namespace UtilitiTools;
public sealed class FireWall
{
	private List<FirewallRuleModel> _rules;
	private readonly IFirewall _firewall;

	private const string ICMPv4In = "File and Printer Sharing (Echo Request - ICMPv4-In)";
	private const string ICMPv4Out = "File and Printer Sharing (Echo Request - ICMPv4-Out)";

	private static readonly object lockObj = new();  
    private static FireWall _instance = null!;

	public static FireWall Instance
	{
		get
		{
            if (_instance != null) return _instance;

            lock (lockObj)
                _instance ??= new FireWall();
            
            return _instance;
		}
	}


    private FireWall()
    {
        var isDefender = FirewallManager.TryGetInstance(out _firewall);

		if (!isDefender)
			throw new Exception("Firewall initialized fail.");

		_rules = new List<FirewallRuleModel>();
	}

    public void Begin()
	{
		Backup();

		var rules = _firewall
			.Rules
			.Where(a => a.Name is ICMPv4In or ICMPv4Out);
		
		foreach (var rule in rules)
            rule.IsEnable = true;
    }

	public void Close() =>
        Restore();

    private void Restore()
	{
		foreach (var rule in _rules)
            _firewall
				.Rules
				.First(a => a.Name == rule.Name && a.Profiles == rule.Profile).IsEnable = rule.Enable;
    }

	private void Backup()
	{
		var rules = _firewall
			.Rules
			.Where(a => a.Name is ICMPv4In or ICMPv4Out);
		foreach (var rule in rules)
            _rules.Add(new FirewallRuleModel(rule));
    }
}
