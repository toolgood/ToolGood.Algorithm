using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_SUMIF : Function_3
    {
        public Function_SUMIF(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsError) { return args1; }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsError) { return args2; }

            var list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args1, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter {1} is error!", "SumIf", 1); }

            List<decimal> sumdbs;
            if (func3 != null) {
                var args3 = func3.Evaluate(work, tempParameter); if (args3.IsError) { return args3; }
                sumdbs = new List<decimal>();
                var o2 = FunctionUtil.F_base_GetList(args3, sumdbs);
                if (o2 == false) { return Operand.Error("Function '{0}' parameter {1} is error!", "SumIf", 3); }
            } else {
                sumdbs = list;
            }

            decimal sum;
            if (args2.IsNumber) {
                sum = FunctionUtil.F_base_countif(list, args2.NumberValue) * args2.NumberValue;
            } else {
                if (decimal.TryParse(args2.TextValue.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
                    sum = FunctionUtil.F_base_sumif(list, d, sumdbs);
                } else {
                    var sunif = args2.TextValue.Trim();
                    var m2 = FunctionUtil.sumifMatch(sunif);
                    if (m2 != null) {
                        sum = FunctionUtil.F_base_sumif(list, m2.Item1, m2.Item2, sumdbs);
                    } else {
                        return Operand.Error("Function '{0}' parameter {1} is error!", "SumIf", 2);
                    }
                }
            }
            return Operand.Create(sum);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "SumIf");
        }
    }

}
