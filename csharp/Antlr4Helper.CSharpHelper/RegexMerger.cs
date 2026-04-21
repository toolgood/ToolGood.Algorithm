using Antlr4Helper.CSharpHelper.DFAs;
using Antlr4Helper.CSharpHelper.Regexs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Antlr4Helper.CSharpHelper.RegexEngine
{
	public sealed class RegexMerger
	{
		private readonly RegexParser _parser;
		private readonly NFABuilder _nfaBuilder;
		private readonly NFAToDFAConverter _converter;
		private readonly DFAMinimizer _minimizer;

		public RegexMerger()
		{
			_parser = new RegexParser();
			_nfaBuilder = new NFABuilder();
			_converter = new NFAToDFAConverter();
			_minimizer = new DFAMinimizer();
		}

		public DFA MergePatterns(List<string> patterns, char[] dict = null)
		{
			if(patterns == null || patterns.Count == 0)
				throw new ArgumentException("模式列表不能为空", nameof(patterns));
			_nfaBuilder.SetDict(dict);

			var nfas = new List<NFA>();

			for(int i = 0; i < patterns.Count; i++) {
				try {
					var ast = _parser.Parse(patterns[i]);
					var nfa = _nfaBuilder.Build(ast, i + 1);
					nfas.Add(nfa);
				} catch(RegexParseException ex) {
					throw new RegexParseException($"模式 {i} 解析失败: {ex.Message}", ex.Position);
				}
			}

			var dfa = _converter.ConvertMultiple(nfas);
			var minimizedDfa = _minimizer.Minimize(dfa);

			return minimizedDfa;
		}

		public OptimizedDFA MergePatternsOptimized(List<string> patterns)
		{
			var dfa = MergePatterns(patterns);
			return new OptimizedDFA(dfa);
		}

		public DFA BuildSinglePattern(string pattern)
		{
			if(string.IsNullOrEmpty(pattern))
				throw new ArgumentException("模式不能为空", nameof(pattern));

			var ast = _parser.Parse(pattern);
			var nfa = _nfaBuilder.Build(ast, 0);
			var dfa = _converter.Convert(nfa);
			var minimizedDfa = _minimizer.Minimize(dfa);

			return minimizedDfa;
		}

		public OptimizedDFA BuildSinglePatternOptimized(string pattern)
		{
			var dfa = BuildSinglePattern(pattern);
			return new OptimizedDFA(dfa);
		}

		public List<RegexParseResult> ValidatePatterns(List<string> patterns)
		{
			var results = new List<RegexParseResult>();

			for(int i = 0; i < patterns.Count; i++) {
				try {
					var ast = _parser.Parse(patterns[i]);
					results.Add(new RegexParseResult(i, true, null, ast));
				} catch(RegexParseException ex) {
					results.Add(new RegexParseResult(i, false, ex.Message, null));
				}
			}

			return results;
		}
	}

	public sealed class RegexParseResult
	{
		public int PatternIndex { get; }
		public bool IsValid { get; }
		public string ErrorMessage { get; }
		public RegexNode Ast { get; }

		public RegexParseResult(int patternIndex, bool isValid, string errorMessage, RegexNode ast)
		{
			PatternIndex = patternIndex;
			IsValid = isValid;
			ErrorMessage = errorMessage;
			Ast = ast;
		}
	}
}
