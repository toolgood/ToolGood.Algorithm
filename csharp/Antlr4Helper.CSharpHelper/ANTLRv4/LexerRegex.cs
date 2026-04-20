using System;
using System.Collections.Generic;
using System.Text;

namespace Antlr4Helper.CSharpHelper.ANTLRv4
{
	public class LexerRegex
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Regex { get; set; }
		public bool IsFragment { get; set; }
		public string LexerCommands { get; set; }

		public override string ToString()
		{
			if(IsFragment) {
				if(LexerCommands != null) {
					return $"LexerRegex: Name={Name}, Regex={Regex}, IsFragment={IsFragment}, LexerCommands={LexerCommands}";
				}
				return $"LexerRegex: Name={Name}, Regex={Regex}, IsFragment={IsFragment}";
			}
			if(LexerCommands != null) {
				return $"LexerRegex: Name={Name}, Regex={Regex}, LexerCommands={LexerCommands}";
			}
			return $"LexerRegex: Name={Name}, Regex={Regex}";
		}
	}
}
