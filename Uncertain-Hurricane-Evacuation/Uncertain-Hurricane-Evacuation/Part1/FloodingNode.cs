using Uncertain_Hurricane_Evacuation.GraphComponents;

namespace Uncertain_Hurricane_Evacuation.Part1
{
    public class FloodingNode : BayesianNode
    {
        protected override string Name { get; }

        public IVertex V;
        public FloodingNode(IVertex v)
        {
            V = v;
            Name = $"Flooding {V.Name}";

            Table.Add(NewTuple(), 1 - V.FloodProbability);
            BuildTable();
        }
    }
}
