using System;
using System.Text;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_LOGINV : Function_3
    {
		public Function_LOGINV(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "LogInv";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsError) return args1;

            var args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsError) return args2;

            var args3 = GetNumber_3(engine, tempParameter);
            if (args3.IsError) return args3;

            var n3 = args3.DoubleValue;
            if (n3 < 0.0) {
                return FunctionError();
            }
            return Operand.Create(ExcelFunctions.LogInv(args1.DoubleValue, args2.DoubleValue, n3));
        }

    }

}
