using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal sealed class Function_EVEN : Function_1
    {
        public Function_EVEN(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "Even";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsErrorOrNone || args1.IsNone) { return args1; }
            var z = args1.NumberValue;
            
            if (z == 0) { return Operand.Zero; }
            
            if (z > 0) {
                if (Math.Floor(z) == z && Math.Abs(z) % 2 == 0) { return args1; }
                z = Math.Ceiling(z);
                if (Math.Abs(z) % 2 == 0) { return Operand.Create(z); }
                z++;
                return Operand.Create(z);
            } else {
                if (Math.Floor(z) == z && Math.Abs(z) % 2 == 0) { return args1; }
                z = Math.Floor(z);
                if (Math.Abs(z) % 2 == 0) { return Operand.Create(z); }
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
