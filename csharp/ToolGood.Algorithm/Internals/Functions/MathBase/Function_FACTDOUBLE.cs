using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal sealed class Function_FACTDOUBLE : Function_1
    {
        public Function_FACTDOUBLE(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length != 1) {
				throw new ArgumentException($"Function '{Name}' requires exactly 1 parameter.");
			}
		}

        public override string Name => "FactDouble";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
			if (args1.IsErrorOrNone) { return args1; }
			// 先检查数值范围,避免 (int)decimal 强转超范围抛 OverflowException
			var number = args1.NumberValue;
			if (number < int.MinValue || number > int.MaxValue) {
				return ParameterError(1);
			}
            var z = (int)number;
            if (z < 0) { return ParameterError(1); }
            // decimal 最大可容纳 45!!(≈2.537e28),46!! 溢出;与 FACT 的 28 限制不同
            if (z > 45) { return ParameterError(1); }

            decimal d = 1;
            for (int i = z; i > 0; i -= 2) {
                d *= i;
            }
            return Operand.Create(d);
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
