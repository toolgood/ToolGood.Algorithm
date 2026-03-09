using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals
{
	/// <summary>
	/// 
	/// </summary>
	public class ParameterType
	{
		/// <summary>
		/// 
		/// </summary>
		public string Name { get; internal set; }
		/// <summary>
		/// 
		/// </summary>
		public OperandType Type { get; internal set; }

		/// <summary>
		/// 操作符，可为空
		/// </summary>
		public string Operator { get; internal set; }

		/// <summary>
		/// 操作值，可为空
		/// </summary>
		public string Value { get; internal set; }
	}
}
