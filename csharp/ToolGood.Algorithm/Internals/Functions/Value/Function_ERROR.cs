using System;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Value
{
	internal sealed class Function_ERROR : Function_1
	{
		public Function_ERROR(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "Error";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			if(func1 == null) { return Operand.Error(""); }
			var args1 = GetText_1(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }
			return Operand.Error(args1.TextValue);
		}
		public override OperandType GetResultType()
		{
			return OperandType.ERROR;
		}

	}

}
