using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathTransformation
{
	internal sealed class Function_BIN2OCT : Function_2
    {
		public Function_BIN2OCT(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length < 1 || funcs.Length > 2) {
				throw new ArgumentException($"Function '{Name}' requires 1 to 2 parameters.");
			}
		}

        public override string Name => "Bin2Oct";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetText_1(engine, tempParameter);
            if (args1.IsErrorOrNone) { return args1; }

            // 文本裁剪空白、校验字符与长度(<=10),并解析 10 位二进制补码
            var value = NumberBaseConverter.ParseComplement(args1.TextValue, 2);
            if (value.HasValue == false) { return NumError(); }
            var bin = (int)value.Value;

            int? places = null;
            if (func2 != null) {
                var args2 = GetNumber_2(engine, tempParameter);
                if (args2.IsErrorOrNone) { return args2; }
                // places 校验先于结果分支:places 非法时即使 number 为负也应返回 #NUM!
                if (NumberBaseConverter.TryGetPlaces(args2, out var p) == false) { return NumError(); }
                places = p;
            }

            if (bin < 0) {
                // 负数:10 位八进制补码,按 Excel 语义忽略 places
                return Operand.Create(Convert.ToString(bin & NumberBaseConverter.OctMask, 8).PadLeft(NumberBaseConverter.MaxPlaces, '0'));
            }
            if (NumberBaseConverter.TryFormat(Convert.ToString(bin, 8), false, places, out var oct) == false) {
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
			func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
			if(func2 != null) {
				func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			}
		}
	}

}
