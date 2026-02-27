using System;
using System.Text;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_EXPONDIST : Function_3
    {
		public Function_EXPONDIST(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "ExpOnDist";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsError) return args1;

            var args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsError) return args2;

            var args3 = GetBoolean_3(engine, tempParameter);
            if (args3.IsError) return args3;

            var n1 = args1.DoubleValue;
            if (n1 < 0.0) {
                return FunctionError();
            }
            return Operand.Create(ExcelFunctions.ExponDist(n1, args2.DoubleValue, args3.BooleanValue));
        }

    }

}
