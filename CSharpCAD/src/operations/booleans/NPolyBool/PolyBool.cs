using System;

#nullable disable
#pragma warning disable CS1591

namespace CSharpCAD.PolyBool
{
    public class PolyBool
    {
        private readonly Epsilon eps;

        public PolyBool(Epsilon eps = null)
        {
            this.eps = eps ?? Epsilon.Default;
        }

        public PolySegments Segments(Polygon poly)
        {
            var i = new Intersecter.RegionIntersecter(eps);
            foreach (Region region in poly.Regions)
            {
                i.AddRegion(region);
            }

            return new PolySegments
            {
                Segments = i.Calculate(poly.Inverted),
                Inverted = poly.Inverted
            };
        }

        public CombinedPolySegments Combine(PolySegments segments1, PolySegments segments2)
        {
            var i3 = new Intersecter.SegmentIntersecter(eps);
            return new CombinedPolySegments
            {
                Combined = i3.Calculate(segments1.Segments, segments1.Inverted, segments2.Segments, segments2.Inverted),
                                        Inverted1 = segments1.Inverted,
                                        Inverted2 = segments2.Inverted
            };
        }

        public PolySegments SelectUnion(CombinedPolySegments combined)
        {
            return new PolySegments
            {
                Segments = SegmentSelector.Union(combined.Combined),
                Inverted = combined.Inverted1 || combined.Inverted2
            };
        }

        public PolySegments SelectIntersect(CombinedPolySegments combined)
        {
            return new PolySegments
            {
                Segments = SegmentSelector.Intersect(combined.Combined),
                Inverted = combined.Inverted1 && combined.Inverted2
            };
        }

        public PolySegments SelectDifference(CombinedPolySegments combined)
        {
            return new PolySegments
            {
                Segments = SegmentSelector.Difference(combined.Combined),
                Inverted = combined.Inverted1 && !combined.Inverted2
            };
        }

        public PolySegments SelectDifferenceRev(CombinedPolySegments combined)
        {
            return new PolySegments
            {
                Segments = SegmentSelector.DifferenceRev(combined.Combined),
                Inverted = !combined.Inverted1 && combined.Inverted2
            };
        }

        public PolySegments SelectXor(CombinedPolySegments combined)
        {
            return new PolySegments
            {
                Segments = SegmentSelector.Xor(combined.Combined),
                Inverted = combined.Inverted1 != combined.Inverted2
            };
        }

        public Polygon Polygon(PolySegments polySegments)
        {
            return new Polygon(SegmentChainer.Chain(polySegments.Segments, eps).ToArray(), polySegments.Inverted);
        }


        public Polygon Operate(Polygon poly1, Polygon poly2, Func<CombinedPolySegments, PolySegments> selector)
        {
            var seg1 = Segments(poly1);
            var seg2 = Segments(poly2);
            var comb = Combine(seg1, seg2);
            var seg3 = selector(comb);
            return Polygon(seg3);
        }

        public Polygon Union(Polygon poly1, Polygon poly2)
        {
            return Operate(poly1, poly2, SelectUnion);
        }

        public Polygon Intersect(Polygon poly1, Polygon poly2)
        {
            return Operate(poly1, poly2, SelectIntersect);
        }

        public Polygon Difference(Polygon poly1, Polygon poly2)
        {
            return Operate(poly1, poly2, SelectDifference);
        }

        public Polygon DifferenceRev(Polygon poly1, Polygon poly2)
        {
            return Operate(poly1, poly2, SelectDifferenceRev);
        }

        public Polygon Xor(Polygon poly1, Polygon poly2)
        {
            return Operate(poly1, poly2, SelectXor);
        }
    }
}
