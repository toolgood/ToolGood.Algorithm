package csharp

import "sort"

// FunctionLOOKCEILING 对应 C# Function_LOOKCEILING：在有序候选集中取不小于 value 的最小值。
type FunctionLOOKCEILING struct{ BaseFunction }

func newFunctionLOOKCEILING(args []Function) Function {
	RequireArgs(args, "LookCeiling", 2, 2)
	return &FunctionLOOKCEILING{BaseFunction: NewBase("LookCeiling", OperandTypeNumber, args)}
}

func (f *FunctionLOOKCEILING) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetNumber(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	a2 := f.GetArray(2, engine, tp)
	if a2.IsErrorOrNone() {
		return a2
	}
	var list []Decimal
	if !flattenToDecimals(a2, &list) {
		return f.ParameterError(2)
	}
	if len(list) == 0 {
		return f.ParameterError(2)
	}
	sort.Slice(list, func(i, j int) bool { return list[i].Less(list[j]) })
	value := a1.NumberValue()
	index := sort.Search(len(list), func(i int) bool { return list[i].GreaterOrEqual(value) })
	if index < len(list) && list[index].Equal(value) {
		return a1
	}
	if index == len(list) {
		// 所有元素都小于 value，返回最大值
		return CreateDecimal(list[len(list)-1])
	}
	return CreateDecimal(list[index])
}

func init() { RegisterFunction("LOOKCEILING", newFunctionLOOKCEILING) }
