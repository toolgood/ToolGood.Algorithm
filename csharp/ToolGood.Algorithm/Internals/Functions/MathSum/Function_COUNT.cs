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
            // 逐个求值并直接展平，免去中间的 List<Operand> 参数中转；
            // 注意：此处必须展平为 Operand 列表——COUNT 的语义是逐项判断
            // “能否转为数值”，不可转换的项跳过而不报错，与 decimal 展平不同
            var list = new List<Operand>();
            for(int i = 0; i < funcs.Length; i++) {
                var aa = funcs[i].Evaluate(engine, tempParameter);
                if(aa.IsErrorOrNone) { return aa; }
                if(FunctionUtil.FlattenToList(aa, list) == false) { return FunctionError(); }
            }

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
