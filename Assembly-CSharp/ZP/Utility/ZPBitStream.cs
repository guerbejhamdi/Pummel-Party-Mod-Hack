using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace ZP.Utility
{
	// Token: 0x020005F1 RID: 1521
	public class ZPBitStream
	{
		// Token: 0x06002706 RID: 9990 RVA: 0x0001BCB7 File Offset: 0x00019EB7
		public ZPBitStream()
		{
			this.SetupBuffer(4);
		}

		// Token: 0x06002707 RID: 9991 RVA: 0x0001BCD2 File Offset: 0x00019ED2
		public ZPBitStream(byte[] data, int bit_length)
		{
			this.SetData(data, bit_length);
		}

		// Token: 0x06002708 RID: 9992 RVA: 0x0001BCEE File Offset: 0x00019EEE
		public ZPBitStream(byte[] data, int start, int length, int bit_length)
		{
			this.SetData(data, start, length, bit_length);
		}

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x06002709 RID: 9993 RVA: 0x0001BD0D File Offset: 0x00019F0D
		public int Length
		{
			get
			{
				return this.data_bit_length / 8;
			}
		}

		// Token: 0x0600270A RID: 9994 RVA: 0x0001BD17 File Offset: 0x00019F17
		public void SetupBuffer(int byteSize)
		{
			this.write_pos = 0;
			this.read_pos = 0;
			this.buffer = new byte[byteSize];
			this.data_bit_length = 0;
			this.buffer_length = byteSize;
			this.buffer_bit_length = byteSize * 8;
		}

		// Token: 0x0600270B RID: 9995 RVA: 0x000EB3E0 File Offset: 0x000E95E0
		public void SetData(byte[] data, int bit_length)
		{
			this.write_pos = 0;
			this.read_pos = 0;
			if (this.buffer.Length < data.Length)
			{
				this.buffer = new byte[data.Length];
			}
			Array.Copy(data, this.buffer, data.Length);
			this.data_bit_length = bit_length;
			this.buffer_length = data.Length;
			this.buffer_bit_length = bit_length;
		}

		// Token: 0x0600270C RID: 9996 RVA: 0x000EB43C File Offset: 0x000E963C
		public void SetData(byte[] data, int start, int length, int bit_length)
		{
			this.write_pos = 0;
			this.read_pos = 0;
			int num = length - start;
			if (this.buffer.Length < num)
			{
				this.buffer = new byte[num];
			}
			Array.Copy(data, start, this.buffer, 0, num);
			this.data_bit_length = bit_length;
			this.buffer_length = data.Length;
			this.buffer_bit_length = bit_length;
		}

		// Token: 0x0600270D RID: 9997 RVA: 0x0001BD4A File Offset: 0x00019F4A
		public byte[] GetBuffer()
		{
			return this.buffer;
		}

		// Token: 0x0600270E RID: 9998 RVA: 0x000EB49C File Offset: 0x000E969C
		public byte[] GetDataCopy()
		{
			int byteLength = this.GetByteLength();
			byte[] array = new byte[byteLength];
			Array.Copy(this.buffer, array, byteLength);
			return array;
		}

		// Token: 0x0600270F RID: 9999 RVA: 0x000EB4C8 File Offset: 0x000E96C8
		public void Reserve(int bytes)
		{
			if (this.buffer_length >= bytes)
			{
				return;
			}
			byte[] destinationArray = new byte[bytes];
			int length = (bytes < this.buffer.Length) ? bytes : this.buffer.Length;
			Array.Copy(this.buffer, destinationArray, length);
			this.buffer = destinationArray;
			this.data_bit_length = 0;
			this.buffer_length = bytes;
			this.buffer_bit_length = bytes * 8;
		}

		// Token: 0x06002710 RID: 10000 RVA: 0x0001BD52 File Offset: 0x00019F52
		public void Reset()
		{
			this.read_pos = 0;
			this.write_pos = 0;
			this.data_bit_length = 0;
			Array.Clear(this.buffer, 0, this.buffer.Length);
		}

		// Token: 0x06002711 RID: 10001 RVA: 0x0001BD7D File Offset: 0x00019F7D
		public void Clear()
		{
			this.read_pos = 0;
			this.write_pos = 0;
			this.data_bit_length = 0;
			this.SetupBuffer(4);
		}

		// Token: 0x06002712 RID: 10002 RVA: 0x0001BD9B File Offset: 0x00019F9B
		public void SetReadPosition(int bit_pos)
		{
			this.read_pos = bit_pos;
		}

		// Token: 0x06002713 RID: 10003 RVA: 0x0001BDA4 File Offset: 0x00019FA4
		public void SetWritePosition(int bit_pos)
		{
			this.write_pos = bit_pos;
		}

		// Token: 0x06002714 RID: 10004 RVA: 0x0001BDAD File Offset: 0x00019FAD
		public int GetReadBytePosition()
		{
			return this.read_pos / 8 + ((this.read_pos % 8 > 0) ? 1 : 0);
		}

		// Token: 0x06002715 RID: 10005 RVA: 0x0001BDC7 File Offset: 0x00019FC7
		public int GetBitLength()
		{
			return this.data_bit_length;
		}

		// Token: 0x06002716 RID: 10006 RVA: 0x0001BDCF File Offset: 0x00019FCF
		public int GetByteLength()
		{
			return this.data_bit_length / 8 + ((this.data_bit_length % 8 > 0) ? 1 : 0);
		}

		// Token: 0x06002717 RID: 10007 RVA: 0x0001BDE9 File Offset: 0x00019FE9
		public void SetDataLength(int bytes)
		{
			this.data_bit_length = bytes * 8;
			this.buffer_length = bytes;
			this.buffer_bit_length = bytes * 8;
		}

		// Token: 0x06002718 RID: 10008 RVA: 0x000EB528 File Offset: 0x000E9728
		public void Write(bool bit)
		{
			if (this.buffer_bit_length - this.data_bit_length < 1)
			{
				this.Expand(1);
			}
			if (bit)
			{
				byte[] array = this.buffer;
				int num = this.write_pos / 8;
				array[num] |= (byte)(1 << this.write_pos % 8);
			}
			else
			{
				byte[] array2 = this.buffer;
				int num2 = this.write_pos / 8;
				array2[num2] &= (byte)(~(byte)(1 << this.write_pos % 8));
			}
			this.data_bit_length++;
			this.write_pos++;
		}

		// Token: 0x06002719 RID: 10009 RVA: 0x0001BE04 File Offset: 0x0001A004
		public void Write(byte val)
		{
			this.Write(val, 8);
		}

		// Token: 0x0600271A RID: 10010 RVA: 0x0001BE0E File Offset: 0x0001A00E
		public void Write(char val)
		{
			this.Write(val, 16);
		}

		// Token: 0x0600271B RID: 10011 RVA: 0x0001BE19 File Offset: 0x0001A019
		public void Write(short val)
		{
			this.Write(val, 16);
		}

		// Token: 0x0600271C RID: 10012 RVA: 0x0001BE24 File Offset: 0x0001A024
		public void Write(ushort val)
		{
			this.Write(val, 16);
		}

		// Token: 0x0600271D RID: 10013 RVA: 0x0001BE2F File Offset: 0x0001A02F
		public void Write(int val)
		{
			this.Write(val, 32);
		}

		// Token: 0x0600271E RID: 10014 RVA: 0x0001BE3A File Offset: 0x0001A03A
		public void Write(uint val)
		{
			this.Write(val, 32);
		}

		// Token: 0x0600271F RID: 10015 RVA: 0x0001BE45 File Offset: 0x0001A045
		public void Write(long val)
		{
			this.Write(val, 64);
		}

		// Token: 0x06002720 RID: 10016 RVA: 0x0001BE50 File Offset: 0x0001A050
		public void Write(ulong val)
		{
			this.Write(val, 64);
		}

		// Token: 0x06002721 RID: 10017 RVA: 0x0001BE5B File Offset: 0x0001A05B
		public void Write(float val)
		{
			this.Write(val, 32);
		}

		// Token: 0x06002722 RID: 10018 RVA: 0x0001BE66 File Offset: 0x0001A066
		public void Write(double val)
		{
			this.Write(val, 64);
		}

		// Token: 0x06002723 RID: 10019 RVA: 0x0001BE71 File Offset: 0x0001A071
		public void Write(byte val, int bits)
		{
			if (this.buffer_bit_length - this.data_bit_length < bits)
			{
				this.Expand(bits);
			}
			this.write_byte(val, bits);
		}

		// Token: 0x06002724 RID: 10020 RVA: 0x000EB5BC File Offset: 0x000E97BC
		public void Write(char val, int bits)
		{
			if (this.buffer_bit_length - this.data_bit_length < bits)
			{
				this.Expand(bits);
			}
			byte[] bytes = BitConverter.GetBytes(val);
			if (bits <= 8)
			{
				this.write_byte(bytes[0], bits);
				return;
			}
			this.write_byte(bytes[0], 8);
			bits -= 8;
			this.write_byte(bytes[1], bits);
		}

		// Token: 0x06002725 RID: 10021 RVA: 0x000EB610 File Offset: 0x000E9810
		public void Write(short val, int bits)
		{
			if (this.buffer_bit_length - this.data_bit_length < bits)
			{
				this.Expand(bits);
			}
			byte[] bytes = BitConverter.GetBytes(val);
			if (bits <= 8)
			{
				this.write_byte(bytes[0], bits);
				return;
			}
			this.write_byte(bytes[0], 8);
			bits -= 8;
			this.write_byte(bytes[1], bits);
		}

		// Token: 0x06002726 RID: 10022 RVA: 0x000EB664 File Offset: 0x000E9864
		public void Write(ushort val, int bits)
		{
			if (this.buffer_bit_length - this.data_bit_length < bits)
			{
				this.Expand(bits);
			}
			byte[] bytes = BitConverter.GetBytes(val);
			if (bits <= 8)
			{
				this.write_byte(bytes[0], bits);
				return;
			}
			this.write_byte(bytes[0], 8);
			bits -= 8;
			this.write_byte(bytes[1], bits);
		}

		// Token: 0x06002727 RID: 10023 RVA: 0x000EB6B8 File Offset: 0x000E98B8
		public void Write(int val, int bits)
		{
			if (this.buffer_bit_length - this.data_bit_length < bits)
			{
				this.Expand(bits);
			}
			byte[] bytes = BitConverter.GetBytes(val);
			for (int i = 0; i < 4; i++)
			{
				if (bits <= 8)
				{
					this.write_byte(bytes[i], bits);
					return;
				}
				this.write_byte(bytes[i], 8);
				bits -= 8;
			}
		}

		// Token: 0x06002728 RID: 10024 RVA: 0x000EB710 File Offset: 0x000E9910
		public void Write(uint val, int bits)
		{
			if (this.buffer_bit_length - this.data_bit_length < bits)
			{
				this.Expand(bits);
			}
			byte[] bytes = BitConverter.GetBytes(val);
			for (int i = 0; i < 4; i++)
			{
				if (bits <= 8)
				{
					this.write_byte(bytes[i], bits);
					return;
				}
				this.write_byte(bytes[i], 8);
				bits -= 8;
			}
		}

		// Token: 0x06002729 RID: 10025 RVA: 0x000EB768 File Offset: 0x000E9968
		public void Write(long val, int bits)
		{
			if (this.buffer_bit_length - this.data_bit_length < bits)
			{
				this.Expand(bits);
			}
			byte[] bytes = BitConverter.GetBytes(val);
			for (int i = 0; i < 8; i++)
			{
				if (bits <= 8)
				{
					this.write_byte(bytes[i], bits);
					return;
				}
				this.write_byte(bytes[i], 8);
				bits -= 8;
			}
		}

		// Token: 0x0600272A RID: 10026 RVA: 0x000EB7C0 File Offset: 0x000E99C0
		public void Write(ulong val, int bits)
		{
			if (this.buffer_bit_length - this.data_bit_length < bits)
			{
				this.Expand(bits);
			}
			byte[] bytes = BitConverter.GetBytes(val);
			for (int i = 0; i < 8; i++)
			{
				if (bits <= 8)
				{
					this.write_byte(bytes[i], bits);
					return;
				}
				this.write_byte(bytes[i], 8);
				bits -= 8;
			}
		}

		// Token: 0x0600272B RID: 10027 RVA: 0x000EB818 File Offset: 0x000E9A18
		public void Write(float val, int bits)
		{
			if (this.buffer_bit_length - this.data_bit_length < bits)
			{
				this.Expand(bits);
			}
			byte[] bytes = BitConverter.GetBytes(val);
			for (int i = 0; i < 4; i++)
			{
				if (bits <= 8)
				{
					this.write_byte(bytes[i], bits);
					return;
				}
				this.write_byte(bytes[i], 8);
				bits -= 8;
			}
		}

		// Token: 0x0600272C RID: 10028 RVA: 0x000EB870 File Offset: 0x000E9A70
		public void Write(double val, int bits)
		{
			if (this.buffer_bit_length - this.data_bit_length < bits)
			{
				this.Expand(bits);
			}
			byte[] bytes = BitConverter.GetBytes(val);
			for (int i = 0; i < 8; i++)
			{
				if (bits <= 8)
				{
					this.write_byte(bytes[i], bits);
					return;
				}
				this.write_byte(bytes[i], 8);
				bits -= 8;
			}
		}

		// Token: 0x0600272D RID: 10029 RVA: 0x000EB8C8 File Offset: 0x000E9AC8
		public void Write(string val)
		{
			if (!string.IsNullOrEmpty(val))
			{
				this.Write((ushort)val.Length);
				for (int i = 0; i < val.Length; i++)
				{
					this.Write(val[i]);
				}
				return;
			}
			this.Write(0);
		}

		// Token: 0x0600272E RID: 10030 RVA: 0x000EB910 File Offset: 0x000E9B10
		public void WriteUnicode(string val)
		{
			if (!string.IsNullOrEmpty(val))
			{
				byte[] bytes = Encoding.Unicode.GetBytes(val);
				this.Write((ushort)bytes.Length);
				for (int i = 0; i < bytes.Length; i++)
				{
					this.Write(bytes[i]);
				}
				return;
			}
			this.Write(0);
		}

		// Token: 0x0600272F RID: 10031 RVA: 0x000EB95C File Offset: 0x000E9B5C
		public bool ReadBool()
		{
			if (1 > this.data_bit_length - this.read_pos)
			{
				throw new EndOfStreamException("Bitstream cannot read " + 1.ToString() + " bits, end of stream reached.");
			}
			bool result = ((int)this.buffer[this.read_pos / 8] & 1 << this.read_pos % 8) != 0;
			this.read_pos++;
			return result;
		}

		// Token: 0x06002730 RID: 10032 RVA: 0x000EB9C8 File Offset: 0x000E9BC8
		public byte ReadByte()
		{
			if (8 > this.data_bit_length - this.read_pos)
			{
				throw new EndOfStreamException("Bitstream cannot read " + 8.ToString() + " bits, end of stream reached.");
			}
			return this.read_byte(8);
		}

		// Token: 0x06002731 RID: 10033 RVA: 0x0001BE92 File Offset: 0x0001A092
		public char ReadChar()
		{
			return this.ReadChar(16);
		}

		// Token: 0x06002732 RID: 10034 RVA: 0x0001BE9C File Offset: 0x0001A09C
		public short ReadShort()
		{
			return this.ReadShort(16);
		}

		// Token: 0x06002733 RID: 10035 RVA: 0x0001BEA6 File Offset: 0x0001A0A6
		public ushort ReadUShort()
		{
			return this.ReadUShort(16);
		}

		// Token: 0x06002734 RID: 10036 RVA: 0x0001BEB0 File Offset: 0x0001A0B0
		public uint ReadUInt()
		{
			return this.ReadUInt(32);
		}

		// Token: 0x06002735 RID: 10037 RVA: 0x0001BEBA File Offset: 0x0001A0BA
		public int ReadInt()
		{
			return this.ReadInt(32);
		}

		// Token: 0x06002736 RID: 10038 RVA: 0x0001BEC4 File Offset: 0x0001A0C4
		public long ReadLong()
		{
			return this.ReadLong(64);
		}

		// Token: 0x06002737 RID: 10039 RVA: 0x0001BECE File Offset: 0x0001A0CE
		public ulong ReadULong()
		{
			return this.ReadULong(64);
		}

		// Token: 0x06002738 RID: 10040 RVA: 0x0001BED8 File Offset: 0x0001A0D8
		public float ReadFloat()
		{
			return this.ReadFloat(32);
		}

		// Token: 0x06002739 RID: 10041 RVA: 0x0001BEE2 File Offset: 0x0001A0E2
		public double ReadDouble()
		{
			return this.ReadDouble(64);
		}

		// Token: 0x0600273A RID: 10042 RVA: 0x000EBA0C File Offset: 0x000E9C0C
		public byte ReadByte(int bits)
		{
			if (bits > this.data_bit_length - this.read_pos)
			{
				throw new EndOfStreamException("Bitstream cannot read " + bits.ToString() + " bits, end of stream reached.");
			}
			if (bits < 1 || bits > 8)
			{
				throw new ArgumentException("Parameter bits = " + bits.ToString() + ", bits must be between 1 and " + 8.ToString());
			}
			return this.read_byte(bits);
		}

		// Token: 0x0600273B RID: 10043 RVA: 0x000EBA7C File Offset: 0x000E9C7C
		public char ReadChar(int bits)
		{
			if (bits > this.data_bit_length - this.read_pos)
			{
				throw new EndOfStreamException("Bitstream cannot read " + bits.ToString() + " bits, end of stream reached.");
			}
			if (bits < 1 || bits > 16)
			{
				throw new ArgumentException("Parameter bits = " + bits.ToString() + ", bits must be between 1 and " + 16.ToString());
			}
			char c = '\0';
			for (int i = 0; i < 2; i++)
			{
				if (bits < 8)
				{
					return c | (char)(this.read_byte(bits) << i * 8);
				}
				c |= (char)(this.read_byte(8) << i * 8);
				bits -= 8;
			}
			return c;
		}

		// Token: 0x0600273C RID: 10044 RVA: 0x000EBB24 File Offset: 0x000E9D24
		public short ReadShort(int bits)
		{
			if (bits > this.data_bit_length - this.read_pos)
			{
				throw new EndOfStreamException("Bitstream cannot read " + bits.ToString() + " bits, end of stream reached.");
			}
			if (bits < 1 || bits > 16)
			{
				throw new ArgumentException("Parameter bits = " + bits.ToString() + ", bits must be between 1 and " + 16.ToString());
			}
			short num = 0;
			for (int i = 0; i < 2; i++)
			{
				if (bits < 8)
				{
					return num | (short)(this.read_byte(bits) << i * 8);
				}
				num |= (short)(this.read_byte(8) << i * 8);
				bits -= 8;
			}
			return num;
		}

		// Token: 0x0600273D RID: 10045 RVA: 0x000EBBCC File Offset: 0x000E9DCC
		public ushort ReadUShort(int bits)
		{
			if (bits > this.data_bit_length - this.read_pos)
			{
				throw new EndOfStreamException("Bitstream cannot read " + bits.ToString() + " bits, end of stream reached.");
			}
			if (bits < 1 || bits > 16)
			{
				throw new ArgumentException("Parameter bits = " + bits.ToString() + ", bits must be between 1 and " + 16.ToString());
			}
			ushort num = 0;
			for (int i = 0; i < 2; i++)
			{
				if (bits < 8)
				{
					return num | (ushort)(this.read_byte(bits) << i * 8);
				}
				num |= (ushort)(this.read_byte(8) << i * 8);
				bits -= 8;
			}
			return num;
		}

		// Token: 0x0600273E RID: 10046 RVA: 0x000EBC74 File Offset: 0x000E9E74
		public int ReadInt(int bits)
		{
			if (bits > this.data_bit_length - this.read_pos)
			{
				throw new EndOfStreamException("Bitstream cannot read " + bits.ToString() + " bits, end of stream reached.");
			}
			if (bits < 1 || bits > 32)
			{
				throw new ArgumentException("Parameter bits = " + bits.ToString() + ", bits must be between 1 and " + 32.ToString());
			}
			int num = 0;
			for (int i = 0; i < 4; i++)
			{
				if (bits < 8)
				{
					return num | (int)this.read_byte(bits) << i * 8;
				}
				num |= (int)this.read_byte(8) << i * 8;
				bits -= 8;
			}
			return num;
		}

		// Token: 0x0600273F RID: 10047 RVA: 0x000EBD18 File Offset: 0x000E9F18
		public uint ReadUInt(int bits)
		{
			if (bits > this.data_bit_length - this.read_pos)
			{
				throw new EndOfStreamException("Bitstream cannot read " + bits.ToString() + " bits, end of stream reached.");
			}
			if (bits < 1 || bits > 32)
			{
				throw new ArgumentException("Parameter bits = " + bits.ToString() + ", bits must be between 1 and " + 32.ToString());
			}
			uint num = 0U;
			for (int i = 0; i < 4; i++)
			{
				if (bits < 8)
				{
					return num | (uint)((uint)this.read_byte(bits) << i * 8);
				}
				num |= (uint)((uint)this.read_byte(8) << i * 8);
				bits -= 8;
			}
			return num;
		}

		// Token: 0x06002740 RID: 10048 RVA: 0x000EBDBC File Offset: 0x000E9FBC
		public long ReadLong(int bits)
		{
			if (bits > this.data_bit_length - this.read_pos)
			{
				throw new EndOfStreamException("Bitstream cannot read " + bits.ToString() + " bits, end of stream reached.");
			}
			if (bits < 1 || bits > 64)
			{
				throw new ArgumentException("Parameter bits = " + bits.ToString() + ", bits must be between 1 and " + 64.ToString());
			}
			long num = 0L;
			for (int i = 0; i < 8; i++)
			{
				if (bits < 8)
				{
					return num | (long)((long)((ulong)this.read_byte(bits)) << i * 8);
				}
				num |= (long)((long)((ulong)this.read_byte(8)) << i * 8);
				bits -= 8;
			}
			return num;
		}

		// Token: 0x06002741 RID: 10049 RVA: 0x000EBE64 File Offset: 0x000EA064
		public ulong ReadULong(int bits)
		{
			if (bits > this.data_bit_length - this.read_pos)
			{
				throw new EndOfStreamException("Bitstream cannot read " + bits.ToString() + " bits, end of stream reached.");
			}
			if (bits < 1 || bits > 64)
			{
				throw new ArgumentException("Parameter bits = " + bits.ToString() + ", bits must be between 1 and " + 64.ToString());
			}
			ulong num = 0UL;
			for (int i = 0; i < 8; i++)
			{
				if (bits < 8)
				{
					return num | (ulong)this.read_byte(bits) << i * 8;
				}
				num |= (ulong)this.read_byte(8) << i * 8;
				bits -= 8;
			}
			return num;
		}

		// Token: 0x06002742 RID: 10050 RVA: 0x000EBF0C File Offset: 0x000EA10C
		public float ReadFloat(int bits)
		{
			if (bits > this.data_bit_length - this.read_pos)
			{
				throw new EndOfStreamException("Bitstream cannot read " + bits.ToString() + " bits, end of stream reached.");
			}
			if (bits < 1 || bits > 32)
			{
				throw new ArgumentException("Parameter bits = " + bits.ToString() + ", bits must be between 1 and " + 32.ToString());
			}
			byte[] array = new byte[4];
			for (int i = 0; i < 4; i++)
			{
				if (bits < 8)
				{
					array[i] = this.read_byte(bits);
					break;
				}
				array[i] = this.read_byte(8);
				bits -= 8;
			}
			return BitConverter.ToSingle(array, 0);
		}

		// Token: 0x06002743 RID: 10051 RVA: 0x000EBFB0 File Offset: 0x000EA1B0
		public double ReadDouble(int bits)
		{
			if (bits > this.data_bit_length - this.read_pos)
			{
				throw new EndOfStreamException("Bitstream cannot read " + bits.ToString() + " bits, end of stream reached.");
			}
			if (bits < 1 || bits > 64)
			{
				throw new ArgumentException("Parameter bits = " + bits.ToString() + ", bits must be between 1 and " + 64.ToString());
			}
			byte[] array = new byte[8];
			for (int i = 0; i < 8; i++)
			{
				if (bits < 8)
				{
					array[i] = this.read_byte(bits);
					break;
				}
				array[i] = this.read_byte(8);
				bits -= 8;
			}
			return BitConverter.ToDouble(array, 0);
		}

		// Token: 0x06002744 RID: 10052 RVA: 0x000EC054 File Offset: 0x000EA254
		public string ReadString()
		{
			string text = "";
			ushort num = this.ReadUShort();
			for (int i = 0; i < (int)num; i++)
			{
				text += this.ReadChar().ToString();
			}
			return text;
		}

		// Token: 0x06002745 RID: 10053 RVA: 0x000EC090 File Offset: 0x000EA290
		public string ReadUnicodeString()
		{
			string result;
			try
			{
				string text = "";
				ushort num = this.ReadUShort();
				byte[] array = new byte[(int)num];
				for (int i = 0; i < (int)num; i++)
				{
					array[i] = this.ReadByte();
				}
				if (num > 0)
				{
					text = Encoding.Unicode.GetString(array);
				}
				result = text;
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.Message);
				result = "";
			}
			return result;
		}

		// Token: 0x06002746 RID: 10054 RVA: 0x000EC100 File Offset: 0x000EA300
		private void Expand(int required_size)
		{
			while (this.buffer_bit_length < this.data_bit_length + required_size && this.buffer_length < 102400000)
			{
				this.buffer_length *= 2;
				this.buffer_bit_length *= 2;
			}
			if (this.buffer_length >= 102400000)
			{
				Debug.LogWarning("BitStream exceeded max length!");
			}
			Array.Resize<byte>(ref this.buffer, this.buffer_length);
		}

		// Token: 0x06002747 RID: 10055 RVA: 0x000EC170 File Offset: 0x000EA370
		private void write_byte(byte val, int bits)
		{
			int num = this.write_pos % 8;
			int num2 = 8 - num;
			int num3 = this.write_pos / 8;
			if (num == 0 && bits == 8)
			{
				this.buffer[num3] = val;
				this.write_pos += 8;
				this.data_bit_length += 8;
				return;
			}
			byte b = (byte)(255 << num);
			this.buffer[num3] = (byte)((int)(this.buffer[num3] & ~(int)b) | ((int)val << num & (int)b));
			if (num2 >= bits)
			{
				this.write_pos += bits;
				this.data_bit_length += bits;
				return;
			}
			bits -= num2;
			this.write_pos += num2;
			this.data_bit_length += num2;
			num = this.write_pos % 8;
			num3 = this.write_pos / 8;
			b = (byte)(255 << num);
			this.buffer[num3] = (byte)((int)(this.buffer[num3] & ~(int)b) | (val >> num2 & (int)b));
			this.write_pos += bits;
			this.data_bit_length += bits;
		}

		// Token: 0x06002748 RID: 10056 RVA: 0x000EC284 File Offset: 0x000EA484
		private byte read_byte(int bits)
		{
			int num = this.read_pos % 8;
			int num2 = 8 - num;
			byte b;
			if (num == 0 && bits == 8)
			{
				b = this.buffer[this.read_pos / 8];
				this.read_pos += 8;
				return b;
			}
			byte b2 = (byte)(255 << num);
			b = (byte)((this.buffer[this.read_pos / 8] & b2) >> num);
			if (bits > num2)
			{
				int num3 = num2;
				this.read_pos += num2;
				b |= (byte)((int)this.buffer[this.read_pos / 8] << num3 & 255 << num3);
				this.read_pos += bits - num3;
			}
			else
			{
				this.read_pos += bits;
			}
			return b & (byte)(~(byte)(255 << bits));
		}

		// Token: 0x04002A94 RID: 10900
		private const int SIZEOF_BOOL = 1;

		// Token: 0x04002A95 RID: 10901
		private const int SIZEOF_BYTE = 8;

		// Token: 0x04002A96 RID: 10902
		private const int SIZEOF_CHAR = 16;

		// Token: 0x04002A97 RID: 10903
		private const int SIZEOF_SHORT = 16;

		// Token: 0x04002A98 RID: 10904
		private const int SIZEOF_INT = 32;

		// Token: 0x04002A99 RID: 10905
		private const int SIZEOF_LONG = 64;

		// Token: 0x04002A9A RID: 10906
		private const int SIZEOF_FLOAT = 32;

		// Token: 0x04002A9B RID: 10907
		private const int SIZEOF_DOUBLE = 64;

		// Token: 0x04002A9C RID: 10908
		private int buffer_length;

		// Token: 0x04002A9D RID: 10909
		private int buffer_bit_length;

		// Token: 0x04002A9E RID: 10910
		private int data_bit_length;

		// Token: 0x04002A9F RID: 10911
		private int write_pos;

		// Token: 0x04002AA0 RID: 10912
		private int read_pos;

		// Token: 0x04002AA1 RID: 10913
		private byte[] buffer = new byte[4];

		// Token: 0x04002AA2 RID: 10914
		private const int MAX_LENGTH = 102400000;
	}
}
