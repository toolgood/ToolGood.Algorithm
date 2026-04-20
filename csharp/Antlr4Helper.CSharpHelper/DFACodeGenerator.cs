using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Antlr4Helper.CSharpHelper.RegexEngine
{
    public sealed class DFACodeGenerator
    {
        public string GenerateCode(DFA dfa, string className, string namespaceName)
        {
            var sb = new StringBuilder();

            sb.AppendLine("using System;");
            sb.AppendLine();
            sb.AppendLine($"namespace {namespaceName}");
            sb.AppendLine("{");
            sb.AppendLine($"    public sealed class {className}");
            sb.AppendLine("    {");
            sb.AppendLine("        private readonly int[][] _transitions;");
            sb.AppendLine("        private readonly int[] _acceptStates;");
            sb.AppendLine("        private readonly int _startState;");
            sb.AppendLine("        private readonly int _patternCount;");
            sb.AppendLine();

            sb.AppendLine($"        public {className}()");
            sb.AppendLine("        {");
            sb.AppendLine("            _startState = " + dfa.StartState.Id + ";");
            sb.AppendLine("            _patternCount = " + dfa.PatternCount + ";");
            sb.AppendLine();

            GenerateTransitionTable(sb, dfa);
            GenerateAcceptStates(sb, dfa);

            sb.AppendLine("        }");
            sb.AppendLine();

            GenerateMatchMethod(sb);
            GenerateMatchAtMethod(sb);
            GenerateGetPatternIdMethod(sb);
            GenerateMatchResultClass(sb);

            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private void GenerateTransitionTable(StringBuilder sb, DFA dfa)
        {
            var charRangeTransitions = OptimizeTransitions(dfa);

            sb.AppendLine("            _transitions = new int[][]");
            sb.AppendLine("            {");

            foreach (var state in dfa.States.OrderBy(s => s.Id))
            {
                var transitionStr = GenerateStateTransitions(state, dfa);
                sb.AppendLine($"                {transitionStr},");
            }

            sb.AppendLine("            };");
            sb.AppendLine();
        }

        private string GenerateStateTransitions(DFAState state, DFA dfa)
        {
            var transitions = new int[256];
            for (int i = 0; i < 256; i++)
                transitions[i] = -1;

            foreach (var kvp in state.Transitions)
            {
                transitions[(int)kvp.Key] = kvp.Value.Id;
            }

            var sb = new StringBuilder();
            sb.AppendLine("new int[]");
            sb.AppendLine("                {");

            for (int i = 0; i < 256; i++)
            {
                if (i % 16 == 0)
                    sb.Append("                    ");
                sb.Append(transitions[i]);
                if (i < 255)
                    sb.Append(", ");
                if ((i + 1) % 16 == 0)
                    sb.AppendLine();
            }

            sb.AppendLine();
            sb.Append("                }");
            return sb.ToString();
        }

        private int GetMostCommonTarget(int[] transitions)
        {
            var counts = new Dictionary<int, int>();
            foreach (var t in transitions)
            {
                if (t != -1)
                {
                    if (!counts.ContainsKey(t))
                        counts[t] = 0;
                    counts[t]++;
                }
            }

            if (counts.Count == 0)
                return -1;

            return counts.OrderByDescending(kvp => kvp.Value).First().Key;
        }

        private Dictionary<char, HashSet<char>> OptimizeTransitions(DFA dfa)
        {
            var ranges = new Dictionary<char, HashSet<char>>();

            foreach (var c in dfa.Alphabet)
            {
                if (!ranges.ContainsKey(c))
                    ranges[c] = new HashSet<char> { c };
            }

            return ranges;
        }

        private void GenerateAcceptStates(StringBuilder sb, DFA dfa)
        {
            var acceptStates = dfa.States.Where(s => s.IsAccept).ToList();

            sb.AppendLine("            _acceptStates = new int[] {");
            foreach (var state in dfa.States.OrderBy(s => s.Id))
            {
                var acceptId = state.IsAccept ? state.AcceptId : -1;
                sb.AppendLine($"                {acceptId},");
            }
            sb.AppendLine("            };");
            sb.AppendLine();
        }

        private void GenerateMatchMethod(StringBuilder sb)
        {
            sb.AppendLine("        public MatchResult Match(string input)");
            sb.AppendLine("        {");
            sb.AppendLine("            if (string.IsNullOrEmpty(input))");
            sb.AppendLine("                return new MatchResult(false, 0, 0, -1);");
            sb.AppendLine();
            sb.AppendLine("            int current = _startState;");
            sb.AppendLine("            int lastAcceptPos = -1;");
            sb.AppendLine("            int lastAcceptId = -1;");
            sb.AppendLine();
            sb.AppendLine("            for (int i = 0; i < input.Length; i++)");
            sb.AppendLine("            {");
            sb.AppendLine("                int c = input[i];");
            sb.AppendLine("                if (c >= 0 && c < 256)");
            sb.AppendLine("                {");
            sb.AppendLine("                    int next = _transitions[current][c];");
            sb.AppendLine("                    if (next == -1)");
            sb.AppendLine("                        break;");
            sb.AppendLine("                    current = next;");
            sb.AppendLine("                    if (_acceptStates[current] >= 0)");
            sb.AppendLine("                    {");
            sb.AppendLine("                        lastAcceptPos = i;");
            sb.AppendLine("                        lastAcceptId = _acceptStates[current];");
            sb.AppendLine("                    }");
            sb.AppendLine("                }");
            sb.AppendLine("                else");
            sb.AppendLine("                {");
            sb.AppendLine("                    break;");
            sb.AppendLine("                }");
            sb.AppendLine("            }");
            sb.AppendLine();
            sb.AppendLine("            if (lastAcceptPos >= 0)");
            sb.AppendLine("                return new MatchResult(true, 0, lastAcceptPos + 1, lastAcceptId);");
            sb.AppendLine();
            sb.AppendLine("            return new MatchResult(false, 0, 0, -1);");
            sb.AppendLine("        }");
            sb.AppendLine();
        }

        private void GenerateMatchAtMethod(StringBuilder sb)
        {
            sb.AppendLine("        public MatchResult MatchAt(string input, int startPos)");
            sb.AppendLine("        {");
            sb.AppendLine("            if (string.IsNullOrEmpty(input) || startPos < 0 || startPos >= input.Length)");
            sb.AppendLine("                return new MatchResult(false, startPos, startPos, -1);");
            sb.AppendLine();
            sb.AppendLine("            int current = _startState;");
            sb.AppendLine("            int lastAcceptPos = -1;");
            sb.AppendLine("            int lastAcceptId = -1;");
            sb.AppendLine();
            sb.AppendLine("            for (int i = startPos; i < input.Length; i++)");
            sb.AppendLine("            {");
            sb.AppendLine("                int c = input[i];");
            sb.AppendLine("                if (c >= 0 && c < 256)");
            sb.AppendLine("                {");
            sb.AppendLine("                    int next = _transitions[current][c];");
            sb.AppendLine("                    if (next == -1)");
            sb.AppendLine("                        break;");
            sb.AppendLine("                    current = next;");
            sb.AppendLine("                    if (_acceptStates[current] >= 0)");
            sb.AppendLine("                    {");
            sb.AppendLine("                        lastAcceptPos = i;");
            sb.AppendLine("                        lastAcceptId = _acceptStates[current];");
            sb.AppendLine("                    }");
            sb.AppendLine("                }");
            sb.AppendLine("                else");
            sb.AppendLine("                {");
            sb.AppendLine("                    break;");
            sb.AppendLine("                }");
            sb.AppendLine("            }");
            sb.AppendLine();
            sb.AppendLine("            if (lastAcceptPos >= 0)");
            sb.AppendLine("                return new MatchResult(true, startPos, lastAcceptPos + 1, lastAcceptId);");
            sb.AppendLine();
            sb.AppendLine("            return new MatchResult(false, startPos, startPos, -1);");
            sb.AppendLine("        }");
            sb.AppendLine();
        }

        private void GenerateGetPatternIdMethod(StringBuilder sb)
        {
            sb.AppendLine("        public int GetPatternCount() => _patternCount;");
            sb.AppendLine();
        }

        private void GenerateMatchResultClass(StringBuilder sb)
        {
            sb.AppendLine("        public sealed class MatchResult");
            sb.AppendLine("        {");
            sb.AppendLine("            public bool Success { get; }");
            sb.AppendLine("            public int StartIndex { get; }");
            sb.AppendLine("            public int EndIndex { get; }");
            sb.AppendLine("            public int PatternId { get; }");
            sb.AppendLine();
            sb.AppendLine("            public MatchResult(bool success, int startIndex, int endIndex, int patternId)");
            sb.AppendLine("            {");
            sb.AppendLine("                Success = success;");
            sb.AppendLine("                StartIndex = startIndex;");
            sb.AppendLine("                EndIndex = endIndex;");
            sb.AppendLine("                PatternId = patternId;");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();
        }

        public string GenerateOptimizedCode(DFA dfa, string className, string namespaceName)
        {
            var sb = new StringBuilder();

            sb.AppendLine("using System;");
            sb.AppendLine();
            sb.AppendLine($"namespace {namespaceName}");
            sb.AppendLine("{");
            sb.AppendLine($"    public sealed class {className}");
            sb.AppendLine("    {");

            GenerateOptimizedTransitions(sb, dfa);

            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private void GenerateOptimizedTransitions(StringBuilder sb, DFA dfa)
        {
            sb.AppendLine("        public bool TryMatch(string input, out int patternId, out int length)");
            sb.AppendLine("        {");
            sb.AppendLine("            patternId = -1;");
            sb.AppendLine("            length = 0;");
            sb.AppendLine();
            sb.AppendLine("            if (string.IsNullOrEmpty(input))");
            sb.AppendLine("                return false;");
            sb.AppendLine();
            sb.AppendLine("            int state = " + dfa.StartState.Id + ";");
            sb.AppendLine("            int lastAcceptLength = 0;");
            sb.AppendLine("            int lastAcceptId = -1;");
            sb.AppendLine();

            GenerateStateSwitch(sb, dfa);

            sb.AppendLine("            if (lastAcceptLength > 0)");
            sb.AppendLine("            {");
            sb.AppendLine("                patternId = lastAcceptId;");
            sb.AppendLine("                length = lastAcceptLength;");
            sb.AppendLine("                return true;");
            sb.AppendLine("            }");
            sb.AppendLine();
            sb.AppendLine("            return false;");
            sb.AppendLine("        }");
        }

        private void GenerateStateSwitch(StringBuilder sb, DFA dfa)
        {
            sb.AppendLine("            for (int i = 0; i < input.Length; i++)");
            sb.AppendLine("            {");
            sb.AppendLine("                char c = input[i];");
            sb.AppendLine();
            sb.AppendLine("                switch (state)");
            sb.AppendLine("                {");

            foreach (var state in dfa.States.OrderBy(s => s.Id))
            {
                sb.AppendLine($"                    case {state.Id}:");
                GenerateStateCase(sb, state, dfa);
            }

            sb.AppendLine("                    default:");
            sb.AppendLine("                        return lastAcceptLength > 0;");
            sb.AppendLine("                }");
            sb.AppendLine("            }");
            sb.AppendLine();
        }

        private void GenerateStateCase(StringBuilder sb, DFAState state, DFA dfa)
        {
            if (state.Transitions.Count == 0)
            {
                if (state.IsAccept)
                {
                    sb.AppendLine($"                        lastAcceptLength = i;");
                    sb.AppendLine($"                        lastAcceptId = {state.AcceptId};");
                }
                sb.AppendLine("                        return lastAcceptLength > 0;");
                return;
            }

            if (state.IsAccept)
            {
                sb.AppendLine($"                        lastAcceptLength = i;");
                sb.AppendLine($"                        lastAcceptId = {state.AcceptId};");
            }

            var charGroups = GroupTransitionsByTarget(state);

            if (charGroups.Count == 1 && charGroups.Values.First().Count > 10)
            {
                var target = charGroups.Keys.First();
                sb.AppendLine($"                        state = {target.Id};");
                sb.AppendLine("                        break;");
            }
            else
            {
                sb.AppendLine("                        switch (c)");
                sb.AppendLine("                        {");

                foreach (var kvp in charGroups)
                {
                    var chars = kvp.Value;
                    var target = kvp.Key;

                    if (chars.Count <= 5)
                    {
                        foreach (var ch in chars)
                        {
                            sb.AppendLine($"                            case '{EscapeChar(ch)}':");
                        }
                        sb.AppendLine($"                                state = {target.Id};");
                        sb.AppendLine("                                break;");
                    }
                    else
                    {
                        var ranges = CharRangeOptimizer.Optimize(chars);
                        foreach (var range in ranges)
                        {
                            if (range.Min == range.Max)
                            {
                                sb.AppendLine($"                            case '{EscapeChar(range.Min)}':");
                            }
                            else
                            {
                                sb.AppendLine($"                            case char ch when ch >= '{EscapeChar(range.Min)}' && ch <= '{EscapeChar(range.Max)}':");
                            }
                        }
                        sb.AppendLine($"                                state = {target.Id};");
                        sb.AppendLine("                                break;");
                    }
                }

                sb.AppendLine("                            default:");
                sb.AppendLine("                                return lastAcceptLength > 0;");
                sb.AppendLine("                        }");
                sb.AppendLine("                        break;");
            }
        }

        private Dictionary<DFAState, List<char>> GroupTransitionsByTarget(DFAState state)
        {
            var groups = new Dictionary<DFAState, List<char>>();

            foreach (var kvp in state.Transitions)
            {
                if (!groups.ContainsKey(kvp.Value))
                    groups[kvp.Value] = new List<char>();
                groups[kvp.Value].Add(kvp.Key);
            }

            return groups;
        }

        private static string EscapeChar(char c)
        {
            switch (c)
            {
                case '\'': return "\\'";
                case '\"': return "\\\"";
                case '\\': return "\\\\";
                case '\n': return "\\n";
                case '\r': return "\\r";
                case '\t': return "\\t";
                case '\0': return "\\0";
                default:
                    if (c < 32 || c > 126)
                        return $"\\x{((int)c):X2}";
                    return c.ToString();
            }
        }
    }

    public static class CharRangeOptimizer
    {
        public static List<(char Min, char Max)> Optimize(List<char> chars)
        {
            if (chars == null || chars.Count == 0)
                return new List<(char, char)>();

            var sorted = chars.Distinct().OrderBy(c => c).ToList();
            var ranges = new List<(char Min, char Max)>();

            char rangeStart = sorted[0];
            char rangeEnd = sorted[0];

            for (int i = 1; i < sorted.Count; i++)
            {
                if (sorted[i] == (char)(rangeEnd + 1))
                {
                    rangeEnd = sorted[i];
                }
                else
                {
                    ranges.Add((rangeStart, rangeEnd));
                    rangeStart = sorted[i];
                    rangeEnd = sorted[i];
                }
            }

            ranges.Add((rangeStart, rangeEnd));
            return ranges;
        }
    }
}
