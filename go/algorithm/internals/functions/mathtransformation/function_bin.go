package mathtransformation

import "strconv"

// 移植自 Internals/Functions/MathTransformation/Function_BIN2DEC.cs、
// Function_BIN2HEX.cs、Function_BIN2OCT.cs。

type FunctionBIN2DEC struct{ BaseFunction }

func newFunctionBIN2DEC(args []Function) Function {
	RequireArgs(args, "Bin2Dec", 1, 2)
	return &FunctionBIN2DEC{BaseFunction: NewBase("Bin2Dec", OperandTypeNumber, args)}
}

func (f *FunctionBIN2DEC) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetText(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	// 文本裁剪空白、校验字符与长度（<=10），并解析 10 位二进制补码
	num, ok := parseComplement(a.TextValue(), 2)
	if !ok {
		return f.NumError()
	}
	if f.ArgCount() > 1 {
		b := f.GetNumber(2, engine, tp)
		if b.IsErrorOrNone() {
			return b
		}
		places, ok := tryGetPlaces(b)
		if !ok {
			return f.NumError()
		}
		text, ok := tryFormatDecimal(num, places)
		if !ok {
			return f.NumError()
		}
		return CreateString(text)
	}
	return CreateInt(int(num))
}

// ResultType 指定 places 时返回文本。
func (f *FunctionBIN2DEC) ResultType() OperandType {
	if f.ArgCount() > 1 {
		return OperandTypeText
	}
	return OperandTypeNumber
}

func (f *FunctionBIN2DEC) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeText)
	}
	if arg := f.Arg(1); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- BIN2HEX

type FunctionBIN2HEX struct{ BaseFunction }

func newFunctionBIN2HEX(args []Function) Function {
	RequireArgs(args, "Bin2Hex", 1, 2)
	return &FunctionBIN2HEX{BaseFunction: NewBase("Bin2Hex", OperandTypeText, args)}
}

func (f *FunctionBIN2HEX) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetText(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	bin, ok := parseComplement(a.TextValue(), 2)
	if !ok {
		return f.NumError()
	}
	places := 0
	hasPlaces := false
	if f.ArgCount() > 1 {
		b := f.GetNumber(2, engine, tp)
		if b.IsErrorOrNone() {
			return b
		}
		p, ok := tryGetPlaces(b)
		if !ok {
			return f.NumError()
		}
		places = p
		hasPlaces = true
	}
	var hex string
	if bin < 0 {
		// 负数：10 位十六进制补码，按 Excel 语义忽略 places
		hex = padLeft(toUpperHex(bin&hexMask), maxPlaces, '0')
	} else {
		hex = toUpperHex(bin)
	}
	text, ok := tryFormat(hex, bin < 0, places, hasPlaces)
	if !ok {
		return f.NumError()
	}
	return CreateString(text)
}

func (f *FunctionBIN2HEX) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeText)
	}
	if arg := f.Arg(1); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- BIN2OCT

type FunctionBIN2OCT struct{ BaseFunction }

func newFunctionBIN2OCT(args []Function) Function {
	RequireArgs(args, "Bin2Oct", 1, 2)
	return &FunctionBIN2OCT{BaseFunction: NewBase("Bin2Oct", OperandTypeText, args)}
}

func (f *FunctionBIN2OCT) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetText(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	bin64, ok := parseComplement(a.TextValue(), 2)
	if !ok {
		return f.NumError()
	}
	bin := int(bin64)

	places := 0
	hasPlaces := false
	if f.ArgCount() > 1 {
		b := f.GetNumber(2, engine, tp)
		if b.IsErrorOrNone() {
			return b
		}
		// places 校验先于结果分支：places 非法时即使 number 为负也应返回 #NUM!
		p, ok := tryGetPlaces(b)
		if !ok {
			return f.NumError()
		}
		places = p
		hasPlaces = true
	}

	if bin < 0 {
		// 负数：10 位八进制补码，按 Excel 语义忽略 places
		return CreateString(padLeft(strconv.FormatInt(int64(bin)&octMask, 8), maxPlaces, '0'))
	}
	text, ok := tryFormat(strconv.FormatInt(int64(bin), 8), false, places, hasPlaces)
	if !ok {
		return f.NumError()
	}
	return CreateString(text)
}

func (f *FunctionBIN2OCT) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeText)
	}
	if arg := f.Arg(1); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

func init() {
	RegisterFunction("BIN2DEC", newFunctionBIN2DEC)
	RegisterFunction("BIN2HEX", newFunctionBIN2HEX)
	RegisterFunction("BIN2OCT", newFunctionBIN2OCT)
}
