using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Antlr4Helper.CSharpHelper.Trees
{
	public class DfaTree
	{
		public byte[] _dict;
		public byte[] _key;
		public ushort[] _next;
		public byte[] _end; //0,无匹配，0xFFFF 跳过

		public DfaTree() { }


		//public List<string> FindAll(string text)
		//{
		//	List<string> root = new List<string>();
		//	var p = 0;

		//	foreach(char t1 in text) {
		//		var t = (char)_dict[t1];
		//		if(t == 0) {
		//			p = 0;
		//			continue;
		//		}
		//		var next = _next[p] + t;
		//		bool find = _key[next] == t;
		//		if(find == false && p != 0) {
		//			p = 0;
		//			next = _next[0] + t;
		//			find = _key[next] == t;
		//		}
		//		if(find) {
		//			var index = _check[next];
		//			if(index > 0) {
		//				foreach(var item in _guides[index]) {
		//					root.Add(_keywords[item]);
		//				}
		//			}
		//			p = next;
		//		}
		//	}
		//	return root;
		//}




		internal void setDict(char[] dict)
		{
			_dict = new byte[0x10000];
			for(int i = 0; i < dict.Length; i++) {
				_dict[i] = (byte)dict[i];
			}
		}

		internal void build(List<TrieNodeEx> nodes)
		{
			var length = (int)_dict.Max(q => q);
			int[] has = new int[0x00FFFFFF];
			bool[] seats = new bool[0x00FFFFFF];
			bool[] seats2 = new bool[0x00FFFFFF];
			Int32 start = 1;
			Int32 oneStart = 1;
			for(int i = 0; i < nodes.Count; i++) {
				var node = nodes[i];
				node.Rank(ref oneStart, ref start, seats, seats2, has);
			}
			Int32 maxCount = has.Length - 1;
			while(has[maxCount] == 0) { maxCount--; }
			length = maxCount + length + 1;

			_key = new byte[length];
			_next = new ushort[length];
			_end = new byte[length];
			for(Int32 i = 0; i < length; i++) {
				var item = nodes[has[i]];
				if(item == null) continue;
				_key[i] = (byte)item.Char;
				_next[i] = (ushort)item.Next;
				if(item.End) {
					_end[i] = (byte)item.Results[0];
				}
			}
		}


	}
}
