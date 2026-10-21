package compare

import "strings"

// 比较运算共用逻辑，移植自 Internals/Functions/Compare/Function_EQ.cs 等
// （六个比较类的 Evaluate / ToString / GetParameterTypes 结构完全一致）。

// compareKind 比较类型。
type compareKind int

const (
	compareEQ compareKind = iota
	compareNE
	compareGT
	compareGE
	compareLT
	compareLE
)

// evaluateCompare 求值两侧参数并按 C# 的分支规则比较。
func evaluateCompare(f *BaseFunction, kind compareKind, engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.EvalArg(0, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	b := f.EvalArg(1, engine, tp)
	if b.IsErrorOrNone() {
		return b
	}

	if a.Type() == b.Type() {
		switch {
		case a.IsNumber():
			return CreateBool(compareDecimal(kind, a.NumberValue(), b.NumberValue()))
		case a.IsText():
			if kind == compareEQ {
				return CreateBool(a.TextValue() == b.TextValue())
			}
			if kind == compareNE {
				return CreateBool(a.TextValue() != b.TextValue())
			}
			return CreateBool(compareOrdinal(kind, strings.Compare(a.TextValue(), b.TextValue())))
		case a.IsBoolean():
			if kind == compareEQ {
				return CreateBool(a.BooleanValue() == b.BooleanValue())
			}
			if kind == compareNE {
				return CreateBool(a.BooleanValue() != b.BooleanValue())
			}
			na := a.ToNumber("")
			if na.IsErrorOrNone() {
				return na
			}
			nb := b.ToNumber("")
			if nb.IsErrorOrNone() {
				return nb
			}
			return CreateBool(compareDecimal(kind, na.NumberValue(), nb.NumberValue()))
		case a.IsDate():
			return CreateBool(compareDecimal(kind, DecFromInt(a.DateValue().ToLong()), DecFromInt(b.DateValue().ToLong())))
		case a.IsNull():
			// null 属于异常数据，两侧同为 null 时无法判断，返回错误。
			return f.CompareError()
		}
		return f.CompareError()
	} else if a.IsNull() || b.IsNull() {
		if kind == compareNE {
			return True
		}
		return False
	} else if a.IsDate() || b.IsDate() || a.IsJson() || b.IsJson() ||
		a.IsArray() || b.IsArray() || a.IsArrayJson() || b.IsArrayJson() {
		return f.CompareError()
	}

	a = f.ConvertToNumber(a, 1)
	if a.IsErrorOrNone() {
		return a
	}
	b = f.ConvertToNumber(b, 2)
	if b.IsErrorOrNone() {
		return b
	}
	return CreateBool(compareDecimal(kind, a.NumberValue(), b.NumberValue()))
}

// compareDecimal 比较两个数值。
func compareDecimal(kind compareKind, a, b Decimal) bool {
	switch kind {
	case compareEQ:
		return a.Equal(b)
	case compareNE:
		return !a.Equal(b)
	case compareGT:
		return a.Greater(b)
	case compareGE:
		return a.GreaterOrEqual(b)
	case compareLT:
		return a.Less(b)
	case compareLE:
		return a.LessOrEqual(b)
	}
	return false
}

// compareOrdinal 按 string.CompareOrdinal 的结果比较。
func compareOrdinal(kind compareKind, r int) bool {
	switch kind {
	case compareEQ:
		return r == 0
	case compareNE:
		return r != 0
	case compareGT:
		return r > 0
	case compareGE:
		return r >= 0
	case compareLT:
		return r < 0
	case compareLE:
		return r <= 0
	}
	return false
}

// writeCompare 输出 `a 运算符 b`。
func writeCompare(sb *strings.Builder, f *BaseFunction, addBrackets bool) {
	if addBrackets {
		sb.WriteByte('(')
	}
	if arg := f.Arg(0); arg != nil {
		arg.WriteTo(sb, false)
	}
	sb.WriteString(" ")
	sb.WriteString(f.Name())
	sb.WriteString(" ")
	if arg := f.Arg(1); arg != nil {
		arg.WriteTo(sb, false)
	}
	if addBrackets {
		sb.WriteByte(')')
	}
}

// compareGetParameterTypes 推导比较右侧字面量类型并传播（对应 C# 各比较类的 GetParameterTypes）。
func compareGetParameterTypes(f *BaseFunction, engine *AlgorithmEngine, result *[]ParameterType) {
	t1 := f.Arg(0).ResultType()
	t2 := f.Arg(1).ResultType()
	if t1 == OperandTypeNone {
		p := engine.Evaluate(f.Arg(1)).ToText("")
		if t2 != OperandTypeError && !p.IsErrorOrNone() {
			f.Arg(0).GetParameterTypes(engine, result, t2, f.Name(), p.TextValue())
			f.Arg(1).GetParameterTypes(engine, result, t2)
			return
		}
	} else if t2 == OperandTypeNone {
		p := engine.Evaluate(f.Arg(0)).ToText("")
		if t1 != OperandTypeError && !p.IsErrorOrNone() {
			f.Arg(1).GetParameterTypes(engine, result, t1, f.Name(), p.TextValue())
			f.Arg(0).GetParameterTypes(engine, result, t1)
			return
		}
	}
	f.Arg(0).GetParameterTypes(engine, result, OperandTypeNone)
	f.Arg(1).GetParameterTypes(engine, result, OperandTypeNone)
}
