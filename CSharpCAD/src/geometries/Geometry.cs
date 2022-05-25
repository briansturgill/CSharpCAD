namespace CSharpCAD;

/// <summary>Abstract class currently joining Geom2 and Geom3.</summary>
public abstract class Geometry
{
    /// <summary>Is this a 2D geometry object?</summary>
    public abstract bool Is2D { get; }

    /// <summary>Is this a 3D geometry object?</summary>
    public abstract bool Is3D { get; }

    /// <summary>Validate this geometry object.</summary>
    public abstract void Validate();
}