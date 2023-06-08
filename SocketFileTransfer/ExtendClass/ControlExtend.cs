using System;
using System.Windows.Forms;

namespace SocketFileTransfer.ExtendClass;
internal static class ControlExtend
{
	internal static void InvokeFunctionInThreadSafeWay<T>(this T control, Action<T> method) where T : Control
	{
		if (control == null || control.IsDisposed)
			return;
		else if (control.InvokeRequired)
			control.Invoke(method);
		else
			method(control);
	}
}
