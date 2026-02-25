using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal class Function_HOUR : Function_1
    {
        public Function_HOUR(FunctionBase[] func1) : base(func1)
        {
        }

        public override string Name => "Hour";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetDate_1(work, tempParameter);
			if (args1.IsError) { return args1; }
            return Operand.Create(args1.DateValue.Hour);
        }

    }

}
