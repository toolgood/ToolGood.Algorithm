using System.IO;

namespace Antlr4Helper.CSharpHelper.Trees
{
	public static class BinaryExtensions
	{
		public static void WriteVarInt32(this BinaryWriter writer, int value)
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

		public static void WriteVarUInt16(this BinaryWriter writer, ushort value)
		{
			if(value < 0x80) {
				writer.Write((byte)value);
			} else if(value < 0x4000) {
				writer.Write((byte)((value >> 8) | 0x80));
				writer.Write((byte)(value & 0xFF));
			} else {
				writer.Write((byte)((value >> 16) | 0xC0));
				writer.Write((byte)((value >> 8) & 0xFF));
				writer.Write((byte)(value & 0xFF));
			}
		}

		public static int ReadVarInt32(this BinaryReader reader)
		{
			byte b0 = reader.ReadByte();
			if((b0 & 0x80) == 0) {
				return b0;
			} else if((b0 & 0xC0) == 0x80) {
				byte b1 = reader.ReadByte();
				return ((b0 & 0x3F) << 8) | b1;
			} else if((b0 & 0xE0) == 0xC0) {
				byte b1 = reader.ReadByte();
				byte b2 = reader.ReadByte();
				return ((b0 & 0x1F) << 16) | (b1 << 8) | b2;
			} else {
				byte b1 = reader.ReadByte();
				byte b2 = reader.ReadByte();
				byte b3 = reader.ReadByte();
				return ((b0 & 0x0F) << 24) | (b1 << 16) | (b2 << 8) | b3;
			}
		}

		public static ushort ReadVarUInt16(this BinaryReader reader)
		{
			byte b0 = reader.ReadByte();
			if((b0 & 0x80) == 0) {
				return b0;
			} else if((b0 & 0xC0) == 0x80) {
				byte b1 = reader.ReadByte();
				return (ushort)(((b0 & 0x3F) << 8) | b1);
			} else {
				byte b1 = reader.ReadByte();
				byte b2 = reader.ReadByte();
				return (ushort)(((b0 & 0x1F) << 16) | (b1 << 8) | b2);
			}
		}
	}
}
