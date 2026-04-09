/*
/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using System;
using Antlr4.Runtime.Dfa;
using Antlr4.Runtime.Misc;
namespace Antlr4.Runtime.Atn
{
	public class LexerATNSimulator : ATNSimulator
	{
		public static readonly int MIN_DFA_EDGE = 0;
		public static readonly int MAX_DFA_EDGE = 127; 
		protected readonly Lexer recog;
		/** The current token's starting index into the character stream.
		 *  Shared across DFA to ATN simulation in case the ATN fails and the
		 *  DFA did not have a previous accept state. In this case, we use the
		 *  ATN-generated exception object.
		 */
		protected int startIndex = -1;
		/** line number 1..n within the input */
		protected int thisLine = 1;
		/** The index of the character relative to the beginning of the line 0..n-1 */
		protected int charPositionInLine = 0;
		public readonly DFA[] decisionToDFA;
		protected int mode = Lexer.DEFAULT_MODE;
		/** Used during DFA/ATN exec to record the most recent accept configuration info */
		readonly SimState prevAccept = new SimState();
		public LexerATNSimulator(Lexer recog, ATN atn,
								 DFA[] decisionToDFA,
								 PredictionContextCache sharedContextCache)
			: base(atn, sharedContextCache)
		{
			this.decisionToDFA = decisionToDFA;
			this.recog = recog;
		}
		public int Match(ICharStream input, int mode)
		{
			this.mode = mode;
			int mark = input.Mark();
			try
			{
				this.startIndex = input.Index;
				this.prevAccept.Reset();
				DFA dfa = decisionToDFA[mode];
				if (dfa.s0 == null)
				{
					return MatchATN(input);
				}
				else
				{
					return ExecATN(input, dfa.s0);
				}
			}
			finally
			{
				input.Release(mark);
			}
		}
		public override void Reset()
		{
			prevAccept.Reset();
			startIndex = -1;
			thisLine = 1;
			charPositionInLine = 0;
			mode = Lexer.DEFAULT_MODE;
		}
		public override void ClearDFA()
		{
			for (int d = 0; d < decisionToDFA.Length; d++)
			{
				decisionToDFA[d] = new DFA(atn.GetDecisionState(d), d);
			}
		}
		protected int MatchATN(ICharStream input)
		{
			ATNState startState = atn.modeToStartState[mode];
            int old_mode = mode;
			ATNConfigSet s0_closure = ComputeStartState(input, startState);
			bool suppressEdge = s0_closure.hasSemanticContext;
			s0_closure.hasSemanticContext = false;
			DFAState next = AddDFAState(s0_closure);
			if (!suppressEdge)
			{
				decisionToDFA[mode].s0 = next;
			}
			int predict = ExecATN(input, next);
            return predict;
		}
		protected int ExecATN(ICharStream input, DFAState ds0)
		{
            if (ds0.isAcceptState)
			{
				CaptureSimState(prevAccept, input, ds0);
			}
			int t = input.LA(1);
			DFAState s = ds0; 
			while (true)
			{ 
                DFAState target = GetExistingTargetState(s, t);
				if (target == null)
				{
					target = ComputeTargetState(input, s, t);
				}
				if (target == ERROR)
				{
					break;
				}
				if (t != IntStreamConstants.EOF)
				{
					Consume(input);
				}
				if (target.isAcceptState)
				{
					CaptureSimState(prevAccept, input, target);
					if (t == IntStreamConstants.EOF)
					{
						break;
					}
				}
				t = input.LA(1);
				s = target; 
			}
			return FailOrAccept(prevAccept, input, s.configSet, t);
		}
		/**
		 * Get an existing target state for an edge in the DFA. If the target state
		 * for the edge has not yet been computed or is otherwise not available,
		 * this method returns {@code null}.
		 *
		 * @param s The current DFA state
		 * @param t The next input symbol
		 * @return The existing target DFA state for the given input symbol
		 * {@code t}, or {@code null} if the target state for this edge is not
		 * already cached
		 */
		protected DFAState GetExistingTargetState(DFAState s, int t)
		{
			if (s.edges == null || t < MIN_DFA_EDGE || t > MAX_DFA_EDGE)
			{
				return null;
			}
			DFAState target = s.edges[t - MIN_DFA_EDGE];
			return target;
		}
		/**
		 * Compute a target state for an edge in the DFA, and attempt to add the
		 * computed state and corresponding edge to the DFA.
		 *
		 * @param input The input stream
		 * @param s The current DFA state
		 * @param t The next input symbol
		 *
		 * @return The computed target DFA state for the given input symbol
		 * {@code t}. If {@code t} does not lead to a valid DFA state, this method
		 * returns {@link #ERROR}.
		 */
		protected DFAState ComputeTargetState(ICharStream input, DFAState s, int t)
		{
			ATNConfigSet reach = new OrderedATNConfigSet();
			GetReachableConfigSet(input, s.configSet, reach, t);
			if (reach.Empty)
			{ 
				if (!reach.hasSemanticContext)
				{
					AddDFAEdge(s, t, ERROR);
				}
				return ERROR;
			}
			return AddDFAEdge(s, t, reach);
		}
		protected int FailOrAccept(SimState prevAccept, ICharStream input,
								   ATNConfigSet reach, int t)
		{
			if (prevAccept.dfaState != null)
			{
				LexerActionExecutor lexerActionExecutor = prevAccept.dfaState.lexerActionExecutor;
				Accept(input, lexerActionExecutor, startIndex,
					prevAccept.index, prevAccept.line, prevAccept.charPos);
				return prevAccept.dfaState.prediction;
			}
			else {
				if (t == IntStreamConstants.EOF && input.Index == startIndex)
				{
					return TokenConstants.EOF;
				}
				throw new LexerNoViableAltException(recog, input, startIndex, reach);
			}
		}
		/** Given a starting configuration set, figure out all ATN configurations
		 *  we can reach upon input {@code t}. Parameter {@code reach} is a return
		 *  parameter.
		 */
		protected void GetReachableConfigSet(ICharStream input, ATNConfigSet closure, ATNConfigSet reach, int t)
		{
			int skipAlt = ATN.INVALID_ALT_NUMBER;
			foreach (ATNConfig c in closure.configs)
			{
				bool currentAltReachedAcceptState = c.alt == skipAlt;
				if (currentAltReachedAcceptState && ((LexerATNConfig)c).hasPassedThroughNonGreedyDecision())
				{
					continue;
				}
				int n = c.state.NumberOfTransitions;
				for (int ti = 0; ti < n; ti++)
				{               
					Transition trans = c.state.Transition(ti);
					ATNState target = GetReachableTarget(trans, t);
					if (target != null)
					{
						LexerActionExecutor lexerActionExecutor = ((LexerATNConfig)c).getLexerActionExecutor();
						if (lexerActionExecutor != null)
						{
							lexerActionExecutor = lexerActionExecutor.FixOffsetBeforeMatch(input.Index - startIndex);
						}
						bool treatEofAsEpsilon = t == IntStreamConstants.EOF;
						if (Closure(input, new LexerATNConfig((LexerATNConfig)c, target, lexerActionExecutor), reach, currentAltReachedAcceptState, true, treatEofAsEpsilon))
						{
							skipAlt = c.alt;
							break;
						}
					}
				}
			}
		}
		protected void Accept(ICharStream input, LexerActionExecutor lexerActionExecutor,
							  int startIndex, int index, int line, int charPos)
		{
			input.Seek(index);
			this.thisLine = line;
			this.charPositionInLine = charPos;
			if (lexerActionExecutor != null && recog != null)
			{
				lexerActionExecutor.Execute(recog, input, startIndex);
			}
		}
		protected ATNState GetReachableTarget(Transition trans, int t)
		{
			if (trans.Matches(t, Lexer.MinCharValue, Lexer.MaxCharValue))
			{
				return trans.target;
			}
			return null;
		}
		protected ATNConfigSet ComputeStartState(ICharStream input,
												 ATNState p)
		{
			PredictionContext initialContext = EmptyPredictionContext.Instance;
			ATNConfigSet configs = new OrderedATNConfigSet();
			for (int i = 0; i < p.NumberOfTransitions; i++)
			{
				ATNState target = p.Transition(i).target;
				LexerATNConfig c = new LexerATNConfig(target, i + 1, initialContext);
				Closure(input, c, configs, false, false, false);
			}
			return configs;
		}
		/**
		 * Since the alternatives within any lexer decision are ordered by
		 * preference, this method stops pursuing the closure as soon as an accept
		 * state is reached. After the first accept state is reached by depth-first
		 * search from {@code config}, all other (potentially reachable) states for
		 * this rule would have a lower priority.
		 *
		 * @return {@code true} if an accept state is reached, otherwise
		 * {@code false}.
		 */
		protected bool Closure(ICharStream input, LexerATNConfig config, ATNConfigSet configs, bool currentAltReachedAcceptState, bool speculative, bool treatEofAsEpsilon)
		{
			if (config.state is RuleStopState)
			{
				if (config.context == null || config.context.HasEmptyPath)
				{
					if (config.context == null || config.context.IsEmpty)
					{
						configs.Add(config);
						return true;
					}
					else {
						configs.Add(new LexerATNConfig(config, config.state, EmptyPredictionContext.Instance));
						currentAltReachedAcceptState = true;
					}
				}
				if (config.context != null && !config.context.IsEmpty)
				{
					for (int i = 0; i < config.context.Size; i++)
					{
						if (config.context.GetReturnState(i) != PredictionContext.EMPTY_RETURN_STATE)
						{
							PredictionContext newContext = config.context.GetParent(i); 
							ATNState returnState = atn.states[config.context.GetReturnState(i)];
							LexerATNConfig c = new LexerATNConfig(config, returnState, newContext);
							currentAltReachedAcceptState = Closure(input, c, configs, currentAltReachedAcceptState, speculative, treatEofAsEpsilon);
						}
					}
				}
				return currentAltReachedAcceptState;
			}
			if (!config.state.OnlyHasEpsilonTransitions)
			{
				if (!currentAltReachedAcceptState || !config.hasPassedThroughNonGreedyDecision())
				{
					configs.Add(config);
				}
			}
			ATNState p = config.state;
			for (int i = 0; i < p.NumberOfTransitions; i++)
			{
				Transition t = p.Transition(i);
				LexerATNConfig c = GetEpsilonTarget(input, config, t, configs, speculative, treatEofAsEpsilon);
				if (c != null)
				{
					currentAltReachedAcceptState = Closure(input, c, configs, currentAltReachedAcceptState, speculative, treatEofAsEpsilon);
				}
			}
			return currentAltReachedAcceptState;
		}
		protected LexerATNConfig GetEpsilonTarget(ICharStream input,
											   LexerATNConfig config,
											   Transition t,
											   ATNConfigSet configs,
											   bool speculative,
											   bool treatEofAsEpsilon)
		{
			LexerATNConfig c = null;
			switch (t.TransitionType)
			{
				case TransitionType.RULE:
					RuleTransition ruleTransition = (RuleTransition)t;
					PredictionContext newContext = new SingletonPredictionContext(config.context, ruleTransition.followState.stateNumber);
					c = new LexerATNConfig(config, t.target, newContext);
					break;
				case TransitionType.PRECEDENCE:
					throw new Exception("Precedence predicates are not supported in lexers.");
				case TransitionType.PREDICATE:
					/*  Track traversing semantic predicates. If we traverse,
					 we cannot add a DFA state for this "reach" computation
					 because the DFA would not test the predicate again in the
					 future. Rather than creating collections of semantic predicates
					 like v3 and testing them on prediction, v4 will test them on the
					 fly all the time using the ATN not the DFA. This is slower but
					 semantically it's not used that often. One of the key elements to
					 this predicate mechanism is not adding DFA states that see
					 predicates immediately afterwards in the ATN. For example,
					 a : ID {p1}? | ID {p2}? ;
					 should create the start state for rule 'a' (to save start state
					 competition), but should not create target of ID state. The
					 collection of ATN states the following ID references includes
					 states reached by traversing predicates. Since this is when we
					 test them, we cannot cash the DFA state target of ID.
				 */
					PredicateTransition pt = (PredicateTransition)t;
					configs.hasSemanticContext = true;
					if (EvaluatePredicate(input, pt.ruleIndex, pt.predIndex, speculative))
					{
						c = new LexerATNConfig(config, t.target);
					}
					break;
				case TransitionType.ACTION:
					if (config.context == null || config.context.HasEmptyPath)
					{
						LexerActionExecutor lexerActionExecutor = LexerActionExecutor.Append(config.getLexerActionExecutor(), atn.lexerActions[((ActionTransition)t).actionIndex]);
						c = new LexerATNConfig(config, t.target, lexerActionExecutor);
						break;
					}
					else {
						c = new LexerATNConfig(config, t.target);
						break;
					}
				case TransitionType.EPSILON:
					c = new LexerATNConfig(config, t.target);
					break;
				case TransitionType.ATOM:
				case TransitionType.RANGE:
				case TransitionType.SET:
					if (treatEofAsEpsilon)
					{
						if (t.Matches(IntStreamConstants.EOF, Lexer.MinCharValue, Lexer.MaxCharValue))
						{
							c = new LexerATNConfig(config, t.target);
							break;
						}
					}
					break;
			}
			return c;
		}
		/**
		 * Evaluate a predicate specified in the lexer.
		 *
		 * <p>If {@code speculative} is {@code true}, this method was called before
		 * {@link #consume} for the matched character. This method should call
		 * {@link #consume} before evaluating the predicate to ensure position
		 * sensitive values, including {@link Lexer#getText}, {@link Lexer#getLine},
		 * and {@link Lexer#getCharPositionInLine}, properly reflect the current
		 * lexer state. This method should restore {@code input} and the simulator
		 * to the original state before returning (i.e. undo the actions made by the
		 * call to {@link #consume}.</p>
		 *
		 * @param input The input stream.
		 * @param ruleIndex The rule containing the predicate.
		 * @param predIndex The index of the predicate within the rule.
		 * @param speculative {@code true} if the current index in {@code input} is
		 * one character before the predicate's location.
		 *
		 * @return {@code true} if the specified predicate evaluates to
		 * {@code true}.
		 */
		protected bool EvaluatePredicate(ICharStream input, int ruleIndex, int predIndex, bool speculative)
		{
			if (recog == null)
			{
				return true;
			}
			if (!speculative)
			{
				return recog.Sempred(null, ruleIndex, predIndex);
			}
			int savedCharPositionInLine = charPositionInLine;
			int savedLine = thisLine;
			int index = input.Index;
			int marker = input.Mark();
			try
			{
				Consume(input);
				return recog.Sempred(null, ruleIndex, predIndex);
			}
			finally
			{
				charPositionInLine = savedCharPositionInLine;
				thisLine = savedLine;
				input.Seek(index);
				input.Release(marker);
			}
		}
		protected void CaptureSimState(SimState settings,
									   ICharStream input,
									   DFAState dfaState)
		{
			settings.index = input.Index;
			settings.line = thisLine;
			settings.charPos = charPositionInLine;
			settings.dfaState = dfaState;
		}
		protected DFAState AddDFAEdge(DFAState from,
									  int t,
									  ATNConfigSet q)
		{
			/* leading to this call, ATNConfigSet.hasSemanticContext is used as a
			 * marker indicating dynamic predicate evaluation makes this edge
			 * dependent on the specific input sequence, so the static edge in the
			 * DFA should be omitted. The target DFAState is still created since
			 * execATN has the ability to resynchronize with the DFA state cache
			 * following the predicate evaluation step.
			 *
			 * TJP notes: next time through the DFA, we see a pred again and eval.
			 * If that gets us to a previously created (but dangling) DFA
			 * state, we can continue in pure DFA mode from there.
			 */
			bool suppressEdge = q.hasSemanticContext;
			q.hasSemanticContext = false;
			DFAState to = AddDFAState(q);
			if (suppressEdge)
			{
				return to;
			}
			AddDFAEdge(from, t, to);
			return to;
		}
		protected void AddDFAEdge(DFAState p, int t, DFAState q)
		{
			if (t < MIN_DFA_EDGE || t > MAX_DFA_EDGE)
			{
				return;
			}
			lock (p)
			{
				if (p.edges == null)
				{
					p.edges = new DFAState[MAX_DFA_EDGE - MIN_DFA_EDGE + 1];
				}
				p.edges[t - MIN_DFA_EDGE] = q; 
			}
		}
		/** Add a new DFA state if there isn't one with this set of
			configurations already. This method also detects the first
			configuration containing an ATN rule stop state. Later, when
			traversing the DFA, we will know which rule to accept.
		 */
		protected DFAState AddDFAState(ATNConfigSet configSet)
		{
			/* the lexer evaluates predicates on-the-fly; by this point configs
			 * should not contain any configurations with unevaluated predicates.
			 */
			DFAState proposed = new DFAState(configSet);
			ATNConfig firstConfigWithRuleStopState = null;
			foreach (ATNConfig c in configSet.configs)
			{
				if (c.state is RuleStopState)
				{
					firstConfigWithRuleStopState = c;
					break;
				}
			}
			if (firstConfigWithRuleStopState != null)
			{
				proposed.isAcceptState = true;
				proposed.lexerActionExecutor = ((LexerATNConfig)firstConfigWithRuleStopState).getLexerActionExecutor();
				proposed.prediction = atn.ruleToTokenType[firstConfigWithRuleStopState.state.ruleIndex];
			}
			DFA dfa = decisionToDFA[mode];
			lock (dfa.states)
			{
				DFAState existing;
				if(dfa.states.TryGetValue(proposed, out existing))
					return existing;
				DFAState newState = proposed;
				newState.stateNumber = dfa.states.Count;
				configSet.IsReadOnly = true;
				newState.configSet = configSet;
				dfa.states[newState] = newState;
				return newState;
			}
		}
		public int Line
		{
			get
			{
				return thisLine;
			}
			set
			{
				this.thisLine = value;
			}
		}
		public int Column
		{
			get
			{
				return charPositionInLine;
			}
			set
			{
				this.charPositionInLine = value;
			}
		}
		public virtual void Consume(ICharStream input)
		{
			int curChar = input.LA(1);
			if (curChar == '\n')
			{
				thisLine++;
				charPositionInLine = 0;
			}
			else {
				charPositionInLine++;
			}
			input.Consume();
		}
	}
	/** When we hit an accept state in either the DFA or the ATN, we
 *  have to notify the character stream to start buffering characters
 *  via {@link IntStream#mark} and record the current state. The current sim state
 *  includes the current index into the input, the current line,
 *  and current character position in that line. Note that the Lexer is
 *  tracking the starting line and characterization of the token. These
 *  variables track the "state" of the simulator when it hits an accept state.
 *
 *  <p>We track these variables separately for the DFA and ATN simulation
 *  because the DFA simulation often has to fail over to the ATN
 *  simulation. If the ATN simulation fails, we need the DFA to fall
 *  back to its previously accepted state, if any. If the ATN succeeds,
 *  then the ATN does the accept and the DFA simulator that invoked it
 *  can simply return the predicted token type.</p>
 */
	public class SimState
	{
		public int index = -1;
		public int line = 0;
		public int charPos = -1;
		public DFAState dfaState;
		public void Reset()
		{
			index = -1;
			line = 0;
			charPos = -1;
			dfaState = null;
		}
	}
}
