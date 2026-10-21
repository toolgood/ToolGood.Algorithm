package csharp

import "sort"

// FunctionLOOKFLOOR 对应 C# Function_LOOKFLOOR：在有序候选集中取不大于 value 的最大值。
type FunctionLOOKFLOOR struct{ BaseFunction }

func newFunctionLOOKFLOOR(args []Function) Function {
	RequireArgs(args, "LookFloor", 2, 2)
	return &FunctionLOOKFLOOR{BaseFunction: NewBase("LookFloor", OperandTypeNumber, args)}
}

func (f *FunctionLOOKFLOOR) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
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
	if index == 0 {
		// 所有元素都大于 value，返回最小值
		return CreateDecimal(list[0])
	}
	return CreateDecimal(list[index-1])
}

func init() { RegisterFunction("LOOKFLOOR", newFunctionLOOKFLOOR) }
