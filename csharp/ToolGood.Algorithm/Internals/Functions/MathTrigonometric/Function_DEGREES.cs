using System;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathTrigonometric
{
	internal sealed class Function_DEGREES : Function_1
    {
        public Function_DEGREES(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "Degrees";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsError) { return args1; }
            var z = args1.NumberValue;
            var r = (z / MathEx.PI * 180);
            return Operand.Create(r);
        }
		public override OperandType GetRestltType()
		{
			return OperandType.NUMBER;
		}
	}

    

}
