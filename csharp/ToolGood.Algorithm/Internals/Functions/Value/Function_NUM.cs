using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Value
{
	internal sealed class Function_NUM : FunctionBase
	{
		private readonly decimal d;
		private readonly string unit;

		public Function_NUM(decimal func1, string func2)
		{
			d = func1;
			unit = func2;
		}

		public override string Name => "Num";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var dict = NumberUnitTypeHelper.GetUnitTypedict();
			var d2 = NumberUnitTypeHelper.TransformationUnit(d, dict[unit], engine.DistanceUnit, engine.AreaUnit, engine.VolumeUnit, engine.MassUnit);
			return Operand.Create(d2);
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			stringBuilder.Append(d);
			stringBuilder.Append(unit);
		}
	}

}
