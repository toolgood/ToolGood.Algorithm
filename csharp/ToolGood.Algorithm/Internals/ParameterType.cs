using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

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
		/// 
		/// </summary>
		public string Value { get; internal set; }
	}
}
