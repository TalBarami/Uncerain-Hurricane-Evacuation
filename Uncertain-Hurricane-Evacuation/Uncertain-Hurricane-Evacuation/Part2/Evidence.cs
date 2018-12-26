using Uncertain_Hurricane_Evacuation.Part1;

namespace Uncertain_Hurricane_Evacuation.Part2
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
