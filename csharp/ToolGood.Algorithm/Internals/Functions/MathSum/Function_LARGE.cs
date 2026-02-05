using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_LARGE : Function_2
    {
        public Function_LARGE(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override string Name => "Large";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            args1 = FunctionUtil.ConvertToArray(args1, "Large", 1);
            if (args1.IsError) { return args1; }

            var args2 = func2.Evaluate(work, tempParameter);
            args2 = FunctionUtil.ConvertToNumber(args2, "Large", 2);
            if (args2.IsError) { return args2; }

            var list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args1, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter {1} is error!", "Large", 1); }

            list = list.OrderByDescending(q => q).ToList();
            int k = args2.IntValue;
            if (k < 1 - work.ExcelIndex || k > list.Count - work.ExcelIndex) {
                return Operand.Error("Function '{0}' parameter {1} is error!", "Large", 2);
            }
            return Operand.Create(list[k - work.ExcelIndex]);
        }

    }

}
