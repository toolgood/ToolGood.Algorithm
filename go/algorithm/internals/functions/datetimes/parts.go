package datetimes

import (
	"strings"
	"time"
)

// YEAR / MONTH / DAY / HOUR / MINUTE / SECOND
// 对应 csharp/.../DateTimes/Function_YEAR.cs 等

type FunctionYEAR struct{ BaseFunction }

func newFunctionYEAR(args []Function) Function {
	RequireArgs(args, "YEAR", 1, 1)
	return &FunctionYEAR{BaseFunction: NewBase("YEAR", OperandTypeNumber, args)}
}

func (f *FunctionYEAR) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.GetDate(1, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	d := args1.DateValue()
	if d.Year == 0 {
		return parameterError(f, 1)
	}
	return CreateInt(d.Year)
}

type FunctionMONTH struct{ BaseFunction }

func newFunctionMONTH(args []Function) Function {
	RequireArgs(args, "MONTH", 1, 1)
	return &FunctionMONTH{BaseFunction: NewBase("MONTH", OperandTypeNumber, args)}
}

func (f *FunctionMONTH) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.GetDate(1, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	d := args1.DateValue()
	if d.Month == 0 {
		return parameterError(f, 1)
	}
	return CreateInt(d.Month)
}

type FunctionDAY struct{ BaseFunction }

func newFunctionDAY(args []Function) Function {
	RequireArgs(args, "DAY", 1, 1)
	return &FunctionDAY{BaseFunction: NewBase("DAY", OperandTypeNumber, args)}
}

func (f *FunctionDAY) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.GetDate(1, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	d := args1.DateValue()
	if d.Day == 0 {
		return parameterError(f, 1)
	}
	return CreateInt(d.Day)
}

type FunctionHOUR struct{ BaseFunction }

func newFunctionHOUR(args []Function) Function {
	RequireArgs(args, "HOUR", 1, 1)
	return &FunctionHOUR{BaseFunction: NewBase("HOUR", OperandTypeNumber, args)}
}

func (f *FunctionHOUR) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.GetDate(1, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	return CreateInt(args1.DateValue().Hour)
}

type FunctionMINUTE struct{ BaseFunction }

func newFunctionMINUTE(args []Function) Function {
	RequireArgs(args, "MINUTE", 1, 1)
	return &FunctionMINUTE{BaseFunction: NewBase("MINUTE", OperandTypeNumber, args)}
}

func (f *FunctionMINUTE) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.GetDate(1, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	return CreateInt(args1.DateValue().Minute)
}

type FunctionSECOND struct{ BaseFunction }

func newFunctionSECOND(args []Function) Function {
	RequireArgs(args, "SECOND", 1, 1)
	return &FunctionSECOND{BaseFunction: NewBase("SECOND", OperandTypeNumber, args)}
}

func (f *FunctionSECOND) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.GetDate(1, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	return CreateInt(args1.DateValue().Second)
}

// ------------------------------------------------------------------ DATEDIF

type FunctionDATEDIF struct{ BaseFunction }

func newFunctionDATEDIF(args []Function) Function {
	RequireArgs(args, "DATEDIF", 3, 3)
	return &FunctionDATEDIF{BaseFunction: NewBase("DATEDIF", OperandTypeNumber, args)}
}

func (f *FunctionDATEDIF) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.GetDate(1, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	args2 := f.GetDate(2, engine, tp)
	if args2.IsErrorOrNone() {
		return args2
	}
	args3 := f.GetText(3, engine, tp)
	if args3.IsErrorOrNone() {
		return args3
	}
	startMyDate := args1.DateValue().ToDateTime()
	endMyDate := args2.DateValue().ToDateTime()
	t := args3.TextValue()

	switch {
	case strings.EqualFold(t, "Y"):
		b := false
		if startMyDate.Month() < endMyDate.Month() {
			b = true
		} else if startMyDate.Month() == endMyDate.Month() {
			if startMyDate.Day() <= endMyDate.Day() {
				b = true
			}
		}
		if b {
			return CreateInt(endMyDate.Year() - startMyDate.Year())
		}
		return CreateInt(endMyDate.Year() - startMyDate.Year() - 1)

	case strings.EqualFold(t, "M"):
		months := endMyDate.Year()*12 + int(endMyDate.Month()) - startMyDate.Year()*12 - int(startMyDate.Month())
		if startMyDate.Day() <= endMyDate.Day() {
			return CreateInt(months)
		}
		return CreateInt(months - 1)

	case strings.EqualFold(t, "D"):
		return CreateInt(wholeDays(startMyDate, endMyDate))

	case strings.EqualFold(t, "YD"):
		// Excel: 将 end 的月日置于 start 年构造候选日期, 若候选早于 start 则加一年, 再求天数差
		candDay := endMyDate.Day()
		if max := daysInMonth(startMyDate.Year(), int(endMyDate.Month())); candDay > max {
			candDay = max
		}
		cand := time.Date(startMyDate.Year(), endMyDate.Month(), candDay, 0, 0, 0, 0, time.UTC)
		if cand.Before(startMyDate) {
			// start 年为 9999 时无法再加一年, 按超范围处理
			if startMyDate.Year() == 9999 {
				return parameterError(f, 3)
			}
			cand = cand.AddDate(1, 0, 0)
		}
		return CreateInt(wholeDays(startMyDate, cand))

	case strings.EqualFold(t, "MD"):
		mo := endMyDate.Day() - startMyDate.Day()
		if mo < 0 {
			mo += daysInMonth(startMyDate.Year(), int(startMyDate.Month()))
		}
		return CreateInt(mo)

	case strings.EqualFold(t, "YM"):
		mo := int(endMyDate.Month()) - int(startMyDate.Month())
		if endMyDate.Day() < startMyDate.Day() {
			mo--
		}
		if mo < 0 {
			mo += 12
		}
		return CreateInt(mo)
	}
	return parameterError(f, 3)
}

// ------------------------------------------------------------------ DAYS

type FunctionDAYS struct{ BaseFunction }

func newFunctionDAYS(args []Function) Function {
	RequireArgs(args, "DAYS", 2, 2)
	return &FunctionDAYS{BaseFunction: NewBase("DAYS", OperandTypeNumber, args)}
}

func (f *FunctionDAYS) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.GetDate(1, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	args2 := f.GetDate(2, engine, tp)
	if args2.IsErrorOrNone() {
		return args2
	}
	endDate := dateOnly(args1.DateValue().ToDateTime())
	startDate := dateOnly(args2.DateValue().ToDateTime())
	return CreateInt(wholeDays(startDate, endDate))
}

// ------------------------------------------------------------------ DAYS360

type FunctionDAYS360 struct{ BaseFunction }

func newFunctionDAYS360(args []Function) Function {
	RequireArgs(args, "DAYS360", 2, 3)
	return &FunctionDAYS360{BaseFunction: NewBase("DAYS360", OperandTypeNumber, args)}
}

func (f *FunctionDAYS360) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.GetDate(1, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	args2 := f.GetDate(2, engine, tp)
	if args2.IsErrorOrNone() {
		return args2
	}
	startMyDate := args1.DateValue().ToDateTime()
	endMyDate := args2.DateValue().ToDateTime()

	method := false
	if f.ArgCount() > 2 {
		args3 := f.GetBoolean(3, engine, tp)
		if args3.IsErrorOrNone() {
			return args3
		}
		method = args3.BooleanValue()
	}

	days := endMyDate.Year()*360 + (int(endMyDate.Month())-1)*30 -
		startMyDate.Year()*360 - (int(startMyDate.Month())-1)*30
	if method {
		// 欧洲方法: 仅将 31 日按 30 日计算
		ed, sd := endMyDate.Day(), startMyDate.Day()
		if ed > 30 {
			ed = 30
		}
		if sd > 30 {
			sd = 30
		}
		days += ed - sd
	} else {
		// US (NASD) 方法
		startDay := startMyDate.Day()
		endDay := endMyDate.Day()
		if startDay == daysInMonth(startMyDate.Year(), int(startMyDate.Month())) {
			startDay = 30
		}
		if endDay == daysInMonth(endMyDate.Year(), int(endMyDate.Month())) {
			if startDay < 30 {
				endDay = 31
			} else {
				endDay = 30
			}
		}
		days += endDay - startDay
	}
	return CreateInt(days)
}

// ------------------------------------------------------------------ YEARFRAC

type FunctionYEARFRAC struct{ BaseFunction }

func newFunctionYEARFRAC(args []Function) Function {
	RequireArgs(args, "YEARFRAC", 2, 3)
	return &FunctionYEARFRAC{BaseFunction: NewBase("YEARFRAC", OperandTypeNumber, args)}
}

func (f *FunctionYEARFRAC) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	startArg := f.GetDate(1, engine, tp)
	if startArg.IsErrorOrNone() {
		return startArg
	}
	startDate := startArg.DateValue().ToDateTime()

	endArg := f.GetDate(2, engine, tp)
	if endArg.IsErrorOrNone() {
		return endArg
	}
	endDate := endArg.DateValue().ToDateTime()

	basis := 0
	if f.ArgCount() > 2 {
		basisArg := f.GetNumber(3, engine, tp)
		if basisArg.IsErrorOrNone() {
			return basisArg
		}
		v, ok := intValue(basisArg.NumberValue())
		if !ok {
			return parameterError(f, 3)
		}
		basis = v
		if basis < 0 || basis > 4 {
			return parameterError(f, 3)
		}
	}
	return CreateDecimal(calculateYearFrac(startDate, endDate, basis))
}

func calculateYearFrac(startDate, endDate time.Time, basis int) Decimal {
	if startDate.After(endDate) {
		startDate, endDate = endDate, startDate
	}
	switch basis {
	case 0:
		return calculate30_360(startDate, endDate)
	case 1:
		return calculateActualActual(startDate, endDate)
	case 2:
		return totalDays(startDate, endDate).Div(DecFromInt(360))
	case 3:
		return totalDays(startDate, endDate).Div(DecFromInt(365))
	case 4:
		return calculate30_360E(startDate, endDate)
	}
	return calculate30_360(startDate, endDate)
}

// totalDays 等价 C# (a - b).TotalDays。
func totalDays(start, end time.Time) Decimal {
	return DecFromFloat(float64(end.Sub(start)) / float64(24*time.Hour))
}

func calculate30_360(startDate, endDate time.Time) Decimal {
	d1, d2 := startDate.Day(), endDate.Day()
	d1FebLast := int(startDate.Month()) == 2 && d1 == daysInMonth(startDate.Year(), 2)
	d2FebLast := int(endDate.Month()) == 2 && d2 == daysInMonth(endDate.Year(), 2)

	switch {
	case d1 == 31 && d2 == 31:
		d1, d2 = 30, 30
	case d1 == 31:
		d1 = 30
	case d1 == 30 && d2 == 31:
		d2 = 30
	case d1FebLast && d2FebLast:
		d1, d2 = 30, 30
	case d1FebLast:
		d1 = 30
	}

	days := 360*(endDate.Year()-startDate.Year()) +
		30*(int(endDate.Month())-int(startDate.Month())) + (d2 - d1)
	return DecFromInt(int64(days)).Div(DecFromInt(360))
}

func calculate30_360E(startDate, endDate time.Time) Decimal {
	d1, d2 := startDate.Day(), endDate.Day()
	if d1 > 30 {
		d1 = 30
	}
	if d2 > 30 {
		d2 = 30
	}
	days := 360*(endDate.Year()-startDate.Year()) +
		30*(int(endDate.Month())-int(startDate.Month())) + (d2 - d1)
	return DecFromInt(int64(days)).Div(DecFromInt(360))
}

func calculateActualActual(startDate, endDate time.Time) Decimal {
	startYear, endYear := startDate.Year(), endDate.Year()

	if startYear == endYear {
		daysInYear := int64(365)
		if isLeapYear(startYear) {
			daysInYear = 366
		}
		return totalDays(startDate, endDate).Div(DecFromInt(daysInYear))
	}

	daysInStartYear := int64(365)
	if isLeapYear(startYear) {
		daysInStartYear = 366
	}
	daysInEndYear := int64(365)
	if isLeapYear(endYear) {
		daysInEndYear = 366
	}

	result := totalDays(startDate, time.Date(startYear, 12, 31, 0, 0, 0, 0, time.UTC)).Div(DecFromInt(daysInStartYear))
	result = result.Add(totalDays(time.Date(endYear, 1, 1, 0, 0, 0, 0, time.UTC), endDate).Div(DecFromInt(daysInEndYear)))
	return result.Add(DecFromInt(int64(endYear - startYear - 1)))
}

func isLeapYear(year int) bool {
	return year%4 == 0 && (year%100 != 0 || year%400 == 0)
}

// ------------------------------------------------------------------ WEEKDAY

type FunctionWEEKDAY struct{ BaseFunction }

func newFunctionWEEKDAY(args []Function) Function {
	RequireArgs(args, "WEEKDAY", 1, 2)
	return &FunctionWEEKDAY{BaseFunction: NewBase("WEEKDAY", OperandTypeNumber, args)}
}

func (f *FunctionWEEKDAY) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.GetDate(1, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	tp2 := 1
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
		if tp2 != 1 && tp2 != 2 && tp2 != 3 && (tp2 < 11 || tp2 > 17) {
			return parameterError(f, 2)
		}
	}

	dayOfWeek := int(args1.DateValue().ToDateTime().Weekday())

	switch tp2 {
	case 1, 17:
		return CreateInt(dayOfWeek + 1)
	case 2, 11:
		if dayOfWeek == 0 {
			return CreateInt(7)
		}
		return CreateInt(dayOfWeek)
	case 3:
		if dayOfWeek == 0 {
			return CreateInt(6)
		}
		return CreateInt(dayOfWeek - 1)
	case 12:
		mapping := [7]int{6, 7, 1, 2, 3, 4, 5}
		return CreateInt(mapping[dayOfWeek])
	case 13:
		mapping := [7]int{5, 6, 7, 1, 2, 3, 4}
		return CreateInt(mapping[dayOfWeek])
	case 14:
		mapping := [7]int{4, 5, 6, 7, 1, 2, 3}
		return CreateInt(mapping[dayOfWeek])
	case 15:
		mapping := [7]int{3, 4, 5, 6, 7, 1, 2}
		return CreateInt(mapping[dayOfWeek])
	case 16:
		mapping := [7]int{2, 3, 4, 5, 6, 7, 1}
		return CreateInt(mapping[dayOfWeek])
	}
	return parameterError(f, 2)
}

// ------------------------------------------------------------------ WEEKNUM

type FunctionWEEKNUM struct{ BaseFunction }

func newFunctionWEEKNUM(args []Function) Function {
	RequireArgs(args, "WEEKNUM", 1, 2)
	return &FunctionWEEKNUM{BaseFunction: NewBase("WEEKNUM", OperandTypeNumber, args)}
}

func (f *FunctionWEEKNUM) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.GetDate(1, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	startMyDate := args1.DateValue().ToDateTime()

	returnType := 1
	if f.ArgCount() > 1 {
		args2 := f.GetNumber(2, engine, tp)
		if args2.IsErrorOrNone() {
			return args2
		}
		v, ok := intValue(args2.NumberValue())
		if !ok {
			return parameterError(f, 2)
		}
		returnType = v
		if returnType != 1 && returnType != 2 && returnType != 11 && returnType != 12 && returnType != 13 &&
			returnType != 14 && returnType != 15 && returnType != 16 && returnType != 17 && returnType != 21 {
			return parameterError(f, 2)
		}
	}

	if returnType == 21 {
		// ISO 8601: 第1周是包含当年第一个周四的周
		isoDow := int(startMyDate.Weekday())
		if isoDow == 0 {
			isoDow = 7
		}
		thursday := startMyDate.AddDate(0, 0, 4-isoDow)
		weekNumber := (thursday.YearDay()-1)/7 + 1
		return CreateInt(weekNumber)
	}

	jan1 := time.Date(startMyDate.Year(), 1, 1, 0, 0, 0, 0, time.UTC)
	dayOfYear := startMyDate.YearDay()
	dayOfWeekJan1 := int(jan1.Weekday())

	var weekStartDay int
	switch returnType {
	case 1, 17:
		weekStartDay = 0
	case 2, 11:
		weekStartDay = 1
	case 12:
		weekStartDay = 2
	case 13:
		weekStartDay = 3
	case 14:
		weekStartDay = 4
	case 15:
		weekStartDay = 5
	default:
		weekStartDay = 6
	}

	daysUntilWeekStart := (dayOfWeekJan1 - weekStartDay + 7) % 7
	adjustedDayOfYear := dayOfYear + daysUntilWeekStart
	week := (adjustedDayOfYear + 6) / 7

	return CreateInt(week)
}

func init() {
	RegisterFunction("YEAR", newFunctionYEAR)
	RegisterFunction("MONTH", newFunctionMONTH)
	RegisterFunction("DAY", newFunctionDAY)
	RegisterFunction("HOUR", newFunctionHOUR)
	RegisterFunction("MINUTE", newFunctionMINUTE)
	RegisterFunction("SECOND", newFunctionSECOND)
	RegisterFunction("DATEDIF", newFunctionDATEDIF)
	RegisterFunction("DAYS", newFunctionDAYS)
	RegisterFunction("DAYS360", newFunctionDAYS360)
	RegisterFunction("YEARFRAC", newFunctionYEARFRAC)
	RegisterFunction("WEEKDAY", newFunctionWEEKDAY)
	RegisterFunction("WEEKNUM", newFunctionWEEKNUM)
}
