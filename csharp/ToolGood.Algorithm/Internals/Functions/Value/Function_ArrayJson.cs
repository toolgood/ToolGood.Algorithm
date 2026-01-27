using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Value
{
	internal class Function_ArrayJson : Function_N
	{
		public Function_ArrayJson(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var result = new OperandKeyValueList();
			foreach (var item in funcs) {
				var o = item.Evaluate(work, tempParameter);
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
