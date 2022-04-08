namespace CSharpCAD;

public static partial class CSCAD
{

    // # class Tree
    // This is the root of a BSP tree.
    // This separate class for the root of the tree in order to hold the PolygonTreeNode root.
    // The actual tree is kept in this.rootnode
    private class Tree
    {
        private PolygonTreeNode polygonTree;
        public readonly Node rootnode;
        public Tree(Poly3[] polygons)
        {
            this.polygonTree = new PolygonTreeNode(null, null);
            this.rootnode = new Node(null);
            if (polygons is not null) this.AddPolygons(polygons);
        }

        public void Invert()
        {
            this.polygonTree.Invert();
            this.rootnode.Invert();
        }

        // Remove all polygons in this BSP tree that are inside the other BSP tree
        // `tree`.
        public void ClipTo(Tree tree, bool alsoRemovecoplanarFront = false)
        {
            this.rootnode.ClipTo(tree, alsoRemovecoplanarFront);
        }

        public List<Poly3> AllPolygons()
        {
            var result = new List<Poly3>();
            this.polygonTree.GetPolygons(result);
            return result;
        }

        public void AddPolygons(Poly3[] polygons)
        {
            var polygontreenodes = new List<PolygonTreeNode>(polygons.Length);
            for (var i = 0; i < polygons.Length; i++)
            {
                polygontreenodes.Add(this.polygonTree.AddChild(polygons[i]));
            }
            this.rootnode.AddPolygonTreeNodes(polygontreenodes);
        }

        public void Clear()
        {
            this.polygonTree.Clear();
        }
    }
}