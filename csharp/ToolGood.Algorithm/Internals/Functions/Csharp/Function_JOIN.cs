using System;
using System.Collections.Generic;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{
	internal class Function_JOIN : Function_N
	{
		public Function_JOIN(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args = new List<Operand>();
			foreach(var item in funcs) { var aa = item.Evaluate(work, tempParameter); if(aa.IsError) { return aa; } args.Add(aa); }
			var args1 = args[0];
			if(args1.IsJson) {
				var o = args1.ToArray(null);
				if(o.IsError == false) {
					args1 = o;
				}
			}
			if(args1.IsArray) {
				var list = new List<string>();
				var o = FunctionUtil.F_base_GetList(args1, list);
				if(o == false) return Operand.Error("Function '{0}' parameter {1} is error!", "Join", 1);

				var args2 = args[1].ToText("Function '{0}' parameter {1} is error!", "Join", 2);
				if(args2.IsError) { return args2; }

				return Operand.Create(string.Join(args2.TextValue, list));
			} else {
				args1 = args1.ToText("Function '{0}' parameter {1} is error!", "Join", 1);
				if(args1.IsError) { return args1; }

				var list = new List<string>();
				for(int i = 1; i < args.Count; i++) {
					var o = FunctionUtil.F_base_GetList(args[i], list);
					if(o == false) return Operand.Error("Function '{0}' parameter {1} is error!", "Join", i + 1);
				}
				return Operand.Create(string.Join(args1.TextValue, list));
			}
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "Join");
		}
	}


}
