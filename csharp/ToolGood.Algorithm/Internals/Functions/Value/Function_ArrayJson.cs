using System;
using System.Text;
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
				result.AddValue((KeyValue)((OperandKeyValue)o).Value);
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
	}

}
