using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.Internals.Functions.Value
{
	internal sealed class Function_ArrayJson : Function_N
	{
		public Function_ArrayJson(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "ArrayJson";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var result = new OperandKeyValueList();
			foreach (var item in funcs) {
				var o = item.Evaluate(engine, tempParameter);
				if(o.IsErrorOrNone) { return o; }
				if(o is OperandKeyValue kv) {
					result.AddValue(kv.Value);
				} else {
					return ParameterError(1);
				}
			}
			return result;
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			stringBuilder.Append('{');
			for (int i = 0; i < funcs.Length; i++) {
				if (i > 0) {
					stringBuilder.Append(", ");
				}
				funcs[i].ToString(stringBuilder, false);
			}
			stringBuilder.Append('}');
		}
		public override OperandType GetResultType()
		{
			return OperandType.ARRAYJSON;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			for(int i = 0; i < funcs.Length; i++) {
				funcs[i].GetParameterTypes(noneEngine, result, OperandType.NONE);
			}
		}
	}

}
