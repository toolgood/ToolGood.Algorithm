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

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter is error!", "TimeValue"); if (args1.IsError) { return args1; } }

            if (TimeSpan.TryParse(args1.TextValue, CultureInfo.InvariantCulture, out TimeSpan dt)) {
                return Operand.Create(dt);
            }
            return Operand.Error("Function '{0}' parameter is error!", "TimeValue");
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "TimeValue");
        }
    }

}
