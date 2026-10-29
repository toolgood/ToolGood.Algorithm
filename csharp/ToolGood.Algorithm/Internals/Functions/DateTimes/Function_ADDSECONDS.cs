using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal sealed class Function_ADDSECONDS : Function_DateAdd
	{
		public Function_ADDSECONDS(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "AddSeconds";

		protected override MyDate Add(MyDate date, int offset)
		{
			return date.AddSeconds(offset);
		}
	}
}
