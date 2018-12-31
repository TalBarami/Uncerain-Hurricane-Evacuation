using Uncertain_Hurricane_Evacuation.BayesNetwork;

namespace Uncertain_Hurricane_Evacuation.Enumeration
{
    public class Query
    {
        public BayesianNode Node { get; }
        public bool Question { get; }

        public Query(BayesianNode node, bool question)
        {
            Node = node;
            Question = question;
        }

        public override string ToString()
        {
            var q = Question ? "" : "not ";
            return $"{q}{Node.Name}";
        }
    }
}
