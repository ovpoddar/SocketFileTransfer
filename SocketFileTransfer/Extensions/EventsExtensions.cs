using System;
using System.ComponentModel;

namespace SocketFileTransfer.Extensions
{
    internal static class EventsExtensions
    {
        internal static object Raise(this MulticastDelegate multicastDelegate, object sender, object eventArgs)
        {
            if (multicastDelegate == null)
                return null;
            object retVal = null;

            foreach (var d in multicastDelegate.GetInvocationList())
                retVal = d.Target is ISynchronizeInvoke ISynchronizeInvoke && ISynchronizeInvoke.InvokeRequired
                    ? ISynchronizeInvoke.EndInvoke(ISynchronizeInvoke.BeginInvoke(d, new[] {sender, eventArgs}))
                    : d.DynamicInvoke(new[] {sender, eventArgs});

            return retVal;
        }
    }
}
