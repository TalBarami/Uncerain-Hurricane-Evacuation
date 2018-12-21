using System;
using System.Collections.Generic;
using System.Linq;
using Uncertain_Hurricane_Evacuation.Environment;
using Uncertain_Hurricane_Evacuation.GraphComponents;

namespace Uncertain_Hurricane_Evacuation.Part1
{
    public class EvacueeNode : BayesianNode
    {
        public IVertex V;
        public List<BlockageNode> BlockageNodes;

        protected override string Name { get; }

        public EvacueeNode(IVertex v, List<BlockageNode> blockageNodes)
        {
            V = v;
            Name = $"Evacuee {V.Name}";
            BlockageNodes = new List<BlockageNode>(blockageNodes);
            Parents.AddRange(blockageNodes);

            for (var i = 0; i < BlockageNodes.Count; i++)
            {
                var tuple = NewTuple().Flip(i+1);
                var prob = Constants.EvacuationProbability * (BlockageNodes[i].E.Weight > Constants.EvacuationEdgeWeight
                               ? Constants.EvacuationMultiplier
                               : 1);
                Table.Add(tuple, 1 - prob);
            }

            BuildTable();
        }
    }
}
