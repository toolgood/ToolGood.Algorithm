using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum2
{
	internal sealed class Function_TINV : Function_2
    {
		public Function_TINV(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length != 2) {
				throw new ArgumentException($"Function '{Name}' requires exactly 2 parameters.");
			}
		}

        public override string Name => "TInv";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsErrorOrNone) return args1;

            var args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsErrorOrNone) return args2;
            var p = args1.NumberValue;
            // Excel 允许 p=1(TInv(1,df)=0)，仅当 p<=0 或 p>1 时报错
            if (p <= 0m || p > 1m) {
                return ParameterError(1);
            }
            if (!TryGetInt(args2, out var degreesFreedom) || degreesFreedom <= 0) {
                return ParameterError(2);
            }
            try {
                return Operand.Create(ExcelFunctions.TInv(p, degreesFreedom));
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
		}
	}

}
