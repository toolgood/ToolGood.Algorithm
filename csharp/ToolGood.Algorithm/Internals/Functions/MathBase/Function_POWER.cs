using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal sealed class Function_POWER : Function_2
    {
		public Function_POWER(FunctionBase[] funcs) : base(funcs)
		{
		}

        public override string Name => "Power";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsErrorOrNone) { return args1; }

            var args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsErrorOrNone) { return args2; }

			var baseValue = args1.NumberValue;
			var exponent = args2.NumberValue;

			if (baseValue == 0 && exponent < 0) {
				return Div0Error();
			}
			if (baseValue < 0 && exponent != Math.Floor(exponent)) {
				return ParameterError(1);
			}

			return Operand.Create(MathEx.Pow(baseValue, exponent));
        }
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}

}
