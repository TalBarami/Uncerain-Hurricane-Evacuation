using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uncertain_Hurricane_Evacuation.GraphComponents;
using Uncertain_Hurricane_Evacuation.Part2;

namespace Uncertain_Hurricane_Evacuation.Part1
{
    public class BayesianNetwork
    {
        public IGraph Graph;
        public List<FloodingNode> FloodingNodes;
        public List<BlockageNode> BlockageNodes;
        public List<EvacueeNode> EvacueeNodes;

        public List<BayesianNode> Nodes => new List<BayesianNode>()
            .Concat(FloodingNodes)
            .Concat(BlockageNodes)
            .Concat(EvacueeNodes)
            .ToList();

        public FloodingNode FloodingNode(IVertex v)
        {
            return FloodingNodes.First(fn => fn.V == v);
        }

        public BlockageNode BlockageNode(IEdge e)
        {
            return BlockageNodes.First(bn => bn.E == e);
        }

        public EvacueeNode EvacueeNode(IVertex v)
        {
            return EvacueeNodes.First(en => en.V == v);
        }

        public BayesianNetwork(IGraph graph)
        {
            Graph = graph;
            FloodingNodes = Graph.Vertices.Select(v => new FloodingNode(v)).ToList();
            BlockageNodes = Graph.Edges.Select(e => new BlockageNode(e, FloodingNode(e.V1), FloodingNode(e.V2))).ToList();
            EvacueeNodes =
                Graph.Vertices.Select(v => 
                    new EvacueeNode(v, v.Connectors.Select(BlockageNode)
                        .ToList())).ToList();
        }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            var vertices = FloodingNodes.Select((node, i) => $"{node.TableToString()}\n{EvacueeNodes[i].TableToString()}");
            var edges = BlockageNodes.Select(bn => bn.TableToString());
            sb.AppendLine(string.Join("\n", vertices));
            sb.AppendLine(string.Join("\n", edges));

            return sb.ToString();
        }
    }
}
