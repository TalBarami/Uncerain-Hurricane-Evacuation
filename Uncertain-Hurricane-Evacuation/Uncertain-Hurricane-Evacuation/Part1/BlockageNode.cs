using System;
using System.Linq;
using Uncertain_Hurricane_Evacuation.Environment;
using Uncertain_Hurricane_Evacuation.GraphComponents;

namespace Uncertain_Hurricane_Evacuation.Part1
{
    public class BlockageNode : BayesianNode
    {
        public IEdge E;
        public FloodingNode V1;
        public FloodingNode V2;
        protected override string Name { get; }


        public BlockageNode(IEdge e, FloodingNode v1, FloodingNode v2)
        {
            E = e;
            V1 = v1;
            V2 = v2;
            Name = $"Blockage {E.Name}";

            Parents.Add(V1);
            Parents.Add(V2);

            var prob = 1 - Constants.BlockageProbability * (1 / E.Weight);

            Table.Add(NewTuple(1), prob);
            Table.Add(NewTuple(2), prob);

            BuildTable();
        }
    }
}
