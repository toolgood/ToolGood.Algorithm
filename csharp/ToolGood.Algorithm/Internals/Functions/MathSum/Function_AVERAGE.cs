using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_AVERAGE : Function_N
    {
        public Function_AVERAGE(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args = new List<Operand>(); foreach (var item in funcs) { var aa = item.Evaluate(work, tempParameter); if (aa.IsError) { return aa; } args.Add(aa); }

            var list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter is error!", "Average"); }
            if (list.Count == 0) { return Operand.Zero; }
            return Operand.Create(list.Average());
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Average");
        }
    }

}
