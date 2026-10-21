package mathtransformation

import "strconv"

// 移植自 Internals/Functions/MathTransformation/Function_HEX2BIN.cs、Function_HEX2DEC.cs、
// Function_HEX2OCT.cs、Function_OCT2BIN.cs、Function_OCT2DEC.cs、Function_OCT2HEX.cs。

type FunctionHEX2BIN struct{ BaseFunction }

func newFunctionHEX2BIN(args []Function) Function {
	RequireArgs(args, "Hex2Bin", 1, 2)
	return &FunctionHEX2BIN{BaseFunction: NewBase("Hex2Bin", OperandTypeText, args)}
}

func (f *FunctionHEX2BIN) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetText(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	num, ok := parseComplement(a.TextValue(), 16)
	if !ok {
		return f.NumError()
	}
	// Excel HEX2BIN 结果范围为 -512~511
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
		p, ok := tryGetPlaces(b)
		if !ok {
			return f.NumError()
		}
		places = p
		hasPlaces = true
	}
	if num < 0 {
		// 负数：10 位二进制补码，按 Excel 语义忽略 places
		return CreateString(padLeft(strconv.FormatInt(num&(binModulus-1), 2), maxPlaces, '0'))
	}
	text, ok := tryFormat(strconv.FormatInt(num, 2), false, places, hasPlaces)
	if !ok {
		return f.NumError()
	}
	return CreateString(text)
}

func (f *FunctionHEX2BIN) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeText)
	}
	if arg := f.Arg(1); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- HEX2DEC

type FunctionHEX2DEC struct{ BaseFunction }

func newFunctionHEX2DEC(args []Function) Function {
	RequireArgs(args, "Hex2Dec", 1, 2)
	return &FunctionHEX2DEC{BaseFunction: NewBase("Hex2Dec", OperandTypeNumber, args)}
}

func (f *FunctionHEX2DEC) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetText(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	num, ok := parseComplement(a.TextValue(), 16)
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
	return CreateInt64(num)
}

// ResultType 指定 places 时返回文本。
func (f *FunctionHEX2DEC) ResultType() OperandType {
	if f.ArgCount() > 1 {
		return OperandTypeText
	}
	return OperandTypeNumber
}

func (f *FunctionHEX2DEC) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeText)
	}
	if arg := f.Arg(1); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- HEX2OCT

type FunctionHEX2OCT struct{ BaseFunction }

func newFunctionHEX2OCT(args []Function) Function {
	RequireArgs(args, "Hex2Oct", 1, 2)
	return &FunctionHEX2OCT{BaseFunction: NewBase("Hex2Oct", OperandTypeText, args)}
}

func (f *FunctionHEX2OCT) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetText(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	num, ok := parseComplement(a.TextValue(), 16)
	if !ok {
		return f.NumError()
	}
	// Excel HEX2OCT 结果范围为 -536870912~536870911
	if num < -octHalf || num > octHalf-1 {
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
	if num < 0 {
		// 负数：10 位八进制补码，按 Excel 语义忽略 places
		return CreateString(padLeft(strconv.FormatInt(num&octMask, 8), maxPlaces, '0'))
	}
	text, ok := tryFormat(strconv.FormatInt(num, 8), false, places, hasPlaces)
	if !ok {
		return f.NumError()
	}
	return CreateString(text)
}

func (f *FunctionHEX2OCT) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeText)
	}
	if arg := f.Arg(1); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- OCT2BIN

type FunctionOCT2BIN struct{ BaseFunction }

func newFunctionOCT2BIN(args []Function) Function {
	RequireArgs(args, "Oct2Bin", 1, 2)
	return &FunctionOCT2BIN{BaseFunction: NewBase("Oct2Bin", OperandTypeText, args)}
}

func (f *FunctionOCT2BIN) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetText(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	num64, ok := parseComplement(a.TextValue(), 8)
	if !ok {
		return f.NumError()
	}
	num := int(num64)
	// Excel OCT2BIN 结果范围为 -512~511
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
		p, ok := tryGetPlaces(b)
		if !ok {
			return f.NumError()
		}
		places = p
		hasPlaces = true
	}
	if num < 0 {
		// 负数：10 位二进制补码，按 Excel 语义忽略 places
		return CreateString(padLeft(strconv.FormatInt(int64(num)&(binModulus-1), 2), maxPlaces, '0'))
	}
	text, ok := tryFormat(strconv.FormatInt(int64(num), 2), false, places, hasPlaces)
	if !ok {
		return f.NumError()
	}
	return CreateString(text)
}

func (f *FunctionOCT2BIN) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeText)
	}
	if arg := f.Arg(1); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- OCT2DEC

type FunctionOCT2DEC struct{ BaseFunction }

func newFunctionOCT2DEC(args []Function) Function {
	RequireArgs(args, "Oct2Dec", 1, 2)
	return &FunctionOCT2DEC{BaseFunction: NewBase("Oct2Dec", OperandTypeNumber, args)}
}

func (f *FunctionOCT2DEC) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetText(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	num, ok := parseComplement(a.TextValue(), 8)
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
func (f *FunctionOCT2DEC) ResultType() OperandType {
	if f.ArgCount() > 1 {
		return OperandTypeText
	}
	return OperandTypeNumber
}

func (f *FunctionOCT2DEC) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeText)
	}
	if arg := f.Arg(1); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- OCT2HEX

type FunctionOCT2HEX struct{ BaseFunction }

func newFunctionOCT2HEX(args []Function) Function {
	RequireArgs(args, "Oct2Hex", 1, 2)
	return &FunctionOCT2HEX{BaseFunction: NewBase("Oct2Hex", OperandTypeText, args)}
}

func (f *FunctionOCT2HEX) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetText(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	num64, ok := parseComplement(a.TextValue(), 8)
	if !ok {
		return f.NumError()
	}
	num := int(num64)
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
	if num < 0 {
		// 负数：10 位十六进制补码，按 Excel 语义忽略 places
		return CreateString(padLeft(toUpperHex(int64(num)&hexMask), maxPlaces, '0'))
	}
	text, ok := tryFormat(toUpperHex(int64(num)), false, places, hasPlaces)
	if !ok {
		return f.NumError()
	}
	return CreateString(text)
}

func (f *FunctionOCT2HEX) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeText)
	}
	if arg := f.Arg(1); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

func init() {
	RegisterFunction("HEX2BIN", newFunctionHEX2BIN)
	RegisterFunction("HEX2DEC", newFunctionHEX2DEC)
	RegisterFunction("HEX2OCT", newFunctionHEX2OCT)
	RegisterFunction("OCT2BIN", newFunctionOCT2BIN)
	RegisterFunction("OCT2DEC", newFunctionOCT2DEC)
	RegisterFunction("OCT2HEX", newFunctionOCT2HEX)
}
