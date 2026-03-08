using System;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{

	internal sealed class Function_GUID : FunctionBase
	{
		public Function_GUID()
		{
		}

		public override string Name => "Guid";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			return Operand.Create(System.Guid.NewGuid().ToString());
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			stringBuilder.Append("GUID()");
		}
		public override OperandType GetResultType()
		{
			return OperandType.TEXT;
		}
	}


}
