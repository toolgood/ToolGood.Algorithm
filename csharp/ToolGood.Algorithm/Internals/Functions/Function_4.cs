using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions
{
	internal abstract class Function_4 : Function_3
	{
		protected FunctionBase func4;

		protected Function_4(FunctionBase[] funcs) : base(funcs)
		{
			if(funcs.Length >= 4) {
				this.func4 = funcs[3];
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
					}
				}
			}
			stringBuilder.Append(')');
		}

		#region Get_4
		protected Operand GetText_4(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args4 = func4.Evaluate(engine, tempParameter);
			if(args4.IsText) return args4;
			return ConvertToText(args4, 4);
		}

		protected Operand GetNumber_4(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args4 = func4.Evaluate(engine, tempParameter);
			if(args4.IsNumber) return args4;
			return ConvertToNumber(args4, 4);
		}

		protected Operand GetDate_4(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args4 = func4.Evaluate(engine, tempParameter);
			if(args4.IsDate) return args4;
			return ConvertToDate(args4, 4);
		}

		protected Operand GetBoolean_4(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args4 = func4.Evaluate(engine, tempParameter);
			if(args4.IsBoolean) return args4;
			return ConvertToBoolean(args4, 4);
		}
		protected Operand GetArray_4(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args4 = func4.Evaluate(engine, tempParameter);
			if(args4.IsArray) return args4;
			return ConvertToArray(args4, 4);
		}
		#endregion
	}


}