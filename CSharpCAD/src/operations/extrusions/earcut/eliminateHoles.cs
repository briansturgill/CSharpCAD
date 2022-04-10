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
                if (list is not null && list == list.next) list.steiner = true;
                if (list is not null) queue.Add(GetLeftmost(list));
            }

            queue.Sort((Node a, Node b) => (int)(a.x - b.x)); // compare X

            // process holes from left to right
            for (var i = 0; i < queue.Count; i++)
            {
#nullable disable
                outerNode = EliminateHole(queue[i], outerNode);
                outerNode = FilterPoints(outerNode, outerNode.next);
            }

            return outerNode;
#nullable enable
        }

        /*
         * find a bridge between vertices that connects hole with an outer ring and link it
         */
        internal static Node? EliminateHole(Node hole, Node outerNode)
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
        internal static Node? FindHoleBridge(Node hole, Node outerNode)
        {
            var p = outerNode;
            var hx = hole.x;
            var hy = hole.y;
            var qx = Double.NegativeInfinity;
            Node? m = null;

            // find a segment intersected by a ray from the hole's leftmost point to the left
            // segment's endpoint with lesser x will be potential connection point
            do
            {
                if (p is null || p.next is null) break;
                if (hy <= p.y && hy >= p.next.y && p.next.y != p.y)
                {
                    var x = p.x + (hy - p.y) * (p.next.x - p.x) / (p.next.y - p.y);
                    if (x <= hx && x > qx)
                    {
                        qx = x;
                        if (x == hx)
                        {
                            if (hy == p.y) return p;
                            if (hy == p.next.y) return p.next;
                        }

                        m = p.x < p.next.x ? p : p.next;
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
            var mx = m.x;
            var my = m.y;
            var tanMin = Double.PositiveInfinity;
            p = m;

            do
            {
                if (hx >= p.x && p.x >= mx && hx != p.x &&
                    PointInTriangle(hy < my ? hx : qx, hy, mx, my, hy < my ? qx : hx, hy, p.x, p.y))
                {
                    var tan = Math.Abs(hy - p.y) / (hx - p.x); // tangential

                    if (LocallyInside(p, hole) && (tan < tanMin || (tan == tanMin && (p.x > m.x || (p.x == m.x && SectorContainsSector(m, p))))))
                    {
                        m = p;
                        tanMin = tan;
                    }
                }

                p = p.next;
            } while (p is not null && p != stop);
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
                if (p.x < leftmost.x || (p.x == leftmost.x && p.y < leftmost.y)) leftmost = p;
                p = p.next;
            } while (p is not null && p != start);

            return leftmost;
        }

    }
}