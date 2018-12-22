using System.Collections.Generic;

namespace Uncertain_Hurricane_Evacuation.GraphComponents
{
    public interface IVertex
    {
        int Id { get; }
        string Name { get; }
        List<IEdge> Connectors { get; }
        List<IVertex> Neighbors { get; }

        double FloodingProbability { get; }
    }
}
