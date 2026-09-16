using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.Internals.Functions.Value
{
	internal sealed class Function_ArrayJsonItem : Function_1
	{
		private readonly string key;

		public Function_ArrayJsonItem(string key, FunctionBase func1) : base(func1)
		{
			this.key = key;
		}

		public override string Name => "ArrayJsonItem";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var v = func1.Evaluate(engine, tempParameter);
			// 必须在此向上传播错误: 若把错误操作数包裹进 KeyValue, 外层 Function_ArrayJson 的 IsErrorOrNone 检查会失效
			if(v.IsErrorOrNone) { return v; }
			var keyValue = new KeyValue {
				Key = key,
				Value = v
			};
			return new OperandKeyValue(keyValue);
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			// key 必须按字符串字面量输出(带引号并转义), 否则含空格/引号/控制字符的 key
			// 还原后不再是合法 key, 导致表达式往返解析失败
			// (Operand.Create(string).ToString() 即 OperandString 的 JSON 风格转义输出)
			stringBuilder.Append(Operand.Create(key).ToString());
			stringBuilder.Append(':');
			func1.ToString(stringBuilder, false);
		}
		public override OperandType GetResultType()
		{
			return OperandType.ARRAYJSON;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NONE);
		}
	}
}
