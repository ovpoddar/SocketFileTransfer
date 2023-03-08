using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFirewallHelper;

namespace UtilitiTools.Models;
internal class FirewallRuleModel
{
    public FirewallRuleModel(IFirewallRule rule)
    {
        Name = rule.Name;
        Profile = rule.Profiles;
        Enable = rule.IsEnable;
    }
    public string Name { get; init; }
    public FirewallProfiles Profile { get; init; }
    public bool Enable { get; init; }
}
