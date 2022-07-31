#nullable disable
namespace CSharpCAD.PolyboolDotNet;

internal class IntersectionPoint
{
    internal Vec2 Pt { get; set; }
    internal int AlongA { get; set; }
    internal int AlongB { get; set; }

    internal IntersectionPoint(int alongA, int alongB, Vec2 pt)
    {
        AlongA = alongA;
        AlongB = alongB;
        Pt = pt;
    }
}