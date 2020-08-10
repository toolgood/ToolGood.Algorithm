package toolgood.algorithm.mathNet.Statistics;

import java.awt.List;

public class Statistics {
    public static double QuantileCustom(final List<Double> data, final double tau,
            final QuantileDefinition definition) {
        double[] array = new double[data.size()];
        for (int i = 0; i < data.size(); i++) {
            array[i]=data.get(i);
        }
        return ArrayStatistics.QuantileCustomInplace(array, tau, definition);
    }

    public static double QuantileRank(final List<Double> data, final double x) {
        List<Double> dt=ShellSort(data);
        double[] array = new double[dt.size()];
        for (int i = 0; i < dt.size(); i++) {
            array[i]=dt.get(i);
        }
        return SortedArrayStatistics.QuantileRank(array, x);
    }

    private static List<Double> ShellSort(List<Double> array) {
        int len = array.size();
        double temp;
        int gap = len / 2;
        while (gap > 0) {
            for (int i = gap; i < len; i++) {
                temp = array.get(i);
                int preIndex = i - gap;
                while (preIndex >= 0 && array.get(preIndex) > temp) {
                    array.set(preIndex + gap, array.get(preIndex));
                    // array[preIndex + gap] = array[preIndex];
                    preIndex -= gap;
                }
                array.set(preIndex + gap, temp);
                // array[preIndex + gap] = temp;
            }
            gap /= 2;
        }
        return array;
    }
}