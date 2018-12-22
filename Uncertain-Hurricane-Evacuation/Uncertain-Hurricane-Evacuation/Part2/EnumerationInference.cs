using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uncertain_Hurricane_Evacuation.Part1;

namespace Uncertain_Hurricane_Evacuation.Part2
{
    class EnumerationInference
    {
        public static List<QueryResult> EnumerationAsk(BayesianNetwork network, List<Query> query, List<Evidence> evidences)
        {
            var result = new List<QueryResult>();
            foreach (var q in query)
            {
                result.Add(new QueryResult(q, EnumerateAll(network.Nodes, new List<Evidence>(evidences) {new Evidence(q.Node, q.Question)})));
            }
            
            return result;
        }

        public static double EnumerateAll(List<BayesianNode> vars, List<Evidence> evidences)
        {
            if (vars.Count == 0)
            {
                return 1.0;
            }

            var y = vars.First();
            vars.Remove(y);
            var tuple = BooleanTuple.Of(1, y.TupleSize);

            if (evidences.Any(e => e.Node == y))
            {
                var ev = evidences.First(e => e.Node == y);
                return y.Table[ev.Report ? tuple : tuple.Flip(0)] * EnumerateAll(vars, evidences);
            }
            else
            {
                return y.Table[tuple] * EnumerateAll(vars, new List<Evidence>(evidences) {new Evidence(y, true)}) + 
                       y.Table[tuple.Flip(0)] * EnumerateAll(vars, new List<Evidence>(evidences) { new Evidence(y, false) });
            }
        }
    }
}
