package string

import "strings"

// FunctionVALUE 对应 C# Function_VALUE：把文本按 InvariantCulture 解析为数值。
type FunctionVALUE struct{ BaseFunction }

func newFunctionVALUE(args []Function) Function {
	RequireArgs(args, "Value", 1, 1)
	return &FunctionVALUE{BaseFunction: NewBase("Value", OperandTypeNumber, args)}
}

func (f *FunctionVALUE) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	if d, ok := parseNumberAny(a1.TextValue()); ok {
		return CreateDecimal(d)
	}
	return f.ParameterError(1)
}

// parseNumberAny 模拟 decimal.TryParse(NumberStyles.Any, InvariantCulture)：
// 允许前后空白、正负号（可在首或尾）、千位分隔符、小数、指数、括号负数
// 以及 InvariantCulture 的货币符号 "¤"。
func parseNumberAny(text string) (Decimal, bool) {
	t := strings.TrimSpace(text)
	if t == "" {
		return Decimal{}, false
	}
	neg := false
	if strings.HasPrefix(t, "(") && strings.HasSuffix(t, ")") {
		neg = true
		t = strings.TrimSpace(t[1 : len(t)-1])
	}
	// InvariantCulture 只有 "¤" 一个货币符号（"$" 属于其他区域，不被接受）
	t = strings.TrimSpace(strings.TrimPrefix(t, "¤"))
	t = strings.TrimSpace(strings.TrimSuffix(t, "¤"))
	hasSign := false
	if strings.HasPrefix(t, "-") || strings.HasPrefix(t, "+") {
		hasSign = true
	}
	if neg && hasSign {
		return Decimal{}, false
	}
	// 结尾的正负号（NumberStyles.AllowTrailingSign）
	if strings.HasSuffix(t, "-") {
		if neg || hasSign {
			return Decimal{}, false
		}
		neg = true
		t = strings.TrimSpace(t[:len(t)-1])
	} else if strings.HasSuffix(t, "+") {
		if neg || hasSign {
			return Decimal{}, false
		}
		t = strings.TrimSpace(t[:len(t)-1])
	}
	if t == "" {
		return Decimal{}, false
	}
	// AllowThousands：逗号只出现在整数部分，且不校验分组长度
	if strings.Contains(t, ",") {
		idx := strings.IndexAny(t, ".eE")
		intPart := t
		rest := ""
		if idx >= 0 {
			intPart = t[:idx]
			rest = t[idx:]
		}
		sign := ""
		if strings.HasPrefix(intPart, "-") || strings.HasPrefix(intPart, "+") {
			sign = intPart[:1]
			intPart = intPart[1:]
		}
		if intPart == "" || strings.HasPrefix(intPart, ",") {
			return Decimal{}, false
		}
		if strings.Contains(rest, ",") {
			return Decimal{}, false
		}
		t = sign + strings.ReplaceAll(intPart, ",", "") + rest
	}
	d, ok := TryParseDecimal(t)
	if !ok {
		return Decimal{}, false
	}
	if neg {
		d = d.Neg()
	}
	return d, true
}

func init() { RegisterFunction("VALUE", newFunctionVALUE) }
