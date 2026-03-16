using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.Flow
{
	internal sealed class Function_ISNULL : Function_2
    {
		public Function_ISNULL(FunctionBase[] funcs) : base(funcs)
		{
		}

        public override string Name => "IsNull";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(engine, tempParameter);
            if (func2 != null) {
                if (args1.IsNull) { return func2.Evaluate(engine, tempParameter); }
                if (args1.IsText && args1.TextValue == null) { return func2.Evaluate(engine, tempParameter); }
                return args1;
            }
            if (args1.IsNull) { return Operand.True; }
            if (args1.IsText && args1.TextValue == null) { return Operand.True; }
            return Operand.False;
        }
		public override OperandType GetResultType()
		{
			return OperandType.BOOLEAN;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NONE);
			if(func2 != null) {
				func2.GetParameterTypes(noneEngine, result, OperandType.NONE);
			}
		}
	}
}
