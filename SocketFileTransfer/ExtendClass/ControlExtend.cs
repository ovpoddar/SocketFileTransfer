using System;
using System.Threading;
using System.Windows.Forms;

namespace SocketFileTransfer.ExtendClass;
internal static class ControlExtend
{
	internal static void InvokeFunctionInThreadSafeWay<T>(this T control, Action<T> method) where T : notnull, Control
	{
		try
		{
			Thread.Sleep(500);
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
