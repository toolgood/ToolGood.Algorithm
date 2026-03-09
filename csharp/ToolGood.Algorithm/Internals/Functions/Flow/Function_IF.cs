using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.Flow
{
	internal sealed class Function_IF : Function_3
	{
		public Function_IF(FunctionBase[] funcs) : base(funcs)
		{
		}



		public override string Name => "If";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetBoolean_1(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }
			if(args1.BooleanValue) return func2.Evaluate(engine, tempParameter);
			if(func3 == null) { return Operand.False; }
			return func3.Evaluate(engine, tempParameter);
		}

		public override OperandType GetResultType()
		{
			var t2 = func2.GetResultType();
			if(t2 != OperandType.NONE) return t2;
			if(func3 == null) {
				var t3 = func2.GetResultType();
				if(t3 != OperandType.NONE) return t3;
			}
			return OperandType.NONE;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.BOOLEAN);
			func2.GetParameterTypes(noneEngine, result, OperandType.NONE);
			if(func3 != null) {
				func3.GetParameterTypes(noneEngine, result, OperandType.NONE);
			}
		}

	}
}
