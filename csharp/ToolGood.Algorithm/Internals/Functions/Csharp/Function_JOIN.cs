using System;
using System.Collections.Generic;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{
	internal sealed class Function_JOIN : Function_N
	{
		public Function_JOIN(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "Join";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args = new List<Operand>(funcs.Length);
			foreach(var item in funcs) { var aa = item.Evaluate(engine, tempParameter); if(aa.IsError) { return aa; } args.Add(aa); }
			var args1 = args[0];
			if(args1.IsJson) {
				var o = args1.ToArray(null);
				if(o.IsError == false) {
					args1 = o;
				}
			}
			if(args1.IsArray) {
				var list = new List<string>(args1.ArrayValue.Count);
				var o = FunctionUtil.F_base_GetList(args1, list);
				if(o == false) return ParameterError(1);

				var args2 = ConvertToText(args[1], 2);
				if(args2.IsError) { return args2; }

				return Operand.Create(string.Join(args2.TextValue, list));
			} else {
				args1 = ConvertToText(args1, 1);
				if(args1.IsError) { return args1; }

				var list = new List<string>(args.Count);
				for(int i = 1; i < args.Count; i++) {
					var o = FunctionUtil.F_base_GetList(args[i], list);
					if(o == false) return ParameterError(i + 1);
				}
				return Operand.Create(string.Join(args1.TextValue, list));
			}
		}

	}


}
