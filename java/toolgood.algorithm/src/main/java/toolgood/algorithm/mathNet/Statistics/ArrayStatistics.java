package toolgood.algorithm.mathNet.Statistics;

public class ArrayStatistics {
    public static double Minimum(double[] data)
    {
        if (data.length == 0) {
            return Double.NaN;
        }

        double min = Double.POSITIVE_INFINITY;
        for (int i = 0; i < data.length; i++) {
            if (data[i] < min || Double.isNaN(data[i])) {
                min = data[i];
            }
        }

        return min;
    }
    public static double Maximum(double[] data)
    {
        if (data.length == 0) {
            return Double.NaN;
        }

        double max = Double.NEGATIVE_INFINITY;
        for (int i = 0; i < data.length; i++) {
            if (data[i] > max || Double.isNaN(data[i])) {
                max = data[i];
            }
        }

        return max;
    }


    public static double QuantileCustomInplace(double[] data, double tau, QuantileDefinition definition)
            throws Exception
    {
        if (tau < 0d || tau > 1d || data.length == 0) {
            return Double.NaN;
        }

        if (tau == 0d || data.length == 1) {
            return Minimum(data);
        }

        if (tau == 1d) {
            return Maximum(data);
        }

        switch (definition) {
            case R1: {
                    double h = data.length * tau + 0.5d;
                    return SelectInplace(data, (int)Math.ceil(h - 0.5d) - 1);
                }

            case R2: {
                    double h = data.length * tau + 0.5d;
                    return (SelectInplace(data, (int)Math.ceil(h - 0.5d) - 1) + SelectInplace(data, (int)(h + 0.5d) - 1)) * 0.5d;
                }

            case R3: {
                    double h = data.length * tau;
                    return SelectInplace(data, (int)Math.round(h) - 1);
                }

            case R4: {
                    double h = data.length * tau;
                    int hf = (int)h;
                    double lower = SelectInplace(data, hf - 1);
                    double upper = SelectInplace(data, hf);
                    return lower + (h - hf) * (upper - lower);
                }

            case R5: {
                    double h = data.length * tau + 0.5d;
                    int hf = (int)h;
                    double lower = SelectInplace(data, hf - 1);
                    double upper = SelectInplace(data, hf);
                    return lower + (h - hf) * (upper - lower);
                }

            case R6: {
                    double h = (data.length + 1) * tau;
                    int hf = (int)h;
                    double lower = SelectInplace(data, hf - 1);
                    double upper = SelectInplace(data, hf);
                    return lower + (h - hf) * (upper - lower);
                }

            case R7: {
                    double h = (data.length - 1) * tau + 1d;
                    int hf = (int)h;
                    double lower = SelectInplace(data, hf - 1);
                    double upper = SelectInplace(data, hf);
                    return lower + (h - hf) * (upper - lower);
                }

            case R8: {
                    double h = (data.length + 1 / 3d) * tau + 1 / 3d;
                    int hf = (int)h;
                    double lower = SelectInplace(data, hf - 1);
                    double upper = SelectInplace(data, hf);
                    return lower + (h - hf) * (upper - lower);
                }

            case R9: {
                    double h = (data.length + 0.25d) * tau + 0.375d;
                    int hf = (int)h;
                    double lower = SelectInplace(data, hf - 1);
                    double upper = SelectInplace(data, hf);
                    return lower + (h - hf) * (upper - lower);
                }

            default:
                throw new Exception();
        }
    }

    static double SelectInplace(double[] workingData, int rank)
    {
        // Numerical Recipes: select
        // http://en.wikipedia.org/wiki/Selection_algorithm
        if (rank <= 0) {
            return Minimum(workingData);
        }

        if (rank >= workingData.length - 1) {
            return Maximum(workingData);
        }

        double[] a = workingData;
        int low = 0;
        int high = a.length - 1;

        while (true) {
            if (high <= low + 1) {
                if (high == low + 1 && a[high] < a[low]) {
                    double tmp = a[low];
                    a[low] = a[high];
                    a[high] = tmp;
                }

                return a[rank];
            }

            int middle = (low + high) >> 1;

            double tmp1 = a[middle];
            a[middle] = a[low + 1];
            a[low + 1] = tmp1;

            if (a[low] > a[high]) {
                double tmp = a[low];
                a[low] = a[high];
                a[high] = tmp;
            }

            if (a[low + 1] > a[high]) {
                double tmp = a[low + 1];
                a[low + 1] = a[high];
                a[high] = tmp;
            }

            if (a[low] > a[low + 1]) {
                double tmp = a[low];
                a[low] = a[low + 1];
                a[low + 1] = tmp;
            }

            int begin = low + 1;
            int end = high;
            double pivot = a[begin];

            while (true) {
                do {
                    begin++;
                }
                while (a[begin] < pivot);

                do {
                    end--;
                }
                while (a[end] > pivot);

                if (end < begin) {
                    break;
                }

                double tmp = a[begin];
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