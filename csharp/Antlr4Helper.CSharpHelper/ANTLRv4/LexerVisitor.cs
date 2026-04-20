using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Text;

namespace Antlr4Helper.CSharpHelper.ANTLRv4
{
	public class LexerVisitor : AbstractParseTreeVisitor<object>, IANTLRv4ParserVisitor<object>
	{


		public object VisitActionBlock([NotNull] ANTLRv4Parser.ActionBlockContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitActionBlock:" + txt);
			return VisitChildren(context);
		}

		public object VisitActionScopeName([NotNull] ANTLRv4Parser.ActionScopeNameContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitActionScopeName:" + txt);
			return VisitChildren(context);
		}

		public object VisitAction_([NotNull] ANTLRv4Parser.Action_Context context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitAction_:" + txt);
			return VisitChildren(context);
		}

		public object VisitAlternative([NotNull] ANTLRv4Parser.AlternativeContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitAlternative:" + txt);
			return VisitChildren(context);
		}

		public object VisitAltList([NotNull] ANTLRv4Parser.AltListContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitAltList:" + txt);
			return VisitChildren(context);
		}

		public object VisitArgActionBlock([NotNull] ANTLRv4Parser.ArgActionBlockContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitArgActionBlock:" + txt);
			return VisitChildren(context);
		}

		public object VisitAtom([NotNull] ANTLRv4Parser.AtomContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitAtom:" + txt);
			return VisitChildren(context);
		}

		public object VisitBlock([NotNull] ANTLRv4Parser.BlockContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitBlock:" + txt);
			return VisitChildren(context);
		}

		public object VisitBlockSet([NotNull] ANTLRv4Parser.BlockSetContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitBlockSet:" + txt);
			return VisitChildren(context);
		}

		public object VisitBlockSuffix([NotNull] ANTLRv4Parser.BlockSuffixContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitBlockSuffix:" + txt);
			return VisitChildren(context);
		}

		public object VisitChannelsSpec([NotNull] ANTLRv4Parser.ChannelsSpecContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitChannelsSpec:" + txt);
			return VisitChildren(context);
		}

		public object VisitCharacterRange([NotNull] ANTLRv4Parser.CharacterRangeContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitCharacterRange:" + txt);
			return VisitChildren(context);
		}

		public object VisitDelegateGrammar([NotNull] ANTLRv4Parser.DelegateGrammarContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitDelegateGrammar:" + txt);
			return VisitChildren(context);
		}

		public object VisitDelegateGrammars([NotNull] ANTLRv4Parser.DelegateGrammarsContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitDelegateGrammars:" + txt);
			return VisitChildren(context);
		}

		public object VisitEbnf([NotNull] ANTLRv4Parser.EbnfContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitEbnf:" + txt);
			return VisitChildren(context);
		}

		public object VisitEbnfSuffix([NotNull] ANTLRv4Parser.EbnfSuffixContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitEbnfSuffix:" + txt);
			return VisitChildren(context);
		}

		public object VisitElement([NotNull] ANTLRv4Parser.ElementContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitElement:" + txt);
			return VisitChildren(context);
		}

		public object VisitElementOption([NotNull] ANTLRv4Parser.ElementOptionContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitElementOption:" + txt);
			return VisitChildren(context);
		}

		public object VisitElementOptions([NotNull] ANTLRv4Parser.ElementOptionsContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitElementOptions:" + txt);
			return VisitChildren(context);
		}



		public object VisitExceptionGroup([NotNull] ANTLRv4Parser.ExceptionGroupContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitExceptionGroup:" + txt);
			return VisitChildren(context);
		}

		public object VisitExceptionHandler([NotNull] ANTLRv4Parser.ExceptionHandlerContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitExceptionHandler:" + txt);
			return VisitChildren(context);
		}

		public object VisitFinallyClause([NotNull] ANTLRv4Parser.FinallyClauseContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitFinallyClause:" + txt);
			return VisitChildren(context);
		}

		public object VisitGrammarDecl([NotNull] ANTLRv4Parser.GrammarDeclContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitGrammarDecl:" + txt);
			return VisitChildren(context);
		}

		public object VisitGrammarSpec([NotNull] ANTLRv4Parser.GrammarSpecContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitGrammarSpec:" + txt);
			return VisitChildren(context);
		}

		public object VisitGrammarType([NotNull] ANTLRv4Parser.GrammarTypeContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitGrammarType:" + txt);
			return VisitChildren(context);
		}

		public object VisitIdentifier([NotNull] ANTLRv4Parser.IdentifierContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitIdentifier:" + txt);
			return VisitChildren(context);
		}

		public object VisitIdList([NotNull] ANTLRv4Parser.IdListContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitIdList:" + txt);
			return VisitChildren(context);
		}

		public object VisitLabeledAlt([NotNull] ANTLRv4Parser.LabeledAltContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitLabeledAlt:" + txt);
			return VisitChildren(context);
		}

		public object VisitLabeledElement([NotNull] ANTLRv4Parser.LabeledElementContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitLabeledElement:" + txt);
			return VisitChildren(context);
		}

		public object VisitLexerAlt([NotNull] ANTLRv4Parser.LexerAltContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitLexerAlt:" + txt);
			return VisitChildren(context);
		}

		public object VisitLexerAltList([NotNull] ANTLRv4Parser.LexerAltListContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitLexerAltList:" + txt);
			return VisitChildren(context);
		}

		public object VisitLexerAtom([NotNull] ANTLRv4Parser.LexerAtomContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitLexerAtom:" + txt);
			return VisitChildren(context);
		}

		public object VisitLexerBlock([NotNull] ANTLRv4Parser.LexerBlockContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitLexerBlock:" + txt);
			return VisitChildren(context);
		}

		public object VisitLexerCommand([NotNull] ANTLRv4Parser.LexerCommandContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitLexerCommand:" + txt);
			return VisitChildren(context);
		}

		public object VisitLexerCommandExpr([NotNull] ANTLRv4Parser.LexerCommandExprContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitLexerCommandExpr:" + txt);
			return VisitChildren(context);
		}

		public object VisitLexerCommandName([NotNull] ANTLRv4Parser.LexerCommandNameContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitLexerCommandName:" + txt);
			return VisitChildren(context);
		}

		public object VisitLexerCommands([NotNull] ANTLRv4Parser.LexerCommandsContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitLexerCommands:" + txt);
			return VisitChildren(context);
		}

		public object VisitLexerElement([NotNull] ANTLRv4Parser.LexerElementContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitLexerElement:" + txt);
			return VisitChildren(context);
		}

		public object VisitLexerElements([NotNull] ANTLRv4Parser.LexerElementsContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitLexerElements:" + txt);
			return VisitChildren(context);
		}

		public object VisitLexerRuleBlock([NotNull] ANTLRv4Parser.LexerRuleBlockContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitLexerRuleBlock:" + txt);
			return VisitChildren(context);
		}

		public object VisitLexerRuleSpec([NotNull] ANTLRv4Parser.LexerRuleSpecContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitLexerRuleSpec:" + txt);
			return VisitChildren(context);
		}

		public object VisitLocalsSpec([NotNull] ANTLRv4Parser.LocalsSpecContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitLocalsSpec:" + txt);
			return VisitChildren(context);
		}

		public object VisitModeSpec([NotNull] ANTLRv4Parser.ModeSpecContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitModeSpec:" + txt);
			return VisitChildren(context);
		}

		public object VisitNotSet([NotNull] ANTLRv4Parser.NotSetContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitNotSet:" + txt);
			return VisitChildren(context);
		}

		public object VisitOption([NotNull] ANTLRv4Parser.OptionContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitOption:" + txt);
			return VisitChildren(context);
		}

		public object VisitOptionsSpec([NotNull] ANTLRv4Parser.OptionsSpecContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitOptionsSpec:" + txt);
			return VisitChildren(context);
		}

		public object VisitOptionValue([NotNull] ANTLRv4Parser.OptionValueContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitOptionValue:" + txt);
			return VisitChildren(context);
		}

		public object VisitParserRuleSpec([NotNull] ANTLRv4Parser.ParserRuleSpecContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitParserRuleSpec:" + txt);
			return VisitChildren(context);
		}

		public object VisitPredicateOption([NotNull] ANTLRv4Parser.PredicateOptionContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitPredicateOption:" + txt);
			return VisitChildren(context);
		}

		public object VisitPredicateOptions([NotNull] ANTLRv4Parser.PredicateOptionsContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitPredicateOptions:" + txt);
			return VisitChildren(context);
		}

		public object VisitPrequelConstruct([NotNull] ANTLRv4Parser.PrequelConstructContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitPrequelConstruct:" + txt);
			return VisitChildren(context);
		}

		public object VisitQualifiedIdentifier([NotNull] ANTLRv4Parser.QualifiedIdentifierContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitQualifiedIdentifier:" + txt);
			return VisitChildren(context);
		}

		public object VisitRuleAction([NotNull] ANTLRv4Parser.RuleActionContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitRuleAction:" + txt);
			return VisitChildren(context);
		}

		public object VisitRuleAltList([NotNull] ANTLRv4Parser.RuleAltListContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitRuleAltList:" + txt);
			return VisitChildren(context);
		}

		public object VisitRuleBlock([NotNull] ANTLRv4Parser.RuleBlockContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitRuleBlock:" + txt);
			return VisitChildren(context);
		}

		public object VisitRuleModifier([NotNull] ANTLRv4Parser.RuleModifierContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitRuleModifier:" + txt);
			return VisitChildren(context);
		}

		public object VisitRuleModifiers([NotNull] ANTLRv4Parser.RuleModifiersContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitRuleModifiers:" + txt);
			return VisitChildren(context);
		}

		public object VisitRulePrequel([NotNull] ANTLRv4Parser.RulePrequelContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitRulePrequel:" + txt);
			return VisitChildren(context);
		}

		public object VisitRuleref([NotNull] ANTLRv4Parser.RulerefContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitRuleref:" + txt);
			return VisitChildren(context);
		}

		public object VisitRuleReturns([NotNull] ANTLRv4Parser.RuleReturnsContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitRuleReturns:" + txt);
			return VisitChildren(context);
		}

		public object VisitRules([NotNull] ANTLRv4Parser.RulesContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitRules:" + txt);
			return VisitChildren(context);
		}

		public object VisitRuleSpec([NotNull] ANTLRv4Parser.RuleSpecContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitRuleSpec:" + txt);
			return VisitChildren(context);
		}

		public object VisitSetElement([NotNull] ANTLRv4Parser.SetElementContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitSetElement:" + txt);
			return VisitChildren(context);
		}


		public object VisitTerminalDef([NotNull] ANTLRv4Parser.TerminalDefContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitTerminalDef:" + txt);
			return VisitChildren(context);
		}

		public object VisitThrowsSpec([NotNull] ANTLRv4Parser.ThrowsSpecContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitThrowsSpec:" + txt);
			return VisitChildren(context);
		}

		public object VisitTokensSpec([NotNull] ANTLRv4Parser.TokensSpecContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitTokensSpec:" + txt);
			return VisitChildren(context);
		}

		public object VisitWildcard([NotNull] ANTLRv4Parser.WildcardContext context)
		{
			var txt = context.GetText(); Console.WriteLine("VisitWildcard:" + txt);
			return VisitChildren(context);
		}

	}
}
