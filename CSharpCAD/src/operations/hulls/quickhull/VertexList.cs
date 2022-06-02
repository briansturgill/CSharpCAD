namespace CSharpCAD;

public static partial class CSCAD
{

    /*
     * Original source from quickhull3d (https://github.com/mauriciopoppe/quickhull3d)
     * Copyright (c) 2015 Mauricio Poppe
     *
     * Adapted to JSCAD by Jeff Gay
     */

    internal class QHVertexList
    {
        internal QHVertex? head;
        internal QHVertex? tail;
        internal QHVertexList()
        {
            this.head = null;
            this.tail = null;
        }

        internal void Clear()
        {
            this.head = this.tail = null;
        }

        /**
         * Inserts a `node` before `target`, it's assumed that
         * `target` belongs to this doubly linked list
         *
         * @param {*} target
         * @param {*} node
         */
        internal void InsertBefore(QHVertex target, QHVertex node)
        {
            node.prev = target.prev;
            node.next = target;
            if (node.prev is null)
            {
                this.head = node;
            }
            else
            {
                node.prev.next = node;
            }
            target.prev = node;
        }

        /**
         * Inserts a `node` after `target`, it's assumed that
         * `target` belongs to this doubly linked list
         *
         * @param {Vertex} target
         * @param {Vertex} node
         */
        internal void InsertAfter(QHVertex target, QHVertex node)
        {
            node.prev = target;
            node.next = target.next;
            if (node.next is null)
            {
                this.tail = node;
            }
            else
            {
                node.next.prev = node;
            }
            target.next = node;
        }

        /**
         * Appends a `node` to the end of this doubly linked list
         * Note: `node.next` will be unlinked from `node`
         * Note: if `node` is part of another linked list call `addAll` instead
         *
         * @param {*} node
         */
        internal void Add(QHVertex node)
        {
            if (this.head is null)
            {
                this.head = node;
            }
            else
            {
                if (this.tail is not null) this.tail.next = node;
            }
            node.prev = this.tail;
            // since node is the new end it doesn't have a next node
            node.next = null;
            this.tail = node;
        }

        /**
         * Appends a chain of nodes where `node` is the head,
         * the difference with `add` is that it correctly sets the position
         * of the node list `tail` property
         *
         * @param {*} node
         */
        internal void AddAll(QHVertex node)
        {
            if (this.head is null)
            {
                this.head = node;
            }
            else
            {
                if (this.tail is not null) this.tail.next = node;
            }
            node.prev = this.tail;

            // find the end of the list
            while (node.next is not null)
            {
                node = node.next;
            }
            this.tail = node;
        }

        /**
         * Deletes a `node` from this linked list, it's assumed that `node` is a
         * member of this linked list
         *
         * @param {*} node
         */
        internal void Remove(QHVertex node)
        {
            if (node.prev is null)
            {
                this.head = node.next;
            }
            else
            {
                node.prev.next = node.next;
            }

            if (node.next is null)
            {
                this.tail = node.prev;
            }
            else
            {
                node.next.prev = node.prev;
            }
        }

        /**
         * Removes a chain of nodes whose head is `a` and whose tail is `b`,
         * it's assumed that `a` and `b` belong to this list and also that `a`
         * comes before `b` in the linked list
         *
         * @param {*} a
         * @param {*} b
         */
        internal void RemoveChain(QHVertex a, QHVertex b)
        {
            if (a.prev is null)
            {
                this.head = b.next;
            }
            else
            {
                a.prev.next = b.next;
            }

            if (b.next is null)
            {
                this.tail = a.prev;
            }
            else
            {
                b.next.prev = a.prev;
            }
        }

        internal QHVertex? First()
        {
            return this.head;
        }

        internal bool IsEmpty()
        {
            return this.head is null;
        }
    }
}