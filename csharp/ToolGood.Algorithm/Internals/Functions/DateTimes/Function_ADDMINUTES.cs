using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal sealed class Function_ADDMINUTES : Function_DateAdd
	{
		public Function_ADDMINUTES(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "AddMinutes";

		protected override MyDate Add(MyDate date, int offset)
		{
			return date.AddMinutes(offset);
		}
	}
}
