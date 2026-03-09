using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Operands
{
	/// <summary>
	/// EvaluateWithoutEngine 特有值
	/// </summary>
	internal class OperandNone : Operand
	{
		public override OperandType Type => OperandType.NONE;
		public override bool IsNone => true;
	}
}
