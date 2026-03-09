using System;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Flow
{
	internal sealed class Function_IFS : Function_N
	{
		public Function_IFS(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "Ifs";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			for(int i = 0; i < funcs.Length - 1; i += 2) {
				var condition = GetBoolean(engine, tempParameter, i);
				if(condition.IsErrorOrNone) { return condition; }
				if(condition.BooleanValue) {
					return funcs[i + 1].Evaluate(engine, tempParameter);
				}
			}
			return FunctionError();
		}
		public override OperandType GetResultType()
		{
			for(int i = 0; i < funcs.Length - 1; i += 2) {
				var t = funcs[i + 1].GetResultType();
				if(t != OperandType.NONE) {
					return t;
				}
			}
			return OperandType.NONE;
		}
	}
}
