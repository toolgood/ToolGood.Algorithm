using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.LitJson;

namespace ToolGood.Algorithm.Internals.Functions.Value
{

	internal sealed class Function_JSON : Function_1
	{
		public Function_JSON(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "Json";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }
			if(args1.IsJson) { return args1; }
			if(args1.IsArrayJson) { args1 = args1.ToText(); }
			if(args1.IsText == false) { return ParameterError(1); }
			var txt = args1.TextValue;
			if((txt.StartsWith('{') && txt.EndsWith('}')) || (txt.StartsWith('[') && txt.EndsWith(']'))) {
				try {
					var json = JsonMapper.ToObject(txt);
					return Operand.Create(json);
				} catch(Exception) { }
			}
			return ParameterError(1);
		}
		public override OperandType GetResultType()
		{
			return OperandType.JSON;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NONE);
		}
	}

}
