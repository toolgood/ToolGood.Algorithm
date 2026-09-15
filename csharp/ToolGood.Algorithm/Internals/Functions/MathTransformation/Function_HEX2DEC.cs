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

            // 文本裁剪空白、校验字符与长度(<=10),并解析 10 位十六进制补码
            var value = NumberBaseConverter.ParseComplement(args1.TextValue, 16);
            if (value.HasValue == false) { return NumError(); }
            var num = value.Value;
			if(func2 != null) {
				var args2 = GetNumber_2(engine, tempParameter);
				if(args2.IsErrorOrNone) { return args2; }
				if(NumberBaseConverter.TryGetPlaces(args2, out var places) == false) { return NumError(); }
				if(NumberBaseConverter.TryFormatDecimal(num, places, out var n) == false) { return NumError(); }
				return Operand.Create(n);
			}
			return Operand.Create(num);
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
