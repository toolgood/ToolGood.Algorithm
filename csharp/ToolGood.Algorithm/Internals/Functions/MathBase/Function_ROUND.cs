using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal sealed class Function_ROUND : Function_2
    {
		public Function_ROUND(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length < 1 || funcs.Length > 2) {
				throw new ArgumentException($"Function '{Name}' requires 1 to 2 parameters.");
			}
		}

        public override string Name => "Round";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
			if (args1.IsErrorOrNone) { return args1; }

			if (func2 == null) {
				return Operand.Create(Math.Round(args1.NumberValue, 0, MidpointRounding.AwayFromZero));
			}
			var args2 = GetNumber_2(engine, tempParameter);
			if (args2.IsErrorOrNone) { return args2; }
			var digits = args2.IntValue;
			if (digits < -15 || digits > 15) {
				return ParameterError(2);
			}
			var num = args1.NumberValue;
			if (digits >= 0) {
				return Operand.Create(Math.Round(num, digits, MidpointRounding.AwayFromZero));
			}
			// Math.Round(decimal, int) 只支持非负位数,负数位数(向左取整)改用先除后乘
			var factor = MathEx.Pow(10, -digits);
			return Operand.Create(Math.Round(num / factor, 0, MidpointRounding.AwayFromZero) * factor);
        }
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			if(func2 != null) {
				func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			}
		}
	}

}
