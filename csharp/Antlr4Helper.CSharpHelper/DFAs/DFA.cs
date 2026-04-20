using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Antlr4Helper.CSharpHelper.DFAs
{
    public sealed class DFAState
    {
        public int Id { get; }
        public bool IsAccept { get; set; }
        public int AcceptId { get; set; }
        public Dictionary<char, DFAState> Transitions { get; }
        public HashSet<NFAState> NFAStates { get; }

        public DFAState(int id, HashSet<NFAState> nfaStates)
        {
            Id = id;
            NFAStates = nfaStates;
            Transitions = new Dictionary<char, DFAState>();
            IsAccept = false;
            AcceptId = -1;
        }

        public override string ToString()
        {
            var nfaStateStr = string.Join(",", NFAStates.Select(s => s.Id));
            return IsAccept ? $"DFA{Id}[{nfaStateStr}](Accept:{AcceptId})" : $"DFA{Id}[{nfaStateStr}]";
        }

        public override bool Equals(object obj)
        {
            if (obj is DFAState other)
            {
                if (NFAStates.Count != other.NFAStates.Count)
                    return false;
                foreach (var s in NFAStates)
                {
                    if (!other.NFAStates.Contains(s))
                        return false;
                }
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            var hash = 0;
            foreach (var s in NFAStates)
                hash ^= s.Id.GetHashCode();
            return hash;
        }
    }

    public sealed class DFA
    {
        public DFAState StartState { get; private set; }
        public List<DFAState> States { get; }
        public HashSet<char> Alphabet { get; }
        public int PatternCount { get; }

        public DFA(DFAState startState, List<DFAState> states, HashSet<char> alphabet, int patternCount)
        {
            StartState = startState;
            States = states;
            Alphabet = alphabet;
            PatternCount = patternCount;
        }

        public MatchResult Match(string input)
        {
            var current = StartState;
            int lastAcceptPos = -1;
            int lastAcceptId = -1;

            for (int i = 0; i < input.Length; i++)
            {
                var c = input[i];
                if (current.Transitions.TryGetValue(c, out var next))
                {
                    current = next;
                    if (current.IsAccept)
                    {
                        lastAcceptPos = i;
                        lastAcceptId = current.AcceptId;
                    }
                }
                else
                {
                    break;
                }
            }

            if (lastAcceptPos >= 0)
            {
                return new MatchResult(true, 0, lastAcceptPos + 1, lastAcceptId);
            }

            return new MatchResult(false, 0, 0, -1);
        }

        public MatchResult MatchAt(string input, int startPos)
        {
            var current = StartState;
            int lastAcceptPos = -1;
            int lastAcceptId = -1;

            for (int i = startPos; i < input.Length; i++)
            {
                var c = input[i];
                if (current.Transitions.TryGetValue(c, out var next))
                {
                    current = next;
                    if (current.IsAccept)
                    {
                        lastAcceptPos = i;
                        lastAcceptId = current.AcceptId;
                    }
                }
                else
                {
                    break;
                }
            }

            if (lastAcceptPos >= 0)
            {
                return new MatchResult(true, startPos, lastAcceptPos + 1, lastAcceptId);
            }

            return new MatchResult(false, startPos, startPos, -1);
        }
    }

    public sealed class MatchResult
    {
        public bool Success { get; }
        public int StartIndex { get; }
        public int EndIndex { get; }
        public int PatternId { get; }

        public MatchResult(bool success, int startIndex, int endIndex, int patternId)
        {
            Success = success;
            StartIndex = startIndex;
            EndIndex = endIndex;
            PatternId = patternId;
        }

        public override string ToString() => Success
            ? $"Match: [{StartIndex}, {EndIndex}), PatternId={PatternId}"
            : "No match";
    }

    public sealed class NFAToDFAConverter
    {
        private int _stateId;
        private readonly Dictionary<HashSet<NFAState>, DFAState> _stateMap;

        public NFAToDFAConverter()
        {
            _stateMap = new Dictionary<HashSet<NFAState>, DFAState>(new StateSetComparer());
        }

        public DFA Convert(NFA nfa)
        {
            _stateId = 0;
            _stateMap.Clear();

            var startClosure = nfa.GetEpsilonClosure(nfa.StartState);
            var startState = CreateDFAState(startClosure, nfa);

            var alphabet = CollectAlphabet(nfa);

            var unmarkedStates = new Queue<DFAState>();
            unmarkedStates.Enqueue(startState);

            var states = new List<DFAState> { startState };

            while (unmarkedStates.Count > 0)
            {
                var current = unmarkedStates.Dequeue();

                foreach (var c in alphabet)
                {
                    var moveResult = nfa.Move(current.NFAStates, c);
                    if (moveResult.Count == 0)
                        continue;

                    var closure = nfa.GetEpsilonClosure(moveResult);
                    if (closure.Count == 0)
                        continue;

                    if (!TryGetDFAState(closure, out var targetState))
                    {
                        targetState = CreateDFAState(closure, nfa);
                        states.Add(targetState);
                        unmarkedStates.Enqueue(targetState);
                    }

                    current.Transitions[c] = targetState;
                }
            }

            return new DFA(startState, states, alphabet, nfa.PatternId + 1);
        }

        public DFA ConvertMultiple(List<NFA> nfas)
        {
            if (nfas == null || nfas.Count == 0)
                throw new ArgumentException("NFA列表不能为空");

            _stateId = 0;
            _stateMap.Clear();

            var combinedStart = new NFAState(-1);
            foreach (var nfa in nfas)
            {
                combinedStart.AddEpsilonTransition(nfa.StartState);
            }

            var allStates = new List<NFAState> { combinedStart };
            foreach (var nfa in nfas)
            {
                allStates.AddRange(nfa.States);
            }

            var combinedNFA = new NFA(combinedStart, allStates, -1);

            var startClosure = combinedNFA.GetEpsilonClosure(combinedStart);
            var startState = CreateDFAStateForMultiple(startClosure, nfas);

            var alphabet = new HashSet<char>();
            foreach (var nfa in nfas)
            {
                foreach (var c in CollectAlphabet(nfa))
                    alphabet.Add(c);
            }

            var unmarkedStates = new Queue<DFAState>();
            unmarkedStates.Enqueue(startState);

            var states = new List<DFAState> { startState };

            while (unmarkedStates.Count > 0)
            {
                var current = unmarkedStates.Dequeue();

                foreach (var c in alphabet)
                {
                    var moveResult = combinedNFA.Move(current.NFAStates, c);
                    if (moveResult.Count == 0)
                        continue;

                    var closure = combinedNFA.GetEpsilonClosure(moveResult);
                    if (closure.Count == 0)
                        continue;

                    if (!TryGetDFAState(closure, out var targetState))
                    {
                        targetState = CreateDFAStateForMultiple(closure, nfas);
                        states.Add(targetState);
                        unmarkedStates.Enqueue(targetState);
                    }

                    current.Transitions[c] = targetState;
                }
            }

            return new DFA(startState, states, alphabet, nfas.Count);
        }

        private DFAState CreateDFAState(HashSet<NFAState> nfaStates, NFA nfa)
        {
            var dfaState = new DFAState(_stateId++, nfaStates);

            foreach (var nfaState in nfaStates)
            {
                if (nfaState.IsAccept)
                {
                    dfaState.IsAccept = true;
                    dfaState.AcceptId = nfaState.AcceptId;
                    break;
                }
            }

            _stateMap[nfaStates] = dfaState;
            return dfaState;
        }

        private DFAState CreateDFAStateForMultiple(HashSet<NFAState> nfaStates, List<NFA> nfas)
        {
            var dfaState = new DFAState(_stateId++, nfaStates);

            int minAcceptId = int.MaxValue;
            foreach (var nfaState in nfaStates)
            {
                if (nfaState.IsAccept && nfaState.AcceptId < minAcceptId)
                {
                    minAcceptId = nfaState.AcceptId;
                }
            }

            if (minAcceptId != int.MaxValue)
            {
                dfaState.IsAccept = true;
                dfaState.AcceptId = minAcceptId;
            }

            _stateMap[nfaStates] = dfaState;
            return dfaState;
        }

        private bool TryGetDFAState(HashSet<NFAState> nfaStates, out DFAState dfaState)
        {
            return _stateMap.TryGetValue(nfaStates, out dfaState);
        }

        private HashSet<char> CollectAlphabet(NFA nfa)
        {
            var alphabet = new HashSet<char>();
            foreach (var state in nfa.States)
            {
                foreach (var c in state.Transitions.Keys)
                    alphabet.Add(c);
            }
            return alphabet;
        }

        private sealed class StateSetComparer : IEqualityComparer<HashSet<NFAState>>
        {
            public bool Equals(HashSet<NFAState> x, HashSet<NFAState> y)
            {
                if (x == null && y == null) return true;
                if (x == null || y == null) return false;
                if (x.Count != y.Count) return false;
                return x.SetEquals(y);
            }

            public int GetHashCode(HashSet<NFAState> obj)
            {
                if (obj == null) return 0;
                var hash = 0;
                foreach (var s in obj.OrderBy(x => x.Id))
                    hash = hash * 31 + s.Id;
                return hash;
            }
        }
    }
}
