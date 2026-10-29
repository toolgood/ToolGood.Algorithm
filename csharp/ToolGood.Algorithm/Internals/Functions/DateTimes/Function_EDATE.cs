using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	/// <summary>
	/// EDATE(start_date, months)：返回与 start_date 相隔指定月数的日期。
	/// 求值与 ADDMONTHS 完全相同，差异仅在函数名。
	/// </summary>
	internal sealed class Function_EDATE : Function_DateAdd
	{
		public Function_EDATE(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "EDate";

		protected override MyDate Add(MyDate date, int offset)
		{
			return date.AddMonths(offset);
		}
	}
}
