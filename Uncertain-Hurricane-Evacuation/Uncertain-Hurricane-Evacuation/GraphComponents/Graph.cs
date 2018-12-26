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

        public List<Path> Dfs(IVertex src, IVertex dst)
        {
            if (src.Equals(dst))
            {
                return new List<Path> { new Path(this, new List<IEdge>{new Edge(-1, src, src, 0)}) };
            }
            var lst = new List<Path>();
            var queue = new Queue<QueueItem>();
            queue.Enqueue(new QueueItem(src, new List<IEdge>()));
            while (queue.Count > 0)
            {
                var currentItem = queue.Dequeue();
                foreach (var edge in currentItem.Node.Connectors)
                {
                    if (!currentItem.Visited.Contains(edge))
                    {
                        var visited = new List<IEdge>(currentItem.Visited) {edge};
                        if (edge.V2 == dst)
                        {
                            lst.Add(new Path(this, visited));
                        }
                        else
                        {
                            queue.Enqueue(new QueueItem(edge.V2, visited));
                        }
                    }
                }
            }

            return lst;
        }

        internal class QueueItem
        {

            public IVertex Node { get; }
            public List<IEdge> Visited { get; }

            public QueueItem(IVertex node, List<IEdge> visited)
            {
                Node = node;
                Visited = visited;
            }

        }
    }
}
