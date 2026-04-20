using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Antlr4Helper.CSharpHelper.DFAs
{
    public sealed class CharClassPartitioner
    {
        private readonly Dictionary<char, int> _charToClass;
        private readonly List<HashSet<char>> _classes;
        private readonly int _classCount;

        public CharClassPartitioner(DFA dfa)
        {
            _charToClass = new Dictionary<char, int>();
            _classes = new List<HashSet<char>>();

            var signatures = ComputeCharSignatures(dfa);
            BuildClasses(signatures);
            _classCount = _classes.Count;
        }

        private Dictionary<char, string> ComputeCharSignatures(DFA dfa)
        {
            var signatures = new Dictionary<char, string>();
            var allChars = new HashSet<char>();

            foreach (var state in dfa.States)
            {
                foreach (var c in state.Transitions.Keys)
                {
                    allChars.Add(c);
                }
            }

            foreach (var c in allChars)
            {
                var sb = new StringBuilder();
                foreach (var state in dfa.States.OrderBy(s => s.Id))
                {
                    if (state.Transitions.TryGetValue(c, out var target))
                    {
                        sb.Append($"->{target.Id}|");
                    }
                    else
                    {
                        sb.Append("-1|");
                    }
                }
                signatures[c] = sb.ToString();
            }

            return signatures;
        }

        private void BuildClasses(Dictionary<char, string> signatures)
        {
            var signatureToClass = new Dictionary<string, int>();

            foreach (var kvp in signatures)
            {
                var c = kvp.Key;
                var signature = kvp.Value;

                if (!signatureToClass.TryGetValue(signature, out var classId))
                {
                    classId = _classes.Count;
                    _classes.Add(new HashSet<char>());
                    signatureToClass[signature] = classId;
                }

                _charToClass[c] = classId;
                _classes[classId].Add(c);
            }

            for (int i = 0; i < 256; i++)
            {
                var c = (char)i;
                if (!_charToClass.ContainsKey(c))
                {
                    if (_classes.Count == 0 || _classes[0].Count == 0)
                    {
                        _classes.Add(new HashSet<char> { c });
                        _charToClass[c] = _classes.Count - 1;
                    }
                    else
                    {
                        _charToClass[c] = 0;
                        _classes[0].Add(c);
                    }
                }
            }
        }

        public int GetClassId(char c)
        {
            return _charToClass.TryGetValue(c, out var classId) ? classId : 0;
        }

        public int ClassCount => _classCount;

        public IReadOnlyList<HashSet<char>> Classes => _classes;

        public Dictionary<char, int> CharToClassMap => new Dictionary<char, int>(_charToClass);

        public int[] BuildCharToClassArray()
        {
            var array = new int[256];
            for (int i = 0; i < 256; i++)
            {
                array[i] = GetClassId((char)i);
            }
            return array;
        }

        public string GenerateCharToClassCode()
        {
            var sb = new StringBuilder();
            sb.AppendLine("        private static readonly int[] _charToClass = new int[]");
            sb.AppendLine("        {");

            var array = BuildCharToClassArray();
            for (int i = 0; i < 256; i++)
            {
                if (i % 16 == 0)
                    sb.Append("            ");
                sb.Append(array[i]);
                if (i < 255)
                    sb.Append(", ");
                if ((i + 1) % 16 == 0)
                    sb.AppendLine();
            }

            sb.AppendLine();
            sb.AppendLine("        };");
            return sb.ToString();
        }

        public string GenerateClassInfo()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"字符等价类数量: {_classCount}");
            sb.AppendLine();

            for (int i = 0; i < _classes.Count; i++)
            {
                var chars = _classes[i].OrderBy(c => c).ToList();
                sb.AppendLine($"类 {i}: {FormatCharList(chars)} ({chars.Count} 个字符)");
            }

            return sb.ToString();
        }

        private string FormatCharList(List<char> chars)
        {
            if (chars.Count == 0)
                return "(空)";

            var ranges = new List<string>();
            int i = 0;
            while (i < chars.Count)
            {
                char start = chars[i];
                char end = start;

                while (i + 1 < chars.Count && chars[i + 1] == (char)(end + 1))
                {
                    end = chars[i + 1];
                    i++;
                }

                if (start == end)
                    ranges.Add(EscapeChar(start));
                else if (end == (char)(start + 1))
                {
                    ranges.Add(EscapeChar(start));
                    ranges.Add(EscapeChar(end));
                }
                else
                    ranges.Add($"{EscapeChar(start)}-{EscapeChar(end)}");

                i++;
            }

            return string.Join(", ", ranges);
        }

        private string EscapeChar(char c)
        {
            if (c >= 32 && c <= 126 && c != '\\' && c != '\'' && c != '-')
                return $"'{c}'";
            if (c == '\n') return "'\\n'";
            if (c == '\r') return "'\\r'";
            if (c == '\t') return "'\\t'";
            return $"0x{((int)c):X2}";
        }
    }

    public sealed class OptimizedDFA
    {
        public DFA OriginalDFA { get; }
        public CharClassPartitioner CharPartitioner { get; }
        public int[][] TransitionTable { get; }
        public int[] AcceptStates { get; }
        public int StartState { get; }
        public int PatternCount { get; }

        public OptimizedDFA(DFA dfa)
        {
            OriginalDFA = dfa;
            CharPartitioner = new CharClassPartitioner(dfa);
            TransitionTable = BuildTransitionTable(dfa, CharPartitioner);
            AcceptStates = BuildAcceptStates(dfa);
            StartState = dfa.StartState.Id;
            PatternCount = dfa.PatternCount;
        }

        private int[][] BuildTransitionTable(DFA dfa, CharClassPartitioner partitioner)
        {
            var table = new int[dfa.States.Count][];

            foreach (var state in dfa.States)
            {
                var row = new int[partitioner.ClassCount];
                for (int i = 0; i < row.Length; i++)
                    row[i] = -1;

                foreach (var kvp in state.Transitions)
                {
                    var classId = partitioner.GetClassId(kvp.Key);
                    row[classId] = kvp.Value.Id;
                }

                table[state.Id] = row;
            }

            return table;
        }

        private int[] BuildAcceptStates(DFA dfa)
        {
            var acceptStates = new int[dfa.States.Count];
            for (int i = 0; i < acceptStates.Length; i++)
            {
                var state = dfa.States.FirstOrDefault(s => s.Id == i);
                if (state != null && state.IsAccept)
                    acceptStates[i] = state.AcceptId;
                else
                    acceptStates[i] = -1;
            }
            return acceptStates;
        }

        public MatchResult Match(string input)
        {
            int current = StartState;
            int lastAcceptPos = -1;
            int lastAcceptId = -1;
            var charToClass = CharPartitioner.BuildCharToClassArray();

            for (int i = 0; i < input.Length; i++)
            {
                int c = input[i];
                if (c >= 0 && c < 256)
                {
                    int classId = charToClass[c];
                    if (classId < TransitionTable[current].Length)
                    {
                        int next = TransitionTable[current][classId];
                        if (next == -1)
                            break;
                        current = next;
                        if (AcceptStates[current] >= 0)
                        {
                            lastAcceptPos = i;
                            lastAcceptId = AcceptStates[current];
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            if (lastAcceptPos >= 0)
                return new MatchResult(true, 0, lastAcceptPos + 1, lastAcceptId);

            return new MatchResult(false, 0, 0, -1);
        }

        public string GenerateCode(string className, string namespaceName)
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
            sb.AppendLine($"            _startState = {StartState};");
            sb.AppendLine($"            _patternCount = {PatternCount};");
            sb.AppendLine();

            sb.Append(CharPartitioner.GenerateCharToClassCode());
            sb.AppendLine();

            GenerateTransitionTableCode(sb);
            GenerateAcceptStatesCode(sb);

            sb.AppendLine("        }");
            sb.AppendLine();

            GenerateMatchMethodCode(sb);
            GenerateMatchAtMethodCode(sb);
            GenerateMatchResultClassCode(sb);

            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private void GenerateTransitionTableCode(StringBuilder sb)
        {
            sb.AppendLine("            _transitions = new int[][]");
            sb.AppendLine("            {");

            foreach (var row in TransitionTable)
            {
                sb.Append("                new int[] { ");
                sb.Append(string.Join(", ", row));
                sb.AppendLine(" },");
            }

            sb.AppendLine("            };");
            sb.AppendLine();
        }

        private void GenerateAcceptStatesCode(StringBuilder sb)
        {
            sb.AppendLine("            _acceptStates = new int[] {");
            sb.Append("                ");
            sb.Append(string.Join(", ", AcceptStates));
            sb.AppendLine();
            sb.AppendLine("            };");
            sb.AppendLine();
        }

        private void GenerateMatchMethodCode(StringBuilder sb)
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
            sb.AppendLine("                    int classId = _charToClass[c];");
            sb.AppendLine("                    int next = _transitions[current][classId];");
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

        private void GenerateMatchAtMethodCode(StringBuilder sb)
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
            sb.AppendLine("                    int classId = _charToClass[c];");
            sb.AppendLine("                    int next = _transitions[current][classId];");
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

        private void GenerateMatchResultClassCode(StringBuilder sb)
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
    }
}
