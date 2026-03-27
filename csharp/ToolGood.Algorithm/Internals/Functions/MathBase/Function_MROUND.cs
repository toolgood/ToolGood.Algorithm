using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal sealed class Function_MROUND : Function_2
    {
		public Function_MROUND(FunctionBase[] funcs) : base(funcs)
		{
		}

        public override string Name => "Mround";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsErrorOrNone) { return args1; }

            var args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsErrorOrNone) { return args2; }
            var multiple = args2.NumberValue;
            if (multiple == 0) { return ParameterError(2); }

            var number = args1.NumberValue;

            if ((number > 0 && multiple < 0) || (number < 0 && multiple > 0)) {
                return ParameterError(2);
            }

            var r = Math.Round(number / multiple, 0, MidpointRounding.AwayFromZero) * multiple;
            return Operand.Create(r);
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
