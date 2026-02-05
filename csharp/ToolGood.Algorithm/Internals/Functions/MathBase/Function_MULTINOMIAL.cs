using System;
using System.Collections.Generic;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal class Function_MULTINOMIAL : Function_N
    {
        public Function_MULTINOMIAL(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override string Name => "Multinomial";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args = new List<Operand>();
            for (int i = 0; i < funcs.Length; i++) { 
                var aa = GetNumber(work, tempParameter, i);
                if (aa.IsError) { return aa; } 
                args.Add(aa); 
            }
            var list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter is error!", "Multinomial"); }

            int sum = 0;
            int n = 1;
            for (int i = 0; i < list.Count; i++) {
                var a = (int)list[i]; // 小于等于0 时，按0处理
                n *= FunctionUtil.F_base_Factorial(a);
                sum += a;
            }

            var r = FunctionUtil.F_base_Factorial(sum) / n;
            return Operand.Create(r);
        }

    }

}