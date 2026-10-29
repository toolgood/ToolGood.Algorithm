using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal sealed class Function_ADDHOURS : Function_DateAdd
	{
		public Function_ADDHOURS(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "AddHours";

		protected override MyDate Add(MyDate date, int offset)
		{
			return date.AddHours(offset);
		}
	}
}
