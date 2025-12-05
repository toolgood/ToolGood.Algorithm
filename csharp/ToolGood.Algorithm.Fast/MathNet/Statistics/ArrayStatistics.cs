using System;

namespace ToolGood.Algorithm.MathNet.Numerics.Statistics
{
    internal static partial class ArrayStatistics
    {
        public static double Minimum(double[] data)
        {
            if (data.Length == 0) {
                return double.NaN;
            }

            double min = double.PositiveInfinity;
            for (int i = 0; i < data.Length; i++) {
                if (data[i] < min || double.IsNaN(data[i])) {
                    min = data[i];
                }
            }

            return min;
        }

        public static double Maximum(double[] data)
        {
            if (data.Length == 0) {
                return double.NaN;
            }

            double max = double.NegativeInfinity;
            for (int i = 0; i < data.Length; i++) {
                if (data[i] > max || double.IsNaN(data[i])) {
                    max = data[i];
                }
            }

            return max;
        }

        public static double QuantileCustomInplace(double[] data, double tau, QuantileDefinition definition)
        {
            if (tau < 0d || tau > 1d || data.Length == 0) {
                return double.NaN;
            }

            if (tau == 0d || data.Length == 1) {
                return Minimum(data);
            }

            if (tau == 1d) {
                return Maximum(data);
            }

            switch (definition) {
                case QuantileDefinition.R1: {
                        double h = data.Length * tau + 0.5d;
                        return SelectInplace(data, (int)Math.Ceiling(h - 0.5d) - 1);
                    }

                case QuantileDefinition.R2: {
                        double h = data.Length * tau + 0.5d;
                        return (SelectInplace(data, (int)Math.Ceiling(h - 0.5d) - 1) + SelectInplace(data, (int)(h + 0.5d) - 1)) * 0.5d;
                    }

                case QuantileDefinition.R3: {
                        double h = data.Length * tau;
                        return SelectInplace(data, (int)Math.Round(h) - 1);
                    }

                case QuantileDefinition.R4: {
                        double h = data.Length * tau;
                        var hf = (int)h;
                        var lower = SelectInplace(data, hf - 1);
                        var upper = SelectInplace(data, hf);
                        return lower + (h - hf) * (upper - lower);
                    }

                case QuantileDefinition.R5: {
                        double h = data.Length * tau + 0.5d;
                        var hf = (int)h;
                        var lower = SelectInplace(data, hf - 1);
                        var upper = SelectInplace(data, hf);
                        return lower + (h - hf) * (upper - lower);
                    }

                case QuantileDefinition.R6: {
                        double h = (data.Length + 1) * tau;
                        var hf = (int)h;
                        var lower = SelectInplace(data, hf - 1);
                        var upper = SelectInplace(data, hf);
                        return lower + (h - hf) * (upper - lower);
                    }

                case QuantileDefinition.R7: {
                        double h = (data.Length - 1) * tau + 1d;
                        var hf = (int)h;
                        var lower = SelectInplace(data, hf - 1);
                        var upper = SelectInplace(data, hf);
                        return lower + (h - hf) * (upper - lower);
                    }

                case QuantileDefinition.R8: {
                        double h = (data.Length + 1 / 3d) * tau + 1 / 3d;
                        var hf = (int)h;
                        var lower = SelectInplace(data, hf - 1);
                        var upper = SelectInplace(data, hf);
                        return lower + (h - hf) * (upper - lower);
                    }

                case QuantileDefinition.R9: {
                        double h = (data.Length + 0.25d) * tau + 0.375d;
                        var hf = (int)h;
                        var lower = SelectInplace(data, hf - 1);
                        var upper = SelectInplace(data, hf);
                        return lower + (h - hf) * (upper - lower);
                    }

                default:
                    throw new NotSupportedException();
            }
        }

        private static double SelectInplace(double[] workingData, int rank)
        {
            // Numerical Recipes: select
            // http://en.wikipedia.org/wiki/Selection_algorithm
            if (rank <= 0) {
                return Minimum(workingData);
            }

            if (rank >= workingData.Length - 1) {
                return Maximum(workingData);
            }

            double[] a = workingData;
            int low = 0;
            int high = a.Length - 1;

            while (true) {
                if (high <= low + 1) {
                    if (high == low + 1 && a[high] < a[low]) {
                        var tmp = a[low];
                        a[low] = a[high];
                        a[high] = tmp;
                    }

                    return a[rank];
                }

                int middle = (low + high) >> 1;

                var tmp1 = a[middle];
                a[middle] = a[low + 1];
                a[low + 1] = tmp1;

                if (a[low] > a[high]) {
                    var tmp = a[low];
                    a[low] = a[high];
                    a[high] = tmp;
                }

                if (a[low + 1] > a[high]) {
                    var tmp = a[low + 1];
                    a[low + 1] = a[high];
                    a[high] = tmp;
                }

                if (a[low] > a[low + 1]) {
                    var tmp = a[low];
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

                    var tmp = a[begin];
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
}