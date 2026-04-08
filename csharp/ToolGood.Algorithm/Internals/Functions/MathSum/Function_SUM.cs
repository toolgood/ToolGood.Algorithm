using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal sealed class Function_SUM : Function_N
    {
        public Function_SUM(FunctionBase[] funcs) : base(funcs)
        {
            if (funcs.Length < 1) {
                throw new ArgumentException($"Function '{Name}' requires at least 1 parameter.");
            }
        }

        public override string Name => "Sum";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args = new List<Operand>(funcs.Length);
            var error = TryEvaluateAll(engine, tempParameter, args);
            if(error != null) { return error; }

            decimal sum = 0;
            for(int i = 0; i < args.Count; i++) {
                var item = args[i];
                if(item.IsArray) {
                    var array = item.ArrayValue;
                    for(int j = 0; j < array.Count; j++) {
                        var elem = array[j];
                        if(elem.IsNumber) {
                            sum += elem.NumberValue;
                        } else if(elem.IsArray || elem.IsJson) {
                            var list = new List<decimal>();
                            if(FunctionUtil.FlattenToList(elem, list)) {
                                for(int k = 0; k < list.Count; k++) {
                                    sum += list[k];
                                }
                            } else {
                                return FunctionError();
                            }
                        } else {
                            var converted = elem.ToNumber(null);
                            if(converted.IsError) { return FunctionError(); }
                            sum += converted.NumberValue;
                        }
                    }
                } else if(item.IsNumber) {
                    sum += item.NumberValue;
                } else if(item.IsJson) {
                    var list = new List<decimal>();
                    if(!FunctionUtil.FlattenToList(item, list)) { return FunctionError(); }
                    for(int k = 0; k < list.Count; k++) {
                        sum += list[k];
                    }
                } else {
                    var converted = item.ToNumber(null);
                    if(converted.IsError) { return FunctionError(); }
                    sum += converted.NumberValue;
                }
            }
            return Operand.Create(sum);
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
