package statistics

// SortedArrayStatistics 已排序数组统计量，对应 C# MathNet/Statistics/SortedArrayStatistics.cs。

// QuantileRank 返回 x 在已排序数组 data 中的分位排名（经验分布，R7）。
func QuantileRank(data []float64, x float64) float64 {
	if x < data[0] {
		return 0.0
	}

	if x >= data[len(data)-1] {
		return 1.0
	}

	right := binarySearch(data, x)
	if right >= 0 {
		left := right

		for left > 0 && data[left-1] == data[left] {
			left--
		}

		for right < len(data)-1 && data[right+1] == data[right] {
			right++
		}

		return float64(left) / float64(len(data)-1)
	}

	right = -right - 1
	left := right - 1

	a := float64(left) / float64(len(data)-1)
	b := float64(right) / float64(len(data)-1)
	return ((data[right]-x)*a + (x-data[left])*b) / (data[right] - data[left])
}

// binarySearch 等价于 C# Array.BinarySearch：找到返回下标，否则返回按位取反的插入位置。
func binarySearch(data []float64, x float64) int {
	lo := 0
	hi := len(data) - 1

	for lo <= hi {
		mid := (lo + hi) >> 1
		switch {
		case data[mid] < x:
			lo = mid + 1
		case data[mid] > x:
			hi = mid - 1
		default:
			return mid
		}
	}

	return -(lo + 1)
}
