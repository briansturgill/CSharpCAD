#nullable disable
namespace CSharpCAD.PolyboolDotNet;

internal class CombinedPolySegments
{
    internal bool IsInverted1 { get; set; }

    internal bool IsInverted2 { get; set; }

    internal List<Segment> Combined { get; set; }
}