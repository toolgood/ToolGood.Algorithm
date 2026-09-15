using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum2
{
	internal sealed class Function_HYPGEOMDIST : Function_4
    {
		public Function_HYPGEOMDIST(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length != 4) {
				throw new ArgumentException($"Function '{Name}' requires exactly 4 parameters.");
			}
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

            if (!TryGetInt(args1, out var k) || k < 0) {
                return ParameterError(1);
            }
            if (!TryGetInt(args2, out var draws) || draws < 0) {
                return ParameterError(2);
            }
            if (!TryGetInt(args3, out var success) || success < 0) {
                return ParameterError(3);
            }
            if (!TryGetInt(args4, out var population) || population < 0) {
                return ParameterError(4);
            }
            // 交叉约束：k 必须落在 [max(0, draws+success-population), min(draws, success)] 区间内
            if (k > draws) {
                return ParameterError(1);
            }
            if (k > success) {
                return ParameterError(1);
            }
            if (k < (long)draws + success - population) {
                return ParameterError(1);
            }
            if (success > population) {
                return ParameterError(3);
            }
            if (draws > population) {
                return ParameterError(2);
            }
            try {
                return Operand.Create(ExcelFunctions.HypgeomDist(k, draws, success, population));
            } catch (Exception ex) when (ex is OverflowException || ex is DivideByZeroException || ex is InvalidOperationException || ex is ArgumentException) {
                // 极端输入导致底层数值计算失败时，与 Excel 返回 #NUM! 保持一致
                return FunctionError();
            }
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
