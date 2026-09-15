using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal sealed class Function_SUMPRODUCT : Function_N
	{
		public Function_SUMPRODUCT(FunctionBase[] funcs) : base(funcs) {
			if (funcs.Length < 1) {
				throw new ArgumentException($"Function '{Name}' requires at least 1 parameter.");
			}
		}

		public override string Name => "SUMPRODUCT";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var arrays = new List<List<decimal>>();
			for (int i = 0; i < funcs.Length; i++) {
				var arg = GetArray(engine, tempParameter, i);
				if (arg.IsErrorOrNone) return arg;
				var list = new List<decimal>(arg.ArrayValue.Count);
				foreach (var item in arg.ArrayValue) {
					// 与 Excel 一致:非数值项按 0 参与运算,保留占位以保证各数组元素位置对齐
					list.Add(item.IsNumber ? item.NumberValue : 0m);
				}
				arrays.Add(list);
			}

			// 与 Excel 一致:各数组长度必须相同,否则返回错误
			int length = arrays[0].Count;
			for (int i = 1; i < arrays.Count; i++) {
				if (arrays[i].Count != length) {
					return FunctionError();
				}
			}

			decimal result = 0;
			for (int i = 0; i < length; i++) {
				decimal product = 1;
				for (int j = 0; j < arrays.Count; j++) {
					product *= arrays[j][i];
				}
				result += product;
			}

			return Operand.Create(result);
		}
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			for(int i = 0; i < funcs.Length; i++) {
				funcs[i].GetParameterTypes(noneEngine, result, OperandType.ARRAY);
			}
		}
	}
}
