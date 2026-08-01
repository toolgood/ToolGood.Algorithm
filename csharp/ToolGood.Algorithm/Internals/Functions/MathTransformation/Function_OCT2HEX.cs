using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathTransformation
{
	internal sealed class Function_OCT2HEX : Function_2
    {
		public Function_OCT2HEX(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length < 1 || funcs.Length > 2) {
				throw new ArgumentException($"Function '{Name}' requires 1 to 2 parameters.");
			}
		}

        public override string Name => "Oct2Hex";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetText_1(engine, tempParameter);
            if (args1.IsErrorOrNone) { return args1; }

            if(RegexHelper.IsOct(args1.TextValue) == false) { return ParameterError(1); }
            var text = args1.TextValue;
            if (text.Length > 10) { return ParameterError(1); }
            // 10 位八进制补码解析
            var num = Convert.ToInt32(text, 8);
            if (num >= 536870912) { num -= 1073741824; }
            string hex;
            if (num < 0) {
                // 负数:10 位十六进制补码
                hex = ((long)num + 0x10000000000L).ToString("X").PadLeft(10, '0');
            } else {
                hex = num.ToString("X");
            }
            if (func2 != null) {
                var args2 = GetNumber_2(engine, tempParameter);
                if (args2.IsErrorOrNone) { return args2; }
                if (args2.IntValue < 0) {
                    return ParameterError(2);
                }
                if (hex.Length > args2.IntValue) {
                    return ParameterError(2);
                }
                return Operand.Create(hex.PadLeft(args2.IntValue, '0'));
            }
            return Operand.Create(hex);
        }
		public override OperandType GetResultType()
		{
			return OperandType.TEXT;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
			if(func2 != null) {
				func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			}
		}
	}

}
