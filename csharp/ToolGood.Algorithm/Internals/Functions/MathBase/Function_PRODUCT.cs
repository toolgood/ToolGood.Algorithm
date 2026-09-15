using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal sealed class Function_PRODUCT : Function_N
    {
        public Function_PRODUCT(FunctionBase[] funcs) : base(funcs)
        {
            if (funcs.Length < 1) {
                throw new ArgumentException($"Function '{Name}' requires at least 1 parameter.");
            }
        }

        public override string Name => "Product";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args = new List<Operand>(funcs.Length);
            var error = TryEvaluateAll(engine, tempParameter, args);
            if (error != null) { return error; }

            // 直接求值并展平,避免对数组参数调用 GetNumber 导致报错
            var list = new List<decimal>();
            if (FunctionUtil.FlattenToList(args, list) == false) { return FunctionError(); }

            decimal d = 1;
            try {
                for (int i = 0; i < list.Count; i++) {
                    d *= list[i];
                }
            } catch (OverflowException) {
                return FunctionError();
            }
            return Operand.Create(d);
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