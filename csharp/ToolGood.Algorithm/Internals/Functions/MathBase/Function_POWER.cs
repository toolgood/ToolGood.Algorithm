using System;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal sealed class Function_POWER : Function_2
    {
		public Function_POWER(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "Power";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
			if (args1.IsError) { return args1; }

			var args2 = GetNumber_2(engine, tempParameter);
			if (args2.IsError) { return args2; }

			var baseValue = args1.NumberValue;
			var exponent = args2.NumberValue;

			if (baseValue == 0 && exponent < 0) {
				return Div0Error();
			}

			return Operand.Create(MathEx.Pow(baseValue, exponent));
        }
		public override OperandType GetRestltType()
		{
			return OperandType.NUMBER;
		}
	}

    

}
