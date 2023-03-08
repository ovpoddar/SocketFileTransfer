using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Windows.Networking.NetworkOperators;

namespace UtilitiTools;
public class Hotspot
{
	public async Task<bool?> StartAsync()
	{
		
		// Create ProvisiongAgent Object
		var provisioningAgent = new ProvisioningAgent();
		// raw Xml
		var xml = await File.ReadAllTextAsync("Models\\HotspotConfiguration.xml");

		try
		{
			// Create ProvisionFromXmlDocumentResults Object
			var result = await provisioningAgent.ProvisionFromXmlDocumentAsync(xml);

			return result.AllElementsProvisioned;
			
		}
		catch (System.Exception ex)
		{
			// See https://learn.microsoft.com/en-us/uwp/api/windows.networking.networkoperators.provisioningagent.provisionfromxmldocumentasync
			// for list of possible exceptions.
			return null;
		}


	}
}
