using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Flow
{
	internal sealed class Function_SWITCH : Function_N
	{
		public Function_SWITCH(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "Switch";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var exprValue = funcs[0].Evaluate(engine, tempParameter);
			if(exprValue.IsError) { return exprValue; }

			int i = 1;
			while(i < funcs.Length - 1) {
				var compareValue = funcs[i].Evaluate(engine, tempParameter);
				if(compareValue.IsError) { return compareValue; }

				if(EqualsOperand(exprValue, compareValue)) {
					return funcs[i + 1].Evaluate(engine, tempParameter);
				}
				i += 2;
			}
			return FunctionError();
		}

		private bool EqualsOperand(Operand a, Operand b)
		{
			if(a.IsNumber && b.IsNumber) {
				return a.NumberValue == b.NumberValue;
			}
			if(a.IsText && b.IsText) {
				return a.TextValue == b.TextValue;
			}
			if(a.IsBoolean && b.IsBoolean) {
				return a.BooleanValue == b.BooleanValue;
			}
			if(a.IsNull && b.IsNull) {
				return true;
			}
			return false;
		}
	}
}
