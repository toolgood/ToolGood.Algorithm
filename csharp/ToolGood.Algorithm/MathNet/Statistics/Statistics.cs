using System;
using System.Collections.Generic;
using System.Linq;


namespace ToolGood.Algorithm.MathNet.Numerics.Statistics
{
    /// <summary>
    /// 条件
    /// </summary>
    internal static class Statistics
    {
        public static decimal QuantileCustom(this IEnumerable<decimal> data, decimal tau, QuantileDefinition definition)
        {
			decimal[] array = data.ToArray();
            return ArrayStatistics.QuantileCustomInplace(array, tau, definition);
        }

        public static decimal QuantileRank(this IEnumerable<decimal> data, decimal x)
        {
			decimal[] array = data.ToArray();
            Array.Sort(array);
            return SortedArrayStatistics.QuantileRank(array, x);
        }
    }
}