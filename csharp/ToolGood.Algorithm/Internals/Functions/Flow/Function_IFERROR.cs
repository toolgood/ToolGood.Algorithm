using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.Flow
{
	internal sealed class Function_IFERROR : Function_3
    {
		public Function_IFERROR(FunctionBase[] funcs) : base(funcs)
		{
		}

        public override string Name => "IfError";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(engine, tempParameter);
			if(args1.IsNone) return args1;
            if (args1.IsError) { return func2.Evaluate(engine, tempParameter); }
			if(func3 == null) return args1;
            return func3.Evaluate(engine, tempParameter);
        }
		public override OperandType GetResultType()
		{
			var t2 = func2.GetResultType();
			return t2 != OperandType.NONE ? t2 : func3?.GetResultType() ?? OperandType.NONE;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NONE);
			func2.GetParameterTypes(noneEngine, result, OperandType.NONE);
			if(func3 != null) {
				func3.GetParameterTypes(noneEngine, result, OperandType.NONE);
			}
		}
	}
}
