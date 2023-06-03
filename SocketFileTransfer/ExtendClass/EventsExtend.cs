using System;
using System.ComponentModel;

namespace SocketFileTransfer.ExtendClass
{
	internal static class EventsExtend
	{
		internal static object Raise(this MulticastDelegate multicastDelegate, object sender, object eventArgs)
		{
			if (multicastDelegate == null)
				return null;
			object returnValue = null;
			foreach (var d in multicastDelegate.GetInvocationList())
			{
				var ISynchronizeInvoke = d.Target as ISynchronizeInvoke;
				if (ISynchronizeInvoke != null && ISynchronizeInvoke.InvokeRequired)
					returnValue = ISynchronizeInvoke.EndInvoke(ISynchronizeInvoke.BeginInvoke(d, new[] { sender, eventArgs }));
				else
					returnValue = d.DynamicInvoke(new[] { sender, eventArgs });
			}
			return returnValue;
		}
	}
}
