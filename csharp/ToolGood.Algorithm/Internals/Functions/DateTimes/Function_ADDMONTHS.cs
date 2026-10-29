using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal sealed class Function_ADDMONTHS : Function_DateAdd
	{
		public Function_ADDMONTHS(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "AddMonths";

		protected override MyDate Add(MyDate date, int offset)
		{
			return date.AddMonths(offset);
		}
	}
}
