using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum2
{
	internal sealed class Function_FDIST : Function_3
    {
		public Function_FDIST(FunctionBase[] funcs) : base(funcs)
		{
		}

        public override string Name => "FDist";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsErrorOrNone) return args1;

            var args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsErrorOrNone) return args2;

            var args3 = GetNumber_3(engine, tempParameter);
            if (args3.IsErrorOrNone) return args3;

            var x = args1.NumberValue;
            if (x < 0) {
                return ParameterError(1);
            }
            var degreesFreedom = args2.IntValue;
            if (degreesFreedom <= 0) {
                return ParameterError(2);
            }
            var degreesFreedom2 = args3.IntValue;
            if (degreesFreedom2 <= 0) {
                return ParameterError(3);
            }
            return Operand.Create(ExcelFunctions.FDist(x, degreesFreedom, degreesFreedom2));
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
