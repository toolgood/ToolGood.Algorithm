using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal sealed class Function_TEXT : Function_2
	{
		public Function_TEXT(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "Text";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(engine, tempParameter); if(args1.IsErrorOrNone) { return args1; }
			var args2 = GetText_2(engine, tempParameter);
			if(args2.IsErrorOrNone) { return args2; }

			if(args1.IsText) {
				return args1;
			} else if(args1.IsBoolean) {
				return Operand.Create(args1.BooleanValue ? "TRUE" : "FALSE");
			} else if(args1.IsNumber) {
				return Operand.Create(args1.NumberValue.ToString(args2.TextValue, CultureInfo.InvariantCulture));
			} else if(args1.IsDate) {
				return Operand.Create(args1.DateValue.ToString(args2.TextValue));
			}
			args1 = ConvertToText(args1, 1);
			if(args1.IsErrorOrNone) { return args1; }
			return Operand.Create(args1.TextValue);
		}
		public override OperandType GetResultType()
		{
			return OperandType.TEXT;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NONE);
			func2.GetParameterTypes(noneEngine, result, OperandType.TEXT);
		}

	}

}
