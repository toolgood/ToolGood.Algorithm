using Antlr4.Runtime;

namespace ToolGood.Algorithm.Internals.Visitors
{
	/// <summary>
	/// This class supports case-insensitive lexing by wrapping an existing
	/// <see cref="ICharStream"/> and forcing the lexer to see either upper or
	/// lowercase characters. Grammar literals should then be either upper or
	/// lower case such as 'BEGIN' or 'begin'. The text of the character
	/// stream is unaffected. Example: input 'BeGiN' would match lexer rule
	/// 'BEGIN' if constructor parameter upper=true but getText() would return
	/// 'BeGiN'.
	/// </summary>
	sealed class AntlrCharStream : AntlrInputStream
	{
		public AntlrCharStream()
		{
		}

		public AntlrCharStream(string input) : base(input)
		{
		}

		public override int LA(int i)
		{
			int c = base.LA(i);

			if(c <= 0) {
				return c;
			}
			return CharUtil.StandardChar((char)c);
		}
	}
}