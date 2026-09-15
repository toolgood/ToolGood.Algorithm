using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{

	internal sealed class Function_HAS : Function_2
	{
		public Function_HAS(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length != 2) {
				throw new ArgumentException($"Function '{Name}' requires exactly 2 parameters.");
			}
		}

		public override string Name => "Has";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }

			var args2 = GetText_2(engine, tempParameter);
			if(args2.IsErrorOrNone) { return args2; }

			var text = args2.TextValue;
			if(args1 is OperandKeyValueList keyValueList) {
				return Operand.Create(keyValueList.ContainsKey(args2));
			} else if(args1 is OperandKeyValue keyValue) {
				return Operand.Create(keyValue.Value.Key == text);
			} else if(args1.IsJson) {
				var json = args1.JsonValue;
				if(json.IsArray) {
					for(int i = 0; i < json.Count; i++) {
						if(FunctionUtil.JsonValueEquals(json[i], text)) { return Operand.True; }
					}
				} else if(json[text] != null) {
					return Operand.True;
				}
				return Operand.False;
			} else if(args1.IsArray) {
				var ar = ((OperandArray)args1);
				foreach(var item in ar.ArrayValue) {
					var t = item.ToText();
					if(t.IsErrorOrNone) { continue; }
					if(t.TextValue == text) {
						return Operand.True;
					}
				}
				return Operand.False;
			}
			return ParameterError(1);
		}
		public override OperandType GetResultType()
		{
			return OperandType.BOOLEAN;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.JSON);
			func2.GetParameterTypes(noneEngine, result, OperandType.TEXT);
		}
	}

}
