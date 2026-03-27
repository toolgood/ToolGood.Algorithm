using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{

    internal sealed class Function_SUMSQ : Function_N
    {
        public Function_SUMSQ(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override string Name => "SumSq";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args = new List<Operand>(funcs.Length);
            for (int i = 0; i < funcs.Length; i++) { var aa = funcs[i].Evaluate(engine, tempParameter); if (aa.IsErrorOrNone) { return aa; } args.Add(aa); }

            var list = new List<decimal>();
            var o = FunctionUtil.FlattenToList(args, list);
            if (o == false) { return FunctionError(); }

            decimal d = 0;
            for (int i = 0; i < list.Count; i++) {
                var a = list[i];
                d += a * a;
            }
            return Operand.Create(d);
        }
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			if(funcs.Length == 1) {
				funcs[0].GetParameterTypes(noneEngine, result, OperandType.ARRAY);
			} else {
				for(int i = 0; i < funcs.Length; i++) {
					funcs[i].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
				}
			}
		}
	}

}
