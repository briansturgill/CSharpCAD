#nullable disable
namespace CSharpCAD.PolyboolDotNet;

internal class Point
{
    internal Point(decimal x, decimal y)
    {
        X = x;
        Y = y;
    }

    internal decimal X { get; set; }
    internal decimal Y { get; set; }
}