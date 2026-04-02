using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Flow
{
	internal sealed class Function_ISNULLORERROR : Function_2
    {
		public Function_ISNULLORERROR(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length < 1 || funcs.Length > 2) {
				throw new ArgumentException($"Function '{Name}' requires 1 to 2 parameters.");
			}
		}

        public override string Name => "IsNullOrError";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(engine, tempParameter);
            if (func2 != null) {
                if (args1.IsNull || args1.IsErrorOrNone) { return func2.Evaluate(engine, tempParameter); }
                if (args1.IsText && args1.TextValue == null) { return func2.Evaluate(engine, tempParameter); }
                return args1;
            }
            if (args1.IsNull || args1.IsErrorOrNone) { return Operand.True; }
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
