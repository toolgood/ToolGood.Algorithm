using System;
using System.Collections.Generic;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions
{
	internal class NoneEngine : AlgorithmEngine
	{
		public NoneEngine(AlgorithmEngine engine)
		{
			this.AreaUnit = engine.AreaUnit;
			this.DistanceUnit = engine.DistanceUnit;
			this.LastError = engine.LastError;
			this.MassUnit = engine.MassUnit;
			this.UseLocalTime = engine.UseLocalTime;
			this.ExcelIndex = engine.ExcelIndex;
		}

		public override Operand GetParameter(string parameter)
		{
			return Operand.None;
		}
		public override Operand ExecuteDiyFunction(string parameter, List<Operand> args)
		{
			return Operand.None;
		}
	}
}
