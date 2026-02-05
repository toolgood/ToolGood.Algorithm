using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions
{
	internal abstract class Function_N : FunctionBase
	{
		protected FunctionBase[] funcs;

		protected Function_N(FunctionBase[] funcs)
		{
			this.funcs = funcs;
		}

		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, Name);
		}
		protected void AddFunction(StringBuilder stringBuilder, string functionName)
		{
			stringBuilder.Append(functionName);
			stringBuilder.Append('(');
			for(int i = 0; i < funcs.Length; i++) {
				if(i > 0) {
					stringBuilder.Append(", ");
				}
				funcs[i].ToString(stringBuilder, false);
			}
			stringBuilder.Append(')');
		}

		#region Get
		protected Operand GetText(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter, int idx)
		{
			var args1 = funcs[idx].Evaluate(work, tempParameter);
			if(args1.IsNotText) return ConvertToText(args1, Name, idx);
			return args1;
		}

		protected Operand GetNumber(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter, int idx)
		{
			var args1 = funcs[idx].Evaluate(work, tempParameter);
			if(args1.IsNotNumber) return ConvertToNumber(args1, Name, idx);
			return args1;
		}

		protected Operand GetDate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter, int idx)
		{
			var args1 = funcs[idx].Evaluate(work, tempParameter);
			if(args1.IsNotDate) return ConvertToDate(args1, Name, idx);
			return args1;
		}

		protected Operand GetBoolean(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter, int idx)
		{
			var args1 = funcs[idx].Evaluate(work, tempParameter);
			if(args1.IsNotBoolean) return ConvertToBoolean(args1, Name, idx);
			return args1;
		}
		protected Operand GetArray(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter, int idx)
		{
			var args1 = funcs[idx].Evaluate(work, tempParameter);
			if(args1.IsNotArray) return ConvertToArray(args1, Name, idx);
			return args1;
		}
		#endregion



	}


}