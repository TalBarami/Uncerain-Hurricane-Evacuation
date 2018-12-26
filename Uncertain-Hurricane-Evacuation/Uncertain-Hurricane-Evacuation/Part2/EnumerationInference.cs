using System.Collections.Generic;
using System.Linq;
using Uncertain_Hurricane_Evacuation.Part1;

namespace Uncertain_Hurricane_Evacuation.Part2
{
    class EnumerationInference
    {

        public static IQueryResult EnumerationAsk(BayesianNetwork network, List<Query> query,
            List<Evidence> evidences)
        {
            var pathEvidences = new List<Evidence>(evidences);
            var prod = 1.0;
            foreach (var q in query)
            {
                prod *= EnumerationAsk(network, q, pathEvidences).Result;
                if (pathEvidences.All(e => e.Node != q.Node))
                {
                    pathEvidences.Add(new Evidence(q.Node, false));
                }
            }

            return new MultiResult(query, prod);
        }

        public static IQueryResult EnumerationAsk(BayesianNetwork network, Query query, List<Evidence> evidences)
        {
            var ev = evidences.FirstOrDefault(e => e.Node == query.Node);
            if (ev != null)
            {
                return new QueryResult(query, ev.Report == query.Question ? 1.0 : 0.0);
            }

            var u1 = EnumerateAll(network.Nodes, Extend(evidences, query.Node, query.Question));
            var u2 = EnumerateAll(network.Nodes, Extend(evidences, query.Node, !query.Question));
            return new QueryResult(query, u1 / (u1 + u2));
        }

        public static double EnumerateAll(List<BayesianNode> vars, List<Evidence> evidences)
        {
            if (vars.Count == 0)
            {
                return 1.0;
            }

            var y = vars.First();
            var tuple = BooleanTuple.Of(0, y.TupleSize).Flip(0);
            for (var i = 0; i < y.Parents.Count; i++)
            {
                var p = y.Parents[i];
                var e = evidences.First(evidence => evidence.Node == p);
                if (e.Report)
                {
                    tuple = tuple.Flip(i + 1);
                }
            }

            var ev = evidences.FirstOrDefault(e => e.Node == y);
            var probability = y.Table[tuple];
            if (ev != null)
            {
                return (ev.Report ? probability : 1 - probability) * EnumerateAll(Rest(vars, y), evidences);
            }

            return probability * EnumerateAll(Rest(vars, y), Extend(evidences, y, true)) + 
                   (1 - probability) * EnumerateAll(Rest(vars, y), Extend(evidences, y, false));
        }

        public static List<Evidence> Extend(List<Evidence> list, BayesianNode node, bool value)
        {
            return new List<Evidence>(list){new Evidence(node, value)};
        }

        public static List<BayesianNode> Rest(List<BayesianNode> list, BayesianNode first)
        {
            return list.Where(n => n != first).ToList();
        }
    }
}
