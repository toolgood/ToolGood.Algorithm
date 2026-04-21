using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Text;

namespace Antlr4Helper.CSharpHelper.Trees
{
	public class DfaTree
	{
		private byte[] _dict ;
		private byte[] _key;
		private ushort[] _next;
		private byte[] _end; //0,无匹配，0xFF 跳过

		public DfaTree() { }

		public List<string> FindAll(string text)
		{
			List<string> root = new List<string>();
			var p = 0;
			var startIdx = 0;
			for(int i = 0; i < text.Length; i++) {
				var t = _dict[text[i]];
				var next = _next[p] + t;
				bool find = _key[next] == t;
				if(find == false) {
					var r = _end[p];
					if(r == 0) {
						startIdx = i;
					} else if(r == 0xFF) {
						startIdx = i;
					} else {
						root.Add(text.Substring(startIdx, i - startIdx));
						startIdx = i;
					}
					p = 0;
					next = t;
					find = _key[next] == t;
					if(find == false) {
						// 出错了
						break;
					}
				}
				p = next;
			}
			var r2 = _end[p];
			if(r2 == 0) {

			} else if(r2 == 0xFF) {

			} else {
				root.Add(text.Substring(startIdx, text.Length - startIdx));
			}
			return root;
		}



		public void Save()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int index=0;
			stringBuilder.Append("private byte[] _dict = new byte[] {");
			List<byte> list = new List<byte>();

			int count = 1;
			byte data = _dict[0];
			for(int i = 1; i < _dict.Length; i++) {
				if(_dict[i]==data) {
					count++;
				} else {
					if(count>255) {
						list.Add((byte)0);
					var bs=	BitConverter.GetBytes((ushort)count);
						list.Add(bs[0]);
						list.Add(bs[1]);
					} else {
						list.Add((byte)count);
					}
					list.Add(data);
					count = 1;
					data = _dict[i];
				}
			}
			if(count > 255) {
				list.Add((byte)0);
				var bs = BitConverter.GetBytes((ushort)count);
				list.Add(bs[0]);
				list.Add(bs[1]);
			} else {
				list.Add((byte)count);
			}
			list.Add(data);

			foreach(var item in list) {
				stringBuilder.Append("0x");
				stringBuilder.Append(item.ToString("X2"));
				stringBuilder.Append(",");
				index++;
				if(index%32==0) {
					stringBuilder.Append("\r\n");
				}
			}
			stringBuilder.Append("};\r\n");

			index = 0; 
			stringBuilder.Append("private byte[] _key = new byte[] {");
			foreach(var item in _key) {
				stringBuilder.Append("0x");
				stringBuilder.Append(item.ToString("X2"));
				stringBuilder.Append(",");
				index++;
				if(index % 32 == 0) {
					stringBuilder.Append("\r\n");
				}
			}
			stringBuilder.Append("};\r\n");


			index = 0; 
			stringBuilder.Append("private ushort[] _next = new ushort[] {");
			foreach(var item in _next) {
				stringBuilder.Append("0x");
				stringBuilder.Append(item.ToString("X4"));
				stringBuilder.Append(",");
				index++;
				if(index % 32 == 0) {
					stringBuilder.Append("\r\n");
				}
			}
			stringBuilder.Append("};\r\n");

			index = 0; 
			stringBuilder.Append("private byte[] _end = new byte[] {");
			foreach(var item in _end) {
				stringBuilder.Append("0x");
				stringBuilder.Append(item.ToString("X2"));
				stringBuilder.Append(",");
				index++;
				if(index % 32 == 0) {
					stringBuilder.Append("\r\n");
				}
			}
			stringBuilder.Append("};\r\n");

			File.WriteAllText("1.txt",stringBuilder.ToString());
		}

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
				//_key[i] = (byte)item.Char;
				_next[i] = (ushort)item.Next;
				if(item.End) {
					_end[i] = (byte)item.Results[0];
					if(item.Next == 0) { //没有子节点了,但是有结果, 需要特殊标记一下地址
						_next[i] = (ushort)(maxCount + 1);
					}
				}
			}

			for(int i = 0; i < nodes.Count; i++) {
				var node = nodes[i];
				var p = node.Next;
				if(node.m_values != null) {
					foreach(var (key, val) in node.m_values) {
						_key[p + key] = (byte)key;
					}
				}
			}


		}


	}
}
