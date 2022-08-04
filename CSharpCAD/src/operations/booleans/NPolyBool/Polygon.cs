#nullable disable
#pragma warning disable CS1591

namespace CSharpCAD.PolyBool
{
    public class Polygon
    {
        public Polygon(Region[] regions, bool inverted = false)
        {
            this.Regions = regions;
            this.Inverted = inverted;
        }

        public Region[] Regions { get; }

        public bool Inverted { get; }
    }
}
