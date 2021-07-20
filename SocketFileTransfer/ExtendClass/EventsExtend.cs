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
            object retval = null;
            foreach (var d in multicastDelegate.GetInvocationList())
            {
                var ISynchronizeInvoke = d.Target as ISynchronizeInvoke;
                if (ISynchronizeInvoke != null && ISynchronizeInvoke.InvokeRequired)
                    retval = ISynchronizeInvoke.EndInvoke(ISynchronizeInvoke.BeginInvoke(d, new[] { sender, eventArgs }));
                else
                    retval = d.DynamicInvoke(new[] { sender, eventArgs });
            }
            return retval;
        }
    }
}
