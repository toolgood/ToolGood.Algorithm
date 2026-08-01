using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal sealed class Function_POWER : Function_2
    {
		public Function_POWER(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length != 2) {
				throw new ArgumentException($"Function '{Name}' requires exactly 2 parameters.");
			}
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
			if (baseValue < 0 && exponent % 1 != 0) {
				return ParameterError(1);
			}

			try {
				// MathEx.Pow 对超大结果会抛 OverflowException,捕获并返回错误
				return Operand.Create(MathEx.Pow(baseValue, exponent));
			} catch (OverflowException) {
				return FunctionError();
			} catch (InvalidOperationException) {
				return ParameterError(1);
			}
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
