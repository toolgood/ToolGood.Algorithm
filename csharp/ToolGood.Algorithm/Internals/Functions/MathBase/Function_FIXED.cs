using System;
using System.Collections.Generic;
using System.Globalization;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal sealed class Function_FIXED : Function_3
    {
		public Function_FIXED(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length < 1 || funcs.Length > 3) {
				throw new ArgumentException($"Function '{Name}' requires 1 to 3 parameters.");
			}
		}

        public override string Name => "Fixed";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var num = 2;
			if (func2 != null) {
				var args2 = GetNumber_2(engine, tempParameter);
				if (args2.IsErrorOrNone) { return args2; }
				num = args2.IntValue;
				// Excel 支持负数 decimals(向左取整),如 FIXED(1234.567,-1)="1,230",范围与 ROUND 一致
				if (num < -15 || num > 15) {
					return ParameterError(2);
				}
			}
			var args1 = GetNumber_1(engine, tempParameter);
			if (args1.IsErrorOrNone) { return args1; }

			var s = args1.NumberValue;
			if (num >= 0) {
				s = Math.Round(s, num, MidpointRounding.AwayFromZero);
			} else {
				// Math.Round(decimal, int) 只支持非负位数,负数位数(向左取整)改用先除后乘
				var factor = MathEx.Pow(10, -num);
				s = Math.Round(s / factor, 0, MidpointRounding.AwayFromZero) * factor;
			}
			var no = false;
			if (func3 != null) {
				var args3 = GetBoolean_3(engine, tempParameter);
				if (args3.IsErrorOrNone) { return args3; }
				no = args3.BooleanValue;
			}
            if (no == false) {
                if (num < 0) {
                    // 负数位数取整后无小数位,用 N0 保持千分位
                    return Operand.Create(s.ToString("N0", CultureInfo.InvariantCulture));
                }
                return Operand.Create(s.ToString('N' + num.ToString(), CultureInfo.InvariantCulture));
            }
            return Operand.Create(s.ToString(CultureInfo.InvariantCulture));
        }
		public override OperandType GetResultType()
		{
			return OperandType.TEXT;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			if(func2 != null) {
				func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			}
			if(func3 != null) {
				func3.GetParameterTypes(noneEngine, result, OperandType.BOOLEAN);
			}
		}
	}

}
