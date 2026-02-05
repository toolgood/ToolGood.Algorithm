using System;
using System.Collections.Generic;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal class Function_PRODUCT : Function_N
    {
        public Function_PRODUCT(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override string Name => "Product";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args = new List<Operand>(funcs.Length);
            for (int i = 0; i < funcs.Length; i++) { 
                var aa = GetNumber(work, tempParameter, i);
                if (aa.IsError) { return aa; } 
                args.Add(aa); 
            }

            var list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter is error!", "Product"); }

            decimal d = 1;
            for (int i = 0; i < list.Count; i++) {
                var a = list[i];
                d *= a;
            }
            return Operand.Create(d);
        }

    }

}