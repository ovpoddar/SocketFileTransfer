using System;
using System.Windows.Forms;

namespace SocketFileTransfer.ExtendClass;
internal static class ControlExtend
{
	internal static void InvokeFunctionInThreadSafeWay<T>(this T control, Action<T> method) where T : notnull, Control
	{
		try
		{
			if (control.IsDisposed)
				return;
			else if (control.InvokeRequired)
				control.Invoke(() =>
				{
					method(control);
				});
			else
				method(control);
		}
		catch
		{
		}
	}
}
