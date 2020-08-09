package toolgood.algorithm.mathNet.Statistics;

public class Statistics {
    public static double QuantileCustom(final IEnumerable<Double> data, final double tau,
            final QuantileDefinition definition) {
        double[] array = data.ToArray();
        return ArrayStatistics.QuantileCustomInplace(array, tau, definition);
    }

    public static double QuantileRank(final IEnumerable<Double> data, final double x) {
        double[] array = data.ToArray();
        Array.sort(array);
        return SortedArrayStatistics.QuantileRank(array, x);
    }

}