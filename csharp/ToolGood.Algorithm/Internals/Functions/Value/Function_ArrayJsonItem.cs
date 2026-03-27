using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.Internals.Functions.Value
{
	internal sealed class Function_ArrayJsonItem : Function_1
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
		public override OperandType GetResultType()
		{
			return OperandType.ARRAYJSON;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NONE);
		}
	}
}
