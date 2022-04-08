namespace CSharpCAD;

public static partial class CSCAD
{

    internal static partial class Earcut
    {

        // Simon Tatham's linked list merge sort algorithm
        // https://www.chiark.greenend.org.uk/~sgtatham/algorithms/listsort.html
        internal static Node? SortLinked(Node? list, Func<Node?, int> fn)
        {
            int i;
            Node? p;
            Node? q;
            Node? e;
            int numMerges;
            int inSize = 1;

            do
            {
                p = list;
                list = null;
                Node? tail = null;
                numMerges = 0;

                while (p is not null)
                {
                    numMerges++;
                  q = p;
                    var pSize = 0;
                    for (i = 0; i < inSize; i++)
                    {
                        pSize++;
                        q = q.nextZ;
                        if (q is null) break;
                    }

                    var qSize = inSize;


                    while (pSize > 0 || (qSize > 0 && q is not null))
                    {
                        if (pSize != 0 && (qSize == 0 || q is null || fn(p) <= fn(q)))
                        {
                            e = p;
                            if (p is not null) p = p.nextZ;
                            pSize--;
                        }
                        else
                        {
                            e = q;
                            if (q is not null) q = q.nextZ;
                            qSize--;
                        }
                        if (tail is not null) tail.nextZ = e;
                        else list = e;

                        if (e is not null) e.prevZ = tail;
                        tail = e;
                    }
                    p = q;
                }
                if (tail is not null) tail.nextZ = null;
                inSize *= 2;
            } while (numMerges > 1);

            return list;
        }
    }
}