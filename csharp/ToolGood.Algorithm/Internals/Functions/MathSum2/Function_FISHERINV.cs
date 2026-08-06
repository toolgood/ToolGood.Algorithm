using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathSum2
{
	internal sealed class Function_FISHERINV : Function_1
    {
        public Function_FISHERINV(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length != 1) {
				throw new ArgumentException($"Function '{Name}' requires exactly 1 parameter.");
			}
		}

        public override string Name => "FisherInv";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsErrorOrNone) { return args1; }
            var x = args1.NumberValue;
            try {
                var n = (MathEx.Exp((2 * x)) - 1) / (MathEx.Exp((2 * x)) + 1);
                return Operand.Create(n);
            } catch (OverflowException) {
                // 2x 超出 decimal 可表示范围(约 ±66)时,tanh 趋近于 ±1,与 Java 版 double 计算结果一致
                return Operand.Create(x >= 0 ? 1.0m : -1.0m);
            }
        }
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}

}
