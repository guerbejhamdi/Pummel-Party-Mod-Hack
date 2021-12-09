using System;
using System.IO;

namespace ZP.Utility
{
	// Token: 0x020005EB RID: 1515
	public class ByteStream
	{
		// Token: 0x060026C3 RID: 9923 RVA: 0x0001B9B8 File Offset: 0x00019BB8
		public ByteStream()
		{
			this.set_data(new byte[4]);
		}

		// Token: 0x060026C4 RID: 9924 RVA: 0x0001B9CC File Offset: 0x00019BCC
		public ByteStream(byte[] data)
		{
			this.set_data(data);
		}

		// Token: 0x060026C5 RID: 9925 RVA: 0x0001B9DB File Offset: 0x00019BDB
		public ByteStream(byte[] data, int start, int length)
		{
			this.set_data(data, start, length);
		}

		// Token: 0x060026C6 RID: 9926 RVA: 0x0001B9EC File Offset: 0x00019BEC
		public void set_data(byte[] data)
		{
			this.write_pos = 0;
			this.read_pos = 0;
			this.buffer = new byte[data.Length];
			Array.Copy(data, this.buffer, data.Length);
			this.data_length = data.Length;
			this.buffer_length = data.Length;
		}

		// Token: 0x060026C7 RID: 9927 RVA: 0x000EA12C File Offset: 0x000E832C
		public void set_data(byte[] data, int start, int length)
		{
			this.write_pos = 0;
			this.read_pos = 0;
			int num = length - start;
			this.buffer = new byte[num];
			Array.Copy(data, start, this.buffer, 0, num);
			this.data_length = data.Length;
			this.buffer_length = data.Length;
		}

		// Token: 0x060026C8 RID: 9928 RVA: 0x0001BA2B File Offset: 0x00019C2B
		public byte[] get_buffer()
		{
			return this.buffer;
		}

		// Token: 0x060026C9 RID: 9929 RVA: 0x000EA178 File Offset: 0x000E8378
		public byte[] get_data_copy()
		{
			byte[] array = new byte[this.data_length];
			Array.Copy(this.buffer, array, this.data_length);
			return array;
		}

		// Token: 0x060026CA RID: 9930 RVA: 0x0001BA33 File Offset: 0x00019C33
		public void reset()
		{
			this.read_pos = 0;
			this.write_pos = 0;
			this.data_length = 0;
		}

		// Token: 0x060026CB RID: 9931 RVA: 0x0001BA4A File Offset: 0x00019C4A
		public void set_read_position(int pos)
		{
			this.read_pos = pos;
		}

		// Token: 0x060026CC RID: 9932 RVA: 0x0001BA53 File Offset: 0x00019C53
		public void set_write_position(int pos)
		{
			this.write_pos = pos;
		}

		// Token: 0x060026CD RID: 9933 RVA: 0x0001BA5C File Offset: 0x00019C5C
		public int get_size()
		{
			return this.data_length;
		}

		// Token: 0x060026CE RID: 9934 RVA: 0x0001BA64 File Offset: 0x00019C64
		public byte read_byte()
		{
			if (this.read_pos + 1 >= this.data_length)
			{
				throw new EndOfStreamException();
			}
			byte result = this.buffer[this.read_pos];
			this.read_pos++;
			return result;
		}

		// Token: 0x060026CF RID: 9935 RVA: 0x0001BA97 File Offset: 0x00019C97
		public bool read_bool()
		{
			if (this.read_pos + 1 >= this.data_length)
			{
				throw new EndOfStreamException();
			}
			bool result = this.buffer[this.read_pos] > 0;
			this.read_pos++;
			return result;
		}

		// Token: 0x060026D0 RID: 9936 RVA: 0x000EA1A4 File Offset: 0x000E83A4
		public char read_char()
		{
			if (this.read_pos + 2 >= this.data_length)
			{
				throw new EndOfStreamException();
			}
			char result = (char)((int)this.buffer[this.read_pos] | (int)this.buffer[this.read_pos + 1] << 8);
			this.read_pos += 2;
			return result;
		}

		// Token: 0x060026D1 RID: 9937 RVA: 0x000EA1F8 File Offset: 0x000E83F8
		public short read_short()
		{
			if (this.read_pos + 2 >= this.data_length)
			{
				throw new EndOfStreamException();
			}
			short result = (short)((int)this.buffer[this.read_pos] | (int)this.buffer[this.read_pos + 1] << 8);
			this.read_pos += 2;
			return result;
		}

		// Token: 0x060026D2 RID: 9938 RVA: 0x000EA1A4 File Offset: 0x000E83A4
		public ushort read_ushort()
		{
			if (this.read_pos + 2 >= this.data_length)
			{
				throw new EndOfStreamException();
			}
			ushort result = (ushort)((int)this.buffer[this.read_pos] | (int)this.buffer[this.read_pos + 1] << 8);
			this.read_pos += 2;
			return result;
		}

		// Token: 0x060026D3 RID: 9939 RVA: 0x000EA24C File Offset: 0x000E844C
		public int read_int()
		{
			if (this.read_pos + 4 >= this.data_length)
			{
				throw new EndOfStreamException();
			}
			int result = (int)this.buffer[this.read_pos] | (int)this.buffer[this.read_pos + 1] << 8 | (int)this.buffer[this.read_pos + 2] << 16 | (int)this.buffer[this.read_pos + 3] << 24;
			this.read_pos += 4;
			return result;
		}

		// Token: 0x060026D4 RID: 9940 RVA: 0x000EA24C File Offset: 0x000E844C
		public uint read_uint()
		{
			if (this.read_pos + 4 >= this.data_length)
			{
				throw new EndOfStreamException();
			}
			uint result = (uint)((int)this.buffer[this.read_pos] | (int)this.buffer[this.read_pos + 1] << 8 | (int)this.buffer[this.read_pos + 2] << 16 | (int)this.buffer[this.read_pos + 3] << 24);
			this.read_pos += 4;
			return result;
		}

		// Token: 0x060026D5 RID: 9941 RVA: 0x000EA2C4 File Offset: 0x000E84C4
		public long read_long()
		{
			if (this.read_pos + 8 >= this.data_length)
			{
				throw new EndOfStreamException();
			}
			long result = (long)((ulong)((int)this.buffer[this.read_pos] | (int)this.buffer[this.read_pos + 1] << 8 | (int)this.buffer[this.read_pos + 2] << 16 | (int)this.buffer[this.read_pos + 3] << 24 | (int)this.buffer[this.read_pos + 4] | (int)this.buffer[this.read_pos + 5] << 8 | (int)this.buffer[this.read_pos + 6] << 16));
			this.read_pos += 8;
			return result;
		}

		// Token: 0x060026D6 RID: 9942 RVA: 0x000EA2C4 File Offset: 0x000E84C4
		public ulong read_ulong()
		{
			if (this.read_pos + 8 >= this.data_length)
			{
				throw new EndOfStreamException();
			}
			ulong result = (ulong)((int)this.buffer[this.read_pos] | (int)this.buffer[this.read_pos + 1] << 8 | (int)this.buffer[this.read_pos + 2] << 16 | (int)this.buffer[this.read_pos + 3] << 24 | (int)this.buffer[this.read_pos + 4] | (int)this.buffer[this.read_pos + 5] << 8 | (int)this.buffer[this.read_pos + 6] << 16);
			this.read_pos += 8;
			return result;
		}

		// Token: 0x060026D7 RID: 9943 RVA: 0x000EA370 File Offset: 0x000E8570
		public float read_float()
		{
			if (this.read_pos + 4 >= this.data_length)
			{
				throw new EndOfStreamException();
			}
			float result = (int)this.buffer[this.read_pos] | (int)this.buffer[this.read_pos + 1] << 8 | (int)this.buffer[this.read_pos + 2] << 16 | (int)this.buffer[this.read_pos + 3] << 24;
			this.read_pos += 4;
			return result;
		}

		// Token: 0x060026D8 RID: 9944 RVA: 0x000EA3E8 File Offset: 0x000E85E8
		public double read_double()
		{
			if (this.read_pos + 8 >= this.data_length)
			{
				throw new EndOfStreamException();
			}
			double result = (int)this.buffer[this.read_pos] | (int)this.buffer[this.read_pos + 1] << 8 | (int)this.buffer[this.read_pos + 2] << 16 | (int)this.buffer[this.read_pos + 3] << 24 | (int)this.buffer[this.read_pos + 4] | (int)this.buffer[this.read_pos + 5] << 8 | (int)this.buffer[this.read_pos + 6] << 16;
			this.read_pos += 8;
			return result;
		}

		// Token: 0x060026D9 RID: 9945 RVA: 0x000EA498 File Offset: 0x000E8698
		public string read_string()
		{
			ushort num = this.read_ushort();
			if (this.read_pos + (int)(num * 2) >= this.data_length)
			{
				throw new EndOfStreamException();
			}
			char[] array = new char[(int)num];
			Buffer.BlockCopy(this.buffer, this.read_pos, array, 0, (int)(num * 2));
			return new string(array);
		}

		// Token: 0x060026DA RID: 9946 RVA: 0x000EA4E8 File Offset: 0x000E86E8
		public void write(byte val)
		{
			if (this.buffer_length < this.data_length + 1)
			{
				this.expand(this.data_length + 1);
			}
			this.buffer[this.write_pos] = val;
			this.write_pos++;
			this.data_length++;
		}

		// Token: 0x060026DB RID: 9947 RVA: 0x000EA540 File Offset: 0x000E8740
		public void write(bool val)
		{
			if (this.buffer_length < this.data_length + 1)
			{
				this.expand(this.data_length + 1);
			}
			this.buffer[this.write_pos] = (val ? 1 : 0);
			this.write_pos++;
			this.data_length++;
		}

		// Token: 0x060026DC RID: 9948 RVA: 0x000EA59C File Offset: 0x000E879C
		public void write(char val)
		{
			if (this.buffer_length < this.data_length + 2)
			{
				this.expand(this.data_length + 2);
			}
			byte[] bytes = BitConverter.GetBytes(val);
			this.buffer[this.write_pos] = bytes[0];
			this.buffer[this.write_pos + 1] = bytes[1];
			this.write_pos += 2;
			this.data_length += 2;
		}

		// Token: 0x060026DD RID: 9949 RVA: 0x000EA60C File Offset: 0x000E880C
		public void write(short val)
		{
			if (this.buffer_length < this.data_length + 2)
			{
				this.expand(this.data_length + 2);
			}
			byte[] bytes = BitConverter.GetBytes(val);
			this.buffer[this.write_pos] = bytes[0];
			this.buffer[this.write_pos + 1] = bytes[1];
			this.write_pos += 2;
			this.data_length += 2;
		}

		// Token: 0x060026DE RID: 9950 RVA: 0x000EA67C File Offset: 0x000E887C
		public void write(ushort val)
		{
			if (this.buffer_length < this.data_length + 2)
			{
				this.expand(this.data_length + 2);
			}
			byte[] bytes = BitConverter.GetBytes(val);
			this.buffer[this.write_pos] = bytes[0];
			this.buffer[this.write_pos + 1] = bytes[1];
			this.write_pos += 2;
			this.data_length += 2;
		}

		// Token: 0x060026DF RID: 9951 RVA: 0x000EA6EC File Offset: 0x000E88EC
		public void write(int val)
		{
			if (this.buffer_length < this.data_length + 4)
			{
				this.expand(this.data_length + 4);
			}
			byte[] bytes = BitConverter.GetBytes(val);
			this.buffer[this.write_pos] = bytes[0];
			this.buffer[this.write_pos + 1] = bytes[1];
			this.buffer[this.write_pos + 2] = bytes[2];
			this.buffer[this.write_pos + 3] = bytes[3];
			this.write_pos += 4;
			this.data_length += 4;
		}

		// Token: 0x060026E0 RID: 9952 RVA: 0x000EA780 File Offset: 0x000E8980
		public void write(uint val)
		{
			if (this.buffer_length < this.data_length + 4)
			{
				this.expand(this.data_length + 4);
			}
			byte[] bytes = BitConverter.GetBytes(val);
			this.buffer[this.write_pos] = bytes[0];
			this.buffer[this.write_pos + 1] = bytes[1];
			this.buffer[this.write_pos + 2] = bytes[2];
			this.buffer[this.write_pos + 3] = bytes[3];
			this.write_pos += 4;
			this.data_length += 4;
		}

		// Token: 0x060026E1 RID: 9953 RVA: 0x000EA814 File Offset: 0x000E8A14
		public void write(long val)
		{
			if (this.buffer_length < this.data_length + 8)
			{
				this.expand(this.data_length + 8);
			}
			byte[] bytes = BitConverter.GetBytes(val);
			this.buffer[this.write_pos] = bytes[0];
			this.buffer[this.write_pos + 1] = bytes[1];
			this.buffer[this.write_pos + 2] = bytes[2];
			this.buffer[this.write_pos + 3] = bytes[3];
			this.buffer[this.write_pos + 4] = bytes[4];
			this.buffer[this.write_pos + 5] = bytes[5];
			this.buffer[this.write_pos + 6] = bytes[6];
			this.buffer[this.write_pos + 7] = bytes[7];
			this.write_pos += 8;
			this.data_length += 8;
		}

		// Token: 0x060026E2 RID: 9954 RVA: 0x000EA8F0 File Offset: 0x000E8AF0
		public void write(ulong val)
		{
			if (this.buffer_length < this.data_length + 8)
			{
				this.expand(this.data_length + 8);
			}
			byte[] bytes = BitConverter.GetBytes(val);
			this.buffer[this.write_pos] = bytes[0];
			this.buffer[this.write_pos + 1] = bytes[1];
			this.buffer[this.write_pos + 2] = bytes[2];
			this.buffer[this.write_pos + 3] = bytes[3];
			this.buffer[this.write_pos + 4] = bytes[4];
			this.buffer[this.write_pos + 5] = bytes[5];
			this.buffer[this.write_pos + 6] = bytes[6];
			this.buffer[this.write_pos + 7] = bytes[7];
			this.write_pos += 8;
			this.data_length += 8;
		}

		// Token: 0x060026E3 RID: 9955 RVA: 0x000EA9CC File Offset: 0x000E8BCC
		public void write(float val)
		{
			if (this.buffer_length < this.data_length + 4)
			{
				this.expand(this.data_length + 4);
			}
			byte[] bytes = BitConverter.GetBytes(val);
			this.buffer[this.write_pos] = bytes[0];
			this.buffer[this.write_pos + 1] = bytes[1];
			this.buffer[this.write_pos + 2] = bytes[2];
			this.buffer[this.write_pos + 3] = bytes[3];
			this.write_pos += 4;
			this.data_length += 4;
		}

		// Token: 0x060026E4 RID: 9956 RVA: 0x000EAA60 File Offset: 0x000E8C60
		public void write(double val)
		{
			if (this.buffer_length < this.data_length + 8)
			{
				this.expand(this.data_length + 8);
			}
			byte[] bytes = BitConverter.GetBytes(val);
			this.buffer[this.write_pos] = bytes[0];
			this.buffer[this.write_pos + 1] = bytes[1];
			this.buffer[this.write_pos + 2] = bytes[2];
			this.buffer[this.write_pos + 3] = bytes[3];
			this.buffer[this.write_pos + 4] = bytes[4];
			this.buffer[this.write_pos + 5] = bytes[5];
			this.buffer[this.write_pos + 6] = bytes[6];
			this.buffer[this.write_pos + 7] = bytes[7];
			this.write_pos += 8;
			this.data_length += 8;
		}

		// Token: 0x060026E5 RID: 9957 RVA: 0x000EAB3C File Offset: 0x000E8D3C
		public void write(string val)
		{
			ushort val2 = (ushort)val.Length;
			this.write(val2);
			int num = val.Length * 2;
			if (this.buffer_length < this.data_length + num)
			{
				this.expand(this.data_length + num);
			}
			Buffer.BlockCopy(val.ToCharArray(), 0, this.buffer, this.write_pos, num);
			this.write_pos += num;
			this.data_length += num;
		}

		// Token: 0x060026E6 RID: 9958 RVA: 0x0001BAD1 File Offset: 0x00019CD1
		private void expand(int required_size)
		{
			while (this.buffer_length < required_size && this.buffer_length < 10240000)
			{
				this.buffer_length *= 2;
			}
			Array.Resize<byte>(ref this.buffer, this.buffer_length);
		}

		// Token: 0x04002A71 RID: 10865
		private int write_pos;

		// Token: 0x04002A72 RID: 10866
		private int read_pos;

		// Token: 0x04002A73 RID: 10867
		private int data_length;

		// Token: 0x04002A74 RID: 10868
		private int buffer_length;

		// Token: 0x04002A75 RID: 10869
		private byte[] buffer;

		// Token: 0x04002A76 RID: 10870
		private const int BASE_LENGTH = 4;

		// Token: 0x04002A77 RID: 10871
		private const int MAX_LENGTH = 10240000;

		// Token: 0x04002A78 RID: 10872
		private const int BYTE_LENGTH = 1;

		// Token: 0x04002A79 RID: 10873
		private const int BOOL_LENGTH = 1;

		// Token: 0x04002A7A RID: 10874
		private const int CHAR_LENGTH = 2;

		// Token: 0x04002A7B RID: 10875
		private const int SHORT_LENGTH = 2;

		// Token: 0x04002A7C RID: 10876
		private const int INT_LENGTH = 4;

		// Token: 0x04002A7D RID: 10877
		private const int LONG_LENGTH = 8;

		// Token: 0x04002A7E RID: 10878
		private const int FLOAT_LENGTH = 4;

		// Token: 0x04002A7F RID: 10879
		private const int DOUBLE_LENGTH = 8;
	}
}
