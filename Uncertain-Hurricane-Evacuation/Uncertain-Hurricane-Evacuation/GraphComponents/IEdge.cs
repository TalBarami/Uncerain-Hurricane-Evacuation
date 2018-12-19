using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uncertain_Hurricane_Evacuation.GraphComponents
{
    interface IEdge
    {
        IVertex V1 { get; }
        IVertex V2 { get; }
        double Weight { get; }
        bool Blocked { get; }
    }
}
