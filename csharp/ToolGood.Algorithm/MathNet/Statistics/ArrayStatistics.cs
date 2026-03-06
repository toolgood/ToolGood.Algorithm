using System;

namespace ToolGood.Algorithm.MathNet.Numerics.Statistics
{
	internal static partial class ArrayStatistics
	{
		public static decimal Minimum(decimal[] data)
		{
			if(data.Length == 0) {
				return decimal.MinValue;
			}

			decimal min = decimal.MaxValue;
			for(int i = 0; i < data.Length; i++) {
				if(data[i] < min /*|| decimal.IsNaN(data[i])*/) {
					min = data[i];
				}
			}

			return min;
		}

		public static decimal Maximum(decimal[] data)
		{
			if(data.Length == 0) {
				return decimal.MinValue;
			}

			decimal max = decimal.MinValue;
			for(int i = 0; i < data.Length; i++) {
				if(data[i] > max /*|| decimal.IsNaN(data[i])*/) {
					max = data[i];
				}
			}

			return max;
		}

		public static decimal QuantileCustomInplace(decimal[] data, decimal tau)
		{
			if(tau < 0m || tau > 1m || data.Length == 0) {
				return decimal.MinValue;
			}

			if(tau == 0m || data.Length == 1) {
				return Minimum(data);
			}

			if(tau == 1m) {
				return Maximum(data);
			}

			decimal h = (data.Length - 1) * tau + 1m;
			var hf = (int)h;
			var lower = SelectInplace(data, hf - 1);
			var upper = SelectInplace(data, hf);
			return lower + (h - hf) * (upper - lower);
		}

		private static decimal SelectInplace(decimal[] workingData, int rank)
		{
			if(rank <= 0) {
				return Minimum(workingData);
			}

			if(rank >= workingData.Length - 1) {
				return Maximum(workingData);
			}

			decimal[] a = workingData;
			int low = 0;
			int high = a.Length - 1;

			while(true) {
				if(high <= low + 1) {
					if(high == low + 1 && a[high] < a[low]) {
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

				if(a[low] > a[high]) {
					var tmp = a[low];
					a[low] = a[high];
					a[high] = tmp;
				}

				if(a[low + 1] > a[high]) {
					var tmp = a[low + 1];
					a[low + 1] = a[high];
					a[high] = tmp;
				}

				if(a[low] > a[low + 1]) {
					var tmp = a[low];
					a[low] = a[low + 1];
					a[low + 1] = tmp;
				}

				int begin = low + 1;
				int end = high;
				decimal pivot = a[begin];

				while(true) {
					do {
						begin++;
					}
					while(a[begin] < pivot);

					do {
						end--;
					}
					while(a[end] > pivot);

					if(end < begin) {
						break;
					}

					var tmp = a[begin];
					a[begin] = a[end];
					a[end] = tmp;
				}

				a[low + 1] = a[end];
				a[end] = pivot;

				if(end >= rank) {
					high = end - 1;
				}

				if(end <= rank) {
					low = begin;
				}
			}
		}
	}
}
