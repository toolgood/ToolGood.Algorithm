using System;
using System.Collections.Generic;
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

		#region TryEvaluateAll
		protected Operand TryEvaluateAll(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter, List<Operand> args)
		{
			for(int i = 0; i < funcs.Length; i++) {
				var aa = funcs[i].Evaluate(engine, tempParameter);
				if(aa.IsErrorOrNone) { return aa; }
				args.Add(aa);
			}
			return null;
		}
		#endregion

		#region Get
		// 注：idx 为 funcs 的数组下标(0-based)，向上抛错误消息时统一转为 1-based 参数序号，
		// 与 Function_1/Function_2/Function_3 的编号方式保持一致。
		protected Operand GetText(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter, int idx)
		{
			var args1 = funcs[idx].Evaluate(engine, tempParameter);
			if(args1.IsText) return args1;
			return ConvertToText(args1, idx + 1);
		}

		protected Operand GetNumber(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter, int idx)
		{
			var args1 = funcs[idx].Evaluate(engine, tempParameter);
			if(args1.IsNumber) return args1;
			return ConvertToNumber(args1, idx + 1);
		}

		protected Operand GetDate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter, int idx)
		{
			var args1 = funcs[idx].Evaluate(engine, tempParameter);
			if(args1.IsDate) return args1;
			return ConvertToDate(args1, idx + 1);
		}

		protected Operand GetBoolean(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter, int idx)
		{
			var args1 = funcs[idx].Evaluate(engine, tempParameter);
			if(args1.IsBoolean) return args1;
			return ConvertToBoolean(args1, idx + 1);
		}
		protected Operand GetArray(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter, int idx)
		{
			var args1 = funcs[idx].Evaluate(engine, tempParameter);
			if(args1.IsArray) return args1;
			return ConvertToArray(args1, idx + 1);
		}
		#endregion

	}

}