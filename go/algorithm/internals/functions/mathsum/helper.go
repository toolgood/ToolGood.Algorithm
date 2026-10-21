package mathsum

import (
	"math"
	"sort"

	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals"
)

// 本文件放 MathSum 各函数共用的内部工具：
// 对应 C# 的 MathEx.Pow / ArrayStatistics.QuantileCustomInplace /
// SortedArrayStatistics.QuantileRank 以及 CORREL、SLOPE 等重复出现的统计公式。

// powDecimal 对应 C# 的 MathEx.Pow(decimal, decimal)。
// 整数次幂使用 Decimal 精确计算；非整数次幂退化为 float64 计算（约 15~16 位有效数字，
// C# 的 decimal 版本为 28~29 位）。第二个返回值 false 表示非法运算或溢出（C# 抛异常）。
func powDecimal(value, power Decimal) (Decimal, bool) {
	if power.IsZero() {
		return DecimalOne, true
	}
	if power.Equal(DecimalOne) {
		return value, true
	}
	if value.Equal(DecimalOne) {
		return DecimalOne, true
	}
	if value.IsZero() {
		if power.Greater(DecimalZero) {
			return DecimalZero, true
		}
		return DecimalZero, false
	}
	if power.Equal(DecFromInt(-1)) {
		return DecimalOne.Div(value), true
	}

	isPowerInteger := power.IsInteger()
	if value.Sign() < 0 && !isPowerInteger {
		// 负底数非整数次幂（C# InvalidOperationException）
		return DecimalZero, false
	}
	if isPowerInteger {
		exp := power.Trunc().Int64()
		if exp > math.MaxInt32 || exp < math.MinInt32 {
			return DecimalZero, false
		}
		n := int(exp)
		if value.Sign() > 0 {
			r := value.Pow(n)
			if overflows(r) {
				return DecimalZero, false
			}
			return r, true
		}
		r := value.Abs().Pow(n)
		if n%2 != 0 {
			r = r.Neg()
		}
		if overflows(r) {
			return DecimalZero, false
		}
		return r, true
	}
	v := math.Pow(value.Float64(), power.Float64())
	if math.IsNaN(v) || math.IsInf(v, 0) {
		return DecimalZero, false
	}
	return DecFromFloat(v), true
}

// overflows 判断结果是否超出 C# decimal 的表示范围（C# 会抛 OverflowException）。
func overflows(v Decimal) bool {
	return v.Abs().Greater(DecimalMax)
}

// mulChecked 乘法，溢出（超出 C# decimal 范围）时返回 false。
func mulChecked(a, b Decimal) (Decimal, bool) {
	r := a.Mul(b)
	if !r.IsZero() && overflows(r) {
		return DecimalZero, false
	}
	return r, true
}

// subChecked 减法，溢出时返回 false。
func subChecked(a, b Decimal) (Decimal, bool) {
	r := a.Sub(b)
	if !r.IsZero() && overflows(r) {
		return DecimalZero, false
	}
	return r, true
}

// addChecked 加法，溢出时返回 false。
func addChecked(a, b Decimal) (Decimal, bool) {
	r := a.Add(b)
	if !r.IsZero() && overflows(r) {
		return DecimalZero, false
	}
	return r, true
}

// sortedCopy 返回升序副本（Decimal 不可直接使用 sort.Slice 之外的比较）。
func sortedCopy(data []Decimal) []Decimal {
	ordered := make([]Decimal, len(data))
	copy(ordered, data)
	sort.Slice(ordered, func(i, j int) bool { return ordered[i].Less(ordered[j]) })
	return ordered
}

// selectAt 对应 C# ArrayStatistics.SelectInplace：已排序时等价于直取下表（越界取两端）。
func selectAt(ordered []Decimal, rank int) Decimal {
	if rank <= 0 {
		return ordered[0]
	}
	if rank >= len(ordered)-1 {
		return ordered[len(ordered)-1]
	}
	return ordered[rank]
}

// quantileCustom 对应 C# ArrayStatistics.QuantileCustomInplace（Excel PERCENTILE 的 R-7 定义）。
func quantileCustom(data []Decimal, tau Decimal) Decimal {
	if tau.Less(DecimalZero) || tau.Greater(DecimalOne) || len(data) == 0 {
		return DecimalMax.Neg()
	}
	ordered := sortedCopy(data)
	n := len(ordered)
	if tau.IsZero() || n == 1 {
		return ordered[0]
	}
	if tau.Equal(DecimalOne) {
		return ordered[n-1]
	}
	h := DecFromInt(int64(n - 1)).Mul(tau).Add(DecimalOne)
	hf := int(h.Trunc().Int64())
	lower := selectAt(ordered, hf-1)
	upper := selectAt(ordered, hf)
	return lower.Add(h.Sub(DecFromInt(int64(hf))).Mul(upper.Sub(lower)))
}

// quantileRank 对应 C# SortedArrayStatistics.QuantileRank。
func quantileRank(data []Decimal, x Decimal) Decimal {
	ordered := sortedCopy(data)
	n := len(ordered)
	if x.Less(ordered[0]) {
		return DecimalZero
	}
	if x.GreaterOrEqual(ordered[n-1]) {
		return DecimalOne
	}
	index := sort.Search(n, func(i int) bool { return ordered[i].GreaterOrEqual(x) })
	if index < n && ordered[index].Equal(x) {
		left, right := index, index
		for left > 0 && ordered[left-1].Equal(ordered[left]) {
			left--
		}
		for right < n-1 && ordered[right+1].Equal(ordered[right]) {
			right++
		}
		return DecFromInt(int64(left)).Div(DecFromInt(int64(n - 1)))
	}
	right := index
	left := right - 1
	a := DecFromInt(int64(left)).Div(DecFromInt(int64(n - 1)))
	b := DecFromInt(int64(right)).Div(DecFromInt(int64(n - 1)))
	numerator := ordered[right].Sub(x).Mul(a).Add(x.Sub(ordered[left]).Mul(b))
	return numerator.Div(ordered[right].Sub(ordered[left]))
}

// correlation 实现 CORREL / PEARSON 的公共逻辑（两个 C# 文件内容完全一致）。
func correlation(f *BaseFunction, engine *AlgorithmEngine, tp TempParameter) Operand {
	array1Arg := f.GetArray(1, engine, tp)
	if array1Arg.IsErrorOrNone() {
		return array1Arg
	}
	array2Arg := f.GetArray(2, engine, tp)
	if array2Arg.IsErrorOrNone() {
		return array2Arg
	}

	xValues := make([]Decimal, 0, len(array1Arg.ArrayValue()))
	for _, item := range array1Arg.ArrayValue() {
		if !item.IsNumber() {
			return f.ParameterError(1)
		}
		xValues = append(xValues, item.NumberValue())
	}
	yValues := make([]Decimal, 0, len(array2Arg.ArrayValue()))
	for _, item := range array2Arg.ArrayValue() {
		if !item.IsNumber() {
			return f.ParameterError(2)
		}
		yValues = append(yValues, item.NumberValue())
	}

	if len(xValues) != len(yValues) || len(xValues) < 2 {
		return f.FunctionError()
	}

	n := len(xValues)
	sumX, sumY := DecimalZero, DecimalZero
	for i := 0; i < n; i++ {
		sumX = sumX.Add(xValues[i])
		sumY = sumY.Add(yValues[i])
	}
	meanX := sumX.Div(DecFromInt(int64(n)))
	meanY := sumY.Div(DecFromInt(int64(n)))

	numerator, denomX, denomY := DecimalZero, DecimalZero, DecimalZero
	for i := 0; i < n; i++ {
		dx := xValues[i].Sub(meanX)
		dy := yValues[i].Sub(meanY)
		numerator = numerator.Add(dx.Mul(dy))
		denomX = denomX.Add(dx.Mul(dx))
		denomY = denomY.Add(dy.Mul(dy))
	}
	if denomX.IsZero() || denomY.IsZero() {
		return f.Div0Error()
	}
	// 分开开方，避免 denomX * denomY 溢出
	return CreateDecimal(numerator.Div(denomX.Sqrt().Mul(denomY.Sqrt())))
}

// regression 实现 SLOPE / INTERCEPT / FORECAST 的公共逻辑
// （y 数组、x 数组的下标为 1 基参数位置）。
func regression(f *BaseFunction, engine *AlgorithmEngine, tp TempParameter, yIndex, xIndex int) (Decimal, Decimal, Operand) {
	yArrayArg := f.GetArray(yIndex, engine, tp)
	if yArrayArg.IsErrorOrNone() {
		return DecimalZero, DecimalZero, yArrayArg
	}
	xArrayArg := f.GetArray(xIndex, engine, tp)
	if xArrayArg.IsErrorOrNone() {
		return DecimalZero, DecimalZero, xArrayArg
	}

	yValues := make([]Decimal, 0, len(yArrayArg.ArrayValue()))
	for _, item := range yArrayArg.ArrayValue() {
		if item.IsNumber() {
			yValues = append(yValues, item.NumberValue())
		}
	}
	xValues := make([]Decimal, 0, len(xArrayArg.ArrayValue()))
	for _, item := range xArrayArg.ArrayValue() {
		if item.IsNumber() {
			xValues = append(xValues, item.NumberValue())
		}
	}

	if len(yValues) != len(xValues) || len(yValues) < 2 {
		return DecimalZero, DecimalZero, f.FunctionError()
	}

	n := len(yValues)
	sumX, sumY := DecimalZero, DecimalZero
	for i := 0; i < n; i++ {
		sumX = sumX.Add(xValues[i])
		sumY = sumY.Add(yValues[i])
	}
	meanX := sumX.Div(DecFromInt(int64(n)))
	meanY := sumY.Div(DecFromInt(int64(n)))

	// 采用中心化两遍法，避免 n*sumX2 - sumX*sumX 的病态消减与溢出
	numerator, denominator := DecimalZero, DecimalZero
	for i := 0; i < n; i++ {
		dx := xValues[i].Sub(meanX)
		numerator = numerator.Add(dx.Mul(yValues[i].Sub(meanY)))
		denominator = denominator.Add(dx.Mul(dx))
	}
	if denominator.IsZero() {
		return DecimalZero, DecimalZero, f.Div0Error()
	}
	slope := numerator.Div(denominator)
	intercept := meanY.Sub(slope.Mul(meanX))
	return slope, intercept, nil
}

// pairedNumberArrays 取两个数组参数，各自只保留数值项（SUMX2MY2 / SUMX2PY2 / SUMXMY2 共用）。
func pairedNumberArrays(f *BaseFunction, engine *AlgorithmEngine, tp TempParameter) ([]Decimal, []Decimal, Operand) {
	arrayXArg := f.GetArray(1, engine, tp)
	if arrayXArg.IsErrorOrNone() {
		return nil, nil, arrayXArg
	}
	arrayX := make([]Decimal, 0, len(arrayXArg.ArrayValue()))
	for _, item := range arrayXArg.ArrayValue() {
		if item.IsNumber() {
			arrayX = append(arrayX, item.NumberValue())
		}
	}

	arrayYArg := f.GetArray(2, engine, tp)
	if arrayYArg.IsErrorOrNone() {
		return nil, nil, arrayYArg
	}
	arrayY := make([]Decimal, 0, len(arrayYArg.ArrayValue()))
	for _, item := range arrayYArg.ArrayValue() {
		if item.IsNumber() {
			arrayY = append(arrayY, item.NumberValue())
		}
	}

	// Excel 在数组长度不等时返回 #N/A，此处无 #N/A 载体，统一用函数错误表示
	if len(arrayX) != len(arrayY) {
		return nil, nil, f.FunctionError()
	}
	return arrayX, arrayY, nil
}

// welfordM2 对应 STDEV / VAR 中逐元素累加的 Welford 算法（数组会先展开，非数值项尝试转数值）。
func welfordM2(f *BaseFunction, engine *AlgorithmEngine, tp TempParameter) (Decimal, int, Operand) {
	var args []Operand
	if err := f.EvaluateAll(engine, tp, &args); err != nil {
		return DecimalZero, 0, err
	}

	mean, m2 := DecimalZero, DecimalZero
	count := 0
	add := func(v Decimal) {
		count++
		delta := v.Sub(mean)
		mean = mean.Add(delta.Div(DecFromInt(int64(count))))
		m2 = m2.Add(delta.Mul(v.Sub(mean)))
	}

	for _, item := range args {
		if item.IsArray() {
			for _, elem := range item.ArrayValue() {
				if elem.IsNumber() {
					add(elem.NumberValue())
				} else if elem.IsArray() || elem.IsJson() {
					var list []Decimal
					if !internals.FlattenToNumberList(elem, &list) {
						return DecimalZero, 0, f.FunctionError()
					}
					for _, v := range list {
						add(v)
					}
				} else {
					converted := elem.ToNumber("")
					if converted.IsError() {
						return DecimalZero, 0, f.FunctionError()
					}
					add(converted.NumberValue())
				}
			}
		} else if item.IsNumber() {
			add(item.NumberValue())
		} else if item.IsJson() {
			var list []Decimal
			if !internals.FlattenToNumberList(item, &list) {
				return DecimalZero, 0, f.FunctionError()
			}
			for _, v := range list {
				add(v)
			}
		} else {
			converted := item.ToNumber("")
			if converted.IsError() {
				return DecimalZero, 0, f.FunctionError()
			}
			add(converted.NumberValue())
		}
	}
	return m2, count, nil
}

// gammaLowerRegularized 正则化下不完全伽马函数 P(a,x)（供 POISSON 使用）。
// 与 C# SpecialFunctions.GammaLowerRegularized 等价（级数 + 连分式），此处为 float64 实现。
func gammaLowerRegularized(a, x float64) float64 {
	if x <= 0 {
		return 0
	}
	if a <= 0 {
		return math.NaN()
	}
	lnPrefix := -x + a*math.Log(x) - lgamma(a)
	if x < a+1 {
		ap := a
		total := 1 / a
		delta := total
		for i := 0; i < 1000; i++ {
			ap++
			delta *= x / ap
			total += delta
			if math.Abs(delta) < math.Abs(total)*1e-16 {
				break
			}
		}
		return total * math.Exp(lnPrefix)
	}
	const fpmin = 1e-300
	b := x + 1 - a
	c := 1 / fpmin
	d := 1 / b
	h := d
	for i := 1; i < 1000; i++ {
		an := -float64(i) * (float64(i) - a)
		b += 2
		d = an*d + b
		if math.Abs(d) < fpmin {
			d = fpmin
		}
		c = b + an/c
		if math.Abs(c) < fpmin {
			c = fpmin
		}
		d = 1 / d
		delta := d * c
		h *= delta
		if math.Abs(delta-1) < 1e-16 {
			break
		}
	}
	q := math.Exp(lnPrefix) * h
	return 1 - q
}

func lgamma(x float64) float64 {
	v, _ := math.Lgamma(x)
	return v
}

// numericResult 把 float64 结果转成操作数；NaN / Inf 视为数值错误（与 C# 捕获溢出异常一致）。
func numericResult(errFn func() Operand, v float64) Operand {
	if math.IsNaN(v) || math.IsInf(v, 0) {
		return errFn()
	}
	return CreateDecimal(DecFromFloat(v))
}

// argTypes 按 C# 各函数 GetParameterTypes 的惯例传播参数类型：
// 单参数按 single 处理，多参数逐个按 multi 处理。
func argTypes(engine *AlgorithmEngine, f *BaseFunction, result *[]ParameterType, single, multi OperandType) {
	count := f.ArgCount()
	if count == 1 {
		f.Arg(0).GetParameterTypes(engine, result, single)
		return
	}
	for i := 0; i < count; i++ {
		f.Arg(i).GetParameterTypes(engine, result, multi)
	}
}
