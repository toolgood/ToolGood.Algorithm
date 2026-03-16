package toolgood.algorithm.mathNet.Statistics;

import java.math.BigDecimal;
import java.math.MathContext;

public class ArrayStatistics {
    private static final BigDecimal MIN_VALUE = new BigDecimal("-79228162514264337593543950335");

    public static BigDecimal Minimum(BigDecimal[] data) {
        if (data.length == 0) {
            return MIN_VALUE;
        }

        BigDecimal min = MIN_VALUE.negate();
        for (int i = 0; i < data.length; i++) {
            if (data[i].compareTo(min) < 0) {
                min = data[i];
            }
        }

        return min;
    }

    public static BigDecimal Maximum(BigDecimal[] data) {
        if (data.length == 0) {
            return MIN_VALUE;
        }

        BigDecimal max = MIN_VALUE;
        for (int i = 0; i < data.length; i++) {
            if (data[i].compareTo(max) > 0) {
                max = data[i];
            }
        }

        return max;
    }

    public static BigDecimal QuantileCustomInplace(BigDecimal[] data, BigDecimal tau) {
        if (tau.compareTo(BigDecimal.ZERO) < 0 || tau.compareTo(BigDecimal.ONE) > 0 || data.length == 0) {
            return MIN_VALUE;
        }

        if (tau.compareTo(BigDecimal.ZERO) == 0 || data.length == 1) {
            return Minimum(data);
        }

        if (tau.compareTo(BigDecimal.ONE) == 0) {
            return Maximum(data);
        }

        BigDecimal h = new BigDecimal(data.length - 1).multiply(tau).add(BigDecimal.ONE);
        int hf = (int) h.doubleValue();
        BigDecimal lower = SelectInplace(data.clone(), hf - 1);
        BigDecimal upper = SelectInplace(data.clone(), hf);
        return lower.add(h.subtract(new BigDecimal(hf)).multiply(upper.subtract(lower)));
    }

    private static BigDecimal SelectInplace(BigDecimal[] workingData, int rank) {
        if (rank <= 0) {
            return Minimum(workingData);
        }

        if (rank >= workingData.length - 1) {
            return Maximum(workingData);
        }

        BigDecimal[] a = workingData;
        int low = 0;
        int high = a.length - 1;

        while (true) {
            if (high <= low + 1) {
                if (high == low + 1 && a[high].compareTo(a[low]) < 0) {
                    BigDecimal tmp = a[low];
                    a[low] = a[high];
                    a[high] = tmp;
                }

                return a[rank];
            }

            int middle = (low + high) >> 1;

            BigDecimal tmp1 = a[middle];
            a[middle] = a[low + 1];
            a[low + 1] = tmp1;

            if (a[low].compareTo(a[high]) > 0) {
                BigDecimal tmp = a[low];
                a[low] = a[high];
                a[high] = tmp;
            }

            if (a[low + 1].compareTo(a[high]) > 0) {
                BigDecimal tmp = a[low + 1];
                a[low + 1] = a[high];
                a[high] = tmp;
            }

            if (a[low].compareTo(a[low + 1]) > 0) {
                BigDecimal tmp = a[low];
                a[low] = a[low + 1];
                a[low + 1] = tmp;
            }

            int begin = low + 1;
            int end = high;
            BigDecimal pivot = a[begin];

            while (true) {
                do {
                    begin++;
                }
                while (a[begin].compareTo(pivot) < 0);

                do {
                    end--;
                }
                while (a[end].compareTo(pivot) > 0);

                if (end < begin) {
                    break;
                }

                BigDecimal tmp = a[begin];
                a[begin] = a[end];
                a[end] = tmp;
            }

            a[low + 1] = a[end];
            a[end] = pivot;

            if (end >= rank) {
                high = end - 1;
            }

            if (end <= rank) {
                low = begin;
            }
        }
    }
}
