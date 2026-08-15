using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{

	internal sealed class Function_LOOKCEILING : Function_2
	{
		public Function_LOOKCEILING(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length != 2) {
				throw new ArgumentException($"Function '{Name}' requires exactly 2 parameters.");
			}
		}

		public override string Name => "LookCeiling";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetNumber_1(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }

			var args2 = GetArray_2(engine, tempParameter);
			if(args2.IsErrorOrNone) { return args2; }

			List<decimal> list = new List<decimal>();
			var o = FunctionUtil.FlattenToList(args2, list);
			if(o == false) { return ParameterError(2); }
			if(list.Count == 0) { return ParameterError(2); }
			list.Sort();
			var value = args1.NumberValue;
			int index = list.BinarySearch(value);
			if(index >= 0) { return args1; }
			index = ~index; // 第一个大于 value 的索引
			if(index == list.Count) {
				// 所有元素都小于 value，返回最大值
				return Operand.Create(list[list.Count - 1]);
			}
			return Operand.Create(list[index]);
		}

		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			func2.GetParameterTypes(noneEngine, result, OperandType.ARRAY);
		}

	}

}
