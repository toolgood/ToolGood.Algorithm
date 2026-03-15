using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

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
			var error = TryEvaluateAll(engine, tempParameter, args);
			if(error != null) { return error; }
			var args1 = args[0];
			if(args1.IsJson) {
				var o = args1.ToArray(null);
				if(o.IsErrorOrNone == false) {
					args1 = o;
				}
			}
			if(args1.IsArray) {
				var list = new List<string>(args1.ArrayValue.Count);
				var o = FunctionUtil.FlattenToList(args1, list);
				if(o == false) return ParameterError(1);

				var args2 = ConvertToText(args[1], 2);
				if(args2.IsErrorOrNone) { return args2; }

				return Operand.Create(string.Join(args2.TextValue, list));
			} else {
				args1 = ConvertToText(args1, 1);
				if(args1.IsErrorOrNone) { return args1; }

				var list = new List<string>(args.Count);
				for(int i = 1; i < args.Count; i++) {
					var o = FunctionUtil.FlattenToList(args[i], list);
					if(o == false) return ParameterError(i + 1);
				}
				return Operand.Create(string.Join(args1.TextValue, list));
			}
		}

		public override OperandType GetResultType()
		{
			return OperandType.TEXT;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			foreach(var item in funcs) {
				item.GetParameterTypes(noneEngine, result, OperandType.NONE);
			}
		}

	}

}
