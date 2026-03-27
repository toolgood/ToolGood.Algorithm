using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal sealed class Function_ODD : Function_1
    {
        public Function_ODD(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "Odd";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsErrorOrNone) { return args1; }
            var z = args1.NumberValue;
            
            if (z == 0) { return Operand.Create(1); }
            
            if (z > 0) {
                if (Math.Floor(z) == z && Math.Abs(z) % 2 == 1) { return args1; }
                z = Math.Ceiling(z);
                if (Math.Abs(z) % 2 == 1) { return Operand.Create(z); }
                z++;
                return Operand.Create(z);
            } else {
                if (Math.Floor(z) == z && Math.Abs(z) % 2 == 1) { return args1; }
                z = Math.Floor(z);
                if (Math.Abs(z) % 2 == 1) { return Operand.Create(z); }
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
