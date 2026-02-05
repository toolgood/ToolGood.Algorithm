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
		protected Operand GetText_1(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter);
			if(args1.IsNotText) return FunctionUtil.ConvertToText(args1, Name, 1);
			return args1;
		}

		protected Operand GetNumber_1(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter);
			if(args1.IsNotNumber) return FunctionUtil.ConvertToNumber(args1, Name, 1);
			return args1;
		}

		protected Operand GetDate_1(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter);
			if(args1.IsNotDate) return FunctionUtil.ConvertToDate(args1, Name, 1);
			return args1;
		}

		protected Operand GetBoolean_1(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter);
			if(args1.IsNotBoolean) return FunctionUtil.ConvertToBoolean(args1, Name, 1);
			return args1;
		}
		protected Operand GetArray_1(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter);
			if(args1.IsNotArray) return FunctionUtil.ConvertToArray(args1, Name, 1);
			return args1;
		} 
		#endregion



	}


}