using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal class Function_YEAR : Function_1
    {
        public Function_YEAR(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "Year";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
			args1 = FunctionUtil.ConvertToDate(args1, "Year", 1);
			if (args1.IsError) { return args1; }
            if (args1.DateValue.Year == null) {
                return Operand.Error("Function '{0}' is error!", "Year");
            }
            return Operand.Create(args1.DateValue.Year.Value);
        }

    }

}
