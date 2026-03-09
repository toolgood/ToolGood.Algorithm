using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal sealed class Function_MEDIAN : Function_N
    {
        public Function_MEDIAN(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override string Name => "Median";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args = new List<Operand>(funcs.Length); foreach (var item in funcs) { var aa = item.Evaluate(engine, tempParameter); if (aa.IsErrorOrNone) { return aa; } args.Add(aa); }

            var list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);

            if (o == false) { return FunctionError(); }
            if (list.Count == 0) { return FunctionError(); }

            list.Sort();
            var mid = list.Count / 2;
            if (list.Count % 2 == 0) {
                return Operand.Create((list[mid - 1] + list[mid]) / 2);
            }
            return Operand.Create(list[mid]);
        }
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}
	}

}
