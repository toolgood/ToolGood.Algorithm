using System;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathSum2
{
	internal sealed class Function_FISHER : Function_1
    {
        public Function_FISHER(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "Fisher";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsError) { return args1; }
            var x = args1.NumberValue;
            if (x >= 1 || x <= -1) {
                return ParameterError(1);
            }
            var n = 0.5m * MathEx.Log(((1 + x) / (1 - x)));
            return Operand.Create(n);
        }
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}
	}

}
