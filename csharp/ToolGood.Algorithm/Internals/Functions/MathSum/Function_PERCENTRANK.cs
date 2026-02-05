using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_PERCENTRANK : Function_3
    {
        public Function_PERCENTRANK(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override string Name => "PercentRank";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetArray_1(work, tempParameter);
            if (args1.IsError) { return args1; }

            var args2 = GetNumber_2(work, tempParameter);
            if (args2.IsError) { return args2; }

            var list = new List<double>();
            var o = FunctionUtil.F_base_GetList(args1, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter is error!", "PercentRank"); }

            var k = args2.DoubleValue;
            var v = ExcelFunctions.PercentRank(list.Select(q => (double)q).ToArray(), (double)k);
            var d = 3;
            if (func3 != null) {
                var args3 = GetNumber_3(work, tempParameter);
                if (args3.IsError) { return args3; }
                d = args3.IntValue;
            }
            return Operand.Create(Math.Round(v, d, MidpointRounding.AwayFromZero));
        }

    }

}
