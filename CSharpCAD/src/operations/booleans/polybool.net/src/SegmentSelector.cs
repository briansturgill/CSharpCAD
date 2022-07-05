#nullable disable
namespace CSharpCAD.PolyboolDotNet;

internal static class SegmentSelector
{
    internal static List<Segment> Union(List<Segment> segments)
    {
        return Select(segments, new[] {
                0, 2, 1, 0,
                2, 2, 0, 0,
                1, 0, 1, 0,
                0, 0, 0, 0
            });
    }

    internal static Polygon Union(Polygon first, Polygon second)
    {
        var firstPolygonRegions = PolyBool.Segments(first);
        var secondPolygonRegions = PolyBool.Segments(second);
        var combinedSegments = PolyBool.Combine(firstPolygonRegions, secondPolygonRegions);

        var union = Select(combinedSegments.Combined, new[] {
                0, 2, 1, 0,
                2, 2, 0, 0,
                1, 0, 1, 0,
                0, 0, 0, 0
            });

        foreach (var s in union)
        {
            Console.WriteLine("{0},{1} -> {2},{3}", s.Start.X, s.Start.Y, s.End.X, s.End.Y);
        }

        return new Polygon(PolyBool.SegmentChainer(union), first.Inverted || second.Inverted);
    }

    internal static List<Segment> Intersect(List<Segment> segments)
    {
        return Select(segments, new[] {   0, 0, 0, 0,
                0, 2, 0, 2,
                0, 0, 1, 1,
                0, 2, 1, 0
            });
    }

    internal static Polygon Intersect(Polygon first, Polygon second)
    {
        var firstPolygonRegions = PolyBool.Segments(first);
        var secondPolygonRegions = PolyBool.Segments(second);
        var combinedSegments = PolyBool.Combine(firstPolygonRegions, secondPolygonRegions);

        var intersection = Select(combinedSegments.Combined, new[] { 0, 0, 0, 0,
                0, 2, 0, 2,
                0, 0, 1, 1,
                0, 2, 1, 0
            });


        foreach (var s in intersection)
        {
            Console.WriteLine("{0},{1} -> {2},{3}", s.Start.X, s.Start.Y, s.End.X, s.End.Y);
        }
        return new Polygon(PolyBool.SegmentChainer(intersection), first.Inverted && second.Inverted);
    }

    internal static PolySegments Difference(CombinedPolySegments combined)
    {
        return new PolySegments
        {
            Segments = Select(combined.Combined, new[]
            {
                    0, 0, 0, 0,
                    2, 0, 2, 0,
                    1, 1, 0, 0,
                    0, 1, 2, 0
                }),
            IsInverted = !combined.IsInverted1 && combined.IsInverted2
        };
    }
    internal static Polygon Difference(Polygon first, Polygon second)
    {
        var firstPolygonRegions = PolyBool.Segments(first);
        var secondPolygonRegions = PolyBool.Segments(second);
        var combinedSegments = PolyBool.Combine(firstPolygonRegions, secondPolygonRegions);

        var difference = Select(combinedSegments.Combined, new[]
            {
                    0, 0, 0, 0,
                    2, 0, 2, 0,
                    1, 1, 0, 0,
                    0, 1, 2, 0
                });

        return new Polygon(PolyBool.SegmentChainer(difference), first.Inverted && !second.Inverted);
    }
    internal static List<Segment> DifferenceRev(List<Segment> segments)
    {
        return Select(segments, new[] {   0, 2, 1, 0,
                0, 0, 1, 1,
                0, 2, 0, 2,
                0, 0, 0, 0
            });
    }

    internal static Polygon DifferenceRev(Polygon first, Polygon second)
    {
        var firstPolygonRegions = PolyBool.Segments(first);
        var secondPolygonRegions = PolyBool.Segments(second);
        var combinedSegments = PolyBool.Combine(firstPolygonRegions, secondPolygonRegions);

        var difference = Select(combinedSegments.Combined, new[] {   0, 2, 1, 0,
                0, 0, 1, 1,
                0, 2, 0, 2,
                0, 0, 0, 0
            });

        return new Polygon(PolyBool.SegmentChainer(difference), !first.Inverted && second.Inverted);
    }
    internal static List<Segment> Xor(List<Segment> segments)
    {
        return Select(segments, new[] {   0, 2, 1, 0,
                2, 0, 0, 1,
                1, 0, 0, 2,
                0, 1, 2, 0
            });
    }
    internal static Polygon Xor(Polygon first, Polygon second)
    {
        var firstPolygonRegions = PolyBool.Segments(first);
        var secondPolygonRegions = PolyBool.Segments(second);
        var combinedSegments = PolyBool.Combine(firstPolygonRegions, secondPolygonRegions);

        var xor = Select(combinedSegments.Combined, new[] {   0, 2, 1, 0,
                2, 0, 0, 1,
                1, 0, 0, 2,
                0, 1, 2, 0
            });

        return new Polygon(PolyBool.SegmentChainer(xor), first.Inverted != second.Inverted);
    }
    private static List<Segment> Select(List<Segment> segments, int[] selection)
    {
        List<Segment> result = new List<Segment>();

        foreach (Segment segment in segments)
        {
            int index = (segment.MyFill.Above.Value ? 8 : 0) +
                        (segment.MyFill.Below.Value ? 4 : 0) +
                        (segment.OtherFill != null && segment.OtherFill.Above.Value ? 2 : 0) +
                        (segment.OtherFill != null && segment.OtherFill.Below.Value ? 1 : 0);

            if (selection[index] != 0)
            {
                result.Add(new Segment
                {
                    Start = segment.Start,
                    End = segment.End,
                    MyFill = new Fill
                    {
                        Above = selection[index] == 1,
                        Below = selection[index] == 2
                    }
                });
            }
        }

        return result;
    }
}