using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Compare
{
	internal sealed class Function_GT : Function_2
	{
		public Function_GT(FunctionBase[] funcs) : base(funcs)
		{
		}
		public Function_GT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override string Name => ">";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(engine, tempParameter); if(args1.IsErrorOrNone) { return args1; }
			var args2 = func2.Evaluate(engine, tempParameter); if(args2.IsErrorOrNone) { return args2; }

			if(args1.Type == args2.Type) {
				if(args1.IsNumber) {
					return Operand.Create(args1.NumberValue > args2.NumberValue);
				} else if(args1.IsText) {
					var r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
					return r > 0 ? Operand.True : Operand.False;
				} else if(args1.IsDate) {
					return Operand.Create(args1.DateValue.ToLong() > args2.DateValue.ToLong());
				} else if(args1.IsBoolean) {
					args1 = args1.ToNumber();
					args2 = args2.ToNumber();
					return Operand.Create(args1.NumberValue > args2.NumberValue);
				} else if(args1.IsNull) {
					return Operand.True;
				}
				return CompareError();
			} else if(args1.IsNull || args2.IsNull) {
				return Operand.False;
			} else if(args1.IsDate || args2.IsDate || args1.IsJson || args2.IsJson || args1.IsArray || args2.IsArray || args1.IsArrayJson || args2.IsArrayJson) {
				return CompareError();
			}
			args1 = ConvertToNumber(args1, 1);
			if(args1.IsErrorOrNone) { return args1; }
			args2 = ConvertToNumber(args2, 2);
			if(args2.IsErrorOrNone) { return args2; }

			return Operand.Create(args1.NumberValue > args2.NumberValue);
		}

		public override OperandType GetResultType()
		{
			return OperandType.BOOLEAN;
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			if(addBrackets) stringBuilder.Append('(');
			func1.ToString(stringBuilder, false);
			stringBuilder.Append(" > ");
			func2.ToString(stringBuilder, false);
			if(addBrackets) stringBuilder.Append(')');
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			var t1 = func1.GetResultType();
			var t2 = func2.GetResultType();
			if(t1 == OperandType.NONE) {
				var p = noneEngine.Evaluate(func2).ToText();
				if(t2 != OperandType.ERROR && p.IsErrorOrNone == false) {
					func1.GetParameterTypes(noneEngine, result, t2, Name, p.TextValue);
					func2.GetParameterTypes(noneEngine, result, t2);
					return;
				}
			} else if(t2 == OperandType.NONE) {
				var p = noneEngine.Evaluate(func1).ToText();
				if(t1 != OperandType.ERROR && p.IsErrorOrNone == false) {
					func2.GetParameterTypes(noneEngine, result, t1, Name, p.TextValue);
					func1.GetParameterTypes(noneEngine, result, t1);
					return;
				}
			}
			func1.GetParameterTypes(noneEngine, result, OperandType.NONE);
			func2.GetParameterTypes(noneEngine, result, OperandType.NONE);
		}
	}

}