using System;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathTrigonometric
{
	internal sealed class Function_ACOS : Function_1
    {
        public Function_ACOS(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "Acos";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsErrorOrNone) { return args1; }
            var x = args1.NumberValue;
            if (x < -1 || x > 1) {
                return ParameterError(1);
            }
            return Operand.Create(MathEx.Acos(x));
        }
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}
	}


}
