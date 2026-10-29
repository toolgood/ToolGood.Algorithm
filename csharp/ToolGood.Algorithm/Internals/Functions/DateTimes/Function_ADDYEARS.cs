using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal sealed class Function_ADDYEARS : Function_DateAdd
	{
		public Function_ADDYEARS(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "AddYears";

		protected override MyDate Add(MyDate date, int offset)
		{
			return date.AddYears(offset);
		}
	}
}
