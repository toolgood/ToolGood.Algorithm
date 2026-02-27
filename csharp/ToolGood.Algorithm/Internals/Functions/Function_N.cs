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
		protected Operand GetText(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter, int idx)
		{
			var args1 = funcs[idx].Evaluate(engine, tempParameter);
			if(args1.IsText) return args1;
			return ConvertToText(args1, idx);
		}

		protected Operand GetNumber(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter, int idx)
		{
			var args1 = funcs[idx].Evaluate(engine, tempParameter);
			if(args1.IsNumber) return args1;
			return ConvertToNumber(args1, idx);
		}

		protected Operand GetDate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter, int idx)
		{
			var args1 = funcs[idx].Evaluate(engine, tempParameter);
			if(args1.IsDate) return args1;
			return ConvertToDate(args1, idx);
		}

		protected Operand GetBoolean(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter, int idx)
		{
			var args1 = funcs[idx].Evaluate(engine, tempParameter);
			if(args1.IsBoolean) return args1;
			return ConvertToBoolean(args1, idx);
		}
		protected Operand GetArray(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter, int idx)
		{
			var args1 = funcs[idx].Evaluate(engine, tempParameter);
			if(args1.IsArray) return args1;
			return ConvertToArray(args1, idx);
		}
		#endregion



	}


}