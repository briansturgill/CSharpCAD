using System.Collections.Generic;

#nullable disable
#pragma warning disable CS1591

namespace CSharpCAD.PolyBool
{
    public class Fill
    {
        public bool? Below { get; set; }

        public bool? Above { get; set; }
    }

    public class Segment
    {
        public Vec2 End { get; set; }
        public Vec2 Start { get; set; }
        public Fill MyFill { get; set; }
        public Fill OtherFill { get; set; }
    }

    public class PolySegments
    {
        public bool Inverted { get; set; }
        public List<Segment> Segments { get; set; }
    }

    public class CombinedPolySegments
    {
        public bool Inverted1 { get; set; }
        public bool Inverted2 { get; set; }
        public List<Segment> Combined { get; set; }
    }
}
