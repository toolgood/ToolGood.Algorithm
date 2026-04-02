using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal sealed class Function_ROUNDDOWN : Function_2
    {
		public Function_ROUNDDOWN(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length != 2) {
				throw new ArgumentException($"Function '{Name}' requires exactly 2 parameters.");
			}
		}

        public override string Name => "RoundDown";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
			if (args1.IsErrorOrNone) { return args1; }

			var args2 = GetNumber_2(engine, tempParameter);
			if (args2.IsErrorOrNone) { return args2; }
			if (args2.IntValue < -15 || args2.IntValue > 15) {
				return ParameterError(2);
			}
            if (args1.NumberValue == 0) {
                return args1;
            }
            var num = args1.NumberValue;
            var digits = args2.IntValue;
            var factor = MathEx.Pow(10, digits);

            decimal result;
            if (num > 0) {
                result = Math.Floor(num * factor) / factor;
            } else {
                result = Math.Ceiling(num * factor) / factor;
            }
            return Operand.Create(result);
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
