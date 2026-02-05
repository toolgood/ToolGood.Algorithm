using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_AVERAGEIF : Function_3
    {
        public Function_AVERAGEIF(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override string Name => "AverageIf";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetArray_1(work, tempParameter); if (args1.IsError) { return args1; }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsError) { return args2; }

            var list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args1, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter {1} is error!", "AverageIf", 1); }

            List<decimal> sumdbs;
            if (func3 != null) {
                var args3 = GetArray_3(work, tempParameter); if (args3.IsError) { return args3; }
                sumdbs = new List<decimal>();
                var o2 = FunctionUtil.F_base_GetList(args3, sumdbs);
                if (o2 == false) { return Operand.Error("Function '{0}' parameter {1} is error!", "AverageIf", 3); }
            } else {
                sumdbs = list;
            }

            decimal sum;
            int count;
            if (args2.IsNumber) {
                count = FunctionUtil.F_base_countif(list, args2.NumberValue);
                sum = count * args2.NumberValue;
            } else {
                if (decimal.TryParse(args2.TextValue.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
                    count = FunctionUtil.F_base_countif(list, d);
                    sum = FunctionUtil.F_base_sumif(list, d, sumdbs);
                } else {
                    var sunif = args2.TextValue.Trim();
                    var m2 = FunctionUtil.sumifMatch(sunif);
                    if (m2 != null) {
                        count = FunctionUtil.F_base_countif(list, m2.Item1, m2.Item2);
                        sum = FunctionUtil.F_base_sumif(list, m2.Item1, m2.Item2, sumdbs);
                    } else {
                        return Operand.Error("Function '{0}' parameter {1} is error!", "AverageIf", 2);
                    }
                }
            }
            if (count == 0) {
                return Operand.Error("Function '{0}' div 0 error!", "AverageIf");
            }
            return Operand.Create(sum / count);
        }

    }

}
