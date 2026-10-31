using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.Internals.Functions.Value
{
	/// <summary>
	/// 常量折叠的产物节点：直接持有编译期算好的 Operand。
	/// 仅由 <see cref="ToolGood.Algorithm.math.FastAstBuilder"/> 对「全部由字面量组成的纯子树」生成，
	/// 求值时零计算、零分配。
	/// </summary>
	internal sealed class Function_Constant : Function_0
	{
		private readonly Operand _value;

		public Function_Constant(Operand value)
		{
			_value = value;
		}

		public override string Name => "Const";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			return _value;
		}

		public override OperandType GetResultType()
		{
			return _value.Type;
		}

		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			stringBuilder.Append(_value.ToString());
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
		}
	}
}
