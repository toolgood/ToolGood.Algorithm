using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal sealed class Function_VARP : Function_N
    {
        public Function_VARP(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override string Name => "VarP";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
			var args = new List<Operand>(funcs.Length); foreach(var item in funcs) { var aa = item.Evaluate(engine, tempParameter); if(aa.IsError) { return aa; } args.Add(aa); }

			if(args.Count == 1) { return Operand.Error("Function '{0}' parameter only one error!", "VarP"); }
            var list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return FunctionError(); }
            if (list.Count == 0) { return FunctionError(); }
            if (list.Count == 1) { return Operand.Zero; }

            decimal mean = 0, m2 = 0;
            for (int i = 0; i < list.Count; i++) {
                decimal delta = list[i] - mean;
                mean += delta / (i + 1);
                m2 += delta * (list[i] - mean);
            }
            return Operand.Create(m2 / list.Count);
        }

    }

}