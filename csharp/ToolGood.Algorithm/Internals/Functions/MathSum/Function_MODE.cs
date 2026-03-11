using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal sealed class Function_MODE : Function_N
    {
        public Function_MODE(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override string Name => "Mode";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args = new List<Operand>(funcs.Length); foreach (var item in funcs) { var aa = item.Evaluate(engine, tempParameter); if (aa.IsErrorOrNone) { return aa; } args.Add(aa); }

            var list = new List<decimal>();
            var o = FunctionUtil.FlattenToList(args, list);
            if (o == false) { return FunctionError(); }
            if (list.Count == 0) { return FunctionError(); }

            var dict = new Dictionary<decimal, int>();
            foreach (var item in list) {
                if (dict.TryGetValue(item, out int count)) {
                    dict[item] = count + 1;
                } else {
                    dict[item] = 1;
                }
            }
            decimal modeKey = 0;
            int maxCount = 1;
            foreach (var kvp in dict) {
                if (kvp.Value > maxCount) {
                    maxCount = kvp.Value;
                    modeKey = kvp.Key;
                }
            }
            if (maxCount == 1) { return FunctionError(); }
            return Operand.Create(modeKey);
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
