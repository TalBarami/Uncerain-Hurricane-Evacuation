using Uncertain_Hurricane_Evacuation.BayesNetwork;

namespace Uncertain_Hurricane_Evacuation.Enumeration
{
    public class Evidence
    {
        public BayesianNode Node { get; }
        public bool Report { get; }

        public Evidence(BayesianNode node, bool report)
        {
            Node = node;
            Report = report;
        }

        public override string ToString()
        {
            var s = Report ? "" : "not ";
            return $"{s}{Node.Name}";
        }
    }
}
