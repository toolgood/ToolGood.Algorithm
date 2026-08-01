using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Operator
{
	internal sealed class Function_Mod : Function_2
	{
		public Function_Mod(FunctionBase[] funcs) : base(funcs)
		{
		}

		public Function_Mod(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override string Name => "%";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetNumber_1(engine, tempParameter); if(args1.IsErrorOrNone) { return args1; }
			var args2 = GetNumber_2(engine, tempParameter); if(args2.IsErrorOrNone) { return args2; }
	 
			if(args2.NumberValue == 0m) { return Div0Error(); }

			var number1 = args1.NumberValue;
			var number2 = args2.NumberValue;
			// Excel MOD 语义:结果符号随除数,即 n - d*INT(n/d);C# % 符号随被除数,需修正
			var r = number1 % number2;
			if(r != 0 && (r < 0 != number2 < 0)) {
				r += number2;
			}
			return Operand.Create(r);
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			if(addBrackets) stringBuilder.Append('(');
			func1.ToString(stringBuilder, true);
			stringBuilder.Append(" % ");
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