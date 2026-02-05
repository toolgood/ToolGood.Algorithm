using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions
{
	internal abstract class Function_3 : Function_2
	{
		protected FunctionBase func3;

		protected Function_3(FunctionBase func1, FunctionBase func2, FunctionBase func3):base(func1, func2)
		{
			this.func3 = func3;
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
		protected Operand GetText_3(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args3 = func3.Evaluate(work, tempParameter);
			if(args3.IsNotText) return ConvertToText(args3, 3);
			return args3;
		}

		protected Operand GetNumber_3(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args3 = func3.Evaluate(work, tempParameter);
			if(args3.IsNotNumber) return ConvertToNumber(args3, 3);
			return args3;
		}

		protected Operand GetDate_3(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args3 = func3.Evaluate(work, tempParameter);
			if(args3.IsNotDate) return ConvertToDate(args3, 3);
			return args3;
		}

		protected Operand GetBoolean_3(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args3 = func3.Evaluate(work, tempParameter);
			if(args3.IsNotBoolean) return ConvertToBoolean(args3, 3);
			return args3;
		}
		protected Operand GetArray_3(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args3 = func3.Evaluate(work, tempParameter);
			if(args3.IsNotArray) return ConvertToArray(args3, 3);
			return args3;
		}
		#endregion

	}


}