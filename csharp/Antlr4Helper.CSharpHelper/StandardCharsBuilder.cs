using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Antlr4Helper.CSharpHelper
{
	internal class StandardCharsBuilder
	{
		public static void Build()
		{
			var bytes = new byte[65536];
			for(int i = 0; i < bytes.Length; i++) {
				var c = (char)i;
				var ch = (char)StandardChar2(c);
				if(ch != c) {
					bytes[i] = (byte)ch;
				}
			}


			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter bw = new BinaryWriter(memoryStream);
			int count = 1;
			byte data = bytes[0];
			for(int i = 1; i < bytes.Length; i++) {
				if(bytes[i] == data) {
					count++;
				} else {
					bw.Write(data);
					WriteVarInt32(bw, count);
					count = 1;
					data = bytes[i];
				}
			}
			bw.Write(data);
			WriteVarInt32(bw, count);

			bytes = memoryStream.ToArray();
			StringBuilder stringBuilder = new StringBuilder();
			var index = 0;
			foreach(var item in bytes) {
				stringBuilder.Append("0x");
				stringBuilder.Append(item.ToString("X2"));
				stringBuilder.Append(',');
				index++;
				if(index % 32 == 0) {
					stringBuilder.AppendLine();
				}
			}
			File.WriteAllText("CharUtil.txt", stringBuilder.ToString());

		}

		public static void WriteVarInt32(BinaryWriter writer, int value)
		{
			uint v = (uint)value;
			if(v < 0x80) {
				writer.Write((byte)v);
			} else if(v < 0x4000) {
				writer.Write((byte)((v >> 8) | 0x80));
				writer.Write((byte)(v & 0xFF));
			} else if(v < 0x200000) {
				writer.Write((byte)((v >> 16) | 0xC0));
				writer.Write((byte)((v >> 8) & 0xFF));
				writer.Write((byte)(v & 0xFF));
			} else {
				writer.Write((byte)((v >> 24) | 0xE0));
				writer.Write((byte)((v >> 16) & 0xFF));
				writer.Write((byte)((v >> 8) & 0xFF));
				writer.Write((byte)(v & 0xFF));
			}
		}



		private static char StandardChar2(char c)
		{
			if(c == '\r') return '\n';
			if(c == '\t') return ' ';
			if(c == '\f') return ' ';
			if(c < 'a') return c;
			if(c <= 'z') return (char)(c - 32);
			if(c > 65280 && c < 65375) {
				c = (char)(c - 65248);
				c = char.ToUpperInvariant(c);
			}
			if((c >= '\u00c0' && c <= '\u00d6') ||
			   (c >= '\u00d8' && c <= '\u00f6') ||
			   (c >= '\u00f8' && c <= '\u00ff') ||
			   (c >= '\u0100' && c <= '\u1fff') ||
			   (c >= '\u2c00' && c <= '\u2fff') ||
			   (c >= '\u3040' && c <= '\u318f') ||
			   (c >= '\u3300' && c <= '\u337f') ||
			   (c >= '\u3400' && c <= '\u3fff') ||
			   (c >= '\u4e00' && c <= '\u9fff') ||
			   (c >= '\ua000' && c <= '\ud7ff') ||
			   (c >= '\uf900' && c <= '\ufaff') ||
			   (c >= '\uff00' && c <= '\ufff0')) return '_';
			if(c < 127) return c;
			switch(c) {
				case (char)215: return '*';
				case (char)247: return '/';
				case (char)8216:
				case (char)8217: return '\'';
				case (char)8220:
				case (char)8221: return '"';
				case (char)12288: return ' ';
				case (char)12304: return '[';
				case (char)12305: return ']';
				case (char)12308: return '(';
				case (char)12309: return ')';
			}

			return char.ToUpperInvariant(c);
		}
	}
}
