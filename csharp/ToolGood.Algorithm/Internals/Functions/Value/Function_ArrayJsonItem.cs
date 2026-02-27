using System;
using System.Text;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.Internals.Functions.Value
{
	internal class Function_ArrayJsonItem : Function_1
	{
		private readonly string key;

		public Function_ArrayJsonItem(string key, FunctionBase func1) : base(func1)
		{
			this.key = key;
		}

		public override string Name => "ArrayJsonItem";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var keyValue = new KeyValue {
				Key = key,
				Value = func1.Evaluate(engine, tempParameter)
			};
			return new OperandKeyValue(keyValue);
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			stringBuilder.Append(key);
			stringBuilder.Append(':');
			func1.ToString(stringBuilder, false);
		}

	}

}
