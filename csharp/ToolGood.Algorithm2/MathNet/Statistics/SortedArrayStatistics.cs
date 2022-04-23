using System;


namespace ToolGood.Algorithm.MathNet.Numerics.Statistics
{
    static partial class SortedArrayStatistics
    {
        public static double QuantileRank(double[] data, double x)
        {
            if (x < data[0]) {
                return 0.0;
            }

            if (x >= data[data.Length - 1]) {
                return 1.0;
            }

            int right = Array.BinarySearch(data, x);
            if (right >= 0) {
                int left = right;

                while (left > 0 && data[left - 1] == data[left]) {
                    left--;
                }

                while (right < data.Length - 1 && data[right + 1] == data[right]) {
                    right++;
                }


                return left / (double)(data.Length - 1);

            } else {
                right = ~right;
                int left = right - 1;


                var a = left / (double)(data.Length - 1);
                var b = right / (double)(data.Length - 1);
                return ((data[right] - x) * a + (x - data[left]) * b) / (data[right] - data[left]);
            }
        }

    }
}
