using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm.MathNet.Numerics.Statistics
{
    public enum RankDefinition
    {
        /// <summary>Replace ties with their mean (non-integer ranks). Default.</summary>
        Average = 1,
        Default = 1,

        /// <summary>Replace ties with their minimum (typical sports ranking).</summary>
        Min = 2,
        Sports = 2,

        /// <summary>Replace ties with their maximum.</summary>
        Max = 3,

        /// <summary>Permutation with increasing values at each index of ties.</summary>
        First = 4,

        EmpiricalCDF = 5
    }
}
