package string

import (
	"math/big"
	"strings"
)

// FunctionRMB 对应 C# Function_RMB：数值转中文大写金额。
type FunctionRMB struct{ BaseFunction }

func newFunctionRMB(args []Function) Function {
	RequireArgs(args, "Rmb", 1, 1)
	return &FunctionRMB{BaseFunction: NewBase("Rmb", OperandTypeText, args)}
}

func (f *FunctionRMB) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetNumber(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	return CreateString(toChineseRMB(a1.NumberValue()))
}

var rmbNumberNames = [...]string{"零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖"}

// rmbGroupNames 对应 C# 格式串中 F/G/H/I/J/K/L 对应的大单位（10^4 起，每 4 位一级）。
var rmbGroupNames = map[int]string{
	4:  "万",
	8:  "亿",
	12: "兆",
	16: "京",
	20: "垓",
	24: "秭",
	28: "穰",
}

// toChineseRMB 复刻 C# Function_RMB 的 "格式串 + 正则消零" 结果：
// 0 返回空串；不输出"整"；负数会丢掉符号（与原实现一致）。
func toChineseRMB(x Decimal) string {
	if x.IsZero() {
		return ""
	}
	// 先放大 100 倍并按四舍五入取整（与原格式串 "0B0A" 的舍入一致）
	cents := x.Abs().Mul(DecFromInt(100)).RoundHalfUp(0)
	n, ok := new(big.Int).SetString(cents.Trunc().String(), 10)
	if !ok {
		return ""
	}
	hundred := big.NewInt(100)
	yuan, rem := new(big.Int).QuoRem(n, hundred, new(big.Int))
	jiao := new(big.Int).Quo(rem, big.NewInt(10)).Int64()
	fen := new(big.Int).Rem(rem, big.NewInt(10)).Int64()

	var sb strings.Builder
	digits := yuan.String()
	length := len(digits)
	zeroPending := false
	for i := 0; i < length; i++ {
		power := length - 1 - i
		d := int(digits[i] - '0')
		if d == 0 {
			zeroPending = true
			// 万/亿/兆…… 一级的末尾：该级内还有非零数字时保留大单位
			if power > 0 && power%4 == 0 && rmbGroupHasNonZero(digits, i) {
				if name, ok := rmbGroupNames[power]; ok {
					sb.WriteString(name)
				}
				zeroPending = false
			}
			continue
		}
		if zeroPending && sb.Len() > 0 {
			sb.WriteString("零")
		}
		zeroPending = false
		sb.WriteString(rmbNumberNames[d])
		sb.WriteString(rmbUnitName(power))
	}

	if yuan.Sign() > 0 {
		sb.WriteString("元")
	}
	if jiao > 0 {
		sb.WriteString(rmbNumberNames[jiao])
		sb.WriteString("角")
	}
	if fen > 0 {
		if jiao == 0 && yuan.Sign() > 0 {
			sb.WriteString("零")
		}
		sb.WriteString(rmbNumberNames[fen])
		sb.WriteString("分")
	}
	return sb.String()
}

// rmbUnitName 返回某一位（10^power）的单位名。
func rmbUnitName(power int) string {
	if power <= 0 {
		return ""
	}
	switch power % 4 {
	case 1:
		return "拾"
	case 2:
		return "佰"
	case 3:
		return "仟"
	}
	return rmbGroupNames[power]
}

// rmbGroupHasNonZero 判断以 i 结尾（10^power 位）的 4 位一级中是否有非零数字。
func rmbGroupHasNonZero(digits string, i int) bool {
	start := i - 3
	if start < 0 {
		start = 0
	}
	for k := start; k <= i; k++ {
		if digits[k] != '0' {
			return true
		}
	}
	return false
}

func init() { RegisterFunction("RMB", newFunctionRMB) }
