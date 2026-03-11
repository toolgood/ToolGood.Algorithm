using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal sealed class Function_RANDBETWEEN : Function_2
    {
		public Function_RANDBETWEEN(FunctionBase[] funcs) : base(funcs)
		{
		}

        public override string Name => "RandBetween";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsErrorOrNone || args1.IsNone) { return args1; }

            var args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsErrorOrNone || args2.IsNone) { return args2; }
#if NETSTANDARD2_1
            var tick = DateTime.Now.Ticks;
            Random rand = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
#else
            Random rand = Random.Shared;
#endif
            return Operand.Create((decimal)rand.NextDouble() * (args2.NumberValue - args1.NumberValue) + args1.NumberValue);
        }
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}

}
