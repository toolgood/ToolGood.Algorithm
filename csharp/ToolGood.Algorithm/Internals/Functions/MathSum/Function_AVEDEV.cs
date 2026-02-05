using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_AVEDEV : Function_N
    {
        public Function_AVEDEV(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override string Name => "AveDev";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args = new List<Operand>(funcs.Length); foreach (var item in funcs) { var aa = item.Evaluate(work, tempParameter); if (aa.IsError) { return aa; } args.Add(aa); }

            var list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return FunctionError(); }
            if (list.Count == 0) { return Operand.Zero; }
            var avg = list.Average();
            decimal sum = 0;
            foreach (var item in list) {
                sum += Math.Abs(item - avg);
            }
            return Operand.Create(sum / list.Count);
        }

    }

}
