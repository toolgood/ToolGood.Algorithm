/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using System;
using System.Collections.Generic;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Sharpen;
namespace Antlr4.Runtime.Dfa
{
	internal class DFA
	{
		public Dictionary<DFAState, DFAState> states = new Dictionary<DFAState, DFAState>();
		public DFAState s0;
		public int decision;
		/** From which ATN state did we create this DFA? */
		public DecisionState atnStartState;
		private bool precedenceDfa;
		public DFA(DecisionState atnStartState)
			: this(atnStartState, 0)
		{
		}
		public DFA(DecisionState atnStartState, int decision)
		{
			this.atnStartState = atnStartState;
			this.decision = decision;
			precedenceDfa = false;
			if (atnStartState is StarLoopEntryState state && state.isPrecedenceDecision)
			{
				this.precedenceDfa = true;
				DFAState precedenceState = new DFAState(new ATNConfigSet());
				precedenceState.edges = new DFAState[0];
				precedenceState.isAcceptState = false;
				precedenceState.requiresFullContext = false;
				this.s0 = precedenceState;
			}
		}
		public bool IsPrecedenceDfa
		{
			get
			{
				return precedenceDfa;
			}
		}
		public DFAState GetPrecedenceStartState(int precedence)
		{
			if (!IsPrecedenceDfa)
			{
				throw new Exception("Only precedence DFAs may contain a precedence start state.");
			}
			if (precedence < 0 || precedence >= s0.edges.Length)
			{
				return null;
			}
			return s0.edges[precedence];
		}
		public void SetPrecedenceStartState(int precedence, DFAState startState)
		{
			if (!IsPrecedenceDfa)
			{
				throw new Exception("Only precedence DFAs may contain a precedence start state.");
			}
			if (precedence < 0)
			{
				return;
			}
			lock (s0)
			{
				if (precedence >= s0.edges.Length)
				{
					s0.edges = Arrays.CopyOf(s0.edges, precedence + 1);
				}
				s0.edges[precedence] = startState;
			}
		}
	}
}
