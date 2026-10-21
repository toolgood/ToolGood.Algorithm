package financial

import "time"

// NPV / IRR / MIRR / XIRR / XNPV
// 对应 csharp/ToolGood.Algorithm/Internals/Functions/Financial/Function_NPV.cs 等

type FunctionNPV struct{ BaseFunction }

func newFunctionNPV(args []Function) Function {
	RequireArgs(args, "NPV", 2, -1)
	return &FunctionNPV{BaseFunction: NewBase("NPV", OperandTypeNumber, args)}
}

func (f *FunctionNPV) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	rateArg := f.GetNumber(1, engine, tp)
	if rateArg.IsErrorOrNone() {
		return rateArg
	}
	rate := rateArg.NumberValue()
	if rate.Equal(DecimalOne.Neg()) {
		return div0Error(f)
	}

	values := make([]Decimal, 0, f.ArgCount()-1)
	for i := 2; i <= f.ArgCount(); i++ {
		arg := f.GetNumber(i, engine, tp)
		if arg.IsErrorOrNone() {
			return arg
		}
		values = append(values, arg.NumberValue())
	}

	base := toPrecision(DecimalOne.Add(rate), 34)
	npv := DecimalZero
	for i, value := range values {
		npv = npv.Add(value.Div(toPrecision(base.Pow(i+1), 34)))
	}
	return CreateDecimal(npv)
}

// ------------------------------------------------------------------ IRR

type FunctionIRR struct{ BaseFunction }

func newFunctionIRR(args []Function) Function {
	RequireArgs(args, "IRR", 1, 2)
	return &FunctionIRR{BaseFunction: NewBase("IRR", OperandTypeNumber, args)}
}

func (f *FunctionIRR) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	valuesArg := f.GetArray(1, engine, tp)
	if valuesArg.IsErrorOrNone() {
		return valuesArg
	}
	values, err := numbersFromArray(valuesArg, "IRR", 1)
	if err != nil {
		return err
	}

	if len(values) == 0 {
		return parameterError(f, 1)
	}
	hasPositive, hasNegative := false, false
	for _, v := range values {
		if v.Sign() > 0 {
			hasPositive = true
		}
		if v.Sign() < 0 {
			hasNegative = true
		}
	}
	if !hasPositive || !hasNegative {
		return parameterError(f, 1)
	}

	guess := decDefaultGuess
	if f.ArgCount() > 1 {
		guessArg := f.GetNumber(2, engine, tp)
		if guessArg.IsErrorOrNone() {
			return guessArg
		}
		guess = guessArg.NumberValue()
	}

	irr, ok := newtonRaphsonIRR(values, guess)
	if !ok {
		return functionError(f)
	}
	return CreateDecimal(irr)
}

// newtonRaphsonIRR 与 C# IRR.NewtonRaphsonIRR 一致。
func newtonRaphsonIRR(values []Decimal, guess Decimal) (Decimal, bool) {
	rate := guess
	for iter := 0; iter < 100; iter++ {
		npv := DecimalZero
		dnpv := DecimalZero
		onePlusRate := DecimalOne.Add(rate)
		if onePlusRate.IsZero() {
			return DecimalZero, false
		}
		factor := DecimalOne
		for i, value := range values {
			npv = npv.Add(value.Div(factor))
			dnpv = dnpv.Sub(DecFromInt(int64(i)).Mul(value).Div(factor.Mul(onePlusRate)))
			factor = toPrecision(factor.Mul(onePlusRate), 34)
		}
		if dnpv.Abs().Less(decEpsSlope) {
			break
		}
		newRate := rate.Sub(npv.Div(dnpv))
		if newRate.Sub(rate).Abs().Less(decEpsStep) {
			return newRate, true
		}
		rate = newRate
	}
	return DecimalZero, false
}

// ------------------------------------------------------------------ MIRR

type FunctionMIRR struct{ BaseFunction }

func newFunctionMIRR(args []Function) Function {
	RequireArgs(args, "MIRR", 3, 3)
	return &FunctionMIRR{BaseFunction: NewBase("MIRR", OperandTypeNumber, args)}
}

func (f *FunctionMIRR) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	valuesArg := f.GetArray(1, engine, tp)
	if valuesArg.IsErrorOrNone() {
		return valuesArg
	}
	values, err := numbersFromArray(valuesArg, "MIRR", 1)
	if err != nil {
		return err
	}

	financeRateArg := f.GetNumber(2, engine, tp)
	if financeRateArg.IsErrorOrNone() {
		return financeRateArg
	}
	financeRate := financeRateArg.NumberValue()

	reinvestRateArg := f.GetNumber(3, engine, tp)
	if reinvestRateArg.IsErrorOrNone() {
		return reinvestRateArg
	}
	reinvestRate := reinvestRateArg.NumberValue()

	n := len(values)
	if n == 0 {
		return parameterError(f, 1)
	}
	if n == 1 {
		return div0Error(f)
	}

	npvNegative := DecimalZero
	npvPositive := DecimalZero
	for i, value := range values {
		if value.Sign() < 0 {
			discountFactor, ok := tryPowPositiveBase(DecimalOne.Add(financeRate), DecFromInt(int64(i)))
			if !ok {
				return numError(f)
			}
			npvNegative = npvNegative.Add(value.Div(discountFactor))
		} else {
			compoundFactor, ok := tryPowPositiveBase(DecimalOne.Add(reinvestRate), DecFromInt(int64(n-1-i)))
			if !ok {
				return numError(f)
			}
			npvPositive = npvPositive.Add(value.Mul(compoundFactor))
		}
	}

	if npvNegative.IsZero() || npvPositive.IsZero() {
		return div0Error(f)
	}

	exponent := DecimalOne.Div(DecFromInt(int64(n - 1)))
	ratio, ok := tryPowPositiveBase(npvPositive.Neg().Div(npvNegative), exponent)
	if !ok {
		return numError(f)
	}
	return CreateDecimal(ratio.Sub(DecimalOne))
}

// ------------------------------------------------------------------ XIRR

type FunctionXIRR struct{ BaseFunction }

func newFunctionXIRR(args []Function) Function {
	RequireArgs(args, "XIRR", 2, 3)
	return &FunctionXIRR{BaseFunction: NewBase("XIRR", OperandTypeNumber, args)}
}

func (f *FunctionXIRR) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	valuesArg := f.GetArray(1, engine, tp)
	if valuesArg.IsErrorOrNone() {
		return valuesArg
	}
	values, err := numbersFromArray(valuesArg, "XIRR", 1)
	if err != nil {
		return err
	}

	datesArg := f.GetArray(2, engine, tp)
	if datesArg.IsErrorOrNone() {
		return datesArg
	}
	mdates := datesFromArray(datesArg)
	if mdates == nil {
		return parameterError(f, 2)
	}

	if len(values) != len(mdates) || len(values) < 2 {
		return functionError(f)
	}

	guess := decDefaultGuess
	if f.ArgCount() > 2 {
		guessArg := f.GetNumber(3, engine, tp)
		if guessArg.IsErrorOrNone() {
			return guessArg
		}
		guess = guessArg.NumberValue()
	}

	xirr, ok := newtonRaphsonXIRR(values, mdates, guess)
	if !ok {
		return functionError(f)
	}
	return CreateDecimal(xirr)
}

// newtonRaphsonXIRR 与 C# XIRR.NewtonRaphsonXIRR 一致。
func newtonRaphsonXIRR(values []Decimal, dates []*MyDate, rate Decimal) (Decimal, bool) {
	baseDate := dates[0].ToDateTime()
	exps := make([]Decimal, len(values))
	for i := range values {
		exps[i] = totalDays(baseDate, dates[i].ToDateTime()).Div(decDaysInYear)
	}

	for iter := 0; iter < 100; iter++ {
		npv := DecimalZero
		dnpv := DecimalZero
		onePlusRate := DecimalOne.Add(rate)
		logBase, ok := decLn(onePlusRate)
		if !ok {
			return DecimalZero, false
		}
		for i := range values {
			factor, ok := decExp(exps[i].Mul(logBase))
			if !ok || factor.IsZero() {
				return DecimalZero, false
			}
			npv = npv.Add(values[i].Div(factor))
			dnpv = dnpv.Sub(values[i].Mul(exps[i]).Div(factor.Mul(onePlusRate)))
		}
		if dnpv.Abs().Less(decEpsSlope) {
			break
		}
		newRate := rate.Sub(npv.Div(dnpv))
		if newRate.Sub(rate).Abs().Less(decEpsStep) {
			return newRate, true
		}
		rate = newRate
	}
	return DecimalZero, false
}

// ------------------------------------------------------------------ XNPV

type FunctionXNPV struct{ BaseFunction }

func newFunctionXNPV(args []Function) Function {
	RequireArgs(args, "XNPV", 3, 3)
	return &FunctionXNPV{BaseFunction: NewBase("XNPV", OperandTypeNumber, args)}
}

func (f *FunctionXNPV) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	rateArg := f.GetNumber(1, engine, tp)
	if rateArg.IsErrorOrNone() {
		return rateArg
	}
	rate := rateArg.NumberValue()
	if rate.Equal(DecimalOne.Neg()) {
		return div0Error(f)
	}

	valuesArg := f.GetArray(2, engine, tp)
	if valuesArg.IsErrorOrNone() {
		return valuesArg
	}
	values, err := numbersFromArray(valuesArg, "XNPV", 2)
	if err != nil {
		return err
	}

	datesArg := f.GetArray(3, engine, tp)
	if datesArg.IsErrorOrNone() {
		return datesArg
	}

	if len(values) != len(datesArg.ArrayValue()) {
		return functionError(f)
	}
	if len(values) == 0 {
		return parameterError(f, 2)
	}

	mdates := datesFromArray(datesArg)
	if mdates == nil {
		return parameterError(f, 3)
	}

	baseDate := mdates[0].ToDateTime()
	xnpv := DecimalZero
	for i := range values {
		days := totalDays(baseDate, mdates[i].ToDateTime())
		factor, ok := tryPowPositiveBase(DecimalOne.Add(rate), days.Div(decDaysInYear))
		if !ok {
			return numError(f)
		}
		xnpv = xnpv.Add(values[i].Div(factor))
	}
	return CreateDecimal(xnpv)
}

// totalDays 等价 C# (a - b).TotalDays。
func totalDays(start, end time.Time) Decimal {
	return DecFromFloat(float64(end.Sub(start)) / float64(24*time.Hour))
}

func init() {
	RegisterFunction("NPV", newFunctionNPV)
	RegisterFunction("IRR", newFunctionIRR)
	RegisterFunction("MIRR", newFunctionMIRR)
	RegisterFunction("XIRR", newFunctionXIRR)
	RegisterFunction("XNPV", newFunctionXNPV)
}
