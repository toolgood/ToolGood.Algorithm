using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathTransformation
{
	internal sealed class Function_HEX2DEC : Function_2
    {
		public Function_HEX2DEC(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length < 1 || funcs.Length > 2) {
				throw new ArgumentException($"Function '{Name}' requires 1 to 2 parameters.");
			}
		}

		public override string Name => "Hex2Dec";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetText_1(engine, tempParameter);
            if (args1.IsErrorOrNone) { return args1; }

            if(RegexHelper.IsHex(args1.TextValue) == false) { return ParameterError(1); }
            var text = args1.TextValue;
            if (text.Length > 10) { return ParameterError(1); }
            // 10 位十六进制补码解析:最高位(bit39)为 1 时表示负数
            var num = Convert.ToInt64(text, 16);
            if (num >= 0x8000000000L) { num -= 0x10000000000L; }
			if(func2 != null) {
				var args2 = GetNumber_2(engine, tempParameter);
				if(args2.IsErrorOrNone) { return args2; }
				if(args2.IntValue < 0) {
					return ParameterError(2);
				}
				var n = num.ToString();
				if(n.Length <= args2.IntValue) {
					if(num < 0) {
						n = "-" + n.Substring(1).PadLeft(args2.IntValue - 1, '0');
					} else {
						n = n.PadLeft(args2.IntValue, '0');
					}
					return Operand.Create(n);
				}
				return ParameterError(2);
			}
			return Operand.Create((decimal)num);
        }
		public override OperandType GetResultType()
		{
			if(func2 != null) {
				return OperandType.TEXT;
			}
			return OperandType.NUMBER;
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
