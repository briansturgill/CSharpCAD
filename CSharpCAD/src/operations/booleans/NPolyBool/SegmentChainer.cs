using System.Collections.Generic;
using System.Linq;

#nullable disable
#pragma warning disable CS1591

namespace CSharpCAD.PolyBool
{
    internal static class SegmentChainer
    {
        private class Matcher
        {
            public int Index { get; set; }
            public bool MatchesHead { get; set; }
            public bool MatchesPt1 { get; set; }
        }

        public static List<Region> Chain(List<Segment> segments, Epsilon eps)
        {
            var chains = new List<List<Vec2>>();
            var regions = new List<Region>();

            foreach (Segment seg in segments)
            {
                var pt1 = seg.Start;
                var pt2 = seg.End;
                if (eps.PointsSame(pt1, pt2))
                    continue;

                // search for two chains that this segment matches
                var firstMatch = new Matcher
                {
                    Index = 0,
                    MatchesHead = false,
                    MatchesPt1 = false
                };
                var secondMatch = new Matcher
                {
                    Index = 0,
                    MatchesHead = false,
                    MatchesPt1 = false
                };
                var nextMatch = firstMatch;

                bool setMatch(int index, bool matchesHead, bool matchesPt1)
                {
                    // return true if we've matched twice
                    nextMatch.Index = index;
                    nextMatch.MatchesHead = matchesHead;
                    nextMatch.MatchesPt1 = matchesPt1;
                    if (ReferenceEquals(nextMatch, firstMatch))
                    {
                        nextMatch = secondMatch;
                        return false;
                    }
                    nextMatch = null;
                    return true; // we've matched twice, we're done here
                }

                for (var i = 0; i < chains.Count; i++)
                {
                    var chain = chains[i];
                    var head = chain[0];
                    var tail = chain[chain.Count - 1];
                    if (eps.PointsSame(head, pt1))
                    {
                        if (setMatch(i, true, true))
                            break;
                    }
                    else if (eps.PointsSame(head, pt2))
                    {
                        if (setMatch(i, true, false))
                            break;
                    }
                    else if (eps.PointsSame(tail, pt1))
                    {
                        if (setMatch(i, false, true))
                            break;
                    }
                    else if (eps.PointsSame(tail, pt2))
                    {
                        if (setMatch(i, false, false))
                            break;
                    }
                }

                if (ReferenceEquals(nextMatch, firstMatch))
                {
                    // we didn't match anything, so create a new chain
                    chains.Push(new List<Vec2> { pt1, pt2 });
                    continue;
                }

                if (ReferenceEquals(nextMatch, secondMatch))
                {
                    // we matched a single chain

                    // add the other point to the apporpriate end, and check to see if we've closed the
                    // chain into a loop

                    var index = firstMatch.Index;
                    var pt = firstMatch.MatchesPt1 ? pt2 : pt1; // if we matched pt1, then we add pt2, etc
                    var addToHead = firstMatch.MatchesHead; // if we matched at head, then add to the head

                    var chain = chains[index];
                    var grow = addToHead ? chain[0] : chain[chain.Count - 1];
                    var grow2 = addToHead ? chain[1] : chain[chain.Count - 2];
                    var oppo = addToHead ? chain[chain.Count - 1] : chain[0];
                    var oppo2 = addToHead ? chain[chain.Count - 2] : chain[1];

                    if (eps.PointsCollinear(grow2, grow, pt))
                    {
                        // grow isn't needed because it's directly between grow2 and pt:
                        // grow2 ---grow---> pt
                        if (addToHead)
                        {
                            chain.Shift();
                        }
                        else
                        {
                            chain.Pop();
                        }
                        grow = grow2; // old grow is gone... new grow is what grow2 was
                    }

                    if (eps.PointsSame(oppo, pt))
                    {
                        // we're closing the loop, so remove chain from chains
                        chains.Splice(index, 1);

                        if (eps.PointsCollinear(oppo2, oppo, grow))
                        {
                            // oppo isn't needed because it's directly between oppo2 and grow:
                            // oppo2 ---oppo--->grow
                            if (addToHead)
                            {
                                chain.Pop();
                            }
                            else
                            {
                                chain.Shift();
                            }
                        }

                        // we have a closed chain!
                        regions.Push(new Region(chain.ToArray()));
                        continue;
                    }

                    // not closing a loop, so just add it to the apporpriate side
                    if (addToHead)
                    {
                        chain.Unshift(pt);
                    }
                    else
                    {
                        chain.Push(pt);
                    }
                    continue;
                }

                // otherwise, we matched two chains, so we need to combine those chains together

                void reverseChain(int index)
                {
                    chains[index].Reverse(); // gee, that's easy
                }

                void appendChain(int index1, int index2)
                {
                    // index1 gets index2 appended to it, and index2 is removed
                    List<Vec2> chain1 = chains[index1];
                    List<Vec2> chain2 = chains[index2];
                    Vec2 tail = chain1[chain1.Count - 1];
                    Vec2 tail2 = chain1[chain1.Count - 2];
                    Vec2 head = chain2[0];
                    Vec2 head2 = chain2[1];

                    if (eps.PointsCollinear(tail2, tail, head))
                    {
                        // tail isn't needed because it's directly between tail2 and head
                        // tail2 ---tail---> head
                        chain1.Pop();
                        tail = tail2; // old tail is gone... new tail is what tail2 was
                    }

                    if (eps.PointsCollinear(tail, head, head2))
                    {
                        // head isn't needed because it's directly between tail and head2
                        // tail ---head---> head2
                        chain2.Shift();
                    }
                    chains[index1] = chain1.Concat(chain2).ToList();
                    chains.Splice(index2, 1);
                }

                var F = firstMatch.Index;
                var S = secondMatch.Index;

                var reverseF = chains[F].Count < chains[S].Count; // reverse the shorter chain, if needed
                if (firstMatch.MatchesHead)
                {
                    if (secondMatch.MatchesHead)
                    {
                        if (reverseF)
                        {
                            // <<<< F <<<< --- >>>> S >>>>
                            reverseChain(F);
                            // >>>> F >>>> --- >>>> S >>>>
                            appendChain(F, S);
                        }
                        else
                        {
                            // <<<< F <<<< --- >>>> S >>>>
                            reverseChain(S);
                            // <<<< F <<<< --- <<<< S <<<<   logically same as:
                            // >>>> S >>>> --- >>>> F >>>>
                            appendChain(S, F);
                        }
                    }
                    else
                    {
                        // <<<< F <<<< --- <<<< S <<<<   logically same as:
                        // >>>> S >>>> --- >>>> F >>>>
                        appendChain(S, F);
                    }
                }
                else
                {
                    if (secondMatch.MatchesHead)
                    {
                        // >>>> F >>>> --- >>>> S >>>>
                        appendChain(F, S);
                    }
                    else
                    {
                        if (reverseF)
                        {
                            // >>>> F >>>> --- <<<< S <<<<
                            reverseChain(F);
                            // <<<< F <<<< --- <<<< S <<<<   logically same as:
                            // >>>> S >>>> --- >>>> F >>>>
                            appendChain(S, F);
                        }
                        else
                        {
                            // >>>> F >>>> --- <<<< S <<<<
                            reverseChain(S);
                            // >>>> F >>>> --- >>>> S >>>>
                            appendChain(F, S);
                        }
                    }
                }
            }

            return regions;
        }
    }
}
