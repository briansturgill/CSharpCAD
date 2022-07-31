#nullable disable
namespace CSharpCAD.PolyboolDotNet;

internal class Segment
{
    internal Vec2 End { get; set; }
    internal Vec2 Start { get; set; }
    internal Fill MyFill { get; set; }
    internal Fill OtherFill { get; set; }
}