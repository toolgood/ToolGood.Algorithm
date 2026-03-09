using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal sealed class Function_MIN : Function_N
    {
        public Function_MIN(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override string Name => "Min";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args = new List<Operand>(funcs.Length); foreach (var item in funcs) { var aa = item.Evaluate(engine, tempParameter); if (aa.IsErrorOrNone) { return aa; } args.Add(aa); }

            var list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return FunctionError(); }
            if (list.Count == 0) { return FunctionError(); }
            
            return Operand.Create(list.Min());
        }
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			if(funcs.Length == 0) {
				funcs[0].GetParameterTypes(noneEngine, result, OperandType.ARRARY);
			} else {
				for(int i = 0; i < funcs.Length; i++) {
					funcs[i].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
				}
			}
		}
	}

}
