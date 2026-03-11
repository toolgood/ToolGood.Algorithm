using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.Value
{
	internal sealed class Function_Parameter : FunctionBase
	{
		private readonly string name;

		public Function_Parameter(string name)
		{
			this.name = name;
		}

		public override string Name => "Parameter";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var txt = name;
			if(tempParameter != null) {
				var r = tempParameter(engine, txt);
				if(r != null) return r;
			}
			return engine.GetParameter(txt);
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			stringBuilder.Append(name);
		}
		public override OperandType GetResultType()
		{
			return OperandType.NONE;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			result.Add(new ParameterType() {
				Name = name,
				Type = operandType,
				Operator = op,
				Value = val
			});
		}
	}

}
