#nullable disable
namespace CSharpCAD.PolyboolDotNet;

internal class Segment
{
    internal Point End { get; set; }
    internal Point Start { get; set; }
    internal Fill MyFill { get; set; }
    internal Fill OtherFill { get; set; }
}