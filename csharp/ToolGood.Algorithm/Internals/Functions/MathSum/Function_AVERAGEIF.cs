using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal sealed class Function_AVERAGEIF : Function_3
    {
		public Function_AVERAGEIF(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "AverageIf";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetArray_1(engine, tempParameter); if (args1.IsError) { return args1; }
            var args2 = func2.Evaluate(engine, tempParameter); if (args2.IsError) { return args2; }

            var list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args1, list);
            if (o == false) { return ParameterError(1); }

            List<decimal> sumdbs;
            if (func3 != null) {
                var args3 = GetArray_3(engine, tempParameter); if (args3.IsError) { return args3; }
                sumdbs = new List<decimal>();
                var o2 = FunctionUtil.F_base_GetList(args3, sumdbs);
                if (o2 == false) { return ParameterError(3); }
            } else {
                sumdbs = list;
            }

            decimal sum;
            int count;
            if (args2.IsNumber) {
                count = FunctionUtil.F_base_countif(list, args2.NumberValue);
                sum = count * args2.NumberValue;
            } else {
                var span = args2.TextValue.AsSpan().Trim();
                if (decimal.TryParse(span, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
                    count = FunctionUtil.F_base_countif(list, d);
                    sum = FunctionUtil.F_base_sumif(list, d, sumdbs);
                } else {
                    var m2 = FunctionUtil.sumifMatch(args2.TextValue.Trim());
                    if (m2 != null) {
                        count = FunctionUtil.F_base_countif(list, m2.Item1, m2.Item2);
                        sum = FunctionUtil.F_base_sumif(list, m2.Item1, m2.Item2, sumdbs);
                    } else {
                        return ParameterError(2);
                    }
                }
            }
            if (count == 0) {
                return Div0Error();
            }
            return Operand.Create(sum / count);
        }

    }

}
