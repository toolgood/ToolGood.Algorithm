using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

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
            var args = new List<Operand>(funcs.Length);
            var error = TryEvaluateAll(engine, tempParameter, args);
            if(error != null) { return error; }

            var list = new List<decimal>();
            var o = FunctionUtil.FlattenToList(args, list);
            if (o == false) { return FunctionError(); }
            if (list.Count == 0) { return FunctionError(); }

            var dict = new Dictionary<decimal, int>();
            for(int i = 0; i < list.Count; i++) {
                if (dict.TryGetValue(list[i], out int count)) {
                    dict[list[i]] = count + 1;
                } else {
                    dict[list[i]] = 1;
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
				funcs[0].GetParameterTypes(noneEngine, result, OperandType.ARRAY);
			} else {
				for(int i = 0; i < funcs.Length; i++) {
					funcs[i].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
				}
			}
		}
	}

}
