using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Operands
{
	/// <summary>
	/// EvaluateWithoutEngine 特有值
	/// </summary>
	internal class OperandNone : Operand
	{
		public override OperandType Type => OperandType.NONE;
		internal override bool IsErrorOrNone => true;
		public override bool IsNone => true;
	}
}
