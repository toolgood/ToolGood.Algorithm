using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Sharpen;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks.Dataflow;
using static ANTLRv4Parser;

namespace Antlr4Helper.CSharpHelper.ANTLRv4
{
	public class LexerVisitor2 : ANTLRv4ParserBaseVisitor<string>, IANTLRv4ParserVisitor<string>
	{
		public List<string> VisitTerminalDefs = new List<string>();
		public List<LexerRegex> LexerRegexes = new List<LexerRegex>();
		public string lexerCommands = null;
		private bool b = true;
		int T = 0;

		public override string VisitTerminalDef([NotNull] ANTLRv4Parser.TerminalDefContext context)
		{
			if(b) {
				var txt = context.GetText();
				if(txt.StartsWith("'")) {
					txt = txt.Substring(1, txt.Length - 2);
					txt = txt.Replace("\\'", "'");
					txt = txt.Replace("\\", "\\\\");
					txt = txt.Replace(".", "\\.");
					txt = txt.Replace("-", "\\-");
					txt = txt.Replace("+", "\\+");
					txt = txt.Replace("?", "\\?");
					txt = txt.Replace("*", "\\*");
					txt = txt.Replace("|", "\\|");
					txt = txt.Replace(")", "\\)");
					txt = txt.Replace("(", "\\(");
					txt = txt.Replace("]", "\\]");
					txt = txt.Replace("[", "\\[");
					txt = txt.Replace("{", "\\{");
					txt = txt.Replace("}", "\\}");
					if(VisitTerminalDefs.Contains(txt) == false) {
						VisitTerminalDefs.Add(txt);
						Console.WriteLine($"VisitTerminalDef: T__{T}: " + txt);
						LexerRegexes.Add(new LexerRegex { Name = $"T__{T}", Regex = txt });
						T++;
					}
					return null;
				}
			}
			if(context.TOKEN_REF() != null) {
				if(context.elementOptions() != null) {
					return context.TOKEN_REF().GetText() + Visit(context.elementOptions());
				}
				return context.TOKEN_REF().GetText();
			}
			var t = context.STRING_LITERAL().GetText();
			t = t.Substring(1, t.Length - 2);
			t = t.Replace("\\'", "'");
			t = t.Replace("\\", "\\\\");
			t = t.Replace(".", "\\.");
			t = t.Replace("-", "\\-");
			t = t.Replace("+", "\\+");
			t = t.Replace("?", "\\?");
			t = t.Replace("*", "\\*");
			t = t.Replace("|", "\\|");
			if(context.elementOptions() != null) {
				t = t + Visit(context.elementOptions());
			}
			return t;
		}


		public override string VisitLexerRuleSpec([NotNull] ANTLRv4Parser.LexerRuleSpecContext context)
		{
			b = false;
			var txt = context.GetText();
			var TOKEN_REF = context.TOKEN_REF().GetText();
			var lexerRuleBlock = Visit(context.lexerRuleBlock());
			if(context.FRAGMENT() != null) {
				LexerRegexes.Add(new LexerRegex { Name = TOKEN_REF, Regex = lexerRuleBlock, IsFragment = true,LexerCommands=lexerCommands });
			} else {
				LexerRegexes.Add(new LexerRegex { Name = TOKEN_REF, Regex = lexerRuleBlock, LexerCommands = lexerCommands });
			}
			lexerCommands = null;
			Console.WriteLine($"VisitLexerRuleSpec {TOKEN_REF}: {lexerRuleBlock}");
			return null;
		}
		public override string VisitLexerRuleBlock([NotNull] LexerRuleBlockContext context)
		{
			return Visit(context.lexerAltList());
			//return "(" + Visit(context.lexerAltList()) + ")";
		}
		//lexerAltList
		public override string VisitLexerAltList([NotNull] LexerAltListContext context)
		{
			var alts = context.lexerAlt();
			List<string> list = new List<string>();
			foreach(var alt in alts) {
				var txt = Visit(alt);
				if(txt != null) {
					list.Add(txt);
				}
			}
			return string.Join("|", list);
		}
		public override string VisitLexerAlt([NotNull] LexerAltContext context)
		{
			var txt = Visit(context.lexerElements());
			if(context.lexerCommands() != null) {
				lexerCommands = Visit(context.lexerCommands());
			}
			return txt;
		}

		public override string VisitLexerElements([NotNull] LexerElementsContext context)
		{
			var ls = context.lexerElement();
			List<string> list = new List<string>();
			foreach(var alt in ls) {
				var txt = Visit(alt);
				if(txt != null) {
					list.Add(txt);
				}
			}
			return string.Join("", list);
		}

		public override string VisitLexerElement([NotNull] LexerElementContext context)
		{
			if(context.lexerAtom() != null) {
				if(context.ebnfSuffix() != null) {
					return "(" + Visit(context.lexerAtom()) + ")" + Visit(context.ebnfSuffix());
				}
				return Visit(context.lexerAtom());
			}
			if(context.lexerBlock() != null) {
				if(context.ebnfSuffix() != null) {
					return "(" + Visit(context.lexerBlock()) + ")" + Visit(context.ebnfSuffix());
				}
				return Visit(context.lexerBlock());
			}
			if(context.QUESTION() != null) {
				return "(" + Visit(context.actionBlock()) + ")" + context.QUESTION().GetText();
			}
			return Visit(context.actionBlock());
		}
		public override string VisitEbnfSuffix([NotNull] EbnfSuffixContext context)
		{
			return context.GetText();
		}



		public override string VisitLexerBlock([NotNull] LexerBlockContext context)
		{
			return "(" + Visit(context.lexerAltList()) + ")";
		}
		public override string VisitActionBlock([NotNull] ActionBlockContext context)
		{
			return context.ACTION().GetText();
		}


		public override string VisitLexerAtom([NotNull] LexerAtomContext context)
		{
			if(context.characterRange() != null) {
				return Visit(context.characterRange());
			}
			if(context.terminalDef() != null) {
				return Visit(context.terminalDef());
			}
			if(context.notSet() != null) {
				return Visit(context.notSet());
			}
			if(context.wildcard() != null) {
				return Visit(context.wildcard());
			}
			return context.LEXER_CHAR_SET().GetText();
		}



		public override string VisitCharacterRange([NotNull] ANTLRv4Parser.CharacterRangeContext context)
		{
			var ts = context.STRING_LITERAL();
			var t1 = ts[0].GetText();
			t1 = t1.Substring(1, t1.Length - 2);
			var t2 = ts[1].GetText();
			t2 = t2.Substring(1, t2.Length - 2);
			return $"[{t1}-{t2}]";
		}

		public override string VisitAtom([NotNull] ANTLRv4Parser.AtomContext context)
		{
			if(context.terminalDef() != null) {
				return Visit(context.terminalDef());
			}
			if(context.ruleref() != null) {
				return Visit(context.ruleref());
			}
			if(context.notSet() != null) {
				return Visit(context.notSet());
			}
			return Visit(context.wildcard());
		}

		public override string VisitNotSet([NotNull] ANTLRv4Parser.NotSetContext context)
		{
			if(context.blockSet() != null) {
				return Visit(context.blockSet()).Replace("[", "[^");
			}
			return Visit(context.setElement()).Replace("[", "[^");
		}
		public override string VisitSetElement([NotNull] ANTLRv4Parser.SetElementContext context)
		{
			return context.GetText();
		}

		public override string VisitWildcard([NotNull] WildcardContext context)
		{
			return ".";
		}

		public override string VisitLexerCommands([NotNull] LexerCommandsContext context)
		{
			var ls = context.lexerCommand();
			List<string> list = new List<string>();
			foreach(var item in ls) {
				var t = item.GetText();
				list.Add(t);
			}
			return string.Join(",", list);
		}
	}
}
