using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.Operator
{

	internal sealed class Function_AND : Function_2
	{
		public Function_AND(FunctionBase[] funcs) : base(funcs)
		{
		}

		public Function_AND(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override string Name => "And";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetBoolean_1(engine, tempParameter);
			if (args1.IsErrorOrNone) { return args1; }
			if(args1.BooleanValue == false) {
				var args2 = GetBoolean_2(engine, tempParameter);
				if(args2.IsErrorOrNone) { return args2; }
				return Operand.False;
			}
			return GetBoolean_2(engine, tempParameter);
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			if(addBrackets) stringBuilder.Append('(');
			func1.ToString(stringBuilder, false);
			stringBuilder.Append(" && ");
			func2.ToString(stringBuilder, false);
			if(addBrackets) stringBuilder.Append(')');
		}
		public override OperandType GetResultType()
		{
			return OperandType.BOOLEAN;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.BOOLEAN);
			func2.GetParameterTypes(noneEngine, result, OperandType.BOOLEAN);
		}
	}


}