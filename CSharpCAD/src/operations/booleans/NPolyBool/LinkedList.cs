using System;

#nullable disable
#pragma warning disable CS1591

namespace CSharpCAD.PolyBool
{
    internal class Node : IEquatable<Node>
    {
        public Node Status { get; set; }

        public Node Other { get; set; }

        public Node Ev { get; set; }

        public Node Previous { get; set; }

        public Node Next { get; set; }

        public bool IsRoot { get; set; }

        public Action Remove { get; set; }

        public bool IsStart { get; set; }

        public Vec2 Pt { get; set; }

        public Segment Seg { get; set; }

        public bool Primary { get; set; }

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

    internal class Transition
    {
        public Node After { get; set; }
        public Node Before { get; set; }

        public Func<Node, Node> Insert { get; set; }
    }

    internal class LinkedList
    {
        public LinkedList()
        {
            Root = new Node { IsRoot = true };
        }

        private Node Root { get; }

        public bool Exists(Node node)
        {
            return node != null && !Equals(node, Root);
        }

        public bool IsEmpty()
        {
            return Root.Next == null;
        }

        public Node GetHead()
        {
            return Root.Next;
        }

        public void InsertBefore(Node node, Func<Node, bool> check)
        {
            var last = Root;
            var here = Root.Next;
            while (here != null)
            {
                if (check(here))
                {
                    node.Previous = here.Previous;
                    node.Next = here;
                    here.Previous.Next = node;
                    here.Previous = node;
                    return;
                }
                last = here;
                here = here.Next;
            }
            last.Next = node;
            node.Previous = last;
            node.Next = null;
        }

        public Transition FindTransition(Func<Node, bool> check)
        {
            var previous = Root;
            var here = Root.Next;
            while (here != null)
            {
                if (check(here))
                    break;
                previous = here;
                here = here.Next;
            }
            return new Transition
            {
                Before = Equals(previous, Root) ? null : previous,
                After = here,
                Insert = node =>
                {
                    node.Previous = previous;
                    node.Next = here;
                    previous.Next = node;
                    if (here != null)
                        here.Previous = node;
                    return node;
                }
            };
        }

        public static Node Node(Node data)
        {
            data.Previous = null;
            data.Next = null;
            data.Remove = () =>
            {
                data.Previous.Next = data.Next;
                if (data.Next != null)
                    data.Next.Previous = data.Previous;
                data.Previous = null;
                data.Next = null;
            };
            return data;
        }
    }
}
