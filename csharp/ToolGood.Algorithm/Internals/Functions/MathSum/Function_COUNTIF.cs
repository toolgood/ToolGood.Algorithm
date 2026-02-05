using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_COUNTIF : Function_2
    {
		public Function_COUNTIF(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "CountIf";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetArray_1(work, tempParameter);
            if (args1.IsError) { return args1; }

            var args2 = func2.Evaluate(work, tempParameter);
            if (args2.IsError) { return args2; }

            var list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args1, list);
            if (o == false) { return ParameterError(1); }
            int count;
            if (args2.IsNumber) {
                count = FunctionUtil.F_base_countif(list, args2.NumberValue);
            } else {
                if (decimal.TryParse(args2.TextValue.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
                    count = FunctionUtil.F_base_countif(list, d);
                } else {
                    var sunif = args2.TextValue.Trim();
                    var m2 = FunctionUtil.sumifMatch(sunif);
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
