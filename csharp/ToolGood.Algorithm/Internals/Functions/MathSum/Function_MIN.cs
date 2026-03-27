using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

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
            var args = new List<Operand>(funcs.Length);
            var error = TryEvaluateAll(engine, tempParameter, args);
            if(error != null) { return error; }

            var list = new List<decimal>();
            var o = FunctionUtil.FlattenToList(args, list);
            if (o == false) { return FunctionError(); }
            if (list.Count == 0) { return FunctionError(); }
            
            decimal min = list[0];
            for(int i = 1; i < list.Count; i++) {
                if(list[i] < min) { min = list[i]; }
            }
            return Operand.Create(min);
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
