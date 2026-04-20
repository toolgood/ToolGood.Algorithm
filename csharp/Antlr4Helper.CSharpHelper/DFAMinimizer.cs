using System;
using System.Collections.Generic;
using System.Linq;

namespace Antlr4Helper.CSharpHelper.RegexEngine
{
    public sealed class DFAMinimizer
    {
        public DFA Minimize(DFA dfa)
        {
            if (dfa.States.Count <= 1)
                return dfa;

            var partitions = CreateInitialPartitions(dfa);

            bool changed;
            do
            {
                changed = false;
                var newPartitions = new List<HashSet<DFAState>>();

                foreach (var partition in partitions)
                {
                    var refined = RefinePartition(partition, partitions, dfa.Alphabet);
                    if (refined.Count > 1)
                        changed = true;
                    newPartitions.AddRange(refined);
                }

                partitions = newPartitions;
            } while (changed);

            return BuildMinimizedDFA(dfa, partitions);
        }

        private List<HashSet<DFAState>> CreateInitialPartitions(DFA dfa)
        {
            var acceptByPattern = new Dictionary<int, HashSet<DFAState>>();
            var nonAccept = new HashSet<DFAState>();

            foreach (var state in dfa.States)
            {
                if (state.IsAccept)
                {
                    if (!acceptByPattern.ContainsKey(state.AcceptId))
                        acceptByPattern[state.AcceptId] = new HashSet<DFAState>();
                    acceptByPattern[state.AcceptId].Add(state);
                }
                else
                {
                    nonAccept.Add(state);
                }
            }

            var partitions = new List<HashSet<DFAState>>();

            if (nonAccept.Count > 0)
                partitions.Add(nonAccept);

            foreach (var partition in acceptByPattern.Values)
                partitions.Add(partition);

            return partitions;
        }

        private List<HashSet<DFAState>> RefinePartition(
            HashSet<DFAState> partition,
            List<HashSet<DFAState>> allPartitions,
            HashSet<char> alphabet)
        {
            if (partition.Count <= 1)
                return new List<HashSet<DFAState>> { partition };

            var result = new List<HashSet<DFAState>>();
            var stateSignatures = new Dictionary<DFAState, string>();

            foreach (var state in partition)
            {
                var signature = ComputeSignature(state, allPartitions, alphabet);
                stateSignatures[state] = signature;
            }

            var groups = partition.GroupBy(s => stateSignatures[s]);

            foreach (var group in groups)
            {
                result.Add(new HashSet<DFAState>(group));
            }

            return result;
        }

        private string ComputeSignature(
            DFAState state,
            List<HashSet<DFAState>> partitions,
            HashSet<char> alphabet)
        {
            var parts = new List<string>();

            foreach (var c in alphabet.OrderBy(x => x))
            {
                int targetPartition = -1;
                if (state.Transitions.TryGetValue(c, out var targetState))
                {
                    targetPartition = FindPartitionIndex(targetState, partitions);
                }
                parts.Add($"{c}:{targetPartition}");
            }

            parts.Add($"Accept:{state.IsAccept}:{state.AcceptId}");

            return string.Join("|", parts);
        }

        private int FindPartitionIndex(DFAState state, List<HashSet<DFAState>> partitions)
        {
            for (int i = 0; i < partitions.Count; i++)
            {
                if (partitions[i].Contains(state))
                    return i;
            }
            return -1;
        }

        private DFA BuildMinimizedDFA(DFA original, List<HashSet<DFAState>> partitions)
        {
            var stateId = 0;
            var partitionToState = new Dictionary<HashSet<DFAState>, DFAState>(
                new PartitionComparer());

            foreach (var partition in partitions)
            {
                var representative = partition.First();
                var newState = new DFAState(stateId++, new HashSet<NFAState>());

                if (representative.IsAccept)
                {
                    newState.IsAccept = true;
                    newState.AcceptId = representative.AcceptId;
                }

                partitionToState[partition] = newState;
            }

            foreach (var partition in partitions)
            {
                var representative = partition.First();
                var newState = partitionToState[partition];

                foreach (var transition in representative.Transitions)
                {
                    var targetPartition = FindPartition(transition.Value, partitions);
                    if (targetPartition != null)
                    {
                        var targetState = partitionToState[targetPartition];
                        newState.Transitions[transition.Key] = targetState;
                    }
                }
            }

            var startPartition = FindPartition(original.StartState, partitions);
            var startState = partitionToState[startPartition];

            var states = partitionToState.Values.ToList();

            return new DFA(startState, states, original.Alphabet, original.PatternCount);
        }

        private HashSet<DFAState> FindPartition(DFAState state, List<HashSet<DFAState>> partitions)
        {
            foreach (var partition in partitions)
            {
                if (partition.Contains(state))
                    return partition;
            }
            return null;
        }

        private sealed class PartitionComparer : IEqualityComparer<HashSet<DFAState>>
        {
            public bool Equals(HashSet<DFAState> x, HashSet<DFAState> y)
            {
                if (x == null && y == null) return true;
                if (x == null || y == null) return false;
                return ReferenceEquals(x, y);
            }

            public int GetHashCode(HashSet<DFAState> obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
