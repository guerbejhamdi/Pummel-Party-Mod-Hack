using System;
using System.IO;
using System.IO.Compression;

// Token: 0x02000453 RID: 1107
public static class DeflateCompression
{
	// Token: 0x06001E61 RID: 7777 RVA: 0x000C411C File Offset: 0x000C231C
	public static byte[] Compress(byte[] data)
	{
		MemoryStream memoryStream = new MemoryStream();
		using (DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionMode.Compress))
		{
			deflateStream.Write(data, 0, data.Length);
		}
		return memoryStream.ToArray();
	}

	// Token: 0x06001E62 RID: 7778 RVA: 0x000C4164 File Offset: 0x000C2364
	public static byte[] Decompress(byte[] data)
	{
		Stream stream = new MemoryStream(data);
		MemoryStream memoryStream = new MemoryStream();
		using (DeflateStream deflateStream = new DeflateStream(stream, CompressionMode.Decompress))
		{
			deflateStream.CopyTo(memoryStream);
		}
		return memoryStream.ToArray();
	}

	// Token: 0x06001E63 RID: 7779 RVA: 0x000C41B0 File Offset: 0x000C23B0
	public static void CopyTo(this Stream source, Stream destination, int bufferSize = 81920)
	{
		byte[] array = new byte[bufferSize];
		int count;
		while ((count = source.Read(array, 0, array.Length)) != 0)
		{
			destination.Write(array, 0, count);
		}
	}
}
