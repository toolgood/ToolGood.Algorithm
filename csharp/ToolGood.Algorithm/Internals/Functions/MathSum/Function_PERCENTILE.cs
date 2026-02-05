using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_PERCENTILE : Function_2
    {
        public Function_PERCENTILE(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override string Name => "Percentile";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            args1 = ConvertToArray(args1, "Percentile", 1);
            if (args1.IsError) { return args1; }

            var args2 = func2.Evaluate(work, tempParameter);
            args2 = ConvertToNumber(args2, "Percentile", 2);
            if (args2.IsError) { return args2; }

            var list = new List<double>();
            var o = FunctionUtil.F_base_GetList(args1, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter {1} is error!", "Percentile", 1); }
            var k = args2.DoubleValue;
            return Operand.Create(ExcelFunctions.Percentile(list.Select(q => (double)q).ToArray(), (double)k));
        }

    }

}
