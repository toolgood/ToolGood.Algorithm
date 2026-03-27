using System;
using System.Collections.Generic;
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
			var args = new List<Operand>(funcs.Length);
			var error = TryEvaluateAll(engine, tempParameter, args);
			if(error != null) { return error; }

			var list = new List<decimal>();
            var o = FunctionUtil.FlattenToList(args, list);
            if (o == false) { return FunctionError(); }

            int sum = 0;
            int n = 1;
            for (int i = 0; i < list.Count; i++) {
                var a = (int)list[i];
                if (a < 0) {
                    return ParameterError(i + 1);
                }
                n *= FunctionUtil.GetFactorial(a);
                sum += a;
            }

            var r = FunctionUtil.GetFactorial(sum) / n;
            return Operand.Create(r);
        }
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			foreach(var item in funcs) {
				item.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			}
		}

	}

}