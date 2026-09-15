using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal sealed class Function_COUNT : Function_N
    {
        public Function_COUNT(FunctionBase[] funcs) : base(funcs)
        {
            if (funcs.Length < 1) {
                throw new ArgumentException($"Function '{Name}' requires at least 1 parameter.");
            }
        }

        public override string Name => "Count";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args = new List<Operand>(funcs.Length);
            var error = TryEvaluateAll(engine, tempParameter, args);
            if(error != null) { return error; }

            var list = new List<Operand>();
            var o = FunctionUtil.FlattenToList(args, list);
            if (o == false) { return FunctionError(); }

            // 与 Excel 一致:只统计可转为数值的项,文本、空值等一律忽略
            int count = 0;
            for (int i = 0; i < list.Count; i++) {
                if (list[i].ToNumber(null).IsErrorOrNone == false) {
                    count++;
                }
            }
            return Operand.Create(count);
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
					funcs[i].GetParameterTypes(noneEngine, result, OperandType.NONE);
				}
			}
		}
	}

}
