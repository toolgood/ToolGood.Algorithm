using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathTransformation
{
	internal sealed class Function_BIN2HEX : Function_2
	{
		public Function_BIN2HEX(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length < 1 || funcs.Length > 2) {
				throw new ArgumentException($"Function '{Name}' requires 1 to 2 parameters.");
			}
		}

		public override string Name => "Bin2Hex";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }

			// 文本裁剪空白、校验字符与长度(<=10),并解析 10 位二进制补码
			var value = NumberBaseConverter.ParseComplement(args1.TextValue, 2);
			if (value.HasValue == false) { return NumError(); }
			var bin = value.Value;
			int? places = null;
			if(func2 != null) {
				var args2 = GetNumber_2(engine, tempParameter);
				if(args2.IsErrorOrNone) { return args2; }
				if(NumberBaseConverter.TryGetPlaces(args2, out var p) == false) { return NumError(); }
				places = p;
			}
			string hex;
			if (bin < 0) {
				// 负数:10 位十六进制补码,按 Excel 语义忽略 places
				hex = (bin & NumberBaseConverter.HexMask).ToString("X").PadLeft(NumberBaseConverter.MaxPlaces, '0');
			} else {
				hex = bin.ToString("X");
			}
			if(NumberBaseConverter.TryFormat(hex, bin < 0, places, out var result) == false) {
				return NumError();
			}
			return Operand.Create(result);
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
