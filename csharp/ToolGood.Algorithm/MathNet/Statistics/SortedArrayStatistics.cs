using System;

namespace ToolGood.Algorithm.MathNet.Numerics.Statistics
{
    internal static partial class SortedArrayStatistics
    {
        public static decimal QuantileRank(decimal[] data, decimal x)
        {
            if (x < data[0]) {
                return 0.0m;
            }

            if (x >= data[data.Length - 1]) {
                return 1.0m;
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

                return left / (decimal)(data.Length - 1);
            } else {
                right = ~right;
                int left = right - 1;

                var a = left / (decimal)(data.Length - 1);
                var b = right / (decimal)(data.Length - 1);
                return ((data[right] - x) * a + (x - data[left]) * b) / (data[right] - data[left]);
            }
        }
    }
}