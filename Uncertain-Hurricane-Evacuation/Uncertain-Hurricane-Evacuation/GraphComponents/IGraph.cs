using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uncertain_Hurricane_Evacuation.GraphComponents
{
    interface IGraph
    {
        List<IVertex> Vertices { get; }
        List<IEdge> Edges { get; }
    }
}
