using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions
{
	internal abstract class Function_6 : Function_5
	{
		protected FunctionBase func6;

		protected Function_6(FunctionBase[] funcs) : base(funcs)
		{
			if(funcs.Length >= 6) {
				this.func6 = funcs[5];
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
							if(func6 != null) {
								stringBuilder.Append(", ");
								func6.ToString(stringBuilder, false);
							}
						}
					}
				}
			}
			stringBuilder.Append(')');
		}

		#region Get_6
		protected Operand GetText_6(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args6 = func6.Evaluate(engine, tempParameter);
			if(args6.IsText) return args6;
			return ConvertToText(args6, 6);
		}

		protected Operand GetNumber_6(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args6 = func6.Evaluate(engine, tempParameter);
			if(args6.IsNumber) return args6;
			return ConvertToNumber(args6, 6);
		}

		protected Operand GetDate_6(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args6 = func6.Evaluate(engine, tempParameter);
			if(args6.IsDate) return args6;
			return ConvertToDate(args6, 6);
		}

		protected Operand GetBoolean_6(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args6 = func6.Evaluate(engine, tempParameter);
			if(args6.IsBoolean) return args6;
			return ConvertToBoolean(args6, 6);
		}
		protected Operand GetArray_6(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args6 = func6.Evaluate(engine, tempParameter);
			if(args6.IsArray) return args6;
			return ConvertToArray(args6, 6);
		}
		#endregion
	}
}
