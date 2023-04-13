using System;
using System.Windows.Forms;

namespace SocketFileTransfer.ExtendClass;
internal static class ControlExtend
{
	internal static void InvokeFunctionInThradeSafeWay(this Control control, Action method)
	{
		if (control.InvokeRequired)
			control.Invoke(method);
		else
			method.Invoke();
	}
}
