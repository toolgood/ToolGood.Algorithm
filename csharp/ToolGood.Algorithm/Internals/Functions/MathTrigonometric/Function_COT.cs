using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.MathTrigonometric
{
	internal sealed class Function_COT : Function_1
    {
        public Function_COT(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "Cot";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsErrorOrNone) { return args1; }
            var d = MathEx.Tan(args1.NumberValue);
            if (d == 0) {
                return Div0Error();
            }
            return Operand.Create(1.0m / d);
        }
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}

}
