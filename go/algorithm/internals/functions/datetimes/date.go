package datetimes

import "time"

// DATE / DATEVALUE / TIME / TIMEVALUE / TIMESTAMP / NOW / TODAY
// 对应 csharp/ToolGood.Algorithm/Internals/Functions/DateTimes/Function_DATE*.cs、
// Function_TIME*.cs、Function_NOW.cs、Function_TODAY.cs

type FunctionDATE struct{ BaseFunction }

func newFunctionDATE(args []Function) Function {
	RequireArgs(args, "DATE", 3, 6)
	return &FunctionDATE{BaseFunction: NewBase("DATE", OperandTypeDate, args)}
}

func (f *FunctionDATE) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	yearArg := f.GetNumber(1, engine, tp)
	if yearArg.IsErrorOrNone() {
		return yearArg
	}
	monthArg := f.GetNumber(2, engine, tp)
	if monthArg.IsErrorOrNone() {
		return monthArg
	}
	dayArg := f.GetNumber(3, engine, tp)
	if dayArg.IsErrorOrNone() {
		return dayArg
	}
	year, ok := intValue(yearArg.NumberValue())
	if !ok {
		return parameterError(f, 1)
	}
	month, ok := intValue(monthArg.NumberValue())
	if !ok {
		return parameterError(f, 2)
	}
	day, ok := intValue(dayArg.NumberValue())
	if !ok {
		return parameterError(f, 3)
	}

	// new DateTime(year, 1, 1).AddMonths(month - 1).AddDays(day - 1)
	if year < 1 || year > 9999 {
		return parameterError(f, 1)
	}
	total := int64(year)*12 + int64(month) - 1
	y := total / 12
	m := total - y*12
	if m < 0 {
		m += 12
		y--
	}
	if y < 1 || y > 9999 {
		return parameterError(f, 1)
	}
	baseDate := time.Date(int(y), time.Month(m+1), 1, 0, 0, 0, 0, time.UTC)
	baseDate = baseDate.AddDate(0, 0, day-1)
	if !validDate(baseDate) {
		return parameterError(f, 1)
	}

	hour, minute, second := 0, 0, 0
	if f.ArgCount() > 3 {
		arg := f.GetNumber(4, engine, tp)
		if arg.IsErrorOrNone() {
			return arg
		}
		v, ok := intValue(arg.NumberValue())
		if !ok || v < 0 || v > 23 {
			return parameterError(f, 4)
		}
		hour = v
	}
	if f.ArgCount() > 4 {
		arg := f.GetNumber(5, engine, tp)
		if arg.IsErrorOrNone() {
			return arg
		}
		v, ok := intValue(arg.NumberValue())
		if !ok || v < 0 || v > 59 {
			return parameterError(f, 5)
		}
		minute = v
	}
	if f.ArgCount() > 5 {
		arg := f.GetNumber(6, engine, tp)
		if arg.IsErrorOrNone() {
			return arg
		}
		v, ok := intValue(arg.NumberValue())
		if !ok || v < 0 || v > 59 {
			return parameterError(f, 6)
		}
		second = v
	}

	return CreateMyDate(NewMyDate(baseDate.Year(), int(baseDate.Month()), baseDate.Day(), hour, minute, second))
}

// ------------------------------------------------------------------ DATEVALUE

var (
	decMaxOADate     = MustDecimal("2958465.99999999")
	decMinOADate     = MustDecimal("-657435")
	decMaxUnixSecond = MustDecimal("253402300800")
	decMaxUnixMilli  = MustDecimal("253402300800000")
)

type FunctionDATEVALUE struct{ BaseFunction }

func newFunctionDATEVALUE(args []Function) Function {
	RequireArgs(args, "DATEVALUE", 1, 2)
	return &FunctionDATEVALUE{BaseFunction: NewBase("DATEVALUE", OperandTypeDate, args)}
}

func (f *FunctionDATEVALUE) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.EvalArg(0, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}

	tp2 := 0
	if f.ArgCount() > 1 {
		args2 := f.GetNumber(2, engine, tp)
		if args2.IsErrorOrNone() {
			return args2
		}
		v, ok := intValue(args2.NumberValue())
		if !ok {
			return parameterError(f, 2)
		}
		tp2 = v
	}
	// 先校验第 2 参数，避免非法 type 被 args1 是日期的短路逻辑静默忽略
	if tp2 < 0 || tp2 > 4 {
		return parameterError(f, 2)
	}
	if args1.IsDate() {
		return args1
	}

	switch tp2 {
	case 0:
		if args1.IsText() {
			if d := ParseMyDate(args1.TextValue()); d != nil {
				return CreateMyDate(d)
			}
		}
		arg1 := f.ConvertToNumber(args1, 1)
		if arg1.IsErrorOrNone() {
			return arg1
		}
		v := arg1.NumberValue()
		if v.LessOrEqual(decMaxOADate) {
			if v.LessOrEqual(decMinOADate) {
				return parameterError(f, 1)
			}
			d, ok := fromOADate(v)
			if !ok {
				return parameterError(f, 1)
			}
			return CreateMyDate(d)
		}
		// Unix 时间戳（秒）
		if v.Less(decMaxUnixSecond) {
			return f.fromUnix(arg1, engine, false)
		}
		// Unix 时间戳（毫秒）
		if v.GreaterOrEqual(decMaxUnixMilli) {
			return parameterError(f, 1)
		}
		return f.fromUnix(arg1, engine, true)
	case 1:
		arg1 := f.ConvertToText(args1, 1)
		if arg1.IsErrorOrNone() {
			return arg1
		}
		if d := ParseMyDate(arg1.TextValue()); d != nil {
			return CreateMyDate(d)
		}
		return parameterError(f, 1)
	case 2:
		arg1 := f.ConvertToNumber(args1, 1)
		if arg1.IsErrorOrNone() {
			return arg1
		}
		v := arg1.NumberValue()
		if v.LessOrEqual(decMinOADate) || v.Greater(decMaxOADate) {
			return parameterError(f, 1)
		}
		d, ok := fromOADate(v)
		if !ok {
			return parameterError(f, 1)
		}
		return CreateMyDate(d)
	case 3:
		arg1 := f.ConvertToNumber(args1, 1)
		if arg1.IsErrorOrNone() {
			return arg1
		}
		return f.fromUnix(arg1, engine, true)
	case 4:
		arg1 := f.ConvertToNumber(args1, 1)
		if arg1.IsErrorOrNone() {
			return arg1
		}
		return f.fromUnix(arg1, engine, false)
	}
	return parameterError(f, 2)
}

// fromUnix 把数字按 Unix 秒/毫秒解释；越界返回参数错误。
func (f *FunctionDATEVALUE) fromUnix(arg Operand, engine *AlgorithmEngine, milli bool) Operand {
	n, ok := int64Value(arg.NumberValue())
	if !ok {
		return parameterError(f, 1)
	}
	if !milli {
		// 先按 DateTime 合法范围（0001-01-01 ~ 9999-12-31）校验，避免乘 1000 溢出
		if n < -62135596800 || n > 253402300799 {
			return parameterError(f, 1)
		}
		n *= 1000
	} else if n < -62135596800000 || n > 253402300799999 {
		return parameterError(f, 1)
	}
	d, ok := myDateFromUnixMilli(n, engine.UseLocalTime)
	if !ok {
		return parameterError(f, 1)
	}
	return CreateMyDate(d)
}

// ------------------------------------------------------------------ TIME

type FunctionTIME struct{ BaseFunction }

func newFunctionTIME(args []Function) Function {
	RequireArgs(args, "TIME", 2, 3)
	return &FunctionTIME{BaseFunction: NewBase("TIME", OperandTypeDate, args)}
}

func (f *FunctionTIME) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.GetNumber(1, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	args2 := f.GetNumber(2, engine, tp)
	if args2.IsErrorOrNone() {
		return args2
	}
	hour, ok := intValue(args1.NumberValue())
	if !ok {
		return parameterError(f, 1)
	}
	minute, ok := intValue(args2.NumberValue())
	if !ok {
		return parameterError(f, 2)
	}
	if hour < 0 || hour > 23 {
		return parameterError(f, 1)
	}
	if minute < 0 || minute > 59 {
		return parameterError(f, 2)
	}
	second := 0
	if f.ArgCount() > 2 {
		args3 := f.GetNumber(3, engine, tp)
		if args3.IsErrorOrNone() {
			return args3
		}
		v, ok := intValue(args3.NumberValue())
		if !ok || v < 0 || v > 59 {
			return parameterError(f, 3)
		}
		second = v
	}
	return CreateMyDate(NewMyDate(0, 0, 0, hour, minute, second))
}

// ------------------------------------------------------------------ TIMEVALUE

type FunctionTIMEVALUE struct{ BaseFunction }

func newFunctionTIMEVALUE(args []Function) Function {
	RequireArgs(args, "TIMEVALUE", 1, 1)
	return &FunctionTIMEVALUE{BaseFunction: NewBase("TIMEVALUE", OperandTypeDate, args)}
}

func (f *FunctionTIMEVALUE) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.GetText(1, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	if d, ok := parseTimeSpan(args1.TextValue()); ok {
		return CreateMyDate(d)
	}
	return parameterError(f, 1)
}

// ------------------------------------------------------------------ TIMESTAMP

type FunctionTIMESTAMP struct{ BaseFunction }

func newFunctionTIMESTAMP(args []Function) Function {
	RequireArgs(args, "TIMESTAMP", 1, 2)
	return &FunctionTIMESTAMP{BaseFunction: NewBase("TIMESTAMP", OperandTypeNumber, args)}
}

func (f *FunctionTIMESTAMP) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	tp2 := 0
	if f.ArgCount() > 1 {
		args2 := f.GetNumber(2, engine, tp)
		if args2.IsErrorOrNone() {
			return args2
		}
		v, ok := intValue(args2.NumberValue())
		if !ok {
			return parameterError(f, 2)
		}
		tp2 = v
	}
	args0 := f.GetDate(1, engine, tp)
	if args0.IsErrorOrNone() {
		return args0
	}
	base := args0.DateValue().ToDateTime()
	loc := time.UTC
	if engine.UseLocalTime {
		loc = time.Local
	}
	t := time.Date(base.Year(), base.Month(), base.Day(), base.Hour(), base.Minute(), base.Second(), 0, loc)
	ms := t.UnixMilli()

	switch tp2 {
	case 0:
		return CreateFloat(float64(ms))
	case 1:
		return CreateFloat(float64(ms) / 1000.0)
	}
	return parameterError(f, 2)
}

// ------------------------------------------------------------------ NOW / TODAY

type FunctionNOW struct{ BaseFunction }

func newFunctionNOW(args []Function) Function {
	RequireArgs(args, "NOW", 0, 0)
	return &FunctionNOW{BaseFunction: NewBase("NOW", OperandTypeDate, args)}
}

func (f *FunctionNOW) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	now := time.Now()
	if !engine.UseLocalTime {
		now = now.UTC()
	}
	return CreateMyDate(NewMyDateFromTime(now))
}

type FunctionTODAY struct{ BaseFunction }

func newFunctionTODAY(args []Function) Function {
	RequireArgs(args, "TODAY", 0, 0)
	return &FunctionTODAY{BaseFunction: NewBase("TODAY", OperandTypeDate, args)}
}

func (f *FunctionTODAY) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	now := time.Now()
	if !engine.UseLocalTime {
		now = now.UTC()
	}
	return CreateMyDate(NewMyDate(now.Year(), int(now.Month()), now.Day(), 0, 0, 0))
}

func init() {
	RegisterFunction("DATE", newFunctionDATE)
	RegisterFunction("DATEVALUE", newFunctionDATEVALUE)
	RegisterFunction("TIME", newFunctionTIME)
	RegisterFunction("TIMEVALUE", newFunctionTIMEVALUE)
	RegisterFunction("TIMESTAMP", newFunctionTIMESTAMP)
	RegisterFunction("NOW", newFunctionNOW)
	RegisterFunction("TODAY", newFunctionTODAY)
}
