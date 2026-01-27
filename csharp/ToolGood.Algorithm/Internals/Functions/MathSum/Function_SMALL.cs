using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_SMALL : Function_2
    {
        public Function_SMALL(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotArray) { args1 = args1.ToArray("Function '{0}' parameter {1} is error!", "Small", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Small", 2); if (args2.IsError) { return args2; } }
            var list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args1, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter {1} is error!", "Small", 1); }
            list = list.OrderBy(q => q).ToList();
            int k = args2.IntValue;
            if (k < 1 - work.ExcelIndex || k > list.Count - work.ExcelIndex) {
                return Operand.Error("Function '{0}' parameter {1} is error!", "Small", 2);
            }
            return Operand.Create(list[k - work.ExcelIndex]);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Small");
        }
    }

}
