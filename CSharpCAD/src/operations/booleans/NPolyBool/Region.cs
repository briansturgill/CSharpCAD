#nullable disable
#pragma warning disable CS1591

namespace CSharpCAD.PolyBool
{
    public class Region
    {
        public Region(Vec2[] points)
        {
            this.Points = points;
        }

        public Vec2[] Points { get; }
    }
}
