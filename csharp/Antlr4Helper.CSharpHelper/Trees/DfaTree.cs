using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Text;

namespace Antlr4Helper.CSharpHelper.Trees
{
	public class DfaTree
	{
		private byte[] _dict;
		private ushort[] _next;
		private int[] _next2;
		private byte[] _end; //0,无匹配，0xFF 跳过


		public DfaTree() { }

		public List<string> FindAll(string text)
		{
			List<string> root = new List<string>();
			var p = 0;
			var startIdx = 0;
			for(int i = 0; i < text.Length; i++) {
				var t = _dict[text[i]];
				var next = _next[_next2[p] + t];
				if(next == 0) {
					var r = _end[p];
					if(r == 0) {
						// 添加未确认token
						break;
					} else if(r != 0xFF) {
						root.Add(text.Substring(startIdx, i - startIdx));
					}
					startIdx = i;
					next = _next[t];
					if(next == 0) {
						// 出错了
						break;
					}
				}
				p = next;
			}
			var r2 = _end[p];
			if(r2 == 0) {
				// 添加未确认token
			} else if(r2 != 0xFF) {
				root.Add(text.Substring(startIdx, text.Length - startIdx));
			}
			return root;
		}


		public void Save()
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter bw = new BinaryWriter(memoryStream);

			bw.Write(_dict.Length);
			var bs = compress(_dict);
			bw.Write(bs.ToArray());

			bw.Write(_next.Length);
			var bs2 = compress(_next);
			foreach(var item in bs2) {
				bw.Write(item);
			}

			bw.Write(_end.Length);
			bs = compress(_end);
			bw.Write(bs.ToArray());

			var len = _dict.Max(q => q);
			var max = _next2.Max(q => q);
			bw.Write(len);
			bw.Write(max);

			File.WriteAllBytes("math.bin", memoryStream.ToArray());
		}

		public void Load(byte[] bytes)
		{
			MemoryStream memoryStream = new MemoryStream(bytes);
			BinaryReader br = new BinaryReader(memoryStream);
			var length = br.ReadInt32();
			_dict = new byte[length];
			var index = 0;
			while(index < length) {
				var b = br.ReadByte();
				if(b == 0) {
					var count2 = br.ReadUInt16();
					for(int i = 0; i < count2; i++) {
						_dict[index++] = br.ReadByte();
					}
				} else {
					var data = br.ReadByte();
					for(int i = 0; i < b; i++) {
						_dict[index++] = data;
					}
				}
			}
			length = br.ReadInt32();
			_next = new ushort[length];
			index = 0;
			while(index < length) {
				var b = br.ReadByte();
				if(b == 0) {
					var count2 = br.ReadUInt16();
					for(int i = 0; i < count2; i++) {
						_next[index++] = br.ReadUInt16();
					}
				} else {
					var data = br.ReadUInt16();
					for(int i = 0; i < b; i++) {
						_next[index++] = data;
					}
				}
			}
			length = br.ReadInt32();
			_end = new byte[length];
			index = 0;
			while(index < length) {
				var b = br.ReadByte();
				if(b == 0) {
					var count2 = br.ReadUInt16();
					for(int i = 0; i < count2; i++) {
						_end[index++] = br.ReadByte();
					}
				} else {
					var data = br.ReadByte();
					for(int i = 0; i < b; i++) {
						_end[index++] = data;
					}
				}
			}

			var len2 = br.ReadInt32();
			var max = br.ReadInt32();
			_next2 = new int[length];
			for(int i = 0; i < length; i++) {
				_next2[i] = Math.Min(max, len2 * i);
			}
		}


		public static List<byte> compress(ushort[] bytes)
		{
			List<byte> list = new List<byte>();
			int count = 1;
			ushort data = bytes[0];
			for(int i = 1; i < bytes.Length; i++) {
				if(bytes[i] == data) {
					count++;
				} else {
					if(count > 255) {
						list.Add((byte)0);
						var bs = BitConverter.GetBytes((ushort)count);
						list.Add(bs[0]);
						list.Add(bs[1]);
					} else {
						list.Add((byte)count);
					}
					var bs2 = BitConverter.GetBytes((ushort)data);
					list.Add(bs2[0]);
					list.Add(bs2[1]);
					count = 1;
					data = bytes[i];
				}
			}
			list.Add((byte)count);
			var bs3 = BitConverter.GetBytes((ushort)data);
			list.Add(bs3[0]);
			list.Add(bs3[1]);
			return list;
		}
		public static List<byte> compress(byte[] bytes)
		{
			List<byte> list = new List<byte>();
			int count = 1;
			byte data = bytes[0];
			for(int i = 1; i < bytes.Length; i++) {
				if(bytes[i] == data) {
					count++;
				} else {
					if(count > 255) {
						list.Add((byte)0);
						var bs = BitConverter.GetBytes((ushort)count);
						list.Add(bs[0]);
						list.Add(bs[1]);
					} else {
						list.Add((byte)count);
					}
					list.Add(data);
					count = 1;
					data = bytes[i];
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
			return list;
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
			var nodes1 = nodes.Where(q => q.m_values != null).ToList();
			var nodes2 = nodes.Where(q => q.m_values == null).ToList();
			var tatol = length * (nodes1.Count() + 1) + 1;

			for(int i = 0; i < nodes1.Count(); i++) {
				nodes1[i].Next = (int)i;
			}
			var next2 = nodes1.Count();
			foreach(var item in nodes2) {
				item.Next = next2;
			}
			nodes1.AddRange(nodes2);
			for(int i = 0; i < nodes1.Count; i++) {
				nodes1[i].Index = i;
			}

			_next = new ushort[tatol];
			_next2 = new int[nodes.Count];
			_end = new byte[nodes.Count];

			for(int i = 0; i < nodes1.Count(); i++) {
				var node = nodes1[i];
				_next2[i] = node.Next * length;
				if(node.m_values != null) {
					foreach(var (k, v) in node.m_values) {
						var p = _next2[i] + k;
						_next[p] = (ushort)v.Index;
					}
				}
				if(node.End) {
					_end[i] = (byte)node.Results[0];
				}
			}
		}


	}
}
