using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal sealed class Function_FLOOR : Function_2
    {
		public Function_FLOOR(FunctionBase[] funcs) : base(funcs)
		{
		}

        public override string Name => "Floor";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
			if (args1.IsErrorOrNone) { return args1; }

			if (func2 == null)
                return Operand.Create(Math.Floor(args1.NumberValue));

            var args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsErrorOrNone) { return args2; }
            var b = args2.NumberValue;
            if (b == 0) { return Operand.Zero; }

            var a = args1.NumberValue;

            if (b > 0) {
                var d = Math.Floor(a / b) * b;
                return Operand.Create(d);
            } else {
                if (a > 0) {
                    return ParameterError(1);
                }
                var d = Math.Floor(a / b) * b;
                return Operand.Create(d);
            }
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
