using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.Value
{
	internal sealed class Function_Number : FunctionBase
	{
		private readonly decimal d;
		private readonly string unit;

		public Function_Number(decimal func1, string func2)
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

		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
		}
	}

}
