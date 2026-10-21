package mathtransformation

import "strconv"

// 移植自 Internals/Functions/MathTransformation/Function_DEC2BIN.cs、
// Function_DEC2HEX.cs、Function_DEC2OCT.cs。

var (
	decHexMin = MustDecimal("-549755813888")
	decHexMax = MustDecimal("549755813887")
	decOctMin = MustDecimal("-536870912")
	decOctMax = MustDecimal("536870911")
)

type FunctionDEC2BIN struct{ BaseFunction }

func newFunctionDEC2BIN(args []Function) Function {
	RequireArgs(args, "Dec2Bin", 1, 2)
	return &FunctionDEC2BIN{BaseFunction: NewBase("Dec2Bin", OperandTypeText, args)}
}

func (f *FunctionDEC2BIN) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	// 数值超出 int 范围时按 #NUM! 处理
	num, ok := tryGetInt(a)
	if !ok {
		return f.NumError()
	}
	// Excel 范围：-512 ~ 511
	if num < -binHalf || num > binHalf-1 {
		return f.NumError()
	}
	places := 0
	hasPlaces := false
	if f.ArgCount() > 1 {
		b := f.GetNumber(2, engine, tp)
		if b.IsErrorOrNone() {
			return b
		}
		// places 校验先于负数分支：places 非法时即使 number 为负也应返回 #NUM!
		p, ok := tryGetPlaces(b)
		if !ok {
			return f.NumError()
		}
		places = p
		hasPlaces = true
	}
	if num < 0 {
		// 负数：返回 10 位二进制补码，按 Excel 语义忽略 places
		return CreateString(padLeft(strconv.FormatInt(int64(num)&(binModulus-1), 2), maxPlaces, '0'))
	}
	text, ok := tryFormat(strconv.FormatInt(int64(num), 2), false, places, hasPlaces)
	if !ok {
		return f.NumError()
	}
	return CreateString(text)
}

func (f *FunctionDEC2BIN) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
	if arg := f.Arg(1); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- DEC2HEX

type FunctionDEC2HEX struct{ BaseFunction }

func newFunctionDEC2HEX(args []Function) Function {
	RequireArgs(args, "Dec2Hex", 1, 2)
	return &FunctionDEC2HEX{BaseFunction: NewBase("Dec2Hex", OperandTypeText, args)}
}

func (f *FunctionDEC2HEX) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	// Excel 会先截去小数部分再校验范围：-549755813888 ~ 549755813887
	numValue := a.NumberValue().Trunc()
	if numValue.Less(decHexMin) || numValue.Greater(decHexMax) {
		return f.NumError()
	}
	num := numValue.Int64()
	places := 0
	hasPlaces := false
	if f.ArgCount() > 1 {
		b := f.GetNumber(2, engine, tp)
		if b.IsErrorOrNone() {
			return b
		}
		// places 校验先于负数分支
		p, ok := tryGetPlaces(b)
		if !ok {
			return f.NumError()
		}
		places = p
		hasPlaces = true
	}
	if num < 0 {
		// 负数：返回 10 位十六进制补码，按 Excel 语义忽略 places
		return CreateString(padLeft(toUpperHex(num&hexMask), maxPlaces, '0'))
	}
	text, ok := tryFormat(toUpperHex(num), false, places, hasPlaces)
	if !ok {
		return f.NumError()
	}
	return CreateString(text)
}

func (f *FunctionDEC2HEX) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
	if arg := f.Arg(1); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- DEC2OCT

type FunctionDEC2OCT struct{ BaseFunction }

func newFunctionDEC2OCT(args []Function) Function {
	RequireArgs(args, "Dec2Oct", 1, 2)
	return &FunctionDEC2OCT{BaseFunction: NewBase("Dec2Oct", OperandTypeText, args)}
}

func (f *FunctionDEC2OCT) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	// Excel 会先截去小数部分再校验范围：-536870912 ~ 536870911
	numValue := a.NumberValue().Trunc()
	if numValue.Less(decOctMin) || numValue.Greater(decOctMax) {
		return f.NumError()
	}
	num := int(numValue.Int64())
	places := 0
	hasPlaces := false
	if f.ArgCount() > 1 {
		b := f.GetNumber(2, engine, tp)
		if b.IsErrorOrNone() {
			return b
		}
		// places 校验先于负数分支
		p, ok := tryGetPlaces(b)
		if !ok {
			return f.NumError()
		}
		places = p
		hasPlaces = true
	}
	if num < 0 {
		// 负数：返回 10 位八进制补码，按 Excel 语义忽略 places
		return CreateString(padLeft(strconv.FormatInt(int64(num)&octMask, 8), maxPlaces, '0'))
	}
	text, ok := tryFormat(strconv.FormatInt(int64(num), 8), false, places, hasPlaces)
	if !ok {
		return f.NumError()
	}
	return CreateString(text)
}

func (f *FunctionDEC2OCT) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
	if arg := f.Arg(1); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

func init() {
	RegisterFunction("DEC2BIN", newFunctionDEC2BIN)
	RegisterFunction("DEC2HEX", newFunctionDEC2HEX)
	RegisterFunction("DEC2OCT", newFunctionDEC2OCT)
}
