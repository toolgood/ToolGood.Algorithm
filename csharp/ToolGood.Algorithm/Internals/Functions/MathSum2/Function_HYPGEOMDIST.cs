using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;
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
            if (args1.IsErrorOrNone) return args1;

            var args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsErrorOrNone) return args2;

            var args3 = GetNumber_3(engine, tempParameter);
            if (args3.IsErrorOrNone) return args3;

            var args4 = GetNumber_4(engine, tempParameter);
            if (args4.IsErrorOrNone) return args4;

            int k = args1.IntValue;
            int draws = args2.IntValue;
            if (draws < 0) {
                return ParameterError(2);
            }
            int success = args3.IntValue;
            if (success < 0) {
                return ParameterError(3);
            }
            int population = args4.IntValue;
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

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			func4.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}

}
