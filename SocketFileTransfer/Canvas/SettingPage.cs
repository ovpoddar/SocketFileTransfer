using SocketFileTransfer.Attributes;
using SocketFileTransfer.Handler;
using System.Linq;
using System.Windows.Forms;

namespace SocketFileTransfer.Canvas;
public partial class SettingPage : Form
{
	public SettingPage()
	{
		InitializeComponent();

		var fieldToInitilized = typeof(ConfigurationSetting).GetProperties()
			.Where(a => a.CustomAttributes.Any(a => a.AttributeType == typeof(SettingOptionAttribute)));
		foreach (var field in fieldToInitilized)
		{
			var value = field.GetValue(null);
		}
	}
}
