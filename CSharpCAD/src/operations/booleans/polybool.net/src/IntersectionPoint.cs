#nullable disable
namespace CSharpCAD.PolyboolDotNet;

internal class IntersectionPoint
{
    internal Point Pt { get; set; }
    internal int AlongA { get; set; }
    internal int AlongB { get; set; }

    internal IntersectionPoint(int alongA, int alongB, Point pt)
    {
        AlongA = alongA;
        AlongB = alongB;
        Pt = pt;
    }
}