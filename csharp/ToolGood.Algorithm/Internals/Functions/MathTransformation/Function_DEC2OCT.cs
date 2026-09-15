using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathTransformation
{
	internal sealed class Function_DEC2OCT : Function_2
    {
		public Function_DEC2OCT(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length < 1 || funcs.Length > 2) {
				throw new ArgumentException($"Function '{Name}' requires 1 to 2 parameters.");
			}
		}

        public override string Name => "Dec2Oct";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsErrorOrNone) { return args1; }
            // Excel 会先截去小数部分再校验范围:-536870912 ~ 536870911
            var numValue = decimal.Truncate(args1.NumberValue);
            if (numValue < -NumberBaseConverter.OctHalf || numValue > NumberBaseConverter.OctHalf - 1) {
                return NumError();
            }
            var num = (int)numValue;
            int? places = null;
            if (func2 != null) {
                var args2 = GetNumber_2(engine, tempParameter);
                if (args2.IsErrorOrNone) { return args2; }
                // places 校验先于负数分支:places 非法时即使 number 为负也应返回 #NUM!
                if (NumberBaseConverter.TryGetPlaces(args2, out var value) == false) { return NumError(); }
                places = value;
            }
            if (num < 0) {
                // 负数:返回 10 位八进制补码,按 Excel 语义忽略 places
                return Operand.Create(Convert.ToString(num & NumberBaseConverter.OctMask, 8).PadLeft(NumberBaseConverter.MaxPlaces, '0'));
            }
            if (NumberBaseConverter.TryFormat(Convert.ToString(num, 8), false, places, out var oct) == false) {
                return NumError();
            }
            return Operand.Create(oct);
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
		}
	}

}
