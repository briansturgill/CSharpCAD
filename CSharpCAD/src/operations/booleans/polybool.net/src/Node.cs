#nullable disable
namespace CSharpCAD.PolyboolDotNet;

internal class Node : IEquatable<Node>
{
    internal Node Status { get; set; }

    internal Node Other { get; set; }

    internal Node Ev { get; set; }

    internal Node Previous { get; set; }

    internal Node Next { get; set; }

    internal bool IsRoot { get; set; }

    internal Action Remove { get; set; }

    internal bool IsStart { get; set; }

    internal Point Pt { get; set; }

    internal Segment Seg { get; set; }

    internal bool Primary { get; set; }

    public bool Equals(Node other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Equals(Previous, other.Previous) && Equals(Next, other.Next) && IsRoot == other.IsRoot;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Node)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (Previous != null ? Previous.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (Next != null ? Next.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ IsRoot.GetHashCode();
            return hashCode;
        }
    }
}