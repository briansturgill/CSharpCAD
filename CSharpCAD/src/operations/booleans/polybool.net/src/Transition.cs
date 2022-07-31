#nullable disable
namespace CSharpCAD.PolyboolDotNet;

internal class Transition
{
    internal Node After { get; set; }
    internal Node Before { get; set; }

    internal Func<Node, Node> Insert { get; set; }
}