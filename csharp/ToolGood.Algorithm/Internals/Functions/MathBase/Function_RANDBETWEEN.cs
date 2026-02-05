using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal class Function_RANDBETWEEN : Function_2
    {
		public Function_RANDBETWEEN(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "RandBetween";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(work, tempParameter);
			if (args1.IsError) { return args1; }

			var args2 = GetNumber_2(work, tempParameter);
			if (args2.IsError) { return args2; }
#if NETSTANDARD2_1
            var tick = DateTime.Now.Ticks;
            Random rand = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
#else
            Random rand = Random.Shared;
#endif
            return Operand.Create((decimal)rand.NextDouble() * (args2.NumberValue - args1.NumberValue) + args1.NumberValue);
        }

    }

    

}
