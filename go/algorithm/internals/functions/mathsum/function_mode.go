package mathsum

import "github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals"

// FunctionMODE 对应 C# Function_MODE（众数）。
type FunctionMODE struct{ BaseFunction }

func newFunctionMODE(args []Function) Function {
	RequireArgs(args, "Mode", 1, -1)
	return &FunctionMODE{BaseFunction: NewBase("Mode", OperandTypeNumber, args)}
}

func (f *FunctionMODE) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	var args []Operand
	if err := f.EvaluateAll(engine, tp, &args); err != nil {
		return err
	}

	var list []Decimal
	if !internals.FlattenToBigDecimalList(args, &list) {
		return f.FunctionError()
	}
	if len(list) == 0 {
		return f.FunctionError()
	}

	// Decimal 内含 *big.Int，不能直接作为 map 键，用去尾零文本归一化后计数；
	// order 保留首次出现顺序，使并列时取最靠前的值（与 C# Dictionary 迭代一致）
	counts := make(map[string]int, len(list))
	order := make([]Decimal, 0, len(list))
	for _, v := range list {
		key := v.PlainStrip()
		if _, ok := counts[key]; !ok {
			order = append(order, v)
		}
		counts[key]++
	}
	modeKey := DecimalZero
	maxCount := 1
	for _, v := range order {
		if c := counts[v.PlainStrip()]; c > maxCount {
			maxCount = c
			modeKey = v
		}
	}
	if maxCount == 1 {
		return f.FunctionError()
	}
	return CreateDecimal(modeKey)
}

func (f *FunctionMODE) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	argTypes(engine, &f.BaseFunction, result, OperandTypeArray, OperandTypeNumber)
}

func init() { RegisterFunction("MODE", newFunctionMODE) }
