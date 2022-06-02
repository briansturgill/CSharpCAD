namespace CSharpCAD;

public static partial class CSCAD
{

    /*
     * Original source from quickhull3d (https://github.com/mauriciopoppe/quickhull3d)
     * Copyright (c) 2015 Mauricio Poppe
     *
     * Adapted to JSCAD by Jeff Gay
     */

    internal class QHHalfEdge
    {
        internal QHVertex vertex;
        internal QHFace face;
        internal QHHalfEdge? next;
        internal QHHalfEdge? prev;
        internal QHHalfEdge? opposite;
        internal QHHalfEdge(QHVertex vertex, QHFace face)
        {
            this.vertex = vertex;
            this.face = face;
            this.next = null;
            this.prev = null;
            this.opposite = null;
        }

        internal QHVertex Head()
        {
            return this.vertex;
        }

        internal QHVertex? Tail()
        {
            return this.prev is not null ? this.prev.vertex : null;
        }

        internal double Length()
        {
            var tail = this.Tail();
            if (tail is not null)
            {
                return tail.point.Distance(this.Head().point);
            }
            return -1;
        }

        internal double LengthSquared()
        {
            var tail = this.Tail();
            if (tail is not null)
            {
                return tail.point.SquaredDistance(this.Head().point);
            }
            return -1;
        }

        internal void SetOpposite(QHHalfEdge edge)
        {
            this.opposite = edge;
            edge.opposite = this;  
        }
    }
}