namespace CSharpCAD;

public static partial class CSCAD
{
    ///<summary>Callback for use with ExtrudeFromSlices. For advanced users only.</summary>
    public delegate Slice? SliceGenerator(double progress, int index, Slice baseSlice);

    private static Slice? defaultCallback(double progress, int index, Slice baseSlice)
    {
        // It was dumb to be handling these in the callback...
        // baseSlice = new Slice((obj).ToSides());
        // baseSlice = new Slice(((Poly3)obj).ToPoints());

        return (progress == 0 || progress == 1) ? baseSlice.Transform(Mat4.FromTranslation(new Vec3(0, 0, progress))) : null;
    }

    /*
     * <summary>Extrude a solid from the slices as returned by the callback function.</summary>
     * @see slice
     *
     * @param {Object} options - options for extrude
     * @param {Integer} [options.numberOfSlices=2] the number of slices to be generated by the callback
     * @param {Boolean} [options.capStart=true] the solid should have a cap at the start
     * @param {Boolean} [options.capEnd=true] the solid should have a cap at the end
     * @param {Boolean} [options.close=false] the solid should have a closing section between start and end
     * @param {Function} [options.callback] the callback function that generates each slice
     * @param {Object} base - the base object which is used to create slices (see the example for callback information)
     * @return {geom3} the extruded shape
     * @alias module:modeling/extrusions.extrudeFromSlices
     *
     * @example
     * // Parameters:
     * //   progress : the percent complete [0..1]
     * //   index : the index of the current slice [0..numberOfSlices - 1]
     * //   base : the base object as given
     * // Return Value:
     * //   slice or null (to skip)
     * var callback = (progress, index, base) => {
     *   ...
     *   return slice
     * }
     */
    internal static Geom3 ExtrudeFromSlices(Slice baseSlice, SliceGenerator? _generate = null,
        int numberOfSlices = 2, bool capStart = true, bool capEnd = true, bool close = false, bool repair = true)
    {
        var generate = _generate ?? defaultCallback;

        if (numberOfSlices < 2) throw new ArgumentException("Option numberOfSlices must be 2 or more.");

        if (repair) {
            baseSlice.RepairSlice();
        }

        var sMax = numberOfSlices - 1;

        Slice? startSlice = null;
        Slice? endSlice = null;
        Slice? prevSlice = null;
        var polygons = new List<Poly3>();
        for (var s = 0; s < numberOfSlices; s++)
        {
            // invoke the callback function to get the next slice
            // NOTE: callback can return null to skip the slice
            var currentSlice = generate((double)s / (double)sMax, s, baseSlice);

            if (currentSlice is not null)
            {

                var edges = currentSlice.ToEdges();
                if (edges.Count == 0) throw new ArgumentException("The callback function must return slices with one or more edges");


                if (prevSlice is not null)
                {
                    polygons.AddRange(ExtrudeWalls(prevSlice, currentSlice));
                }

                // save start and end slices for caps if necessary
                if (s == 0) startSlice = currentSlice;
                if (s == (numberOfSlices - 1)) endSlice = currentSlice;


                prevSlice = currentSlice;
            }
        }

        if (capEnd && endSlice is not null)
        {
            // create a cap at the end
            var endPolygons = endSlice.ToPolygons();
            polygons.AddRange(endPolygons);
        }
        if (capStart && startSlice is not null)
        {
            // create a cap at the start
            var startPolygons = startSlice.ToPolygons().Select((poly) => poly.Invert());
            polygons.AddRange(startPolygons);
        }
        if (!capStart && !capEnd && endSlice is not null && startSlice is not null)
        {
            // create walls between end and start slices
            if (close && !(endSlice.IsNearlyEqual(startSlice)))
            {
                polygons.AddRange(ExtrudeWalls(endSlice, startSlice));
            }
        }
        return new Geom3(polygons.ToArray());
    }
}