using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uncertain_Hurricane_Evacuation.GraphComponents
{
    interface IVertex
    {
        bool Flood { get; }
        bool Evacuate { get; }
        List<IEdge> Connectors { get; }
        List<IVertex> Neighbors { get; }
    }
}
