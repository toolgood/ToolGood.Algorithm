using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal sealed class Function_AVEDEV : Function_N
    {
        public Function_AVEDEV(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override string Name => "AveDev";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args = new List<Operand>(funcs.Length);
            var error = TryEvaluateAll(engine, tempParameter, args);
            if(error != null) { return error; }

            var list = new List<decimal>();
            var o = FunctionUtil.FlattenToList(args, list);
            if (o == false) { return FunctionError(); }
            if (list.Count == 0) { return Operand.Zero; }
            decimal sum = 0;
            for(int i = 0; i < list.Count; i++) { sum += list[i]; }
            var avg = sum / list.Count;
            decimal sum2 = 0;
            for(int i = 0; i < list.Count; i++) {
                sum2 += Math.Abs(list[i] - avg);
            }
            return Operand.Create(sum2 / list.Count);
        }
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
            if(funcs.Length==0) {
				funcs[0].GetParameterTypes(noneEngine, result, OperandType.ARRAY);
			} else {
				for(int i = 0; i < funcs.Length; i++) {
					funcs[i].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
				}
			}
		}
	}

}
