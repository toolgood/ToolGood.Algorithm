using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions
{
	internal abstract class Function_2 : Function_1
	{
		protected FunctionBase func2;

		public Function_2(FunctionBase func1, FunctionBase func2) : base(func1)
		{
			this.func2 = func2;
		}

		protected Function_2(FunctionBase[] funcs) : base(funcs)
		{
			if(funcs.Length >= 2) {
				this.func2 = funcs[1];
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
			}
			stringBuilder.Append(')');
		}


		#region Get_2
		protected Operand GetText_2(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args2 = func2.Evaluate(engine, tempParameter);
			if(args2.IsText) return args2;
			return ConvertToText(args2, 2);
		}

		protected Operand GetNumber_2(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args2 = func2.Evaluate(engine, tempParameter);
			if(args2.IsNumber) return args2;
			return ConvertToNumber(args2, 2);
		}

		protected Operand GetDate_2(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args2 = func2.Evaluate(engine, tempParameter);
			if(args2.IsDate) return args2;
			return ConvertToDate(args2, 2);
		}

		protected Operand GetBoolean_2(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args2 = func2.Evaluate(engine, tempParameter);
			if(args2.IsBoolean) return args2;
			return ConvertToBoolean(args2, 2);
		}
		protected Operand GetArray_2(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args2 = func2.Evaluate(engine, tempParameter);
			if(args2.IsArray) return args2;
			return ConvertToArray(args2, 2);
		}
		#endregion

	}


}