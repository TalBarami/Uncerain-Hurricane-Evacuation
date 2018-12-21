namespace Uncertain_Hurricane_Evacuation.GraphComponents
{
    public interface IEdge
    {
        int Id { get; }
        string Name { get; }
        IVertex V1 { get; }
        IVertex V2 { get; }
        double Weight { get; }
        bool Blocked { get; }
    }
}
