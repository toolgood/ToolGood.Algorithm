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

            decimal sum = 0;
            foreach (var item in list) { sum += item; }
            decimal avg = sum / list.Count;
            decimal sum2 = 0;
            for (int i = 0; i < list.Count; i++) {
                sum2 += (avg - list[i]) * (avg - list[i]);
            }
            return Operand.Create(sum2 / list.Count);
        }

    }

}