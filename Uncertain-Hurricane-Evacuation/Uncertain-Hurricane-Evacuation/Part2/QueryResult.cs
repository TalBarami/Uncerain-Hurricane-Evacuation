using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uncertain_Hurricane_Evacuation.Part2
{
    class QueryResult
    {
        public Query Query { get; }
        public double Result { get; }

        public QueryResult(Query query, double result)
        {
            Query = query;
            Result = result;
        }

        public override string ToString()
        {
            return $"{Query} with probability of {Result}";
        }
    }
}
