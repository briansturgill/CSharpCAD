#nullable disable
namespace CSharpCAD;

internal static partial class CSharpCADInternals
{

    internal static partial class Earcut
    {

        internal class Node
        {
            internal Node prev;
            internal Node next;
            internal int i;
            internal double X;
            internal double Y;
            internal int Z;
            internal Node prevZ;
            internal Node nextZ;
            internal bool steiner;

            internal Node(int i, double x, double y)
            {
                // vertex index in coordinates array
                this.i = i;

                // vertex coordinates
                this.X = x;
                this.Y = y;

                // previous and next vertex nodes in a polygon ring
                this.prev = null;
                this.next = null;

                // z-order curve value
                this.Z = -1;

                // previous and next nodes in z-order
                this.prevZ = null;
                this.nextZ = null;

                // indicates whether this is a steiner point
                this.steiner = false;
            }
        }

        /*
         * create a node and optionally link it with previous one (in a circular doubly linked list)
         */
        internal static Node InsertNode(int i, double x, double y, Node last)
        {
            var p = new Node(i, x, y);

            if (last is null)
            {
                p.prev = p;
                p.next = p;
            }
            else
            {
                p.next = last.next;
                p.prev = last;
                last.next.prev = p;
                last.next = p;
            }

            return p;
        }

        /*
         * remove a node and join prev with next nodes
         */
        internal static void RemoveNode(Node p)
        {
            p.next.prev = p.prev;
            p.prev.next = p.next;

            if (p.prevZ is not null) p.prevZ.nextZ = p.nextZ;
            if (p.nextZ is not null) p.nextZ.prevZ = p.prevZ;
        }

    }
}