using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal sealed class Function_MIN : Function_N
    {
        public Function_MIN(FunctionBase[] funcs) : base(funcs)
        {
            if (funcs.Length < 1) {
                throw new ArgumentException($"Function '{Name}' requires at least 1 parameter.");
            }
        }

        public override string Name => "Min";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args = new List<Operand>(funcs.Length);
            var error = TryEvaluateAll(engine, tempParameter, args);
            if(error != null) { return error; }

            bool found = false;
            decimal min = decimal.MaxValue;
            
            for(int i = 0; i < args.Count; i++) {
                var item = args[i];
                if(item.IsArray) {
                    var array = item.ArrayValue;
                    for(int j = 0; j < array.Count; j++) {
                        var elem = array[j];
                        if(elem.IsNumber) {
                            if(!found || elem.NumberValue < min) {
                                min = elem.NumberValue;
                                found = true;
                            }
                        } else if(elem.IsArray || elem.IsJson) {
                            var list = new List<decimal>();
                            if(FunctionUtil.FlattenToList(elem, list)) {
                                for(int k = 0; k < list.Count; k++) {
                                    if(!found || list[k] < min) {
                                        min = list[k];
                                        found = true;
                                    }
                                }
                            } else {
                                return FunctionError();
                            }
                        } else {
                            var converted = elem.ToNumber(null);
                            if(converted.IsError) { return FunctionError(); }
                            if(!found || converted.NumberValue < min) {
                                min = converted.NumberValue;
                                found = true;
                            }
                        }
                    }
                } else if(item.IsNumber) {
                    if(!found || item.NumberValue < min) {
                        min = item.NumberValue;
                        found = true;
                    }
                } else if(item.IsJson) {
                    var list = new List<decimal>();
                    if(!FunctionUtil.FlattenToList(item, list)) { return FunctionError(); }
                    for(int k = 0; k < list.Count; k++) {
                        if(!found || list[k] < min) {
                            min = list[k];
                            found = true;
                        }
                    }
                } else {
                    var converted = item.ToNumber(null);
                    if(converted.IsError) { return FunctionError(); }
                    if(!found || converted.NumberValue < min) {
                        min = converted.NumberValue;
                        found = true;
                    }
                }
            }
            
            if(!found) { return FunctionError(); }
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
			} else if(funcs.Length == 2) {
				var t1 = funcs[0].GetResultType();
				var t2 = funcs[1].GetResultType();
				if(t1 == OperandType.NONE && t2 == OperandType.NUMBER) {
					var p = noneEngine.Evaluate(funcs[1]).ToText();
					if(t2 != OperandType.ERROR && p.IsErrorOrNone == false) {
						funcs[0].GetParameterTypes(noneEngine, result, t2, Name, p.TextValue);
						funcs[1].GetParameterTypes(noneEngine, result, t2);
						return;
					}
				} else if(t1 == OperandType.NUMBER && t2 == OperandType.NONE) {
					var p = noneEngine.Evaluate(funcs[0]).ToText();
					if(t1 != OperandType.ERROR && p.IsErrorOrNone == false) {
						funcs[1].GetParameterTypes(noneEngine, result, t1, Name, p.TextValue);
						funcs[0].GetParameterTypes(noneEngine, result, t1);
						return;
					}
				}
				funcs[0].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
				funcs[1].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			} else {
				for(int i = 0; i < funcs.Length; i++) {
					funcs[i].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
				}
			}
		}
	}

}
