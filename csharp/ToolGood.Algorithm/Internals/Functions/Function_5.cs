using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions
{
	internal abstract class Function_5 : Function_4
	{
		protected FunctionBase func5;

		protected Function_5(FunctionBase[] funcs) : base(funcs)
		{
			if(funcs.Length >= 5) {
				this.func5 = funcs[4];
			}
		}

		protected override void AddFunction(StringBuilder stringBuilder, string functionName)
		{
			stringBuilder.Append(functionName);
			stringBuilder.Append('(');
			func1.ToString(stringBuilder, false);
			if(func2 != null) {
				stringBuilder.Append(", ");
				func2.ToString(stringBuilder, false);
				if(func3 != null) {
					stringBuilder.Append(", ");
					func3.ToString(stringBuilder, false);
					if(func4 != null) {
						stringBuilder.Append(", ");
						func4.ToString(stringBuilder, false);
						if(func5 != null) {
							stringBuilder.Append(", ");
							func5.ToString(stringBuilder, false);
						}
					}
				}
			}
			stringBuilder.Append(')');
		}

		#region Get_5
		protected Operand GetText_5(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args5 = func5.Evaluate(engine, tempParameter);
			if(args5.IsText) return args5;
			return ConvertToText(args5, 5);
		}

		protected Operand GetNumber_5(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args5 = func5.Evaluate(engine, tempParameter);
			if(args5.IsNumber) return args5;
			return ConvertToNumber(args5, 5);
		}

		protected Operand GetDate_5(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args5 = func5.Evaluate(engine, tempParameter);
			if(args5.IsDate) return args5;
			return ConvertToDate(args5, 5);
		}

		protected Operand GetBoolean_5(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args5 = func5.Evaluate(engine, tempParameter);
			if(args5.IsBoolean) return args5;
			return ConvertToBoolean(args5, 5);
		}
		protected Operand GetArray_5(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args5 = func5.Evaluate(engine, tempParameter);
			if(args5.IsArray) return args5;
			return ConvertToArray(args5, 5);
		}
		#endregion
	}
}
