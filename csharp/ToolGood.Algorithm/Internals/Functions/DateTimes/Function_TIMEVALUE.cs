using System;
using System.Globalization;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal class Function_TIMEVALUE : Function_1
    {
        public Function_TIMEVALUE(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "TimeValue";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetText_1(engine, tempParameter);
			if (args1.IsError) { return args1; }

            if (TimeSpan.TryParse(args1.TextValue, CultureInfo.InvariantCulture, out TimeSpan dt)) {
                return Operand.Create(dt);
            }
            return FunctionError();
        }

    }

}
