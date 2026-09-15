using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathTransformation
{
	internal sealed class Function_OCT2BIN : Function_2
    {
		public Function_OCT2BIN(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length < 1 || funcs.Length > 2) {
				throw new ArgumentException($"Function '{Name}' requires 1 to 2 parameters.");
			}
		}

        public override string Name => "Oct2Bin";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetText_1(engine, tempParameter);
            if (args1.IsErrorOrNone) { return args1; }

            // 文本裁剪空白、校验字符与长度(<=10),并解析 10 位八进制补码
            var value = NumberBaseConverter.ParseComplement(args1.TextValue, 8);
            if (value.HasValue == false) { return NumError(); }
            var num = (int)value.Value;
            // Excel OCT2BIN 结果范围为 -512~511
            if (num < -NumberBaseConverter.BinHalf || num > NumberBaseConverter.BinHalf - 1) {
                return NumError();
            }

            int? places = null;
            if (func2 != null) {
                var args2 = GetNumber_2(engine, tempParameter);
                if (args2.IsErrorOrNone) { return args2; }
                // places 校验先于结果分支:places 非法时即使 number 为负也应返回 #NUM!
                if (NumberBaseConverter.TryGetPlaces(args2, out var p) == false) { return NumError(); }
                places = p;
            }

            if (num < 0) {
                // 负数:10 位二进制补码,按 Excel 语义忽略 places
                return Operand.Create(Convert.ToString(num & (NumberBaseConverter.BinModulus - 1), 2).PadLeft(NumberBaseConverter.MaxPlaces, '0'));
            }
            if (NumberBaseConverter.TryFormat(Convert.ToString(num, 2), false, places, out var bin) == false) {
                return NumError();
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
