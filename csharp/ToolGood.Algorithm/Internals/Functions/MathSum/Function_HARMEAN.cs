using System;
using System.Collections.Generic;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_HARMEAN : Function_N
    {
        public Function_HARMEAN(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override string Name => "HarMean";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
			var args = new List<Operand>(funcs.Length); foreach(var item in funcs) { var aa = item.Evaluate(work, tempParameter); if(aa.IsError) { return aa; } args.Add(aa); }


			if(args.Count == 1) return args[0];

            var list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return FunctionError(); }

            decimal sum = 0;
            foreach (var db in list) {
                if (db == 0) {
                    return FunctionError();
                }
                sum += 1 / db;
            }
            if (sum == 0) {
                return FunctionError();
            }
            return Operand.Create(list.Count / sum);
        }

    }

}