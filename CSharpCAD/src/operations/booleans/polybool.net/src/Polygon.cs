#nullable disable
namespace CSharpCAD.PolyboolDotNet;

internal class Polygon
{
    internal Polygon()
    {
        List<List<Vec2>> Regions = new List<List<Vec2>>();
    }

    internal Polygon(List<List<Vec2>> regions, bool isInverted = false)
    {
        Regions = regions;
        Inverted = isInverted;
    }

    internal List<List<Vec2>> Regions { get; set; }
    internal bool Inverted { get; set; }
}