package toolgood.algorithm.mathNet.Statistics;

public class SortedArrayStatistics {
    public static double QuantileRank(double[] data, double x) {
        if (x < data[0]) {
            return 0.0;
        }

        if (x >= data[data.length - 1]) {
            return 1.0;
        }

        int right = binarySearch(data, x);
        if (right >= 0) {
            int left = right;

            while (left > 0 && data[left - 1] == data[left]) {
                left--;
            }

            while (right < data.length - 1 && data[right + 1] == data[right]) {
                right++;
            }

            return left / (double) (data.length - 1);

        } else {
            right = ~right;
            int left = right - 1;

            double a = left / (double) (data.length - 1);
            double b = right / (double) (data.length - 1);
            return ((data[right] - x) * a + (x - data[left]) * b) / (data[right] - data[left]);
        }
    }
    private static int binarySearch(double[] array, double key) {
        int low = 0;
        int high = array.length - 1;
        while (low <= high) {
            int mid = (low + high) >>> 1;
            if (key < array[mid]) {
                high = mid - 1;
            } else if (key > array[mid]) {
                low = mid + 1;
            } else {
                return mid;
            }
        }
        return -1;
    }
}