#nullable disable

namespace CSharpCAD.PolyboolDotNet;

internal class LinkedList
{
    internal LinkedList()
    {
        Root = new Node { IsRoot = true };
    }

    private Node Root { get; }

    internal bool Exists(Node node)
    {
        if (node == null || Equals(node, Root))
            return false;
        return true;
    }

    internal bool IsEmpty()
    {
        return Root.Next == null;
    }

    internal Node GetHead()
    {
        return Root.Next;
    }

    internal void InsertBefore(Node node, Func<Node, bool> check)
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

    internal Transition FindTransition(Func<Node, bool> check)
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

    internal static Node Node(Node data)
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