using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Flow
{
	internal sealed class Function_ISODD : Function_1
    {
        public Function_ISODD(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length != 1) {
				throw new ArgumentException($"Function '{Name}' requires exactly 1 parameter.");
			}
		}

        public override string Name => "IsOdd";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(engine, tempParameter);
            if (args1.IsNumber) {
                // 用 decimal.Truncate 取整,避免 (int)decimal 强转超范围抛 OverflowException
                var number = decimal.Truncate(args1.NumberValue);
                // C# 取模符号跟随被除数,负数奇数取模结果为 -1,需取绝对值判断
                if (Math.Abs(number) % 2 == 1) { return Operand.True; }
            }
            return Operand.False;
        }
		public override OperandType GetResultType()
		{
			return OperandType.BOOLEAN;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}

}
