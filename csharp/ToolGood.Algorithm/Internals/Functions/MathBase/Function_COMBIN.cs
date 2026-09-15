using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal sealed class Function_COMBIN : Function_2
    {
		public Function_COMBIN(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length != 2) {
				throw new ArgumentException($"Function '{Name}' requires exactly 2 parameters.");
			}
		}

        public override string Name => "Combin";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsErrorOrNone) { return args1; }

            var args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsErrorOrNone) { return args2; }

			// 先检查数值范围,避免 (int)decimal 强转超范围抛 OverflowException
			var num1 = args1.NumberValue;
			var num2 = args2.NumberValue;
			if (num1 < int.MinValue || num1 > int.MaxValue) {
				return ParameterError(1);
			}
			if (num2 < int.MinValue || num2 > int.MaxValue) {
				return ParameterError(2);
			}
            var total = (int)num1;
            var count = (int)num2;
            if (total < 0) {
                return ParameterError(1);
            }
            if (count < 0) {
                return ParameterError(2);
            }
            if (total < count) {
                return ParameterError(2);
            }
            // C(n,k) 具有对称性 C(n,k)=C(n,n-k)，取较小的 k 计算，避免大 count 时中间乘积溢出
            var k = Math.Min(count, total - count);
            decimal result = 1;
            try {
                for (int i = 1; i <= k; i++) {
                    // 边乘边除:每步结果都等于某个组合数，不会产生无谓的中间膨胀
                    result = result * (total - k + i) / i;
                }
            } catch (OverflowException) {
                return NumError();
            }
            return Operand.Create(result);
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
