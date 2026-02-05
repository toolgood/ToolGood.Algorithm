using System;
using System.Collections.Generic;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{

    internal class Function_SUMSQ : Function_N
    {
        public Function_SUMSQ(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override string Name => "SumSq";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args = new List<Operand>(funcs.Length);
            for (int i = 0; i < funcs.Length; i++) { var aa = funcs[i].Evaluate(work, tempParameter); if (aa.IsError) { return aa; } args.Add(aa); }

            var list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter is error!", "SumSQ"); }

            decimal d = 0;
            for (int i = 0; i < list.Count; i++) {
                var a = list[i];
                d += a * a;
            }
            return Operand.Create(d);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "SumSQ");
        }
    }

    

}
