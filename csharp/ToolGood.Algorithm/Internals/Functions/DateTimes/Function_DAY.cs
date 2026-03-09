using System;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal sealed class Function_DAY : Function_1
    {
        public Function_DAY(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "Day";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetDate_1(engine, tempParameter);
			if (args1.IsErrorOrNone) { return args1; }
            if (args1.DateValue.Day == null) {
                return ParameterError(1);
            }
            return Operand.Create(args1.DateValue.Day.Value);
        }
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}
	}

}
