namespace CSharpCAD;

#nullable disable

internal static partial class Geom2Booleans
{

    /*
        This file contains a specialized version for this algorithm, translated to C#.
        You can find a more general version (in JavaScript) at:
        https://github.com/mourner/tinyqueue

        LICENSE:

        ISC License

        Copyright (c) 2017, Vladimir Agafonkin

        Permission to use, copy, modify, and/or distribute this software for any purpose
        with or without fee is hereby granted, provided that the above copyright notice
        and this permission notice appear in all copies.

        THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES WITH
        REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF MERCHANTABILITY AND
        FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY SPECIAL, DIRECT,
        INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES WHATSOEVER RESULTING FROM LOSS
        OF USE, DATA OR PROFITS, WHETHER IN AN ACTION OF CONTRACT, NEGLIGENCE OR OTHER
        TORTIOUS ACTION, ARISING OUT OF OR IN CONNECTION WITH THE USE OR PERFORMANCE OF
        THIS SOFTWARE.
    */

    internal class TinyQueue
    {
        internal int length { get => this.data.Count; }
        internal List<SweepEvent> data;
        internal TinyQueue()
        {
            this.data = new List<SweepEvent>();
        }

        internal void push(SweepEvent item)
        {
            this.data.Add(item);
            this._up(this.data.Count - 1);
        }

        internal SweepEvent pop()
        {
            if (this.data.Count == 0) return null;

            var top = this.data[0];
            var end = this.data.Count - 1;
            var bottom = this.data[end];
            this.data.RemoveAt(end); // Pop

            if (this.data.Count > 0)
            {
                this.data[0] = bottom;
                this._down(0);
            }

            return top;
        }

        internal SweepEvent peek()
        {
            return this.data[0];
        }

        internal void _up(int pos)
        {
            var item = data[pos];

            while (pos > 0)
            {
                var parent = (pos - 1) >> 1;
                var current = data[parent];
                if (compareEvents(item, current) >= 0) break;
                data[pos] = current;
                pos = parent;
            }

            data[pos] = item;
        }

        internal void _down(int pos)
        {
            var halfLength = this.data.Count >> 1;
            var item = data[pos];

            while (pos < halfLength)
            {
                var bestChild = (pos << 1) + 1; // initially it is the left child
                var right = bestChild + 1;

                if (right < this.data.Count && compareEvents(data[right], data[bestChild]) < 0)
                {
                    bestChild = right;
                }
                if (compareEvents(data[bestChild], item) >= 0) break;

                data[pos] = data[bestChild];
                pos = bestChild;
            }

            data[pos] = item;
        }
    }
}