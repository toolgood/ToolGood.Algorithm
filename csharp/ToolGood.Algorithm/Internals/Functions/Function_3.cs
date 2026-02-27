using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions
{
	internal abstract class Function_3 : Function_2
	{
		protected FunctionBase func3;

		protected Function_3(FunctionBase[] funcs) : base(funcs)
		{
			if(funcs.Length >= 3) {
				this.func3 = funcs[2];
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
				}
			}
			stringBuilder.Append(')');
		}


		#region Get_3
		protected Operand GetText_3(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args3 = func3.Evaluate(engine, tempParameter);
			if(args3.IsText) return args3;
			return ConvertToText(args3, 3);
		}

		protected Operand GetNumber_3(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args3 = func3.Evaluate(engine, tempParameter);
			if(args3.IsNumber) return args3;
			return ConvertToNumber(args3, 3);
		}

		protected Operand GetDate_3(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args3 = func3.Evaluate(engine, tempParameter);
			if(args3.IsDate) return args3;
			return ConvertToDate(args3, 3);
		}

		protected Operand GetBoolean_3(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args3 = func3.Evaluate(engine, tempParameter);
			if(args3.IsBoolean) return args3;
			return ConvertToBoolean(args3, 3);
		}
		protected Operand GetArray_3(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args3 = func3.Evaluate(engine, tempParameter);
			if(args3.IsArray) return args3;
			return ConvertToArray(args3, 3);
		}
		#endregion

	}


}