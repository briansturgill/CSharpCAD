namespace CSharpCAD;

#nullable disable

internal static partial class Geom2Booleans
{
    internal static void computeFields(SweepEvent evt, SweepEvent prev, int operation)
    {
        // compute inOut and otherInOut fields
        if (prev is null)
        {
            evt.inOut = false;
            evt.otherInOut = true;

            // previous line segment in sweepline belongs to the same polygon
        }
        else
        {
            if (evt.isSubject == prev.isSubject)
            {
                evt.inOut = !prev.inOut;
                evt.otherInOut = prev.otherInOut;

                // previous line segment in sweepline belongs to the clipping polygon
            }
            else
            {
                evt.inOut = !prev.otherInOut;
                evt.otherInOut = prev.isVertical() ? !prev.inOut : prev.inOut;
            }

            // compute prevInResult field
            if (prev is not null)
            {
                evt.prevInResult = (!inResult(prev, operation) || prev.isVertical())
                  ? prev.prevInResult : prev;
            }
        }

        // check if the line segment belongs to the Boolean operation
        var isInResult = inResult(evt, operation);
        if (isInResult)
        {
            evt.resultTransition = determineResultTransition(evt, operation);
        }
        else
        {
            evt.resultTransition = 0;
        }
    }


    /* eslint-disable indent */
    internal static bool inResult(SweepEvent evt, int operation)
    {
        switch (evt.type)
        {
            case NORMAL:
                switch (operation)
                {
                    case INTERSECTION:
                        return !evt.otherInOut;
                    case UNION:
                        return evt.otherInOut;
                    case DIFFERENCE:
                        // return (evt.isSubject && !evt.otherInOut) ||
                        //         (!evt.isSubject && evt.otherInOut);
                        return (evt.isSubject && evt.otherInOut) ||
                                (!evt.isSubject && !evt.otherInOut);
                    case XOR:
                        return true;
                }
                break;
            case SAME_TRANSITION:
                return operation == INTERSECTION || operation == UNION;
            case DIFFERENT_TRANSITION:
                return operation == DIFFERENCE;
            case NON_CONTRIBUTING:
                return false;
        }
        return false;
    }


    internal static int determineResultTransition(SweepEvent evt, int operation)
    {
        var thisIn = !evt.inOut;
        var thatIn = !evt.otherInOut;

        bool isIn = false;
        switch (operation)
        {
            case INTERSECTION:
                isIn = thisIn && thatIn; break;
            case UNION:
                isIn = thisIn || thatIn; break;
            case XOR:
                isIn = thisIn ^ thatIn; break;
            case DIFFERENCE:
                if (evt.isSubject)
                {
                    isIn = thisIn && !thatIn;
                }
                else
                {
                    isIn = thatIn && !thisIn;
                }
                break;
        }
        return isIn ? +1 : -1;
    }
}