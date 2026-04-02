using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum2
{
	internal sealed class Function_FINV : Function_3
    {
		public Function_FINV(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length != 3) {
				throw new ArgumentException($"Function '{Name}' requires exactly 3 parameters.");
			}
		}

        public override string Name => "FInv";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsErrorOrNone) return args1;

            var args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsErrorOrNone) return args2;

            var args3 = GetNumber_3(engine, tempParameter);
            if (args3.IsErrorOrNone) return args3;

            var p = args1.NumberValue;
            if (p <= 0m || p >= 1m) {
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
            return Operand.Create(ExcelFunctions.FInv(p, degreesFreedom, degreesFreedom2));
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
