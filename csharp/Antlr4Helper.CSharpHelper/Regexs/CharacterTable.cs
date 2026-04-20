using System;
using System.Collections.Generic;
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
			var tableIndex = new ushort[table.Length];
			Dictionary<string, ushort> dict = new Dictionary<string, ushort>();
			for(int i = 0; i < table.Length; i++) {
				var txt = table[i];
				if(dict.TryGetValue(txt, out ushort key)) {
					tableIndex[i] = key;
				} else {
					ushort newKey = (ushort)dict.Count;
					dict[txt] = newKey;
					tableIndex[i] = newKey;
				}
			}
			return tableIndex;
		}


		public void Visit(CharNode node)
		{
			layer++;
			table[node.Value] += $"[{index},{layer}]";
		}

		public void Visit(CharClassNode node)
		{
			layer++;
			if(node.Negated == false) {
				foreach(var (min, max) in node.Ranges) {
					for(int i = min; i <= max; i++) {
						table[i] += $"[{index},{layer}]";
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
						table[i] += $"[{index},{layer}]";
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
