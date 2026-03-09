using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal sealed class Function_MULTINOMIAL : Function_N
    {
        public Function_MULTINOMIAL(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override string Name => "Multinomial";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
			var args = new List<Operand>(funcs.Length); foreach(var item in funcs) { var aa = item.Evaluate(engine, tempParameter); if(aa.IsErrorOrNone || aa.IsNone) { return aa; } args.Add(aa); }

			var list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return FunctionError(); }

            int sum = 0;
            int n = 1;
            for (int i = 0; i < list.Count; i++) {
                var a = (int)list[i]; // С�ڵ���0 ʱ����0����
                n *= FunctionUtil.F_base_Factorial(a);
                sum += a;
            }

            var r = FunctionUtil.F_base_Factorial(sum) / n;
            return Operand.Create(r);
        }
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

	}

}