namespace CSharpCAD;

public static partial class CSCAD
{
    /*
     * Original source from quickhull3d (https://github.com/mauriciopoppe/quickhull3d)
     * Copyright (c) 2015 Mauricio Poppe
     *
     * Adapted to JSCAD by Jeff Gay
     */

    internal class QHVertex
    {
        internal Vec3 point;
        internal int index;
        internal QHVertex? next;
        internal QHVertex? prev;
        internal QHFace? face;

        internal QHVertex(Vec3 point, int index) // LATER is index actually necessary?
        {
            this.point = point;
            // index in the input array
            this.index = index;
            // vertex is a double linked list node
            this.next = null;
            this.prev = null;
            // the face that is able to see this point
            this.face = null;
        }
    }
}