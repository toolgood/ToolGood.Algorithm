package datetimes

import "time"

// NETWORKDAYS / WORKDAY / EDATE / EOMONTH
// 对应 csharp/.../DateTimes/Function_NETWORKDAYS.cs 等

// holidayList 等价 C# 的 HashSet<DateTime>（去重 + 保留插入顺序便于遍历）。
type holidayList struct {
	set   map[int64]struct{}
	items []time.Time
}

func newHolidayList() *holidayList {
	return &holidayList{set: map[int64]struct{}{}}
}

func (h *holidayList) add(t time.Time) {
	k := int64(t.Year())*10000 + int64(t.Month())*100 + int64(t.Day())
	if _, ok := h.set[k]; ok {
		return
	}
	h.set[k] = struct{}{}
	h.items = append(h.items, t)
}

func (h *holidayList) contains(t time.Time) bool {
	k := int64(t.Year())*10000 + int64(t.Month())*100 + int64(t.Day())
	_, ok := h.set[k]
	return ok
}

func isWeekend(t time.Time) bool {
	wd := t.Weekday()
	return wd == time.Saturday || wd == time.Sunday
}

// addMonthsDateTime 等价 C# DateTime.AddMonths（目标月天数不足时夹到月末）。
func addMonthsDateTime(t time.Time, months int) time.Time {
	total := int64(t.Year())*12 + int64(int(t.Month())-1) + int64(months)
	y := total / 12
	m := total - y*12
	if m < 0 {
		m += 12
		y--
	}
	day := t.Day()
	if max := daysInMonth(int(y), int(m)+1); day > max {
		day = max
	}
	return time.Date(int(y), time.Month(m+1), day, t.Hour(), t.Minute(), t.Second(), 0, time.UTC)
}

// ------------------------------------------------------------------ NETWORKDAYS

type FunctionNETWORKDAYS struct{ BaseFunction }

func newFunctionNETWORKDAYS(args []Function) Function {
	RequireArgs(args, "NETWORKDAYS", 2, -1)
	return &FunctionNETWORKDAYS{BaseFunction: NewBase("NETWORKDAYS", OperandTypeNumber, args)}
}

func (f *FunctionNETWORKDAYS) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.GetDate(1, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	args2 := f.GetDate(2, engine, tp)
	if args2.IsErrorOrNone() {
		return args2
	}
	startMyDate := dateOnly(args1.DateValue().ToDateTime())
	endMyDate := dateOnly(args2.DateValue().ToDateTime())

	holidays := newHolidayList()
	for i := 3; i <= f.ArgCount(); i++ {
		ar := f.GetDate(i, engine, tp)
		if ar.IsErrorOrNone() {
			return ar
		}
		holidays.add(dateOnly(ar.DateValue().ToDateTime()))
	}

	negative := false
	if startMyDate.After(endMyDate) {
		startMyDate, endMyDate = endMyDate, startMyDate
		negative = true
	}
	// 数学统计工作日: 完整周数 × 5 + 余数逐天, 再减去落在工作日的节假日
	totalDays := wholeDays(startMyDate, endMyDate) + 1
	fullWeeks := totalDays / 7
	remainder := totalDays % 7
	days := fullWeeks * 5
	for i := 0; i < remainder; i++ {
		d := startMyDate.AddDate(0, 0, i)
		if !isWeekend(d) {
			days++
		}
	}
	for _, h := range holidays.items {
		if !h.Before(startMyDate) && !h.After(endMyDate) && !isWeekend(h) {
			days--
		}
	}
	if negative {
		return CreateInt(-days)
	}
	return CreateInt(days)
}

// ------------------------------------------------------------------ WORKDAY

type FunctionWORKDAY struct{ BaseFunction }

func newFunctionWORKDAY(args []Function) Function {
	RequireArgs(args, "WORKDAY", 2, -1)
	return &FunctionWORKDAY{BaseFunction: NewBase("WORKDAY", OperandTypeDate, args)}
}

func (f *FunctionWORKDAY) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.GetDate(1, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	args2 := f.GetNumber(2, engine, tp)
	if args2.IsErrorOrNone() {
		return args2
	}
	days, ok := intValue(args2.NumberValue())
	if !ok {
		return parameterError(f, 2)
	}
	// 归一到当天零点；时间型操作数（年月日为空）按参数错误处理
	d := args1.DateValue()
	if d.Year <= 0 || d.Month <= 0 || d.Day <= 0 {
		return parameterError(f, 1)
	}
	startMyDate := dateOnly(d.ToDateTime())

	holidays := newHolidayList()
	for i := 3; i <= f.ArgCount(); i++ {
		ar := f.GetDate(i, engine, tp)
		if ar.IsErrorOrNone() {
			return ar
		}
		hd := ar.DateValue()
		if hd.Year <= 0 || hd.Month <= 0 || hd.Day <= 0 {
			return parameterError(f, i)
		}
		holidays.add(dateOnly(hd.ToDateTime()))
	}

	cur := startMyDate
	shift := func(n int) bool {
		r, ok := addDaysChecked(cur, n)
		if !ok {
			return false
		}
		cur = r
		return true
	}

	if days > 0 {
		// 先逐天对齐到周一（最多 6 天），期间消耗工作日
		for cur.Weekday() != time.Monday && days > 0 {
			if !shift(1) {
				return parameterError(f, 2)
			}
			if isWeekend(cur) || holidays.contains(cur) {
				continue
			}
			days--
		}
		if days == 0 {
			return CreateMyDate(NewMyDateFromTime(cur))
		}

		// 整周粗跳：起点已是周一，每 5 个工作日 = 7 天
		afterJump, ok := addDaysChecked(cur, (days/5)*7)
		if !ok {
			return parameterError(f, 2)
		}
		extra := 0
		for _, h := range holidays.items {
			if h.After(cur) && !h.After(afterJump) && !isWeekend(h) {
				extra++
			}
		}
		cur = afterJump
		days = days%5 + extra
		for days > 0 {
			if !shift(1) {
				return parameterError(f, 2)
			}
			if isWeekend(cur) || holidays.contains(cur) {
				continue
			}
			days--
		}
	} else if days < 0 {
		// 先逐天对齐到周五（最多 6 天），期间消耗工作日
		for cur.Weekday() != time.Friday && days < 0 {
			if !shift(-1) {
				return parameterError(f, 2)
			}
			if isWeekend(cur) || holidays.contains(cur) {
				continue
			}
			days++
		}
		if days == 0 {
			return CreateMyDate(NewMyDateFromTime(cur))
		}

		// 整周粗跳：起点已是周五，每 5 个工作日 = 7 天
		afterJump, ok := addDaysChecked(cur, (-days/5)*-7)
		if !ok {
			return parameterError(f, 2)
		}
		extra := 0
		for _, h := range holidays.items {
			if !h.Before(afterJump) && h.Before(cur) && !isWeekend(h) {
				extra++
			}
		}
		cur = afterJump
		days = -((-days) % 5) - extra
		for days < 0 {
			if !shift(-1) {
				return parameterError(f, 2)
			}
			if isWeekend(cur) || holidays.contains(cur) {
				continue
			}
			days++
		}
	}
	return CreateMyDate(NewMyDateFromTime(cur))
}

// ------------------------------------------------------------------ EDATE

type FunctionEDATE struct{ BaseFunction }

func newFunctionEDATE(args []Function) Function {
	RequireArgs(args, "EDATE", 2, 2)
	return &FunctionEDATE{BaseFunction: NewBase("EDATE", OperandTypeDate, args)}
}

func (f *FunctionEDATE) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.GetDate(1, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	args2 := f.GetNumber(2, engine, tp)
	if args2.IsErrorOrNone() {
		return args2
	}
	n, ok := intValue(args2.NumberValue())
	if !ok {
		return parameterError(f, 2)
	}
	return CreateMyDate(args1.DateValue().AddMonths(n))
}

// ------------------------------------------------------------------ EOMONTH

type FunctionEOMONTH struct{ BaseFunction }

func newFunctionEOMONTH(args []Function) Function {
	RequireArgs(args, "EOMONTH", 2, 2)
	return &FunctionEOMONTH{BaseFunction: NewBase("EOMONTH", OperandTypeDate, args)}
}

func (f *FunctionEOMONTH) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.GetDate(1, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	args2 := f.GetNumber(2, engine, tp)
	if args2.IsErrorOrNone() {
		return args2
	}
	n, ok := intValue(args2.NumberValue())
	if !ok {
		return parameterError(f, 2)
	}
	dt := addMonthsDateTime(args1.DateValue().ToDateTime(), n+1)
	dt = time.Date(dt.Year(), dt.Month(), 1, 0, 0, 0, 0, time.UTC).AddDate(0, 0, -1)
	return CreateMyDate(NewMyDateFromTime(dt))
}

func init() {
	RegisterFunction("NETWORKDAYS", newFunctionNETWORKDAYS)
	RegisterFunction("WORKDAY", newFunctionWORKDAY)
	RegisterFunction("EDATE", newFunctionEDATE)
	RegisterFunction("EOMONTH", newFunctionEOMONTH)
}
