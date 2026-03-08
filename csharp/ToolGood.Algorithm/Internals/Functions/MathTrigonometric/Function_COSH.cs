using System;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathTrigonometric
{
	internal sealed class Function_COSH : Function_1
    {
        public Function_COSH(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "Cosh";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsError) { return args1; }
            return Operand.Create(MathEx.Cosh(args1.NumberValue));
        }
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}
	}


}
