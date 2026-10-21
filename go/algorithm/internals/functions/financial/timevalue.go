package financial

// FV / PV / PMT / IPMT / PPMT / NPER / RATE
// 对应 csharp/ToolGood.Algorithm/Internals/Functions/Financial/Function_FV.cs 等

// getTypeArg 读取 type 参数（仅允许 0/1），先校验再转换。
func getTypeArg(f *BaseFunction, engine *AlgorithmEngine, tp TempParameter, index int) (int, Operand) {
	arg := f.GetNumber(index, engine, tp)
	if arg.IsErrorOrNone() {
		return 0, arg
	}
	v := arg.NumberValue()
	if !v.IsZero() && !v.Equal(DecimalOne) {
		return 0, parameterError(f, index)
	}
	return int(v.Int64()), nil
}

type FunctionFV struct{ BaseFunction }

func newFunctionFV(args []Function) Function {
	RequireArgs(args, "FV", 3, 5)
	return &FunctionFV{BaseFunction: NewBase("FV", OperandTypeNumber, args)}
}

func (f *FunctionFV) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	rateArg := f.GetNumber(1, engine, tp)
	if rateArg.IsErrorOrNone() {
		return rateArg
	}
	rate := rateArg.NumberValue()

	nperArg := f.GetNumber(2, engine, tp)
	if nperArg.IsErrorOrNone() {
		return nperArg
	}
	nper := nperArg.NumberValue()

	pmtArg := f.GetNumber(3, engine, tp)
	if pmtArg.IsErrorOrNone() {
		return pmtArg
	}
	pmt := pmtArg.NumberValue()

	pv := DecimalZero
	if f.ArgCount() > 3 {
		pvArg := f.GetNumber(4, engine, tp)
		if pvArg.IsErrorOrNone() {
			return pvArg
		}
		pv = pvArg.NumberValue()
	}

	tp5 := 0
	if f.ArgCount() > 4 {
		v, err := getTypeArg(&f.BaseFunction, engine, tp, 5)
		if err != nil {
			return err
		}
		tp5 = v
	}

	if rate.IsZero() {
		return CreateDecimal(pmt.Mul(nper).Neg().Sub(pv))
	}

	factor, ok := tryPowPositiveBase(DecimalOne.Add(rate), nper)
	if !ok {
		return numError(f)
	}
	fv := pv.Mul(factor).Neg().Sub(pmt.Mul(factor.Sub(DecimalOne)).Div(rate))
	if tp5 == 1 {
		fv = pv.Mul(factor).Neg().Sub(pmt.Mul(DecimalOne.Add(rate)).Mul(factor.Sub(DecimalOne)).Div(rate))
	}
	return CreateDecimal(fv)
}

type FunctionPV struct{ BaseFunction }

func newFunctionPV(args []Function) Function {
	RequireArgs(args, "PV", 3, 5)
	return &FunctionPV{BaseFunction: NewBase("PV", OperandTypeNumber, args)}
}

func (f *FunctionPV) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	rateArg := f.GetNumber(1, engine, tp)
	if rateArg.IsErrorOrNone() {
		return rateArg
	}
	rate := rateArg.NumberValue()

	nperArg := f.GetNumber(2, engine, tp)
	if nperArg.IsErrorOrNone() {
		return nperArg
	}
	nper := nperArg.NumberValue()

	pmtArg := f.GetNumber(3, engine, tp)
	if pmtArg.IsErrorOrNone() {
		return pmtArg
	}
	pmt := pmtArg.NumberValue()

	fv := DecimalZero
	if f.ArgCount() > 3 {
		fvArg := f.GetNumber(4, engine, tp)
		if fvArg.IsErrorOrNone() {
			return fvArg
		}
		fv = fvArg.NumberValue()
	}

	tp5 := 0
	if f.ArgCount() > 4 {
		v, err := getTypeArg(&f.BaseFunction, engine, tp, 5)
		if err != nil {
			return err
		}
		tp5 = v
	}

	if rate.IsZero() {
		return CreateDecimal(pmt.Mul(nper).Neg().Sub(fv))
	}

	factor, ok := tryPowPositiveBase(DecimalOne.Add(rate), nper)
	if !ok {
		return numError(f)
	}
	pv := fv.Add(pmt.Mul(factor.Sub(DecimalOne)).Div(rate)).Div(factor).Neg()
	if tp5 == 1 {
		pv = fv.Add(pmt.Mul(DecimalOne.Add(rate)).Mul(factor.Sub(DecimalOne)).Div(rate)).Div(factor).Neg()
	}
	return CreateDecimal(pv)
}

type FunctionPMT struct{ BaseFunction }

func newFunctionPMT(args []Function) Function {
	RequireArgs(args, "PMT", 3, 5)
	return &FunctionPMT{BaseFunction: NewBase("PMT", OperandTypeNumber, args)}
}

func (f *FunctionPMT) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	rateArg := f.GetNumber(1, engine, tp)
	if rateArg.IsErrorOrNone() {
		return rateArg
	}
	rate := rateArg.NumberValue()

	nperArg := f.GetNumber(2, engine, tp)
	if nperArg.IsErrorOrNone() {
		return nperArg
	}
	nper := nperArg.NumberValue()

	pvArg := f.GetNumber(3, engine, tp)
	if pvArg.IsErrorOrNone() {
		return pvArg
	}
	pv := pvArg.NumberValue()

	if nper.IsZero() {
		return div0Error(f)
	}

	fv := DecimalZero
	if f.ArgCount() > 3 {
		fvArg := f.GetNumber(4, engine, tp)
		if fvArg.IsErrorOrNone() {
			return fvArg
		}
		fv = fvArg.NumberValue()
	}

	tp5 := 0
	if f.ArgCount() > 4 {
		v, err := getTypeArg(&f.BaseFunction, engine, tp, 5)
		if err != nil {
			return err
		}
		tp5 = v
	}

	if rate.IsZero() {
		return CreateDecimal(pv.Add(fv).Neg().Div(nper))
	}

	factor, ok := tryPowPositiveBase(DecimalOne.Add(rate), nper)
	if !ok {
		return numError(f)
	}
	pmt := pv.Mul(factor).Add(fv).Neg().Mul(rate).Div(factor.Sub(DecimalOne))
	if tp5 == 1 {
		pmt = pmt.Div(DecimalOne.Add(rate))
	}
	return CreateDecimal(pmt)
}

type FunctionIPMT struct{ BaseFunction }

func newFunctionIPMT(args []Function) Function {
	RequireArgs(args, "IPMT", 4, 6)
	return &FunctionIPMT{BaseFunction: NewBase("IPMT", OperandTypeNumber, args)}
}

func (f *FunctionIPMT) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	rateArg := f.GetNumber(1, engine, tp)
	if rateArg.IsErrorOrNone() {
		return rateArg
	}
	rate := rateArg.NumberValue()

	perArg := f.GetNumber(2, engine, tp)
	if perArg.IsErrorOrNone() {
		return perArg
	}
	per := perArg.NumberValue()

	nperArg := f.GetNumber(3, engine, tp)
	if nperArg.IsErrorOrNone() {
		return nperArg
	}
	nper := nperArg.NumberValue()

	pvArg := f.GetNumber(4, engine, tp)
	if pvArg.IsErrorOrNone() {
		return pvArg
	}
	pv := pvArg.NumberValue()

	fv := DecimalZero
	if f.ArgCount() > 4 {
		fvArg := f.GetNumber(5, engine, tp)
		if fvArg.IsErrorOrNone() {
			return fvArg
		}
		fv = fvArg.NumberValue()
	}

	tp6 := 0
	if f.ArgCount() > 5 {
		v, err := getTypeArg(&f.BaseFunction, engine, tp, 6)
		if err != nil {
			return err
		}
		tp6 = v
	}

	if per.Less(DecimalOne) || per.Greater(nper) {
		return parameterError(f, 2)
	}
	if rate.IsZero() {
		return CreateInt(0)
	}

	pmt, ok := calculatePMT(rate, nper, pv, fv, tp6)
	if !ok {
		return numError(f)
	}
	factor, ok := tryPowPositiveBase(DecimalOne.Add(rate), per.Sub(DecimalOne))
	if !ok {
		return numError(f)
	}
	ipmt := pv.Mul(factor).Add(pmt.Mul(factor.Sub(DecimalOne)).Div(rate)).Neg().Mul(rate)
	if tp6 == 1 && per.Equal(DecimalOne) {
		ipmt = DecimalZero
	}
	return CreateDecimal(ipmt)
}

type FunctionPPMT struct{ BaseFunction }

func newFunctionPPMT(args []Function) Function {
	RequireArgs(args, "PPMT", 4, 6)
	return &FunctionPPMT{BaseFunction: NewBase("PPMT", OperandTypeNumber, args)}
}

func (f *FunctionPPMT) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	rateArg := f.GetNumber(1, engine, tp)
	if rateArg.IsErrorOrNone() {
		return rateArg
	}
	rate := rateArg.NumberValue()

	perArg := f.GetNumber(2, engine, tp)
	if perArg.IsErrorOrNone() {
		return perArg
	}
	per := perArg.NumberValue()

	nperArg := f.GetNumber(3, engine, tp)
	if nperArg.IsErrorOrNone() {
		return nperArg
	}
	nper := nperArg.NumberValue()

	pvArg := f.GetNumber(4, engine, tp)
	if pvArg.IsErrorOrNone() {
		return pvArg
	}
	pv := pvArg.NumberValue()

	fv := DecimalZero
	if f.ArgCount() > 4 {
		fvArg := f.GetNumber(5, engine, tp)
		if fvArg.IsErrorOrNone() {
			return fvArg
		}
		fv = fvArg.NumberValue()
	}

	tp6 := 0
	if f.ArgCount() > 5 {
		v, err := getTypeArg(&f.BaseFunction, engine, tp, 6)
		if err != nil {
			return err
		}
		tp6 = v
	}

	if per.Less(DecimalOne) || per.Greater(nper) {
		return parameterError(f, 2)
	}

	pmt, ok := calculatePMT(rate, nper, pv, fv, tp6)
	if !ok {
		return numError(f)
	}
	ipmt, ok := calculateIPMT(rate, per, nper, pv, fv, tp6)
	if !ok {
		return numError(f)
	}
	return CreateDecimal(pmt.Sub(ipmt))
}

type FunctionNPER struct{ BaseFunction }

func newFunctionNPER(args []Function) Function {
	RequireArgs(args, "NPER", 3, 5)
	return &FunctionNPER{BaseFunction: NewBase("NPER", OperandTypeNumber, args)}
}

func (f *FunctionNPER) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	rateArg := f.GetNumber(1, engine, tp)
	if rateArg.IsErrorOrNone() {
		return rateArg
	}
	rate := rateArg.NumberValue()

	pmtArg := f.GetNumber(2, engine, tp)
	if pmtArg.IsErrorOrNone() {
		return pmtArg
	}
	pmt := pmtArg.NumberValue()

	pvArg := f.GetNumber(3, engine, tp)
	if pvArg.IsErrorOrNone() {
		return pvArg
	}
	pv := pvArg.NumberValue()

	fv := DecimalZero
	if f.ArgCount() > 3 {
		fvArg := f.GetNumber(4, engine, tp)
		if fvArg.IsErrorOrNone() {
			return fvArg
		}
		fv = fvArg.NumberValue()
	}

	tp5 := 0
	if f.ArgCount() > 4 {
		v, err := getTypeArg(&f.BaseFunction, engine, tp, 5)
		if err != nil {
			return err
		}
		tp5 = v
	}

	if rate.IsZero() {
		if pmt.IsZero() {
			return div0Error(f)
		}
		return CreateDecimal(pv.Add(fv).Neg().Div(pmt))
	}
	if rate.Equal(DecimalOne.Neg()) {
		return div0Error(f)
	}

	factor := pmt
	if tp5 == 1 {
		factor = pmt.Mul(DecimalOne.Add(rate))
	}

	logBase := DecimalOne.Add(rate)
	if logBase.Sign() <= 0 {
		return numError(f)
	}
	denominator := pv.Mul(rate).Add(factor)
	if denominator.IsZero() {
		return div0Error(f)
	}
	ratio := fv.Mul(rate).Neg().Add(factor).Div(denominator)
	if ratio.Sign() <= 0 {
		return numError(f)
	}

	lnRatio, ok := decLn(ratio)
	if !ok {
		return numError(f)
	}
	lnBase, ok := decLn(logBase)
	if !ok {
		return numError(f)
	}
	return CreateDecimal(lnRatio.Div(lnBase))
}

// ------------------------------------------------------------------ RATE

type FunctionRATE struct{ BaseFunction }

func newFunctionRATE(args []Function) Function {
	RequireArgs(args, "RATE", 3, 6)
	return &FunctionRATE{BaseFunction: NewBase("RATE", OperandTypeNumber, args)}
}

func (f *FunctionRATE) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	nperArg := f.GetNumber(1, engine, tp)
	if nperArg.IsErrorOrNone() {
		return nperArg
	}
	nper := nperArg.NumberValue()

	pmtArg := f.GetNumber(2, engine, tp)
	if pmtArg.IsErrorOrNone() {
		return pmtArg
	}
	pmt := pmtArg.NumberValue()

	pvArg := f.GetNumber(3, engine, tp)
	if pvArg.IsErrorOrNone() {
		return pvArg
	}
	pv := pvArg.NumberValue()

	if nper.LessOrEqual(DecimalZero) {
		return parameterError(f, 1)
	}

	fv := DecimalZero
	if f.ArgCount() > 3 {
		fvArg := f.GetNumber(4, engine, tp)
		if fvArg.IsErrorOrNone() {
			return fvArg
		}
		fv = fvArg.NumberValue()
	}

	tp5 := 0
	if f.ArgCount() > 4 {
		v, err := getTypeArg(&f.BaseFunction, engine, tp, 5)
		if err != nil {
			return err
		}
		tp5 = v
	}

	guess := decDefaultGuess
	if f.ArgCount() > 5 {
		guessArg := f.GetNumber(6, engine, tp)
		if guessArg.IsErrorOrNone() {
			return guessArg
		}
		guess = guessArg.NumberValue()
	}

	rate, ok := newtonRaphsonRATE(nper, pmt, pv, fv, DecFromInt(int64(tp5)), guess)
	if !ok {
		return functionError(f)
	}
	return CreateDecimal(rate)
}

// newtonRaphsonRATE 与 C# RATE.NewtonRaphson 一致：最多 100 次迭代，
// 精度 1e-10，斜率下限 1e-12。
func newtonRaphsonRATE(n, p, v, f, typ, rate Decimal) (Decimal, bool) {
	for i := 0; i < 100; i++ {
		if rate.IsZero() {
			// C# 中此处会除零抛异常，最终返回函数错误
			return DecimalZero, false
		}
		onePlusRate := DecimalOne.Add(rate)
		factor, ok := tryPowPositiveBase(onePlusRate, n)
		if !ok {
			return DecimalZero, false
		}
		powNMinus1, ok := tryPowPositiveBase(onePlusRate, n.Sub(DecimalOne))
		if !ok {
			return DecimalZero, false
		}

		var fn Decimal
		if typ.Equal(DecimalOne) {
			fn = v.Mul(factor).Add(p.Mul(onePlusRate).Mul(factor.Sub(DecimalOne)).Div(rate)).Add(f)
		} else {
			fn = v.Mul(factor).Add(p.Mul(factor.Sub(DecimalOne)).Div(rate)).Add(f)
		}

		var dfn Decimal
		if typ.Equal(DecimalOne) {
			dfn = v.Mul(n).Mul(powNMinus1).
				Add(p.Mul(onePlusRate).Mul(n.Mul(rate).Mul(powNMinus1).Sub(factor.Sub(DecimalOne))).Div(rate.Mul(rate))).
				Add(p.Mul(factor.Sub(DecimalOne)).Div(rate))
		} else {
			dfn = v.Mul(n).Mul(powNMinus1).
				Add(p.Mul(n.Mul(rate).Mul(powNMinus1).Sub(factor.Sub(DecimalOne))).Div(rate.Mul(rate)))
		}

		if dfn.Abs().Less(decEpsSlope) {
			break
		}
		newRate := rate.Sub(fn.Div(dfn))
		if newRate.Sub(rate).Abs().Less(decEpsStep) {
			return newRate, true
		}
		rate = newRate
	}
	return DecimalZero, false
}

func init() {
	RegisterFunction("FV", newFunctionFV)
	RegisterFunction("PV", newFunctionPV)
	RegisterFunction("PMT", newFunctionPMT)
	RegisterFunction("IPMT", newFunctionIPMT)
	RegisterFunction("PPMT", newFunctionPPMT)
	RegisterFunction("NPER", newFunctionNPER)
	RegisterFunction("RATE", newFunctionRATE)
}
