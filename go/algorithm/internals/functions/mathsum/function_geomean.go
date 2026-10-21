package mathsum

import "github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals"

// FunctionGEOMEAN 对应 C# Function_GEOMEAN（几何平均数）。
type FunctionGEOMEAN struct{ BaseFunction }

func newFunctionGEOMEAN(args []Function) Function {
	RequireArgs(args, "GeoMean", 1, -1)
	return &FunctionGEOMEAN{BaseFunction: NewBase("GeoMean", OperandTypeNumber, args)}
}

func (f *FunctionGEOMEAN) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	var args []Operand
	if err := f.EvaluateAll(engine, tp, &args); err != nil {
		return err
	}

	var list []Decimal
	if !internals.FlattenToBigDecimalList(args, &list) {
		return f.ParameterError(1)
	}
	if len(list) == 0 {
		return f.ParameterError(1)
	}
	product := DecimalOne
	for _, v := range list {
		if v.LessOrEqual(DecimalZero) {
			return f.ParameterError(1)
		}
		var ok bool
		product, ok = mulChecked(product, v)
		// 连乘可能溢出 decimal 范围，与 C# 捕获 OverflowException 一致
		if !ok {
			return f.FunctionError()
		}
	}
	geoMean, ok := powDecimal(product, DecimalOne.Div(DecFromInt(int64(len(list)))))
	if !ok {
		return f.FunctionError()
	}
	return CreateDecimal(geoMean)
}

func (f *FunctionGEOMEAN) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	argTypes(engine, &f.BaseFunction, result, OperandTypeArray, OperandTypeNumber)
}

func init() { RegisterFunction("GEOMEAN", newFunctionGEOMEAN) }
