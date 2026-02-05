using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions
{
	internal abstract class Function_2 : Function_1
	{
		protected FunctionBase func2;

		public Function_2(FunctionBase func1, FunctionBase func2):base(func1)
		{
			this.func2 = func2;
		}
		protected override void AddFunction(StringBuilder stringBuilder, string functionName)
		{
			stringBuilder.Append(functionName);
			stringBuilder.Append('(');
			func2.ToString(stringBuilder, false);
			if(func2 != null) {
				stringBuilder.Append(", ");
				func2.ToString(stringBuilder, false);
			}
			stringBuilder.Append(')');
		}


		#region Get_2
		protected Operand GetText_2(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args2 = func2.Evaluate(work, tempParameter);
			if(args2.IsNotText) return ConvertToText(args2, 2);
			return args2;
		}

		protected Operand GetNumber_2(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args2 = func2.Evaluate(work, tempParameter);
			if(args2.IsNotNumber) return ConvertToNumber(args2, 2);
			return args2;
		}

		protected Operand GetDate_2(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args2 = func2.Evaluate(work, tempParameter);
			if(args2.IsNotDate) return ConvertToDate(args2, 2);
			return args2;
		}

		protected Operand GetBoolean_2(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args2 = func2.Evaluate(work, tempParameter);
			if(args2.IsNotBoolean) return ConvertToBoolean(args2, 2);
			return args2;
		}
		protected Operand GetArray_2(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args2 = func2.Evaluate(work, tempParameter);
			if(args2.IsNotArray) return ConvertToArray(args2, 2);
			return args2;
		}
		#endregion

	}


}