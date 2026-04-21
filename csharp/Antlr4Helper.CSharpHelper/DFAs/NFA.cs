using Antlr4Helper.CSharpHelper.Regexs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Antlr4Helper.CSharpHelper.DFAs
{
	public sealed class NFAState
	{
		public int Id { get; }
		public bool IsAccept { get; set; }
		public int AcceptId { get; set; }
		public Dictionary<char, List<NFAState>> Transitions { get; }
		public List<NFAState> EpsilonTransitions { get; }

		public NFAState(int id)
		{
			Id = id;
			IsAccept = false;
			AcceptId = -1;
			Transitions = new Dictionary<char, List<NFAState>>();
			EpsilonTransitions = new List<NFAState>();
		}

		public void AddTransition(char c, NFAState target)
		{
			if(!Transitions.ContainsKey(c))
				Transitions[c] = new List<NFAState>();
			Transitions[c].Add(target);
		}

		public void AddEpsilonTransition(NFAState target)
		{
			EpsilonTransitions.Add(target);
		}

		public override string ToString() => IsAccept ? $"State{Id}(Accept:{AcceptId})" : $"State{Id}";
	}

	public sealed class NFAFragment
	{
		public NFAState Start { get; }
		public NFAState End { get; }

		public NFAFragment(NFAState start, NFAState end)
		{
			Start = start;
			End = end;
		}
	}

	public sealed class NFA
	{
		public NFAState StartState { get; private set; }
		public List<NFAState> States { get; }
		public int PatternId { get; }

		public NFA(NFAState startState, List<NFAState> states, int patternId)
		{
			StartState = startState;
			States = states;
			PatternId = patternId;
		}

		public HashSet<NFAState> GetEpsilonClosure(NFAState state)
		{
			var closure = new HashSet<NFAState>();
			var stack = new Stack<NFAState>();
			stack.Push(state);

			while(stack.Count > 0) {
				var current = stack.Pop();
				if(closure.Add(current)) {
					foreach(var next in current.EpsilonTransitions) {
						if(!closure.Contains(next))
							stack.Push(next);
					}
				}
			}

			return closure;
		}

		public HashSet<NFAState> GetEpsilonClosure(IEnumerable<NFAState> states)
		{
			var closure = new HashSet<NFAState>();
			foreach(var state in states) {
				foreach(var s in GetEpsilonClosure(state))
					closure.Add(s);
			}
			return closure;
		}

		public HashSet<NFAState> Move(IEnumerable<NFAState> states, char c)
		{
			var result = new HashSet<NFAState>();
			foreach(var state in states) {
				if(state.Transitions.TryGetValue(c, out var targets)) {
					foreach(var target in targets)
						result.Add(target);
				}
			}
			return result;
		}

		public Dictionary<char, HashSet<char>> GetAlphabet()
		{
			var alphabet = new HashSet<char>();
			foreach(var state in States) {
				foreach(var c in state.Transitions.Keys)
					alphabet.Add(c);
			}

			var ranges = new Dictionary<char, HashSet<char>>();
			foreach(var c in alphabet) {
				if(!ranges.ContainsKey(c))
					ranges[c] = new HashSet<char> { c };
			}

			return ranges;
		}
	}

	public sealed class NFABuilder : IRegexNodeVisitor
	{
		private int _stateId;
		private NFAFragment _result;
		private readonly Stack<NFAFragment> _fragmentStack;
		private int _patternId;
		private char[] _dict;

		public NFABuilder()
		{
			_stateId = 0;
			_fragmentStack = new Stack<NFAFragment>();
		}

		public void SetDict(char[] dict)
		{
			_dict = dict;
		}

		public NFA Build(RegexNode ast, int patternId = 0)
		{
			_stateId = 0;
			_patternId = patternId;
			_fragmentStack.Clear();

			ast.Accept(this);
			_result = _fragmentStack.Pop();

			_result.End.IsAccept = true;
			_result.End.AcceptId = patternId;

			var states = CollectStates(_result.Start);
			return new NFA(_result.Start, states, patternId);
		}

		private NFAState NewState() => new NFAState(_stateId++);

		private List<NFAState> CollectStates(NFAState start)
		{
			var states = new List<NFAState>();
			var visited = new HashSet<NFAState>();
			var stack = new Stack<NFAState>();
			stack.Push(start);

			while(stack.Count > 0) {
				var current = stack.Pop();
				if(visited.Add(current)) {
					states.Add(current);

					foreach(var targets in current.Transitions.Values) {
						foreach(var target in targets) {
							if(!visited.Contains(target))
								stack.Push(target);
						}
					}

					foreach(var target in current.EpsilonTransitions) {
						if(!visited.Contains(target))
							stack.Push(target);
					}
				}
			}

			return states;
		}

		public void Visit(CharNode node)
		{
			var start = NewState();
			var end = NewState();
			if(_dict == null) {
				start.AddTransition(node.Value, end);
			} else {
				start.AddTransition(_dict[node.Value], end);
			}
			_fragmentStack.Push(new NFAFragment(start, end));
		}

		public void Visit(CharClassNode node)
		{
			var start = NewState();
			var end = NewState();

			HashSet<char> charSet = new HashSet<char>();
			if(node.Negated) {
				var excludedChars = new HashSet<char>();
				foreach(var range in node.Ranges) {
					for(var c = range.Min; c <= range.Max; c++)
						excludedChars.Add(c);
				}

				for(int c = 0; c <= char.MaxValue; c++) {
					if(!excludedChars.Contains((char)c)) {
						var ch = (char)c;
						if(_dict != null) {
							ch = _dict[ch];
						}
						if(charSet.Add(ch)) {
							start.AddTransition((char)ch, end);
						}
					}
				}
			} else {
				foreach(var range in node.Ranges) {
					for(var c = range.Min; c <= range.Max; c++) {
						var ch = (char)c;
						if(_dict != null) {
							ch = _dict[ch];
						}
						if(charSet.Add(ch)) {
							start.AddTransition((char)ch, end);
						}
					}
				}
			}

			_fragmentStack.Push(new NFAFragment(start, end));
		}

		public void Visit(AnyCharNode node)
		{
			var start = NewState();
			var end = NewState();
			HashSet<char> charSet = new HashSet<char>();
			for(int c = 0; c <= char.MaxValue; c++) {
				var ch = (char)c;
				if(_dict != null) {
					ch = _dict[ch];
				}
				if(charSet.Add(ch)) {
					start.AddTransition((char)ch, end);
				}
			}
			_fragmentStack.Push(new NFAFragment(start, end));
		}

		public void Visit(ConcatNode node)
		{
			if(node.Children.Count == 0) {
				var start = NewState();
				var end = NewState();
				start.AddEpsilonTransition(end);
				_fragmentStack.Push(new NFAFragment(start, end));
				return;
			}

			foreach(var child in node.Children)
				child.Accept(this);

			var fragments = new List<NFAFragment>();
			for(int i = 0; i < node.Children.Count; i++)
				fragments.Insert(0, _fragmentStack.Pop());

			for(int i = 0; i < fragments.Count - 1; i++) {
				fragments[i].End.AddEpsilonTransition(fragments[i + 1].Start);
			}

			_fragmentStack.Push(new NFAFragment(fragments[0].Start, fragments[fragments.Count - 1].End));
		}

		public void Visit(AlternationNode node)
		{
			var start = NewState();
			var end = NewState();

			foreach(var alt in node.Alternatives) {
				alt.Accept(this);
				var fragment = _fragmentStack.Pop();
				start.AddEpsilonTransition(fragment.Start);
				fragment.End.AddEpsilonTransition(end);
			}

			_fragmentStack.Push(new NFAFragment(start, end));
		}

		public void Visit(QuantifierNode node)
		{
			var start = NewState();
			var end = NewState();

			if(node.MinCount == 0 && node.MaxCount == null) {
				node.Child.Accept(this);
				var fragment = _fragmentStack.Pop();
				start.AddEpsilonTransition(fragment.Start);
				start.AddEpsilonTransition(end);
				fragment.End.AddEpsilonTransition(fragment.Start);
				fragment.End.AddEpsilonTransition(end);
			} else if(node.MinCount == 1 && node.MaxCount == null) {
				node.Child.Accept(this);
				var fragment = _fragmentStack.Pop();
				start.AddEpsilonTransition(fragment.Start);
				fragment.End.AddEpsilonTransition(fragment.Start);
				fragment.End.AddEpsilonTransition(end);
			} else if(node.MinCount == 0 && node.MaxCount == 1) {
				node.Child.Accept(this);
				var fragment = _fragmentStack.Pop();
				start.AddEpsilonTransition(fragment.Start);
				start.AddEpsilonTransition(end);
				fragment.End.AddEpsilonTransition(end);
			} else {
				var currentStart = start;

				for(int i = 0; i < node.MinCount; i++) {
					node.Child.Accept(this);
					var fragment = _fragmentStack.Pop();
					currentStart.AddEpsilonTransition(fragment.Start);
					currentStart = fragment.End;
				}

				if(node.MaxCount == null) {
					node.Child.Accept(this);
					var fragment = _fragmentStack.Pop();
					currentStart.AddEpsilonTransition(fragment.Start);
					fragment.End.AddEpsilonTransition(fragment.Start);
					fragment.End.AddEpsilonTransition(end);
					currentStart.AddEpsilonTransition(end);
				} else {
					var optionalCount = node.MaxCount.Value - node.MinCount;
					for(int i = 0; i < optionalCount; i++) {
						currentStart.AddEpsilonTransition(end);
						node.Child.Accept(this);
						var fragment = _fragmentStack.Pop();
						currentStart.AddEpsilonTransition(fragment.Start);
						currentStart = fragment.End;
					}
					currentStart.AddEpsilonTransition(end);
				}
			}

			_fragmentStack.Push(new NFAFragment(start, end));
		}

		public void Visit(GroupNode node)
		{
			node.Child.Accept(this);
		}

		public void Visit(AnchorNode node)
		{
			var start = NewState();
			var end = NewState();
			start.AddEpsilonTransition(end);
			_fragmentStack.Push(new NFAFragment(start, end));
		}

		public void Visit(EmptyNode node)
		{
			var start = NewState();
			var end = NewState();
			start.AddEpsilonTransition(end);
			_fragmentStack.Push(new NFAFragment(start, end));
		}
	}
}
