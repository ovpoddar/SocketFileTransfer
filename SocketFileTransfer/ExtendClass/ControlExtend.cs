using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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
