using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal sealed class Function_GCD : Function_N
    {
        public Function_GCD(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override string Name => "Gcd";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args = new List<Operand>(funcs.Length);
            for (int i = 0; i < funcs.Length; i++) { 
                var aa = GetNumber(engine, tempParameter, i);
                if (aa.IsErrorOrNone) { return aa; } 
                args.Add(aa); 
            }

            var list = new List<decimal>();
            var o = FunctionUtil.FlattenToList(args, list);
            if (o == false) { return FunctionError(); }

            for (int i = 0; i < list.Count; i++) {
                if (list[i] < 0) {
                    return ParameterError(i + 1);
                }
            }

            return Operand.Create(FunctionUtil.GetGcd(list));
        }
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			for(int i = 0; i < funcs.Length; i++) {
				funcs[i].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			}
		}
	}

}