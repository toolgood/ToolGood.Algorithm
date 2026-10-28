using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.Internals.Functions.Operator
{
	internal sealed class Function_Div : Function_2
	{
		public Function_Div(FunctionBase[] funcs) : base(funcs)
		{
		}

		public Function_Div(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override string Name => "/";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetNumber_1(engine, tempParameter); if(args1.IsErrorOrNone) { return args1; }
			var args2 = GetNumber_2(engine, tempParameter); if(args2.IsErrorOrNone) { return args2; }
			// 整数快路径：x/0、x/1 直接返回，其余整数除法直接转 decimal，省两次 NumberValue 虚调用
			if(args1 is OperandInt n1 && args2 is OperandInt n2) {
				if(n2.RawInt == 0) { return Div0Error(); }
				if(n2.RawInt == 1) { return args1; }
				return Operand.CreateArithmetic((decimal)n1.RawInt / n2.RawInt);
			}

			var number2 = args2.NumberValue;
			if(number2 == 0) { return Div0Error(); }
			if(number2 == 1) { return args1; }

			try {
				return Operand.CreateArithmetic(args1.NumberValue / number2);
			} catch (OverflowException) {
				return NumError();
			}
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			if(addBrackets) stringBuilder.Append('(');
			func1.ToString(stringBuilder, true);
			stringBuilder.Append(" / ");
			func2.ToString(stringBuilder, true);
			if(addBrackets) stringBuilder.Append(')');
		}
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}
}