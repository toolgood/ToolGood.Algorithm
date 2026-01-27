using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Value
{
	internal class Function_NUM : FunctionBase
	{
		private readonly decimal d;
		private readonly string unit;

		public Function_NUM(decimal func1, string func2)
		{
			d = func1;
			unit = func2;
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var dict = NumberUnitTypeHelper.GetUnitTypedict();
			var d2 = NumberUnitTypeHelper.TransformationUnit(d, dict[unit], work.DistanceUnit, work.AreaUnit, work.VolumeUnit, work.MassUnit);
			return Operand.Create(d2);
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			stringBuilder.Append(d);
			stringBuilder.Append(unit);
		}
	}

}
