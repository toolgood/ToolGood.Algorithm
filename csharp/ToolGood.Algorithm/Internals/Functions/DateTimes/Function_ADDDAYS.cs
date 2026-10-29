using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal sealed class Function_ADDDAYS : Function_DateAdd
	{
		public Function_ADDDAYS(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "AddDays";

		protected override MyDate Add(MyDate date, int offset)
		{
			return date.AddDays(offset);
		}
	}
}
