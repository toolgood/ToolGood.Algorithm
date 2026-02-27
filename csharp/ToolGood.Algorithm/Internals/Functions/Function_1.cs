using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions
{
	internal abstract class Function_1 : FunctionBase
	{
		protected FunctionBase func1;

		protected Function_1(FunctionBase func1)
		{
			this.func1 = func1;
		}
		protected Function_1(FunctionBase[] funcs)
		{
			this.func1 = funcs[0];
		}


		#region ToString
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, Name);
		}
		protected virtual void AddFunction(StringBuilder stringBuilder, string functionName)
		{
			stringBuilder.Append(functionName);
			stringBuilder.Append('(');
			func1.ToString(stringBuilder, false);
			stringBuilder.Append(')');
		}
		#endregion

		#region Get_1
		protected Operand GetText_1(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(engine, tempParameter);
			if(args1.IsText) return args1;  
			return ConvertToText(args1, 1);
		}

		protected Operand GetNumber_1(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(engine, tempParameter);
			if(args1.IsNumber) return args1;  
			return ConvertToNumber(args1, 1);
		}

		protected Operand GetDate_1(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(engine, tempParameter);
			if(args1.IsDate) return args1;   
			return ConvertToDate(args1, 1);
		}

		protected Operand GetBoolean_1(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(engine, tempParameter);
			if(args1.IsBoolean) return args1;  
			return ConvertToBoolean(args1, 1);
		}
		protected Operand GetArray_1(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(engine, tempParameter);
			if(args1.IsArray) return args1;  
			return ConvertToArray(args1, 1);
		}
		#endregion



	}


}