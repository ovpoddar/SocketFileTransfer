using System.Data;
using UtilitiTools.Models;
using WindowsFirewallHelper;

namespace UtilitiTools;
public sealed class FireWall
{
	private List<FirewallRuleModel> _rules;
	private IFirewall _firewall;

	private const string ICMPv4In = "File and Printer Sharing (Echo Request - ICMPv4-In)";
	private const string ICMPv4Out = "File and Printer Sharing (Echo Request - ICMPv4-Out)";

	private static readonly object lockObj = new object ();  
    private static FireWall instance = null;
	public static FireWall Instance
	{
		get
		{
			if (instance == null)
			{
				lock (lockObj)
					{
						if (instance == null)
						{
							instance = new FireWall();
						}
					}
			}
			return instance;
		}
	}


	FireWall()
    {
		var isDefender = FirewallManager.TryGetInstance(out _firewall);
		if (!isDefender)
			throw new Exception("Firewall initialized fail.");
		_rules = new List<FirewallRuleModel>();
	}

    public void Begin()
	{
		Backup();

		var rulses = _firewall
			.Rules
			.Where(a => a.Name == ICMPv4In
			|| a.Name == ICMPv4Out);
		
		foreach (var rule in rulses)
		{
			rule.IsEnable = true;
		}		
	}

	public void Close()
	{
		Restore();
	}

	private void Restore()
	{
		foreach (var rule in _rules)
		{
			_firewall
				.Rules
				.First(a => a.Name == rule.Name && a.Profiles == rule.Profile).IsEnable = rule.Enable;
		}
	}

	private void Backup()
	{
		var rulses = _firewall
			.Rules
			.Where(a => a.Name == ICMPv4In
			|| a.Name == ICMPv4Out);
		foreach (var rule in rulses)
		{
			_rules.Add(new FirewallRuleModel(rule));
		}
	}
}
