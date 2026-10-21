package mathbase

import "github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals"

// 移植自 Internals/Functions/MathBase/Function_FACT.cs、Function_FACTDOUBLE.cs、
// Function_COMBIN.cs、Function_PERMUT.cs、Function_MULTINOMIAL.cs、
// Function_PRODUCT.cs、Function_GCD.cs、Function_LCM.cs。
// 大数运算保留 Decimal 精度，并按 C# decimal 的溢出行为返回错误值(DecimalMax)。

type FunctionFACT struct{ BaseFunction }

func newFunctionFACT(args []Function) Function {
	RequireArgs(args, "Fact", 1, 1)
	return &FunctionFACT{BaseFunction: NewBase("Fact", OperandTypeNumber, args)}
}

func (f *FunctionFACT) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	// 先检查数值范围,避免超范围转换
	number := a.NumberValue()
	if number.Less(decIntMin) || number.Greater(decIntMax) {
		return f.ParameterError(1)
	}
	z := int(number.Int64())
	if z < 0 || z > 27 {
		return f.ParameterError(1)
	}
	d := DecimalOne
	for i := 1; i <= z; i++ {
		d = d.Mul(DecFromInt(int64(i)))
	}
	return CreateDecimal(d)
}

func (f *FunctionFACT) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- FACTDOUBLE

type FunctionFACTDOUBLE struct{ BaseFunction }

func newFunctionFACTDOUBLE(args []Function) Function {
	RequireArgs(args, "FactDouble", 1, 1)
	return &FunctionFACTDOUBLE{BaseFunction: NewBase("FactDouble", OperandTypeNumber, args)}
}

func (f *FunctionFACTDOUBLE) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	number := a.NumberValue()
	if number.Less(decIntMin) || number.Greater(decIntMax) {
		return f.ParameterError(1)
	}
	z := int(number.Int64())
	if z < 0 {
		return f.ParameterError(1)
	}
	// decimal 最大可容纳 45!!，46!! 溢出
	if z > 45 {
		return f.ParameterError(1)
	}
	d := DecimalOne
	for i := z; i > 0; i -= 2 {
		d = d.Mul(DecFromInt(int64(i)))
	}
	return CreateDecimal(d)
}

func (f *FunctionFACTDOUBLE) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- COMBIN

type FunctionCOMBIN struct{ BaseFunction }

func newFunctionCOMBIN(args []Function) Function {
	RequireArgs(args, "Combin", 2, 2)
	return &FunctionCOMBIN{BaseFunction: NewBase("Combin", OperandTypeNumber, args)}
}

func (f *FunctionCOMBIN) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	b := f.GetNumber(2, engine, tp)
	if b.IsErrorOrNone() {
		return b
	}
	num1 := a.NumberValue()
	num2 := b.NumberValue()
	if num1.Less(decIntMin) || num1.Greater(decIntMax) {
		return f.ParameterError(1)
	}
	if num2.Less(decIntMin) || num2.Greater(decIntMax) {
		return f.ParameterError(2)
	}
	total := int(num1.Int64())
	count := int(num2.Int64())
	if total < 0 {
		return f.ParameterError(1)
	}
	if count < 0 {
		return f.ParameterError(2)
	}
	if total < count {
		return f.ParameterError(2)
	}
	// C(n,k)=C(n,n-k)，取较小的 k 计算
	k := count
	if total-count < k {
		k = total - count
	}
	result := DecimalOne
	for i := 1; i <= k; i++ {
		product := result.Mul(DecFromInt(int64(total - k + i)))
		if isOverflow(product) {
			return f.NumError()
		}
		result = product.Div(DecFromInt(int64(i)))
	}
	return CreateDecimal(result)
}

func (f *FunctionCOMBIN) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
	if arg := f.Arg(1); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- PERMUT

type FunctionPERMUT struct{ BaseFunction }

func newFunctionPERMUT(args []Function) Function {
	RequireArgs(args, "Permut", 2, 2)
	return &FunctionPERMUT{BaseFunction: NewBase("Permut", OperandTypeNumber, args)}
}

func (f *FunctionPERMUT) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	b := f.GetNumber(2, engine, tp)
	if b.IsErrorOrNone() {
		return b
	}
	num1 := a.NumberValue()
	num2 := b.NumberValue()
	if num1.Less(decIntMin) || num1.Greater(decIntMax) {
		return f.ParameterError(1)
	}
	if num2.Less(decIntMin) || num2.Greater(decIntMax) {
		return f.ParameterError(2)
	}
	total := int(num1.Int64())
	count := int(num2.Int64())
	if total < 0 {
		return f.ParameterError(1)
	}
	if count < 0 {
		return f.ParameterError(2)
	}
	if total < count {
		return f.ParameterError(2)
	}
	sum := DecimalOne
	for i := 0; i < count; i++ {
		sum = sum.Mul(DecFromInt(int64(total - i)))
		if isOverflow(sum) {
			return f.ParameterError(1)
		}
	}
	return CreateDecimal(sum)
}

func (f *FunctionPERMUT) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
	if arg := f.Arg(1); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- MULTINOMIAL

type FunctionMULTINOMIAL struct{ BaseFunction }

func newFunctionMULTINOMIAL(args []Function) Function {
	RequireArgs(args, "Multinomial", 1, -1)
	return &FunctionMULTINOMIAL{BaseFunction: NewBase("Multinomial", OperandTypeNumber, args)}
}

func (f *FunctionMULTINOMIAL) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	var args []Operand
	if err := f.EvaluateAll(engine, tp, &args); err != nil {
		return err
	}
	list := make([]Decimal, 0, len(args))
	for i, arg := range args {
		// 按参数逐个展平，使错误消息给出真实的参数序号
		var item []Decimal
		if !internals.FlattenToNumberList(arg, &item) {
			return f.FunctionError()
		}
		for _, value := range item {
			if value.Less(decIntMin) || value.Greater(decIntMax) {
				return f.ParameterError(i + 1)
			}
			if value.Trunc().Sign() < 0 {
				return f.ParameterError(i + 1)
			}
		}
		list = append(list, item...)
	}

	sum := DecimalZero
	n := DecimalOne
	for _, value := range list {
		a := int(value.Int64())
		factorial, ok := factorialChecked(a)
		if !ok {
			return f.FunctionError()
		}
		n = n.Mul(factorial)
		if isOverflow(n) {
			return f.FunctionError()
		}
		sum = sum.Add(DecFromInt(int64(a)))
	}
	total, ok := factorialChecked(int(sum.Int64()))
	if !ok {
		return f.FunctionError()
	}
	return CreateDecimal(total.Div(n))
}

func (f *FunctionMULTINOMIAL) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	for i := 0; i < f.ArgCount(); i++ {
		if arg := f.Arg(i); arg != nil {
			arg.GetParameterTypes(engine, result, OperandTypeNumber)
		}
	}
}

// ---------------------------------------------------------------- PRODUCT

type FunctionPRODUCT struct{ BaseFunction }

func newFunctionPRODUCT(args []Function) Function {
	RequireArgs(args, "Product", 1, -1)
	return &FunctionPRODUCT{BaseFunction: NewBase("Product", OperandTypeNumber, args)}
}

func (f *FunctionPRODUCT) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	var args []Operand
	if err := f.EvaluateAll(engine, tp, &args); err != nil {
		return err
	}
	var list []Decimal
	if !internals.FlattenToBigDecimalList(args, &list) {
		return f.FunctionError()
	}
	d := DecimalOne
	for _, value := range list {
		d = d.Mul(value)
		if isOverflow(d) {
			return f.FunctionError()
		}
	}
	return CreateDecimal(d)
}

func (f *FunctionPRODUCT) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	for i := 0; i < f.ArgCount(); i++ {
		if arg := f.Arg(i); arg != nil {
			arg.GetParameterTypes(engine, result, OperandTypeNumber)
		}
	}
}

// ---------------------------------------------------------------- GCD

type FunctionGCD struct{ BaseFunction }

func newFunctionGCD(args []Function) Function {
	RequireArgs(args, "Gcd", 1, -1)
	return &FunctionGCD{BaseFunction: NewBase("Gcd", OperandTypeNumber, args)}
}

func (f *FunctionGCD) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args := make([]Operand, 0, f.ArgCount())
	for i := 0; i < f.ArgCount(); i++ {
		aa := f.GetNumber(i+1, engine, tp)
		if aa.IsErrorOrNone() {
			return aa
		}
		args = append(args, aa)
	}
	var list []Decimal
	if !internals.FlattenToBigDecimalList(args, &list) {
		return f.FunctionError()
	}
	for i, value := range list {
		if value.Sign() < 0 {
			return f.ParameterError(i + 1)
		}
	}
	return CreateDecimal(internals.GetGCD(list))
}

func (f *FunctionGCD) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	for i := 0; i < f.ArgCount(); i++ {
		if arg := f.Arg(i); arg != nil {
			arg.GetParameterTypes(engine, result, OperandTypeNumber)
		}
	}
}

// ---------------------------------------------------------------- LCM

type FunctionLCM struct{ BaseFunction }

func newFunctionLCM(args []Function) Function {
	RequireArgs(args, "Lcm", 1, -1)
	return &FunctionLCM{BaseFunction: NewBase("Lcm", OperandTypeNumber, args)}
}

func (f *FunctionLCM) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args := make([]Operand, 0, f.ArgCount())
	for i := 0; i < f.ArgCount(); i++ {
		aa := f.GetNumber(i+1, engine, tp)
		if aa.IsErrorOrNone() {
			return aa
		}
		args = append(args, aa)
	}
	var list []Decimal
	if !internals.FlattenToBigDecimalList(args, &list) {
		return f.FunctionError()
	}
	for i, value := range list {
		if value.Sign() < 0 {
			return f.ParameterError(i + 1)
		}
	}
	return CreateDecimal(internals.GetLCM(list))
}

func (f *FunctionLCM) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	for i := 0; i < f.ArgCount(); i++ {
		if arg := f.Arg(i); arg != nil {
			arg.GetParameterTypes(engine, result, OperandTypeNumber)
		}
	}
}

func init() {
	RegisterFunction("FACT", newFunctionFACT)
	RegisterFunction("FACTDOUBLE", newFunctionFACTDOUBLE)
	RegisterFunction("COMBIN", newFunctionCOMBIN)
	RegisterFunction("PERMUT", newFunctionPERMUT)
	RegisterFunction("MULTINOMIAL", newFunctionMULTINOMIAL)
	RegisterFunction("PRODUCT", newFunctionPRODUCT)
	RegisterFunction("GCD", newFunctionGCD)
	RegisterFunction("LCM", newFunctionLCM)
}
