namespace CSharpCAD;

internal static partial class Geom2Booleans
{
    internal class Node<Key, Value>
    {
        internal Key? key;
        internal Value? data;
        internal Node<Key, Value>? left;
        internal Node<Key, Value>? right;
        internal Node<Key, Value>? next;

        internal Node(Key? key, Value? data)
        {
            this.key = key;
            this.data = data;
            this.left = null;
            this.right = null;
            this.next = null;
        }
    }
    internal class SplayTree<Key, Value>
    {
        // LATER export type Comparator<Key> = (a:Key, b:Key) => number;
        // LATER export type Visitor<Key, Value> = (node:Node<Key, Value>) => void;
        // LATER export type NodePrinter<Key, Value> = (node:Node<Key, Value>) => string;
        // LATER type TreeNodeList<Key, Value> = { head: Node<Key, Value> | null };
        // LATER type StringCollector = (s: string) => void;

        /* follows "An implementation of top-down splaying"
         * by D. Sleator <sleator@cs.cmu.edu> March 1992
         */


        internal delegate int STComparator<KeyT>(KeyT? a, KeyT? b);
        internal delegate void Visitor(Node<Key, Value> node);

        internal int DEFAULT_COMPARE(Key? a, Key? b)
        {
            throw new NotImplementedException();
        }

        /*
         * Simple top down splay, not requiring i to be in the tree t.
         */
        private Node<Key, Value> splay(Key? i, Node<Key, Value> t, STComparator<Key> comparator)
        {
            var N = new Node<Key, Value>(default(Key), default(Value));
            var l = N;
            var r = N;

            while (true)
            {
                var cmp = comparator(i, t.key);
                //if (i < t.key)
                if (cmp < 0)
                {
                    if (t.left == null) break;
                    //if (i < t.left.key)
                    if (comparator(i, t.left.key) < 0)
                    {
                        var y = t.left;                           /* rotate right */
                        t.left = y.right;
                        y.right = t;
                        t = y;
                        if (t.left == null) break;
                    }
                    r.left = t;                               /* link right */
                    r = t;
                    t = t.left;
                    // else if (i > t.key)
                }
                else if (cmp > 0)
                {
                    if (t.right == null) break;
                    //if (i > t.right.key)
                    if (comparator(i, t.right.key) > 0)
                    {
                        var y = t.right;                          /* rotate left */
                        t.right = y.left;
                        y.left = t;
                        t = y;
                        if (t.right == null) break;
                    }
                    l.right = t;                              /* link left */
                    l = t;
                    t = t.right;
                }
                else break;
            }
            /* assemble */
            l.right = t.left;
            r.left = t.right;
            t.left = N.right;
            t.right = N.left;
            return t;
        }


        internal Node<Key, Value> insert(Key i, Value? data, Node<Key, Value>? t, STComparator<Key> comparator)
        {
            var node = new Node<Key, Value>(i, data);

            if (t == null)
            {
                node.left = node.right = null;
                return node;
            }

            t = splay(i, t, comparator);
            var cmp = comparator(i, t.key);
            if (cmp < 0)
            {
                node.left = t.left;
                node.right = t;
                t.left = null;
            }
            else if (cmp >= 0)
            {
                node.right = t.right;
                node.left = t;
                t.right = null;
            }
            return node;
        }


        // returns left node, right node as a tuple.
        internal (Node<Key, Value>?, Node<Key, Value>?) split(Key? key, Node<Key, Value>? v, STComparator<Key> comparator)
        {
            Node<Key, Value>? left = null;
            Node<Key, Value>? right = null;
            if (v is not null)
            {
                v = splay(key, v, comparator);

                var cmp = comparator(v.key, key);
                if (cmp == 0)
                {
                    left = v.left;
                    right = v.right;
                }
                else if (cmp < 0)
                {
                    right = v.right;
                    v.right = null;
                    left = v;
                }
                else
                {
                    left = v.left;
                    v.left = null;
                    right = v;
                }
            }
            return (left, right);
        }

        internal Node<Key, Value>? merge(Node<Key, Value>? left, Node<Key, Value>? right, STComparator<Key> comparator)
        {
            if (right is null) return left;
            if (left is null) return right;

            right = splay(left.key, right, comparator);
            right.left = left;
            return right;
        }


        private STComparator<Key> _comparator;
        private Node<Key, Value>? _root;
        private int _size = 0;

        internal SplayTree(STComparator<Key>? comparator = null)
        {
            this._comparator = comparator ?? DEFAULT_COMPARE;
        }

        /*
         * Inserts a key, allows duplicates
         */
        internal Node<Key, Value> insert(Key key, Value? data)
        {
            this._size++;
            return this._root = insert(key, data, this._root, this._comparator);
        }

        /*
         * Adds a key, if it is not present in the tree
         */
        internal Node<Key, Value> add(Key key, Value? data)
        {
            var node = new Node<Key, Value>(key, data);

            if (this._root == null)
            {
                node.left = node.right = null;
                this._size++;
                this._root = node;
            }

            var comparator = this._comparator;
            var t = splay(key, this._root, comparator);
            var cmp = comparator(key, t.key);
            if (cmp == 0) this._root = t;
            else
            {
                if (cmp < 0)
                {
                    node.left = t.left;
                    node.right = t;
                    t.left = null;
                }
                else if (cmp > 0)
                {
                    node.right = t.right;
                    node.left = t;
                    t.right = null;
                }
                this._size++;
                this._root = node;
            }

            return this._root;
        }


        /*
         * @param  {Key} key
         * @return {Node|null}
         */
        internal void remove(Key? key)
        {
            this._root = this._remove(key, this._root, this._comparator);
        }


        /*
         * Deletes i from the tree if it's there
         */
        private Node<Key, Value>? _remove(Key? i, Node<Key, Value>? t, STComparator<Key> comparator)
        {
            Node<Key, Value>? x;
            if (t is null) return null;
            t = splay(i, t, comparator);
            var cmp = comparator(i, t.key);
            if (cmp == 0)
            {               /* found it */
                if (t.left is null)
                {
                    x = t.right;
                }
                else
                {
                    x = splay(i, t.left, comparator);
                    x.right = t.right;
                }
                this._size--;
                return x;
            }
            return t;                         /* It wasn't there */
        }


        /*
         * Removes and returns the node with smallest key
         */
        internal (Key?, Value?)? pop()
        {
            var node = this._root;
            if (node is not null && this._root is not null)
            {
                while (node.left is not null) node = node.left;
                this._root = splay(node.key, this._root, this._comparator);
                this._root = this._remove(node.key, this._root, this._comparator);
                return (node.key, node.data);
            }
            return null;
        }


        /*
         * Find without splaying
         */
        internal Node<Key, Value>? findStatic(Key key)
        {
            var current = this._root;
            var compare = this._comparator;
            while (current is not null)
            {
                var cmp = compare(key, current.key);
                if (cmp == 0) return current;
                else if (cmp < 0) current = current.left;
                else current = current.right;
            }
            return null;
        }


        internal Node<Key, Value>? find(Key key)
        {
            if (this._root is not null)
            {
                this._root = splay(key, this._root, this._comparator);
                if (this._comparator(key, this._root.key) != 0) return null;
            }
            return this._root;
        }


        internal bool contains(Key key)
        {
            var current = this._root;
            var compare = this._comparator;
            while (current is not null)
            {
                var cmp = compare(key, current.key);
                if (cmp == 0) return true;
                else if (cmp < 0) current = current.left;
                else current = current.right;
            }
            return false;
        }


        internal SplayTree<Key, Value> forEach(Visitor visitor)
        {
            var current = this._root;
            var Q = new Stack<Node<Key, Value>>();  /* Initialize stack s */
            var done = false;

            while (!done)
            {
                if (current != null)
                {
                    Q.Push(current);
                    current = current.left;
                }
                else
                {
                    if (Q.Count != 0)
                    {
                        current = Q.Pop();
                        visitor(current);

                        current = current.right;
                    }
                    else done = true;
                }
            }
            return this;
        }


#if LATER
// This function is broken... Vistor is defined to return void.
        /*
         * Walk key range from `low` to `high`. Stops if `fn` returns a value.
         */
        internal SplayTree<Key, Value> range(Key low, Key high, Visitor fn)
        {
            var Q = new Stack<Node<Key, Value>>();
            var compare = this._comparator;
            var node = this._root;
            int cmp;

            while (Q.Count != 0 || node is not null)
            {
                if (node is not null)
                {
                    Q.Push(node);
                    node = node.left;
                }
                else
                {
                    node = Q.Pop();
                    cmp = compare(node.key, high);
                    if (cmp > 0)
                    {
                        break;
                    }
                    else if (compare(node.key, low) >= 0)
                    {
                        if (fn(node)) return this; // stop if smth is returned
                    }
                    node = node.right;
                }
            }
            return this;
        }
#endif

        /*
         * Returns array of keys
         */
        internal List<Key?> keys()
        {
            var keys = new List<Key?>();
            this.forEach((node) => keys.Add(node.key));
            return keys;
        }


        /*
         * Returns array of all the data in the nodes
         */
        internal List<Value?> values()
        {
            var values = new List<Value?>();
            this.forEach((node) => values.Add(node.data));
            return values;
        }


        internal Key? min()
        {
            if (this._root is not null)
            {
                var n = this.minNode(this._root);
                if (n is not null) return n.key;
            }
            return default(Key);
        }


        internal Key? max()
        {
            if (this._root is not null)
            {
                var n = this.maxNode(this._root);
                if (n is not null) return n.key;
            }
            return default(Key);
        }


        internal Node<Key, Value>? minNode(Node<Key, Value>? t)
        {
            if (t is not null) while (t.left is not null) t = t.left;
            return t;
        }


        internal Node<Key, Value>? maxNode(Node<Key, Value>? t)
        {
            if (t is not null) while (t.right is not null) t = t.right;
            return t;
        }


        /*
         * Returns node at given index
         */
        internal Node<Key, Value>? at(int index)
        {
            var current = this._root;
            var done = false;
            var i = 0;
            var Q = new Stack<Node<Key, Value>>();

            while (!done)
            {
                if (current is not null)
                {
                    Q.Push(current);
                    current = current.left;
                }
                else
                {
                    if (Q.Count > 0)
                    {
                        current = Q.Pop();
                        if (i == index) return current;
                        i++;
                        current = current.right;
                    }
                    else done = true;
                }
            }
            return null;
        }


        internal Node<Key, Value>? next(Node<Key, Value> d)
        {
            var root = this._root;
            Node<Key, Value>? successor = null;

            if (d.right is not null)
            {
                successor = d.right;
                while (successor.left is not null) successor = successor.left;
                return successor;
            }

            var comparator = this._comparator;
            while (root is not null)
            {
                var cmp = comparator(d.key, root.key);
                if (cmp == 0) break;
                else if (cmp < 0)
                {
                    successor = root;
                    root = root.left;
                }
                else root = root.right;
            }

            return successor;
        }


        internal Node<Key, Value>? prev(Node<Key, Value> d)
        {
            var root = this._root;
            Node<Key, Value>? predecessor = null;

            if (d.left is not null)
            {
                predecessor = d.left;
                while (predecessor.right is not null) predecessor = predecessor.right;
                return predecessor;
            }

            var comparator = this._comparator;
            while (root is not null)
            {
                var cmp = comparator(d.key, root.key);
                if (cmp == 0) break;
                else if (cmp < 0) root = root.left;
                else
                {
                    predecessor = root;
                    root = root.right;
                }
            }
            return predecessor;
        }


        internal SplayTree<Key, Value> clear()
        {
            this._root = null;
            this._size = 0;
            return this;
        }


        internal Node<Key, Value>? toList()
        {
            return toList(this._root);
        }


#if LATER
//Needed??
        /**
         * Bulk-load items. Both array have to be same size
         */
        internal load(keys:Key[], values: Value[] = [], presort:boolean = false)
        {
            var size = keys.length;
            var comparator = this._comparator;

            // sort if needed
            if (presort) sort(keys, values, 0, size - 1, comparator);

            if (this._root == null)
            { // empty tree
                this._root = loadRecursive(keys, values, 0, size);
                this._size = size;
            }
            else
            { // that re-builds the whole tree from two in-order traversals
                var mergedList = mergeLists(this.toList(), createList(keys, values), comparator);
                size = this._size + size;
                this._root = sortedListToBST({ head: mergedList }, 0, size);
            }
            return this;
        }
#endif


        internal bool isEmpty()
        {
            return this._root == null;
        }


        internal int size { get => this._size; }
        internal Node<Key, Value>? root { get => this._root; }

#if LATER
internal toString(printNode:NodePrinter<Key, Value> = (n) => String(n.key)) : string
{
    var out:string[] = [];
    printRow(this._root, '', true, (v) => out.push(v), printNode);
    return out.join('');
}
#endif


        internal void update(Key key, Key newKey, Value? newData)
        {
            var comparator = this._comparator;
            var (left, right) = split(key, this._root, comparator);
            if (comparator(key, newKey) < 0)
            {
                right = insert(newKey, newData, right, comparator);
            }
            else
            {
                left = insert(newKey, newData, left, comparator);
            }
            this._root = merge(left, right, comparator);
        }


        internal (Node<Key, Value>?, Node<Key, Value>?) split(Key key)
        {
            return split(key, this._root, this._comparator);
        }

#if LATER
*[Symbol.iterator] () {
    var n = this.minNode();
    while (n)
    {
        yield n;
n = this.next(n);
}
}
#endif


#if LATER
function loadRecursive(keys:Key[], values: Value[], start:number, end: number) : Node<Key, Value> | null {
    var size = end - start;
    if (size > 0)
    {
        var middle = start + Math.floor(size / 2);
        var key = keys[middle];
        var data = values[middle];
        var node = new Node(key, data);
        node.left = loadRecursive(keys, values, start, middle);
        node.right = loadRecursive(keys, values, middle + 1, end);
        return node;
    }
    return null;
}
#endif


        internal Node<Key, Value>? createList(List<Key> keys, List<Value> values)
        {
            var head = new Node<Key, Value>(default(Key), default(Value));
            var p = head;
            for (var i = 0; i < keys.Count; i++)
            {
                p = p.next = new Node<Key, Value>(keys[i], values[i]);
            }
            p.next = null;
            return head.next;
        }


        internal Node<Key, Value>? toList(Node<Key, Value>? root)
        {
            var current = root;
            var Q = new Stack<Node<Key, Value>>();
            var done = false;

            var head = new Node<Key, Value>(default(Key), default(Value));
            var p = head;

            while (!done)
            {
                if (current is not null)
                {
                    Q.Push(current);
                    current = current.left;
                }
                else
                {
                    if (Q.Count > 0)
                    {
                        current = p = p.next = Q.Pop();
                        current = current.right;
                    }
                    else done = true;
                }
            }
            p.next = null; // that'll work even if the tree was empty
            return head.next;
        }


#if LATER
// What is this?
internal Node<Key, Value> sortedListToBST(list:TreeNodeList<Key, Value>, start: number, end: number) {
    var size = end - start;
    if (size > 0)
    {
        var middle = start + Math.floor(size / 2);
        var left = sortedListToBST(list, start, middle);

        var root = list.head;
        root.left = left;

        list.head = list.head.next;

        root.right = sortedListToBST(list, middle + 1, end);
        return root;
    }
    return null;
}
#endif


        internal Node<Key, Value>? mergeLists(Node<Key, Value> l1, Node<Key, Value> l2, STComparator<Key> compare)
        {
            var head = new Node<Key, Value>(default(Key), default(Value)); // dummy
            var p = head;

            var p1 = l1;
            var p2 = l2;

            while (p1 != null && p2 != null)
            {
                if (compare(p1.key, p2.key) < 0)
                {
                    p.next = p1;
                    p1 = p1.next;
                }
                else
                {
                    p.next = p2;
                    p2 = p2.next;
                }
                p = p.next;
            }

            if (p1 != null)
            {
                p.next = p1;
            }
            else if (p2 != null)
            {
                p.next = p2;
            }

            return head.next;
        }


#if LATER
        function sort(
          keys:Key[], values: Value[],
          left:number, right: number, compare: Comparator<Key>,
        )
        {
            if (left >= right) return;

            var pivot = keys[(left + right) >> 1];
            var i = left - 1;
            var j = right + 1;

            while (true)
            {
                do i++; while (compare(keys[i], pivot) < 0);
                do j--; while (compare(keys[j], pivot) > 0);
                if (i >= j) break;

                var tmp = keys[i];
                keys[i] = keys[j];
                keys[j] = tmp;

                tmp = values[i];
                values[i] = values[j];
                values[j] = tmp;
            }

            sort(keys, values, left, j, compare);
            sort(keys, values, j + 1, right, compare);
        }
#endif
    }
}