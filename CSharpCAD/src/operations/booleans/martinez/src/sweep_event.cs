namespace CSharpCAD;

#nullable disable
internal static partial class Geom2Booleans
{
    internal class SweepEvent
    {


        /*
         * Sweepline event
         *
         * @class {SweepEvent}
         * @param {Array.<Number>}  point
         * @param {Boolean}         left
         * @param {SweepEvent=}     otherEvent
         * @param {Boolean}         isSubject
         * @param {Number}          edgeType
         */
        internal Vec2 point;
        internal bool left;
        internal SweepEvent otherEvent;
        internal bool isSubject;
        internal int type; // edgeType
        internal bool inOut; // In-out transition for the sweepline crossing polygon
        internal bool otherInOut; // Other in-out transition for the sweepline crossing polygon
        internal SweepEvent prevInResult; // Previous event in result?
        internal int resultTransition; // Type of result transition (0 = not in result, +1 = out-in, -1, in-out)
        internal int otherPos;
        internal int outputContourId;
        internal bool isExteriorRing;

        internal SweepEvent(Vec2 point, bool left, SweepEvent otherEvent, bool isSubject, int edgeType = NORMAL)
        {
            this.left = left;
            this.point = point;
            this.otherEvent = otherEvent;
            this.isSubject = isSubject;
            this.type = edgeType;
            this.inOut = false;
            this.otherInOut = false;
            this.prevInResult = null;
            this.resultTransition = 0;
            // connection step
            this.otherPos = -1;
            this.outputContourId = -1;
            this.isExteriorRing = true;
        }

        internal bool isBelow(Vec2 p)
        {
            var p0 = this.point;
            var p1 = this.otherEvent.point;
            return this.left
              ? (p0.X - p.X) * (p1.Y - p.Y) - (p1.X - p.X) * (p0.Y - p.Y) > 0
              // signedArea(this.point, this.otherEvent.point, p) > 0 :
              : (p1.X - p.X) * (p0.Y - p.Y) - (p0.X - p.X) * (p1.Y - p.Y) > 0;
            //signedArea(this.otherEvent.point, this.point, p) > 0;
        }

        internal bool isAbove(Vec2 p)
        {
            return !this.isBelow(p);
        }


        internal bool isVertical()
        {
            return this.point.X == this.otherEvent.point.X;
        }


        /*
         * Does event belong to result?
         */
        internal bool inResult()
        {
            return this.resultTransition != 0;
        }


        internal SweepEvent clone()
        {
            var copy = new SweepEvent(
              this.point, this.left, this.otherEvent, this.isSubject, this.type);

            copy.outputContourId = this.outputContourId;
            copy.resultTransition = this.resultTransition;
            copy.prevInResult = this.prevInResult;
            //copy.isExteriorRing = this.isExteriorRing;
            copy.inOut = this.inOut;
            copy.otherInOut = this.otherInOut;

            return copy;
        }
    }
}
