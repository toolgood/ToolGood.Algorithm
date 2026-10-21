package financial

import (
	"fmt"
	"math/big"
	"strings"

	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/decimal"
)

// 财务函数共用的数值守卫与高精度幂/对数实现，
// 对应 C# 的 FinancialMath 与 System.MathEx（decimal 版 Pow / Log / Exp）。

const bigPrec = 200

var (
	decMaxSafeExponent = MustDecimal("1000000")
	decEpsSlope        = MustDecimal("0.000000000001") // 1e-12
	decEpsStep         = MustDecimal("0.0000000001")   // 1e-10
	decDaysInYear      = MustDecimal("365")
	decDefaultGuess    = MustDecimal("0.1")
	decMinInt32        = MustDecimal("-2147483648")
	decMaxInt32        = MustDecimal("2147483647")

	bigLn2 = func() *big.Float {
		// ln2 = 2 * atanh(1/3)
		z := new(big.Float).SetPrec(bigPrec).Quo(
			new(big.Float).SetPrec(bigPrec).SetInt64(1),
			new(big.Float).SetPrec(bigPrec).SetInt64(3))
		return new(big.Float).SetPrec(bigPrec).Add(bigAtanhSeries(z), bigAtanhSeries(z))
	}()
)

// toBig Decimal -> big.Float。
func toBig(d Decimal) *big.Float {
	if d.IsZero() {
		return new(big.Float).SetPrec(bigPrec)
	}
	f, _, err := big.ParseFloat(d.String(), 10, bigPrec, big.ToNearestEven)
	if err != nil {
		return new(big.Float).SetPrec(bigPrec)
	}
	return f
}

// fromBig big.Float -> Decimal（保留 34 位有效数字）。
func fromBig(f *big.Float) Decimal {
	if f.Sign() == 0 {
		return DecimalZero
	}
	if f.IsInf() {
		return DecimalZero
	}
	d, err := DecFromString(f.Text('e', 34))
	if err != nil {
		return DecimalZero
	}
	return d
}

// bigAtanhSeries atanh(z) = z + z^3/3 + z^5/5 + ...，|z| <= 1/3 时收敛良好。
func bigAtanhSeries(z *big.Float) *big.Float {
	sum := new(big.Float).SetPrec(bigPrec).Set(z)
	z2 := new(big.Float).SetPrec(bigPrec).Mul(z, z)
	term := new(big.Float).SetPrec(bigPrec).Set(z)
	eps := new(big.Float).SetPrec(bigPrec).SetMantExp(
		new(big.Float).SetPrec(bigPrec).SetInt64(1), -bigPrec-8)
	for k := int64(3); ; k += 2 {
		term = new(big.Float).SetPrec(bigPrec).Mul(term, z2)
		t := new(big.Float).SetPrec(bigPrec).Quo(term, new(big.Float).SetPrec(bigPrec).SetInt64(k))
		sum = new(big.Float).SetPrec(bigPrec).Add(sum, t)
		if new(big.Float).SetPrec(bigPrec).Abs(t).Cmp(eps) < 0 {
			break
		}
	}
	return sum
}

// bigLn 自然对数，x 必须大于 0。
func bigLn(x *big.Float) *big.Float {
	mant := new(big.Float).SetPrec(bigPrec)
	exp := x.MantExp(mant) // x = mant * 2^exp, mant in [0.5,1)

	// ln(mant) = 2 * atanh((mant-1)/(mant+1))
	one := new(big.Float).SetPrec(bigPrec).SetInt64(1)
	num := new(big.Float).SetPrec(bigPrec).Sub(mant, one)
	den := new(big.Float).SetPrec(bigPrec).Add(mant, one)
	z := new(big.Float).SetPrec(bigPrec).Quo(num, den)
	lnMant := new(big.Float).SetPrec(bigPrec).Add(bigAtanhSeries(z), bigAtanhSeries(z))

	lnExp := new(big.Float).SetPrec(bigPrec).Mul(
		new(big.Float).SetPrec(bigPrec).SetInt64(int64(exp)), bigLn2)
	return new(big.Float).SetPrec(bigPrec).Add(lnExp, lnMant)
}

// bigExp e^x。
func bigExp(x *big.Float) *big.Float {
	one := new(big.Float).SetPrec(bigPrec).SetInt64(1)
	// x = k*ln2 + r, |r| <= ln2/2
	kf := new(big.Float).SetPrec(bigPrec).Quo(x, bigLn2)
	k, _ := kf.Int(nil)
	kf = new(big.Float).SetPrec(bigPrec).SetInt(k)
	r := new(big.Float).SetPrec(bigPrec).Sub(x, new(big.Float).SetPrec(bigPrec).Mul(kf, bigLn2))
	half := new(big.Float).SetPrec(bigPrec).Quo(bigLn2, new(big.Float).SetPrec(bigPrec).SetInt64(2))
	if r.Cmp(half) > 0 {
		k.Add(k, big.NewInt(1))
		r = new(big.Float).SetPrec(bigPrec).Sub(r, bigLn2)
	} else if new(big.Float).SetPrec(bigPrec).Neg(half).Cmp(r) > 0 {
		k.Sub(k, big.NewInt(1))
		r = new(big.Float).SetPrec(bigPrec).Add(r, bigLn2)
	}

	// exp(r) = 1 + r + r^2/2! + ...
	sum := new(big.Float).SetPrec(bigPrec).Set(one)
	term := new(big.Float).SetPrec(bigPrec).Set(one)
	eps := new(big.Float).SetPrec(bigPrec).SetMantExp(
		new(big.Float).SetPrec(bigPrec).SetInt64(1), -bigPrec-8)
	for n := int64(1); ; n++ {
		term = new(big.Float).SetPrec(bigPrec).Mul(term, r)
		term = new(big.Float).SetPrec(bigPrec).Quo(term, new(big.Float).SetPrec(bigPrec).SetInt64(n))
		sum = new(big.Float).SetPrec(bigPrec).Add(sum, term)
		if new(big.Float).SetPrec(bigPrec).Abs(term).Cmp(eps) < 0 {
			break
		}
	}
	if !k.IsInt64() {
		return new(big.Float).SetPrec(bigPrec)
	}
	return new(big.Float).SetPrec(bigPrec).SetMantExp(sum, int(k.Int64()))
}

// decLn ln(x)，x <= 0 时返回 false（对应 C# MathEx.Log 抛 ArgumentException）。
func decLn(x Decimal) (Decimal, bool) {
	if x.Sign() <= 0 {
		return DecimalZero, false
	}
	if x.Equal(DecimalOne) {
		return DecimalZero, true
	}
	return fromBig(bigLn(toBig(x))), true
}

// decExp e^x；结果超出 decimal 范围时返回 false（对应 OverflowException）。
func decExp(x Decimal) (Decimal, bool) {
	r := fromBig(bigExp(toBig(x)))
	if !inDecimalRange(r) {
		return DecimalZero, false
	}
	return r, true
}

// inDecimalRange C# decimal 的取值范围检查。
func inDecimalRange(d Decimal) bool {
	return d.Abs().LessOrEqual(DecimalMax)
}

// toPrecision 把 Decimal 归一到 prec 位有效数字（HALF_EVEN）。
//
// C# 的 decimal 每一步运算都受约 28~29 位精度约束，而本仓库的 Decimal 是
// 任意精度：整数次幂会产生上万位标度，再参与除法时会把精度丢在标度上。
// 这里在幂运算后按 DECIMAL128 精度归一，既贴近 C# 语义也避免该问题。
func toPrecision(d Decimal, prec int) Decimal {
	u := d.Unscaled()
	if u.Sign() == 0 {
		return d
	}
	s := u.String()
	digits := len(s)
	if strings.HasPrefix(s, "-") {
		digits--
	}
	if digits <= prec {
		return d
	}
	drop := digits - prec
	div := new(big.Int).Exp(big.NewInt(10), big.NewInt(int64(drop)), nil)
	q, r := new(big.Int).QuoRem(u, div, new(big.Int))
	twice := new(big.Int).Abs(r)
	twice.Mul(twice, big.NewInt(2))
	cmp := twice.Cmp(div)
	if cmp > 0 || (cmp == 0 && q.Bit(0) == 1) {
		if u.Sign() < 0 {
			q.Sub(q, big.NewInt(1))
		} else {
			q.Add(q, big.NewInt(1))
		}
	}
	return decimal.DecFromUnscaled(q, d.Scale()-int32(drop))
}

// tryPowPositiveBase 以正数为底求幂（对应 FinancialMath.TryPowPositiveBase）。
func tryPowPositiveBase(baseValue, exponent Decimal) (Decimal, bool) {
	if baseValue.Sign() <= 0 {
		return DecimalZero, false
	}
	if exponent.Greater(decMaxSafeExponent) || exponent.Less(decMaxSafeExponent.Neg()) {
		return DecimalZero, false
	}
	if exponent.IsInteger() {
		n, ok := intValue(exponent)
		if !ok {
			return DecimalZero, false
		}
		r := toPrecision(baseValue.Pow(n), 34)
		if !inDecimalRange(r) {
			return DecimalZero, false
		}
		return r, true
	}
	ln, ok := decLn(baseValue)
	if !ok {
		return DecimalZero, false
	}
	r, ok := decExp(exponent.Mul(ln))
	if !ok {
		return DecimalZero, false
	}
	return r, true
}

// intValue 等价 C# decimal -> int（先向零截断）。
func intValue(d Decimal) (int, bool) {
	t := d.Trunc()
	if t.Less(decMinInt32) || t.Greater(decMaxInt32) {
		return 0, false
	}
	return int(t.Int64()), true
}

// ---------------------------------------------------------------- 数组参数

// numbersFromArray 把数组元素转成数值，错误文案与 C# 一致。
func numbersFromArray(arr Operand, name string, index int) ([]Decimal, Operand) {
	items := arr.ArrayValue()
	values := make([]Decimal, 0, len(items))
	for _, v := range items {
		if v.IsNumber() {
			values = append(values, v.NumberValue())
			continue
		}
		v2 := v.ToNumber(fmt.Sprintf("Function '%s' parameter %d is error!", name, index))
		if v2.IsErrorOrNone() {
			return nil, v2
		}
		values = append(values, v2.NumberValue())
	}
	return values, nil
}

// datesFromArray 把数组元素转成日期（日期或可解析的文本）。
// 无法转换时返回 nil，由调用方按参数错误处理。
func datesFromArray(arr Operand) []*MyDate {
	items := arr.ArrayValue()
	dates := make([]*MyDate, 0, len(items))
	for _, d := range items {
		var md *MyDate
		if d.IsDate() {
			md = d.DateValue()
		} else if d.IsText() {
			md = ParseMyDate(d.TextValue())
		} else {
			return nil
		}
		// 纯时间串(如 "12:00:00")的年月日为空，无法作为日期使用
		if md == nil || md.Year <= 0 || md.Month <= 0 || md.Day <= 0 {
			return nil
		}
		dates = append(dates, md)
	}
	return dates
}

// calculatePMT 与 C# PPMT/IPMT 中的 TryCalculatePMT 等价。
func calculatePMT(rate, nper, pv, fv Decimal, tp int) (Decimal, bool) {
	if rate.IsZero() {
		if nper.IsZero() {
			return DecimalZero, false
		}
		return pv.Add(fv).Neg().Div(nper), true
	}
	factor, ok := tryPowPositiveBase(DecimalOne.Add(rate), nper)
	if !ok {
		return DecimalZero, false
	}
	pmt := pv.Mul(factor).Add(fv).Neg().Mul(rate).Div(factor.Sub(DecimalOne))
	if tp == 1 {
		pmt = pmt.Div(DecimalOne.Add(rate))
	}
	return pmt, true
}

// calculateIPMT 与 C# PPMT/IPMT 中的 TryCalculateIPMT 等价。
func calculateIPMT(rate, per, nper, pv, fv Decimal, tp int) (Decimal, bool) {
	if rate.IsZero() {
		return DecimalZero, true
	}
	pmt, ok := calculatePMT(rate, nper, pv, fv, tp)
	if !ok {
		return DecimalZero, false
	}
	factor, ok := tryPowPositiveBase(DecimalOne.Add(rate), per.Sub(DecimalOne))
	if !ok {
		return DecimalZero, false
	}
	ipmt := pv.Mul(factor).Add(pmt.Mul(factor.Sub(DecimalOne)).Div(rate)).Neg().Mul(rate)
	if tp == 1 && per.Equal(DecimalOne) {
		ipmt = DecimalZero
	}
	return ipmt, true
}
