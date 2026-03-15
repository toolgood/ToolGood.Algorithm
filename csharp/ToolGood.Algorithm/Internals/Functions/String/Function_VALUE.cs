using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.String
{

	internal sealed class Function_VALUE : Function_1
	{
		public Function_VALUE(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "Value";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(engine, tempParameter);
			if(args1.IsNumber) { return args1; }
			if(args1.IsBoolean) { return args1.BooleanValue ? Operand.One : Operand.Zero; }
			if(args1.IsErrorOrNone) { return args1; }

			if(decimal.TryParse(args1.TextValue.AsSpan(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
				return Operand.Create(d);
			}
			return ParameterError(1);
		}
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
		}
	}

}
