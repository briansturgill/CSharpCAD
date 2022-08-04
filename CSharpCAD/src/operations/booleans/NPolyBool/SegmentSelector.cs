using System.Collections.Generic;

#nullable disable
#pragma warning disable CS1591

namespace CSharpCAD.PolyBool
{
    internal static class SegmentSelector
    {
        private static List<Segment> select(List<Segment> segments, int[] selection)
        {
            var result = new List<Segment>();
            foreach (var seg in segments)
            {
                var index = (seg.MyFill.Above == true ? 8 : 0)
                            + (seg.MyFill.Below == true ? 4 : 0)
                            + (seg.OtherFill?.Above == true ? 2 : 0)
                            + (seg.OtherFill?.Below == true ? 1 : 0);

                if (selection[index] != 0)
                {
                    result.Add(new Segment
                    {
                        Start = seg.Start,
                        End = seg.End,
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

        public static List<Segment> Union(List<Segment> segments)
        {
            //                                      primary | secondary
            // above1 below1 above2 below2    Keep?               Value
            //    0      0      0      0   =>   no                  0
            //    0      0      0      1   =>   yes filled below    2
            //    0      0      1      0   =>   yes filled above    1
            //    0      0      1      1   =>   no                  0
            //    0      1      0      0   =>   yes filled below    2
            //    0      1      0      1   =>   yes filled below    2
            //    0      1      1      0   =>   no                  0
            //    0      1      1      1   =>   no                  0
            //    1      0      0      0   =>   yes filled above    1
            //    1      0      0      1   =>   no                  0
            //    1      0      1      0   =>   yes filled above    1
            //    1      0      1      1   =>   no                  0
            //    1      1      0      0   =>   no                  0
            //    1      1      0      1   =>   no                  0
            //    1      1      1      0   =>   no                  0
            //    1      1      1      1   =>   no                  0
            return select(segments,
                          new[] { 0, 2, 1, 0,
                                  2, 2, 0, 0,
                                  1, 0, 1, 0,
                                  0, 0, 0, 0 });
        }

        public static List<Segment> Intersect(List<Segment> segments)
        {
            //                                      primary & secondary
            // above1 below1 above2 below2    Keep?               Value
            //    0      0      0      0   =>   no                  0
            //    0      0      0      1   =>   no                  0
            //    0      0      1      0   =>   no                  0
            //    0      0      1      1   =>   no                  0
            //    0      1      0      0   =>   no                  0
            //    0      1      0      1   =>   yes filled below    2
            //    0      1      1      0   =>   no                  0
            //    0      1      1      1   =>   yes filled below    2
            //    1      0      0      0   =>   no                  0
            //    1      0      0      1   =>   no                  0
            //    1      0      1      0   =>   yes filled above    1
            //    1      0      1      1   =>   yes filled above    1
            //    1      1      0      0   =>   no                  0
            //    1      1      0      1   =>   yes filled below    2
            //    1      1      1      0   =>   yes filled above    1
            //    1      1      1      1   =>   no                  0
            return select(segments,
                          new[] { 0, 0, 0, 0,
                                  0, 2, 0, 2,
                                  0, 0, 1, 1,
                                  0, 2, 1, 0 });
        }

        public static List<Segment> Difference(List<Segment> segments)
        {
            //                                      primary - secondary
            // above1 below1 above2 below2    Keep?               Value
            //    0      0      0      0   =>   no                  0
            //    0      0      0      1   =>   no                  0
            //    0      0      1      0   =>   no                  0
            //    0      0      1      1   =>   no                  0
            //    0      1      0      0   =>   yes filled below    2
            //    0      1      0      1   =>   no                  0
            //    0      1      1      0   =>   yes filled below    2
            //    0      1      1      1   =>   no                  0
            //    1      0      0      0   =>   yes filled above    1
            //    1      0      0      1   =>   yes filled above    1
            //    1      0      1      0   =>   no                  0
            //    1      0      1      1   =>   no                  0
            //    1      1      0      0   =>   no                  0
            //    1      1      0      1   =>   yes filled above    1
            //    1      1      1      0   =>   yes filled below    2
            //    1      1      1      1   =>   no                  0
            return select(segments,
                          new[] { 0, 0, 0, 0,
                                  2, 0, 2, 0,
                                  1, 1, 0, 0,
                                  0, 1, 2, 0 });
        }

        public static List<Segment> DifferenceRev(List<Segment> segments)
        {
            //                                      secondary - primary
            // above1 below1 above2 below2    Keep?               Value
            //    0      0      0      0   =>   no                  0
            //    0      0      0      1   =>   yes filled below    2
            //    0      0      1      0   =>   yes filled above    1
            //    0      0      1      1   =>   no                  0
            //    0      1      0      0   =>   no                  0
            //    0      1      0      1   =>   no                  0
            //    0      1      1      0   =>   yes filled above    1
            //    0      1      1      1   =>   yes filled above    1
            //    1      0      0      0   =>   no                  0
            //    1      0      0      1   =>   yes filled below    2
            //    1      0      1      0   =>   no                  0
            //    1      0      1      1   =>   yes filled below    2
            //    1      1      0      0   =>   no                  0
            //    1      1      0      1   =>   no                  0
            //    1      1      1      0   =>   no                  0
            //    1      1      1      1   =>   no                  0
            return select(segments,
                          new[] { 0, 2, 1, 0,
                                  0, 0, 1, 1,
                                  0, 2, 0, 2,
                                  0, 0, 0, 0 });
        }

        public static List<Segment> Xor(List<Segment> segments)
        {
            //                                      primary ^ secondary
            // above1 below1 above2 below2    Keep?               Value
            //    0      0      0      0   =>   no                  0
            //    0      0      0      1   =>   yes filled below    2
            //    0      0      1      0   =>   yes filled above    1
            //    0      0      1      1   =>   no                  0
            //    0      1      0      0   =>   yes filled below    2
            //    0      1      0      1   =>   no                  0
            //    0      1      1      0   =>   no                  0
            //    0      1      1      1   =>   yes filled above    1
            //    1      0      0      0   =>   yes filled above    1
            //    1      0      0      1   =>   no                  0
            //    1      0      1      0   =>   no                  0
            //    1      0      1      1   =>   yes filled below    2
            //    1      1      0      0   =>   no                  0
            //    1      1      0      1   =>   yes filled above    1
            //    1      1      1      0   =>   yes filled below    2
            //    1      1      1      1   =>   no                  0
            return select(segments,
                          new[] { 0, 2, 1, 0,
                                  2, 0, 0, 1,
                                  1, 0, 0, 2,
                                  0, 1, 2, 0 });
        }
    }
}
