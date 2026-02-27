using System;
using System.Collections.Generic;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal class Function_LCM : Function_N
    {
        public Function_LCM(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override string Name => "Lcm";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args = new List<Operand>(funcs.Length);
            for (int i = 0; i < funcs.Length; i++) { 
                var aa = GetNumber(engine, tempParameter, i);
                if (aa.IsError) { return aa; } 
                args.Add(aa); 
            }

            var list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return FunctionError(); }

            return Operand.Create(FunctionUtil.F_base_lgm(list));
        }

    }

}