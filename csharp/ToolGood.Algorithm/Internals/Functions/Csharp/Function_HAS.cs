using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{

	internal class Function_HAS : Function_2
	{
		public Function_HAS(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override string Name => "Has";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter);
			if(args1.IsError) { return args1; }

			var args2 = func2.Evaluate(work, tempParameter);
			args2 = ConvertToText(args2, "Has", 2);
			if(args2.IsError) { return args2; }

			if(args1.IsArrayJson) {
				return Operand.Create(((OperandKeyValueList)args1).ContainsKey(args2));
			} else if(args1.IsJson) {
				var json = args1.JsonValue;
				if(json.IsArray) {
					for(int i = 0; i < json.Count; i++) {
						var v = json[i];
						if(v.IsString) {
							if(v.StringValue == args2.TextValue) { return Operand.True; }
						} else if(v.IsDouble) {
							if(v.NumberValue.ToString() == args2.TextValue) { return Operand.True; }
						} else if(v.IsBoolean) {
							if(v.BooleanValue.ToString().Equals(args2.TextValue, StringComparison.CurrentCultureIgnoreCase)) { return Operand.True; }
						}
					}
				} else {
					var v = json[args2.TextValue];
					if(v != null) {
						return Operand.True;
					}
				}
				return Operand.False;
			} else if(args1.IsArray) {
				var ar = ((OperandArray)args1);
				foreach(var item in ar.ArrayValue) {
					var t = item.ToText();
					if(t.IsError) { continue; }
					if(t.TextValue == args2.TextValue) {
						return Operand.True;
					}
				}
				return Operand.False;
			}
			return Operand.Error("Function '{0}' parameter {1} is error!", "Has", 1);
		}

	}


}
