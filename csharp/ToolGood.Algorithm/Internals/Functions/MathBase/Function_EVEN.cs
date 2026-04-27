using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal sealed class Function_EVEN : Function_1
    {
        public Function_EVEN(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length != 1) {
				throw new ArgumentException($"Function '{Name}' requires exactly 1 parameter.");
			}
		}

        public override string Name => "Even";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsErrorOrNone) { return args1; }
            var z = args1.NumberValue;
            
            if (z == 0) { return Operand.Zero; }
            
            if (z > 0) {
                if (z % 2 == 0) { return args1; }
                z = Math.Ceiling(z);
                if (z % 2 == 0) { return Operand.Create(z); }
                z++;
                return Operand.Create(z);
            } else {
                if (z % 2 == 0) { return args1; }
                z = Math.Floor(z);
                if (z % 2 == 0) { return Operand.Create(z); }
                z--;
                return Operand.Create(z);
            }
        }
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

        internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
        {
            func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        }
	}

}
