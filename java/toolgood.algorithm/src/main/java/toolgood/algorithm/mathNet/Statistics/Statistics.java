package toolgood.algorithm.mathNet.Statistics;

public class Statistics {
    public static double QuantileCustom(this IEnumerable<Double> data, double tau, QuantileDefinition definition)
    {
        double[] array = data.ToArray();
        return ArrayStatistics.QuantileCustomInplace(array, tau, definition);
    }
    public static double QuantileRank(this IEnumerable<Double> data, double x)
    {
        double[] array = data.ToArray();
        Array.Sort(array);
        return SortedArrayStatistics.QuantileRank(array, x);
    }

}