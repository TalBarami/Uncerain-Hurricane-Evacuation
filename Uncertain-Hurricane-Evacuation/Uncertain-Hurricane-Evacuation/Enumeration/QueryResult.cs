namespace Uncertain_Hurricane_Evacuation.Enumeration
{
    class QueryResult : IQueryResult
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
