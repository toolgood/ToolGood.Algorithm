using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal sealed class Function_COUNTIF : Function_2
    {
		public Function_COUNTIF(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "CountIf";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetArray_1(engine, tempParameter);
            if (args1.IsError) { return args1; }

            var args2 = func2.Evaluate(engine, tempParameter);
            if (args2.IsError) { return args2; }

            var list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args1, list);
            if (o == false) { return ParameterError(1); }
            int count;
            if (args2.IsNumber) {
                count = FunctionUtil.F_base_countif(list, args2.NumberValue);
            } else {
                var span = args2.TextValue.AsSpan().Trim();
                if (decimal.TryParse(span, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
                    count = FunctionUtil.F_base_countif(list, d);
                } else {
                    var m2 = FunctionUtil.sumifMatch(args2.TextValue.Trim());
                    if (m2 != null) {
                        count = FunctionUtil.F_base_countif(list, m2.Item1, m2.Item2);
                    } else {
                        return ParameterError(2);
                    }
                }
            }
            return Operand.Create(count);
        }

    }

}
