using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal sealed class Function_TRUNC : Function_2
    {
        public Function_TRUNC(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override string Name => "Trunc";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
			if (args1.IsErrorOrNone) { return args1; }

            var digits = 0;
            if (func2 != null) {
                var args2 = GetNumber_2(engine, tempParameter);
                if (args2.IsErrorOrNone) { return args2; }
                digits = args2.IntValue;
            }

            if (args1.NumberValue == 0) {
                return args1;
            }

            var num = args1.NumberValue;
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
			if (func2 != null) {
				func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			}
		}
	}

}
