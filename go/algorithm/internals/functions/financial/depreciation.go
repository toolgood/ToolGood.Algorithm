package financial

// SLN / SYD / DB / DDB
// 对应 csharp/ToolGood.Algorithm/Internals/Functions/Financial/Function_SLN.cs 等

type FunctionSLN struct{ BaseFunction }

func newFunctionSLN(args []Function) Function {
	RequireArgs(args, "SLN", 3, 3)
	return &FunctionSLN{BaseFunction: NewBase("SLN", OperandTypeNumber, args)}
}

func (f *FunctionSLN) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	costArg := f.GetNumber(1, engine, tp)
	if costArg.IsErrorOrNone() {
		return costArg
	}
	cost := costArg.NumberValue()

	salvageArg := f.GetNumber(2, engine, tp)
	if salvageArg.IsErrorOrNone() {
		return salvageArg
	}
	salvage := salvageArg.NumberValue()

	lifeArg := f.GetNumber(3, engine, tp)
	if lifeArg.IsErrorOrNone() {
		return lifeArg
	}
	life := lifeArg.NumberValue()

	if life.IsZero() {
		return div0Error(f)
	}
	if life.Sign() < 0 {
		return parameterError(f, 3)
	}
	return CreateDecimal(cost.Sub(salvage).Div(life))
}

type FunctionSYD struct{ BaseFunction }

func newFunctionSYD(args []Function) Function {
	RequireArgs(args, "SYD", 4, 4)
	return &FunctionSYD{BaseFunction: NewBase("SYD", OperandTypeNumber, args)}
}

func (f *FunctionSYD) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	costArg := f.GetNumber(1, engine, tp)
	if costArg.IsErrorOrNone() {
		return costArg
	}
	cost := costArg.NumberValue()

	salvageArg := f.GetNumber(2, engine, tp)
	if salvageArg.IsErrorOrNone() {
		return salvageArg
	}
	salvage := salvageArg.NumberValue()

	lifeArg := f.GetNumber(3, engine, tp)
	if lifeArg.IsErrorOrNone() {
		return lifeArg
	}
	life := lifeArg.NumberValue()

	periodArg := f.GetNumber(4, engine, tp)
	if periodArg.IsErrorOrNone() {
		return periodArg
	}
	period := periodArg.NumberValue()

	if life.IsZero() {
		return div0Error(f)
	}
	if life.Sign() <= 0 {
		return parameterError(f, 3)
	}
	if period.Less(DecimalOne) || period.Greater(life) {
		return parameterError(f, 4)
	}

	syd := cost.Sub(salvage).Mul(life.Sub(period).Add(DecimalOne)).Mul(DecFromInt(2)).
		Div(life.Mul(life.Add(DecimalOne)))
	return CreateDecimal(syd)
}

// ------------------------------------------------------------------ DB

type FunctionDB struct{ BaseFunction }

func newFunctionDB(args []Function) Function {
	RequireArgs(args, "DB", 4, 5)
	return &FunctionDB{BaseFunction: NewBase("DB", OperandTypeNumber, args)}
}

func (f *FunctionDB) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	costArg := f.GetNumber(1, engine, tp)
	if costArg.IsErrorOrNone() {
		return costArg
	}
	cost := costArg.NumberValue()

	salvageArg := f.GetNumber(2, engine, tp)
	if salvageArg.IsErrorOrNone() {
		return salvageArg
	}
	salvage := salvageArg.NumberValue()

	lifeArg := f.GetNumber(3, engine, tp)
	if lifeArg.IsErrorOrNone() {
		return lifeArg
	}
	life := lifeArg.NumberValue()

	periodArg := f.GetNumber(4, engine, tp)
	if periodArg.IsErrorOrNone() {
		return periodArg
	}
	period := periodArg.NumberValue()

	month := 12
	if f.ArgCount() > 4 {
		monthArg := f.GetNumber(5, engine, tp)
		if monthArg.IsErrorOrNone() {
			return monthArg
		}
		monthValue := monthArg.NumberValue()
		if monthValue.Less(DecimalOne) || monthValue.Greater(DecFromInt(12)) {
			return parameterError(f, 5)
		}
		month = int(monthValue.Int64())
	}

	if life.IsZero() || cost.IsZero() {
		return div0Error(f)
	}
	if life.Less(DecimalOne) || life.Greater(decMaxInt32) {
		return parameterError(f, 3)
	}

	// Excel: month<12 时折旧跨越 life+1 个期间
	totalPeriods := int(life.Int64())
	if month != 12 {
		totalPeriods = int(life.Int64()) + 1
	}
	if period.Less(DecimalOne) || period.Greater(DecFromInt(int64(totalPeriods))) {
		return parameterError(f, 4)
	}

	var rate Decimal
	if salvage.IsZero() {
		// Pow(0, 正指数) = 0, 即残值为 0 时折旧率为 1
		rate = DecimalOne
	} else {
		factor, ok := tryPowPositiveBase(salvage.Div(cost), DecimalOne.Div(life))
		if !ok {
			return numError(f)
		}
		rate = DecimalOne.Sub(factor)
	}
	// C# Math.Round(decimal, 3) 默认银行家舍入
	rate = rate.RoundHalfEven(3)

	remainingCost := cost
	depreciation := DecimalZero
	monthDec := DecFromInt(int64(month))
	yearDec := DecFromInt(12)
	for i := 1; !DecFromInt(int64(i)).Greater(period); i++ {
		if i == 1 {
			depreciation = cost.Mul(rate).Mul(monthDec).Div(yearDec)
		} else if i == totalPeriods && month != 12 {
			depreciation = remainingCost.Mul(rate).Mul(DecFromInt(int64(12 - month))).Div(yearDec)
		} else {
			depreciation = remainingCost.Mul(rate)
		}
		remainingCost = remainingCost.Sub(depreciation)
	}
	return CreateDecimal(depreciation)
}

// ------------------------------------------------------------------ DDB

type FunctionDDB struct{ BaseFunction }

func newFunctionDDB(args []Function) Function {
	RequireArgs(args, "DDB", 4, 5)
	return &FunctionDDB{BaseFunction: NewBase("DDB", OperandTypeNumber, args)}
}

func (f *FunctionDDB) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	costArg := f.GetNumber(1, engine, tp)
	if costArg.IsErrorOrNone() {
		return costArg
	}
	cost := costArg.NumberValue()

	salvageArg := f.GetNumber(2, engine, tp)
	if salvageArg.IsErrorOrNone() {
		return salvageArg
	}
	salvage := salvageArg.NumberValue()

	lifeArg := f.GetNumber(3, engine, tp)
	if lifeArg.IsErrorOrNone() {
		return lifeArg
	}
	life := lifeArg.NumberValue()

	periodArg := f.GetNumber(4, engine, tp)
	if periodArg.IsErrorOrNone() {
		return periodArg
	}
	period := periodArg.NumberValue()

	factor := DecFromInt(2)
	if f.ArgCount() > 4 {
		factorArg := f.GetNumber(5, engine, tp)
		if factorArg.IsErrorOrNone() {
			return factorArg
		}
		factor = factorArg.NumberValue()
	}

	if factor.Sign() <= 0 {
		return parameterError(f, 5)
	}
	if life.IsZero() {
		return div0Error(f)
	}
	if life.Sign() < 0 {
		return parameterError(f, 3)
	}
	if period.Less(DecimalOne) || period.Greater(life) {
		return parameterError(f, 4)
	}

	remainingCost := cost

	// 累计 period 之前各期的折旧
	for i := 1; DecFromInt(int64(i)).Less(period); i++ {
		ddb := remainingCost.Mul(factor).Div(life)
		maxDepreciation := remainingCost.Sub(salvage)
		if ddb.Greater(maxDepreciation) {
			ddb = maxDepreciation
		}
		remainingCost = remainingCost.Sub(ddb)
		if remainingCost.LessOrEqual(salvage) {
			break
		}
	}

	// 计算当前期间(period)的折旧, period 可为小数
	depreciation := remainingCost.Mul(factor).Div(life)
	maxDep := remainingCost.Sub(salvage)
	if depreciation.Greater(maxDep) {
		depreciation = maxDep
	}
	return CreateDecimal(depreciation)
}

func init() {
	RegisterFunction("SLN", newFunctionSLN)
	RegisterFunction("SYD", newFunctionSYD)
	RegisterFunction("DB", newFunctionDB)
	RegisterFunction("DDB", newFunctionDDB)
}
