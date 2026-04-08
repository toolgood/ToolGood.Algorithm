using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal sealed class Function_VAR : Function_N
    {
        public Function_VAR(FunctionBase[] funcs) : base(funcs)
        {
            if (funcs.Length < 1) {
                throw new ArgumentException($"Function '{Name}' requires at least 1 parameter.");
            }
        }

        public override string Name => "Var";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
			var args = new List<Operand>(funcs.Length);
			var error = TryEvaluateAll(engine, tempParameter, args);
			if(error != null) { return error; }

            decimal mean = 0, m2 = 0;
            int count = 0;

            for(int i = 0; i < args.Count; i++) {
                var item = args[i];
                if(item.IsArray) {
                    var array = item.ArrayValue;
                    for(int j = 0; j < array.Count; j++) {
                        var elem = array[j];
                        if(elem.IsNumber) {
                            count++;
                            decimal delta = elem.NumberValue - mean;
                            mean += delta / count;
                            m2 += delta * (elem.NumberValue - mean);
                        } else if(elem.IsArray || elem.IsJson) {
                            var list = new List<decimal>();
                            if(!FunctionUtil.FlattenToList(elem, list)) { return FunctionError(); }
                            for(int k = 0; k < list.Count; k++) {
                                count++;
                                decimal delta = list[k] - mean;
                                mean += delta / count;
                                m2 += delta * (list[k] - mean);
                            }
                        } else {
                            var converted = elem.ToNumber(null);
                            if(converted.IsError) { return FunctionError(); }
                            count++;
                            decimal delta = converted.NumberValue - mean;
                            mean += delta / count;
                            m2 += delta * (converted.NumberValue - mean);
                        }
                    }
                } else if(item.IsNumber) {
                    count++;
                    decimal delta = item.NumberValue - mean;
                    mean += delta / count;
                    m2 += delta * (item.NumberValue - mean);
                } else if(item.IsJson) {
                    var list = new List<decimal>();
                    if(!FunctionUtil.FlattenToList(item, list)) { return FunctionError(); }
                    for(int k = 0; k < list.Count; k++) {
                        count++;
                        decimal delta = list[k] - mean;
                        mean += delta / count;
                        m2 += delta * (list[k] - mean);
                    }
                } else {
                    var converted = item.ToNumber(null);
                    if(converted.IsError) { return FunctionError(); }
                    count++;
                    decimal delta = converted.NumberValue - mean;
                    mean += delta / count;
                    m2 += delta * (converted.NumberValue - mean);
                }
            }

            if(count <= 1) { return FunctionError(); }
            return Operand.Create(m2 / (count - 1));
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