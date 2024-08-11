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
        public static double QuantileCustom(this IEnumerable<double> data, double tau, QuantileDefinition definition)
        {
            double[] array = data.ToArray();
            return ArrayStatistics.QuantileCustomInplace(array, tau, definition);
        }

        public static double QuantileRank(this IEnumerable<double> data, double x)
        {
            double[] array = data.ToArray();
            Array.Sort(array);
            return SortedArrayStatistics.QuantileRank(array, x);
        }
    }
}