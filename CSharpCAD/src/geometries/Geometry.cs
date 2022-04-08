namespace CSharpCAD;

public abstract class Geometry
{
    public abstract bool Is2D { get; }
    public abstract bool Is3D { get; }
}