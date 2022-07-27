namespace CSharpCAD;

#nullable disable

internal static partial class Geom2Booleans
{
    internal class Contour
    {
        internal List<Vec2> points;
        internal List<int> holeIds;
        internal int holeOf;
        internal int depth;
        internal Contour()
        {
            this.points = new List<Vec2>();
            this.holeIds = new List<int>();
            this.holeOf = -1;
            this.depth = -1;
        }

        internal bool isExterior()
        {
            return this.holeOf == -1;
        }
    }
}
