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
	public async Task StartAsync()
	{
		var xmlDoc = new XmlDocument();
		// Load with XML parser
		xmlDoc.Load("Models/HotspotConfiguration.xml");

		// Get raw XML
		var provisioningXml = xmlDoc.GetXml();

		// Create ProvisiongAgent Object
		var provisioningAgent = new ProvisioningAgent();

		try
		{
			// Create ProvisionFromXmlDocumentResults Object
			var result = await provisioningAgent.ProvisionFromXmlDocumentAsync(provisioningXml);

			if (result.AllElementsProvisioned)
			{
				rootPage.NotifyUser("Provisioning was successful", NotifyType.StatusMessage);
			}
			else
			{
				rootPage.NotifyUser("Provisioning result: " + result.ProvisionResultsXml, NotifyType.ErrorMessage);
			}
		}
		catch (System.Exception ex)
		{
			// See https://learn.microsoft.com/en-us/uwp/api/windows.networking.networkoperators.provisioningagent.provisionfromxmldocumentasync
			// for list of possible exceptions.
			rootPage.NotifyUser($"Unable to provision: {ex}", NotifyType.ErrorMessage);
		}


	}
}
