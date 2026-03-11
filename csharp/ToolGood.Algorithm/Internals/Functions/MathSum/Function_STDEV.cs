using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal sealed class Function_STDEV : Function_N
    {
        public Function_STDEV(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override string Name => "StDev";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args = new List<Operand>(funcs.Length); foreach (var item in funcs) { var aa = item.Evaluate(engine, tempParameter); if (aa.IsErrorOrNone) { return aa; } args.Add(aa); }

            var list = new List<decimal>();
            var o = FunctionUtil.FlattenToList(args, list);
            if (o == false) { return FunctionError(); }
            if (list.Count <= 1) { return FunctionError(); }

            decimal mean = 0, m2 = 0;
            for (int i = 0; i < list.Count; i++) {
				decimal delta = list[i] - mean;
                mean += delta / (i + 1);
                m2 += delta * (list[i] - mean);
            }
            return Operand.Create(MathEx.Sqrt(m2 / (list.Count - 1)));
        }
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			if(funcs.Length == 1) {
				funcs[0].GetParameterTypes(noneEngine, result, OperandType.ARRARY);
			} else {
				for(int i = 0; i < funcs.Length; i++) {
					funcs[i].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
				}
			}
		}
	}

}
