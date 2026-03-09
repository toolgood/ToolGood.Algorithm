using System;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum2
{
	internal sealed class Function_TDIST : Function_3
    {
		public Function_TDIST(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "TDist";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsErrorOrNone) return args1;

            var args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsErrorOrNone) return args2;

            var args3 = GetNumber_3(engine, tempParameter);
            if (args3.IsErrorOrNone) return args3;
            var x = args1.DoubleValue;
            var degreesFreedom = args2.IntValue;
            var tails = args3.IntValue;
            if (degreesFreedom <= 0) {
                return ParameterError(2);
            }
            if (tails < 1 || tails > 2) {
                return ParameterError(3);
            }
            return Operand.Create(ExcelFunctions.TDist(x, degreesFreedom, tails));
        }
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}
	}

}
