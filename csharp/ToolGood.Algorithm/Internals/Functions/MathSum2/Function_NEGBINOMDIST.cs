using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum2
{
	internal sealed class Function_NEGBINOMDIST : Function_3
    {
		public Function_NEGBINOMDIST(FunctionBase[] funcs) : base(funcs)
		{
		}

        public override string Name => "NegBinomDist";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsErrorOrNone) return args1;

            var args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsErrorOrNone) return args2;

            var args3 = GetNumber_3(engine, tempParameter);
            if (args3.IsErrorOrNone) return args3;
            int k = args1.IntValue;
            if (k < 0) {
                return ParameterError(1);
            }
            var r = args2.NumberValue;
            if (r < 0m) {
                return ParameterError(2);
            }
            var p = args3.NumberValue;
            if (p < 0m || p > 1m) {
                return ParameterError(3);
            }
            return Operand.Create(ExcelFunctions.NegbinomDist(k, r, p));
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
		}
	}

}
