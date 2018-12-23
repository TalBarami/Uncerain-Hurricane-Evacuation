using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uncertain_Hurricane_Evacuation.GraphComponents
{
    class Graph : IGraph
    {
        public List<IVertex> Vertices { get; }
        public List<IEdge> Edges { get; }
        public IVertex Vertex(int Id)
        {
            return Vertices[Id - 1];
        }

        public IEdge Edge(int Id)
        {
            return Edges[Id - 1];
        }

        public Graph(List<IVertex> vertices, List<IEdge> edges)
        {
            Vertices = vertices;
            Edges = edges;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Vertices: {string.Join(", ", Vertices)}");
            sb.AppendLine($"Edges: {string.Join(", ", Edges)}");
            return sb.ToString();
        }
    }
}
