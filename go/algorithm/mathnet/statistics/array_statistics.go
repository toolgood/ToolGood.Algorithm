package statistics

// ArrayStatistics 数组统计量，对应 C# MathNet/Statistics/ArrayStatistics.cs。

// 与 C# decimal 边界值保持一致。
const (
	decimalMinValue = -79228162514264337593543950335.0
	decimalMaxValue = 79228162514264337593543950335.0
)

// Minimum 返回数组最小值；空数组返回 decimal.MinValue。
func Minimum(data []float64) float64 {
	if len(data) == 0 {
		return decimalMinValue
	}

	min := decimalMaxValue
	for i := 0; i < len(data); i++ {
		if data[i] < min {
			min = data[i]
		}
	}

	return min
}

// Maximum 返回数组最大值；空数组返回 decimal.MinValue。
func Maximum(data []float64) float64 {
	if len(data) == 0 {
		return decimalMinValue
	}

	max := decimalMinValue
	for i := 0; i < len(data); i++ {
		if data[i] > max {
			max = data[i]
		}
	}

	return max
}

// QuantileCustomInplace 就地重排数组并计算分位数 tau（R7 定义）。
func QuantileCustomInplace(data []float64, tau float64) float64 {
	if tau < 0 || tau > 1 || len(data) == 0 {
		return decimalMinValue
	}

	if tau == 0 || len(data) == 1 {
		return Minimum(data)
	}

	if tau == 1 {
		return Maximum(data)
	}

	h := float64(len(data)-1)*tau + 1.0
	hf := int(h)
	lower := SelectInplace(data, hf-1)
	upper := SelectInplace(data, hf)
	return lower + (h-float64(hf))*(upper-lower)
}

// SelectInplace 就地选出第 rank 小（0 基）的元素。
func SelectInplace(workingData []float64, rank int) float64 {
	if rank <= 0 {
		return Minimum(workingData)
	}

	if rank >= len(workingData)-1 {
		return Maximum(workingData)
	}

	a := workingData
	low := 0
	high := len(a) - 1

	for {
		if high <= low+1 {
			if high == low+1 && a[high] < a[low] {
				tmp := a[low]
				a[low] = a[high]
				a[high] = tmp
			}

			return a[rank]
		}

		middle := (low + high) >> 1

		tmp1 := a[middle]
		a[middle] = a[low+1]
		a[low+1] = tmp1

		if a[low] > a[high] {
			tmp := a[low]
			a[low] = a[high]
			a[high] = tmp
		}

		if a[low+1] > a[high] {
			tmp := a[low+1]
			a[low+1] = a[high]
			a[high] = tmp
		}

		if a[low] > a[low+1] {
			tmp := a[low]
			a[low] = a[low+1]
			a[low+1] = tmp
		}

		begin := low + 1
		end := high
		pivot := a[begin]

		for {
			begin++
			for a[begin] < pivot {
				begin++
			}

			end--
			for a[end] > pivot {
				end--
			}

			if end < begin {
				break
			}

			tmp := a[begin]
			a[begin] = a[end]
			a[end] = tmp
		}

		a[low+1] = a[end]
		a[end] = pivot

		if end >= rank {
			high = end - 1
		}

		if end <= rank {
			low = begin
		}
	}
}
