using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal sealed class Function_MODE : Function_N
    {
        public Function_MODE(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override string Name => "Mode";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args = new List<Operand>(funcs.Length); foreach (var item in funcs) { var aa = item.Evaluate(engine, tempParameter); if (aa.IsError) { return aa; } args.Add(aa); }

            var list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return FunctionError(); }

            var dict = new Dictionary<decimal, int>();
            foreach (var item in list) {
                if (dict.TryGetValue(item, out int count)) {
                    dict[item] = count + 1;
                } else {
                    dict[item] = 1;
                }
            }
            decimal modeKey = 0;
            int maxCount = 0;
            foreach (var kvp in dict) {
                if (kvp.Value > maxCount) {
                    maxCount = kvp.Value;
                    modeKey = kvp.Key;
                }
            }
            return Operand.Create(modeKey);
        }

    }

}
