using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm.MathNet.Numerics.Statistics
{
    public static class Statistics
    {
        public static double QuantileCustom(this IEnumerable<double> data, double tau, QuantileDefinition definition)
        {
            double[] array = data.ToArray();
            return ArrayStatistics.QuantileCustomInplace(array, tau, definition);
        }
        public static double QuantileRank(this IEnumerable<double> data, double x, RankDefinition definition = RankDefinition.Default)
        {
            double[] array = data.ToArray();
            Array.Sort(array);
            return SortedArrayStatistics.QuantileRank(array, x, definition);
        }


    }
}
