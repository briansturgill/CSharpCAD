namespace CSharpCAD;

public static partial class CSCAD
{

    // Ugh, everything is public in a Javascript class.

    // # class Node
    // Holds a node in a BSP tree.
    // A BSP tree is built from a collection of polygons by picking a polygon to split along.
    // Polygons are not stored directly in the tree, but in PolygonTreeNodes, stored in this.polygontreenodes.
    // Those PolygonTreeNodes are children of the owning Tree.polygonTree.
    // This is not a leafy BSP tree since there is no distinction between internal and leaf nodes.
    private class Node
    {
        public Plane? plane;
        public Node? front;
        public Node? back;
        public List<PolygonTreeNode> polygontreenodes;
        public Node? parent;
        public Node(Node? parent)
        {
            this.plane = null;
            this.front = null;
            this.back = null;
            this.polygontreenodes = new List<PolygonTreeNode>();
            this.parent = parent;
        }

        // Convert solid space to empty space and empty space to solid space.
        public void Invert()
        {
            var queue = new List<Node> { this };
            for (var i = 0; i < queue.Count; i++)
            {
                var node = queue[i];
                if (node.plane is not null) node.plane = node.plane.Flip();
                if (node.front is not null) queue.Add(node.front);
                if (node.back is not null) queue.Add(node.back);
                var temp = node.front;
                node.front = node.back;
                node.back = temp;
            }
        }

        // clip polygontreenodes to our plane
        // calls remove() for all clipped PolygonTreeNodes
        public void ClipPolygons(List<PolygonTreeNode> polygontreenodes, bool alsoRemovecoplanarFront)
        {
            var current = (this, polygontreenodes);
            var stack = new Stack<(Node, List<PolygonTreeNode>)>();
            Node node;
            for (; ; )
            {
                (node, polygontreenodes) = current;

                // begin "function"
                if (node.plane is not null)
                {
                    var backnodes = new List<PolygonTreeNode>();
                    var frontnodes = new List<PolygonTreeNode>();
                    var coplanarfrontnodes = alsoRemovecoplanarFront ? backnodes : frontnodes;
                    var plane = node.plane;
                    var numpolygontreenodes = polygontreenodes.Count;
                    for (var i = 0; i < numpolygontreenodes; i++)
                    {
                        var treenode = polygontreenodes[i];
                        if (!treenode.IsRemoved())
                        {
                            // split this polygon tree node using the plane
                            // NOTE: children are added to the tree if there are spanning polygons
                            treenode.SplitByPlane(plane, coplanarfrontnodes, backnodes, frontnodes, backnodes);
                        }
                    }

                    if (node.front is not null && (frontnodes.Count > 0))
                    {
                        // add front node for further splitting
                        stack.Push((node.front, frontnodes));
                    }
                    var numbacknodes = backnodes.Count;
                    if (node.back is not null && (numbacknodes > 0))
                    {
                        // add back node for further splitting
                        stack.Push((node.back, backnodes));
                    }
                    else
                    {
                        // remove all back nodes from processing
                        for (var i = 0; i < numbacknodes; i++)
                        {
                            backnodes[i].Remove();
                        }
                    }
                }
                if (stack.Count == 0)
                {
                    break;
                }
                current = stack.Pop();
            }
        }

        // Remove all polygons in this BSP tree that are inside the other BSP tree
        // `tree`.
        public void ClipTo(Tree tree, bool alsoRemovecoplanarFront)
        {
            var node = this;
            var stack = new Stack<Node>();
            for (; ; )
            {
                if (node.polygontreenodes.Count > 0)
                {
                    tree.rootnode.ClipPolygons(node.polygontreenodes, alsoRemovecoplanarFront);
                }
                if (node.front is not null) { stack.Push(node.front); }
                if (node.back is not null) { stack.Push(node.back); }
                if (stack.Count == 0)
                {
                    break;
                }
                node = stack.Pop();
            }
        }

        public void AddPolygonTreeNodes(List<PolygonTreeNode> newpolygontreenodes)
        {
            // (Node, List<PolyTreeNode)
            var current = (this, newpolygontreenodes);
            var stack = new Stack<(Node, List<PolygonTreeNode>)>();
            for (; ; )
            {
                var (node, polygontreenodes) = current;

                if (polygontreenodes.Count == 0)
                {
                    if (stack.Count == 0)
                    {
                        break;
                    }
                    current = stack.Pop();
                    continue;
                }
                if (node.plane is null)
                {
                    var index = 0; // default
                    index = polygontreenodes.Count / 2;
                    // index = polygontreenodes.length >> 1
                    // index = Math.floor(Math.random()*polygontreenodes.length)
                    var bestpoly = polygontreenodes[index].GetPolygon();
                    node.plane = bestpoly.Plane();
                }
                var frontnodes = new List<PolygonTreeNode>();
                var backnodes = new List<PolygonTreeNode>();
                var n = polygontreenodes.Count;
                for (var i = 0; i < n; ++i)
                {
                    polygontreenodes[i].SplitByPlane(node.plane, node.polygontreenodes, backnodes, frontnodes, backnodes);
                }

                if (frontnodes.Count > 0)
                {
                    if (node.front is null) node.front = new Node(node);

                    // unable to split by any of the current nodes
                    var stopCondition = (n == frontnodes.Count && backnodes.Count == 0);
                    if (stopCondition) node.front.polygontreenodes = frontnodes;
                    else stack.Push((node.front, frontnodes));
                }
                if (backnodes.Count > 0)
                {
                    if (node.back is null) node.back = new Node(node);

                    // unable to split by any of the current nodes
                    var stopCondition = n == backnodes.Count && frontnodes.Count == 0;


                    if (stopCondition)
                    {
                        node.back.polygontreenodes = backnodes;
                    }
                    else
                    {
                        stack.Push((node.back, backnodes));
                    }
                }

                if (stack.Count == 0)
                {
                    break;
                }
                current = stack.Pop();
            }
        }
    }
}