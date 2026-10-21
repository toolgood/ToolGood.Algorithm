package decimal

// 十进制数：等价于 C# 的 decimal / Java 的 BigDecimal。
//
// 采用「非标度整数 + 标度」表示：value = unscaled * 10^-scale。
// 与 BigDecimal 保持一致：
//   - 加减乘为精确运算（不舍入）；
//   - 除法按 34 位有效数字（DECIMAL128）舍入，HALF_EVEN；
//   - String() 等价于 BigDecimal.toPlainString()，保留末尾 0。
//
// Decimal 是值类型且所有运算都返回新值，可安全共享。

import (
	"errors"
	"math"
	"math/big"
	"strconv"
	"strings"
)

const (
	// decimalPrecision DECIMAL128 有效数字位数。
	decimalPrecision = 34
	// decimalPrecision32 DECIMAL32 有效数字位数。
	decimalPrecision32 = 7
)

// Decimal 任意精度定点十进制数。
type Decimal struct {
	unscaled *big.Int
	scale    int32
}

var (
	// DecimalZero 常量 0。
	DecimalZero = Decimal{}
	// DecimalOne 常量 1。
	DecimalOne = Decimal{unscaled: big.NewInt(1)}
	// DecimalMaxRate C# decimal.MaxValue。
	DecimalMax = MustDecimal("79228162514264337593543950335")
	errBadNum  = errors.New("invalid number")
)

// DecFromInt 由 int64 创建 Decimal。
func DecFromInt(i int64) Decimal {
	if i == 0 {
		return Decimal{}
	}
	return Decimal{unscaled: big.NewInt(i)}
}

// DecFromUint 由 uint64 创建 Decimal。
func DecFromUint(i uint64) Decimal {
	if i == 0 {
		return Decimal{}
	}
	return Decimal{unscaled: new(big.Int).SetUint64(i)}
}

// DecFromBigInt 由 big.Int 创建 Decimal。
func DecFromBigInt(i *big.Int) Decimal {
	if i == nil || i.Sign() == 0 {
		return Decimal{}
	}
	return Decimal{unscaled: new(big.Int).Set(i)}
}

// DecFromFloat 由 float64 创建 Decimal（按最短往返字符串，等同 Java BigDecimal.valueOf）。
func DecFromFloat(f float64) Decimal {
	if math.IsNaN(f) || math.IsInf(f, 0) {
		return Decimal{}
	}
	d, err := DecFromString(strconv.FormatFloat(f, 'g', -1, 64))
	if err != nil {
		return Decimal{}
	}
	return d
}

// DecFromUnscaled 由非标度整数与标度创建 Decimal。
func DecFromUnscaled(unscaled *big.Int, scale int32) Decimal {
	if unscaled == nil || unscaled.Sign() == 0 {
		return Decimal{}
	}
	return Decimal{unscaled: new(big.Int).Set(unscaled), scale: scale}
}

// DecFromString 解析十进制字符串，接受符号、小数与指数。
func DecFromString(s string) (Decimal, error) {
	t := strings.TrimSpace(s)
	if t == "" {
		return Decimal{}, errBadNum
	}
	neg := false
	if t[0] == '+' || t[0] == '-' {
		neg = t[0] == '-'
		t = t[1:]
	}
	mantissa := t
	exp := 0
	if idx := strings.IndexAny(t, "eE"); idx >= 0 {
		mantissa = t[:idx]
		e, err := strconv.Atoi(t[idx+1:])
		if err != nil {
			return Decimal{}, errBadNum
		}
		exp = e
	}
	intPart := mantissa
	fracPart := ""
	if idx := strings.IndexByte(mantissa, '.'); idx >= 0 {
		intPart = mantissa[:idx]
		fracPart = mantissa[idx+1:]
	}
	if intPart == "" && fracPart == "" {
		return Decimal{}, errBadNum
	}
	digits := intPart + fracPart
	if digits == "" {
		return Decimal{}, errBadNum
	}
	for i := 0; i < len(digits); i++ {
		if digits[i] < '0' || digits[i] > '9' {
			return Decimal{}, errBadNum
		}
	}
	u, ok := new(big.Int).SetString(digits, 10)
	if !ok {
		return Decimal{}, errBadNum
	}
	if neg {
		u.Neg(u)
	}
	scale := int32(len(fracPart)) - int32(exp)
	if scale < 0 {
		u.Mul(u, pow10(-scale))
		scale = 0
	}
	if u.Sign() == 0 {
		return Decimal{}, nil
	}
	return Decimal{unscaled: u, scale: scale}, nil
}

// MustDecimal 解析十进制字符串，失败时返回 0。
func MustDecimal(s string) Decimal {
	d, err := DecFromString(s)
	if err != nil {
		return Decimal{}
	}
	return d
}

// TryParseDecimal 解析成功时返回 (值, true)。
func TryParseDecimal(s string) (Decimal, bool) {
	d, err := DecFromString(s)
	if err != nil {
		return Decimal{}, false
	}
	return d, true
}

// NewDecimal 是 DecFromInt 的别名，便于移植代码阅读。
func NewDecimal(i int64) Decimal { return DecFromInt(i) }

func pow10(n int32) *big.Int {
	if n <= 0 {
		return big.NewInt(1)
	}
	result := new(big.Int).Exp(big.NewInt(10), big.NewInt(int64(n)), nil)
	return result
}

func (d Decimal) u() *big.Int {
	if d.unscaled == nil {
		return big.NewInt(0)
	}
	return d.unscaled
}

func (d Decimal) rat() *big.Rat {
	if d.scale == 0 {
		return new(big.Rat).SetInt(d.u())
	}
	return new(big.Rat).SetFrac(d.u(), pow10(d.scale))
}

// Scale 返回标度（小数位数）。
func (d Decimal) Scale() int32 { return d.scale }

// Unscaled 返回非标度整数副本。
func (d Decimal) Unscaled() *big.Int { return new(big.Int).Set(d.u()) }

// IsZero 是否为零。
func (d Decimal) IsZero() bool { return d.u().Sign() == 0 }

// Sign 返回 -1 / 0 / 1。
func (d Decimal) Sign() int { return d.u().Sign() }

// IsNegative 是否为负数。
func (d Decimal) IsNegative() bool { return d.u().Sign() < 0 }

// IsPositive 是否为正数。
func (d Decimal) IsPositive() bool { return d.u().Sign() > 0 }

func (d Decimal) align(e Decimal) (a, b *big.Int, scale int32) {
	if d.scale == e.scale {
		return d.u(), e.u(), d.scale
	}
	if d.scale < e.scale {
		return new(big.Int).Mul(d.u(), pow10(e.scale-d.scale)), e.u(), e.scale
	}
	return d.u(), new(big.Int).Mul(e.u(), pow10(d.scale-e.scale)), d.scale
}

// Add 加法（精确）。
func (d Decimal) Add(e Decimal) Decimal {
	a, b, scale := d.align(e)
	sum := new(big.Int).Add(a, b)
	if sum.Sign() == 0 {
		return Decimal{}
	}
	return Decimal{unscaled: sum, scale: scale}
}

// Sub 减法（精确）。
func (d Decimal) Sub(e Decimal) Decimal {
	a, b, scale := d.align(e)
	diff := new(big.Int).Sub(a, b)
	if diff.Sign() == 0 {
		return Decimal{}
	}
	return Decimal{unscaled: diff, scale: scale}
}

// Mul 乘法（精确）。
func (d Decimal) Mul(e Decimal) Decimal {
	product := new(big.Int).Mul(d.u(), e.u())
	if product.Sign() == 0 {
		return Decimal{}
	}
	return Decimal{unscaled: product, scale: d.scale + e.scale}
}

// Neg 取反。
func (d Decimal) Neg() Decimal {
	if d.IsZero() {
		return Decimal{}
	}
	return Decimal{unscaled: new(big.Int).Neg(d.u()), scale: d.scale}
}

// Abs 绝对值。
func (d Decimal) Abs() Decimal {
	if d.Sign() >= 0 {
		return d
	}
	return d.Neg()
}

// Cmp 比较：d<e 返回 -1，相等 0，d>e 返回 1。
func (d Decimal) Cmp(e Decimal) int {
	a, b, _ := d.align(e)
	return a.Cmp(b)
}

// Equal 是否相等（数值比较，忽略标度）。
func (d Decimal) Equal(e Decimal) bool { return d.Cmp(e) == 0 }

// Less d < e。
func (d Decimal) Less(e Decimal) bool { return d.Cmp(e) < 0 }

// LessOrEqual d <= e。
func (d Decimal) LessOrEqual(e Decimal) bool { return d.Cmp(e) <= 0 }

// Greater d > e。
func (d Decimal) Greater(e Decimal) bool { return d.Cmp(e) > 0 }

// GreaterOrEqual d >= e。
func (d Decimal) GreaterOrEqual(e Decimal) bool { return d.Cmp(e) >= 0 }

// Div 除法，按 DECIMAL128（34 位有效数字）舍入。
func (d Decimal) Div(e Decimal) Decimal {
	return divideWithPrecision(d, e, decimalPrecision)
}

// Div32 除法，按 DECIMAL32（7 位有效数字）舍入。
func (d Decimal) Div32(e Decimal) Decimal {
	return divideWithPrecision(d, e, decimalPrecision32)
}

// DivExact 精确除法，除不尽时按 34 位有效数字舍入。
func (d Decimal) DivExact(e Decimal) Decimal {
	return divideWithPrecision(d, e, decimalPrecision)
}

func divideWithPrecision(a, b Decimal, prec int) Decimal {
	if b.IsZero() {
		return Decimal{}
	}
	if a.IsZero() {
		return Decimal{}
	}
	ratio := new(big.Rat).SetFrac(a.u(), b.u())
	exp10 := int(b.scale) - int(a.scale)

	// 有限小数：直接给出精确结果，不补零（等价 BigDecimal.Divide 的精确分支）。
	if s, ok := terminatingScale(ratio.Denom()); ok {
		// ratio = num / den = N / 10^s
		n := new(big.Int).Mul(ratio.Num(), new(big.Int).Div(pow10(int32(s)), ratio.Denom()))
		scale := int32(s) - int32(exp10)
		if scale < 0 {
			n.Mul(n, pow10(-scale))
			scale = 0
		}
		return roundUnscaledToPrecision(n, scale, prec)
	}

	// 无限小数：取 prec 位有效数字。
	e := ratExponent(ratio) + exp10
	scale := int32(prec - e)
	if scale < 0 {
		scale = 0
	}
	shift := int(scale) + exp10
	unscaled := roundRatToInt(ratio, shift)
	return normalize(unscaled, scale)
}

func normalize(unscaled *big.Int, scale int32) Decimal {
	if unscaled.Sign() == 0 {
		return Decimal{}
	}
	return Decimal{unscaled: unscaled, scale: scale}
}

// roundUnscaledToPrecision 将非标度整数舍入到 prec 位有效数字。
func roundUnscaledToPrecision(unscaled *big.Int, scale int32, prec int) Decimal {
	if unscaled.Sign() == 0 {
		return Decimal{}
	}
	digits := len(strings.TrimPrefix(unscaled.String(), "-"))
	if digits <= prec {
		return normalize(unscaled, scale)
	}
	drop := digits - prec
	divisor := pow10(int32(drop))
	q, r := new(big.Int).QuoRem(unscaled, divisor, new(big.Int))
	// HALF_EVEN
	twice := new(big.Int).Abs(r)
	twice.Mul(twice, big.NewInt(2))
	cmp := twice.Cmp(divisor)
	if cmp > 0 || (cmp == 0 && q.Bit(0) == 1) {
		if unscaled.Sign() < 0 {
			q.Sub(q, big.NewInt(1))
		} else {
			q.Add(q, big.NewInt(1))
		}
	}
	return normalize(q, scale-int32(drop))
}

// terminatingScale 若分母仅含因子 2/5，返回所需标度。
func terminatingScale(den *big.Int) (int, bool) {
	if den == nil || den.Sign() == 0 {
		return 0, false
	}
	d := new(big.Int).Abs(den)
	two, five := 0, 0
	r := new(big.Int)
	for {
		q := new(big.Int)
		q.QuoRem(d, big.NewInt(2), r)
		if r.Sign() != 0 {
			break
		}
		d = q
		two++
	}
	for {
		q := new(big.Int)
		q.QuoRem(d, big.NewInt(5), r)
		if r.Sign() != 0 {
			break
		}
		d = q
		five++
	}
	if d.Cmp(big.NewInt(1)) != 0 {
		return 0, false
	}
	if five > two {
		return five, true
	}
	return two, true
}

// ratExponent 返回 e，使 10^(e-1) <= |r| < 10^e。
func ratExponent(r *big.Rat) int {
	if r.Sign() == 0 {
		return 1
	}
	f, _ := r.Float64()
	e := 1
	if f > 0 && !math.IsInf(f, 0) && f != 0 {
		e = int(math.Floor(math.Log10(f))) + 1
	}
	// 用精确比较修正浮点误差。
	for i := 0; i < 4; i++ {
		if cmpPow10(r, e) >= 0 {
			e++
		} else if cmpPow10(r, e-1) < 0 {
			e--
		} else {
			break
		}
	}
	return e
}

func cmpPow10(r *big.Rat, e int) int {
	if e >= 0 {
		return r.Cmp(new(big.Rat).SetInt(pow10(int32(e))))
	}
	return r.Cmp(new(big.Rat).SetFrac(big.NewInt(1), pow10(int32(-e))))
}

// roundRatToInt 计算 round(r * 10^shift)（HALF_EVEN）。
func roundRatToInt(r *big.Rat, shift int) *big.Int {
	num := new(big.Int).Set(r.Num())
	den := new(big.Int).Set(r.Denom())
	if shift >= 0 {
		num.Mul(num, pow10(int32(shift)))
	} else {
		den.Mul(den, pow10(int32(-shift)))
	}
	q, rem := new(big.Int).QuoRem(num, den, new(big.Int))
	if rem.Sign() == 0 {
		return q
	}
	twice := new(big.Int).Abs(rem)
	twice.Mul(twice, big.NewInt(2))
	cmp := twice.Cmp(new(big.Int).Abs(den))
	if cmp > 0 || (cmp == 0 && q.Bit(0) == 1) {
		if num.Sign() < 0 {
			q.Sub(q, big.NewInt(1))
		} else {
			q.Add(q, big.NewInt(1))
		}
	}
	return q
}

func floorRat(r *big.Rat) *big.Int {
	q, rem := new(big.Int).QuoRem(r.Num(), r.Denom(), new(big.Int))
	if rem.Sign() < 0 {
		q.Sub(q, big.NewInt(1))
	}
	return q
}

// Mod 取余，语义同 Java BigDecimal.remainder 再按 Excel MOD 规则修正符号之前的基础值：
// 结果为 d - e*trunc(d/e)，符号与 d 相同。
func (d Decimal) Mod(e Decimal) Decimal {
	if e.IsZero() {
		return Decimal{}
	}
	q := new(big.Rat).Quo(d.rat(), e.rat())
	trunc := new(big.Int).Quo(q.Num(), q.Denom())
	prod := new(big.Rat).Mul(e.rat(), new(big.Rat).SetInt(trunc))
	rem := new(big.Rat).Sub(d.rat(), prod)
	return ratToDecimal(rem, decimalPrecision)
}

// FloorDiv 向下取整除法结果（Python 风格 //）。
func (d Decimal) FloorDiv(e Decimal) Decimal {
	if e.IsZero() {
		return Decimal{}
	}
	q := new(big.Rat).Quo(d.rat(), e.rat())
	return DecFromBigInt(floorRat(q))
}

// FloorMod 取余，结果符号与除数相同（Python / Excel MOD 语义）。
func (d Decimal) FloorMod(e Decimal) Decimal {
	if e.IsZero() {
		return Decimal{}
	}
	q := new(big.Rat).Quo(d.rat(), e.rat())
	fl := floorRat(q)
	prod := new(big.Rat).Mul(e.rat(), new(big.Rat).SetInt(fl))
	rem := new(big.Rat).Sub(d.rat(), prod)
	return ratToDecimal(rem, decimalPrecision)
}

func ratToDecimal(r *big.Rat, prec int) Decimal {
	if r.Sign() == 0 {
		return Decimal{}
	}
	if s, ok := terminatingScale(r.Denom()); ok {
		n := new(big.Int).Mul(r.Num(), new(big.Int).Div(pow10(int32(s)), r.Denom()))
		return roundUnscaledToPrecision(n, int32(s), prec)
	}
	e := ratExponent(r)
	scale := int32(prec - e)
	if scale < 0 {
		scale = 0
	}
	return normalize(roundRatToInt(r, int(scale)), scale)
}

// Trunc 向零取整。
func (d Decimal) Trunc() Decimal {
	if d.scale == 0 {
		return d
	}
	return normalize(new(big.Int).Quo(d.u(), pow10(d.scale)), 0)
}

// Floor 向下取整。
func (d Decimal) Floor() Decimal {
	if d.scale == 0 {
		return d
	}
	div := pow10(d.scale)
	q, r := new(big.Int).QuoRem(d.u(), div, new(big.Int))
	if r.Sign() < 0 {
		q.Sub(q, big.NewInt(1))
	}
	return normalize(q, 0)
}

// Ceil 向上取整。
func (d Decimal) Ceil() Decimal {
	if d.scale == 0 {
		return d
	}
	div := pow10(d.scale)
	q, r := new(big.Int).QuoRem(d.u(), div, new(big.Int))
	if r.Sign() > 0 {
		q.Add(q, big.NewInt(1))
	}
	return normalize(q, 0)
}

// RoundHalfUp 四舍五入（远离零），digits 为保留小数位数。
func (d Decimal) RoundHalfUp(digits int32) Decimal {
	if d.scale <= digits {
		return d
	}
	drop := d.scale - digits
	div := pow10(drop)
	q, r := new(big.Int).QuoRem(d.u(), div, new(big.Int))
	twice := new(big.Int).Abs(r)
	twice.Mul(twice, big.NewInt(2))
	if twice.Cmp(div) >= 0 {
		if d.u().Sign() < 0 {
			q.Sub(q, big.NewInt(1))
		} else {
			q.Add(q, big.NewInt(1))
		}
	}
	return normalize(q, digits)
}

// RoundHalfEven 银行家舍入。
func (d Decimal) RoundHalfEven(digits int32) Decimal {
	if d.scale <= digits {
		return d
	}
	drop := d.scale - digits
	div := pow10(drop)
	q, r := new(big.Int).QuoRem(d.u(), div, new(big.Int))
	twice := new(big.Int).Abs(r)
	twice.Mul(twice, big.NewInt(2))
	cmp := twice.Cmp(div)
	if cmp > 0 || (cmp == 0 && q.Bit(0) == 1) {
		if d.u().Sign() < 0 {
			q.Sub(q, big.NewInt(1))
		} else {
			q.Add(q, big.NewInt(1))
		}
	}
	return normalize(q, digits)
}

// Pow 整数次幂（精确）。n<0 时按 34 位有效数字舍入。
func (d Decimal) Pow(n int) Decimal {
	if n == 0 {
		return DecimalOne
	}
	if n < 0 {
		return DecimalOne.Div(d.Pow(-n))
	}
	result := DecimalOne
	base := d
	for n > 0 {
		if n&1 == 1 {
			result = result.Mul(base)
		}
		n >>= 1
		if n > 0 {
			base = base.Mul(base)
		}
	}
	return result
}

// Sqrt 平方根，按 34 位有效数字舍入。
func (d Decimal) Sqrt() Decimal {
	if d.Sign() < 0 {
		return Decimal{}
	}
	if d.IsZero() {
		return Decimal{}
	}
	f := new(big.Float).SetPrec(200).SetRat(d.rat())
	f.Sqrt(f)
	text := f.Text('e', decimalPrecision+5)
	result, err := DecFromString(text)
	if err != nil {
		return Decimal{}
	}
	return roundUnscaledToPrecision(result.u(), result.scale, decimalPrecision)
}

// Float64 转换为 float64。
func (d Decimal) Float64() float64 {
	f, _ := d.rat().Float64()
	return f
}

// Int64 向零截断为 int64（越界时饱和）。
func (d Decimal) Int64() int64 {
	t := d.Trunc()
	if !t.u().IsInt64() {
		if t.Sign() < 0 {
			return math.MinInt64
		}
		return math.MaxInt64
	}
	return t.u().Int64()
}

// Int 向零截断为 int。
func (d Decimal) Int() int { return int(d.Int64()) }

// IsInteger 是否为整数。
func (d Decimal) IsInteger() bool { return d.Trunc().Equal(d) }

// String 等价于 BigDecimal.toPlainString()，保留末尾 0。
func (d Decimal) String() string {
	u := d.u()
	if u.Sign() == 0 {
		return "0"
	}
	neg := u.Sign() < 0
	abs := new(big.Int).Abs(u).String()
	scale := int(d.scale)
	var sb strings.Builder
	if neg {
		sb.WriteByte('-')
	}
	if scale <= 0 {
		sb.WriteString(abs)
		if scale < 0 {
			sb.WriteString(strings.Repeat("0", -scale))
		}
		return sb.String()
	}
	if len(abs) <= scale {
		sb.WriteString("0.")
		sb.WriteString(strings.Repeat("0", scale-len(abs)))
		sb.WriteString(abs)
	} else {
		cut := len(abs) - scale
		sb.WriteString(abs[:cut])
		sb.WriteByte('.')
		sb.WriteString(abs[cut:])
	}
	return sb.String()
}

// PlainStrip 去掉末尾 0 的普通字符串（Operand.ToText 使用）。
func (d Decimal) PlainStrip() string {
	s := d.String()
	if strings.Contains(s, ".") {
		s = strings.TrimRight(s, "0")
		s = strings.TrimRight(s, ".")
	}
	if s == "" || s == "-" {
		return "0"
	}
	return s
}

// StringFixed 保留 digits 位小数（四舍五入）。
func (d Decimal) StringFixed(digits int32) string {
	return d.RoundHalfUp(digits).String()
}

// MarshalJSON 输出为普通数字文本。
func (d Decimal) MarshalJSON() ([]byte, error) {
	return []byte(d.String()), nil
}

// UnmarshalJSON 解析 JSON 数字。
func (d *Decimal) UnmarshalJSON(b []byte) error {
	v, err := DecFromString(string(b))
	if err != nil {
		return err
	}
	*d = v
	return nil
}
