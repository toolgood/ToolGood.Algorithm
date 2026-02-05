using System;
using System.Collections.Generic;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_VAR : Function_N
    {
        public Function_VAR(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override string Name => "Var";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
			var args = new List<Operand>(); foreach(var item in funcs) { var aa = item.Evaluate(work, tempParameter); if(aa.IsError) { return aa; } args.Add(aa); }


			if(args.Count == 1) { return Operand.Error("Function '{0}' parameter only one error!", "Var"); }

            var list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return FunctionError(); }
            if (list.Count <= 1) { return FunctionError(); }
            decimal sum = 0;
            decimal sum2 = 0;
            for (int i = 0; i < list.Count; i++) {
                sum += list[i] * list[i];
                sum2 += list[i];
            }
            return Operand.Create((list.Count * sum - sum2 * sum2) / list.Count / (list.Count - 1));
        }

    }

}