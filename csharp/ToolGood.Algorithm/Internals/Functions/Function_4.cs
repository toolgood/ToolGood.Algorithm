using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions
{
	internal abstract class Function_4 : Function_3
	{
		protected FunctionBase func4;

		protected Function_4(FunctionBase func1, FunctionBase func2, FunctionBase func3, FunctionBase func4):base(func1, func2, func3)
		{
			this.func4 = func4;
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
					}
				}
			}
			stringBuilder.Append(')');
		}


		#region Get_4
		protected Operand GetText_4(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args4 = func4.Evaluate(work, tempParameter);
			if(args4.IsNotText) return ConvertToText(args4, Name, 4);
			return args4;
		}

		protected Operand GetNumber_4(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args4 = func4.Evaluate(work, tempParameter);
			if(args4.IsNotNumber) return ConvertToNumber(args4, Name, 4);
			return args4;
		}

		protected Operand GetDate_4(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args4 = func4.Evaluate(work, tempParameter);
			if(args4.IsNotDate) return ConvertToDate(args4, Name, 4);
			return args4;
		}

		protected Operand GetBoolean_4(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args4 = func4.Evaluate(work, tempParameter);
			if(args4.IsNotBoolean) return ConvertToBoolean(args4, Name, 4);
			return args4;
		}
		protected Operand GetArray_4(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args4 = func4.Evaluate(work, tempParameter);
			if(args4.IsNotArray) return ConvertToArray(args4, Name, 4);
			return args4;
		}
		#endregion
	}


}