using System;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal sealed class Function_RAND : Function_0
    {
#if NETSTANDARD2_1
        private static readonly Random _random = new Random();
        private static readonly object _randomLock = new object();
#endif

        public Function_RAND()
        {
        }

        public override string Name => "Rand";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
#if NETSTANDARD2_1
            lock(_randomLock) {
                return Operand.Create(_random.NextDouble());
            }
#else
            return Operand.Create(Random.Shared.NextDouble());
#endif
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("Rand()");
        }
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

	}

}
