using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal sealed class Function_MID : Function_3
	{
		public Function_MID(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length != 3) {
				throw new ArgumentException($"Function '{Name}' requires exactly 3 parameters.");
			}
		}

		public override string Name => "Mid";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }
			var args2 = GetNumber_2(engine, tempParameter);
			if(args2.IsErrorOrNone) { return args2; }
			var args3 = GetNumber_3(engine, tempParameter);
			if(args3.IsErrorOrNone) { return args3; }

			var text = args1.TextValue;
			// 数值超出 int 范围时按参数错误处理,避免 OverflowException 外泄
			if (TryGetInt(args2, out int startValue) == false) {
				return ParameterError(2);
			}
			if (TryGetInt(args3, out int length) == false) {
				return ParameterError(3);
			}
			// 用 long 计算,避免 startValue 为 int.MinValue 时相减溢出
			var startIndex = (long)startValue - engine.ExcelIndex;

			if(startIndex < 0) {
				return ParameterError(2);
			}
			if(length < 0) {
				return ParameterError(3);
			}
			if(startIndex == 0 && length >= text.Length) {
				return args1;
			}
			if(startIndex >= text.Length) {
				return Operand.Create(string.Empty);
			}
			// 用减法比较,避免 startIndex + length 溢出为负数
			if(length > text.Length - startIndex) {
				length = text.Length - (int)startIndex;
			}
			return Operand.Create(text.Substring((int)startIndex, length));
		}
		public override OperandType GetResultType()
		{
			return OperandType.TEXT;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
			func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}

}
