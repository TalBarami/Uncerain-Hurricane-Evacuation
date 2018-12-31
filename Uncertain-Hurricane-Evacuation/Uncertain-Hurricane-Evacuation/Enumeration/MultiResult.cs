using System.Collections.Generic;

namespace Uncertain_Hurricane_Evacuation.Enumeration
{
    class MultiResult : IQueryResult
    {
        public List<Query> Queries { get; }
        public double Result { get; }

        public MultiResult(List<Query> queries, double result)
        {
            Queries = queries;
            Result = result;
        }

        public override string ToString()
        {
            return $"{string.Join(", ", Queries)} with probability of {Result}";
        }
    }
}
