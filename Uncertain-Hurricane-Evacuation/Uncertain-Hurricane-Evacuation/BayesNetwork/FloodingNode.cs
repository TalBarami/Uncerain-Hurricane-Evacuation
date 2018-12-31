using Uncertain_Hurricane_Evacuation.GraphComponents;

namespace Uncertain_Hurricane_Evacuation.BayesNetwork
{
    public class FloodingNode : BayesianNode
    {
        public override string Name { get; }

        public IVertex V;
        public FloodingNode(IVertex v)
        {
            V = v;
            Name = $"Flooding {V.Name}";

            Table.Add(NewTuple(), 1 - V.FloodingProbability);
            BuildTable();
        }
    }
}
