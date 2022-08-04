#nullable disable
namespace CSharpCAD;

internal static partial class CSharpCADInternals
{

    internal static partial class Earcut
    {

        /*
         * link every hole into the outer loop, producing a single-ring polygon without holes
         *
         * Original source from https://github.com/mapbox/earcut
         * Copyright (c) 2016 Mapbox
         */
        internal static Node EliminateHoles(double[] data, List<int> holeIndices, Node outerNode, int dim)
        {
            var queue = new List<Node>();

            for (int i = 0, len = holeIndices.Count; i < len; i++)
            {
                var start = holeIndices[i] * dim;
                var end = i < len - 1 ? holeIndices[i + 1] * dim : data.Length;
                var list = LinkedPolygon(data, start, end, dim, false);
                if (list == list.next) list.steiner = true;
                queue.Add(GetLeftmost(list));
            }

            queue.Sort((Node a, Node b) => Math.Sign(a.X - b.X)); // compare X

            // process holes from left to right
            for (var i = 0; i < queue.Count; i++)
            {
                outerNode = EliminateHole(queue[i], outerNode);
                outerNode = FilterPoints(outerNode, outerNode.next);
            }

            return outerNode;
        }

        /*
         * find a bridge between vertices that connects hole with an outer ring and link it
         */
        internal static Node EliminateHole(Node hole, Node outerNode)
        {
            var bridge = FindHoleBridge(hole, outerNode);
            if (bridge is null)
            {
                return outerNode;
            }

            var bridgeReverse = SplitPolygon(bridge, hole);

            // filter colinear points around the cuts
            var filteredBridge = FilterPoints(bridge, bridge.next);
            FilterPoints(bridgeReverse, bridgeReverse.next);

            // Check if input node was removed by the filtering
            return outerNode == bridge ? filteredBridge : outerNode;
        }

        /*
         * David Eberly's algorithm for finding a bridge between hole and outer polygon
         */
        internal static Node FindHoleBridge(Node hole, Node outerNode)
        {
            var p = outerNode;
            var hx = hole.X;
            var hy = hole.Y;
            var qx = Double.NegativeInfinity;
            Node m = null;

            // find a segment intersected by a ray from the hole's leftmost point to the left
            // segment's endpoint with lesser x will be potential connection point
            do
            {
                if (hy <= p.Y && hy >= p.next.Y && p.next.Y != p.Y)
                {
                    var x = p.X + (hy - p.Y) * (p.next.X - p.X) / (p.next.Y - p.Y);
                    if (x <= hx && x > qx)
                    {
                        qx = x;
                        if (x == hx)
                        {
                            if (hy == p.Y) return p;
                            if (hy == p.next.Y) return p.next;
                        }

                        m = p.X < p.next.X ? p : p.next;
                    }
                }
                p = p.next;
            } while (p != outerNode);

            if (m is null) return null;

            if (hx == qx) return m; // hole touches outer segment; pick leftmost endpoint

            // look for points inside the triangle of hole point, segment intersection and endpoint
            // if there are no points found, we have a valid connection
            // otherwise choose the point of the minimum angle with the ray as connection point

            var stop = m;
            var mx = m.X;
            var my = m.Y;
            var tanMin = Double.PositiveInfinity;
            p = m;

            do
            {
                if (hx >= p.X && p.X >= mx && hx != p.X &&
                    PointInTriangle(hy < my ? hx : qx, hy, mx, my, hy < my ? qx : hx, hy, p.X, p.Y))
                {
                    var tan = Math.Abs(hy - p.Y) / (hx - p.X); // tangential

                    if (LocallyInside(p, hole) && (tan < tanMin || (tan == tanMin && (p.X > m.X || (p.X == m.X && SectorContainsSector(m, p))))))
                    {
                        m = p;
                        tanMin = tan;
                    }
                }

                p = p.next;
            } while (p != stop);
            return m;
        }

        /*
         * whether sector in vertex m contains sector in vertex p in the same coordinates
         */
        internal static bool SectorContainsSector(Node m, Node p) => AreaOfT(m.prev, m, p.prev) < 0 && AreaOfT(p.next, m, m.next) < 0;

        /*
         * find the leftmost node of a polygon ring
         */
        internal static Node GetLeftmost(Node start)
        {
            var p = start;
            var leftmost = start;
            do
            {
                if (p.X < leftmost.X || (p.X == leftmost.X && p.Y < leftmost.Y)) leftmost = p;
                p = p.next;
            } while (p != start);

            return leftmost;
        }

    }
}