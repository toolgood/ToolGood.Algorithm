using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Flow
{
	internal sealed class Function_SWITCH : Function_N
	{
		public Function_SWITCH(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "Switch";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var exprValue = funcs[0].Evaluate(engine, tempParameter);
			if(exprValue.IsErrorOrNone) { return exprValue; }

			int i = 1;
			while(i < funcs.Length - 1) {
				var compareValue = funcs[i].Evaluate(engine, tempParameter);
				if(compareValue.IsErrorOrNone) { return compareValue; }

				if(EqualsOperand(exprValue, compareValue)) {
					return funcs[i + 1].Evaluate(engine, tempParameter);
				}
				i += 2;
			}

			// 参数数量为偶数时，最后一个参数是默认值
			// 例如: SWITCH(expr, v1, r1, default) - 4个参数
			//       SWITCH(expr, v1, r1, v2, r2, default) - 6个参数
			if (funcs.Length % 2 == 0) {
				return funcs[funcs.Length - 1].Evaluate(engine, tempParameter);
			}

			return FunctionError();
		}

		private bool EqualsOperand(Operand a, Operand b)
		{
			if(a.IsNumber && b.IsNumber) {
				return a.NumberValue == b.NumberValue;
			}
			if(a.IsText && b.IsText) {
				return a.TextValue == b.TextValue;
			}
			if(a.IsBoolean && b.IsBoolean) {
				return a.BooleanValue == b.BooleanValue;
			}
			if(a.IsNull && b.IsNull) {
				return true;
			}
			return false;
		}
		public override OperandType GetResultType()
		{
			int i = 1;
			while(i < funcs.Length - 1) {
				var t = funcs[i + 1].GetResultType();
				if(t != OperandType.NONE) {
					return t;
				}
				i += 2;
			}
			// 检查默认值的类型
			if (funcs.Length % 2 == 0) {
				var t = funcs[funcs.Length - 1].GetResultType();
				if(t != OperandType.NONE) {
					return t;
				}
			}
			return OperandType.NONE;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			funcs[0].GetParameterTypes(noneEngine, result, OperandType.NONE);
			int i = 1;
			while(i < funcs.Length - 1) {
				funcs[i].GetParameterTypes(noneEngine, result, OperandType.NONE);
				funcs[i + 1].GetParameterTypes(noneEngine, result, OperandType.NONE);
				i += 2;
			}
			// 默认值参数
			if (funcs.Length % 2 == 0) {
				funcs[funcs.Length - 1].GetParameterTypes(noneEngine, result, OperandType.NONE);
			}
		}
	}
}
