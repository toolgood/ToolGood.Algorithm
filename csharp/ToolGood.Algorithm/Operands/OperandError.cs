using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm
{
	internal sealed class OperandError : Operand
	{
		public override OperandType Type => OperandType.ERROR;
		public override bool IsError => true;
		private readonly string _errorMsg;
		public override string ErrorMsg => _errorMsg;

		public OperandError(string msg)
		{
			_errorMsg = msg;
		}

		public override Operand ToNumber(string errorMessage) { return this; }
		public override Operand ToNumber(string errorMessage, params object[] args) { return this; }

		public override Operand ToBoolean(string errorMessage) { return this; }
		public override Operand ToBoolean(string errorMessage, params object[] args) { return this; }

		public override Operand ToText(string errorMessage) { return this; }
		public override Operand ToText(string errorMessage, params object[] args) { return this; }

		public override Operand ToArray(string errorMessage) { return this; }
		public override Operand ToArray(string errorMessage, params object[] args) { return this; }

		public override Operand ToMyDate(string errorMessage) { return this; }
		public override Operand ToMyDate(string errorMessage, params object[] args) { return this; }
	}
}