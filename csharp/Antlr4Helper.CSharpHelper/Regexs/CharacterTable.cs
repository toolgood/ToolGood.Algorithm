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

		public ushort[] BuildIndex()
		{
			for(int i = 0; i < table.Length; i++) {
				var sp = table[i].Split('|',StringSplitOptions.RemoveEmptyEntries).OrderBy(q => q);
				table[i] = string.Join("|", sp);
			}

			HashSet<string> texts = new HashSet<string>();
			foreach(string s in table) {
				texts.Add(s);
			}
			var list = texts.OrderByDescending(q => q.Split('|').Length).ThenBy(q => q).ToList();
			var tableIndex = new ushort[table.Length];
			Dictionary<string, ushort> dict = new Dictionary<string, ushort>();
			for(int i = 0; i < list.Count; i++) {
				dict[list[i]] = (ushort)(i + 1);
			}

			for(int i = 0; i < table.Length; i++) {
				var txt = table[i];
				if(dict.TryGetValue(txt, out ushort key)) {
					tableIndex[i] = key;
				} else {
					ushort newKey = (ushort)(dict.Count + 1);
					dict[txt] = newKey;
					tableIndex[i] = newKey;
				}
			}
			return tableIndex;
		}


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
