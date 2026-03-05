using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal sealed class Function_MONTH : Function_1
    {
        public Function_MONTH(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "Month";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetDate_1(engine, tempParameter);
			if (args1.IsError) { return args1; }
            if (args1.DateValue.Month == null) {
                return FunctionError();
            }
            return Operand.Create((int)args1.DateValue.Month.Value);
        }

    }

}
