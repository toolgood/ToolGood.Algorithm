using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Antlr4Helper.CSharpHelper.Regexs
{
	internal class CharacterTable : IRegexNodeVisitor
	{
		public string[] table;
		public int index;
		public int layer;

		public CharacterTable()
		{
			table = new string[char.MaxValue + 1];
			for(int i = 0; i < char.MaxValue; i++) {
				table[i] = "";
			}
		}
		public void SetIndex(int index, RegexNode regexNode)
		{
			this.index = index;
			this.layer = 0;
			regexNode.Accept(this);
		}

		public char[] BuildIndex()
		{
			for(int i = 'a'; i < table.Length; i++) {
				var c = (char)i;
				var c2 = StandardChar(c);
				if(c != c2) {
					table[i] = table[c2];
				}
			}

			for(int i = 0; i < table.Length; i++) {
				var sp = table[i].Split('|', StringSplitOptions.RemoveEmptyEntries).OrderBy(q => q);
				table[i] = string.Join("|", sp);
			}

			HashSet<string> texts = new HashSet<string>();
			foreach(string s in table) {
				texts.Add(s);
			}
			var list = texts.OrderByDescending(q => q.Split('|').Length).ThenBy(q => q).ToList();
			var tableIndex = new char[table.Length];
			Dictionary<string, char> dict = new Dictionary<string, char>();
			for(int i = '0'; i <= '9'; i++) {
				if(dict.ContainsKey(table[i]) == false) {
					dict[table[i]] = (char)(dict.Count + 1);
				}
			}
			for(int i = 'A'; i <= 'Z'; i++) {
				if(dict.ContainsKey(table[i]) == false) {
					dict[table[i]] = (char)(dict.Count + 1);
				}
			}
			if(dict.ContainsKey(table['.']) == false) {
				dict[table['.']] = (char)(dict.Count + 1);
			}

			for(int i = 0; i < list.Count; i++) {
				char newKey = (char)(dict.Count + 1);
				if(dict.ContainsKey(list[i])==false) {
					dict[list[i]] = newKey;
				}
			}

			for(int i = 0; i < table.Length; i++) {
				var txt = table[i];
				if(dict.TryGetValue(txt, out char key)) {
					tableIndex[i] = key;
				} else {
					char newKey = (char)(dict.Count + 1);
					dict[txt] = newKey;
					tableIndex[i] = newKey;
				}
			}
			return tableIndex;
		}

		public static char StandardChar(char o) => o switch {
			>= 'a' and <= 'z' => (char)(o - 32),
			< (char)127 => o,
			(char)215 => '*',
			(char)247 => '/',
			(char)8216 or (char)8217 => '\'',
			(char)8220 or (char)8221 => '"',
			(char)12288 => ' ',
			(char)12304 => '[',
			(char)12305 => ']',
			(char)12308 => '(',
			(char)12309 => ')',
			> (char)65280 and < (char)65375 => (char)(o - 65248),
			_ => char.ToUpperInvariant(o)
		};


		public void Visit(CharNode node)
		{
			layer++;
			table[node.Value] += $"{layer:D2},{index:D3}|";
		}

		public void Visit(CharClassNode node)
		{
			layer++;
			if(node.Negated == false) {
				foreach(var (min, max) in node.Ranges) {
					for(int i = min; i <= max; i++) {
						table[i] += $"{layer:D2},{index:D3}|";
					}
				}
			} else {
				List<int> ranges = new List<int>(0x10000);
				foreach(var (min, max) in node.Ranges) {
					for(int i = min; i <= max; i++) {
						ranges.Add(i);
					}
				}
				for(int i = 0; i <= char.MaxValue; i++) {
					if(ranges.Contains(i) == false) {
						table[i] += $"{layer:D2},{index:D3}|";
					}
				}
			}
		}

		public void Visit(AnyCharNode node)
		{
			layer++;
		}

		public void Visit(ConcatNode node)
		{
			foreach(var item in node.Children) {
				item.Accept(this);
			}
		}

		public void Visit(AlternationNode node)
		{
			var old = layer;
			foreach(var item in node.Alternatives) {
				layer = old;
				item.Accept(this);
			}
		}

		public void Visit(QuantifierNode node)
		{
			node.Child.Accept(this);
		}

		public void Visit(GroupNode node)
		{
			node.Child.Accept(this);
		}

		public void Visit(AnchorNode node)
		{
		}

		public void Visit(EmptyNode node)
		{
		}
	}
}
