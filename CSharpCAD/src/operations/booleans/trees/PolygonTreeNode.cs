namespace CSharpCAD;


public static partial class CSCAD
{

    // # class PolygonTreeNode
    // This class manages hierarchical splits of polygons.
    // At the top is a root node which does not hold a polygon, only child PolygonTreeNodes.
    // Below that are zero or more 'top' nodes; each holds a polygon.
    // The polygons can be in different planes.
    // splitByPlane() splits a node by a plane. If the plane intersects the polygon, two new child nodes
    // are created holding the splitted polygon.
    // getPolygons() retrieves the polygons from the tree. If for PolygonTreeNode the polygon is split but
    // the two split parts (child nodes) are still intact, then the unsplit polygon is returned.
    // This ensures that we can safely split a polygon into many fragments. If the fragments are untouched,
    // getPolygons() will return the original unsplit polygon instead of the fragments.
    // remove() removes a polygon from the tree. Once a polygon is removed, the parent polygons are invalidated
    // since they are no longer intact.
    private class PolygonTreeNode
    {
        private PolygonTreeNode? parent;
        private Poly3? polygon;
        private List<PolygonTreeNode> children;
        private bool removed;
        // constructor creates the root node
        public PolygonTreeNode(PolygonTreeNode? parent, Poly3? polygon)
        {
            this.parent = parent;
            this.children = new List<PolygonTreeNode>();
            this.polygon = polygon;
            this.removed = false;
        }

        // fill the tree with polygons. Should be called on the root node only; child nodes must
        // always be a derivate (split) of the parent node.
        public void AddPolygons(List<Poly3> polygons)
        {
            // new polygons can only be added to root node; children can only be splitted polygons
            if (!this.isRootNode())
            {
                Debug.Assert(!this.isRootNode(), "Attempt to add new polygons to a node that is not the root node.");
            }
            foreach (var polygon in polygons)
            {
                AddChild(polygon);
            }
        }

        // remove a node
        // - the siblings become toplevel nodes
        // - the parent is removed recursively
        public void Remove()
        {
            if (!this.removed)
            {
                this.removed = true;
                this.polygon = null;

                if (this.parent is not null && this.parent.children is not null) // Should always be true.
                {
                    // remove ourselves from the parent's children list:
                    var parentschildren = this.parent.children;
                    var i = parentschildren.IndexOf(this);
                    if (i < 0) throw new Exception("Assertion failed");
                    parentschildren.RemoveAt(i);
                }

                // Invalidate the parent's polygon, and of all parents above it:
                if (this.parent is not null) // Should always be true.
                {
                    this.parent.RecursivelyInvalidatePolygon();
                }
            }
        }

        public bool IsRemoved()
        {
            return this.removed;
        }

        public bool isRootNode()
        {
            return this.parent is null;
        }

        // invert all polygons in the tree. Call on the root node
        public void Invert()
        {
            Debug.Assert(this.isRootNode(), "Attempt to invert a PT node other than the root.");
            this.InvertSub();
        }

        public Poly3 GetPolygon()
        {
            if (this.polygon is null)
            {
                throw new NullReferenceException("Unexpected missing polygon."); // doesn't have a polygon, which means that it has been broken down
            }
            return this.polygon;
        }

        public void GetPolygons(List<Poly3> result)
        {
            var children = new List<PolygonTreeNode> { this };
            var queue = new List<List<PolygonTreeNode>> { children };
            int i, j, l;
            PolygonTreeNode node;
            for (i = 0; i < queue.Count; ++i)
            { // queue size can change in loop, don't cache length
                children = queue[i];
                for (j = 0, l = children.Count; j < l; j++)
                { // ok to cache length
                    node = children[j];
                    if (node.polygon is not null)
                    {
                        // the polygon hasn't been broken yet. We can ignore the children and return our polygon:
                        result.Add(node.polygon);
                    }
                    else
                    {
                        // our polygon has been split up and broken, so gather all subpolygons from the children
                        if (node.children.Count > 0) queue.Add(node.children);
                    }
                }
            }
        }

        // split the node by a plane; add the resulting nodes to the frontnodes and backnodes array
        // If the plane doesn't intersect the polygon, the 'this' object is added to one of the arrays
        // If the plane does intersect the polygon, two new child nodes are created for the front and back fragments,
        //  and added to both arrays.
        public void SplitByPlane(Plane plane, List<PolygonTreeNode> coplanarfrontnodes, List<PolygonTreeNode> coplanarbacknodes,
          List<PolygonTreeNode> frontnodes, List<PolygonTreeNode> backnodes)
        {
            if (this.children is not null && this.children.Count > 0)
            {
                var queue = new List<List<PolygonTreeNode>> { children };
                int i, j, l;
                PolygonTreeNode node;
                var nodes = new List<PolygonTreeNode>();
                for (i = 0; i < queue.Count; i++)
                { // queue.length can increase, do not cache
                    nodes = queue[i];
                    for (j = 0, l = nodes.Count; j < l; j++)
                    { // ok to cache length
                        node = nodes[j];
                        if (node.children.Count > 0)
                        {
                            queue.Add(node.children);
                        }
                        else
                        {
                            // no children. Split the polygon:
                            node._splitByPlane(plane, coplanarfrontnodes, coplanarbacknodes, frontnodes, backnodes);
                        }
                    }
                }
            }
            else
            {
                this._splitByPlane(plane, coplanarfrontnodes, coplanarbacknodes, frontnodes, backnodes);
            }
        }

        // only to be called for nodes with no children
        private void _splitByPlane(Plane splane, List<PolygonTreeNode> coplanarfrontnodes,
          List<PolygonTreeNode> coplanarbacknodes, List<PolygonTreeNode> frontnodes, List<PolygonTreeNode> backnodes)
        {
            var polygon = this.polygon;
            if (polygon is not null)
            {
                var (min, max) = polygon.BoundingSphere();
                var sphereradius = max + C.EPS; // ensure radius is LARGER then polygon
                var spherecenter = min;
                var d = splane.Normal.Dot(spherecenter) - splane.W;
                if (d > sphereradius)
                {
                    frontnodes.Add(this);
                }
                else if (d < -sphereradius)
                {
                    backnodes.Add(this);
                }
                else
                {
                    var (splitresult_type, splitresult_front, splitresult_back) = SplitPolygonByPlane(splane, polygon);
                    switch (splitresult_type)
                    {
                        case 0: // coplanar front:
                            coplanarfrontnodes.Add(this);
                            break;

                        case 1: // coplanar back:
                            coplanarbacknodes.Add(this);
                            break;

                        case 2: // front:
                            frontnodes.Add(this);
                            break;

                        case 3:
                            // back:
                            backnodes.Add(this);
                            break;

                        case 4: // spanning:
                            if (splitresult_front is not null)
                            {
                                var frontnode = this.AddChild(splitresult_front);
                                frontnodes.Add(frontnode);
                            }
                            if (splitresult_back is not null)
                            {
                                var backnode = this.AddChild(splitresult_back);
                                backnodes.Add(backnode);
                            }
                            break;
                    }
                }
            }
        }

        // PRIVATE methods from here:
        // add child to a node
        // this should be called whenever the polygon is split
        // a child should be created for every fragment of the split polygon
        // returns the newly created child
        public PolygonTreeNode AddChild(Poly3 polygon)
        {
            var newchild = new PolygonTreeNode(this, polygon);
            this.children.Add(newchild);
            return newchild;
        }

        public void InvertSub()
        {
            var children = new List<PolygonTreeNode> { this };
            var queue = new List<List<PolygonTreeNode>> { children };
            int i, j, l;
            PolygonTreeNode node;
            for (i = 0; i < queue.Count; i++)
            {
                children = queue[i];
                for (j = 0, l = children.Count; j < l; j++)
                {
                    node = children[j];
                    if (node.polygon is not null)
                    {
                        node.polygon = node.polygon.Invert();
                    }
                    if (node.children.Count > 0) queue.Add(node.children);
                }
            }
        }

        // private method
        // remove the polygon from the node, and all parent nodes above it
        // called to invalidate parents of removed nodes
        private void RecursivelyInvalidatePolygon()
        {
            this.polygon = null;
            if (this.parent is not null) {
                this.parent.RecursivelyInvalidatePolygon();
            }
        }

        public void Clear()
        {
            var children = new List<PolygonTreeNode> { this };
            var queue = new List<List<PolygonTreeNode>> { children };
            for (var i = 0; i < queue.Count; ++i)
            { // queue size can change in loop, don't cache length
                children = queue[i];
                var l = children.Count;
                for (var j = 0; j < l; j++)
                {
                    var node = children[j];
                    if (node.polygon is not null)
                    {
                        node.polygon = null;
                    }
                    if (node.parent is not null)
                    {
                        node.parent = null;
                    }
                    if (node.children.Count > 0) queue.Add(node.children);
                    node.children = new List<PolygonTreeNode>();
                }
            }
        }
    }
}