using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.LitJson;

namespace ToolGood.Algorithm.Internals.Functions.Value
{

	internal class Function_JSON : Function_1
	{
		public Function_JSON(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "Json";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter);
			if (args1.IsError) { return args1; }
			if (args1.IsJson) { return args1; }
			if (args1.IsArrayJson) { args1 = args1.ToText(); }
			if (args1.IsNotText) { return Operand.Error("Function '{0}' parameter is error!", "Json"); }
			var txt = args1.TextValue;
			if ((txt.StartsWith('{') && txt.EndsWith('}')) || (txt.StartsWith('[') && txt.EndsWith(']'))) {
				try {
					var json = JsonMapper.ToObject(txt);
					return Operand.Create(json);
				} catch (Exception) { }
			}
			return Operand.Error("Function '{0}' parameter is error!", "Json");
		}

	}

}
