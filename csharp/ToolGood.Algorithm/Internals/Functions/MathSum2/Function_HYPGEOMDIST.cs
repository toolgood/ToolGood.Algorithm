using System;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum2
{
	internal sealed class Function_HYPGEOMDIST : Function_4
    {
		public Function_HYPGEOMDIST(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "HypgeomDist";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsError) return args1;

            var args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsError) return args2;

            var args3 = GetNumber_3(engine, tempParameter);
            if (args3.IsError) return args3;

            var args4 = GetNumber_4(engine, tempParameter);
            if (args4.IsError) return args4;

            int k = args1.IntValue;
            int draws = args2.IntValue;
            int success = args3.IntValue;
            int population = args4.IntValue;
            if (draws < 0) {
                return ParameterError(2);
            }
            if (success < 0) {
                return ParameterError(3);
            }
            if (population < 0) {
                return ParameterError(4);
            }
            if (success > population || draws > population) {
                return FunctionError();
            }
            return Operand.Create(ExcelFunctions.HypgeomDist(k, draws, success, population));
        }
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}
	}

}
