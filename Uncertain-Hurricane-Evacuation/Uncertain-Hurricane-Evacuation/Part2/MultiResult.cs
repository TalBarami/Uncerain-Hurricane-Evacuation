using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uncertain_Hurricane_Evacuation.Part2
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
