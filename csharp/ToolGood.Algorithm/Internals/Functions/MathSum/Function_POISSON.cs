using System;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal sealed class Function_POISSON : Function_3
    {
		public Function_POISSON(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "Poisson";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsErrorOrNone) return args1;

            var args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsErrorOrNone) return args2;

            var args3 = GetBoolean_3(engine, tempParameter);
            if (args3.IsErrorOrNone) return args3;
            int k = args1.IntValue;
            var lambda = args2.DoubleValue;
            bool state = args3.BooleanValue;
            if (!(lambda > 0.0)) {
                return ParameterError(2);
            }
            return Operand.Create(ExcelFunctions.Poisson(k, lambda, state));
        }
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}
	}

}
