using System;
using System.Windows.Forms;

namespace SocketFileTransfer.ExtendClass;
internal static class ControlExtend
{
	internal static void InvokeFunctionInThreadSafeWay(this Control control, Action method)
	{
		if (control == null && control.IsDisposed)
			return;
		else if (control.InvokeRequired)
			control.Invoke(method);
		else
			method.Invoke();
	}
}
