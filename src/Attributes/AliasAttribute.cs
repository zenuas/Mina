namespace Mina.Attributes;

public class AliasAttribute(string Name) : System.Attribute
{
    public string Name { get; } = Name;
}
