using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal class Function_DAY : Function_1
    {
        public Function_DAY(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "Day";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetDate_1(work, tempParameter);
			if (args1.IsError) { return args1; }
            if (args1.DateValue.Day == null) {
                return FunctionError();
            }
            return Operand.Create(args1.DateValue.Day.Value);
        }

    }

}
