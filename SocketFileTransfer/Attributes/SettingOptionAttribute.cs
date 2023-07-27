using System;

namespace SocketFileTransfer.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public class SettingOptionAttribute : Attribute
{
	public Type Type { get; }
	public string DisplayText { get; }

	public SettingOptionAttribute(Type type, string displayText = null)
	{
		Type = type;
		DisplayText = displayText;
	}
}
