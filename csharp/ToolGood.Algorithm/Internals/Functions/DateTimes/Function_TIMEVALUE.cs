using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal sealed class Function_TIMEVALUE : Function_1
    {
        public Function_TIMEVALUE(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "TimeValue";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetText_1(engine, tempParameter);
			if (args1.IsErrorOrNone) { return args1; }

            if (TimeSpan.TryParse(args1.TextValue, CultureInfo.InvariantCulture, out TimeSpan dt)) {
                return Operand.Create(dt);
            }
            return ParameterError(1);
        }
		public override OperandType GetResultType()
		{
			return OperandType.DATE;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
		}
    }
}
