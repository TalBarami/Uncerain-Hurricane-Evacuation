using System.Collections.Generic;

namespace Uncertain_Hurricane_Evacuation.GraphComponents
{
    public interface IGraph
    {
        List<IVertex> Vertices { get; }
        List<IEdge> Edges { get; }

        IVertex Vertex(int Id);

        IEdge Edge(int Id);

        List<Path> Dfs(IVertex src, IVertex dst);
    }
}
