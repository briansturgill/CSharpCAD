namespace CSharpCAD;

public static partial class CSCAD
{

    // Earcut contains a lot of junk names so will namespace it in its own class.
    internal static partial class Earcut
    {
#nullable disable
        /*
         * An implementation of the earcut polygon triangulation algorithm.
         *
         * Original source from https://github.com/mapbox/earcut
         * Copyright (c) 2016 Mapbox
         *
         * @param {data} A flat array of vertex coordinates.
         * @param {holeIndices} An array of hole indices if any.
         * @param {dim} The number of coordinates per vertex in the input array.
         */
        public static List<int> Triangulate(double[] data, List<int> holeIndices, int dim = 2)
        {
            var hasHoles = holeIndices.Count > 0;
            var outerLen = hasHoles ? holeIndices[0] * dim : data.Length;
            var outerNode = LinkedPolygon(data, 0, outerLen, dim, true);
            var triangles = new List<int>();

            if (outerNode is null || outerNode.next == outerNode.prev) return triangles;

            double minX = 0, minY = 0, maxX = 0, maxY = 0, invSize = 0;

            if (hasHoles) outerNode = EliminateHoles(data, holeIndices, outerNode, dim);

            // if the shape is not too simple, we'll use z-order curve hash later; calculate polygon bbox
            if (data.Length > 80 * dim)
            {
                minX = maxX = data[0];
                minY = maxY = data[1];

                for (var i = dim; i < outerLen; i += dim)
                {
                    var x = data[i];
                    var y = data[i + 1];
                    if (x < minX) minX = x;
                    if (y < minY) minY = y;
                    if (x > maxX) maxX = x;
                    if (y > maxY) maxY = y;
                }

                // minX, minY and invSize are later used to transform coords into integers for z-order calculation
                invSize = Math.Max(maxX - minX, maxY - minY);
                invSize = invSize != 0 ? 1 / invSize : 0;
            }

            EarcutLinked(outerNode, triangles, dim, minX, minY, invSize);

            return triangles;
        }

        /*
         * main ear slicing loop which triangulates a polygon (given as a linked list)
         */
        internal static void EarcutLinked(Node ear, List<int> triangles, int dim, double minX,
          double minY, double invSize, int pass = 0)
        {
            if (ear is null) return;

            // interlink polygon nodes in z-order1G
            if (pass == 0 && invSize != 0) IndexCurve(ear, minX, minY, invSize);


            var stop = ear;
            Node prev;
            Node next;

            // iterate through ears, slicing them one by one
            while (ear.prev != ear.next)
            {
                prev = ear.prev;
                next = ear.next;

                if (invSize != 0 ? IsEarHashed(ear, minX, minY, invSize) : IsEar(ear))
                {
                    // cut off the triangle
                    triangles.Add(prev.i / dim);
                    triangles.Add(ear.i / dim);
                    triangles.Add(next.i / dim);

                    RemoveNode(ear);

                    // skipping the next vertex leads to less sliver triangles
                    ear = next.next;
                    stop = next.next;

                    continue;
                }
                ear = next;

                // if we looped through the whole remaining polygon and can't find any more ears
                if (ear == stop)
                {
                    // try filtering points and slicing again
                    if (pass == 0)
                    {
                        EarcutLinked(FilterPoints(ear), triangles, dim, minX, minY, invSize, 1);

                        // if this didn't work, try curing all small self-intersections locally
                    }
                    else if (pass == 1)
                    {
                        ear = CureLocalIntersections(FilterPoints(ear), triangles, dim);
                        EarcutLinked(ear, triangles, dim, minX, minY, invSize, 2);

                        // as a last resort, try splitting the remaining polygon into two
                    }
                    else if (pass == 2)
                    {
                        SplitEarcut(ear, triangles, dim, minX, minY, invSize);
                    }

                    break;
                }
            }
        }

        /*
         * check whether a polygon node forms a valid ear with adjacent nodes
         */
        internal static bool IsEar(Node ear)
        {
            var a = ear.prev;
            var b = ear;
            var c = ear.next;

            if (AreaOfT(a, b, c) >= 0) return false; // reflex, can't be an ear

            // now make sure we don't have other points inside the potential ear
            var p = ear.next.next;

            while (p != ear.prev)
            {
                if (PointInTriangle(a.x, a.y, b.x, b.y, c.x, c.y, p.x, p.y) && AreaOfT(p.prev, p, p.next) >= 0)
                {
                    return false;
                }
                p = p.next;
            }
            return true;
        }


        internal static bool IsEarHashed(Node ear, double minX, double minY, double invSize)
        {
            var a = ear.prev;
            var b = ear;
            var c = ear.next;

            if (AreaOfT(a, b, c) >= 0) return false; // reflex, can't be an ear

            // triangle bbox; min & max are calculated like this for speed
            var minTX = a.x < b.x ? (a.x < c.x ? a.x : c.x) : (b.x < c.x ? b.x : c.x);
            var minTY = a.y < b.y ? (a.y < c.y ? a.y : c.y) : (b.y < c.y ? b.y : c.y);
            var maxTX = a.x > b.x ? (a.x > c.x ? a.x : c.x) : (b.x > c.x ? b.x : c.x);
            var maxTY = a.y > b.y ? (a.y > c.y ? a.y : c.y) : (b.y > c.y ? b.y : c.y);

            // z-order range for the current triangle bbox
            var minZ = ZOrder(minTX, minTY, minX, minY, invSize);
            var maxZ = ZOrder(maxTX, maxTY, minX, minY, invSize);


            var p = ear.prevZ;
            var n = ear.nextZ;

            // look for points inside the triangle in both directions
            while (p is not null && p.z >= minZ && n is not null && n.z <= maxZ)
            {
                if (p != ear.prev && p != ear.next &&
                  PointInTriangle(a.x, a.y, b.x, b.y, c.x, c.y, p.x, p.y) &&
                  AreaOfT(p.prev, p, p.next) >= 0) return false;

                p = p.prevZ;

                if (n != ear.prev && n != ear.next &&
                  PointInTriangle(a.x, a.y, b.x, b.y, c.x, c.y, n.x, n.y) &&
                  AreaOfT(n.prev, n, n.next) >= 0) return false;

                n = n.nextZ;
            }

            // look for remaining points in decreasing z-order
            while (p is not null && p.z >= minZ)
            {
                if (p != ear.prev && p != ear.next &&
                  PointInTriangle(a.x, a.y, b.x, b.y, c.x, c.y, p.x, p.y) &&
                  AreaOfT(p.prev, p, p.next) >= 0) return false;

                p = p.prevZ;

            }

            // look for remaining points in increasing z-order
            while (n is not null && n.z <= maxZ)
            {
                if (n != ear.prev && n != ear.next &&
                  PointInTriangle(a.x, a.y, b.x, b.y, c.x, c.y, n.x, n.y) &&
                  AreaOfT(n.prev, n, n.next) >= 0) return false;

                n = n.nextZ;
            }

            return true;
        }

        /*
         * try splitting polygon into two and triangulate them independently
         */
        internal static void SplitEarcut(Node start, List<int> triangles, int dim, double minX, double minY, double invSize)
        {
            // look for a valid diagonal that divides the polygon into two
            var a = start;

            do
            {
                var b = a.next.next;

                while (b != a.prev)
                {
                    if (a.i != b.i && IsValidDiagonal(a, b))
                    {
                        // split the polygon in two by the diagonal
                        var c = SplitPolygon(a, b);
                        // filter colinear points around the cuts
                        a = FilterPoints(a, a.next);
                        c = FilterPoints(c, c.next);
                        // run earcut on each half
                        EarcutLinked(a, triangles, dim, minX, minY, invSize);
                        EarcutLinked(c, triangles, dim, minX, minY, invSize);
                        return;
                    }

                    b = b.next;
                }
                a = a.next;
            } while (a != start);
        }

        /*
         * interlink polygon nodes in z-order
         */
        internal static void IndexCurve(Node start, double minX, double minY, double invSize)
        {
            var p = start;
            do
            {
                if (p.z == null) p.z = ZOrder(p.x, p.y, minX, minY, invSize);
                p.prevZ = p.prev;
                p.nextZ = p.next;
                p = p.next;
            } while (p != start);

            if (p.prevZ is not null) p.prevZ.nextZ = null;
            p.prevZ = null;

            SortLinked(p, (p) => p.z ?? 0);
        }

        /*
         * z-order of a point given coords and inverse of the longer side of data bbox
         */
        internal static int ZOrder(double _x, double _y, double minX, double minY, double invSize)
        {
            // coords are transformed into non-negative 15-bit integer range
            int x = (int)(32767 * (_x - minX) * invSize);
            int y = (int)(32767 * (_y - minY) * invSize);

            x = (x | (x << 8)) & 0x00FF00FF;
            x = (x | (x << 4)) & 0x0F0F0F0F;
            x = (x | (x << 2)) & 0x33333333;
            x = (x | (x << 1)) & 0x55555555;

            y = (y | (y << 8)) & 0x00FF00FF;
            y = (y | (y << 4)) & 0x0F0F0F0F;
            y = (y | (y << 2)) & 0x33333333;
            y = (y | (y << 1)) & 0x55555555;

            return x | (y << 1);
        }
#nullable enable
    }
}