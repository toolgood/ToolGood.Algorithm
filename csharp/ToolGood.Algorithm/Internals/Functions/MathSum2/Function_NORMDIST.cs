using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum2
{
	internal sealed class Function_NORMDIST : Function_4
    {
		public Function_NORMDIST(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length != 4) {
				throw new ArgumentException($"Function '{Name}' requires exactly 4 parameters.");
			}
		}

        public override string Name => "NormDist";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsErrorOrNone) return args1;

            var args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsErrorOrNone) return args2;

            var args3 = GetNumber_3(engine, tempParameter);
            if (args3.IsErrorOrNone) return args3;

            var args4 = GetBoolean_4(engine, tempParameter);
            if (args4.IsErrorOrNone) return args4;

            var num = args1.NumberValue;
            var avg = args2.NumberValue;
            var STDEV = args3.NumberValue;
            if (STDEV <= 0m) {
                return ParameterError(3);
            }
            var b = args4.BooleanValue;

            try {
                return Operand.Create(ExcelFunctions.NormDist(num, avg, STDEV, b));
            } catch (Exception ex) when (ex is OverflowException || ex is DivideByZeroException || ex is InvalidOperationException || ex is ArgumentException) {
                // |num-avg|/STDEV 过大时指数运算结果超出 decimal 范围，与 Excel 返回 #NUM! 保持一致
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
			func4.GetParameterTypes(noneEngine, result, OperandType.BOOLEAN);
		}
	}

}
