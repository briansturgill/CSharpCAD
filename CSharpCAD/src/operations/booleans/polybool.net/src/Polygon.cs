#nullable disable
namespace CSharpCAD.PolyboolDotNet;

internal class Polygon
{
    internal Polygon()
    {
        Regions = new List<Region>();
    }

    internal Polygon(List<Region> regions, bool isInverted = false)
    {
        Regions = regions;
        Inverted = isInverted;
    }

    internal List<Region> Regions { get; set; }
    internal bool Inverted { get; set; }
}