using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathTransformation
{
	internal sealed class Function_HEX2BIN : Function_2
    {
		public Function_HEX2BIN(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length < 1 || funcs.Length > 2) {
				throw new ArgumentException($"Function '{Name}' requires 1 to 2 parameters.");
			}
		}

        public override string Name => "Hex2Bin";

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
            // Excel HEX2BIN 结果范围为 -512~511
            if (num < -512 || num > 511) {
                return ParameterError(1);
            }
            string bin;
            if (num < 0) {
                // 负数:10 位二进制补码
                bin = Convert.ToString(num & 1023, 2).PadLeft(10, '0');
            } else {
                bin = Convert.ToString(num, 2);
            }
            if (func2 != null) {
                var args2 = GetNumber_2(engine, tempParameter);
                if (args2.IsErrorOrNone) { return args2; }
                if (args2.IntValue < 0 || args2.IntValue > 10) {
                    return ParameterError(2);
                }
                if (bin.Length > args2.IntValue) {
                    return ParameterError(2);
                }
                return Operand.Create(bin.PadLeft(args2.IntValue, '0'));
            }
            return Operand.Create(bin);
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
