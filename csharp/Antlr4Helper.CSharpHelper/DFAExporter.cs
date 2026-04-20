using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Antlr4Helper.CSharpHelper.RegexEngine
{
    public sealed class DFAExporter
    {
        public string ExportToJson(DFA dfa, List<string> patternNames = null)
        {
            var data = new DFAData
            {
                StateCount = dfa.States.Count,
                AlphabetSize = dfa.Alphabet.Count,
                PatternCount = dfa.PatternCount,
                StartState = dfa.StartState.Id,
                States = dfa.States.OrderBy(s => s.Id).Select(s => new DFAStateData
                {
                    Id = s.Id,
                    IsAccept = s.IsAccept,
                    AcceptId = s.AcceptId,
                    Transitions = s.Transitions.ToDictionary(
                        kvp => ((int)kvp.Key).ToString(),
                        kvp => kvp.Value.Id)
                }).ToList(),
                Alphabet = dfa.Alphabet.OrderBy(c => c).Select(c => (int)c).ToList(),
                PatternNames = patternNames
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return JsonSerializer.Serialize(data, options);
        }

        public DFA ImportFromJson(string json)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var data = JsonSerializer.Deserialize<DFAData>(json, options);

            var stateMap = new Dictionary<int, DFAState>();
            foreach (var stateData in data.States)
            {
                var nfaStates = new HashSet<NFAState>();
                var state = new DFAState(stateData.Id, nfaStates)
                {
                    IsAccept = stateData.IsAccept,
                    AcceptId = stateData.AcceptId
                };
                stateMap[stateData.Id] = state;
            }

            foreach (var stateData in data.States)
            {
                var state = stateMap[stateData.Id];
                foreach (var transition in stateData.Transitions)
                {
                    var c = (char)int.Parse(transition.Key);
                    var targetState = stateMap[transition.Value];
                    state.Transitions[c] = targetState;
                }
            }

            var states = stateMap.Values.ToList();
            var alphabet = new HashSet<char>(data.Alphabet.Select(i => (char)i));
            var startState = stateMap[data.StartState];

            return new DFA(startState, states, alphabet, data.PatternCount);
        }

        public string ExportToDot(DFA dfa, string graphName = "DFA")
        {
            var sb = new System.Text.StringBuilder();

            sb.AppendLine($"digraph {graphName} {{");
            sb.AppendLine("    rankdir=LR;");
            sb.AppendLine("    node [shape=circle];");
            sb.AppendLine();

            var acceptStates = dfa.States.Where(s => s.IsAccept).ToList();
            if (acceptStates.Count > 0)
            {
                sb.Append("    node [shape=doublecircle];");
                foreach (var state in acceptStates)
                {
                    sb.Append($" {state.Id}");
                }
                sb.AppendLine(";");
            }

            sb.AppendLine();
            sb.AppendLine($"    {dfa.StartState.Id} [style=filled, fillcolor=lightgray];");
            sb.AppendLine();

            foreach (var state in dfa.States)
            {
                var transitionsByTarget = state.Transitions
                    .GroupBy(kvp => kvp.Value.Id)
                    .OrderBy(g => g.Key);

                foreach (var group in transitionsByTarget)
                {
                    var chars = group.Select(g => g.Key).ToList();
                    var label = FormatCharRange(chars);
                    sb.AppendLine($"    {state.Id} -> {group.Key} [label=\"{label}\"];");
                }
            }

            sb.AppendLine("}");
            return sb.ToString();
        }

        private string FormatCharRange(List<char> chars)
        {
            if (chars.Count == 0)
                return "";

            var sorted = chars.Distinct().OrderBy(c => c).ToList();
            var ranges = new List<string>();

            int i = 0;
            while (i < sorted.Count)
            {
                char start = sorted[i];
                char end = start;

                while (i + 1 < sorted.Count && sorted[i + 1] == (char)(end + 1))
                {
                    end = sorted[i + 1];
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

            var result = string.Join("", ranges);
            if (result.Length > 20)
                return result.Substring(0, 17) + "...";
            return result;
        }

        private string EscapeChar(char c)
        {
            if (c >= 32 && c <= 126 && c != '\\' && c != '"' && c != '-' && c != '>')
                return c.ToString();
            if (c == '\n') return "\\\\n";
            if (c == '\r') return "\\\\r";
            if (c == '\t') return "\\\\t";
            return $"\\\\x{((int)c):X2}";
        }

        public string ExportToTable(DFA dfa)
        {
            var sb = new System.Text.StringBuilder();

            sb.AppendLine("状态转换表:");
            sb.AppendLine("===========");
            sb.AppendLine();

            sb.Append($"{"状态",-8}");
            var alphabet = dfa.Alphabet.OrderBy(c => c).Take(20).ToList();
            foreach (var c in alphabet)
            {
                sb.Append($"{GetCharDisplay(c),-6}");
            }
            sb.AppendLine();
            sb.AppendLine(new string('-', 8 + alphabet.Count * 6));

            foreach (var state in dfa.States.OrderBy(s => s.Id))
            {
                var acceptMark = state.IsAccept ? "*" : " ";
                sb.Append($"{acceptMark}{state.Id,-7}");

                foreach (var c in alphabet)
                {
                    if (state.Transitions.TryGetValue(c, out var target))
                        sb.Append($"{target.Id,-6}");
                    else
                        sb.Append($"{"-",-6}");
                }
                sb.AppendLine();
            }

            sb.AppendLine();
            sb.AppendLine("* 表示接受状态");
            sb.AppendLine("- 表示无转换");

            return sb.ToString();
        }

        private string GetCharDisplay(char c)
        {
            if (c >= 32 && c <= 126 && c != ' ')
                return c.ToString();
            if (c == ' ') return "SP";
            if (c == '\n') return "\\n";
            if (c == '\r') return "\\r";
            if (c == '\t') return "\\t";
            return $"x{((int)c):X2}";
        }
    }

    public sealed class DFAData
    {
        public int StateCount { get; set; }
        public int AlphabetSize { get; set; }
        public int PatternCount { get; set; }
        public int StartState { get; set; }
        public List<DFAStateData> States { get; set; }
        public List<int> Alphabet { get; set; }
        public List<string> PatternNames { get; set; }
    }

    public sealed class DFAStateData
    {
        public int Id { get; set; }
        public bool IsAccept { get; set; }
        public int AcceptId { get; set; }
        public Dictionary<string, int> Transitions { get; set; }
    }
}
