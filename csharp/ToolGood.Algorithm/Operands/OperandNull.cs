using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm
{
	internal sealed class OperandNull : Operand
	{
		public override bool IsNull => true;
		public override bool IsNotNull => false;
		public override OperandType Type => OperandType.NULL;
		public override string ToString() { return "null"; }
	}
}