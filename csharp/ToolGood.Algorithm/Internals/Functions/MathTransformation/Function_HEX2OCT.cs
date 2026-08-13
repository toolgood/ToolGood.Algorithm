using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathTransformation
{
	internal sealed class Function_HEX2OCT : Function_2
    {
		public Function_HEX2OCT(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length < 1 || funcs.Length > 2) {
				throw new ArgumentException($"Function '{Name}' requires 1 to 2 parameters.");
			}
		}

        public override string Name => "Hex2Oct";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetText_1(engine, tempParameter);
            if (args1.IsErrorOrNone) { return args1; }
            if(RegexHelper.IsHex(args1.TextValue) == false) { return ParameterError(1); }
            var text = args1.TextValue;
            if (text.Length > 10) { return ParameterError(1); }
            // 10 位十六进制补码解析
            var num = Convert.ToInt64(text, 16);
            if (num >= 0x8000000000L) { num -= 0x10000000000L; }
            // Excel HEX2OCT 结果范围为 -536870912~536870911
            if (num < -536870912L || num > 536870911L) {
                return ParameterError(1);
            }
            string oct;
            if (num < 0) {
                // 负数:10 位八进制补码
                oct = Convert.ToString(num & 0x3FFFFFFF, 8).PadLeft(10, '0');
            } else {
                oct = Convert.ToString(num, 8);
            }
            if (func2 != null) {
                var args2 = GetNumber_2(engine, tempParameter);
                if (args2.IsErrorOrNone) { return args2; }
                if (args2.IntValue < 0 || args2.IntValue > 10) {
                    return ParameterError(2);
                }
                if (oct.Length > args2.IntValue) {
                    return ParameterError(2);
                }
                return Operand.Create(oct.PadLeft(args2.IntValue, '0'));
            }
            return Operand.Create(oct);
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
