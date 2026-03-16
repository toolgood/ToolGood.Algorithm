package toolgood.algorithm.mathNet.Statistics;

import java.math.BigDecimal;
import java.math.MathContext;

public class SortedArrayStatistics {
    public static BigDecimal QuantileRank(BigDecimal[] data, BigDecimal x) {
        if (x.compareTo(data[0]) < 0) {
            return BigDecimal.ZERO;
        }

        if (x.compareTo(data[data.length - 1]) >= 0) {
            return BigDecimal.ONE;
        }

        int right = binarySearch(data, x);
        if (right >= 0) {
            int left = right;

            while (left > 0 && data[left - 1].compareTo(data[left]) == 0) {
                left--;
            }

            while (right < data.length - 1 && data[right + 1].compareTo(data[right]) == 0) {
                right++;
            }

            return new BigDecimal(left).divide(new BigDecimal(data.length - 1), MathContext.DECIMAL128);

        } else {
            right = ~right;
            int left = right - 1;

            BigDecimal a = new BigDecimal(left).divide(new BigDecimal(data.length - 1), MathContext.DECIMAL128);
            BigDecimal b = new BigDecimal(right).divide(new BigDecimal(data.length - 1), MathContext.DECIMAL128);
            return data[right].subtract(x).multiply(a).add(x.subtract(data[left]).multiply(b))
                    .divide(data[right].subtract(data[left]), MathContext.DECIMAL128);
        }
    }

    private static int binarySearch(BigDecimal[] array, BigDecimal key) {
        int low = 0;
        int high = array.length - 1;
        while (low <= high) {
            int mid = (low + high) >>> 1;
            if (key.compareTo(array[mid]) < 0) {
                high = mid - 1;
            } else if (key.compareTo(array[mid]) > 0) {
                low = mid + 1;
            } else {
                return mid;
            }
        }
        return -1;
    }
}
