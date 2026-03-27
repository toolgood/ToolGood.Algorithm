using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Operands
{
	internal sealed class OperandError : Operand
	{
		private readonly string _errorMsg;
		private readonly object[] _args;
		private string _formattedMsg;
		public override OperandType Type => OperandType.ERROR;
		internal override bool IsErrorOrNone => true;
		public override bool IsError => true;
		public override string ErrorMsg => _formattedMsg ?? (_formattedMsg = FormatMessage());

		public OperandError(string msg)
		{
			_errorMsg = msg;
			_args = null;
		}

		public OperandError(string msg, object[] args)
		{
			_errorMsg = msg;
			_args = args;
		}

		private string FormatMessage()
		{
			if (_args == null || _args.Length == 0)
				return _errorMsg;
			return string.Format(_errorMsg, _args);
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