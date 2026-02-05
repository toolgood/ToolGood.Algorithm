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
            var args1 = func1.Evaluate(work, tempParameter);
			args1 = ConvertToDate(args1, "Day", 1);
			if (args1.IsError) { return args1; }
            if (args1.DateValue.Day == null) {
                return Operand.Error("Function '{0}' is error!", "Day");
            }
            return Operand.Create(args1.DateValue.Day.Value);
        }

    }

}
