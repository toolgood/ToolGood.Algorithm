using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum2
{
	internal sealed class Function_TDIST : Function_3
    {
		public Function_TDIST(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length != 3) {
				throw new ArgumentException($"Function '{Name}' requires exactly 3 parameters.");
			}
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
            var x = args1.NumberValue;
            // Excel 允许 x=0(TDIST(0,df,1)=0.5, TDIST(0,df,2)=1)，仅当 x<0 时报错
            if (x < 0) {
                return ParameterError(1);
            }
            if (!TryGetInt(args2, out var degreesFreedom) || degreesFreedom <= 0) {
                return ParameterError(2);
            }
            if (!TryGetInt(args3, out var tails) || tails < 1 || tails > 2) {
                return ParameterError(3);
            }
            try {
                return Operand.Create(ExcelFunctions.TDist(x, degreesFreedom, tails));
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
		}
	}

}
