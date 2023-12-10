using System;

namespace Command;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
public class CommandHelpAttribute(string s) : Attribute
{
    public string Message { get; init; } = s;
}
